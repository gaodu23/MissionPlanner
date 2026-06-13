using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Utilities;

namespace MissionPlanner
{
    public partial class PosExtract : Form
    {
        private List<PosData> _posDataList = new List<PosData>();
        private string _logFilePath;
        private string _binFilePath;

        // Controls
        private Panel panelLeft;
        private Label labelTitle;
        private Button btnImportBin;
        private Label lblCount;
        private Label labelFilter;
        private ComboBox cmbFilter;
        private Label labelPrefix;
        private TextBox txtPhotoPrefix;
        private Label labelExt;
        private TextBox txtPhotoExt;
        private Label labelStartNum;
        private TextBox txtPhotoStartNum;
        private Button btnExtractPos;
        private Button btnSave;
        private ProgressBar progressBar1;
        private Label labelRawData;
        private SplitContainer splitContainer1;
        private DataGridView dgvRawData;
        private DataGridView dgvResultData;
        private Label lblStatus;

        public PosExtract()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PosExtract));
            this.panelLeft = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnImportBin = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.txtPhotoPrefix = new System.Windows.Forms.TextBox();
            this.labelExt = new System.Windows.Forms.Label();
            this.txtPhotoExt = new System.Windows.Forms.TextBox();
            this.labelStartNum = new System.Windows.Forms.Label();
            this.txtPhotoStartNum = new System.Windows.Forms.TextBox();
            this.btnExtractPos = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelRawData = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvRawData = new System.Windows.Forms.DataGridView();
            this.dgvResultData = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRawData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultData)).BeginInit();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLeft.Controls.Add(this.labelTitle);
            this.panelLeft.Controls.Add(this.btnImportBin);
            this.panelLeft.Controls.Add(this.lblCount);
            this.panelLeft.Controls.Add(this.lblStatus);
            this.panelLeft.Controls.Add(this.labelFilter);
            this.panelLeft.Controls.Add(this.cmbFilter);
            this.panelLeft.Controls.Add(this.labelPrefix);
            this.panelLeft.Controls.Add(this.txtPhotoPrefix);
            this.panelLeft.Controls.Add(this.labelExt);
            this.panelLeft.Controls.Add(this.txtPhotoExt);
            this.panelLeft.Controls.Add(this.labelStartNum);
            this.panelLeft.Controls.Add(this.txtPhotoStartNum);
            this.panelLeft.Controls.Add(this.btnExtractPos);
            this.panelLeft.Controls.Add(this.btnSave);
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(185, 490);
            this.panelLeft.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(160, 25);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "POS点提取工具";
            // 
            // btnImportBin
            // 
            this.btnImportBin.Location = new System.Drawing.Point(12, 50);
            this.btnImportBin.Name = "btnImportBin";
            this.btnImportBin.Size = new System.Drawing.Size(160, 30);
            this.btnImportBin.TabIndex = 1;
            this.btnImportBin.Text = "导入 .bin文件";
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(12, 98);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(160, 23);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "统计0";
            // 
            // labelFilter
            // 
            this.labelFilter.Location = new System.Drawing.Point(12, 128);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(160, 20);
            this.labelFilter.TabIndex = 3;
            this.labelFilter.Text = "CAM/TRIG筛选";
            // 
            // cmbFilter
            // 
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.Items.AddRange(new object[] {
            "全部(CAM+TRIG)",
            "仅CAM",
            "仅TRIG"});
            this.cmbFilter.Location = new System.Drawing.Point(12, 151);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(160, 20);
            this.cmbFilter.TabIndex = 4;
            // 
            // labelPrefix
            // 
            this.labelPrefix.Location = new System.Drawing.Point(12, 183);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(160, 20);
            this.labelPrefix.TabIndex = 5;
            this.labelPrefix.Text = "照片前缀";
            // 
            // txtPhotoPrefix
            // 
            this.txtPhotoPrefix.Location = new System.Drawing.Point(12, 206);
            this.txtPhotoPrefix.Name = "txtPhotoPrefix";
            this.txtPhotoPrefix.Size = new System.Drawing.Size(160, 21);
            this.txtPhotoPrefix.TabIndex = 6;
            this.txtPhotoPrefix.Text = "DSC";
            // 
            // labelExt
            // 
            this.labelExt.Location = new System.Drawing.Point(12, 238);
            this.labelExt.Name = "labelExt";
            this.labelExt.Size = new System.Drawing.Size(160, 20);
            this.labelExt.TabIndex = 7;
            this.labelExt.Text = "照片扩展名";
            // 
            // txtPhotoExt
            // 
            this.txtPhotoExt.Location = new System.Drawing.Point(12, 261);
            this.txtPhotoExt.Name = "txtPhotoExt";
            this.txtPhotoExt.Size = new System.Drawing.Size(160, 21);
            this.txtPhotoExt.TabIndex = 8;
            this.txtPhotoExt.Text = "JPG";
            // 
            // labelStartNum
            // 
            this.labelStartNum.Location = new System.Drawing.Point(12, 293);
            this.labelStartNum.Name = "labelStartNum";
            this.labelStartNum.Size = new System.Drawing.Size(160, 20);
            this.labelStartNum.TabIndex = 9;
            this.labelStartNum.Text = "照片起始编号";
            // 
            // txtPhotoStartNum
            // 
            this.txtPhotoStartNum.Location = new System.Drawing.Point(12, 316);
            this.txtPhotoStartNum.Name = "txtPhotoStartNum";
            this.txtPhotoStartNum.Size = new System.Drawing.Size(160, 21);
            this.txtPhotoStartNum.TabIndex = 10;
            this.txtPhotoStartNum.Text = "00006";
            // 
            // btnExtractPos
            // 
            this.btnExtractPos.Location = new System.Drawing.Point(12, 353);
            this.btnExtractPos.Name = "btnExtractPos";
            this.btnExtractPos.Size = new System.Drawing.Size(160, 30);
            this.btnExtractPos.TabIndex = 11;
            this.btnExtractPos.Text = "pos点提取";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 393);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 30);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保存";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(197, 458);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(781, 20);
            this.progressBar1.TabIndex = 13;
            // 
            // labelRawData
            // 
            this.labelRawData.Location = new System.Drawing.Point(195, 10);
            this.labelRawData.Name = "labelRawData";
            this.labelRawData.Size = new System.Drawing.Size(400, 20);
            this.labelRawData.TabIndex = 1;
            this.labelRawData.Text = "原始提取数据（上） / POS结果（下）";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(195, 35);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvRawData);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvResultData);
            this.splitContainer1.Size = new System.Drawing.Size(780, 410);
            this.splitContainer1.SplitterDistance = 190;
            this.splitContainer1.TabIndex = 2;
            // 
            // dgvRawData
            // 
            this.dgvRawData.AllowUserToAddRows = false;
            this.dgvRawData.AllowUserToDeleteRows = false;
            this.dgvRawData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRawData.Location = new System.Drawing.Point(0, 0);
            this.dgvRawData.Name = "dgvRawData";
            this.dgvRawData.ReadOnly = true;
            this.dgvRawData.Size = new System.Drawing.Size(780, 190);
            this.dgvRawData.TabIndex = 0;
            // 
            // dgvResultData
            // 
            this.dgvResultData.AllowUserToAddRows = false;
            this.dgvResultData.AllowUserToDeleteRows = false;
            this.dgvResultData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResultData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResultData.Location = new System.Drawing.Point(0, 0);
            this.dgvResultData.Name = "dgvResultData";
            this.dgvResultData.ReadOnly = true;
            this.dgvResultData.Size = new System.Drawing.Size(780, 216);
            this.dgvResultData.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(13, 447);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(159, 23);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "就绪";
            // 
            // PosExtract
            // 
            this.ClientSize = new System.Drawing.Size(990, 490);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.labelRawData);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 420);
            this.Name = "PosExtract";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "POS点提取工具";
            this.panelLeft.ResumeLayout(false);
            this.panelLeft.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRawData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultData)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 导入 .bin 文件并转换为 .log
        /// </summary>
        private void btnImportBin_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Binary Log|*.bin;*.BIN";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _binFilePath = ofd.FileName;
                    string outfilename = Path.GetDirectoryName(_binFilePath) + Path.DirectorySeparatorChar +
                                         Path.GetFileNameWithoutExtension(_binFilePath) + ".log";

                    try
                    {
                        progressBar1.Style = ProgressBarStyle.Marquee;
                        lblStatus.Text = "正在转换 .bin 到 .log ...";
                        Application.DoEvents();

                        // Step 1: Convert .bin to .log
                        BinaryLog.ConvertBin(_binFilePath, outfilename);
                        _logFilePath = outfilename;

                        // Step 2: Parse CAM/TRIG data from .log
                        ParseLogFile(_logFilePath);

                        progressBar1.Style = ProgressBarStyle.Blocks;
                        lblStatus.Text = $"导入完成，共提取 {_posDataList.Count} 条记录";
                    }
                    catch (Exception ex)
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        MessageBox.Show("转换失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 解析 .log 文件中的 CAM 和 TRIG 数据
        /// </summary>
        private void ParseLogFile(string logFilePath)
        {
            _posDataList.Clear();

            using (StreamReader sr = new StreamReader(logFilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Check if line starts with CAM, or TRIG,
                    if (line.StartsWith("CAM,") || line.StartsWith("TRIG,"))
                    {
                        string[] parts = line.Split(',');

                        // Expected format:
                        // msgtype, TimeUS, I, Img, GPSTime, GPSWeek, Lat, Lng, Alt, RelAlt, GPSAlt, Roll, Pitch, Yaw
                        //   0        1     2   3      4        5      6    7    8     9      10     11    12     13
                        if (parts.Length >= 14)
                        {
                            var pos = new PosData
                            {
                                MsgType = parts[0].Trim(),
                                TimeUS = parts[1].Trim(),
                                I = parts[2].Trim(),
                                Img = parts[3].Trim(),
                                GPSTime = parts[4].Trim(),
                                GPSWeek = parts[5].Trim(),
                                Lat = parts[6].Trim(),
                                Lng = parts[7].Trim(),
                                Alt = parts[8].Trim(),
                                RelAlt = parts[9].Trim(),
                                GPSAlt = parts[10].Trim(),
                                Roll = parts[11].Trim(),
                                Pitch = parts[12].Trim(),
                                Yaw = parts[13].Trim()
                            };

                            _posDataList.Add(pos);
                        }
                    }
                }
            }

            // Update UI
            UpdateDataGrid();
            lblCount.Text = $"统计{_posDataList.Count}";
        }

        /// <summary>
        /// 根据CAM/TRIG筛选更新DataGridView
        /// </summary>
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        /// <summary>
        /// 获取当前筛选后的数据列表
        /// </summary>
        private List<PosData> GetFilteredData()
        {
            if (cmbFilter.SelectedIndex == 1) // 仅CAM
                return _posDataList.Where(p => p.MsgType == "CAM").ToList();
            else if (cmbFilter.SelectedIndex == 2) // 仅TRIG
                return _posDataList.Where(p => p.MsgType == "TRIG").ToList();
            else // 全部
                return _posDataList;
        }

        /// <summary>
        /// 更新上部DataGridView显示原始提取数据
        /// </summary>
        private void UpdateDataGrid()
        {
            var filtered = GetFilteredData();
            dgvRawData.DataSource = null;
            dgvRawData.DataSource = filtered.Select((p, i) => new
            {
                Id = i,
                p.TimeUS,
                p.MsgType,
                GPSTime = p.GPSTime,
                GPSWeek = p.GPSWeek,
                Lat = p.Lat,
                Lng = p.Lng,
                RelAlt = p.RelAlt,
                Alt = p.Alt,
                p.Roll,
                p.Pitch,
                p.Yaw
            }).ToList();

            lblCount.Text = $"统计{filtered.Count}";
        }

        /// <summary>
        /// POS点提取 - 根据配置的照片名称更新下部表格
        /// </summary>
        private void btnExtractPos_Click(object sender, EventArgs e)
        {
            var filtered = GetFilteredData();
            if (filtered.Count == 0)
            {
                MessageBox.Show("没有数据可提取，请先导入.bin文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string prefix = txtPhotoPrefix.Text.Trim();
            string ext = txtPhotoExt.Text.Trim();
            if (!ext.StartsWith("."))
                ext = "." + ext;

            int startNum;
            if (!int.TryParse(txtPhotoStartNum.Text.Trim(), out startNum))
            {
                MessageBox.Show("照片起始编号无效，请输入数字。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int digits = txtPhotoStartNum.Text.Trim().Length;

            // Update lower DataGridView with photo names
            dgvResultData.DataSource = null;
            dgvResultData.DataSource = filtered.Select((p, i) => new
            {
                Id = i,
                Img = $"{prefix}{(startNum + i).ToString("D" + digits)}{ext}",
                GPSTime = p.GPSTime,
                GPSWeek = p.GPSWeek,
                Lat = p.Lat,
                Lng = p.Lng,
                RelAlt = p.RelAlt,
                Alt = p.Alt,
                p.Roll,
                p.Pitch,
                p.Yaw
            }).ToList();

            lblStatus.Text = $"已生成 {filtered.Count} 条POS数据";
        }

        /// <summary>
        /// 保存为CSV文件
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvResultData.DataSource == null)
            {
                MessageBox.Show("请先点击「pos点提取」生成数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files|*.csv";

                // 默认文件名与导入的BIN文件同名，保存路径相同
                if (!string.IsNullOrEmpty(_binFilePath))
                {
                    string defaultCsv = Path.GetDirectoryName(_binFilePath) + Path.DirectorySeparatorChar +
                                        Path.GetFileNameWithoutExtension(_binFilePath) + ".csv";
                    sfd.FileName = Path.GetFileName(defaultCsv);
                    sfd.InitialDirectory = Path.GetDirectoryName(_binFilePath);
                }
                else
                {
                    sfd.FileName = "pos_data.csv";
                }

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SaveToCsv(sfd.FileName);
                        lblStatus.Text = $"已保存到: {sfd.FileName}";
                        MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 将数据保存为CSV格式
        /// Output format: Id,Img,GPSTime,GPSWeek,Lat,Lng,RelAlt,Alt,Roll,Pitch,Yaw
        /// </summary>
        private void SaveToCsv(string filePath)
        {
            string prefix = txtPhotoPrefix.Text.Trim();
            string ext = txtPhotoExt.Text.Trim();
            if (!ext.StartsWith("."))
                ext = "." + ext;

            int startNum;
            int.TryParse(txtPhotoStartNum.Text.Trim(), out startNum);
            int digits = txtPhotoStartNum.Text.Trim().Length;

            var filtered = GetFilteredData();

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write header
                sw.WriteLine("Id,Img,GPSTime,GPSWeek,Lat,Lng,RelAlt,Alt,Roll,Pitch,Yaw");

                for (int i = 0; i < filtered.Count; i++)
                {
                    var p = filtered[i];
                    string photoName = $"{prefix}{(startNum + i).ToString("D" + digits)}{ext}";

                    sw.WriteLine($"{i},{photoName},{p.GPSTime},{p.GPSWeek},{p.Lat},{p.Lng},{p.RelAlt},{p.Alt},{p.Roll},{p.Pitch},{p.Yaw}");
                }
            }
        }

        /// <summary>
        /// 从外部设置bin文件路径并自动处理
        /// </summary>
        public void LoadBinFile(string binFilePath)
        {
            _binFilePath = binFilePath;
            string outfilename = Path.GetDirectoryName(_binFilePath) + Path.DirectorySeparatorChar +
                                 Path.GetFileNameWithoutExtension(_binFilePath) + ".log";

            try
            {
                BinaryLog.ConvertBin(_binFilePath, outfilename);
                _logFilePath = outfilename;
                ParseLogFile(_logFilePath);
                lblStatus.Text = $"导入完成，共提取 {_posDataList.Count} 条记录";
            }
            catch (Exception ex)
            {
                MessageBox.Show("转换失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class PosData
        {
            public string MsgType { get; set; }
            public string TimeUS { get; set; }
            public string I { get; set; }
            public string Img { get; set; }
            public string GPSTime { get; set; }
            public string GPSWeek { get; set; }
            public string Lat { get; set; }
            public string Lng { get; set; }
            public string Alt { get; set; }
            public string RelAlt { get; set; }
            public string GPSAlt { get; set; }
            public string Roll { get; set; }
            public string Pitch { get; set; }
            public string Yaw { get; set; }
        }
    }
}
