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
            this.btnAirspeedCalib = new System.Windows.Forms.ToolStripButton();
            this.btnTakePhoto = new System.Windows.Forms.ToolStripButton();
            this.btnAutoMode = new System.Windows.Forms.ToolStripButton();
            this.btnRTL = new System.Windows.Forms.ToolStripButton();
            this.btnArmDisarm = new System.Windows.Forms.ToolStripButton();
            this.cmbWPJump = new System.Windows.Forms.ToolStripComboBox();
            this.btnWPJump = new System.Windows.Forms.ToolStripButton();
            this.btnClearTrack = new System.Windows.Forms.ToolStripButton();
            this.btnChangeSpeed = new System.Windows.Forms.ToolStripButton();
            this.btnReadWPs = new System.Windows.Forms.ToolStripButton();
            this.btnResumeMission = new System.Windows.Forms.ToolStripButton();
            this.btnRTKInject = new System.Windows.Forms.ToolStripButton();
            this.btnBinToPos = new System.Windows.Forms.ToolStripButton();
            this.MenuConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripConnectionControl = new MissionPlanner.Controls.ToolStripConnectionControl();
            this.hostSpeedInput = new System.Windows.Forms.NumericUpDown();
            this.menu = new MissionPlanner.Controls.MyButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.status1 = new MissionPlanner.Controls.Status();
            this.MainMenu.SuspendLayout();
            this.CTX_mainmenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hostSpeedInput)).BeginInit();
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
            this.btnAirspeedCalib,
            this.btnTakePhoto,
            this.btnAutoMode,
            this.btnRTL,
            this.btnArmDisarm,
            this.cmbWPJump,
            this.btnWPJump,
            this.btnClearTrack,
            this.btnChangeSpeed,
            this.btnReadWPs,
            this.btnResumeMission,
            this.btnRTKInject,
            this.btnBinToPos,
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
            this.MenuFlightData.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.MenuFlightData, "MenuFlightData");
            this.MenuFlightData.Margin = new System.Windows.Forms.Padding(0);
            this.MenuFlightData.Name = "MenuFlightData";
            this.MenuFlightData.Click += new System.EventHandler(this.MenuFlightData_Click);
            // 
            // MenuFlightPlanner
            // 
            this.MenuFlightPlanner.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.MenuFlightPlanner, "MenuFlightPlanner");
            this.MenuFlightPlanner.Margin = new System.Windows.Forms.Padding(0);
            this.MenuFlightPlanner.Name = "MenuFlightPlanner";
            this.MenuFlightPlanner.Click += new System.EventHandler(this.MenuFlightPlanner_Click);
            // 
            // MenuInitConfig
            // 
            this.MenuInitConfig.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.MenuInitConfig, "MenuInitConfig");
            this.MenuInitConfig.Margin = new System.Windows.Forms.Padding(0);
            this.MenuInitConfig.Name = "MenuInitConfig";
            this.MenuInitConfig.Click += new System.EventHandler(this.MenuSetup_Click);
            // 
            // MenuConfigTune
            // 
            this.MenuConfigTune.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.MenuConfigTune, "MenuConfigTune");
            this.MenuConfigTune.Margin = new System.Windows.Forms.Padding(0);
            this.MenuConfigTune.Name = "MenuConfigTune";
            this.MenuConfigTune.Click += new System.EventHandler(this.MenuTuning_Click);
            // 
            // MenuSimulation
            // 
            this.MenuSimulation.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.MenuSimulation, "MenuSimulation");
            this.MenuSimulation.Margin = new System.Windows.Forms.Padding(0);
            this.MenuSimulation.Name = "MenuSimulation";
            this.MenuSimulation.Click += new System.EventHandler(this.MenuSimulation_Click);
            // 
            // btnAirspeedCalib
            // 
            this.btnAirspeedCalib.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.btnAirspeedCalib, "btnAirspeedCalib");
            this.btnAirspeedCalib.Margin = new System.Windows.Forms.Padding(0);
            this.btnAirspeedCalib.Name = "btnAirspeedCalib";
            this.btnAirspeedCalib.Click += new System.EventHandler(this.btnAirspeedCalib_Click);
            // 
            // btnTakePhoto
            // 
            this.btnTakePhoto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTakePhoto.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnTakePhoto.Margin = new System.Windows.Forms.Padding(0);
            this.btnTakePhoto.Name = "btnTakePhoto";
            resources.ApplyResources(this.btnTakePhoto, "btnTakePhoto");
            this.btnTakePhoto.Click += new System.EventHandler(this.btnTakePhoto_Click);
            // 
            // btnAutoMode
            // 
            this.btnAutoMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAutoMode.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnAutoMode.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoMode.Name = "btnAutoMode";
            resources.ApplyResources(this.btnAutoMode, "btnAutoMode");
            this.btnAutoMode.Click += new System.EventHandler(this.btnAutoMode_Click);
            // 
            // btnRTL
            // 
            this.btnRTL.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.btnRTL, "btnRTL");
            this.btnRTL.Margin = new System.Windows.Forms.Padding(0);
            this.btnRTL.Name = "btnRTL";
            this.btnRTL.Click += new System.EventHandler(this.btnRTL_Click);
            // 
            // btnArmDisarm
            // 
            this.btnArmDisarm.ForeColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.btnArmDisarm, "btnArmDisarm");
            this.btnArmDisarm.Margin = new System.Windows.Forms.Padding(0);
            this.btnArmDisarm.Name = "btnArmDisarm";
            this.btnArmDisarm.Click += new System.EventHandler(this.btnArmDisarm_Click);
            // 
            // cmbWPJump
            // 
            this.cmbWPJump.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWPJump.DropDownWidth = 100;
            this.cmbWPJump.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbWPJump.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.cmbWPJump.Name = "cmbWPJump";
            resources.ApplyResources(this.cmbWPJump, "cmbWPJump");
            this.cmbWPJump.DropDown += new System.EventHandler(this.cmbWPJump_DropDown);
            // 
            // btnWPJump
            // 
            this.btnWPJump.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnWPJump.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnWPJump.Margin = new System.Windows.Forms.Padding(0);
            this.btnWPJump.Name = "btnWPJump";
            resources.ApplyResources(this.btnWPJump, "btnWPJump");
            this.btnWPJump.Click += new System.EventHandler(this.btnWPJump_Click);
            // 
            // btnClearTrack
            // 
            this.btnClearTrack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClearTrack.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnClearTrack.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearTrack.Name = "btnClearTrack";
            resources.ApplyResources(this.btnClearTrack, "btnClearTrack");
            this.btnClearTrack.Click += new System.EventHandler(this.btnClearTrack_Click);
            // 
            // btnChangeSpeed
            // 
            this.btnChangeSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnChangeSpeed.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnChangeSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeSpeed.Name = "btnChangeSpeed";
            resources.ApplyResources(this.btnChangeSpeed, "btnChangeSpeed");
            this.btnChangeSpeed.Click += new System.EventHandler(this.btnChangeSpeed_Click);
            // 
            // btnReadWPs
            // 
            this.btnReadWPs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReadWPs.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnReadWPs.Margin = new System.Windows.Forms.Padding(0);
            this.btnReadWPs.Name = "btnReadWPs";
            resources.ApplyResources(this.btnReadWPs, "btnReadWPs");
            this.btnReadWPs.Click += new System.EventHandler(this.btnReadWPs_Click);
            // 
            // btnResumeMission
            // 
            this.btnResumeMission.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResumeMission.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnResumeMission.Margin = new System.Windows.Forms.Padding(0);
            this.btnResumeMission.Name = "btnResumeMission";
            resources.ApplyResources(this.btnResumeMission, "btnResumeMission");
            this.btnResumeMission.Click += new System.EventHandler(this.btnResumeMission_Click);
            // 
            // btnRTKInject
            // 
            this.btnRTKInject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRTKInject.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnRTKInject.Margin = new System.Windows.Forms.Padding(0);
            this.btnRTKInject.Name = "btnRTKInject";
            resources.ApplyResources(this.btnRTKInject, "btnRTKInject");
            this.btnRTKInject.Click += new System.EventHandler(this.btnRTKInject_Click);
            // 
            // btnBinToPos
            // 
            this.btnBinToPos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBinToPos.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnBinToPos.Margin = new System.Windows.Forms.Padding(0);
            this.btnBinToPos.Name = "btnBinToPos";
            resources.ApplyResources(this.btnBinToPos, "btnBinToPos");
            this.btnBinToPos.Click += new System.EventHandler(this.btnBinToPos_Click);
            // 
            // MenuConnect
            // 
            this.MenuConnect.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.MenuConnect.ForeColor = System.Drawing.SystemColors.ControlLight;
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
            // hostSpeedInput
            // 
            resources.ApplyResources(this.hostSpeedInput, "hostSpeedInput");
            this.hostSpeedInput.Name = "hostSpeedInput";
            // 
            // menu
            // 
            resources.ApplyResources(this.menu, "menu");
            this.menu.Name = "menu";
            this.menu.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
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
        public System.Windows.Forms.ToolStripButton btnAirspeedCalib;
        public System.Windows.Forms.ToolStripButton btnTakePhoto;
        public System.Windows.Forms.ToolStripButton btnAutoMode;
        public System.Windows.Forms.ToolStripButton btnRTL;
        public System.Windows.Forms.ToolStripButton btnArmDisarm;
        public System.Windows.Forms.ToolStripComboBox cmbWPJump;
        public System.Windows.Forms.ToolStripButton btnWPJump;
        public System.Windows.Forms.ToolStripButton btnClearTrack;
        public System.Windows.Forms.NumericUpDown numericUpDownSpeed;
        public System.Windows.Forms.ToolStripButton btnChangeSpeed;
        public System.Windows.Forms.ToolStripButton btnReadWPs;
        public System.Windows.Forms.ToolStripButton btnResumeMission;
        public System.Windows.Forms.ToolStripButton btnRTKInject;
        public System.Windows.Forms.ToolStripButton btnBinToPos;
        private Controls.ToolStripConnectionControl toolStripConnectionControl;
        private Controls.MyButton menu;
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
    }
}