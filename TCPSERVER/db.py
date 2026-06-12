#!/usr/bin/env python3
"""数据库模块 - 无人机白名单、用户账号、配对关系管理

使用 SQLite + PBKDF2-SHA256 密码哈希，无额外依赖。
"""

import sqlite3
import os
import hashlib
import hmac as _hmac
import secrets
import threading
import time
import logging
from typing import Optional

logger = logging.getLogger("db")

DB_PATH = os.environ.get("DB_PATH", "/data/db/server.db")

_lock = threading.Lock()
_conn: Optional[sqlite3.Connection] = None


def _get_conn() -> sqlite3.Connection:
    global _conn
    if _conn is None:
        db_dir = os.path.dirname(DB_PATH)
        if db_dir:
            os.makedirs(db_dir, exist_ok=True)
        _conn = sqlite3.connect(DB_PATH, check_same_thread=False)
        _conn.row_factory = sqlite3.Row
        _conn.execute("PRAGMA journal_mode=WAL")
        _conn.execute("PRAGMA foreign_keys=ON")
        _conn.execute("PRAGMA synchronous=NORMAL")
    return _conn


def init_db(admin_user: Optional[str] = None, admin_password: Optional[str] = None) -> None:
    """初始化数据库，创建表结构，可选初始化默认管理员账号"""
    with _lock:
        conn = _get_conn()
        conn.executescript("""
            CREATE TABLE IF NOT EXISTS drones (
                imei        TEXT PRIMARY KEY,
                name        TEXT NOT NULL DEFAULT '',
                created_at  REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS users (
                username    TEXT PRIMARY KEY,
                pwd_hash    TEXT NOT NULL,
                pwd_salt    TEXT NOT NULL,
                is_admin    INTEGER NOT NULL DEFAULT 0,
                created_at  REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS pairings (
                username    TEXT NOT NULL,
                imei        TEXT NOT NULL,
                paired_at   REAL NOT NULL,
                PRIMARY KEY (username, imei),
                FOREIGN KEY (username) REFERENCES users(username) ON DELETE CASCADE,
                FOREIGN KEY (imei) REFERENCES drones(imei) ON DELETE CASCADE
            );
        """)
        conn.commit()
        logger.info(f"数据库初始化完成: {DB_PATH}")

    if admin_user and admin_password:
        with _lock:
            conn = _get_conn()
            row = conn.execute(
                "SELECT 1 FROM users WHERE username=?", (admin_user,)
            ).fetchone()
            if not row:
                _create_user_locked(conn, admin_user, admin_password, is_admin=True)
                logger.info(f"已创建默认管理员账号: {admin_user}")


# ---------- 密码工具 ----------

def _hash_password(password: str, salt: str) -> str:
    """PBKDF2-SHA256，260000 轮，符合 NIST 2024 建议"""
    dk = hashlib.pbkdf2_hmac(
        "sha256", password.encode("utf-8"), salt.encode("utf-8"), 260000
    )
    return dk.hex()


def _create_user_locked(
    conn: sqlite3.Connection, username: str, password: str, is_admin: bool = False
) -> None:
    """直接创建用户（调用方需持有 _lock）"""
    salt = secrets.token_hex(16)
    pwd_hash = _hash_password(password, salt)
    conn.execute(
        "INSERT INTO users (username, pwd_hash, pwd_salt, is_admin, created_at) VALUES (?,?,?,?,?)",
        (username, pwd_hash, salt, 1 if is_admin else 0, time.time()),
    )
    conn.commit()


# ---------- 无人机管理 ----------

def drone_add(imei: str, name: str = "") -> bool:
    """添加无人机到白名单，返回 True=成功，False=已存在"""
    with _lock:
        conn = _get_conn()
        try:
            conn.execute(
                "INSERT INTO drones (imei, name, created_at) VALUES (?,?,?)",
                (imei, name, time.time()),
            )
            conn.commit()
            return True
        except sqlite3.IntegrityError:
            return False


def drone_remove(imei: str) -> bool:
    """从白名单删除无人机（同时级联删除相关配对）"""
    with _lock:
        conn = _get_conn()
        cur = conn.execute("DELETE FROM drones WHERE imei=?", (imei,))
        conn.commit()
        return cur.rowcount > 0


def drone_update_name(imei: str, name: str) -> bool:
    with _lock:
        conn = _get_conn()
        cur = conn.execute("UPDATE drones SET name=? WHERE imei=?", (name, imei))
        conn.commit()
        return cur.rowcount > 0


