#!/usr/bin/env python3
"""UAV TCP 中继服务器
- 端口 5760：无人机接入（ML307C IMEI 注册包认证 + MAVLink 数据流）
- 端口 5761：地面站接入（TLS + 用户名密码认证，多机合并转发）
- 端口 8080：Web 监控 + 管理 API
"""

import socket
import ssl
import threading
import os
import re
import time
import signal
import sys
import secrets
import logging
import json
import math
import mimetypes
import shutil
import http.client
import urllib.parse
from http.server import HTTPServer, BaseHTTPRequestHandler
from socketserver import ThreadingMixIn
from typing import Optional
from pymavlink.dialects.v20 import ardupilotmega as mavlink2

import db

# =============================================================================
# 配置
# =============================================================================
HOST       = "0.0.0.0"
DRONE_PORT = 5760
GCS_PORT   = 5761
HTTP_PORT  = int(os.environ.get("HTTP_PORT", "8080"))

DATA_DIR = os.environ.get("DATA_DIR", "/data/received")
WEB_DIR  = os.path.join(os.path.dirname(os.path.abspath(__file__)), "web")
SAVE_DIR = DATA_DIR  # 日志文件目录与数据接收目录相同
PROXY_EXE = os.path.join(
    os.path.dirname(os.environ.get("DB_PATH", "/data/db/server.db")),
    "gcs_proxy.exe"
)

TLS_CERT = os.environ.get("TLS_CERT", "/certs/server.crt")
TLS_KEY  = os.environ.get("TLS_KEY",  "/certs/server.key")
NO_TLS   = os.environ.get("NO_TLS", "0").lower() in ("1", "true", "yes")

ADMIN_USER = os.environ.get("ADMIN_USER", "admin")
ADMIN_PASS = os.environ.get("ADMIN_PASS", "changeme123")

# 航线规划服务地址（Docker 内部网络）
PLANNER_URL = os.environ.get("PLANNER_URL", "http://localhost:8010")

DRONE_REGISTER_TIMEOUT = 5     # 秒：等待注册包超时
DRONE_OFFLINE_TIMEOUT  = 90    # 秒：心跳超时（60s 心跳 + 30s 容差）
API_SESSION_TTL        = 28800  # 秒：API Token 有效期（8 小时）

MAX_LOGIN_FAILURES = 5    # 最多连续失败次数
LOGIN_BAN_DURATION = 900  # IP 封禁时长（秒）
LOGIN_FAIL_WINDOW  = 300  # 失败计次统计窗口（秒）

BUFFER_SIZE     = 4096
MAX_CONNECTIONS = 100

IMEI_PATTERN = re.compile(rb"^\d{15}$")

# =============================================================================
# 日志
# =============================================================================
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s [%(levelname)s] %(message)s",
    handlers=[logging.StreamHandler(sys.stdout)],
)
logger = logging.getLogger("server")

# =============================================================================
# 全局状态
# =============================================================================
shutdown_event = threading.Event()

# 无人机路由表：{ imei: { socket, addr, sysid, last_seen } }
drone_routes: dict = {}
drone_lock   = threading.Lock()

# SYSID→IMEI 映射（从 HEARTBEAT 动态学习）
sysid_to_imei: dict = {}

# GCS 会话：{ username: { socket, addr } }
gcs_sessions: dict = {}
gcs_lock     = threading.Lock()

# 配对缓存：{ "i2u": {imei: [username,...]}, "u2i": {username: [imei,...]} }
_pairing_cache: dict      = {"i2u": {}, "u2i": {}}
_pairing_cache_lock = threading.Lock()

# API Token：{ token: { username, is_admin, expires } }
_api_tokens: dict   = {}
_token_lock  = threading.Lock()

# IP 安全：登录失败计次 & 封禁
_ip_failures: dict  = {}   # ip -> [timestamp, ...]
_ip_bans: dict      = {}   # ip -> ban_expires_timestamp
_security_lock = threading.Lock()

# 飞机状态（供 Web 地图使用）
aircraft_states: dict = {}
aircraft_lock   = threading.Lock()

# 原始16进制缓存（IMEI -> hex string，供调试面板使用）
raw_hex_per_imei: dict = {}
raw_hex_lock     = threading.Lock()

# TLS 上下文（启动时初始化）
ssl_context: Optional[ssl.SSLContext] = None

