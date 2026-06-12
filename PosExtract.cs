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
            this.Text = "POS点提取工具";
            this.ClientSize = new Size(990, 490);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(800, 420);

            // Left panel
            panelLeft = new Panel();
            panelLeft.Location = new Point(0, 0);
            panelLeft.Size = new Size(185, 490);
            panelLeft.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            // Title label
            labelTitle = new Label();
            labelTitle.Text = "POS点提取工具";
            labelTitle.Location = new Point(12, 15);
            labelTitle.Size = new Size(160, 25);
            labelTitle.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);

            // Import button
            btnImportBin = new Button();
            btnImportBin.Text = "导入 .bin文件";
            btnImportBin.Location = new Point(12, 50);
            btnImportBin.Size = new Size(160, 30);
            btnImportBin.Click += btnImportBin_Click;

            // Count label
            lblCount = new Label();
            lblCount.Text = "统计0";
            lblCount.Location = new Point(12, 90);
            lblCount.Size = new Size(160, 23);

            // CAM/TRIG filter
            labelFilter = new Label();
            labelFilter.Text = "CAM/TRIG筛选";
            labelFilter.Location = new Point(12, 120);
            labelFilter.Size = new Size(160, 20);

            cmbFilter = new ComboBox();
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.Items.AddRange(new object[] { "全部(CAM+TRIG)", "仅CAM", "仅TRIG" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.Location = new Point(12, 143);
            cmbFilter.Size = new Size(160, 23);
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;

            // Photo prefix
            labelPrefix = new Label();
            labelPrefix.Text = "照片前缀";
            labelPrefix.Location = new Point(12, 175);
            labelPrefix.Size = new Size(160, 20);

            txtPhotoPrefix = new TextBox();
            txtPhotoPrefix.Text = "DSC";
            txtPhotoPrefix.Location = new Point(12, 198);
            txtPhotoPrefix.Size = new Size(160, 23);

            // Photo extension
            labelExt = new Label();
            labelExt.Text = "照片扩展名";
            labelExt.Location = new Point(12, 230);
            labelExt.Size = new Size(160, 20);

            txtPhotoExt = new TextBox();
            txtPhotoExt.Text = "JPG";
            txtPhotoExt.Location = new Point(12, 253);
            txtPhotoExt.Size = new Size(160, 23);

            // Photo start number
            labelStartNum = new Label();
            labelStartNum.Text = "照片起始编号";
            labelStartNum.Location = new Point(12, 285);
            labelStartNum.Size = new Size(160, 20);

            txtPhotoStartNum = new TextBox();
            txtPhotoStartNum.Text = "00006";
            txtPhotoStartNum.Location = new Point(12, 308);
            txtPhotoStartNum.Size = new Size(160, 23);

            // Extract POS button
            btnExtractPos = new Button();
            btnExtractPos.Text = "pos点提取";
            btnExtractPos.Location = new Point(12, 345);
            btnExtractPos.Size = new Size(160, 30);
            btnExtractPos.Click += btnExtractPos_Click;

            // Save button
            btnSave = new Button();
            btnSave.Text = "保存";
            btnSave.Location = new Point(12, 385);
            btnSave.Size = new Size(160, 30);
            btnSave.Click += btnSave_Click;

            // Progress bar
            progressBar1 = new ProgressBar();
            progressBar1.Location = new Point(12, 430);
            progressBar1.Size = new Size(160, 20);

            // Add controls to left panel
            panelLeft.Controls.AddRange(new Control[] {
                labelTitle, btnImportBin, lblCount,
                labelFilter, cmbFilter,
                labelPrefix, txtPhotoPrefix,
                labelExt, txtPhotoExt,
                labelStartNum, txtPhotoStartNum,
                btnExtractPos, btnSave, progressBar1
            });

            // Raw data label
            labelRawData = new Label();
            labelRawData.Text = "原始提取数据（上） / POS结果（下）";
            labelRawData.Location = new Point(195, 10);
            labelRawData.Size = new Size(400, 20);
            labelRawData.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Split container for two DataGridViews
            splitContainer1 = new SplitContainer();
            splitContainer1.Location = new Point(195, 35);
            splitContainer1.Size = new Size(780, 410);
            splitContainer1.Orientation = Orientation.Horizontal;
            splitContainer1.SplitterDistance = 190;
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Upper DataGridView - raw data
            dgvRawData = new DataGridView();
            dgvRawData.Dock = DockStyle.Fill;
            dgvRawData.AllowUserToAddRows = false;
            dgvRawData.AllowUserToDeleteRows = false;
            dgvRawData.ReadOnly = true;
            dgvRawData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Lower DataGridView - result data
            dgvResultData = new DataGridView();
            dgvResultData.Dock = DockStyle.Fill;
            dgvResultData.AllowUserToAddRows = false;
            dgvResultData.AllowUserToDeleteRows = false;
            dgvResultData.ReadOnly = true;
            dgvResultData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            splitContainer1.Panel1.Controls.Add(dgvRawData);
            splitContainer1.Panel2.Controls.Add(dgvResultData);

            // Status label
            lblStatus = new Label();
            lblStatus.Text = "就绪";
            lblStatus.Location = new Point(195, 455);
            lblStatus.Size = new Size(780, 23);
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // Add all to form
            this.Controls.AddRange(new Control[] {
                panelLeft, labelRawData, splitContainer1, lblStatus
            });
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