def drone_list() -> list:
    with _lock:
        conn = _get_conn()
        rows = conn.execute(
            "SELECT imei, name, created_at FROM drones ORDER BY created_at"
        ).fetchall()
        return [dict(r) for r in rows]


def drone_exists(imei: str) -> bool:
    with _lock:
        conn = _get_conn()
        return (
            conn.execute(
                "SELECT 1 FROM drones WHERE imei=?", (imei,)
            ).fetchone()
            is not None
        )


# ---------- 用户管理 ----------

def user_add(username: str, password: str, is_admin: bool = False) -> bool:
    """新建用户，返回 True=成功，False=用户名已存在"""
    with _lock:
        conn = _get_conn()
        try:
            _create_user_locked(conn, username, password, is_admin)
            return True
        except sqlite3.IntegrityError:
            return False


def user_remove(username: str) -> bool:
    with _lock:
        conn = _get_conn()
        cur = conn.execute("DELETE FROM users WHERE username=?", (username,))
        conn.commit()
        return cur.rowcount > 0


def user_list() -> list:
    with _lock:
        conn = _get_conn()
        rows = conn.execute(
            "SELECT username, is_admin, created_at FROM users ORDER BY created_at"
        ).fetchall()
        return [dict(r) for r in rows]


def user_get(username: str) -> Optional[dict]:
    with _lock:
        conn = _get_conn()
        row = conn.execute(
            "SELECT username, is_admin FROM users WHERE username=?", (username,)
        ).fetchone()
        return dict(row) if row else None


def user_verify(username: str, password: str) -> Optional[dict]:
    """验证用户名密码，成功返回用户信息字典，失败返回 None
    使用固定时间比较，防止计时侧信道攻击"""
    with _lock:
        conn = _get_conn()
        row = conn.execute(
            "SELECT username, pwd_hash, pwd_salt, is_admin FROM users WHERE username=?",
            (username,),
        ).fetchone()

    if not row:
        # 固定时间消耗，防止用户名枚举
        hashlib.pbkdf2_hmac("sha256", password.encode("utf-8"), b"dummy_salt_x", 260000)
        return None

    expected = _hash_password(password, row["pwd_salt"])
    if _hmac.compare_digest(expected, row["pwd_hash"]):
        return {"username": row["username"], "is_admin": bool(row["is_admin"])}
    return None


def user_change_password(username: str, new_password: str) -> bool:
    with _lock:
        conn = _get_conn()
        salt = secrets.token_hex(16)
        pwd_hash = _hash_password(new_password, salt)
        cur = conn.execute(
            "UPDATE users SET pwd_hash=?, pwd_salt=? WHERE username=?",
            (pwd_hash, salt, username),
        )
        conn.commit()
        return cur.rowcount > 0


# ---------- 配对管理 ----------

def pairing_add(username: str, imei: str) -> bool:
    """为用户添加一架无人机配对，返回 True=成功，False=已存在"""
    with _lock:
        conn = _get_conn()
        try:
            conn.execute(
                "INSERT INTO pairings (username, imei, paired_at) VALUES (?,?,?)",
                (username, imei, time.time()),
            )
            conn.commit()
            return True
        except sqlite3.IntegrityError:
            return False


def pairing_remove(username: str, imei: str) -> bool:
    with _lock:
        conn = _get_conn()
        cur = conn.execute(
            "DELETE FROM pairings WHERE username=? AND imei=?", (username, imei)
        )
        conn.commit()
        return cur.rowcount > 0


def pairing_list_all() -> list:
    with _lock:
        conn = _get_conn()
        rows = conn.execute(
            "SELECT username, imei, paired_at FROM pairings ORDER BY paired_at"
        ).fetchall()
        return [dict(r) for r in rows]


def pairing_get_drones(username: str) -> list:
    """获取某用户配对的全部 IMEI 列表"""
    with _lock:
        conn = _get_conn()
        rows = conn.execute(
            "SELECT imei FROM pairings WHERE username=?", (username,)
        ).fetchall()
        return [r["imei"] for r in rows]


def pairing_get_users_for_imei(imei: str) -> list:
    """获取配对了某架无人机的全部用户名列表"""
    with _lock:
        conn = _get_conn()
        rows = conn.execute(
            "SELECT username FROM pairings WHERE imei=?", (imei,)
        ).fetchall()
        return [r["username"] for r in rows]