# =============================================================================
# 飞控常量
# =============================================================================
COPTER_MODES = {
    0: "STABILIZE", 1: "ACRO",    2: "ALT_HOLD",    3: "AUTO",
    4: "GUIDED",    5: "LOITER",  6: "RTL",          7: "CIRCLE",
    9: "LAND",     11: "DRIFT",  13: "SPORT",        14: "FLIP",
    15: "AUTOTUNE",16: "POSHOLD",17: "BRAKE",        18: "THROW",
    19: "AVOID_ADSB",20: "GUIDED_NOGPS",21: "SMART_RTL",
    22: "FLOWHOLD",23: "FOLLOW", 24: "ZIGZAG",       25: "SYSTEMID",
    26: "AUTOROTATE",27: "AUTO_RTL",
}
PLANE_MODES = {
    0: "MANUAL",    1: "CIRCLE",        2: "STABILIZE",     3: "TRAINING",
    4: "ACRO",      5: "FLY_BY_WIRE_A", 6: "FLY_BY_WIRE_B", 7: "CRUISE",
    8: "AUTOTUNE", 10: "AUTO",         11: "RTL",           12: "LOITER",
    14: "AVOID_ADSB",15: "GUIDED",     17: "QSTABILIZE",   18: "QHOVER",
    19: "QLOITER", 20: "QLAND",        21: "QRTL",          22: "QAUTOTUNE",
    23: "QACRO",   24: "THERMAL",
}
SYSTEM_STATUS = {
    0: "UNINIT", 1: "BOOT", 2: "CALIBRATING", 3: "STANDBY",
    4: "ACTIVE", 5: "CRITICAL", 6: "EMERGENCY", 7: "POWEROFF",
    8: "FLIGHT_TERMINATION",
}
MAV_TYPE_NAMES = {
    0: "通用",    1: "固定翼",   2: "四旋翼",   3: "同轴双桨",
    4: "直升机",  5: "天线追踪", 6: "地面站",  13: "六旋翼",
    14: "八旋翼", 15: "三旋翼", 20: "垂起双旋翼",21: "垂起四旋翼",
    22: "垂起倾转旋翼", 29: "十二旋翼",
}


def get_flight_mode(mav_type: int, custom_mode: int) -> str:
    if mav_type in (1, 20, 21, 22):
        return PLANE_MODES.get(custom_mode, f"MODE_{custom_mode}")
    return COPTER_MODES.get(custom_mode, f"MODE_{custom_mode}")


# =============================================================================
# 启动初始化
# =============================================================================
def init_tls() -> None:
    global ssl_context
    if NO_TLS:
        logger.info("NO_TLS=1，GCS 端口将使用明文传输")
        return
    if not os.path.exists(TLS_CERT) or not os.path.exists(TLS_KEY):
        logger.warning(
            f"TLS 证书未找到 ({TLS_CERT})，GCS 端口将使用明文传输！"
            " 仅限开发/内网环境，生产环境请挂载证书。"
        )
        return
    try:
        ctx = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
        ctx.load_cert_chain(TLS_CERT, TLS_KEY)
        ctx.minimum_version = ssl.TLSVersion.TLSv1_2
        ctx.verify_mode = ssl.CERT_NONE
        ssl_context = ctx
        logger.info("TLS 已启用（GCS 端口加密）")
    except Exception as e:
        logger.error(f"TLS 初始化失败: {e}，GCS 端口将使用明文传输！")


def ensure_data_dir() -> None:
    os.makedirs(DATA_DIR, exist_ok=True)
    logger.info(f"数据存储目录: {DATA_DIR}")


def refresh_pairing_cache() -> None:
    """从数据库重建内存配对缓存，每次配对变更后调用"""
    all_pairings = db.pairing_list_all()
    i2u: dict = {}
    u2i: dict = {}
    for p in all_pairings:
        i2u.setdefault(p["imei"], []).append(p["username"])
        u2i.setdefault(p["username"], []).append(p["imei"])
    with _pairing_cache_lock:
        _pairing_cache["i2u"] = i2u
        _pairing_cache["u2i"] = u2i
    logger.debug("配对缓存已刷新")


# =============================================================================
# IP 安全（登录失败限速 & 封禁）
# =============================================================================
def _is_ip_banned(ip: str) -> bool:
    with _security_lock:
        ban_until = _ip_bans.get(ip, 0)
        if ban_until > time.time():
            return True
        if ban_until:
            del _ip_bans[ip]
        return False


def _record_login_failure(ip: str) -> bool:
    """记录一次登录失败，超过阈值则封禁该 IP，返回是否已封禁"""
    now = time.time()
    with _security_lock:
        failures = [t for t in _ip_failures.get(ip, []) if now - t < LOGIN_FAIL_WINDOW]
        failures.append(now)
        _ip_failures[ip] = failures
        if len(failures) >= MAX_LOGIN_FAILURES:
            _ip_bans[ip] = now + LOGIN_BAN_DURATION
            _ip_failures[ip] = []
            logger.warning(f"IP 已封禁（登录失败过多）: {ip}")
            return True
        return False


def _clear_login_failures(ip: str) -> None:
    with _security_lock:
        _ip_failures.pop(ip, None)


# =============================================================================
# API Token 管理
# =============================================================================
def _issue_api_token(username: str, is_admin: bool) -> str:
    token = secrets.token_hex(32)
    now = time.time()
    with _token_lock:
        expired = [t for t, v in _api_tokens.items() if v["expires"] < now]
        for t in expired:
            _api_tokens.pop(t, None)
        _api_tokens[token] = {
            "username": username,
            "is_admin": is_admin,
            "expires":  now + API_SESSION_TTL,
        }
    return token


def _verify_api_token(token: str) -> Optional[dict]:
    with _token_lock:
        entry = _api_tokens.get(token)
        if not entry:
            return None
        if entry["expires"] < time.time():
            _api_tokens.pop(token, None)
            return None
        return entry


