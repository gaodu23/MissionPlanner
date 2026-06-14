using System;
using System.IO;

namespace MissionPlanner
{
    partial class MainV2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("mainv2_Dispose");
            if (PluginThreadrunner != null)
                PluginThreadrunner.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainV2));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.CTX_mainmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.autoHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readonlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFlightData = new System.Windows.Forms.ToolStripButton();
            this.MenuFlightPlanner = new System.Windows.Forms.ToolStripButton();
            this.MenuInitConfig = new System.Windows.Forms.ToolStripButton();
            this.MenuConfigTune = new System.Windows.Forms.ToolStripButton();
            this.MenuSimulation = new System.Windows.Forms.ToolStripButton();
            this.btnAirspeedCalib = new MissionPlanner.Controls.MyButton();
            this.btnTakePhoto = new MissionPlanner.Controls.MyButton();
            this.btnAutoMode = new MissionPlanner.Controls.MyButton();
            this.btnRTL = new MissionPlanner.Controls.MyButton();
            this.btnArmDisarm = new MissionPlanner.Controls.MyButton();
            this.cmbWPJump = new System.Windows.Forms.ToolStripComboBox();
            this.btnWPJump = new MissionPlanner.Controls.MyButton();
            this.btnClearTrack = new MissionPlanner.Controls.MyButton();
            this.hostSpeedInput = new System.Windows.Forms.NumericUpDown();
            this.speedHost = new System.Windows.Forms.ToolStripControlHost(this.hostSpeedInput);
            this.btnChangeSpeed = new MissionPlanner.Controls.MyButton();
            this.btnReadWPs = new MissionPlanner.Controls.MyButton();
            this.btnResumeMission = new MissionPlanner.Controls.MyButton();
            this.btnRTKInject = new MissionPlanner.Controls.MyButton();
            this.btnBinToPos = new MissionPlanner.Controls.MyButton();
            this.camtriggDistInput = new System.Windows.Forms.NumericUpDown();
            this.camtriggDistHost = new System.Windows.Forms.ToolStripControlHost(this.camtriggDistInput);
            this.btnSetCamTriggDist = new MissionPlanner.Controls.MyButton();
            this.hostAirspeedCalib = new System.Windows.Forms.ToolStripControlHost(this.btnAirspeedCalib);
            this.hostTakePhoto = new System.Windows.Forms.ToolStripControlHost(this.btnTakePhoto);
            this.hostAutoMode = new System.Windows.Forms.ToolStripControlHost(this.btnAutoMode);
            this.hostRTL = new System.Windows.Forms.ToolStripControlHost(this.btnRTL);
            this.hostArmDisarm = new System.Windows.Forms.ToolStripControlHost(this.btnArmDisarm);
            this.hostWPJump = new System.Windows.Forms.ToolStripControlHost(this.btnWPJump);
            this.hostClearTrack = new System.Windows.Forms.ToolStripControlHost(this.btnClearTrack);
            this.hostChangeSpeed = new System.Windows.Forms.ToolStripControlHost(this.btnChangeSpeed);
            this.hostReadWPs = new System.Windows.Forms.ToolStripControlHost(this.btnReadWPs);
            this.hostResumeMission = new System.Windows.Forms.ToolStripControlHost(this.btnResumeMission);
            this.hostRTKInject = new System.Windows.Forms.ToolStripControlHost(this.btnRTKInject);
            this.hostBinToPos = new System.Windows.Forms.ToolStripControlHost(this.btnBinToPos);
            this.hostSetCamTriggDist = new System.Windows.Forms.ToolStripControlHost(this.btnSetCamTriggDist);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.MenuConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripConnectionControl = new MissionPlanner.Controls.ToolStripConnectionControl();
            this.menu = new MissionPlanner.Controls.MyButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.status1 = new MissionPlanner.Controls.Status();
            this.MainMenu.SuspendLayout();
            this.CTX_mainmenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hostSpeedInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camtriggDistInput)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.ContextMenuStrip = this.CTX_mainmenu;
            this.MainMenu.GripMargin = new System.Windows.Forms.Padding(0);
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(45, 39);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFlightData,
            this.MenuFlightPlanner,
            this.MenuInitConfig,
            this.MenuConfigTune,
            this.MenuSimulation,
            // 1 解锁锁定
            this.hostArmDisarm,
            // 2 校准
            this.hostAirspeedCalib,
            // 3 试拍
            this.hostTakePhoto,
            this.toolStripSeparator1,
            // 4 速度
            this.speedHost,
            this.hostChangeSpeed,
            // 5 距离
            this.camtriggDistHost,
            this.hostSetCamTriggDist,
            // 6 航点跳转
            this.cmbWPJump,
            this.hostWPJump,
            this.toolStripSeparator2,
            // 7 读取
            this.hostReadWPs,
            // 8 自动
            this.hostAutoMode,
            // 9 返航
            this.hostRTL,
            // 10 恢复
            this.hostResumeMission,
            this.toolStripSeparator3,
            // 11 RTK
            this.hostRTKInject,
            // 12 POS
            this.hostBinToPos,
            // 13 清除
            this.hostClearTrack,
            this.MenuConnect,
            this.toolStripConnectionControl});
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.ShowItemToolTips = true;
            this.MainMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MainMenu_ItemClicked);
            this.MainMenu.MouseLeave += new System.EventHandler(this.MainMenu_MouseLeave);
            // 
            // CTX_mainmenu
            // 
            this.CTX_mainmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoHideToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this.readonlyToolStripMenuItem,
            this.connectionOptionsToolStripMenuItem,
            this.connectionListToolStripMenuItem});
            this.CTX_mainmenu.Name = "CTX_mainmenu";
            resources.ApplyResources(this.CTX_mainmenu, "CTX_mainmenu");
            // 
            // autoHideToolStripMenuItem
            // 
            this.autoHideToolStripMenuItem.CheckOnClick = true;
            this.autoHideToolStripMenuItem.Name = "autoHideToolStripMenuItem";
            resources.ApplyResources(this.autoHideToolStripMenuItem, "autoHideToolStripMenuItem");
            this.autoHideToolStripMenuItem.Click += new System.EventHandler(this.autoHideToolStripMenuItem_Click);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.CheckOnClick = true;
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            resources.ApplyResources(this.fullScreenToolStripMenuItem, "fullScreenToolStripMenuItem");
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // readonlyToolStripMenuItem
            // 
            this.readonlyToolStripMenuItem.CheckOnClick = true;
            this.readonlyToolStripMenuItem.Name = "readonlyToolStripMenuItem";
            resources.ApplyResources(this.readonlyToolStripMenuItem, "readonlyToolStripMenuItem");
            this.readonlyToolStripMenuItem.Click += new System.EventHandler(this.readonlyToolStripMenuItem_Click);
            // 
            // connectionOptionsToolStripMenuItem
            // 
            this.connectionOptionsToolStripMenuItem.Name = "connectionOptionsToolStripMenuItem";
            resources.ApplyResources(this.connectionOptionsToolStripMenuItem, "connectionOptionsToolStripMenuItem");
            this.connectionOptionsToolStripMenuItem.Click += new System.EventHandler(this.connectionOptionsToolStripMenuItem_Click);
            // 
            // connectionListToolStripMenuItem
            // 
            this.connectionListToolStripMenuItem.Name = "connectionListToolStripMenuItem";
            resources.ApplyResources(this.connectionListToolStripMenuItem, "connectionListToolStripMenuItem");
            this.connectionListToolStripMenuItem.Click += new System.EventHandler(this.connectionListToolStripMenuItem_Click);
            // 
            // MenuFlightData
            // 
            this.MenuFlightData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.MenuFlightData.Image = global::MissionPlanner.Properties.Resources.light_flightdata_icon;
            resources.ApplyResources(this.MenuFlightData, "MenuFlightData");
            this.MenuFlightData.Margin = new System.Windows.Forms.Padding(0);
            this.MenuFlightData.Name = "MenuFlightData";
            this.MenuFlightData.Click += new System.EventHandler(this.MenuFlightData_Click);
            // 
            // MenuFlightPlanner
            // 
            this.MenuFlightPlanner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.MenuFlightPlanner.Image = global::MissionPlanner.Properties.Resources.light_flightplan_icon;
            resources.ApplyResources(this.MenuFlightPlanner, "MenuFlightPlanner");
            this.MenuFlightPlanner.Margin = new System.Windows.Forms.Padding(0);
            this.MenuFlightPlanner.Name = "MenuFlightPlanner";
            this.MenuFlightPlanner.Click += new System.EventHandler(this.MenuFlightPlanner_Click);
            // 
            // MenuInitConfig
            // 
            this.MenuInitConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.MenuInitConfig.Image = global::MissionPlanner.Properties.Resources.light_initialsetup_icon;
            resources.ApplyResources(this.MenuInitConfig, "MenuInitConfig");
            this.MenuInitConfig.Margin = new System.Windows.Forms.Padding(0);
            this.MenuInitConfig.Name = "MenuInitConfig";
            this.MenuInitConfig.Click += new System.EventHandler(this.MenuSetup_Click);
            // 
            // MenuConfigTune
            // 
            this.MenuConfigTune.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.MenuConfigTune.Image = global::MissionPlanner.Properties.Resources.light_tuningconfig_icon;
            resources.ApplyResources(this.MenuConfigTune, "MenuConfigTune");
            this.MenuConfigTune.Margin = new System.Windows.Forms.Padding(0);
            this.MenuConfigTune.Name = "MenuConfigTune";
            this.MenuConfigTune.Click += new System.EventHandler(this.MenuTuning_Click);
            // 
            // MenuSimulation
            // 
            this.MenuSimulation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.MenuSimulation.Image = global::MissionPlanner.Properties.Resources.light_simulation_icon;
            resources.ApplyResources(this.MenuSimulation, "MenuSimulation");
            this.MenuSimulation.Margin = new System.Windows.Forms.Padding(0);
            this.MenuSimulation.Name = "MenuSimulation";
            this.MenuSimulation.Click += new System.EventHandler(this.MenuSimulation_Click);
            // 
            // btnAirspeedCalib
            // 
            this.btnAirspeedCalib.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnAirspeedCalib.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnAirspeedCalib.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnAirspeedCalib, "btnAirspeedCalib");
            this.btnAirspeedCalib.Name = "btnAirspeedCalib";
            this.btnAirspeedCalib.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnAirspeedCalib.UseVisualStyleBackColor = true;
            this.btnAirspeedCalib.Click += new System.EventHandler(this.btnAirspeedCalib_Click);
            this.toolTip1.SetToolTip(this.btnAirspeedCalib, resources.GetString("btnAirspeedCalib.ToolTipText"));
            // 
            // btnTakePhoto
            // 
            this.btnTakePhoto.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnTakePhoto.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnTakePhoto.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnTakePhoto, "btnTakePhoto");
            this.btnTakePhoto.Name = "btnTakePhoto";
            this.btnTakePhoto.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnTakePhoto.UseVisualStyleBackColor = true;
            this.btnTakePhoto.Click += new System.EventHandler(this.btnTakePhoto_Click);
            this.toolTip1.SetToolTip(this.btnTakePhoto, resources.GetString("btnTakePhoto.ToolTipText"));
            // 
            // btnAutoMode
            // 
            this.btnAutoMode.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnAutoMode.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnAutoMode.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnAutoMode, "btnAutoMode");
            this.btnAutoMode.Name = "btnAutoMode";
            this.btnAutoMode.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnAutoMode.UseVisualStyleBackColor = true;
            this.btnAutoMode.Click += new System.EventHandler(this.btnAutoMode_Click);
            this.toolTip1.SetToolTip(this.btnAutoMode, resources.GetString("btnAutoMode.ToolTipText"));
            // 
            // btnRTL
            // 
            this.btnRTL.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnRTL.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnRTL.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnRTL, "btnRTL");
            this.btnRTL.Name = "btnRTL";
            this.btnRTL.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnRTL.UseVisualStyleBackColor = true;
            this.btnRTL.Click += new System.EventHandler(this.btnRTL_Click);
            this.toolTip1.SetToolTip(this.btnRTL, resources.GetString("btnRTL.ToolTipText"));
            // 
            // btnArmDisarm
            // 
            this.btnArmDisarm.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnArmDisarm.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnArmDisarm.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnArmDisarm, "btnArmDisarm");
            this.btnArmDisarm.Name = "btnArmDisarm";
            this.btnArmDisarm.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnArmDisarm.UseVisualStyleBackColor = true;
            this.btnArmDisarm.Click += new System.EventHandler(this.btnArmDisarm_Click);
            this.toolTip1.SetToolTip(this.btnArmDisarm, resources.GetString("btnArmDisarm.ToolTipText"));
            // 
            // cmbWPJump
            // 
            this.cmbWPJump.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWPJump.DropDownWidth = 80;
            this.cmbWPJump.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbWPJump.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.cmbWPJump.Name = "cmbWPJump";
            resources.ApplyResources(this.cmbWPJump, "cmbWPJump");
            this.cmbWPJump.DropDown += new System.EventHandler(this.cmbWPJump_DropDown);
            // 
            // btnWPJump
            // 
            this.btnWPJump.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnWPJump.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnWPJump.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnWPJump, "btnWPJump");
            this.btnWPJump.Name = "btnWPJump";
            this.btnWPJump.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnWPJump.UseVisualStyleBackColor = true;
            this.btnWPJump.Click += new System.EventHandler(this.btnWPJump_Click);
            this.toolTip1.SetToolTip(this.btnWPJump, resources.GetString("btnWPJump.ToolTipText"));
            // 
            // btnClearTrack
            // 
            this.btnClearTrack.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnClearTrack.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnClearTrack.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnClearTrack, "btnClearTrack");
            this.btnClearTrack.Name = "btnClearTrack";
            this.btnClearTrack.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnClearTrack.UseVisualStyleBackColor = true;
            this.btnClearTrack.Click += new System.EventHandler(this.btnClearTrack_Click);
            this.toolTip1.SetToolTip(this.btnClearTrack, resources.GetString("btnClearTrack.ToolTipText"));
            // 
            // speedHost
            // 
            this.speedHost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            // 
            // btnChangeSpeed
            // 
            this.btnChangeSpeed.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnChangeSpeed.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnChangeSpeed.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnChangeSpeed, "btnChangeSpeed");
            this.btnChangeSpeed.Name = "btnChangeSpeed";
            this.btnChangeSpeed.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnChangeSpeed.UseVisualStyleBackColor = true;
            this.btnChangeSpeed.Click += new System.EventHandler(this.btnChangeSpeed_Click);
            this.toolTip1.SetToolTip(this.btnChangeSpeed, resources.GetString("btnChangeSpeed.ToolTipText"));
            // 
            // btnReadWPs
            // 
            this.btnReadWPs.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnReadWPs.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnReadWPs.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnReadWPs, "btnReadWPs");
            this.btnReadWPs.Name = "btnReadWPs";
            this.btnReadWPs.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnReadWPs.UseVisualStyleBackColor = true;
            this.btnReadWPs.Click += new System.EventHandler(this.btnReadWPs_Click);
            this.toolTip1.SetToolTip(this.btnReadWPs, resources.GetString("btnReadWPs.ToolTipText"));
            // 
            // btnResumeMission
            // 
            this.btnResumeMission.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnResumeMission.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnResumeMission.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnResumeMission, "btnResumeMission");
            this.btnResumeMission.Name = "btnResumeMission";
            this.btnResumeMission.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnResumeMission.UseVisualStyleBackColor = true;
            this.btnResumeMission.Click += new System.EventHandler(this.btnResumeMission_Click);
            this.toolTip1.SetToolTip(this.btnResumeMission, resources.GetString("btnResumeMission.ToolTipText"));
            // 
            // btnRTKInject
            // 
            this.btnRTKInject.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnRTKInject.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnRTKInject.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnRTKInject, "btnRTKInject");
            this.btnRTKInject.Name = "btnRTKInject";
            this.btnRTKInject.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnRTKInject.UseVisualStyleBackColor = true;
            this.btnRTKInject.Click += new System.EventHandler(this.btnRTKInject_Click);
            this.toolTip1.SetToolTip(this.btnRTKInject, resources.GetString("btnRTKInject.ToolTipText"));
            // 
            // btnBinToPos
            // 
            this.btnBinToPos.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnBinToPos.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnBinToPos.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnBinToPos, "btnBinToPos");
            this.btnBinToPos.Name = "btnBinToPos";
            this.btnBinToPos.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnBinToPos.UseVisualStyleBackColor = true;
            this.btnBinToPos.Click += new System.EventHandler(this.btnBinToPos_Click);
            this.toolTip1.SetToolTip(this.btnBinToPos, resources.GetString("btnBinToPos.ToolTipText"));
            // 
            // camtriggDistHost
            // 
            this.camtriggDistHost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            // 
            // btnSetCamTriggDist
            // 
            this.btnSetCamTriggDist.ColorMouseDown = System.Drawing.Color.Empty;
            this.btnSetCamTriggDist.ColorMouseOver = System.Drawing.Color.Empty;
            this.btnSetCamTriggDist.ColorNotEnabled = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnSetCamTriggDist, "btnSetCamTriggDist");
            this.btnSetCamTriggDist.Name = "btnSetCamTriggDist";
            this.btnSetCamTriggDist.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.btnSetCamTriggDist.UseVisualStyleBackColor = true;
            this.btnSetCamTriggDist.Click += new System.EventHandler(this.btnSetCamTriggDist_Click);
            this.toolTip1.SetToolTip(this.btnSetCamTriggDist, resources.GetString("btnSetCamTriggDist.ToolTipText"));
            // 
            // MenuConnect
            // 
            this.MenuConnect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MenuConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.MenuConnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            resources.ApplyResources(this.MenuConnect, "MenuConnect");
            this.MenuConnect.Margin = new System.Windows.Forms.Padding(0);
            this.MenuConnect.Name = "MenuConnect";
            this.MenuConnect.Click += new System.EventHandler(this.MenuConnect_Click);
            // 
            // toolStripConnectionControl
            // 
            this.toolStripConnectionControl.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.toolStripConnectionControl, "toolStripConnectionControl");
            this.toolStripConnectionControl.ForeColor = System.Drawing.Color.Black;
            this.toolStripConnectionControl.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripConnectionControl.Name = "toolStripConnectionControl";
            this.toolStripConnectionControl.MouseLeave += new System.EventHandler(this.MainMenu_MouseLeave);
            // 
            // menu
            // 
            resources.ApplyResources(this.menu, "menu");
            this.menu.Name = "menu";
            this.menu.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(193)))), ((int)(((byte)(31)))));
            this.menu.UseVisualStyleBackColor = true;
            this.menu.MouseEnter += new System.EventHandler(this.menu_MouseEnter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.status1);
            this.panel1.Controls.Add(this.MainMenu);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.MouseLeave += new System.EventHandler(this.MainMenu_MouseLeave);
            // 
            // status1
            // 
            resources.ApplyResources(this.status1, "status1");
            this.status1.Name = "status1";
            this.status1.Percent = 0D;
            // 
            // MainV2
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "MainV2";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainV2_KeyDown);
            this.Resize += new System.EventHandler(this.MainV2_Resize);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.CTX_mainmenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hostSpeedInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camtriggDistInput)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ToolStripButton MenuFlightData;
        public System.Windows.Forms.ToolStripButton MenuFlightPlanner;
        public System.Windows.Forms.ToolStripButton MenuInitConfig;
        public System.Windows.Forms.ToolStripButton MenuSimulation;
        public System.Windows.Forms.ToolStripButton MenuConfigTune;
        public System.Windows.Forms.ToolStripButton MenuConnect;
        public Controls.MyButton btnAirspeedCalib;
        public Controls.MyButton btnTakePhoto;
        public Controls.MyButton btnAutoMode;
        public Controls.MyButton btnRTL;
        public Controls.MyButton btnArmDisarm;
        public System.Windows.Forms.ToolStripComboBox cmbWPJump;
        public Controls.MyButton btnWPJump;
        public Controls.MyButton btnClearTrack;
        public Controls.MyButton btnChangeSpeed;
        public Controls.MyButton btnReadWPs;
        public Controls.MyButton btnResumeMission;
        public Controls.MyButton btnRTKInject;
        public Controls.MyButton btnBinToPos;
        private Controls.ToolStripConnectionControl toolStripConnectionControl;
        private Controls.MyButton menu;
        private System.Windows.Forms.ToolStripControlHost hostAirspeedCalib;
        private System.Windows.Forms.ToolStripControlHost hostTakePhoto;
        private System.Windows.Forms.ToolStripControlHost hostAutoMode;
        private System.Windows.Forms.ToolStripControlHost hostRTL;
        private System.Windows.Forms.ToolStripControlHost hostArmDisarm;
        private System.Windows.Forms.ToolStripControlHost hostWPJump;
        private System.Windows.Forms.ToolStripControlHost hostClearTrack;
        private System.Windows.Forms.ToolStripControlHost hostChangeSpeed;
        private System.Windows.Forms.ToolStripControlHost hostReadWPs;
        private System.Windows.Forms.ToolStripControlHost hostResumeMission;
        private System.Windows.Forms.ToolStripControlHost hostRTKInject;
        private System.Windows.Forms.ToolStripControlHost hostBinToPos;
        private System.Windows.Forms.ToolStripControlHost hostSetCamTriggDist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip CTX_mainmenu;
        private System.Windows.Forms.ToolStripMenuItem autoHideToolStripMenuItem;
        public System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readonlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionListToolStripMenuItem;
        public Controls.Status status1;
        private System.Windows.Forms.NumericUpDown hostSpeedInput;
        private System.Windows.Forms.ToolStripControlHost speedHost;
        private System.Windows.Forms.NumericUpDown camtriggDistInput;
        private System.Windows.Forms.ToolStripControlHost camtriggDistHost;
        public Controls.MyButton btnSetCamTriggDist;
    }
}