# =============================================================================
# MAVLink 数据处理（更新 Web 地图状态）
# =============================================================================
def _process_mavlink_message(msg, imei: str, client_ip: str) -> None:
    sys_id = msg.get_srcSystem()
    if sys_id == 0:
        return

    with drone_lock:
        sysid_to_imei[sys_id] = imei

    with aircraft_lock:
        if sys_id not in aircraft_states:
            logger.info(f"新飞机上线: SYSID={sys_id}, IMEI={imei}, IP={client_ip}")
            aircraft_states[sys_id] = {
                "system_id": sys_id, "imei": imei,
                "lat": 0.0, "lon": 0.0, "alt": 0.0, "relative_alt": 0.0,
                "heading": 0, "groundspeed": 0.0, "airspeed": 0.0,
                "climb": 0.0, "throttle": 0,
                "roll": 0.0, "pitch": 0.0, "yaw": 0.0,
                "battery_voltage": 0.0, "battery_current": 0.0,
                "battery_remaining": -1,
                "gps_fix": 0, "satellites": 0,
                "flight_mode": "UNKNOWN", "armed": False,
                "system_status": "UNKNOWN", "mav_type": "UNKNOWN",
                "last_update": time.time(), "client_ip": client_ip,
                "img_idx": -1,
                "trail": [],
            }

        state = aircraft_states[sys_id]
        state["last_update"] = time.time()
        state["client_ip"]   = client_ip
        msg_type = msg.get_type()

        if msg_type == "HEARTBEAT":
            state["armed"]         = bool(msg.base_mode & 128)
            state["flight_mode"]   = get_flight_mode(msg.type, msg.custom_mode)
            state["system_status"] = SYSTEM_STATUS.get(msg.system_status, "UNKNOWN")
            state["mav_type"]      = MAV_TYPE_NAMES.get(msg.type, f"TYPE_{msg.type}")

        elif msg_type == "GLOBAL_POSITION_INT":
            state["lat"]          = msg.lat / 1e7
            state["lon"]          = msg.lon / 1e7
            state["alt"]          = round(msg.alt / 1000.0, 1)
            state["relative_alt"] = round(msg.relative_alt / 1000.0, 1)
            state["heading"]      = msg.hdg // 100 if msg.hdg != 65535 else state["heading"]
            if state["lat"] != 0 and state["lon"] != 0:
                trail = state["trail"]
                trail.append([state["lon"], state["lat"]])
                if len(trail) > 500:
                    state["trail"] = trail[-500:]

        elif msg_type == "GPS_RAW_INT":
            state["gps_fix"]    = msg.fix_type
            state["satellites"] = msg.satellites_visible

        elif msg_type == "SYS_STATUS":
            state["battery_voltage"]   = round(msg.voltage_battery / 1000.0, 2)
            state["battery_current"]   = round(msg.current_battery / 100.0, 1)
            state["battery_remaining"] = msg.battery_remaining

        elif msg_type == "ATTITUDE":
            state["roll"]  = round(math.degrees(msg.roll),  1)
            state["pitch"] = round(math.degrees(msg.pitch), 1)
            state["yaw"]   = round(math.degrees(msg.yaw),   1)

        elif msg_type == "VFR_HUD":
            state["airspeed"]    = round(msg.airspeed,    1)
            state["groundspeed"] = round(msg.groundspeed, 1)
            state["heading"]     = msg.heading
            state["throttle"]    = msg.throttle
            state["climb"]       = round(msg.climb, 1)
            state["alt"]         = round(msg.alt,   1)

        elif msg_type == "CAMERA_FEEDBACK":
            if hasattr(msg, 'img_idx'):
                state["img_idx"] = msg.img_idx


# =============================================================================
# 数据转发
# =============================================================================
def _forward_drone_to_gcs(imei: str, data: bytes) -> None:
    """将无人机数据转发给所有配对了该无人机的在线 GCS 用户"""
    with _pairing_cache_lock:
        usernames = list(_pairing_cache["i2u"].get(imei, []))
    for username in usernames:
        with gcs_lock:
            session = gcs_sessions.get(username)
        if not session:
            continue
        try:
            session["socket"].sendall(data)
        except Exception:
            pass  # GCS 断线，由其连接线程负责清理


def _route_gcs_to_drones(username: str, data: bytes) -> None:
    """将 GCS 数据转发给该用户配对的所有无人机"""
    with _pairing_cache_lock:
        imeis = list(_pairing_cache["u2i"].get(username, []))
    for imei in imeis:
        with drone_lock:
            entry = drone_routes.get(imei)
        if not entry:
            continue
        try:
            entry["socket"].sendall(data)
        except Exception:
            pass


# =============================================================================
# 阶段二：无人机连接处理（5760 端口）
# =============================================================================
def _extract_imei(data: bytes) -> Optional[str]:
    """尝试从数据帧中提取 IMEI（15 位纯数字字符串）"""
    stripped = data.strip()
    if IMEI_PATTERN.match(stripped):
        return stripped.decode("ascii")
    return None


def handle_drone(sock: socket.socket, addr: tuple) -> None:
    """处理一个无人机 TCP 连接（5760 端口）"""
    ip = addr[0]
    logger.info(f"无人机新连接: {ip}:{addr[1]}")

    # ── 等待注册包 ──────────────────────────────────────────────
    sock.settimeout(DRONE_REGISTER_TIMEOUT)
    try:
        raw = sock.recv(256)
    except socket.timeout:
        logger.warning(f"注册包超时，断开: {ip}:{addr[1]}")
        sock.close()
        return
    except OSError:
        sock.close()
        return

    imei = _extract_imei(raw)
    if not imei:
        logger.warning(f"无效注册包（非 IMEI），断开: {ip}:{addr[1]} data={raw[:32]!r}")
        sock.close()
        return

    if not db.drone_exists(imei):
        logger.warning(f"IMEI 不在白名单，断开: IMEI={imei} from {ip}")
        sock.close()
        return

    logger.info(f"无人机注册成功: IMEI={imei} from {ip}:{addr[1]}")

    # ── 注册到路由表（踢掉同 IMEI 的老连接）────────────────────
    with drone_lock:
        old = drone_routes.get(imei)
        if old:
            try:
                old["socket"].close()
            except Exception:
                pass
        drone_routes[imei] = {
            "socket":    sock,
            "addr":      addr,
            "sysid":     None,
            "last_seen": time.time(),
        }

    # ── 数据文件（按 IMEI / 日期子文件夹归档）──────────────────
    timestamp  = time.strftime("%Y%m%d_%H%M%S")
    date_dir   = os.path.join(DATA_DIR, imei, time.strftime("%Y-%m-%d"))
    os.makedirs(date_dir, exist_ok=True)
    tlog_path  = os.path.join(date_dir, f"{timestamp}_{imei}.tlog")

    # ── MAVLink 解析器（每连接独立实例）─────────────────────────
    mav = mavlink2.MAVLink(None)
    mav.robust_parsing = True
    msg_count = 0

    sock.settimeout(5.0)

    try:
        with open(tlog_path, "ab") as f:
            while not shutdown_event.is_set():
                try:
                    data = sock.recv(BUFFER_SIZE)
                except socket.timeout:
                    with drone_lock:
                        entry = drone_routes.get(imei)
                    if entry and entry["socket"] is sock:
                        if time.time() - entry["last_seen"] > DRONE_OFFLINE_TIMEOUT:
                            logger.warning(f"无人机心跳超时: IMEI={imei}")
                            break
                    continue
                except OSError:
                    break

                if not data:
                    logger.info(f"无人机断开: IMEI={imei}")
                    break

                # ── 心跳包检测（IMEI 字符串）──────────────────────
                if _extract_imei(data) == imei:
                    with drone_lock:
                        entry = drone_routes.get(imei)
                        if entry and entry["socket"] is sock:
                            entry["last_seen"] = time.time()
                    logger.debug(f"心跳包: IMEI={imei}")
                    continue

                # ── 正常 MAVLink 数据 ──────────────────────────────
                with drone_lock:
                    entry = drone_routes.get(imei)
                    if entry and entry["socket"] is sock:
                        entry["last_seen"] = time.time()

                f.write(data)
                f.flush()

                try:
                    msgs = mav.parse_buffer(data)
                    if msgs:
                        for msg in msgs:
                            msg_count += 1
                            if msg_count <= 3:
                                logger.info(
                                    f"MAVLink: IMEI={imei} type={msg.get_type()} "
                                    f"sysid={msg.get_srcSystem()}"
                                )
                            elif msg_count == 4:
                                logger.info(f"IMEI={imei} 后续消息不再逐条日志")
                            _process_mavlink_message(msg, imei, ip)
                except Exception as e:
                    logger.debug(f"MAVLink 解析警告: {e}")

                _forward_drone_to_gcs(imei, data)

                # 更新原始16进制调试缓存（保留最近512字节）
                with raw_hex_lock:
                    raw_hex_per_imei[imei] = data[:512].hex(' ')

    except OSError as e:
        logger.error(f"无人机连接文件写入错误: IMEI={imei}, {e}")
    finally:
        with drone_lock:
            entry = drone_routes.get(imei)
            if entry and entry["socket"] is sock:
                drone_routes.pop(imei, None)
                sysid = entry.get("sysid")
                if sysid and sysid_to_imei.get(sysid) == imei:
                    sysid_to_imei.pop(sysid, None)
        sock.close()
        file_size = os.path.getsize(tlog_path) if os.path.exists(tlog_path) else 0
        if file_size == 0 and os.path.exists(tlog_path):
            os.remove(tlog_path)
        else:
            logger.info(f"数据已保存: {tlog_path} ({file_size} 字节)")
        logger.info(f"无人机连接已关闭: IMEI={imei}")


# =============================================================================
# 阶段三：地面站连接处理（5761 端口，TLS + 认证）
# =============================================================================
def _recv_line(sock, max_bytes: int = 512) -> Optional[str]:
    """从 socket 逐字节读取一行（以 \\n 结束）"""
    buf = b""
    while len(buf) < max_bytes:
        try:
            c = sock.recv(1)
        except OSError:
            return None
        if not c:
            return None
        if c == b"\n":
            return buf.decode("utf-8", errors="ignore").strip()
        buf += c
    return None


def handle_gcs(raw_sock: socket.socket, addr: tuple) -> None:
    """处理一个地面站 TCP 连接（5761 端口）"""
    ip = addr[0]

    # ── IP 封禁检查 ─────────────────────────────────────────────
    if _is_ip_banned(ip):
        try:
            raw_sock.sendall(b"ERR IP_BANNED\n")
        except Exception:
            pass
        raw_sock.close()
        logger.warning(f"GCS 连接被封禁 IP 拒绝: {ip}")
        return

    # ── TLS 握手 ─────────────────────────────────────────────────
    if ssl_context is not None:
        try:
            sock = ssl_context.wrap_socket(raw_sock, server_side=True)
        except ssl.SSLError as e:
            logger.warning(f"TLS 握手失败: {ip}, {e}")
            raw_sock.close()
            return
    else:
        sock = raw_sock  # 开发模式明文

    sock.settimeout(15.0)

    # ── 读取 AUTH 命令（格式: AUTH <username> <password>\n）────
    auth_line = _recv_line(sock)
    if not auth_line:
        sock.close()
        return

    parts = auth_line.split(" ", 2)
    if len(parts) != 3 or parts[0] != "AUTH":
        try:
            sock.sendall(b"ERR BAD_REQUEST\n")
        except Exception:
            pass
        sock.close()
        return

    username = parts[1]
    password = parts[2]

    user = db.user_verify(username, password)
    if not user:
        banned = _record_login_failure(ip)
        try:
            sock.sendall(b"ERR IP_BANNED\n" if banned else b"ERR INVALID_CREDENTIALS\n")
        except Exception:
            pass
        sock.close()
        logger.warning(f"GCS 认证失败: user={username!r} from {ip}")
        return

    _clear_login_failures(ip)
    token = secrets.token_hex(16)
    try:
        sock.sendall(f"OK {token}\n".encode())
    except Exception:
        sock.close()
        return

    logger.info(f"GCS 认证成功: user={username} from {ip}:{addr[1]}")

    # ── 注册 GCS 会话（踢掉同用户的老连接）────────────────────
    with gcs_lock:
        old = gcs_sessions.get(username)
        if old:
            try:
                old["socket"].close()
            except Exception:
                pass
        gcs_sessions[username] = {"socket": sock, "addr": addr}

    sock.settimeout(5.0)

    # ── 数据中继循环（MP → 无人机）────────────────────────────
    try:
        while not shutdown_event.is_set():
            try:
                data = sock.recv(BUFFER_SIZE)
            except socket.timeout:
                continue
            except OSError:
                break
            if not data:
                break
            _route_gcs_to_drones(username, data)
    except Exception as e:
        logger.error(f"GCS 连接异常: user={username}, {e}")
    finally:
        with gcs_lock:
            entry = gcs_sessions.get(username)
            if entry and entry["socket"] is sock:
                gcs_sessions.pop(username, None)
        sock.close()
        logger.info(f"GCS 断线: user={username}")


# =============================================================================
# HTTP 服务器（Web 监控 + 管理 API）
# =============================================================================
class ThreadedHTTPServer(ThreadingMixIn, HTTPServer):
    daemon_threads = True


class APIHandler(BaseHTTPRequestHandler):

    # ── 工具方法 ─────────────────────────────────────────────────
    def _json(self, status: int, data) -> None:
        body = json.dumps(data, ensure_ascii=False).encode()
        self.send_response(status)
        self.send_header("Content-Type",   "application/json")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Access-Control-Allow-Origin", "*")
        self.end_headers()
        self.wfile.write(body)

    def _read_json(self) -> dict:
        length = int(self.headers.get("Content-Length", 0))
        if length <= 0:
            return {}
        try:
            return json.loads(self.rfile.read(length))
        except json.JSONDecodeError:
            return {}

    def _auth_user(self) -> Optional[dict]:
        """验证认证令牌：优先 Authorization 头，其次 ?token= 查询参数（供文件下载使用）"""
        auth = self.headers.get("Authorization", "")
        if auth.startswith("Bearer "):
            return _verify_api_token(auth[7:])
        # 从 URL 查询参数读取（用于浏览器原生下载）
        qs = urllib.parse.parse_qs(self.path.split("?", 1)[1] if "?" in self.path else "")
        tk = qs.get("token", [""])[0]
        if tk:
            return _verify_api_token(tk)
        return None

    def _require_admin(self) -> Optional[dict]:
        user = self._auth_user()
        if not user:
            self._json(401, {"error": "Unauthorized"})
            return None
        if not user.get("is_admin"):
            self._json(403, {"error": "Forbidden"})
            return None
        return user

    def _send_file_stream(self, filepath: str, filename: str) -> None:
        """分块流式发送文件，浏览器可立即显示下载进度"""
        try:
            size = os.path.getsize(filepath)
            self.send_response(200)
            self.send_header("Content-Type", "application/octet-stream")
            self.send_header("Content-Disposition", f'attachment; filename="{filename}"')
            self.send_header("Content-Length", str(size))
            self.send_header("Cache-Control", "no-cache")
            self.end_headers()
            with open(filepath, "rb") as f:
                while True:
                    chunk = f.read(65536)  # 64 KB 块
                    if not chunk:
                        break
                    self.wfile.write(chunk)
        except FileNotFoundError:
            self.send_error(404)
        except Exception:
            pass  # 客户端中断下载时正常退出

    def _serve_file(self, filepath: str, content_type: str) -> None:
        try:
            with open(filepath, "rb") as f:
                data = f.read()
            self.send_response(200)
            self.send_header("Content-Type",   content_type)
            self.send_header("Content-Length", str(len(data)))
            self.end_headers()
            self.wfile.write(data)
        except FileNotFoundError:
            self.send_error(404)

    def log_message(self, fmt, *args):
        pass

    # ── 航线规划反向代理 ──────────────────────────────────────────
    def _proxy_to_planner(self) -> None:
        """将 /planner/* 请求透明代理到 FlightPlanner 服务"""
        target_path = self.path[len("/planner"):]
        if not target_path or target_path[0] != '/':
            target_path = '/' + (target_path or '')
        try:
            parsed  = urllib.parse.urlparse(PLANNER_URL)
            host    = parsed.netloc
            content_length = int(self.headers.get("Content-Length", 0))
            body    = self.rfile.read(content_length) if content_length > 0 else None
            fwd_headers: dict = {}
            for key, val in self.headers.items():
                if key.lower() not in ('host', 'transfer-encoding', 'connection'):
                    fwd_headers[key] = val
            fwd_headers['X-Forwarded-Prefix'] = '/planner'
            fwd_headers['Host'] = host
            conn = http.client.HTTPConnection(host, timeout=120)
            conn.request(self.command, target_path, body=body, headers=fwd_headers)
            resp = conn.getresponse()
            self.send_response(resp.status)
            for key, val in resp.getheaders():
                if key.lower() not in ('transfer-encoding', 'connection', 'server'):
                    self.send_header(key, val)
            self.end_headers()
            shutil.copyfileobj(resp, self.wfile)
            conn.close()
        except Exception as exc:
            logger.warning(f"Planner proxy error [{self.command} {self.path}]: {exc}")
            try:
                self.send_error(502, "Bad Gateway")
            except Exception:
                pass

    # ── CORS 预检 ─────────────────────────────────────────────────
    def do_OPTIONS(self):
        self.send_response(200)
        self.send_header("Access-Control-Allow-Origin",  "*")
        self.send_header("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT, OPTIONS")
        self.send_header("Access-Control-Allow-Headers", "Authorization, Content-Type")
        self.send_header("Content-Length", "0")
        self.end_headers()

    # ── GET ───────────────────────────────────────────────────────
    def do_GET(self):
        path = self.path.split("?")[0]

        # 代理到航线规划服务
        if path.startswith("/planner"):
            return self._proxy_to_planner()

        # 公开：飞机状态（Web 地图）
        if path == "/api/aircraft":
            with raw_hex_lock:
                _rxh = dict(raw_hex_per_imei)
            with aircraft_lock:
                data = {}
                for sys_id, state in aircraft_states.items():
                    d = {k: v for k, v in state.items() if k != "trail"}
                    d["trail_length"] = len(state["trail"])
                    imei = state.get("imei", "")
                    with drone_lock:
                        d["online"] = imei in drone_routes
                    d["last_raw_hex"] = _rxh.get(imei, "")
                    data[str(sys_id)] = d
            return self._json(200, data)

        if path.startswith("/api/trail/"):
            try:
                sys_id = int(path.split("/")[3])
                with aircraft_lock:
                    trail = aircraft_states.get(sys_id, {}).get("trail", [])
                return self._json(200, trail)
            except (ValueError, IndexError):
                self.send_error(400)
                return

        # 管理 API（需 Admin Token）
        if path == "/api/drones":
            if not self._require_admin():
                return
            drones = db.drone_list()
            with drone_lock:
                for d in drones:
                    imei  = d["imei"]
                    entry = drone_routes.get(imei)
                    d["online"]    = entry is not None
                    d["sysid"]     = entry.get("sysid")     if entry else None
                    d["last_seen"] = entry.get("last_seen") if entry else None
                    with _pairing_cache_lock:
                        d["paired_users"] = _pairing_cache["i2u"].get(imei, [])
            return self._json(200, drones)

        if path == "/api/users":
            if not self._require_admin():
                return
            return self._json(200, db.user_list())

        if path == "/api/pairings":
            if not self._require_admin():
                return
            return self._json(200, db.pairing_list_all())

        if path == "/api/status":
            user = self._auth_user()
            if not user:
                return self._json(401, {"error": "Unauthorized"})
            with drone_lock:
                online_drones = [
                    {"imei": imei, "sysid": e["sysid"], "last_seen": e["last_seen"]}
                    for imei, e in drone_routes.items()
                ]
            with gcs_lock:
                online_gcs = list(gcs_sessions.keys())
            return self._json(200, {
                "online_drones": online_drones,
                "online_gcs":    online_gcs,
            })

        # 地面站代理软件下载
        if path == "/api/download/proxy":
            if not self._auth_user():
                return self._json(401, {"error": "Unauthorized"})
            if not os.path.isfile(PROXY_EXE):
                return self._json(404, {"error": "File not found"})
            return self._send_file_stream(PROXY_EXE, os.path.basename(PROXY_EXE))

        # 日志浏览 API（需认证，普通用户只能看自己配对 IMEI 的日志）
        if path == "/api/logs":
            user = self._auth_user()
            if not user:
                return self._json(401, {"error": "Unauthorized"})
            try:
                if not os.path.isdir(SAVE_DIR):
                    return self._json(200, [])
                all_imeis = [
                    d for d in os.listdir(SAVE_DIR)
                    if os.path.isdir(os.path.join(SAVE_DIR, d)) and re.match(r'^\d{15}$', d)
                ]
                if user.get("is_admin"):
                    visible = sorted(all_imeis)
                else:
                    with _pairing_cache_lock:
                        paired = set(_pairing_cache["u2i"].get(user["username"], []))
                    visible = sorted([i for i in all_imeis if i in paired])
                result = []
                for imei in visible:
                    imei_dir = os.path.join(SAVE_DIR, imei)
                    dates = sorted(
                        [d for d in os.listdir(imei_dir)
                         if os.path.isdir(os.path.join(imei_dir, d)) and re.match(r'^\d{4}-\d{2}-\d{2}$', d)],
                        reverse=True
                    )
                    result.append({"imei": imei, "dates": dates})
                return self._json(200, result)
            except Exception:
                return self._json(500, {"error": "Internal error"})

        if path == "/api/logs/files":
            user = self._auth_user()
            if not user:
                return self._json(401, {"error": "Unauthorized"})
            qs = urllib.parse.parse_qs(self.path.split("?", 1)[1] if "?" in self.path else "")
            imei = qs.get("imei", [""])[0]
            date = qs.get("date", [""])[0]
            if not re.match(r'^\d{15}$', imei) or not re.match(r'^\d{4}-\d{2}-\d{2}$', date):
                return self._json(400, {"error": "Invalid params"})
            if not user.get("is_admin"):
                with _pairing_cache_lock:
                    paired = set(_pairing_cache["u2i"].get(user["username"], []))
                if imei not in paired:
                    return self._json(403, {"error": "Forbidden"})
            dir_path = os.path.realpath(os.path.join(SAVE_DIR, imei, date))
            save_real = os.path.realpath(SAVE_DIR)
            if not dir_path.startswith(save_real + os.sep):
                return self._json(403, {"error": "Forbidden"})
            try:
                if not os.path.isdir(dir_path):
                    return self._json(200, [])
                files = sorted(
                    [f for f in os.listdir(dir_path) if os.path.isfile(os.path.join(dir_path, f))]
                )
                result = [{"name": f, "size": os.path.getsize(os.path.join(dir_path, f))} for f in files]
                return self._json(200, result)
            except Exception:
                return self._json(500, {"error": "Internal error"})

        if path == "/api/logs/download":
            user = self._auth_user()
            if not user:
                return self._json(401, {"error": "Unauthorized"})
            qs = urllib.parse.parse_qs(self.path.split("?", 1)[1] if "?" in self.path else "")
            file_param = qs.get("file", [""])[0]
            filepath = os.path.realpath(os.path.join(SAVE_DIR, file_param))
            save_real = os.path.realpath(SAVE_DIR)
            if not filepath.startswith(save_real + os.sep):
                return self._json(403, {"error": "Forbidden"})
            # 权限：从路径中提取 IMEI（第一层目录）并校验
            rel = os.path.relpath(filepath, save_real)
            imei_from_path = rel.split(os.sep)[0]
            if not user.get("is_admin"):
                with _pairing_cache_lock:
                    paired = set(_pairing_cache["u2i"].get(user["username"], []))
                if imei_from_path not in paired:
                    return self._json(403, {"error": "Forbidden"})
            if not os.path.isfile(filepath):
                return self._json(404, {"error": "Not found"})
            return self._send_file_stream(filepath, os.path.basename(filepath))

        # 静态文件
        if path in ("/", "/index.html"):
            return self._serve_file(
                os.path.join(WEB_DIR, "index.html"), "text/html; charset=utf-8"
            )
        safe = os.path.normpath(path.lstrip("/"))
        fp   = os.path.join(WEB_DIR, safe)
        if os.path.isfile(fp) and os.path.commonpath([WEB_DIR, fp]) == WEB_DIR:
            ct = mimetypes.guess_type(fp)[0] or "application/octet-stream"
            return self._serve_file(fp, ct)
        self.send_error(404)

    # ── POST ──────────────────────────────────────────────────────
    def do_POST(self):
        path = self.path.split("?")[0]

        # 代理到航线规划服务
        if path.startswith("/planner"):
            return self._proxy_to_planner()

        # 登录（公开）
        if path == "/api/login":
            ip = self.client_address[0]
            if _is_ip_banned(ip):
                return self._json(429, {"error": "IP_BANNED"})
            body     = self._read_json()
            username = str(body.get("username", ""))
            password = str(body.get("password", ""))
            user     = db.user_verify(username, password)
            if not user:
                banned = _record_login_failure(ip)
                return self._json(429 if banned else 401, {
                    "error": "IP_BANNED" if banned else "INVALID_CREDENTIALS"
                })
            _clear_login_failures(ip)
            token = _issue_api_token(username, user["is_admin"])
            return self._json(200, {
                "token":    token,
                "username": username,
                "is_admin": user["is_admin"],
            })

        if not self._require_admin():
            return

        if path == "/api/drones":
            body = self._read_json()
            imei = str(body.get("imei", "")).strip()
            name = str(body.get("name", "")).strip()
            if not re.match(r"^\d{15}$", imei):
                return self._json(400, {"error": "IMEI 格式错误，需为 15 位数字"})
            ok = db.drone_add(imei, name)
            if ok:
                refresh_pairing_cache()
            return self._json(200 if ok else 409, {
                "ok": ok, "error": None if ok else "IMEI 已存在"
            })

        if path == "/api/users":
            body     = self._read_json()
            username = str(body.get("username", "")).strip()
            password = str(body.get("password", "")).strip()
            is_admin = bool(body.get("is_admin", False))
            if not username or not password:
                return self._json(400, {"error": "username 和 password 不能为空"})
            if len(password) < 8:
                return self._json(400, {"error": "密码至少 8 位"})
            ok = db.user_add(username, password, is_admin)
            return self._json(200 if ok else 409, {
                "ok": ok, "error": None if ok else "用户名已存在"
            })

        if path == "/api/pairings":
            body     = self._read_json()
            username = str(body.get("username", "")).strip()
            imei     = str(body.get("imei", "")).strip()
            if not username or not imei:
                return self._json(400, {"error": "username 和 imei 不能为空"})
            ok = db.pairing_add(username, imei)
            if ok:
                refresh_pairing_cache()
            return self._json(200 if ok else 409, {
                "ok": ok, "error": None if ok else "配对已存在"
            })

        self.send_error(404)

    # ── DELETE ────────────────────────────────────────────────────
    def do_DELETE(self):
        path = self.path.split("?")[0]
        if path.startswith("/planner"):
            return self._proxy_to_planner()
        if not self._require_admin():
            return

        m = re.match(r"^/api/drones/([^/]+)$", path)
        if m:
            imei = m.group(1)
            ok   = db.drone_remove(imei)
            if ok:
                refresh_pairing_cache()
            return self._json(200, {"ok": ok})

        m = re.match(r"^/api/users/([^/]+)$", path)
        if m:
            username = m.group(1)
            ok = db.user_remove(username)
            if ok:
                refresh_pairing_cache()
                with gcs_lock:
                    session = gcs_sessions.pop(username, None)
                if session:
                    try:
                        session["socket"].close()
                    except Exception:
                        pass
            return self._json(200, {"ok": ok})

        if path == "/api/pairings":
            body     = self._read_json()
            username = str(body.get("username", "")).strip()
            imei     = str(body.get("imei", "")).strip()
            ok = db.pairing_remove(username, imei)
            if ok:
                refresh_pairing_cache()
            return self._json(200, {"ok": ok})

        self.send_error(404)

    # ── PUT ───────────────────────────────────────────────────────
    def do_PUT(self):
        path = self.path.split("?")[0]
        if path.startswith("/planner"):
            return self._proxy_to_planner()
        user = self._auth_user()
        if not user:
            return self._json(401, {"error": "Unauthorized"})

        m = re.match(r"^/api/users/([^/]+)/password$", path)
        if m:
            target = m.group(1)
            if not user["is_admin"] and user["username"] != target:
                return self._json(403, {"error": "Forbidden"})
            body   = self._read_json()
            new_pw = str(body.get("password", "")).strip()
            if len(new_pw) < 8:
                return self._json(400, {"error": "密码至少 8 位"})
            ok = db.user_change_password(target, new_pw)
            return self._json(200, {"ok": ok})

        m = re.match(r"^/api/drones/([^/]+)$", path)
        if m:
            if not user.get("is_admin"):
                return self._json(403, {"error": "Forbidden"})
            imei = m.group(1)
            body = self._read_json()
            name = str(body.get("name", "")).strip()
            ok   = db.drone_update_name(imei, name)
            return self._json(200, {"ok": ok})

        self.send_error(404)


# =============================================================================
# 服务器启动函数
# =============================================================================
def run_drone_server() -> None:
    srv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    srv.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    srv.settimeout(2.0)
    try:
        srv.bind((HOST, DRONE_PORT))
        srv.listen(MAX_CONNECTIONS)
        logger.info(f"无人机接入服务器已启动，监听 {HOST}:{DRONE_PORT}")
        while not shutdown_event.is_set():
            try:
                client_sock, client_addr = srv.accept()
                threading.Thread(
                    target=handle_drone, args=(client_sock, client_addr), daemon=True
                ).start()
            except socket.timeout:
                continue
    except OSError as e:
        logger.error(f"无人机服务器错误: {e}")
    finally:
        srv.close()
        logger.info("无人机接入服务器已关闭")


def run_gcs_server() -> None:
    srv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    srv.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    srv.settimeout(2.0)
    try:
        srv.bind((HOST, GCS_PORT))
        srv.listen(MAX_CONNECTIONS)
        tls_status = "TLS 加密" if ssl_context else "明文（无证书）"
        logger.info(f"GCS 接入服务器已启动，监听 {HOST}:{GCS_PORT} [{tls_status}]")
        while not shutdown_event.is_set():
            try:
                client_sock, client_addr = srv.accept()
                threading.Thread(
                    target=handle_gcs, args=(client_sock, client_addr), daemon=True
                ).start()
            except socket.timeout:
                continue
    except OSError as e:
        logger.error(f"GCS 服务器错误: {e}")
    finally:
        srv.close()
        logger.info("GCS 接入服务器已关闭")


def run_http_server() -> None:
    server = ThreadedHTTPServer((HOST, HTTP_PORT), APIHandler)
    logger.info(f"HTTP 服务器已启动，监听 {HOST}:{HTTP_PORT}")
    server.serve_forever()
    server.server_close()
    logger.info("HTTP 服务器已关闭")


def signal_handler(signum, frame) -> None:
    logger.info("收到退出信号，正在关闭...")
    shutdown_event.set()


# =============================================================================
# 主入口
# =============================================================================
def main() -> None:
    signal.signal(signal.SIGTERM, signal_handler)
    signal.signal(signal.SIGINT,  signal_handler)

    ensure_data_dir()
    db.init_db(admin_user=ADMIN_USER, admin_password=ADMIN_PASS)
    refresh_pairing_cache()
    init_tls()

    threading.Thread(target=run_http_server, daemon=True).start()
    threading.Thread(target=run_gcs_server,  daemon=True).start()

    run_drone_server()


if __name__ == "__main__":
    main()
