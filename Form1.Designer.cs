using System;

namespace RadarConnect
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btn_UseZoom = new System.Windows.Forms.Button();
            this.btn_EnableOsd = new System.Windows.Forms.Button();
            this.btn_RemoveOsd = new System.Windows.Forms.Button();
            this.btn_Snapshot = new System.Windows.Forms.Button();
            this.btn_ZoomOut = new System.Windows.Forms.Button();
            this.btn_ZoomIn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SetCoordinate = new System.Windows.Forms.Button();
            this.cbx_Coordinate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_StopSample = new System.Windows.Forms.Button();
            this.btn_StartSample = new System.Windows.Forms.Button();
            this.btn_SetMode = new System.Windows.Forms.Button();
            this.cbx_WorkMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SetScanPattern = new System.Windows.Forms.Button();
            this.cbx_ScanPattern = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Disconnect = new System.Windows.Forms.Button();
            this.btn_HandShake = new System.Windows.Forms.Button();
            this.btn_StopListen = new System.Windows.Forms.Button();
            this.btn_StartListen = new System.Windows.Forms.Button();
            this.listView_Devices = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel_Video = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_CameraIp = new System.Windows.Forms.TextBox();
            this.btn_PlayCamera = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_ExportPcd = new System.Windows.Forms.Button();
            this.btn_SaveBEV = new System.Windows.Forms.Button();
            this.btn_ShowRaw = new System.Windows.Forms.Button();
            this.btn_SaveImage = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker_Query = new System.Windows.Forms.DateTimePicker();
            this.btn_Reconstruct = new System.Windows.Forms.Button();
            this.groupBox_DepthCompletion = new System.Windows.Forms.GroupBox();
            this.btn_CompleteDepth = new System.Windows.Forms.Button();
            this.tableLayoutPanelSplit = new System.Windows.Forms.TableLayoutPanel();
            this.renderWindowControl1 = new Kitware.VTK.RenderWindowControl();
            this.renderWindowControl2 = new Kitware.VTK.RenderWindowControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox_FusionResult = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_ExecuteFusion = new System.Windows.Forms.Button();
            this.btn_SelectVideo = new System.Windows.Forms.Button();
            this.txt_VideoPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_Fusion = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_LoadPcd = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.gbxPtzNet = new System.Windows.Forms.GroupBox();
            this.lblLIn = new System.Windows.Forms.Label();
            this.txt_PtzLocalIp = new System.Windows.Forms.TextBox();
            this.nud_PtzLocalPort = new System.Windows.Forms.NumericUpDown();
            this.lblRIn = new System.Windows.Forms.Label();
            this.txt_PtzIp = new System.Windows.Forms.TextBox();
            this.nud_PtzPort = new System.Windows.Forms.NumericUpDown();
            this.lblAddr = new System.Windows.Forms.Label();
            this.nud_PtzAddress = new System.Windows.Forms.NumericUpDown();
            this.btnPtzOpen = new System.Windows.Forms.Button();
            this.btnPtzClose = new System.Windows.Forms.Button();
            this.gbxPtzQuery = new System.Windows.Forms.GroupBox();
            this.btnPtzQueryStatus = new System.Windows.Forms.Button();
            this.btnPtzQueryMode = new System.Windows.Forms.Button();
            this.lblRti = new System.Windows.Forms.Label();
            this.nud_PtzRealtimeInterval = new System.Windows.Forms.NumericUpDown();
            this.btnPtzRealtimeAngleOn = new System.Windows.Forms.Button();
            this.btnPtzRealtimeAngleOff = new System.Windows.Forms.Button();
            this.lbl_PtzHAngle = new System.Windows.Forms.Label();
            this.lbl_PtzVAngle = new System.Windows.Forms.Label();
            this.lbl_PtzStatus = new System.Windows.Forms.Label();
            this.gbxPtzPreset = new System.Windows.Forms.GroupBox();
            this.lblPno = new System.Windows.Forms.Label();
            this.nud_PtzPreset = new System.Windows.Forms.NumericUpDown();
            this.btnPtzPresetSet = new System.Windows.Forms.Button();
            this.btnPtzPresetCall = new System.Windows.Forms.Button();
            this.btnPtzPresetDel = new System.Windows.Forms.Button();
            this.lblPs = new System.Windows.Forms.Label();
            this.nud_PtzPresetStart = new System.Windows.Forms.NumericUpDown();
            this.lblPe = new System.Windows.Forms.Label();
            this.nud_PtzPresetEnd = new System.Windows.Forms.NumericUpDown();
            this.lblPt = new System.Windows.Forms.Label();
            this.nud_PtzPresetTime = new System.Windows.Forms.NumericUpDown();
            this.btnPtzPresetScanStart = new System.Windows.Forms.Button();
            this.btnPtzPresetScanStop = new System.Windows.Forms.Button();
            this.gbxPtzRaw = new System.Windows.Forms.GroupBox();
            this.txt_PtzRawHex = new System.Windows.Forms.TextBox();
            this.btnPtzSendRaw = new System.Windows.Forms.Button();
            this.gbxPtzDirect = new System.Windows.Forms.GroupBox();
            this.lblHS = new System.Windows.Forms.Label();
            this.nud_PtzHSpeed = new System.Windows.Forms.NumericUpDown();
            this.lblVS = new System.Windows.Forms.Label();
            this.nud_PtzVSpeed = new System.Windows.Forms.NumericUpDown();
            this.btnPtzSupDirLeftUp = new System.Windows.Forms.Button();
            this.btnPtzSupDirRightUp = new System.Windows.Forms.Button();
            this.btnPtzSupDirLeftDown = new System.Windows.Forms.Button();
            this.btnPtzSupDirRightDown = new System.Windows.Forms.Button();
            this.btnPtzUp = new System.Windows.Forms.Button();
            this.btnPtzLeft = new System.Windows.Forms.Button();
            this.btnPtzStop = new System.Windows.Forms.Button();
            this.btnPtzRight = new System.Windows.Forms.Button();
            this.btnPtzDown = new System.Windows.Forms.Button();
            this.gbxPtzArea = new System.Windows.Forms.GroupBox();
            this.lblAn = new System.Windows.Forms.Label();
            this.nud_PtzArea = new System.Windows.Forms.NumericUpDown();
            this.lblAhs = new System.Windows.Forms.Label();
            this.nud_PtzAreaHStart = new System.Windows.Forms.NumericUpDown();
            this.lblAhe = new System.Windows.Forms.Label();
            this.nud_PtzAreaHEnd = new System.Windows.Forms.NumericUpDown();
            this.lblAhi = new System.Windows.Forms.Label();
            this.nud_PtzAreaHInterval = new System.Windows.Forms.NumericUpDown();
            this.lblAvs = new System.Windows.Forms.Label();
            this.nud_PtzAreaVStart = new System.Windows.Forms.NumericUpDown();
            this.lblAve = new System.Windows.Forms.Label();
            this.nud_PtzAreaVEnd = new System.Windows.Forms.NumericUpDown();
            this.lblAvi = new System.Windows.Forms.Label();
            this.nud_PtzAreaVInterval = new System.Windows.Forms.NumericUpDown();
            this.lblAtm = new System.Windows.Forms.Label();
            this.nud_PtzAreaTime = new System.Windows.Forms.NumericUpDown();
            this.btnPtzAreaSetBound = new System.Windows.Forms.Button();
            this.btnPtzAreaSetInterval = new System.Windows.Forms.Button();
            this.lblAs = new System.Windows.Forms.Label();
            this.nud_PtzAreaStart = new System.Windows.Forms.NumericUpDown();
            this.lblAe = new System.Windows.Forms.Label();
            this.nud_PtzAreaEnd = new System.Windows.Forms.NumericUpDown();
            this.btnPtzAreaScanStart = new System.Windows.Forms.Button();
            this.btnPtzAreaScanStop = new System.Windows.Forms.Button();
            this.gbxPtzLocate = new System.Windows.Forms.GroupBox();
            this.lblHA = new System.Windows.Forms.Label();
            this.nud_PtzHAngle = new System.Windows.Forms.NumericUpDown();
            this.lblVA = new System.Windows.Forms.Label();
            this.nud_PtzVAngle = new System.Windows.Forms.NumericUpDown();
            this.chk_PtzUseSpeedLocate = new System.Windows.Forms.CheckBox();
            this.btnPtzLocate = new System.Windows.Forms.Button();
            this.gbxPtzExtPreset = new System.Windows.Forms.GroupBox();
            this.flpPtzSupplementPreset = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPtzSupPresetSetByAngle = new System.Windows.Forms.Button();
            this.btnPtzSupPresetSetHAngle = new System.Windows.Forms.Button();
            this.btnPtzSupPresetSetVAngle = new System.Windows.Forms.Button();
            this.btnPtzSupPresetSetTime = new System.Windows.Forms.Button();
            this.btnPtzSupPresetSetSpeed = new System.Windows.Forms.Button();
            this.btnPtzSupPresetPause = new System.Windows.Forms.Button();
            this.btnPtzSupPresetContinue = new System.Windows.Forms.Button();
            this.btnPtzSupPresetEndReturnOn = new System.Windows.Forms.Button();
            this.btnPtzSupPresetEndReturnOff = new System.Windows.Forms.Button();
            this.btnPtzSupPresetArriveReturnOn = new System.Windows.Forms.Button();
            this.btnPtzSupPresetArriveReturnOff = new System.Windows.Forms.Button();
            this.btnPtzSupPresetCallReturnOn = new System.Windows.Forms.Button();
            this.btnPtzSupPresetCallReturnOff = new System.Windows.Forms.Button();
            this.gbxPtzExtArea = new System.Windows.Forms.GroupBox();
            this.flpPtzSupplementArea = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPtzSupAreaVideoHa = new System.Windows.Forms.Button();
            this.btnPtzSupAreaVideoHb = new System.Windows.Forms.Button();
            this.btnPtzSupAreaVideoVa = new System.Windows.Forms.Button();
            this.btnPtzSupAreaVideoVb = new System.Windows.Forms.Button();
            this.btnPtzSupAreaSetSpeed = new System.Windows.Forms.Button();
            this.btnPtzSupAreaSetTime = new System.Windows.Forms.Button();
            this.btnPtzSupAreaEnable = new System.Windows.Forms.Button();
            this.btnPtzSupAreaDisable = new System.Windows.Forms.Button();
            this.btnPtzSupAreaStartSingle = new System.Windows.Forms.Button();
            this.btnPtzSupAreaPause = new System.Windows.Forms.Button();
            this.btnPtzSupAreaContinue = new System.Windows.Forms.Button();
            this.btnPtzSupAreaModeStep = new System.Windows.Forms.Button();
            this.btnPtzSupAreaModeContinuous = new System.Windows.Forms.Button();
            this.btnPtzSupAreaSave = new System.Windows.Forms.Button();
            this.btnPtzSupAreaQuery = new System.Windows.Forms.Button();
            this.btnPtzSupAreaEndReturnOn = new System.Windows.Forms.Button();
            this.btnPtzSupAreaEndReturnOff = new System.Windows.Forms.Button();
            this.btnPtzSupAreaStepReturnOn = new System.Windows.Forms.Button();
            this.btnPtzSupAreaStepReturnOff = new System.Windows.Forms.Button();
            this.gbxPtzExtMaintenance = new System.Windows.Forms.GroupBox();
            this.flpPtzSupplementZero = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPtzSupZeroHCurrent = new System.Windows.Forms.Button();
            this.btnPtzSupZeroVCurrent = new System.Windows.Forms.Button();
            this.btnPtzSupZeroHvCurrent = new System.Windows.Forms.Button();
            this.btnPtzSupZeroHAngle = new System.Windows.Forms.Button();
            this.btnPtzSupZeroVAngle = new System.Windows.Forms.Button();
            this.btnPtzSupZeroDelete = new System.Windows.Forms.Button();
            this.btnPtzSupReboot = new System.Windows.Forms.Button();
            this.btnPtzSupSelfCheck = new System.Windows.Forms.Button();
            this.gbxPtzExtBasic = new System.Windows.Forms.GroupBox();
            this.flpPtzSupplementBasic = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPtzSupLocateH = new System.Windows.Forms.Button();
            this.btnPtzSupLocateV = new System.Windows.Forms.Button();
            this.btnPtzSupQueryHAngle = new System.Windows.Forms.Button();
            this.btnPtzSupQueryVAngle = new System.Windows.Forms.Button();
            this.btnPtzSupPower1On = new System.Windows.Forms.Button();
            this.btnPtzSupPower2On = new System.Windows.Forms.Button();
            this.btnPtzSupPower1Off = new System.Windows.Forms.Button();
            this.btnPtzSupPower2Off = new System.Windows.Forms.Button();
            this.btnPtzSupReturnZero = new System.Windows.Forms.Button();
            this.gbxPtzExtQuery = new System.Windows.Forms.GroupBox();
            this.flpPtzSupplementQuery = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPtzSupAckOn = new System.Windows.Forms.Button();
            this.btnPtzSupAckOff = new System.Windows.Forms.Button();
            this.btnPtzSupQueryTemperature = new System.Windows.Forms.Button();
            this.btnPtzSupQueryVoltage = new System.Windows.Forms.Button();
            this.btnPtzSupQueryCurrent = new System.Windows.Forms.Button();
            this.btnPtzSupQueryHSpeed = new System.Windows.Forms.Button();
            this.btnPtzSupQueryVSpeed = new System.Windows.Forms.Button();
            this.btnPtzSupQueryAllSpeed = new System.Windows.Forms.Button();
            this.btnPtzSupSpeedRealtimeOn = new System.Windows.Forms.Button();
            this.btnPtzSupSpeedRealtimeOff = new System.Windows.Forms.Button();
            this.btnPtzSupLocateReturnOn = new System.Windows.Forms.Button();
            this.btnPtzSupLocateReturnOff = new System.Windows.Forms.Button();
            this.tabPtzNavigation = new System.Windows.Forms.TabControl();
            this.tabPtzHome = new System.Windows.Forms.TabPage();
            this.tabPtzPresetPage = new System.Windows.Forms.TabPage();
            this.tabPtzConfigPage = new System.Windows.Forms.TabPage();
            this.tabPtzAreaPage = new System.Windows.Forms.TabPage();
            this.tabPtzZeroPage = new System.Windows.Forms.TabPage();
            this.gbxPtzSupplement = new System.Windows.Forms.GroupBox();
            this.tabPtzSupplementCommands = new System.Windows.Forms.TabControl();
            this.tabPtzSupplementBasic = new System.Windows.Forms.TabPage();
            this.tabPtzSupplementArea = new System.Windows.Forms.TabPage();
            this.tabPtzSupplementPreset = new System.Windows.Forms.TabPage();
            this.tabPtzSupplementQuery = new System.Windows.Forms.TabPage();
            this.tabPtzSupplementZero = new System.Windows.Forms.TabPage();
            this.btnPtzSupDirUp = new System.Windows.Forms.Button();
            this.btnPtzSupDirDown = new System.Windows.Forms.Button();
            this.btnPtzSupDirLeft = new System.Windows.Forms.Button();
            this.btnPtzSupDirRight = new System.Windows.Forms.Button();
            this.btnPtzSupStop = new System.Windows.Forms.Button();
            this.btnPtzSupAreaAngleHa = new System.Windows.Forms.Button();
            this.btnPtzSupAreaAngleHb = new System.Windows.Forms.Button();
            this.btnPtzSupAreaAngleVa = new System.Windows.Forms.Button();
            this.btnPtzSupAreaAngleVb = new System.Windows.Forms.Button();
            this.btnPtzSupAreaIntervalH = new System.Windows.Forms.Button();
            this.btnPtzSupAreaIntervalV = new System.Windows.Forms.Button();
            this.btnPtzSupAreaStartMulti = new System.Windows.Forms.Button();
            this.btnPtzSupAreaClose = new System.Windows.Forms.Button();
            this.btnPtzSupPresetStandardSet = new System.Windows.Forms.Button();
            this.btnPtzSupPresetStandardCall = new System.Windows.Forms.Button();
            this.btnPtzSupPresetStandardDelete = new System.Windows.Forms.Button();
            this.btnPtzSupPresetStart = new System.Windows.Forms.Button();
            this.btnPtzSupPresetClose = new System.Windows.Forms.Button();
            this.btnPtzSupQueryMode = new System.Windows.Forms.Button();
            this.btnPtzSupQueryStatus = new System.Windows.Forms.Button();
            this.btnPtzSupAngleRealtimeOn = new System.Windows.Forms.Button();
            this.btnPtzSupAngleRealtimeOff = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox_DepthCompletion.SuspendLayout();
            this.tableLayoutPanelSplit.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FusionResult)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.gbxPtzNet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzLocalPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAddress)).BeginInit();
            this.gbxPtzQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzRealtimeInterval)).BeginInit();
            this.gbxPtzPreset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPreset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPresetStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPresetEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPresetTime)).BeginInit();
            this.gbxPtzRaw.SuspendLayout();
            this.gbxPtzDirect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzHSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzVSpeed)).BeginInit();
            this.gbxPtzArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaHStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaHEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaHInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaVStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaVEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaVInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaEnd)).BeginInit();
            this.gbxPtzLocate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzHAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzVAngle)).BeginInit();
            this.gbxPtzExtPreset.SuspendLayout();
            this.flpPtzSupplementPreset.SuspendLayout();
            this.gbxPtzExtArea.SuspendLayout();
            this.flpPtzSupplementArea.SuspendLayout();
            this.gbxPtzExtMaintenance.SuspendLayout();
            this.flpPtzSupplementZero.SuspendLayout();
            this.gbxPtzExtBasic.SuspendLayout();
            this.flpPtzSupplementBasic.SuspendLayout();
            this.gbxPtzExtQuery.SuspendLayout();
            this.flpPtzSupplementQuery.SuspendLayout();
            this.tabPtzNavigation.SuspendLayout();
            this.tabPtzSupplementCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(2057, 822);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.panel_Video);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(2049, 796);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "控制与日志";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btn_UseZoom);
            this.groupBox7.Controls.Add(this.btn_EnableOsd);
            this.groupBox7.Controls.Add(this.btn_RemoveOsd);
            this.groupBox7.Controls.Add(this.btn_Snapshot);
            this.groupBox7.Controls.Add(this.btn_ZoomOut);
            this.groupBox7.Controls.Add(this.btn_ZoomIn);
            this.groupBox7.Location = new System.Drawing.Point(882, 391);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(209, 359);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "相机控制";
            // 
            // btn_EnableOsd
            // 
            this.btn_EnableOsd.Location = new System.Drawing.Point(118, 208);
            this.btn_EnableOsd.Name = "btn_EnableOsd";
            this.btn_EnableOsd.Size = new System.Drawing.Size(75, 23);
            this.btn_EnableOsd.TabIndex = 4;
            this.btn_EnableOsd.Text = "恢复水印";
            this.btn_EnableOsd.UseVisualStyleBackColor = true;
            this.btn_EnableOsd.Click += new System.EventHandler(this.btn_EnableOsd_Click_1);
            // 
            // btn_RemoveOsd
            // 
            this.btn_RemoveOsd.Location = new System.Drawing.Point(20, 208);
            this.btn_RemoveOsd.Name = "btn_RemoveOsd";
            this.btn_RemoveOsd.Size = new System.Drawing.Size(75, 23);
            this.btn_RemoveOsd.TabIndex = 3;
            this.btn_RemoveOsd.Text = "去除水印";
            this.btn_RemoveOsd.UseVisualStyleBackColor = true;
            this.btn_RemoveOsd.Click += new System.EventHandler(this.btn_RemoveOsd_Click);
            // 
            // btn_Snapshot
            // 
            this.btn_Snapshot.Location = new System.Drawing.Point(118, 81);
            this.btn_Snapshot.Name = "btn_Snapshot";
            this.btn_Snapshot.Size = new System.Drawing.Size(75, 23);
            this.btn_Snapshot.TabIndex = 2;
            this.btn_Snapshot.Text = "抓拍";
            this.btn_Snapshot.UseVisualStyleBackColor = true;
            this.btn_Snapshot.Click += new System.EventHandler(this.btn_Snapshot_Click);
            // 
            // btn_ZoomOut
            // 
            this.btn_ZoomOut.Location = new System.Drawing.Point(17, 116);
            this.btn_ZoomOut.Name = "btn_ZoomOut";
            this.btn_ZoomOut.Size = new System.Drawing.Size(75, 23);
            this.btn_ZoomOut.TabIndex = 1;
            this.btn_ZoomOut.Text = "焦距减小";
            this.btn_ZoomOut.UseVisualStyleBackColor = true;
            this.btn_ZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomOut_MouseDown);
            this.btn_ZoomOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomOut_MouseUp);
            // 
            // btn_ZoomIn
            // 
            this.btn_ZoomIn.Location = new System.Drawing.Point(17, 51);
            this.btn_ZoomIn.Name = "btn_ZoomIn";
            this.btn_ZoomIn.Size = new System.Drawing.Size(75, 23);
            this.btn_ZoomIn.TabIndex = 0;
            this.btn_ZoomIn.Text = "焦距放大";
            this.btn_ZoomIn.UseVisualStyleBackColor = true;
            this.btn_ZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomIn_MouseDown);
            this.btn_ZoomIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomIn_MouseUp);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.listBox_Log);
            this.groupBox3.Location = new System.Drawing.Point(12, 218);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(864, 557);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "日志";
            // 
            // listBox_Log
            // 
            this.listBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.ItemHeight = 12;
            this.listBox_Log.Location = new System.Drawing.Point(3, 17);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.Size = new System.Drawing.Size(858, 537);
            this.listBox_Log.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_SetCoordinate);
            this.groupBox2.Controls.Add(this.cbx_Coordinate);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btn_StopSample);
            this.groupBox2.Controls.Add(this.btn_StartSample);
            this.groupBox2.Controls.Add(this.btn_SetMode);
            this.groupBox2.Controls.Add(this.cbx_WorkMode);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btn_SetScanPattern);
            this.groupBox2.Controls.Add(this.cbx_ScanPattern);
            this.groupBox2.Location = new System.Drawing.Point(882, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 359);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "雷达控制";
            // 
            // btn_SetCoordinate
            // 
            this.btn_SetCoordinate.Location = new System.Drawing.Point(133, 282);
            this.btn_SetCoordinate.Name = "btn_SetCoordinate";
            this.btn_SetCoordinate.Size = new System.Drawing.Size(50, 23);
            this.btn_SetCoordinate.TabIndex = 9;
            this.btn_SetCoordinate.Text = "设置";
            this.btn_SetCoordinate.UseVisualStyleBackColor = true;
            this.btn_SetCoordinate.Click += new System.EventHandler(this.btn_SetCoordinate_Click);
            // 
            // cbx_Coordinate
            // 
            this.cbx_Coordinate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_Coordinate.FormattingEnabled = true;
            this.cbx_Coordinate.Items.AddRange(new object[] {
            "直角坐标",
            "球坐标"});
            this.cbx_Coordinate.Location = new System.Drawing.Point(17, 285);
            this.cbx_Coordinate.Name = "cbx_Coordinate";
            this.cbx_Coordinate.Size = new System.Drawing.Size(110, 20);
            this.cbx_Coordinate.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 267);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "坐标系：";
            // 
            // btn_StopSample
            // 
            this.btn_StopSample.Location = new System.Drawing.Point(17, 156);
            this.btn_StopSample.Name = "btn_StopSample";
            this.btn_StopSample.Size = new System.Drawing.Size(160, 30);
            this.btn_StopSample.TabIndex = 12;
            this.btn_StopSample.Text = "停止采样";
            this.btn_StopSample.UseVisualStyleBackColor = true;
            this.btn_StopSample.Click += new System.EventHandler(this.btn_StopSample_Click_1);
            // 
            // btn_StartSample
            // 
            this.btn_StartSample.Location = new System.Drawing.Point(17, 120);
            this.btn_StartSample.Name = "btn_StartSample";
            this.btn_StartSample.Size = new System.Drawing.Size(160, 30);
            this.btn_StartSample.TabIndex = 13;
            this.btn_StartSample.Text = "开始采样";
            this.btn_StartSample.UseVisualStyleBackColor = true;
            this.btn_StartSample.Click += new System.EventHandler(this.btn_StartSample_Click_1);
            // 
            // btn_SetMode
            // 
            this.btn_SetMode.Location = new System.Drawing.Point(17, 75);
            this.btn_SetMode.Name = "btn_SetMode";
            this.btn_SetMode.Size = new System.Drawing.Size(160, 30);
            this.btn_SetMode.TabIndex = 14;
            this.btn_SetMode.Text = "设置工作模式";
            this.btn_SetMode.UseVisualStyleBackColor = true;
            this.btn_SetMode.Click += new System.EventHandler(this.btn_SetMode_Click);
            // 
            // cbx_WorkMode
            // 
            this.cbx_WorkMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_WorkMode.FormattingEnabled = true;
            this.cbx_WorkMode.Items.AddRange(new object[] {
            "正常模式",
            "省电模式",
            "待机模式"});
            this.cbx_WorkMode.Location = new System.Drawing.Point(17, 45);
            this.cbx_WorkMode.Name = "cbx_WorkMode";
            this.cbx_WorkMode.Size = new System.Drawing.Size(160, 20);
            this.cbx_WorkMode.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "工作模式：";
            // 
            // btn_SetScanPattern
            // 
            this.btn_SetScanPattern.Location = new System.Drawing.Point(17, 227);
            this.btn_SetScanPattern.Name = "btn_SetScanPattern";
            this.btn_SetScanPattern.Size = new System.Drawing.Size(160, 25);
            this.btn_SetScanPattern.TabIndex = 21;
            this.btn_SetScanPattern.Text = "设置扫描模式";
            this.btn_SetScanPattern.UseVisualStyleBackColor = true;
            this.btn_SetScanPattern.Click += new System.EventHandler(this.btn_SetScanPattern_Click);
            // 
            // cbx_ScanPattern
            // 
            this.cbx_ScanPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_ScanPattern.FormattingEnabled = true;
            this.cbx_ScanPattern.Items.AddRange(new object[] {
            "非重复扫描",
            "重复扫描"});
            this.cbx_ScanPattern.Location = new System.Drawing.Point(17, 201);
            this.cbx_ScanPattern.Name = "cbx_ScanPattern";
            this.cbx_ScanPattern.Size = new System.Drawing.Size(160, 20);
            this.cbx_ScanPattern.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Disconnect);
            this.groupBox1.Controls.Add(this.btn_HandShake);
            this.groupBox1.Controls.Add(this.btn_StopListen);
            this.groupBox1.Controls.Add(this.btn_StartListen);
            this.groupBox1.Controls.Add(this.listView_Devices);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(864, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接管理";
            // 
            // btn_Disconnect
            // 
            this.btn_Disconnect.Location = new System.Drawing.Point(745, 133);
            this.btn_Disconnect.Name = "btn_Disconnect";
            this.btn_Disconnect.Size = new System.Drawing.Size(100, 30);
            this.btn_Disconnect.TabIndex = 4;
            this.btn_Disconnect.Text = "断开连接";
            this.btn_Disconnect.UseVisualStyleBackColor = true;
            this.btn_Disconnect.Click += new System.EventHandler(this.btn_Disconnect_Click_1);
            // 
            // btn_HandShake
            // 
            this.btn_HandShake.Location = new System.Drawing.Point(745, 97);
            this.btn_HandShake.Name = "btn_HandShake";
            this.btn_HandShake.Size = new System.Drawing.Size(100, 30);
            this.btn_HandShake.TabIndex = 3;
            this.btn_HandShake.Text = "握手(连接)";
            this.btn_HandShake.UseVisualStyleBackColor = true;
            this.btn_HandShake.Click += new System.EventHandler(this.btn_HandShake_Click);
            // 
            // btn_StopListen
            // 
            this.btn_StopListen.Enabled = false;
            this.btn_StopListen.Location = new System.Drawing.Point(745, 61);
            this.btn_StopListen.Name = "btn_StopListen";
            this.btn_StopListen.Size = new System.Drawing.Size(100, 30);
            this.btn_StopListen.TabIndex = 2;
            this.btn_StopListen.Text = "停止监听";
            this.btn_StopListen.UseVisualStyleBackColor = true;
            this.btn_StopListen.Click += new System.EventHandler(this.btn_StopListen_Click);
            // 
            // btn_StartListen
            // 
            this.btn_StartListen.Location = new System.Drawing.Point(745, 25);
            this.btn_StartListen.Name = "btn_StartListen";
            this.btn_StartListen.Size = new System.Drawing.Size(100, 30);
            this.btn_StartListen.TabIndex = 1;
            this.btn_StartListen.Text = "开始监听";
            this.btn_StartListen.UseVisualStyleBackColor = true;
            this.btn_StartListen.Click += new System.EventHandler(this.btn_StartListen_Click);
            // 
            // listView_Devices
            // 
            this.listView_Devices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView_Devices.FullRowSelect = true;
            this.listView_Devices.GridLines = true;
            this.listView_Devices.HideSelection = false;
            this.listView_Devices.Location = new System.Drawing.Point(6, 20);
            this.listView_Devices.MultiSelect = false;
            this.listView_Devices.Name = "listView_Devices";
            this.listView_Devices.Size = new System.Drawing.Size(733, 170);
            this.listView_Devices.TabIndex = 0;
            this.listView_Devices.UseCompatibleStateImageBehavior = false;
            this.listView_Devices.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "发现时间";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "IP地址";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "型号";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "状态";
            this.columnHeader4.Width = 100;
            // 
            // panel_Video
            // 
            this.panel_Video.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Video.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Video.Location = new System.Drawing.Point(1110, 75);
            this.panel_Video.Name = "panel_Video";
            this.panel_Video.Size = new System.Drawing.Size(922, 685);
            this.panel_Video.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txt_CameraIp);
            this.groupBox4.Controls.Add(this.btn_PlayCamera);
            this.groupBox4.Location = new System.Drawing.Point(1110, 9);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(922, 60);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "相机控制";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "相机 IP:";
            // 
            // txt_CameraIp
            // 
            this.txt_CameraIp.Location = new System.Drawing.Point(75, 22);
            this.txt_CameraIp.Name = "txt_CameraIp";
            this.txt_CameraIp.Size = new System.Drawing.Size(115, 21);
            this.txt_CameraIp.TabIndex = 1;
            this.txt_CameraIp.Text = "192.168.1.168";
            // 
            // btn_PlayCamera
            // 
            this.btn_PlayCamera.Location = new System.Drawing.Point(210, 20);
            this.btn_PlayCamera.Name = "btn_PlayCamera";
            this.btn_PlayCamera.Size = new System.Drawing.Size(120, 30);
            this.btn_PlayCamera.TabIndex = 2;
            this.btn_PlayCamera.Text = "播放相机";
            this.btn_PlayCamera.UseVisualStyleBackColor = true;
            this.btn_PlayCamera.Click += new System.EventHandler(this.btn_PlayCamera_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox_DepthCompletion);
            this.tabPage2.Controls.Add(this.tableLayoutPanelSplit);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(2049, 796);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "可视化视图";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_ExportPcd);
            this.groupBox5.Controls.Add(this.btn_SaveBEV);
            this.groupBox5.Controls.Add(this.btn_ShowRaw);
            this.groupBox5.Controls.Add(this.btn_SaveImage);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.dateTimePicker_Query);
            this.groupBox5.Controls.Add(this.btn_Reconstruct);
            this.groupBox5.Location = new System.Drawing.Point(8, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1010, 60);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "点云还原查询";
            // 
            // btn_ExportPcd
            // 
            this.btn_ExportPcd.Location = new System.Drawing.Point(777, 19);
            this.btn_ExportPcd.Name = "btn_ExportPcd";
            this.btn_ExportPcd.Size = new System.Drawing.Size(120, 30);
            this.btn_ExportPcd.TabIndex = 23;
            this.btn_ExportPcd.Text = "导出PCD";
            this.btn_ExportPcd.UseVisualStyleBackColor = true;
            this.btn_ExportPcd.Click += new System.EventHandler(this.btn_ExportPcd_Click);
            // 
            // btn_SaveBEV
            // 
            this.btn_SaveBEV.Location = new System.Drawing.Point(651, 19);
            this.btn_SaveBEV.Name = "btn_SaveBEV";
            this.btn_SaveBEV.Size = new System.Drawing.Size(120, 30);
            this.btn_SaveBEV.TabIndex = 22;
            this.btn_SaveBEV.Text = "保存为鸟瞰图";
            this.btn_SaveBEV.UseVisualStyleBackColor = true;
            this.btn_SaveBEV.Click += new System.EventHandler(this.btn_SaveBEV_Click);
            // 
            // btn_ShowRaw
            // 
            this.btn_ShowRaw.Location = new System.Drawing.Point(525, 19);
            this.btn_ShowRaw.Name = "btn_ShowRaw";
            this.btn_ShowRaw.Size = new System.Drawing.Size(120, 30);
            this.btn_ShowRaw.TabIndex = 21;
            this.btn_ShowRaw.Text = "原始点云";
            this.btn_ShowRaw.UseVisualStyleBackColor = true;
            this.btn_ShowRaw.Click += new System.EventHandler(this.btn_ShowRaw_Click);
            // 
            // btn_SaveImage
            // 
            this.btn_SaveImage.Location = new System.Drawing.Point(390, 20);
            this.btn_SaveImage.Name = "btn_SaveImage";
            this.btn_SaveImage.Size = new System.Drawing.Size(120, 30);
            this.btn_SaveImage.TabIndex = 20;
            this.btn_SaveImage.Text = "保存为图片";
            this.btn_SaveImage.UseVisualStyleBackColor = true;
            this.btn_SaveImage.Click += new System.EventHandler(this.btn_SaveImage_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "查询时间：";
            // 
            // dateTimePicker_Query
            // 
            this.dateTimePicker_Query.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_Query.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Query.Location = new System.Drawing.Point(85, 22);
            this.dateTimePicker_Query.Name = "dateTimePicker_Query";
            this.dateTimePicker_Query.ShowUpDown = true;
            this.dateTimePicker_Query.Size = new System.Drawing.Size(160, 21);
            this.dateTimePicker_Query.TabIndex = 18;
            // 
            // btn_Reconstruct
            // 
            this.btn_Reconstruct.Location = new System.Drawing.Point(260, 20);
            this.btn_Reconstruct.Name = "btn_Reconstruct";
            this.btn_Reconstruct.Size = new System.Drawing.Size(120, 30);
            this.btn_Reconstruct.TabIndex = 17;
            this.btn_Reconstruct.Text = "查询并还原点云";
            this.btn_Reconstruct.UseVisualStyleBackColor = true;
            this.btn_Reconstruct.Click += new System.EventHandler(this.btn_Reconstruct_Click);
            // 
            // groupBox_DepthCompletion
            // 
            this.groupBox_DepthCompletion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_DepthCompletion.Controls.Add(this.btn_CompleteDepth);
            this.groupBox_DepthCompletion.Location = new System.Drawing.Point(1030, 6);
            this.groupBox_DepthCompletion.Name = "groupBox_DepthCompletion";
            this.groupBox_DepthCompletion.Size = new System.Drawing.Size(1010, 60);
            this.groupBox_DepthCompletion.TabIndex = 8;
            this.groupBox_DepthCompletion.TabStop = false;
            this.groupBox_DepthCompletion.Text = "深度补全";
            // 
            // btn_CompleteDepth
            // 
            this.btn_CompleteDepth.Location = new System.Drawing.Point(15, 20);
            this.btn_CompleteDepth.Name = "btn_CompleteDepth";
            this.btn_CompleteDepth.Size = new System.Drawing.Size(150, 30);
            this.btn_CompleteDepth.TabIndex = 23;
            this.btn_CompleteDepth.Text = "执行深度补全并渲染";
            this.btn_CompleteDepth.UseVisualStyleBackColor = true;
            this.btn_CompleteDepth.Click += new System.EventHandler(this.btn_CompleteDepth_Click);
            // 
            // tableLayoutPanelSplit
            // 
            this.tableLayoutPanelSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelSplit.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanelSplit.ColumnCount = 2;
            this.tableLayoutPanelSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSplit.Controls.Add(this.renderWindowControl1, 0, 0);
            this.tableLayoutPanelSplit.Controls.Add(this.renderWindowControl2, 1, 0);
            this.tableLayoutPanelSplit.Location = new System.Drawing.Point(6, 75);
            this.tableLayoutPanelSplit.Name = "tableLayoutPanelSplit";
            this.tableLayoutPanelSplit.RowCount = 1;
            this.tableLayoutPanelSplit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSplit.Size = new System.Drawing.Size(2034, 689);
            this.tableLayoutPanelSplit.TabIndex = 10;
            // 
            // renderWindowControl1
            // 
            this.renderWindowControl1.AddTestActors = false;
            this.renderWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderWindowControl1.Location = new System.Drawing.Point(5, 5);
            this.renderWindowControl1.Name = "renderWindowControl1";
            this.renderWindowControl1.Size = new System.Drawing.Size(1008, 679);
            this.renderWindowControl1.TabIndex = 4;
            this.renderWindowControl1.TestText = null;
            // 
            // renderWindowControl2
            // 
            this.renderWindowControl2.AddTestActors = false;
            this.renderWindowControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderWindowControl2.Location = new System.Drawing.Point(1021, 5);
            this.renderWindowControl2.Name = "renderWindowControl2";
            this.renderWindowControl2.Size = new System.Drawing.Size(1008, 679);
            this.renderWindowControl2.TabIndex = 9;
            this.renderWindowControl2.TestText = null;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox_FusionResult);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(2049, 796);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "点云CCD融合";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox_FusionResult
            // 
            this.pictureBox_FusionResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_FusionResult.BackColor = System.Drawing.Color.Black;
            this.pictureBox_FusionResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_FusionResult.Location = new System.Drawing.Point(8, 72);
            this.pictureBox_FusionResult.Name = "pictureBox_FusionResult";
            this.pictureBox_FusionResult.Size = new System.Drawing.Size(980, 684);
            this.pictureBox_FusionResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_FusionResult.TabIndex = 1;
            this.pictureBox_FusionResult.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.btn_ExecuteFusion);
            this.groupBox6.Controls.Add(this.btn_SelectVideo);
            this.groupBox6.Controls.Add(this.txt_VideoPath);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.dateTimePicker_Fusion);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.btn_LoadPcd);
            this.groupBox6.Location = new System.Drawing.Point(8, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(980, 60);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "离线融合控制";
            // 
            // btn_ExecuteFusion
            // 
            this.btn_ExecuteFusion.Location = new System.Drawing.Point(733, 17);
            this.btn_ExecuteFusion.Name = "btn_ExecuteFusion";
            this.btn_ExecuteFusion.Size = new System.Drawing.Size(100, 30);
            this.btn_ExecuteFusion.TabIndex = 5;
            this.btn_ExecuteFusion.Text = "执行融合";
            this.btn_ExecuteFusion.UseVisualStyleBackColor = true;
            this.btn_ExecuteFusion.Click += new System.EventHandler(this.btn_ExecuteFusion_Click);
            // 
            // btn_SelectVideo
            // 
            this.btn_SelectVideo.Location = new System.Drawing.Point(647, 20);
            this.btn_SelectVideo.Name = "btn_SelectVideo";
            this.btn_SelectVideo.Size = new System.Drawing.Size(80, 25);
            this.btn_SelectVideo.TabIndex = 4;
            this.btn_SelectVideo.Text = "选择视频...";
            this.btn_SelectVideo.UseVisualStyleBackColor = true;
            this.btn_SelectVideo.Click += new System.EventHandler(this.btn_SelectVideo_Click);
            // 
            // txt_VideoPath
            // 
            this.txt_VideoPath.Location = new System.Drawing.Point(341, 22);
            this.txt_VideoPath.Name = "txt_VideoPath";
            this.txt_VideoPath.ReadOnly = true;
            this.txt_VideoPath.Size = new System.Drawing.Size(300, 21);
            this.txt_VideoPath.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(270, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "视频路径：";
            // 
            // dateTimePicker_Fusion
            // 
            this.dateTimePicker_Fusion.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker_Fusion.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Fusion.Location = new System.Drawing.Point(86, 22);
            this.dateTimePicker_Fusion.Name = "dateTimePicker_Fusion";
            this.dateTimePicker_Fusion.ShowUpDown = true;
            this.dateTimePicker_Fusion.Size = new System.Drawing.Size(160, 21);
            this.dateTimePicker_Fusion.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "目标时间：";
            // 
            // btn_LoadPcd
            // 
            this.btn_LoadPcd.Location = new System.Drawing.Point(839, 16);
            this.btn_LoadPcd.Name = "btn_LoadPcd";
            this.btn_LoadPcd.Size = new System.Drawing.Size(120, 30);
            this.btn_LoadPcd.TabIndex = 6;
            this.btn_LoadPcd.Text = "加载并投影PCD";
            this.btn_LoadPcd.UseVisualStyleBackColor = true;
            this.btn_LoadPcd.Click += new System.EventHandler(this.btn_LoadPcd_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.gbxPtzNet);
            this.tabPage4.Controls.Add(this.gbxPtzQuery);
            this.tabPage4.Controls.Add(this.gbxPtzPreset);
            this.tabPage4.Controls.Add(this.gbxPtzRaw);
            this.tabPage4.Controls.Add(this.gbxPtzDirect);
            this.tabPage4.Controls.Add(this.gbxPtzArea);
            this.tabPage4.Controls.Add(this.gbxPtzLocate);
            this.tabPage4.Controls.Add(this.gbxPtzExtPreset);
            this.tabPage4.Controls.Add(this.gbxPtzExtArea);
            this.tabPage4.Controls.Add(this.gbxPtzExtMaintenance);
            this.tabPage4.Controls.Add(this.gbxPtzExtBasic);
            this.tabPage4.Controls.Add(this.gbxPtzExtQuery);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(2049, 796);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "云台控制";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // gbxPtzNet
            // 
            this.gbxPtzNet.Controls.Add(this.lblLIn);
            this.gbxPtzNet.Controls.Add(this.txt_PtzLocalIp);
            this.gbxPtzNet.Controls.Add(this.nud_PtzLocalPort);
            this.gbxPtzNet.Controls.Add(this.lblRIn);
            this.gbxPtzNet.Controls.Add(this.txt_PtzIp);
            this.gbxPtzNet.Controls.Add(this.nud_PtzPort);
            this.gbxPtzNet.Controls.Add(this.lblAddr);
            this.gbxPtzNet.Controls.Add(this.nud_PtzAddress);
            this.gbxPtzNet.Controls.Add(this.btnPtzOpen);
            this.gbxPtzNet.Controls.Add(this.btnPtzClose);
            this.gbxPtzNet.Location = new System.Drawing.Point(10, 10);
            this.gbxPtzNet.Name = "gbxPtzNet";
            this.gbxPtzNet.Size = new System.Drawing.Size(430, 120);
            this.gbxPtzNet.TabIndex = 0;
            this.gbxPtzNet.TabStop = false;
            this.gbxPtzNet.Text = "通讯与基础配置";
            // 
            // lblLIn
            // 
            this.lblLIn.Location = new System.Drawing.Point(10, 25);
            this.lblLIn.Name = "lblLIn";
            this.lblLIn.Size = new System.Drawing.Size(80, 15);
            this.lblLIn.TabIndex = 0;
            this.lblLIn.Text = "本地IP/端口:";
            // 
            // txt_PtzLocalIp
            // 
            this.txt_PtzLocalIp.Location = new System.Drawing.Point(95, 22);
            this.txt_PtzLocalIp.Name = "txt_PtzLocalIp";
            this.txt_PtzLocalIp.Size = new System.Drawing.Size(110, 21);
            this.txt_PtzLocalIp.TabIndex = 1;
            this.txt_PtzLocalIp.Text = "0.0.0.0";
            // 
            // nud_PtzLocalPort
            // 
            this.nud_PtzLocalPort.Location = new System.Drawing.Point(210, 22);
            this.nud_PtzLocalPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nud_PtzLocalPort.Name = "nud_PtzLocalPort";
            this.nud_PtzLocalPort.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzLocalPort.TabIndex = 2;
            this.nud_PtzLocalPort.Value = new decimal(new int[] {
            6666,
            0,
            0,
            0});
            // 
            // lblRIn
            // 
            this.lblRIn.Location = new System.Drawing.Point(10, 55);
            this.lblRIn.Name = "lblRIn";
            this.lblRIn.Size = new System.Drawing.Size(80, 15);
            this.lblRIn.TabIndex = 3;
            this.lblRIn.Text = "云台IP/端口:";
            // 
            // txt_PtzIp
            // 
            this.txt_PtzIp.Location = new System.Drawing.Point(95, 52);
            this.txt_PtzIp.Name = "txt_PtzIp";
            this.txt_PtzIp.Size = new System.Drawing.Size(110, 21);
            this.txt_PtzIp.TabIndex = 4;
            // 
            // nud_PtzPort
            // 
            this.nud_PtzPort.Location = new System.Drawing.Point(210, 52);
            this.nud_PtzPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nud_PtzPort.Name = "nud_PtzPort";
            this.nud_PtzPort.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzPort.TabIndex = 5;
            this.nud_PtzPort.Value = new decimal(new int[] {
            6666,
            0,
            0,
            0});
            // 
            // lblAddr
            // 
            this.lblAddr.Location = new System.Drawing.Point(10, 85);
            this.lblAddr.Name = "lblAddr";
            this.lblAddr.Size = new System.Drawing.Size(90, 15);
            this.lblAddr.TabIndex = 6;
            this.lblAddr.Text = "云台地址(add):";
            // 
            // nud_PtzAddress
            // 
            this.nud_PtzAddress.Location = new System.Drawing.Point(105, 82);
            this.nud_PtzAddress.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_PtzAddress.Name = "nud_PtzAddress";
            this.nud_PtzAddress.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzAddress.TabIndex = 7;
            this.nud_PtzAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnPtzOpen
            // 
            this.btnPtzOpen.Location = new System.Drawing.Point(290, 20);
            this.btnPtzOpen.Name = "btnPtzOpen";
            this.btnPtzOpen.Size = new System.Drawing.Size(130, 30);
            this.btnPtzOpen.TabIndex = 8;
            this.btnPtzOpen.Text = "打开UDP";
            this.btnPtzOpen.Click += new System.EventHandler(this.btnPtzOpen_Click);
            // 
            // btnPtzClose
            // 
            this.btnPtzClose.Location = new System.Drawing.Point(290, 55);
            this.btnPtzClose.Name = "btnPtzClose";
            this.btnPtzClose.Size = new System.Drawing.Size(130, 30);
            this.btnPtzClose.TabIndex = 9;
            this.btnPtzClose.Text = "关闭UDP";
            this.btnPtzClose.Click += new System.EventHandler(this.btnPtzClose_Click);
            // 
            // gbxPtzQuery
            // 
            this.gbxPtzQuery.Controls.Add(this.btnPtzQueryStatus);
            this.gbxPtzQuery.Controls.Add(this.btnPtzQueryMode);
            this.gbxPtzQuery.Controls.Add(this.lblRti);
            this.gbxPtzQuery.Controls.Add(this.nud_PtzRealtimeInterval);
            this.gbxPtzQuery.Controls.Add(this.btnPtzRealtimeAngleOn);
            this.gbxPtzQuery.Controls.Add(this.btnPtzRealtimeAngleOff);
            this.gbxPtzQuery.Controls.Add(this.lbl_PtzHAngle);
            this.gbxPtzQuery.Controls.Add(this.lbl_PtzVAngle);
            this.gbxPtzQuery.Controls.Add(this.lbl_PtzStatus);
            this.gbxPtzQuery.Location = new System.Drawing.Point(10, 140);
            this.gbxPtzQuery.Name = "gbxPtzQuery";
            this.gbxPtzQuery.Size = new System.Drawing.Size(430, 200);
            this.gbxPtzQuery.TabIndex = 5;
            this.gbxPtzQuery.TabStop = false;
            this.gbxPtzQuery.Text = "云台运行参数查询与主动回传";
            // 
            // btnPtzQueryStatus
            // 
            this.btnPtzQueryStatus.Location = new System.Drawing.Point(15, 25);
            this.btnPtzQueryStatus.Name = "btnPtzQueryStatus";
            this.btnPtzQueryStatus.Size = new System.Drawing.Size(180, 30);
            this.btnPtzQueryStatus.TabIndex = 0;
            this.btnPtzQueryStatus.Text = "指令查询工作状态";
            this.btnPtzQueryStatus.Click += new System.EventHandler(this.btnPtzQueryStatus_Click);
            // 
            // btnPtzQueryMode
            // 
            this.btnPtzQueryMode.Location = new System.Drawing.Point(210, 25);
            this.btnPtzQueryMode.Name = "btnPtzQueryMode";
            this.btnPtzQueryMode.Size = new System.Drawing.Size(180, 30);
            this.btnPtzQueryMode.TabIndex = 1;
            this.btnPtzQueryMode.Text = "指令查询当前模式";
            this.btnPtzQueryMode.Click += new System.EventHandler(this.btnPtzQueryMode_Click);
            // 
            // lblRti
            // 
            this.lblRti.Location = new System.Drawing.Point(15, 75);
            this.lblRti.Name = "lblRti";
            this.lblRti.Size = new System.Drawing.Size(90, 15);
            this.lblRti.TabIndex = 2;
            this.lblRti.Text = "回传间隔(ms):";
            // 
            // nud_PtzRealtimeInterval
            // 
            this.nud_PtzRealtimeInterval.Location = new System.Drawing.Point(110, 72);
            this.nud_PtzRealtimeInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_PtzRealtimeInterval.Name = "nud_PtzRealtimeInterval";
            this.nud_PtzRealtimeInterval.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzRealtimeInterval.TabIndex = 3;
            this.nud_PtzRealtimeInterval.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // btnPtzRealtimeAngleOn
            // 
            this.btnPtzRealtimeAngleOn.Location = new System.Drawing.Point(180, 70);
            this.btnPtzRealtimeAngleOn.Name = "btnPtzRealtimeAngleOn";
            this.btnPtzRealtimeAngleOn.Size = new System.Drawing.Size(100, 25);
            this.btnPtzRealtimeAngleOn.TabIndex = 4;
            this.btnPtzRealtimeAngleOn.Text = "开启连续回传";
            this.btnPtzRealtimeAngleOn.Click += new System.EventHandler(this.btnPtzRealtimeAngleOn_Click);
            // 
            // btnPtzRealtimeAngleOff
            // 
            this.btnPtzRealtimeAngleOff.Location = new System.Drawing.Point(290, 70);
            this.btnPtzRealtimeAngleOff.Name = "btnPtzRealtimeAngleOff";
            this.btnPtzRealtimeAngleOff.Size = new System.Drawing.Size(100, 25);
            this.btnPtzRealtimeAngleOff.TabIndex = 5;
            this.btnPtzRealtimeAngleOff.Text = "关闭回传";
            this.btnPtzRealtimeAngleOff.Click += new System.EventHandler(this.btnPtzRealtimeAngleOff_Click);
            // 
            // lbl_PtzHAngle
            // 
            this.lbl_PtzHAngle.Location = new System.Drawing.Point(15, 115);
            this.lbl_PtzHAngle.Name = "lbl_PtzHAngle";
            this.lbl_PtzHAngle.Size = new System.Drawing.Size(120, 15);
            this.lbl_PtzHAngle.TabIndex = 6;
            this.lbl_PtzHAngle.Text = "水平角度: 0.00°";
            // 
            // lbl_PtzVAngle
            // 
            this.lbl_PtzVAngle.Location = new System.Drawing.Point(145, 115);
            this.lbl_PtzVAngle.Name = "lbl_PtzVAngle";
            this.lbl_PtzVAngle.Size = new System.Drawing.Size(120, 15);
            this.lbl_PtzVAngle.TabIndex = 7;
            this.lbl_PtzVAngle.Text = "垂直角度: 0.00°";
            // 
            // lbl_PtzStatus
            // 
            this.lbl_PtzStatus.Location = new System.Drawing.Point(15, 155);
            this.lbl_PtzStatus.Name = "lbl_PtzStatus";
            this.lbl_PtzStatus.Size = new System.Drawing.Size(140, 15);
            this.lbl_PtzStatus.TabIndex = 8;
            this.lbl_PtzStatus.Text = "云台模式: 常规正常模式";
            // 
            // gbxPtzPreset
            // 
            this.gbxPtzPreset.Controls.Add(this.lblPno);
            this.gbxPtzPreset.Controls.Add(this.nud_PtzPreset);
            this.gbxPtzPreset.Controls.Add(this.btnPtzPresetSet);
            this.gbxPtzPreset.Controls.Add(this.btnPtzPresetCall);
            this.gbxPtzPreset.Controls.Add(this.btnPtzPresetDel);
            this.gbxPtzPreset.Controls.Add(this.lblPs);
            this.gbxPtzPreset.Controls.Add(this.nud_PtzPresetStart);
            this.gbxPtzPreset.Controls.Add(this.lblPe);
            this.gbxPtzPreset.Controls.Add(this.nud_PtzPresetEnd);
            this.gbxPtzPreset.Controls.Add(this.lblPt);
            this.gbxPtzPreset.Controls.Add(this.nud_PtzPresetTime);
            this.gbxPtzPreset.Controls.Add(this.btnPtzPresetScanStart);
            this.gbxPtzPreset.Controls.Add(this.btnPtzPresetScanStop);
            this.gbxPtzPreset.Location = new System.Drawing.Point(10, 350);
            this.gbxPtzPreset.Name = "gbxPtzPreset";
            this.gbxPtzPreset.Size = new System.Drawing.Size(430, 170);
            this.gbxPtzPreset.TabIndex = 3;
            this.gbxPtzPreset.TabStop = false;
            this.gbxPtzPreset.Text = "预置位与扫描控制";
            // 
            // lblPno
            // 
            this.lblPno.Location = new System.Drawing.Point(10, 25);
            this.lblPno.Name = "lblPno";
            this.lblPno.Size = new System.Drawing.Size(75, 15);
            this.lblPno.TabIndex = 0;
            this.lblPno.Text = "预置位编号:";
            // 
            // nud_PtzPreset
            // 
            this.nud_PtzPreset.Location = new System.Drawing.Point(90, 22);
            this.nud_PtzPreset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_PtzPreset.Name = "nud_PtzPreset";
            this.nud_PtzPreset.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzPreset.TabIndex = 1;
            // 
            // btnPtzPresetSet
            // 
            this.btnPtzPresetSet.Location = new System.Drawing.Point(150, 20);
            this.btnPtzPresetSet.Name = "btnPtzPresetSet";
            this.btnPtzPresetSet.Size = new System.Drawing.Size(80, 25);
            this.btnPtzPresetSet.TabIndex = 2;
            this.btnPtzPresetSet.Text = "设置当前";
            this.btnPtzPresetSet.Click += new System.EventHandler(this.btnPtzPresetSet_Click);
            // 
            // btnPtzPresetCall
            // 
            this.btnPtzPresetCall.Location = new System.Drawing.Point(240, 20);
            this.btnPtzPresetCall.Name = "btnPtzPresetCall";
            this.btnPtzPresetCall.Size = new System.Drawing.Size(80, 25);
            this.btnPtzPresetCall.TabIndex = 3;
            this.btnPtzPresetCall.Text = "调用该位";
            this.btnPtzPresetCall.Click += new System.EventHandler(this.btnPtzPresetCall_Click);
            // 
            // btnPtzPresetDel
            // 
            this.btnPtzPresetDel.Location = new System.Drawing.Point(330, 20);
            this.btnPtzPresetDel.Name = "btnPtzPresetDel";
            this.btnPtzPresetDel.Size = new System.Drawing.Size(80, 25);
            this.btnPtzPresetDel.TabIndex = 4;
            this.btnPtzPresetDel.Text = "删除该位";
            this.btnPtzPresetDel.Click += new System.EventHandler(this.btnPtzPresetDel_Click);
            // 
            // lblPs
            // 
            this.lblPs.Location = new System.Drawing.Point(10, 65);
            this.lblPs.Name = "lblPs";
            this.lblPs.Size = new System.Drawing.Size(75, 15);
            this.lblPs.TabIndex = 5;
            this.lblPs.Text = "巡航起始位:";
            // 
            // nud_PtzPresetStart
            // 
            this.nud_PtzPresetStart.Location = new System.Drawing.Point(90, 62);
            this.nud_PtzPresetStart.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_PtzPresetStart.Name = "nud_PtzPresetStart";
            this.nud_PtzPresetStart.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzPresetStart.TabIndex = 6;
            // 
            // lblPe
            // 
            this.lblPe.Location = new System.Drawing.Point(150, 65);
            this.lblPe.Name = "lblPe";
            this.lblPe.Size = new System.Drawing.Size(50, 15);
            this.lblPe.TabIndex = 7;
            this.lblPe.Text = "结束位:";
            // 
            // nud_PtzPresetEnd
            // 
            this.nud_PtzPresetEnd.Location = new System.Drawing.Point(205, 62);
            this.nud_PtzPresetEnd.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_PtzPresetEnd.Name = "nud_PtzPresetEnd";
            this.nud_PtzPresetEnd.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzPresetEnd.TabIndex = 8;
            // 
            // lblPt
            // 
            this.lblPt.Location = new System.Drawing.Point(265, 65);
            this.lblPt.Name = "lblPt";
            this.lblPt.Size = new System.Drawing.Size(85, 15);
            this.lblPt.TabIndex = 9;
            this.lblPt.Text = "驻留时间(ms):";
            // 
            // nud_PtzPresetTime
            // 
            this.nud_PtzPresetTime.Location = new System.Drawing.Point(350, 62);
            this.nud_PtzPresetTime.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.nud_PtzPresetTime.Name = "nud_PtzPresetTime";
            this.nud_PtzPresetTime.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzPresetTime.TabIndex = 10;
            this.nud_PtzPresetTime.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // btnPtzPresetScanStart
            // 
            this.btnPtzPresetScanStart.Location = new System.Drawing.Point(90, 95);
            this.btnPtzPresetScanStart.Name = "btnPtzPresetScanStart";
            this.btnPtzPresetScanStart.Size = new System.Drawing.Size(160, 30);
            this.btnPtzPresetScanStart.TabIndex = 11;
            this.btnPtzPresetScanStart.Text = "开启预置巡航扫描";
            this.btnPtzPresetScanStart.Click += new System.EventHandler(this.btnPtzPresetScanStart_Click);
            // 
            // btnPtzPresetScanStop
            // 
            this.btnPtzPresetScanStop.Location = new System.Drawing.Point(260, 95);
            this.btnPtzPresetScanStop.Name = "btnPtzPresetScanStop";
            this.btnPtzPresetScanStop.Size = new System.Drawing.Size(150, 30);
            this.btnPtzPresetScanStop.TabIndex = 12;
            this.btnPtzPresetScanStop.Text = "彻底关闭巡航";
            this.btnPtzPresetScanStop.Click += new System.EventHandler(this.btnPtzPresetScanStop_Click);
            // 
            // gbxPtzRaw
            // 
            this.gbxPtzRaw.Controls.Add(this.txt_PtzRawHex);
            this.gbxPtzRaw.Controls.Add(this.btnPtzSendRaw);
            this.gbxPtzRaw.Location = new System.Drawing.Point(10, 530);
            this.gbxPtzRaw.Name = "gbxPtzRaw";
            this.gbxPtzRaw.Size = new System.Drawing.Size(430, 60);
            this.gbxPtzRaw.TabIndex = 6;
            this.gbxPtzRaw.TabStop = false;
            this.gbxPtzRaw.Text = "自定义 Hex 原始指令下发流";
            // 
            // txt_PtzRawHex
            // 
            this.txt_PtzRawHex.Location = new System.Drawing.Point(15, 25);
            this.txt_PtzRawHex.Name = "txt_PtzRawHex";
            this.txt_PtzRawHex.Size = new System.Drawing.Size(280, 21);
            this.txt_PtzRawHex.TabIndex = 0;
            this.txt_PtzRawHex.Text = "FF 01 00 51 00 00 52";
            // 
            // btnPtzSendRaw
            // 
            this.btnPtzSendRaw.Location = new System.Drawing.Point(305, 22);
            this.btnPtzSendRaw.Name = "btnPtzSendRaw";
            this.btnPtzSendRaw.Size = new System.Drawing.Size(110, 25);
            this.btnPtzSendRaw.TabIndex = 1;
            this.btnPtzSendRaw.Text = "发送裸流";
            this.btnPtzSendRaw.Click += new System.EventHandler(this.btnPtzSendRaw_Click);
            // 
            // gbxPtzDirect
            // 
            this.gbxPtzDirect.Controls.Add(this.lblHS);
            this.gbxPtzDirect.Controls.Add(this.nud_PtzHSpeed);
            this.gbxPtzDirect.Controls.Add(this.lblVS);
            this.gbxPtzDirect.Controls.Add(this.nud_PtzVSpeed);
            this.gbxPtzDirect.Controls.Add(this.btnPtzSupDirLeftUp);
            this.gbxPtzDirect.Controls.Add(this.btnPtzSupDirRightUp);
            this.gbxPtzDirect.Controls.Add(this.btnPtzSupDirLeftDown);
            this.gbxPtzDirect.Controls.Add(this.btnPtzSupDirRightDown);
            this.gbxPtzDirect.Controls.Add(this.btnPtzUp);
            this.gbxPtzDirect.Controls.Add(this.btnPtzLeft);
            this.gbxPtzDirect.Controls.Add(this.btnPtzStop);
            this.gbxPtzDirect.Controls.Add(this.btnPtzRight);
            this.gbxPtzDirect.Controls.Add(this.btnPtzDown);
            this.gbxPtzDirect.Location = new System.Drawing.Point(450, 10);
            this.gbxPtzDirect.Name = "gbxPtzDirect";
            this.gbxPtzDirect.Size = new System.Drawing.Size(430, 330);
            this.gbxPtzDirect.TabIndex = 1;
            this.gbxPtzDirect.TabStop = false;
            this.gbxPtzDirect.Text = "手动方向控制 (速度单位: r/min)";
            // 
            // lblHS
            // 
            this.lblHS.Location = new System.Drawing.Point(15, 28);
            this.lblHS.Name = "lblHS";
            this.lblHS.Size = new System.Drawing.Size(60, 15);
            this.lblHS.TabIndex = 0;
            this.lblHS.Text = "水平转速:";
            // 
            // nud_PtzHSpeed
            // 
            this.nud_PtzHSpeed.DecimalPlaces = 1;
            this.nud_PtzHSpeed.Location = new System.Drawing.Point(95, 25);
            this.nud_PtzHSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.nud_PtzHSpeed.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            this.nud_PtzHSpeed.Name = "nud_PtzHSpeed";
            this.nud_PtzHSpeed.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzHSpeed.TabIndex = 1;
            this.nud_PtzHSpeed.Value = new decimal(new int[] {
            12,
            0,
            0,
            65536});
            // 
            // lblVS
            // 
            this.lblVS.Location = new System.Drawing.Point(205, 28);
            this.lblVS.Name = "lblVS";
            this.lblVS.Size = new System.Drawing.Size(60, 15);
            this.lblVS.TabIndex = 2;
            this.lblVS.Text = "垂直转速:";
            // 
            // nud_PtzVSpeed
            // 
            this.nud_PtzVSpeed.DecimalPlaces = 1;
            this.nud_PtzVSpeed.Location = new System.Drawing.Point(285, 25);
            this.nud_PtzVSpeed.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nud_PtzVSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_PtzVSpeed.Name = "nud_PtzVSpeed";
            this.nud_PtzVSpeed.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzVSpeed.TabIndex = 3;
            this.nud_PtzVSpeed.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // btnPtzSupDirLeftUp
            // 
            this.btnPtzSupDirLeftUp.AutoEllipsis = true;
            this.btnPtzSupDirLeftUp.Location = new System.Drawing.Point(35, 85);
            this.btnPtzSupDirLeftUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirLeftUp.Name = "btnPtzSupDirLeftUp";
            this.btnPtzSupDirLeftUp.Size = new System.Drawing.Size(95, 42);
            this.btnPtzSupDirLeftUp.TabIndex = 0;
            this.btnPtzSupDirLeftUp.Tag = "dir_left_up";
            this.btnPtzSupDirLeftUp.Text = "左上";
            this.btnPtzSupDirLeftUp.UseVisualStyleBackColor = true;
            this.btnPtzSupDirLeftUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirLeftUp.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirLeftUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupDirRightUp
            // 
            this.btnPtzSupDirRightUp.AutoEllipsis = true;
            this.btnPtzSupDirRightUp.Location = new System.Drawing.Point(255, 85);
            this.btnPtzSupDirRightUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirRightUp.Name = "btnPtzSupDirRightUp";
            this.btnPtzSupDirRightUp.Size = new System.Drawing.Size(95, 42);
            this.btnPtzSupDirRightUp.TabIndex = 0;
            this.btnPtzSupDirRightUp.Tag = "dir_right_up";
            this.btnPtzSupDirRightUp.Text = "右上";
            this.btnPtzSupDirRightUp.UseVisualStyleBackColor = true;
            this.btnPtzSupDirRightUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirRightUp.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirRightUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupDirLeftDown
            // 
            this.btnPtzSupDirLeftDown.AutoEllipsis = true;
            this.btnPtzSupDirLeftDown.Location = new System.Drawing.Point(35, 195);
            this.btnPtzSupDirLeftDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirLeftDown.Name = "btnPtzSupDirLeftDown";
            this.btnPtzSupDirLeftDown.Size = new System.Drawing.Size(95, 42);
            this.btnPtzSupDirLeftDown.TabIndex = 0;
            this.btnPtzSupDirLeftDown.Tag = "dir_left_down";
            this.btnPtzSupDirLeftDown.Text = "左下";
            this.btnPtzSupDirLeftDown.UseVisualStyleBackColor = true;
            this.btnPtzSupDirLeftDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirLeftDown.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirLeftDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupDirRightDown
            // 
            this.btnPtzSupDirRightDown.AutoEllipsis = true;
            this.btnPtzSupDirRightDown.Location = new System.Drawing.Point(255, 195);
            this.btnPtzSupDirRightDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirRightDown.Name = "btnPtzSupDirRightDown";
            this.btnPtzSupDirRightDown.Size = new System.Drawing.Size(95, 42);
            this.btnPtzSupDirRightDown.TabIndex = 0;
            this.btnPtzSupDirRightDown.Tag = "dir_right_down";
            this.btnPtzSupDirRightDown.Text = "右下";
            this.btnPtzSupDirRightDown.UseVisualStyleBackColor = true;
            this.btnPtzSupDirRightDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirRightDown.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirRightDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzUp
            // 
            this.btnPtzUp.Location = new System.Drawing.Point(145, 85);
            this.btnPtzUp.Name = "btnPtzUp";
            this.btnPtzUp.Size = new System.Drawing.Size(95, 42);
            this.btnPtzUp.TabIndex = 4;
            this.btnPtzUp.Text = "上";
            this.btnPtzUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPtzUp_MouseDown);
            this.btnPtzUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPtzDirection_MouseUp);
            // 
            // btnPtzLeft
            // 
            this.btnPtzLeft.Location = new System.Drawing.Point(35, 140);
            this.btnPtzLeft.Name = "btnPtzLeft";
            this.btnPtzLeft.Size = new System.Drawing.Size(95, 42);
            this.btnPtzLeft.TabIndex = 5;
            this.btnPtzLeft.Text = "左";
            this.btnPtzLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPtzLeft_MouseDown);
            this.btnPtzLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPtzDirection_MouseUp);
            // 
            // btnPtzStop
            // 
            this.btnPtzStop.Location = new System.Drawing.Point(145, 140);
            this.btnPtzStop.Name = "btnPtzStop";
            this.btnPtzStop.Size = new System.Drawing.Size(95, 42);
            this.btnPtzStop.TabIndex = 6;
            this.btnPtzStop.Text = "■ 停止";
            this.btnPtzStop.Click += new System.EventHandler(this.btnPtzStop_Click);
            // 
            // btnPtzRight
            // 
            this.btnPtzRight.Location = new System.Drawing.Point(255, 140);
            this.btnPtzRight.Name = "btnPtzRight";
            this.btnPtzRight.Size = new System.Drawing.Size(95, 42);
            this.btnPtzRight.TabIndex = 7;
            this.btnPtzRight.Text = "右";
            this.btnPtzRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPtzRight_MouseDown);
            this.btnPtzRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPtzDirection_MouseUp);
            // 
            // btnPtzDown
            // 
            this.btnPtzDown.Location = new System.Drawing.Point(145, 195);
            this.btnPtzDown.Name = "btnPtzDown";
            this.btnPtzDown.Size = new System.Drawing.Size(95, 42);
            this.btnPtzDown.TabIndex = 8;
            this.btnPtzDown.Text = "下";
            this.btnPtzDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPtzDown_MouseDown);
            this.btnPtzDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPtzDirection_MouseUp);
            // 
            // gbxPtzArea
            // 
            this.gbxPtzArea.Controls.Add(this.lblAn);
            this.gbxPtzArea.Controls.Add(this.nud_PtzArea);
            this.gbxPtzArea.Controls.Add(this.lblAhs);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaHStart);
            this.gbxPtzArea.Controls.Add(this.lblAhe);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaHEnd);
            this.gbxPtzArea.Controls.Add(this.lblAhi);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaHInterval);
            this.gbxPtzArea.Controls.Add(this.lblAvs);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaVStart);
            this.gbxPtzArea.Controls.Add(this.lblAve);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaVEnd);
            this.gbxPtzArea.Controls.Add(this.lblAvi);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaVInterval);
            this.gbxPtzArea.Controls.Add(this.lblAtm);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaTime);
            this.gbxPtzArea.Controls.Add(this.btnPtzAreaSetBound);
            this.gbxPtzArea.Controls.Add(this.btnPtzAreaSetInterval);
            this.gbxPtzArea.Controls.Add(this.lblAs);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaStart);
            this.gbxPtzArea.Controls.Add(this.lblAe);
            this.gbxPtzArea.Controls.Add(this.nud_PtzAreaEnd);
            this.gbxPtzArea.Controls.Add(this.btnPtzAreaScanStart);
            this.gbxPtzArea.Controls.Add(this.btnPtzAreaScanStop);
            this.gbxPtzArea.Location = new System.Drawing.Point(450, 350);
            this.gbxPtzArea.Name = "gbxPtzArea";
            this.gbxPtzArea.Size = new System.Drawing.Size(430, 400);
            this.gbxPtzArea.TabIndex = 4;
            this.gbxPtzArea.TabStop = false;
            this.gbxPtzArea.Text = "高级区域扫描配置";
            // 
            // lblAn
            // 
            this.lblAn.Location = new System.Drawing.Point(10, 25);
            this.lblAn.Name = "lblAn";
            this.lblAn.Size = new System.Drawing.Size(110, 15);
            this.lblAn.TabIndex = 0;
            this.lblAn.Text = "扫描区域号(0-14):";
            // 
            // nud_PtzArea
            // 
            this.nud_PtzArea.Location = new System.Drawing.Point(125, 22);
            this.nud_PtzArea.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.nud_PtzArea.Name = "nud_PtzArea";
            this.nud_PtzArea.Size = new System.Drawing.Size(50, 21);
            this.nud_PtzArea.TabIndex = 1;
            // 
            // lblAhs
            // 
            this.lblAhs.Location = new System.Drawing.Point(10, 55);
            this.lblAhs.Name = "lblAhs";
            this.lblAhs.Size = new System.Drawing.Size(60, 15);
            this.lblAhs.TabIndex = 2;
            this.lblAhs.Text = "水平始°:";
            // 
            // nud_PtzAreaHStart
            // 
            this.nud_PtzAreaHStart.DecimalPlaces = 2;
            this.nud_PtzAreaHStart.Location = new System.Drawing.Point(70, 52);
            this.nud_PtzAreaHStart.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nud_PtzAreaHStart.Name = "nud_PtzAreaHStart";
            this.nud_PtzAreaHStart.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaHStart.TabIndex = 3;
            // 
            // lblAhe
            // 
            this.lblAhe.Location = new System.Drawing.Point(140, 55);
            this.lblAhe.Name = "lblAhe";
            this.lblAhe.Size = new System.Drawing.Size(60, 15);
            this.lblAhe.TabIndex = 4;
            this.lblAhe.Text = "水平终°:";
            // 
            // nud_PtzAreaHEnd
            // 
            this.nud_PtzAreaHEnd.DecimalPlaces = 2;
            this.nud_PtzAreaHEnd.Location = new System.Drawing.Point(200, 52);
            this.nud_PtzAreaHEnd.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nud_PtzAreaHEnd.Name = "nud_PtzAreaHEnd";
            this.nud_PtzAreaHEnd.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaHEnd.TabIndex = 5;
            // 
            // lblAhi
            // 
            this.lblAhi.Location = new System.Drawing.Point(270, 55);
            this.lblAhi.Name = "lblAhi";
            this.lblAhi.Size = new System.Drawing.Size(70, 15);
            this.lblAhi.TabIndex = 6;
            this.lblAhi.Text = "水平步进°:";
            // 
            // nud_PtzAreaHInterval
            // 
            this.nud_PtzAreaHInterval.DecimalPlaces = 2;
            this.nud_PtzAreaHInterval.Location = new System.Drawing.Point(345, 52);
            this.nud_PtzAreaHInterval.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nud_PtzAreaHInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_PtzAreaHInterval.Name = "nud_PtzAreaHInterval";
            this.nud_PtzAreaHInterval.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaHInterval.TabIndex = 7;
            this.nud_PtzAreaHInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            // 
            // lblAvs
            // 
            this.lblAvs.Location = new System.Drawing.Point(10, 85);
            this.lblAvs.Name = "lblAvs";
            this.lblAvs.Size = new System.Drawing.Size(60, 15);
            this.lblAvs.TabIndex = 8;
            this.lblAvs.Text = "垂直始°:";
            // 
            // nud_PtzAreaVStart
            // 
            this.nud_PtzAreaVStart.DecimalPlaces = 2;
            this.nud_PtzAreaVStart.Location = new System.Drawing.Point(70, 82);
            this.nud_PtzAreaVStart.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nud_PtzAreaVStart.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.nud_PtzAreaVStart.Name = "nud_PtzAreaVStart";
            this.nud_PtzAreaVStart.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaVStart.TabIndex = 9;
            // 
            // lblAve
            // 
            this.lblAve.Location = new System.Drawing.Point(140, 85);
            this.lblAve.Name = "lblAve";
            this.lblAve.Size = new System.Drawing.Size(60, 15);
            this.lblAve.TabIndex = 10;
            this.lblAve.Text = "垂直终°:";
            // 
            // nud_PtzAreaVEnd
            // 
            this.nud_PtzAreaVEnd.DecimalPlaces = 2;
            this.nud_PtzAreaVEnd.Location = new System.Drawing.Point(200, 82);
            this.nud_PtzAreaVEnd.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nud_PtzAreaVEnd.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.nud_PtzAreaVEnd.Name = "nud_PtzAreaVEnd";
            this.nud_PtzAreaVEnd.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaVEnd.TabIndex = 11;
            // 
            // lblAvi
            // 
            this.lblAvi.Location = new System.Drawing.Point(270, 85);
            this.lblAvi.Name = "lblAvi";
            this.lblAvi.Size = new System.Drawing.Size(70, 15);
            this.lblAvi.TabIndex = 12;
            this.lblAvi.Text = "垂直步进°:";
            // 
            // nud_PtzAreaVInterval
            // 
            this.nud_PtzAreaVInterval.DecimalPlaces = 2;
            this.nud_PtzAreaVInterval.Location = new System.Drawing.Point(345, 82);
            this.nud_PtzAreaVInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nud_PtzAreaVInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nud_PtzAreaVInterval.Name = "nud_PtzAreaVInterval";
            this.nud_PtzAreaVInterval.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaVInterval.TabIndex = 13;
            this.nud_PtzAreaVInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            // 
            // lblAtm
            // 
            this.lblAtm.Location = new System.Drawing.Point(10, 115);
            this.lblAtm.Name = "lblAtm";
            this.lblAtm.Size = new System.Drawing.Size(110, 15);
            this.lblAtm.TabIndex = 14;
            this.lblAtm.Text = "单步停止时间(ms):";
            // 
            // nud_PtzAreaTime
            // 
            this.nud_PtzAreaTime.Location = new System.Drawing.Point(125, 112);
            this.nud_PtzAreaTime.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.nud_PtzAreaTime.Name = "nud_PtzAreaTime";
            this.nud_PtzAreaTime.Size = new System.Drawing.Size(60, 21);
            this.nud_PtzAreaTime.TabIndex = 15;
            this.nud_PtzAreaTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // btnPtzAreaSetBound
            // 
            this.btnPtzAreaSetBound.Location = new System.Drawing.Point(200, 110);
            this.btnPtzAreaSetBound.Name = "btnPtzAreaSetBound";
            this.btnPtzAreaSetBound.Size = new System.Drawing.Size(100, 25);
            this.btnPtzAreaSetBound.TabIndex = 16;
            this.btnPtzAreaSetBound.Text = "写入配置边界";
            this.btnPtzAreaSetBound.Click += new System.EventHandler(this.btnPtzAreaSetBound_Click);
            // 
            // btnPtzAreaSetInterval
            // 
            this.btnPtzAreaSetInterval.Location = new System.Drawing.Point(305, 110);
            this.btnPtzAreaSetInterval.Name = "btnPtzAreaSetInterval";
            this.btnPtzAreaSetInterval.Size = new System.Drawing.Size(100, 25);
            this.btnPtzAreaSetInterval.TabIndex = 17;
            this.btnPtzAreaSetInterval.Text = "写入配置步进";
            this.btnPtzAreaSetInterval.Click += new System.EventHandler(this.btnPtzAreaSetInterval_Click);
            // 
            // lblAs
            // 
            this.lblAs.Location = new System.Drawing.Point(10, 155);
            this.lblAs.Name = "lblAs";
            this.lblAs.Size = new System.Drawing.Size(100, 15);
            this.lblAs.TabIndex = 18;
            this.lblAs.Text = "多区域循环：始于";
            // 
            // nud_PtzAreaStart
            // 
            this.nud_PtzAreaStart.Location = new System.Drawing.Point(110, 152);
            this.nud_PtzAreaStart.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.nud_PtzAreaStart.Name = "nud_PtzAreaStart";
            this.nud_PtzAreaStart.Size = new System.Drawing.Size(40, 21);
            this.nud_PtzAreaStart.TabIndex = 19;
            // 
            // lblAe
            // 
            this.lblAe.Location = new System.Drawing.Point(160, 155);
            this.lblAe.Name = "lblAe";
            this.lblAe.Size = new System.Drawing.Size(35, 15);
            this.lblAe.TabIndex = 20;
            this.lblAe.Text = "终于";
            // 
            // nud_PtzAreaEnd
            // 
            this.nud_PtzAreaEnd.Location = new System.Drawing.Point(195, 152);
            this.nud_PtzAreaEnd.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.nud_PtzAreaEnd.Name = "nud_PtzAreaEnd";
            this.nud_PtzAreaEnd.Size = new System.Drawing.Size(40, 21);
            this.nud_PtzAreaEnd.TabIndex = 21;
            // 
            // btnPtzAreaScanStart
            // 
            this.btnPtzAreaScanStart.Location = new System.Drawing.Point(10, 190);
            this.btnPtzAreaScanStart.Name = "btnPtzAreaScanStart";
            this.btnPtzAreaScanStart.Size = new System.Drawing.Size(180, 35);
            this.btnPtzAreaScanStart.TabIndex = 22;
            this.btnPtzAreaScanStart.Text = "开启该区/多区扫描";
            this.btnPtzAreaScanStart.Click += new System.EventHandler(this.btnPtzAreaScanStart_Click);
            // 
            // btnPtzAreaScanStop
            // 
            this.btnPtzAreaScanStop.Location = new System.Drawing.Point(210, 190);
            this.btnPtzAreaScanStop.Name = "btnPtzAreaScanStop";
            this.btnPtzAreaScanStop.Size = new System.Drawing.Size(180, 35);
            this.btnPtzAreaScanStop.TabIndex = 23;
            this.btnPtzAreaScanStop.Text = "彻底关闭区域扫描";
            this.btnPtzAreaScanStop.Click += new System.EventHandler(this.btnPtzAreaScanStop_Click);
            // 
            // gbxPtzLocate
            // 
            this.gbxPtzLocate.Controls.Add(this.lblHA);
            this.gbxPtzLocate.Controls.Add(this.nud_PtzHAngle);
            this.gbxPtzLocate.Controls.Add(this.lblVA);
            this.gbxPtzLocate.Controls.Add(this.nud_PtzVAngle);
            this.gbxPtzLocate.Controls.Add(this.chk_PtzUseSpeedLocate);
            this.gbxPtzLocate.Controls.Add(this.btnPtzLocate);
            this.gbxPtzLocate.Location = new System.Drawing.Point(890, 10);
            this.gbxPtzLocate.Name = "gbxPtzLocate";
            this.gbxPtzLocate.Size = new System.Drawing.Size(500, 330);
            this.gbxPtzLocate.TabIndex = 2;
            this.gbxPtzLocate.TabStop = false;
            this.gbxPtzLocate.Text = "精确绝对角度定位";
            // 
            // lblHA
            // 
            this.lblHA.Location = new System.Drawing.Point(10, 25);
            this.lblHA.Name = "lblHA";
            this.lblHA.Size = new System.Drawing.Size(70, 15);
            this.lblHA.TabIndex = 0;
            this.lblHA.Text = "目标水平°:";
            // 
            // nud_PtzHAngle
            // 
            this.nud_PtzHAngle.DecimalPlaces = 2;
            this.nud_PtzHAngle.Location = new System.Drawing.Point(85, 22);
            this.nud_PtzHAngle.Maximum = new decimal(new int[] {
            36000,
            0,
            0,
            131072});
            this.nud_PtzHAngle.Name = "nud_PtzHAngle";
            this.nud_PtzHAngle.Size = new System.Drawing.Size(70, 21);
            this.nud_PtzHAngle.TabIndex = 1;
            // 
            // lblVA
            // 
            this.lblVA.Location = new System.Drawing.Point(10, 55);
            this.lblVA.Name = "lblVA";
            this.lblVA.Size = new System.Drawing.Size(70, 15);
            this.lblVA.TabIndex = 2;
            this.lblVA.Text = "目标垂直°:";
            // 
            // nud_PtzVAngle
            // 
            this.nud_PtzVAngle.DecimalPlaces = 2;
            this.nud_PtzVAngle.Location = new System.Drawing.Point(85, 52);
            this.nud_PtzVAngle.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            131072});
            this.nud_PtzVAngle.Minimum = new decimal(new int[] {
            6000,
            0,
            0,
            -2147352576});
            this.nud_PtzVAngle.Name = "nud_PtzVAngle";
            this.nud_PtzVAngle.Size = new System.Drawing.Size(70, 21);
            this.nud_PtzVAngle.TabIndex = 3;
            // 
            // chk_PtzUseSpeedLocate
            // 
            this.chk_PtzUseSpeedLocate.Location = new System.Drawing.Point(10, 85);
            this.chk_PtzUseSpeedLocate.Name = "chk_PtzUseSpeedLocate";
            this.chk_PtzUseSpeedLocate.Size = new System.Drawing.Size(180, 20);
            this.chk_PtzUseSpeedLocate.TabIndex = 4;
            this.chk_PtzUseSpeedLocate.Text = "启用自定义速度定位";
            // 
            // btnPtzLocate
            // 
            this.btnPtzLocate.Location = new System.Drawing.Point(10, 110);
            this.btnPtzLocate.Name = "btnPtzLocate";
            this.btnPtzLocate.Size = new System.Drawing.Size(190, 30);
            this.btnPtzLocate.TabIndex = 5;
            this.btnPtzLocate.Text = "执行定位控制";
            this.btnPtzLocate.Click += new System.EventHandler(this.btnPtzLocate_Click);
            // 
            // gbxPtzExtPreset
            // 
            this.gbxPtzExtPreset.Controls.Add(this.flpPtzSupplementPreset);
            this.gbxPtzExtPreset.Location = new System.Drawing.Point(890, 350);
            this.gbxPtzExtPreset.Name = "gbxPtzExtPreset";
            this.gbxPtzExtPreset.Size = new System.Drawing.Size(500, 350);
            this.gbxPtzExtPreset.TabIndex = 7;
            this.gbxPtzExtPreset.TabStop = false;
            this.gbxPtzExtPreset.Text = "预置位高级";
            // 
            // flpPtzSupplementPreset
            // 
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetSetByAngle);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetSetHAngle);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetSetVAngle);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetSetTime);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetSetSpeed);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetPause);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetContinue);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetEndReturnOn);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetEndReturnOff);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetArriveReturnOn);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetArriveReturnOff);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetCallReturnOn);
            this.flpPtzSupplementPreset.Controls.Add(this.btnPtzSupPresetCallReturnOff);
            this.flpPtzSupplementPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPtzSupplementPreset.Location = new System.Drawing.Point(3, 17);
            this.flpPtzSupplementPreset.Name = "flpPtzSupplementPreset";
            this.flpPtzSupplementPreset.Padding = new System.Windows.Forms.Padding(8);
            this.flpPtzSupplementPreset.Size = new System.Drawing.Size(494, 330);
            this.flpPtzSupplementPreset.TabIndex = 0;
            // 
            // btnPtzSupPresetSetByAngle
            // 
            this.btnPtzSupPresetSetByAngle.AutoEllipsis = true;
            this.btnPtzSupPresetSetByAngle.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupPresetSetByAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetSetByAngle.Name = "btnPtzSupPresetSetByAngle";
            this.btnPtzSupPresetSetByAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetSetByAngle.TabIndex = 0;
            this.btnPtzSupPresetSetByAngle.Tag = "preset_set_by_angle";
            this.btnPtzSupPresetSetByAngle.Text = "按角度设预置";
            this.btnPtzSupPresetSetByAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetSetByAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetSetHAngle
            // 
            this.btnPtzSupPresetSetHAngle.AutoEllipsis = true;
            this.btnPtzSupPresetSetHAngle.Location = new System.Drawing.Point(145, 12);
            this.btnPtzSupPresetSetHAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetSetHAngle.Name = "btnPtzSupPresetSetHAngle";
            this.btnPtzSupPresetSetHAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetSetHAngle.TabIndex = 0;
            this.btnPtzSupPresetSetHAngle.Tag = "preset_set_h_angle";
            this.btnPtzSupPresetSetHAngle.Text = "水平角设预置";
            this.btnPtzSupPresetSetHAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetSetHAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetSetVAngle
            // 
            this.btnPtzSupPresetSetVAngle.AutoEllipsis = true;
            this.btnPtzSupPresetSetVAngle.Location = new System.Drawing.Point(278, 12);
            this.btnPtzSupPresetSetVAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetSetVAngle.Name = "btnPtzSupPresetSetVAngle";
            this.btnPtzSupPresetSetVAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetSetVAngle.TabIndex = 0;
            this.btnPtzSupPresetSetVAngle.Tag = "preset_set_v_angle";
            this.btnPtzSupPresetSetVAngle.Text = "垂直角设预置";
            this.btnPtzSupPresetSetVAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetSetVAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetSetTime
            // 
            this.btnPtzSupPresetSetTime.AutoEllipsis = true;
            this.btnPtzSupPresetSetTime.Location = new System.Drawing.Point(12, 48);
            this.btnPtzSupPresetSetTime.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetSetTime.Name = "btnPtzSupPresetSetTime";
            this.btnPtzSupPresetSetTime.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetSetTime.TabIndex = 0;
            this.btnPtzSupPresetSetTime.Tag = "preset_set_time";
            this.btnPtzSupPresetSetTime.Text = "写入驻留时间";
            this.btnPtzSupPresetSetTime.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetSetTime.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetSetSpeed
            // 
            this.btnPtzSupPresetSetSpeed.AutoEllipsis = true;
            this.btnPtzSupPresetSetSpeed.Location = new System.Drawing.Point(145, 48);
            this.btnPtzSupPresetSetSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetSetSpeed.Name = "btnPtzSupPresetSetSpeed";
            this.btnPtzSupPresetSetSpeed.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetSetSpeed.TabIndex = 0;
            this.btnPtzSupPresetSetSpeed.Tag = "preset_set_speed";
            this.btnPtzSupPresetSetSpeed.Text = " 写入扫描速度";
            this.btnPtzSupPresetSetSpeed.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetSetSpeed.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetPause
            // 
            this.btnPtzSupPresetPause.AutoEllipsis = true;
            this.btnPtzSupPresetPause.Location = new System.Drawing.Point(278, 48);
            this.btnPtzSupPresetPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetPause.Name = "btnPtzSupPresetPause";
            this.btnPtzSupPresetPause.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetPause.TabIndex = 0;
            this.btnPtzSupPresetPause.Tag = "preset_pause";
            this.btnPtzSupPresetPause.Text = "暂停预置扫描";
            this.btnPtzSupPresetPause.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetPause.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetContinue
            // 
            this.btnPtzSupPresetContinue.AutoEllipsis = true;
            this.btnPtzSupPresetContinue.Location = new System.Drawing.Point(12, 84);
            this.btnPtzSupPresetContinue.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetContinue.Name = "btnPtzSupPresetContinue";
            this.btnPtzSupPresetContinue.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetContinue.TabIndex = 0;
            this.btnPtzSupPresetContinue.Tag = "preset_continue";
            this.btnPtzSupPresetContinue.Text = "恢复预置扫描";
            this.btnPtzSupPresetContinue.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetContinue.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetEndReturnOn
            // 
            this.btnPtzSupPresetEndReturnOn.AutoEllipsis = true;
            this.btnPtzSupPresetEndReturnOn.Location = new System.Drawing.Point(145, 84);
            this.btnPtzSupPresetEndReturnOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetEndReturnOn.Name = "btnPtzSupPresetEndReturnOn";
            this.btnPtzSupPresetEndReturnOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetEndReturnOn.TabIndex = 0;
            this.btnPtzSupPresetEndReturnOn.Tag = "preset_end_return_on";
            this.btnPtzSupPresetEndReturnOn.Text = " 扫描结束回传开";
            this.btnPtzSupPresetEndReturnOn.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetEndReturnOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetEndReturnOff
            // 
            this.btnPtzSupPresetEndReturnOff.AutoEllipsis = true;
            this.btnPtzSupPresetEndReturnOff.Location = new System.Drawing.Point(278, 84);
            this.btnPtzSupPresetEndReturnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetEndReturnOff.Name = "btnPtzSupPresetEndReturnOff";
            this.btnPtzSupPresetEndReturnOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetEndReturnOff.TabIndex = 0;
            this.btnPtzSupPresetEndReturnOff.Tag = "preset_end_return_off";
            this.btnPtzSupPresetEndReturnOff.Text = " 扫描结束回传关";
            this.btnPtzSupPresetEndReturnOff.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetEndReturnOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetArriveReturnOn
            // 
            this.btnPtzSupPresetArriveReturnOn.AutoEllipsis = true;
            this.btnPtzSupPresetArriveReturnOn.Location = new System.Drawing.Point(12, 120);
            this.btnPtzSupPresetArriveReturnOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetArriveReturnOn.Name = "btnPtzSupPresetArriveReturnOn";
            this.btnPtzSupPresetArriveReturnOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetArriveReturnOn.TabIndex = 0;
            this.btnPtzSupPresetArriveReturnOn.Tag = "preset_arrive_return_on";
            this.btnPtzSupPresetArriveReturnOn.Text = "扫描到位回传开";
            this.btnPtzSupPresetArriveReturnOn.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetArriveReturnOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetArriveReturnOff
            // 
            this.btnPtzSupPresetArriveReturnOff.AutoEllipsis = true;
            this.btnPtzSupPresetArriveReturnOff.Location = new System.Drawing.Point(145, 120);
            this.btnPtzSupPresetArriveReturnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetArriveReturnOff.Name = "btnPtzSupPresetArriveReturnOff";
            this.btnPtzSupPresetArriveReturnOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetArriveReturnOff.TabIndex = 0;
            this.btnPtzSupPresetArriveReturnOff.Tag = "preset_arrive_return_off";
            this.btnPtzSupPresetArriveReturnOff.Text = " 扫描到位回传关";
            this.btnPtzSupPresetArriveReturnOff.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetArriveReturnOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetCallReturnOn
            // 
            this.btnPtzSupPresetCallReturnOn.AutoEllipsis = true;
            this.btnPtzSupPresetCallReturnOn.Location = new System.Drawing.Point(278, 120);
            this.btnPtzSupPresetCallReturnOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetCallReturnOn.Name = "btnPtzSupPresetCallReturnOn";
            this.btnPtzSupPresetCallReturnOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetCallReturnOn.TabIndex = 0;
            this.btnPtzSupPresetCallReturnOn.Tag = "preset_call_return_on";
            this.btnPtzSupPresetCallReturnOn.Text = "调用到位回传开";
            this.btnPtzSupPresetCallReturnOn.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetCallReturnOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetCallReturnOff
            // 
            this.btnPtzSupPresetCallReturnOff.AutoEllipsis = true;
            this.btnPtzSupPresetCallReturnOff.Location = new System.Drawing.Point(12, 156);
            this.btnPtzSupPresetCallReturnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetCallReturnOff.Name = "btnPtzSupPresetCallReturnOff";
            this.btnPtzSupPresetCallReturnOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetCallReturnOff.TabIndex = 0;
            this.btnPtzSupPresetCallReturnOff.Tag = "preset_call_return_off";
            this.btnPtzSupPresetCallReturnOff.Text = "调用到位回传关";
            this.btnPtzSupPresetCallReturnOff.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetCallReturnOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // gbxPtzExtArea
            // 
            this.gbxPtzExtArea.Controls.Add(this.flpPtzSupplementArea);
            this.gbxPtzExtArea.Location = new System.Drawing.Point(1400, 350);
            this.gbxPtzExtArea.Name = "gbxPtzExtArea";
            this.gbxPtzExtArea.Size = new System.Drawing.Size(635, 400);
            this.gbxPtzExtArea.TabIndex = 8;
            this.gbxPtzExtArea.TabStop = false;
            this.gbxPtzExtArea.Text = "区域扫描";
            // 
            // flpPtzSupplementArea
            // 
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaVideoHa);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaVideoHb);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaVideoVa);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaVideoVb);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaSetSpeed);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaSetTime);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaEnable);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaDisable);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaStartSingle);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaPause);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaContinue);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaModeStep);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaModeContinuous);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaSave);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaQuery);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaEndReturnOn);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaEndReturnOff);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaStepReturnOn);
            this.flpPtzSupplementArea.Controls.Add(this.btnPtzSupAreaStepReturnOff);
            this.flpPtzSupplementArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPtzSupplementArea.Location = new System.Drawing.Point(3, 17);
            this.flpPtzSupplementArea.Name = "flpPtzSupplementArea";
            this.flpPtzSupplementArea.Padding = new System.Windows.Forms.Padding(8);
            this.flpPtzSupplementArea.Size = new System.Drawing.Size(629, 380);
            this.flpPtzSupplementArea.TabIndex = 0;
            // 
            // btnPtzSupAreaVideoHa
            // 
            this.btnPtzSupAreaVideoHa.AutoEllipsis = true;
            this.btnPtzSupAreaVideoHa.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupAreaVideoHa.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaVideoHa.Name = "btnPtzSupAreaVideoHa";
            this.btnPtzSupAreaVideoHa.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaVideoHa.TabIndex = 0;
            this.btnPtzSupAreaVideoHa.Tag = "area_video_ha";
            this.btnPtzSupAreaVideoHa.Text = "当前水平->HA";
            this.btnPtzSupAreaVideoHa.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaVideoHa.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaVideoHb
            // 
            this.btnPtzSupAreaVideoHb.AutoEllipsis = true;
            this.btnPtzSupAreaVideoHb.Location = new System.Drawing.Point(145, 12);
            this.btnPtzSupAreaVideoHb.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaVideoHb.Name = "btnPtzSupAreaVideoHb";
            this.btnPtzSupAreaVideoHb.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaVideoHb.TabIndex = 0;
            this.btnPtzSupAreaVideoHb.Tag = "area_video_hb";
            this.btnPtzSupAreaVideoHb.Text = "当前水平->HB";
            this.btnPtzSupAreaVideoHb.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaVideoHb.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaVideoVa
            // 
            this.btnPtzSupAreaVideoVa.AutoEllipsis = true;
            this.btnPtzSupAreaVideoVa.Location = new System.Drawing.Point(278, 12);
            this.btnPtzSupAreaVideoVa.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaVideoVa.Name = "btnPtzSupAreaVideoVa";
            this.btnPtzSupAreaVideoVa.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaVideoVa.TabIndex = 0;
            this.btnPtzSupAreaVideoVa.Tag = "area_video_va";
            this.btnPtzSupAreaVideoVa.Text = "当前垂直->VA";
            this.btnPtzSupAreaVideoVa.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaVideoVa.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaVideoVb
            // 
            this.btnPtzSupAreaVideoVb.AutoEllipsis = true;
            this.btnPtzSupAreaVideoVb.Location = new System.Drawing.Point(411, 12);
            this.btnPtzSupAreaVideoVb.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaVideoVb.Name = "btnPtzSupAreaVideoVb";
            this.btnPtzSupAreaVideoVb.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaVideoVb.TabIndex = 0;
            this.btnPtzSupAreaVideoVb.Tag = "area_video_vb";
            this.btnPtzSupAreaVideoVb.Text = "当前垂直->VB";
            this.btnPtzSupAreaVideoVb.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaVideoVb.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaSetSpeed
            // 
            this.btnPtzSupAreaSetSpeed.AutoEllipsis = true;
            this.btnPtzSupAreaSetSpeed.Location = new System.Drawing.Point(12, 48);
            this.btnPtzSupAreaSetSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaSetSpeed.Name = "btnPtzSupAreaSetSpeed";
            this.btnPtzSupAreaSetSpeed.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaSetSpeed.TabIndex = 0;
            this.btnPtzSupAreaSetSpeed.Tag = "area_set_speed";
            this.btnPtzSupAreaSetSpeed.Text = "写入区域转速";
            this.btnPtzSupAreaSetSpeed.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaSetSpeed.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaSetTime
            // 
            this.btnPtzSupAreaSetTime.AutoEllipsis = true;
            this.btnPtzSupAreaSetTime.Location = new System.Drawing.Point(145, 48);
            this.btnPtzSupAreaSetTime.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaSetTime.Name = "btnPtzSupAreaSetTime";
            this.btnPtzSupAreaSetTime.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaSetTime.TabIndex = 0;
            this.btnPtzSupAreaSetTime.Tag = "area_set_time";
            this.btnPtzSupAreaSetTime.Text = "写入区域停留";
            this.btnPtzSupAreaSetTime.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaSetTime.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaEnable
            // 
            this.btnPtzSupAreaEnable.AutoEllipsis = true;
            this.btnPtzSupAreaEnable.Location = new System.Drawing.Point(278, 48);
            this.btnPtzSupAreaEnable.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaEnable.Name = "btnPtzSupAreaEnable";
            this.btnPtzSupAreaEnable.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaEnable.TabIndex = 0;
            this.btnPtzSupAreaEnable.Tag = "area_enable";
            this.btnPtzSupAreaEnable.Text = "使能区域";
            this.btnPtzSupAreaEnable.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaEnable.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaDisable
            // 
            this.btnPtzSupAreaDisable.AutoEllipsis = true;
            this.btnPtzSupAreaDisable.Location = new System.Drawing.Point(411, 48);
            this.btnPtzSupAreaDisable.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaDisable.Name = "btnPtzSupAreaDisable";
            this.btnPtzSupAreaDisable.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaDisable.TabIndex = 0;
            this.btnPtzSupAreaDisable.Tag = "area_disable";
            this.btnPtzSupAreaDisable.Text = " 禁用区域";
            this.btnPtzSupAreaDisable.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaDisable.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaStartSingle
            // 
            this.btnPtzSupAreaStartSingle.AutoEllipsis = true;
            this.btnPtzSupAreaStartSingle.Location = new System.Drawing.Point(12, 84);
            this.btnPtzSupAreaStartSingle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaStartSingle.Name = "btnPtzSupAreaStartSingle";
            this.btnPtzSupAreaStartSingle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaStartSingle.TabIndex = 0;
            this.btnPtzSupAreaStartSingle.Tag = "area_start_single";
            this.btnPtzSupAreaStartSingle.Text = "单区域扫描";
            this.btnPtzSupAreaStartSingle.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaStartSingle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaPause
            // 
            this.btnPtzSupAreaPause.AutoEllipsis = true;
            this.btnPtzSupAreaPause.Location = new System.Drawing.Point(145, 84);
            this.btnPtzSupAreaPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaPause.Name = "btnPtzSupAreaPause";
            this.btnPtzSupAreaPause.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaPause.TabIndex = 0;
            this.btnPtzSupAreaPause.Tag = "area_pause";
            this.btnPtzSupAreaPause.Text = "暂停扫描";
            this.btnPtzSupAreaPause.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaPause.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaContinue
            // 
            this.btnPtzSupAreaContinue.AutoEllipsis = true;
            this.btnPtzSupAreaContinue.Location = new System.Drawing.Point(278, 84);
            this.btnPtzSupAreaContinue.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaContinue.Name = "btnPtzSupAreaContinue";
            this.btnPtzSupAreaContinue.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaContinue.TabIndex = 0;
            this.btnPtzSupAreaContinue.Tag = "area_continue";
            this.btnPtzSupAreaContinue.Text = "恢复扫描";
            this.btnPtzSupAreaContinue.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaContinue.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaModeStep
            // 
            this.btnPtzSupAreaModeStep.AutoEllipsis = true;
            this.btnPtzSupAreaModeStep.Location = new System.Drawing.Point(411, 84);
            this.btnPtzSupAreaModeStep.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaModeStep.Name = "btnPtzSupAreaModeStep";
            this.btnPtzSupAreaModeStep.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaModeStep.TabIndex = 0;
            this.btnPtzSupAreaModeStep.Tag = "area_mode_step";
            this.btnPtzSupAreaModeStep.Text = "单步模式";
            this.btnPtzSupAreaModeStep.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaModeStep.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaModeContinuous
            // 
            this.btnPtzSupAreaModeContinuous.AutoEllipsis = true;
            this.btnPtzSupAreaModeContinuous.Location = new System.Drawing.Point(12, 120);
            this.btnPtzSupAreaModeContinuous.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaModeContinuous.Name = "btnPtzSupAreaModeContinuous";
            this.btnPtzSupAreaModeContinuous.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaModeContinuous.TabIndex = 0;
            this.btnPtzSupAreaModeContinuous.Tag = "area_mode_continuous";
            this.btnPtzSupAreaModeContinuous.Text = "连续模式";
            this.btnPtzSupAreaModeContinuous.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaModeContinuous.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaSave
            // 
            this.btnPtzSupAreaSave.AutoEllipsis = true;
            this.btnPtzSupAreaSave.Location = new System.Drawing.Point(145, 120);
            this.btnPtzSupAreaSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaSave.Name = "btnPtzSupAreaSave";
            this.btnPtzSupAreaSave.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaSave.TabIndex = 0;
            this.btnPtzSupAreaSave.Tag = "area_save";
            this.btnPtzSupAreaSave.Text = "保存区域数据";
            this.btnPtzSupAreaSave.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaSave.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaQuery
            // 
            this.btnPtzSupAreaQuery.AutoEllipsis = true;
            this.btnPtzSupAreaQuery.Location = new System.Drawing.Point(278, 120);
            this.btnPtzSupAreaQuery.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaQuery.Name = "btnPtzSupAreaQuery";
            this.btnPtzSupAreaQuery.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaQuery.TabIndex = 0;
            this.btnPtzSupAreaQuery.Tag = "area_query";
            this.btnPtzSupAreaQuery.Text = "查询区域配置";
            this.btnPtzSupAreaQuery.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaQuery.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaEndReturnOn
            // 
            this.btnPtzSupAreaEndReturnOn.AutoEllipsis = true;
            this.btnPtzSupAreaEndReturnOn.Location = new System.Drawing.Point(411, 120);
            this.btnPtzSupAreaEndReturnOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaEndReturnOn.Name = "btnPtzSupAreaEndReturnOn";
            this.btnPtzSupAreaEndReturnOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaEndReturnOn.TabIndex = 0;
            this.btnPtzSupAreaEndReturnOn.Tag = "area_end_return_on";
            this.btnPtzSupAreaEndReturnOn.Text = " 区域结束回传开";
            this.btnPtzSupAreaEndReturnOn.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaEndReturnOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaEndReturnOff
            // 
            this.btnPtzSupAreaEndReturnOff.AutoEllipsis = true;
            this.btnPtzSupAreaEndReturnOff.Location = new System.Drawing.Point(12, 156);
            this.btnPtzSupAreaEndReturnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaEndReturnOff.Name = "btnPtzSupAreaEndReturnOff";
            this.btnPtzSupAreaEndReturnOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaEndReturnOff.TabIndex = 0;
            this.btnPtzSupAreaEndReturnOff.Tag = "area_end_return_off";
            this.btnPtzSupAreaEndReturnOff.Text = "区域结束回传关";
            this.btnPtzSupAreaEndReturnOff.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaEndReturnOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaStepReturnOn
            // 
            this.btnPtzSupAreaStepReturnOn.AutoEllipsis = true;
            this.btnPtzSupAreaStepReturnOn.Location = new System.Drawing.Point(145, 156);
            this.btnPtzSupAreaStepReturnOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaStepReturnOn.Name = "btnPtzSupAreaStepReturnOn";
            this.btnPtzSupAreaStepReturnOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaStepReturnOn.TabIndex = 0;
            this.btnPtzSupAreaStepReturnOn.Tag = "area_step_return_on";
            this.btnPtzSupAreaStepReturnOn.Text = "单步到位回传开";
            this.btnPtzSupAreaStepReturnOn.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaStepReturnOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaStepReturnOff
            // 
            this.btnPtzSupAreaStepReturnOff.AutoEllipsis = true;
            this.btnPtzSupAreaStepReturnOff.Location = new System.Drawing.Point(278, 156);
            this.btnPtzSupAreaStepReturnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaStepReturnOff.Name = "btnPtzSupAreaStepReturnOff";
            this.btnPtzSupAreaStepReturnOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaStepReturnOff.TabIndex = 0;
            this.btnPtzSupAreaStepReturnOff.Tag = "area_step_return_off";
            this.btnPtzSupAreaStepReturnOff.Text = "单步到位回传关";
            this.btnPtzSupAreaStepReturnOff.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaStepReturnOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // gbxPtzExtMaintenance
            // 
            this.gbxPtzExtMaintenance.Controls.Add(this.flpPtzSupplementZero);
            this.gbxPtzExtMaintenance.Location = new System.Drawing.Point(10, 610);
            this.gbxPtzExtMaintenance.Name = "gbxPtzExtMaintenance";
            this.gbxPtzExtMaintenance.Size = new System.Drawing.Size(430, 170);
            this.gbxPtzExtMaintenance.TabIndex = 9;
            this.gbxPtzExtMaintenance.TabStop = false;
            this.gbxPtzExtMaintenance.Text = "零位、校准与维护";
            // 
            // flpPtzSupplementZero
            // 
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupZeroHCurrent);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupZeroVCurrent);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupZeroHvCurrent);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupZeroHAngle);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupZeroVAngle);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupZeroDelete);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupReboot);
            this.flpPtzSupplementZero.Controls.Add(this.btnPtzSupSelfCheck);
            this.flpPtzSupplementZero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPtzSupplementZero.Location = new System.Drawing.Point(3, 17);
            this.flpPtzSupplementZero.Name = "flpPtzSupplementZero";
            this.flpPtzSupplementZero.Padding = new System.Windows.Forms.Padding(8);
            this.flpPtzSupplementZero.Size = new System.Drawing.Size(424, 150);
            this.flpPtzSupplementZero.TabIndex = 0;
            // 
            // btnPtzSupZeroHCurrent
            // 
            this.btnPtzSupZeroHCurrent.AutoEllipsis = true;
            this.btnPtzSupZeroHCurrent.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupZeroHCurrent.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupZeroHCurrent.Name = "btnPtzSupZeroHCurrent";
            this.btnPtzSupZeroHCurrent.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupZeroHCurrent.TabIndex = 0;
            this.btnPtzSupZeroHCurrent.Tag = "zero_h_current";
            this.btnPtzSupZeroHCurrent.Text = " 当前水平设0";
            this.btnPtzSupZeroHCurrent.UseVisualStyleBackColor = true;
            this.btnPtzSupZeroHCurrent.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupZeroVCurrent
            // 
            this.btnPtzSupZeroVCurrent.AutoEllipsis = true;
            this.btnPtzSupZeroVCurrent.Location = new System.Drawing.Point(145, 12);
            this.btnPtzSupZeroVCurrent.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupZeroVCurrent.Name = "btnPtzSupZeroVCurrent";
            this.btnPtzSupZeroVCurrent.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupZeroVCurrent.TabIndex = 0;
            this.btnPtzSupZeroVCurrent.Tag = "zero_v_current";
            this.btnPtzSupZeroVCurrent.Text = "当前垂直设0";
            this.btnPtzSupZeroVCurrent.UseVisualStyleBackColor = true;
            this.btnPtzSupZeroVCurrent.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupZeroHvCurrent
            // 
            this.btnPtzSupZeroHvCurrent.AutoEllipsis = true;
            this.btnPtzSupZeroHvCurrent.Location = new System.Drawing.Point(278, 12);
            this.btnPtzSupZeroHvCurrent.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupZeroHvCurrent.Name = "btnPtzSupZeroHvCurrent";
            this.btnPtzSupZeroHvCurrent.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupZeroHvCurrent.TabIndex = 0;
            this.btnPtzSupZeroHvCurrent.Tag = "zero_hv_current";
            this.btnPtzSupZeroHvCurrent.Text = " 当前H/V设0";
            this.btnPtzSupZeroHvCurrent.UseVisualStyleBackColor = true;
            this.btnPtzSupZeroHvCurrent.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupZeroHAngle
            // 
            this.btnPtzSupZeroHAngle.AutoEllipsis = true;
            this.btnPtzSupZeroHAngle.Location = new System.Drawing.Point(12, 48);
            this.btnPtzSupZeroHAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupZeroHAngle.Name = "btnPtzSupZeroHAngle";
            this.btnPtzSupZeroHAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupZeroHAngle.TabIndex = 0;
            this.btnPtzSupZeroHAngle.Tag = "zero_h_angle";
            this.btnPtzSupZeroHAngle.Text = "按水平角设0";
            this.btnPtzSupZeroHAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupZeroHAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupZeroVAngle
            // 
            this.btnPtzSupZeroVAngle.AutoEllipsis = true;
            this.btnPtzSupZeroVAngle.Location = new System.Drawing.Point(145, 48);
            this.btnPtzSupZeroVAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupZeroVAngle.Name = "btnPtzSupZeroVAngle";
            this.btnPtzSupZeroVAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupZeroVAngle.TabIndex = 0;
            this.btnPtzSupZeroVAngle.Tag = "zero_v_angle";
            this.btnPtzSupZeroVAngle.Text = "按垂直角设0";
            this.btnPtzSupZeroVAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupZeroVAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupZeroDelete
            // 
            this.btnPtzSupZeroDelete.AutoEllipsis = true;
            this.btnPtzSupZeroDelete.Location = new System.Drawing.Point(278, 48);
            this.btnPtzSupZeroDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupZeroDelete.Name = "btnPtzSupZeroDelete";
            this.btnPtzSupZeroDelete.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupZeroDelete.TabIndex = 0;
            this.btnPtzSupZeroDelete.Tag = "zero_delete";
            this.btnPtzSupZeroDelete.Text = " 删除H/V零位";
            this.btnPtzSupZeroDelete.UseVisualStyleBackColor = true;
            this.btnPtzSupZeroDelete.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupReboot
            // 
            this.btnPtzSupReboot.AutoEllipsis = true;
            this.btnPtzSupReboot.Location = new System.Drawing.Point(12, 84);
            this.btnPtzSupReboot.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupReboot.Name = "btnPtzSupReboot";
            this.btnPtzSupReboot.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupReboot.TabIndex = 0;
            this.btnPtzSupReboot.Tag = "reboot";
            this.btnPtzSupReboot.Text = " 复位重启";
            this.btnPtzSupReboot.UseVisualStyleBackColor = true;
            this.btnPtzSupReboot.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupSelfCheck
            // 
            this.btnPtzSupSelfCheck.AutoEllipsis = true;
            this.btnPtzSupSelfCheck.Location = new System.Drawing.Point(145, 84);
            this.btnPtzSupSelfCheck.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupSelfCheck.Name = "btnPtzSupSelfCheck";
            this.btnPtzSupSelfCheck.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupSelfCheck.TabIndex = 0;
            this.btnPtzSupSelfCheck.Tag = "self_check";
            this.btnPtzSupSelfCheck.Text = "全范围自检";
            this.btnPtzSupSelfCheck.UseVisualStyleBackColor = true;
            this.btnPtzSupSelfCheck.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // gbxPtzExtBasic
            // 
            this.gbxPtzExtBasic.Controls.Add(this.flpPtzSupplementBasic);
            this.gbxPtzExtBasic.Location = new System.Drawing.Point(1400, 10);
            this.gbxPtzExtBasic.Name = "gbxPtzExtBasic";
            this.gbxPtzExtBasic.Size = new System.Drawing.Size(300, 330);
            this.gbxPtzExtBasic.TabIndex = 10;
            this.gbxPtzExtBasic.TabStop = false;
            this.gbxPtzExtBasic.Text = "扩展运动与设备";
            // 
            // flpPtzSupplementBasic
            // 
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupLocateH);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupLocateV);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupQueryHAngle);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupQueryVAngle);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupPower1On);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupPower2On);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupPower1Off);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupPower2Off);
            this.flpPtzSupplementBasic.Controls.Add(this.btnPtzSupReturnZero);
            this.flpPtzSupplementBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPtzSupplementBasic.Location = new System.Drawing.Point(3, 17);
            this.flpPtzSupplementBasic.Name = "flpPtzSupplementBasic";
            this.flpPtzSupplementBasic.Padding = new System.Windows.Forms.Padding(8);
            this.flpPtzSupplementBasic.Size = new System.Drawing.Size(294, 310);
            this.flpPtzSupplementBasic.TabIndex = 0;
            // 
            // btnPtzSupLocateH
            // 
            this.btnPtzSupLocateH.AutoEllipsis = true;
            this.btnPtzSupLocateH.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupLocateH.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupLocateH.Name = "btnPtzSupLocateH";
            this.btnPtzSupLocateH.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupLocateH.TabIndex = 0;
            this.btnPtzSupLocateH.Tag = "locate_h";
            this.btnPtzSupLocateH.Text = "水平定位";
            this.btnPtzSupLocateH.UseVisualStyleBackColor = true;
            this.btnPtzSupLocateH.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupLocateV
            // 
            this.btnPtzSupLocateV.AutoEllipsis = true;
            this.btnPtzSupLocateV.Location = new System.Drawing.Point(145, 12);
            this.btnPtzSupLocateV.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupLocateV.Name = "btnPtzSupLocateV";
            this.btnPtzSupLocateV.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupLocateV.TabIndex = 0;
            this.btnPtzSupLocateV.Tag = "locate_v";
            this.btnPtzSupLocateV.Text = "垂直定位";
            this.btnPtzSupLocateV.UseVisualStyleBackColor = true;
            this.btnPtzSupLocateV.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryHAngle
            // 
            this.btnPtzSupQueryHAngle.AutoEllipsis = true;
            this.btnPtzSupQueryHAngle.Location = new System.Drawing.Point(12, 48);
            this.btnPtzSupQueryHAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryHAngle.Name = "btnPtzSupQueryHAngle";
            this.btnPtzSupQueryHAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryHAngle.TabIndex = 0;
            this.btnPtzSupQueryHAngle.Tag = "query_h_angle";
            this.btnPtzSupQueryHAngle.Text = " 查询水平角";
            this.btnPtzSupQueryHAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryHAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryVAngle
            // 
            this.btnPtzSupQueryVAngle.AutoEllipsis = true;
            this.btnPtzSupQueryVAngle.Location = new System.Drawing.Point(145, 48);
            this.btnPtzSupQueryVAngle.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryVAngle.Name = "btnPtzSupQueryVAngle";
            this.btnPtzSupQueryVAngle.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryVAngle.TabIndex = 0;
            this.btnPtzSupQueryVAngle.Tag = "query_v_angle";
            this.btnPtzSupQueryVAngle.Text = "查询垂直角";
            this.btnPtzSupQueryVAngle.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryVAngle.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPower1On
            // 
            this.btnPtzSupPower1On.AutoEllipsis = true;
            this.btnPtzSupPower1On.Location = new System.Drawing.Point(12, 84);
            this.btnPtzSupPower1On.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPower1On.Name = "btnPtzSupPower1On";
            this.btnPtzSupPower1On.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPower1On.TabIndex = 0;
            this.btnPtzSupPower1On.Tag = "power_1_on";
            this.btnPtzSupPower1On.Text = "电源1开";
            this.btnPtzSupPower1On.UseVisualStyleBackColor = true;
            this.btnPtzSupPower1On.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPower2On
            // 
            this.btnPtzSupPower2On.AutoEllipsis = true;
            this.btnPtzSupPower2On.Location = new System.Drawing.Point(145, 84);
            this.btnPtzSupPower2On.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPower2On.Name = "btnPtzSupPower2On";
            this.btnPtzSupPower2On.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPower2On.TabIndex = 0;
            this.btnPtzSupPower2On.Tag = "power_2_on";
            this.btnPtzSupPower2On.Text = " 电源2开";
            this.btnPtzSupPower2On.UseVisualStyleBackColor = true;
            this.btnPtzSupPower2On.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPower1Off
            // 
            this.btnPtzSupPower1Off.AutoEllipsis = true;
            this.btnPtzSupPower1Off.Location = new System.Drawing.Point(12, 120);
            this.btnPtzSupPower1Off.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPower1Off.Name = "btnPtzSupPower1Off";
            this.btnPtzSupPower1Off.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPower1Off.TabIndex = 0;
            this.btnPtzSupPower1Off.Tag = "power_1_off";
            this.btnPtzSupPower1Off.Text = "电源1关";
            this.btnPtzSupPower1Off.UseVisualStyleBackColor = true;
            this.btnPtzSupPower1Off.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPower2Off
            // 
            this.btnPtzSupPower2Off.AutoEllipsis = true;
            this.btnPtzSupPower2Off.Location = new System.Drawing.Point(145, 120);
            this.btnPtzSupPower2Off.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPower2Off.Name = "btnPtzSupPower2Off";
            this.btnPtzSupPower2Off.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPower2Off.TabIndex = 0;
            this.btnPtzSupPower2Off.Tag = "power_2_off";
            this.btnPtzSupPower2Off.Text = " 电源2关";
            this.btnPtzSupPower2Off.UseVisualStyleBackColor = true;
            this.btnPtzSupPower2Off.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupReturnZero
            // 
            this.btnPtzSupReturnZero.AutoEllipsis = true;
            this.btnPtzSupReturnZero.Location = new System.Drawing.Point(12, 156);
            this.btnPtzSupReturnZero.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupReturnZero.Name = "btnPtzSupReturnZero";
            this.btnPtzSupReturnZero.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupReturnZero.TabIndex = 0;
            this.btnPtzSupReturnZero.Tag = "return_zero";
            this.btnPtzSupReturnZero.Text = "回到0位 H/V";
            this.btnPtzSupReturnZero.UseVisualStyleBackColor = true;
            this.btnPtzSupReturnZero.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // gbxPtzExtQuery
            // 
            this.gbxPtzExtQuery.Controls.Add(this.flpPtzSupplementQuery);
            this.gbxPtzExtQuery.Location = new System.Drawing.Point(1710, 10);
            this.gbxPtzExtQuery.Name = "gbxPtzExtQuery";
            this.gbxPtzExtQuery.Size = new System.Drawing.Size(325, 330);
            this.gbxPtzExtQuery.TabIndex = 11;
            this.gbxPtzExtQuery.TabStop = false;
            this.gbxPtzExtQuery.Text = "查询与回传";
            // 
            // flpPtzSupplementQuery
            // 
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupAckOn);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupAckOff);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupQueryTemperature);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupQueryVoltage);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupQueryCurrent);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupQueryHSpeed);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupQueryVSpeed);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupQueryAllSpeed);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupSpeedRealtimeOn);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupSpeedRealtimeOff);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupLocateReturnOn);
            this.flpPtzSupplementQuery.Controls.Add(this.btnPtzSupLocateReturnOff);
            this.flpPtzSupplementQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPtzSupplementQuery.Location = new System.Drawing.Point(3, 17);
            this.flpPtzSupplementQuery.Name = "flpPtzSupplementQuery";
            this.flpPtzSupplementQuery.Padding = new System.Windows.Forms.Padding(8);
            this.flpPtzSupplementQuery.Size = new System.Drawing.Size(319, 310);
            this.flpPtzSupplementQuery.TabIndex = 0;
            // 
            // btnPtzSupAckOn
            // 
            this.btnPtzSupAckOn.AutoEllipsis = true;
            this.btnPtzSupAckOn.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupAckOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAckOn.Name = "btnPtzSupAckOn";
            this.btnPtzSupAckOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAckOn.TabIndex = 0;
            this.btnPtzSupAckOn.Tag = "ack_on";
            this.btnPtzSupAckOn.Text = "指令回复开";
            this.btnPtzSupAckOn.UseVisualStyleBackColor = true;
            this.btnPtzSupAckOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAckOff
            // 
            this.btnPtzSupAckOff.AutoEllipsis = true;
            this.btnPtzSupAckOff.Location = new System.Drawing.Point(145, 12);
            this.btnPtzSupAckOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAckOff.Name = "btnPtzSupAckOff";
            this.btnPtzSupAckOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAckOff.TabIndex = 0;
            this.btnPtzSupAckOff.Tag = "ack_off";
            this.btnPtzSupAckOff.Text = " 指令回复关";
            this.btnPtzSupAckOff.UseVisualStyleBackColor = true;
            this.btnPtzSupAckOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryTemperature
            // 
            this.btnPtzSupQueryTemperature.AutoEllipsis = true;
            this.btnPtzSupQueryTemperature.Location = new System.Drawing.Point(12, 48);
            this.btnPtzSupQueryTemperature.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryTemperature.Name = "btnPtzSupQueryTemperature";
            this.btnPtzSupQueryTemperature.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryTemperature.TabIndex = 0;
            this.btnPtzSupQueryTemperature.Tag = "query_temperature";
            this.btnPtzSupQueryTemperature.Text = "查询温度";
            this.btnPtzSupQueryTemperature.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryTemperature.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryVoltage
            // 
            this.btnPtzSupQueryVoltage.AutoEllipsis = true;
            this.btnPtzSupQueryVoltage.Location = new System.Drawing.Point(145, 48);
            this.btnPtzSupQueryVoltage.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryVoltage.Name = "btnPtzSupQueryVoltage";
            this.btnPtzSupQueryVoltage.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryVoltage.TabIndex = 0;
            this.btnPtzSupQueryVoltage.Tag = "query_voltage";
            this.btnPtzSupQueryVoltage.Text = "查询电压";
            this.btnPtzSupQueryVoltage.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryVoltage.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryCurrent
            // 
            this.btnPtzSupQueryCurrent.AutoEllipsis = true;
            this.btnPtzSupQueryCurrent.Location = new System.Drawing.Point(12, 84);
            this.btnPtzSupQueryCurrent.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryCurrent.Name = "btnPtzSupQueryCurrent";
            this.btnPtzSupQueryCurrent.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryCurrent.TabIndex = 0;
            this.btnPtzSupQueryCurrent.Tag = "query_current";
            this.btnPtzSupQueryCurrent.Text = "查询电流";
            this.btnPtzSupQueryCurrent.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryCurrent.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryHSpeed
            // 
            this.btnPtzSupQueryHSpeed.AutoEllipsis = true;
            this.btnPtzSupQueryHSpeed.Location = new System.Drawing.Point(145, 84);
            this.btnPtzSupQueryHSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryHSpeed.Name = "btnPtzSupQueryHSpeed";
            this.btnPtzSupQueryHSpeed.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryHSpeed.TabIndex = 0;
            this.btnPtzSupQueryHSpeed.Tag = "query_h_speed";
            this.btnPtzSupQueryHSpeed.Text = "查询水平转速";
            this.btnPtzSupQueryHSpeed.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryHSpeed.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryVSpeed
            // 
            this.btnPtzSupQueryVSpeed.AutoEllipsis = true;
            this.btnPtzSupQueryVSpeed.Location = new System.Drawing.Point(12, 120);
            this.btnPtzSupQueryVSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryVSpeed.Name = "btnPtzSupQueryVSpeed";
            this.btnPtzSupQueryVSpeed.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryVSpeed.TabIndex = 0;
            this.btnPtzSupQueryVSpeed.Tag = "query_v_speed";
            this.btnPtzSupQueryVSpeed.Text = " 查询垂直转速";
            this.btnPtzSupQueryVSpeed.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryVSpeed.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryAllSpeed
            // 
            this.btnPtzSupQueryAllSpeed.AutoEllipsis = true;
            this.btnPtzSupQueryAllSpeed.Location = new System.Drawing.Point(145, 120);
            this.btnPtzSupQueryAllSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryAllSpeed.Name = "btnPtzSupQueryAllSpeed";
            this.btnPtzSupQueryAllSpeed.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryAllSpeed.TabIndex = 0;
            this.btnPtzSupQueryAllSpeed.Tag = "query_all_speed";
            this.btnPtzSupQueryAllSpeed.Text = " 查询全部转速";
            this.btnPtzSupQueryAllSpeed.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryAllSpeed.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupSpeedRealtimeOn
            // 
            this.btnPtzSupSpeedRealtimeOn.AutoEllipsis = true;
            this.btnPtzSupSpeedRealtimeOn.Location = new System.Drawing.Point(12, 156);
            this.btnPtzSupSpeedRealtimeOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupSpeedRealtimeOn.Name = "btnPtzSupSpeedRealtimeOn";
            this.btnPtzSupSpeedRealtimeOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupSpeedRealtimeOn.TabIndex = 0;
            this.btnPtzSupSpeedRealtimeOn.Tag = "speed_realtime_on";
            this.btnPtzSupSpeedRealtimeOn.Text = " 转速实时回传开";
            this.btnPtzSupSpeedRealtimeOn.UseVisualStyleBackColor = true;
            this.btnPtzSupSpeedRealtimeOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupSpeedRealtimeOff
            // 
            this.btnPtzSupSpeedRealtimeOff.AutoEllipsis = true;
            this.btnPtzSupSpeedRealtimeOff.Location = new System.Drawing.Point(145, 156);
            this.btnPtzSupSpeedRealtimeOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupSpeedRealtimeOff.Name = "btnPtzSupSpeedRealtimeOff";
            this.btnPtzSupSpeedRealtimeOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupSpeedRealtimeOff.TabIndex = 0;
            this.btnPtzSupSpeedRealtimeOff.Tag = "speed_realtime_off";
            this.btnPtzSupSpeedRealtimeOff.Text = " 转速实时回传关";
            this.btnPtzSupSpeedRealtimeOff.UseVisualStyleBackColor = true;
            this.btnPtzSupSpeedRealtimeOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupLocateReturnOn
            // 
            this.btnPtzSupLocateReturnOn.AutoEllipsis = true;
            this.btnPtzSupLocateReturnOn.Location = new System.Drawing.Point(12, 192);
            this.btnPtzSupLocateReturnOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupLocateReturnOn.Name = "btnPtzSupLocateReturnOn";
            this.btnPtzSupLocateReturnOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupLocateReturnOn.TabIndex = 0;
            this.btnPtzSupLocateReturnOn.Tag = "locate_return_on";
            this.btnPtzSupLocateReturnOn.Text = " 定位回传开";
            this.btnPtzSupLocateReturnOn.UseVisualStyleBackColor = true;
            this.btnPtzSupLocateReturnOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupLocateReturnOff
            // 
            this.btnPtzSupLocateReturnOff.AutoEllipsis = true;
            this.btnPtzSupLocateReturnOff.Location = new System.Drawing.Point(145, 192);
            this.btnPtzSupLocateReturnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupLocateReturnOff.Name = "btnPtzSupLocateReturnOff";
            this.btnPtzSupLocateReturnOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupLocateReturnOff.TabIndex = 0;
            this.btnPtzSupLocateReturnOff.Tag = "locate_return_off";
            this.btnPtzSupLocateReturnOff.Text = "定位回传关";
            this.btnPtzSupLocateReturnOff.UseVisualStyleBackColor = true;
            this.btnPtzSupLocateReturnOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // tabPtzNavigation
            // 
            this.tabPtzNavigation.Controls.Add(this.tabPtzHome);
            this.tabPtzNavigation.Controls.Add(this.tabPtzPresetPage);
            this.tabPtzNavigation.Controls.Add(this.tabPtzConfigPage);
            this.tabPtzNavigation.Controls.Add(this.tabPtzAreaPage);
            this.tabPtzNavigation.Controls.Add(this.tabPtzZeroPage);
            this.tabPtzNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPtzNavigation.Location = new System.Drawing.Point(3, 3);
            this.tabPtzNavigation.Name = "tabPtzNavigation";
            this.tabPtzNavigation.Padding = new System.Drawing.Point(18, 6);
            this.tabPtzNavigation.SelectedIndex = 0;
            this.tabPtzNavigation.Size = new System.Drawing.Size(2043, 790);
            this.tabPtzNavigation.TabIndex = 0;
            // 
            // tabPtzHome
            // 
            this.tabPtzHome.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPtzHome.Location = new System.Drawing.Point(4, 30);
            this.tabPtzHome.Name = "tabPtzHome";
            this.tabPtzHome.Padding = new System.Windows.Forms.Padding(10);
            this.tabPtzHome.Size = new System.Drawing.Size(2035, 756);
            this.tabPtzHome.TabIndex = 0;
            this.tabPtzHome.Text = "首页";
            // 
            // tabPtzPresetPage
            // 
            this.tabPtzPresetPage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPtzPresetPage.Location = new System.Drawing.Point(4, 30);
            this.tabPtzPresetPage.Name = "tabPtzPresetPage";
            this.tabPtzPresetPage.Padding = new System.Windows.Forms.Padding(10);
            this.tabPtzPresetPage.Size = new System.Drawing.Size(2035, 756);
            this.tabPtzPresetPage.TabIndex = 1;
            this.tabPtzPresetPage.Text = "预置位";
            // 
            // tabPtzConfigPage
            // 
            this.tabPtzConfigPage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPtzConfigPage.Location = new System.Drawing.Point(4, 30);
            this.tabPtzConfigPage.Name = "tabPtzConfigPage";
            this.tabPtzConfigPage.Padding = new System.Windows.Forms.Padding(10);
            this.tabPtzConfigPage.Size = new System.Drawing.Size(2035, 756);
            this.tabPtzConfigPage.TabIndex = 2;
            this.tabPtzConfigPage.Text = "配置信息";
            // 
            // tabPtzAreaPage
            // 
            this.tabPtzAreaPage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPtzAreaPage.Location = new System.Drawing.Point(4, 30);
            this.tabPtzAreaPage.Name = "tabPtzAreaPage";
            this.tabPtzAreaPage.Padding = new System.Windows.Forms.Padding(10);
            this.tabPtzAreaPage.Size = new System.Drawing.Size(2035, 756);
            this.tabPtzAreaPage.TabIndex = 3;
            this.tabPtzAreaPage.Text = "区域扫描";
            // 
            // tabPtzZeroPage
            // 
            this.tabPtzZeroPage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPtzZeroPage.Location = new System.Drawing.Point(4, 30);
            this.tabPtzZeroPage.Name = "tabPtzZeroPage";
            this.tabPtzZeroPage.Padding = new System.Windows.Forms.Padding(10);
            this.tabPtzZeroPage.Size = new System.Drawing.Size(2035, 756);
            this.tabPtzZeroPage.TabIndex = 4;
            this.tabPtzZeroPage.Text = "设置0位";
            // 
            // gbxPtzSupplement
            // 
            this.gbxPtzSupplement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxPtzSupplement.Location = new System.Drawing.Point(895, 10);
            this.gbxPtzSupplement.Name = "gbxPtzSupplement";
            this.gbxPtzSupplement.Size = new System.Drawing.Size(1140, 756);
            this.gbxPtzSupplement.TabIndex = 8;
            this.gbxPtzSupplement.TabStop = false;
            this.gbxPtzSupplement.Text = "扩展功能（按功能分区）";
            // 
            // tabPtzSupplementCommands
            // 
            this.tabPtzSupplementCommands.Controls.Add(this.tabPtzSupplementBasic);
            this.tabPtzSupplementCommands.Controls.Add(this.tabPtzSupplementArea);
            this.tabPtzSupplementCommands.Controls.Add(this.tabPtzSupplementPreset);
            this.tabPtzSupplementCommands.Controls.Add(this.tabPtzSupplementQuery);
            this.tabPtzSupplementCommands.Controls.Add(this.tabPtzSupplementZero);
            this.tabPtzSupplementCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPtzSupplementCommands.Location = new System.Drawing.Point(3, 17);
            this.tabPtzSupplementCommands.Name = "tabPtzSupplementCommands";
            this.tabPtzSupplementCommands.SelectedIndex = 0;
            this.tabPtzSupplementCommands.Size = new System.Drawing.Size(1134, 230);
            this.tabPtzSupplementCommands.TabIndex = 0;
            // 
            // tabPtzSupplementBasic
            // 
            this.tabPtzSupplementBasic.Location = new System.Drawing.Point(4, 22);
            this.tabPtzSupplementBasic.Name = "tabPtzSupplementBasic";
            this.tabPtzSupplementBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPtzSupplementBasic.Size = new System.Drawing.Size(1126, 204);
            this.tabPtzSupplementBasic.TabIndex = 0;
            this.tabPtzSupplementBasic.Text = "基础";
            this.tabPtzSupplementBasic.UseVisualStyleBackColor = true;
            // 
            // tabPtzSupplementArea
            // 
            this.tabPtzSupplementArea.Location = new System.Drawing.Point(4, 22);
            this.tabPtzSupplementArea.Name = "tabPtzSupplementArea";
            this.tabPtzSupplementArea.Padding = new System.Windows.Forms.Padding(3);
            this.tabPtzSupplementArea.Size = new System.Drawing.Size(1126, 204);
            this.tabPtzSupplementArea.TabIndex = 1;
            this.tabPtzSupplementArea.Text = "区域扫描";
            this.tabPtzSupplementArea.UseVisualStyleBackColor = true;
            // 
            // tabPtzSupplementPreset
            // 
            this.tabPtzSupplementPreset.Location = new System.Drawing.Point(4, 22);
            this.tabPtzSupplementPreset.Name = "tabPtzSupplementPreset";
            this.tabPtzSupplementPreset.Padding = new System.Windows.Forms.Padding(3);
            this.tabPtzSupplementPreset.Size = new System.Drawing.Size(1126, 204);
            this.tabPtzSupplementPreset.TabIndex = 2;
            this.tabPtzSupplementPreset.Text = "预置位";
            this.tabPtzSupplementPreset.UseVisualStyleBackColor = true;
            // 
            // tabPtzSupplementQuery
            // 
            this.tabPtzSupplementQuery.Location = new System.Drawing.Point(4, 22);
            this.tabPtzSupplementQuery.Name = "tabPtzSupplementQuery";
            this.tabPtzSupplementQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabPtzSupplementQuery.Size = new System.Drawing.Size(1126, 204);
            this.tabPtzSupplementQuery.TabIndex = 3;
            this.tabPtzSupplementQuery.Text = "查询回传";
            this.tabPtzSupplementQuery.UseVisualStyleBackColor = true;
            // 
            // tabPtzSupplementZero
            // 
            this.tabPtzSupplementZero.Location = new System.Drawing.Point(4, 22);
            this.tabPtzSupplementZero.Name = "tabPtzSupplementZero";
            this.tabPtzSupplementZero.Padding = new System.Windows.Forms.Padding(3);
            this.tabPtzSupplementZero.Size = new System.Drawing.Size(1126, 204);
            this.tabPtzSupplementZero.TabIndex = 4;
            this.tabPtzSupplementZero.Text = "零位维护";
            this.tabPtzSupplementZero.UseVisualStyleBackColor = true;
            // 
            // btnPtzSupDirUp
            // 
            this.btnPtzSupDirUp.AutoEllipsis = true;
            this.btnPtzSupDirUp.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupDirUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirUp.Name = "btnPtzSupDirUp";
            this.btnPtzSupDirUp.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupDirUp.TabIndex = 0;
            this.btnPtzSupDirUp.Tag = "dir_up";
            this.btnPtzSupDirUp.Text = " 上(按住)";
            this.btnPtzSupDirUp.UseVisualStyleBackColor = true;
            this.btnPtzSupDirUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirUp.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupDirDown
            // 
            this.btnPtzSupDirDown.AutoEllipsis = true;
            this.btnPtzSupDirDown.Location = new System.Drawing.Point(230, 12);
            this.btnPtzSupDirDown.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirDown.Name = "btnPtzSupDirDown";
            this.btnPtzSupDirDown.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupDirDown.TabIndex = 0;
            this.btnPtzSupDirDown.Tag = "dir_down";
            this.btnPtzSupDirDown.Text = "下(按住)";
            this.btnPtzSupDirDown.UseVisualStyleBackColor = true;
            this.btnPtzSupDirDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirDown.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupDirLeft
            // 
            this.btnPtzSupDirLeft.AutoEllipsis = true;
            this.btnPtzSupDirLeft.Location = new System.Drawing.Point(448, 12);
            this.btnPtzSupDirLeft.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirLeft.Name = "btnPtzSupDirLeft";
            this.btnPtzSupDirLeft.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupDirLeft.TabIndex = 0;
            this.btnPtzSupDirLeft.Tag = "dir_left";
            this.btnPtzSupDirLeft.Text = "左(按住)";
            this.btnPtzSupDirLeft.UseVisualStyleBackColor = true;
            this.btnPtzSupDirLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirLeft.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupDirRight
            // 
            this.btnPtzSupDirRight.AutoEllipsis = true;
            this.btnPtzSupDirRight.Location = new System.Drawing.Point(666, 12);
            this.btnPtzSupDirRight.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupDirRight.Name = "btnPtzSupDirRight";
            this.btnPtzSupDirRight.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupDirRight.TabIndex = 0;
            this.btnPtzSupDirRight.Tag = "dir_right";
            this.btnPtzSupDirRight.Text = "右(按住)";
            this.btnPtzSupDirRight.UseVisualStyleBackColor = true;
            this.btnPtzSupDirRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseDown);
            this.btnPtzSupDirRight.MouseLeave += new System.EventHandler(this.PtzSupplementDirectionButton_MouseLeave);
            this.btnPtzSupDirRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PtzSupplementDirectionButton_MouseUp);
            // 
            // btnPtzSupStop
            // 
            this.btnPtzSupStop.AutoEllipsis = true;
            this.btnPtzSupStop.Location = new System.Drawing.Point(666, 52);
            this.btnPtzSupStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupStop.Name = "btnPtzSupStop";
            this.btnPtzSupStop.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupStop.TabIndex = 0;
            this.btnPtzSupStop.Tag = "stop";
            this.btnPtzSupStop.Text = "停止";
            this.btnPtzSupStop.UseVisualStyleBackColor = true;
            this.btnPtzSupStop.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaAngleHa
            // 
            this.btnPtzSupAreaAngleHa.AutoEllipsis = true;
            this.btnPtzSupAreaAngleHa.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupAreaAngleHa.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaAngleHa.Name = "btnPtzSupAreaAngleHa";
            this.btnPtzSupAreaAngleHa.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaAngleHa.TabIndex = 0;
            this.btnPtzSupAreaAngleHa.Tag = "area_angle_ha";
            this.btnPtzSupAreaAngleHa.Text = "角度HA边界";
            this.btnPtzSupAreaAngleHa.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaAngleHa.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaAngleHb
            // 
            this.btnPtzSupAreaAngleHb.AutoEllipsis = true;
            this.btnPtzSupAreaAngleHb.Location = new System.Drawing.Point(230, 12);
            this.btnPtzSupAreaAngleHb.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaAngleHb.Name = "btnPtzSupAreaAngleHb";
            this.btnPtzSupAreaAngleHb.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaAngleHb.TabIndex = 0;
            this.btnPtzSupAreaAngleHb.Tag = "area_angle_hb";
            this.btnPtzSupAreaAngleHb.Text = "角度HB边界";
            this.btnPtzSupAreaAngleHb.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaAngleHb.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaAngleVa
            // 
            this.btnPtzSupAreaAngleVa.AutoEllipsis = true;
            this.btnPtzSupAreaAngleVa.Location = new System.Drawing.Point(448, 12);
            this.btnPtzSupAreaAngleVa.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaAngleVa.Name = "btnPtzSupAreaAngleVa";
            this.btnPtzSupAreaAngleVa.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaAngleVa.TabIndex = 0;
            this.btnPtzSupAreaAngleVa.Tag = "area_angle_va";
            this.btnPtzSupAreaAngleVa.Text = "角度VA边界";
            this.btnPtzSupAreaAngleVa.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaAngleVa.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaAngleVb
            // 
            this.btnPtzSupAreaAngleVb.AutoEllipsis = true;
            this.btnPtzSupAreaAngleVb.Location = new System.Drawing.Point(666, 12);
            this.btnPtzSupAreaAngleVb.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaAngleVb.Name = "btnPtzSupAreaAngleVb";
            this.btnPtzSupAreaAngleVb.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaAngleVb.TabIndex = 0;
            this.btnPtzSupAreaAngleVb.Tag = "area_angle_vb";
            this.btnPtzSupAreaAngleVb.Text = "角度VB边界";
            this.btnPtzSupAreaAngleVb.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaAngleVb.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaIntervalH
            // 
            this.btnPtzSupAreaIntervalH.AutoEllipsis = true;
            this.btnPtzSupAreaIntervalH.Location = new System.Drawing.Point(12, 92);
            this.btnPtzSupAreaIntervalH.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaIntervalH.Name = "btnPtzSupAreaIntervalH";
            this.btnPtzSupAreaIntervalH.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaIntervalH.TabIndex = 0;
            this.btnPtzSupAreaIntervalH.Tag = "area_interval_h";
            this.btnPtzSupAreaIntervalH.Text = "水平步进";
            this.btnPtzSupAreaIntervalH.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaIntervalH.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaIntervalV
            // 
            this.btnPtzSupAreaIntervalV.AutoEllipsis = true;
            this.btnPtzSupAreaIntervalV.Location = new System.Drawing.Point(230, 92);
            this.btnPtzSupAreaIntervalV.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaIntervalV.Name = "btnPtzSupAreaIntervalV";
            this.btnPtzSupAreaIntervalV.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaIntervalV.TabIndex = 0;
            this.btnPtzSupAreaIntervalV.Tag = "area_interval_v";
            this.btnPtzSupAreaIntervalV.Text = "垂直步进";
            this.btnPtzSupAreaIntervalV.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaIntervalV.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaStartMulti
            // 
            this.btnPtzSupAreaStartMulti.AutoEllipsis = true;
            this.btnPtzSupAreaStartMulti.Location = new System.Drawing.Point(666, 132);
            this.btnPtzSupAreaStartMulti.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaStartMulti.Name = "btnPtzSupAreaStartMulti";
            this.btnPtzSupAreaStartMulti.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaStartMulti.TabIndex = 0;
            this.btnPtzSupAreaStartMulti.Tag = "area_start_multi";
            this.btnPtzSupAreaStartMulti.Text = "多区域扫描";
            this.btnPtzSupAreaStartMulti.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaStartMulti.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAreaClose
            // 
            this.btnPtzSupAreaClose.AutoEllipsis = true;
            this.btnPtzSupAreaClose.Location = new System.Drawing.Point(448, 172);
            this.btnPtzSupAreaClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAreaClose.Name = "btnPtzSupAreaClose";
            this.btnPtzSupAreaClose.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAreaClose.TabIndex = 0;
            this.btnPtzSupAreaClose.Tag = "area_close";
            this.btnPtzSupAreaClose.Text = " 关闭扫描";
            this.btnPtzSupAreaClose.UseVisualStyleBackColor = true;
            this.btnPtzSupAreaClose.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetStandardSet
            // 
            this.btnPtzSupPresetStandardSet.AutoEllipsis = true;
            this.btnPtzSupPresetStandardSet.Location = new System.Drawing.Point(12, 12);
            this.btnPtzSupPresetStandardSet.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetStandardSet.Name = "btnPtzSupPresetStandardSet";
            this.btnPtzSupPresetStandardSet.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetStandardSet.TabIndex = 0;
            this.btnPtzSupPresetStandardSet.Tag = "preset_standard_set";
            this.btnPtzSupPresetStandardSet.Text = "设置预置";
            this.btnPtzSupPresetStandardSet.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetStandardSet.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetStandardCall
            // 
            this.btnPtzSupPresetStandardCall.AutoEllipsis = true;
            this.btnPtzSupPresetStandardCall.Location = new System.Drawing.Point(230, 12);
            this.btnPtzSupPresetStandardCall.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetStandardCall.Name = "btnPtzSupPresetStandardCall";
            this.btnPtzSupPresetStandardCall.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetStandardCall.TabIndex = 0;
            this.btnPtzSupPresetStandardCall.Tag = "preset_standard_call";
            this.btnPtzSupPresetStandardCall.Text = "调用预置";
            this.btnPtzSupPresetStandardCall.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetStandardCall.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetStandardDelete
            // 
            this.btnPtzSupPresetStandardDelete.AutoEllipsis = true;
            this.btnPtzSupPresetStandardDelete.Location = new System.Drawing.Point(448, 12);
            this.btnPtzSupPresetStandardDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetStandardDelete.Name = "btnPtzSupPresetStandardDelete";
            this.btnPtzSupPresetStandardDelete.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetStandardDelete.TabIndex = 0;
            this.btnPtzSupPresetStandardDelete.Tag = "preset_standard_delete";
            this.btnPtzSupPresetStandardDelete.Text = "删除预置";
            this.btnPtzSupPresetStandardDelete.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetStandardDelete.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetStart
            // 
            this.btnPtzSupPresetStart.AutoEllipsis = true;
            this.btnPtzSupPresetStart.Location = new System.Drawing.Point(666, 52);
            this.btnPtzSupPresetStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetStart.Name = "btnPtzSupPresetStart";
            this.btnPtzSupPresetStart.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetStart.TabIndex = 0;
            this.btnPtzSupPresetStart.Tag = "preset_start";
            this.btnPtzSupPresetStart.Text = "开始预置扫描";
            this.btnPtzSupPresetStart.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetStart.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupPresetClose
            // 
            this.btnPtzSupPresetClose.AutoEllipsis = true;
            this.btnPtzSupPresetClose.Location = new System.Drawing.Point(230, 92);
            this.btnPtzSupPresetClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupPresetClose.Name = "btnPtzSupPresetClose";
            this.btnPtzSupPresetClose.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupPresetClose.TabIndex = 0;
            this.btnPtzSupPresetClose.Tag = "preset_close";
            this.btnPtzSupPresetClose.Text = "关闭预置扫描";
            this.btnPtzSupPresetClose.UseVisualStyleBackColor = true;
            this.btnPtzSupPresetClose.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryMode
            // 
            this.btnPtzSupQueryMode.AutoEllipsis = true;
            this.btnPtzSupQueryMode.Location = new System.Drawing.Point(448, 12);
            this.btnPtzSupQueryMode.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryMode.Name = "btnPtzSupQueryMode";
            this.btnPtzSupQueryMode.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryMode.TabIndex = 0;
            this.btnPtzSupQueryMode.Tag = "query_mode";
            this.btnPtzSupQueryMode.Text = "查询模式";
            this.btnPtzSupQueryMode.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryMode.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupQueryStatus
            // 
            this.btnPtzSupQueryStatus.AutoEllipsis = true;
            this.btnPtzSupQueryStatus.Location = new System.Drawing.Point(666, 12);
            this.btnPtzSupQueryStatus.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupQueryStatus.Name = "btnPtzSupQueryStatus";
            this.btnPtzSupQueryStatus.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupQueryStatus.TabIndex = 0;
            this.btnPtzSupQueryStatus.Tag = "query_status";
            this.btnPtzSupQueryStatus.Text = "查询状态";
            this.btnPtzSupQueryStatus.UseVisualStyleBackColor = true;
            this.btnPtzSupQueryStatus.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAngleRealtimeOn
            // 
            this.btnPtzSupAngleRealtimeOn.AutoEllipsis = true;
            this.btnPtzSupAngleRealtimeOn.Location = new System.Drawing.Point(448, 52);
            this.btnPtzSupAngleRealtimeOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAngleRealtimeOn.Name = "btnPtzSupAngleRealtimeOn";
            this.btnPtzSupAngleRealtimeOn.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAngleRealtimeOn.TabIndex = 0;
            this.btnPtzSupAngleRealtimeOn.Tag = "angle_realtime_on";
            this.btnPtzSupAngleRealtimeOn.Text = "角度回传开";
            this.btnPtzSupAngleRealtimeOn.UseVisualStyleBackColor = true;
            this.btnPtzSupAngleRealtimeOn.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // btnPtzSupAngleRealtimeOff
            // 
            this.btnPtzSupAngleRealtimeOff.AutoEllipsis = true;
            this.btnPtzSupAngleRealtimeOff.Location = new System.Drawing.Point(666, 52);
            this.btnPtzSupAngleRealtimeOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnPtzSupAngleRealtimeOff.Name = "btnPtzSupAngleRealtimeOff";
            this.btnPtzSupAngleRealtimeOff.Size = new System.Drawing.Size(125, 28);
            this.btnPtzSupAngleRealtimeOff.TabIndex = 0;
            this.btnPtzSupAngleRealtimeOff.Tag = "angle_realtime_off";
            this.btnPtzSupAngleRealtimeOff.Text = "角度回传关";
            this.btnPtzSupAngleRealtimeOff.UseVisualStyleBackColor = true;
            this.btnPtzSupAngleRealtimeOff.Click += new System.EventHandler(this.PtzSupplementButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2057, 822);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Livox Radar Connect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox_DepthCompletion.ResumeLayout(false);
            this.tableLayoutPanelSplit.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FusionResult)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.gbxPtzNet.ResumeLayout(false);
            this.gbxPtzNet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzLocalPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAddress)).EndInit();
            this.gbxPtzQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzRealtimeInterval)).EndInit();
            this.gbxPtzPreset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPreset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPresetStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPresetEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzPresetTime)).EndInit();
            this.gbxPtzRaw.ResumeLayout(false);
            this.gbxPtzRaw.PerformLayout();
            this.gbxPtzDirect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzHSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzVSpeed)).EndInit();
            this.gbxPtzArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaHStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaHEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaHInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaVStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaVEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaVInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzAreaEnd)).EndInit();
            this.gbxPtzLocate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzHAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PtzVAngle)).EndInit();
            this.gbxPtzExtPreset.ResumeLayout(false);
            this.flpPtzSupplementPreset.ResumeLayout(false);
            this.gbxPtzExtArea.ResumeLayout(false);
            this.flpPtzSupplementArea.ResumeLayout(false);
            this.gbxPtzExtMaintenance.ResumeLayout(false);
            this.flpPtzSupplementZero.ResumeLayout(false);
            this.gbxPtzExtBasic.ResumeLayout(false);
            this.flpPtzSupplementBasic.ResumeLayout(false);
            this.gbxPtzExtQuery.ResumeLayout(false);
            this.flpPtzSupplementQuery.ResumeLayout(false);
            this.tabPtzNavigation.ResumeLayout(false);
            this.tabPtzSupplementCommands.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btn_EnableOsd;
        private System.Windows.Forms.Button btn_RemoveOsd;
        private System.Windows.Forms.Button btn_Snapshot;
        private System.Windows.Forms.Button btn_ZoomOut;
        private System.Windows.Forms.Button btn_ZoomIn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox_Log;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_SetCoordinate;
        private System.Windows.Forms.ComboBox cbx_Coordinate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_StopSample;
        private System.Windows.Forms.Button btn_StartSample;
        private System.Windows.Forms.Button btn_SetMode;
        private System.Windows.Forms.ComboBox cbx_WorkMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SetScanPattern;
        private System.Windows.Forms.ComboBox cbx_ScanPattern;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Disconnect;
        private System.Windows.Forms.Button btn_HandShake;
        private System.Windows.Forms.Button btn_StopListen;
        private System.Windows.Forms.Button btn_StartListen;
        private System.Windows.Forms.ListView listView_Devices;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel_Video;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_CameraIp;
        private System.Windows.Forms.Button btn_PlayCamera;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_ExportPcd;
        private System.Windows.Forms.Button btn_SaveBEV;
        private System.Windows.Forms.Button btn_ShowRaw;
        private System.Windows.Forms.Button btn_SaveImage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Query;
        private System.Windows.Forms.Button btn_Reconstruct;
        private System.Windows.Forms.GroupBox groupBox_DepthCompletion;
        private System.Windows.Forms.Button btn_CompleteDepth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSplit;
        private Kitware.VTK.RenderWindowControl renderWindowControl1;
        private Kitware.VTK.RenderWindowControl renderWindowControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox_FusionResult;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_ExecuteFusion;
        private System.Windows.Forms.Button btn_SelectVideo;
        private System.Windows.Forms.TextBox txt_VideoPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Fusion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_LoadPcd;

        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabPtzNavigation;
        private System.Windows.Forms.TabPage tabPtzHome;
        private System.Windows.Forms.TabPage tabPtzPresetPage;
        private System.Windows.Forms.TabPage tabPtzConfigPage;
        private System.Windows.Forms.TabPage tabPtzAreaPage;
        private System.Windows.Forms.TabPage tabPtzZeroPage;

        private System.Windows.Forms.TextBox txt_PtzLocalIp;
        private System.Windows.Forms.NumericUpDown nud_PtzLocalPort;
        private System.Windows.Forms.TextBox txt_PtzIp;
        private System.Windows.Forms.NumericUpDown nud_PtzPort;
        private System.Windows.Forms.NumericUpDown nud_PtzAddress;
        private System.Windows.Forms.Label lbl_PtzHAngle;
        private System.Windows.Forms.Label lbl_PtzVAngle;
        private System.Windows.Forms.Label lbl_PtzStatus;
        private System.Windows.Forms.TextBox txt_PtzRawHex;
        private System.Windows.Forms.NumericUpDown nud_PtzHSpeed;
        private System.Windows.Forms.NumericUpDown nud_PtzVSpeed;
        private System.Windows.Forms.NumericUpDown nud_PtzHAngle;
        private System.Windows.Forms.NumericUpDown nud_PtzVAngle;
        private System.Windows.Forms.CheckBox chk_PtzUseSpeedLocate;
        private System.Windows.Forms.NumericUpDown nud_PtzPreset;
        private System.Windows.Forms.NumericUpDown nud_PtzPresetStart;
        private System.Windows.Forms.NumericUpDown nud_PtzPresetEnd;
        private System.Windows.Forms.NumericUpDown nud_PtzPresetTime;
        private System.Windows.Forms.NumericUpDown nud_PtzArea;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaHStart;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaHEnd;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaVStart;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaVEnd;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaHInterval;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaVInterval;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaTime;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaStart;
        private System.Windows.Forms.NumericUpDown nud_PtzAreaEnd;
        private System.Windows.Forms.NumericUpDown nud_PtzRealtimeInterval;
        private System.Windows.Forms.GroupBox gbxPtzNet;
        private System.Windows.Forms.Label lblLIn;
        private System.Windows.Forms.Label lblRIn;
        private System.Windows.Forms.Label lblAddr;
        private System.Windows.Forms.Button btnPtzOpen;
        private System.Windows.Forms.Button btnPtzClose;
        private System.Windows.Forms.GroupBox gbxPtzDirect;
        private System.Windows.Forms.Label lblHS;
        private System.Windows.Forms.Label lblVS;
        private System.Windows.Forms.Button btnPtzUp;
        private System.Windows.Forms.Button btnPtzLeft;
        private System.Windows.Forms.Button btnPtzStop;
        private System.Windows.Forms.Button btnPtzRight;
        private System.Windows.Forms.Button btnPtzDown;
        private System.Windows.Forms.GroupBox gbxPtzLocate;
        private System.Windows.Forms.Label lblHA;
        private System.Windows.Forms.Label lblVA;
        private System.Windows.Forms.Button btnPtzLocate;
        private System.Windows.Forms.GroupBox gbxPtzPreset;
        private System.Windows.Forms.Label lblPno;
        private System.Windows.Forms.Button btnPtzPresetSet;
        private System.Windows.Forms.Button btnPtzPresetCall;
        private System.Windows.Forms.Button btnPtzPresetDel;
        private System.Windows.Forms.Label lblPs;
        private System.Windows.Forms.Label lblPe;
        private System.Windows.Forms.Label lblPt;
        private System.Windows.Forms.Button btnPtzPresetScanStart;
        private System.Windows.Forms.Button btnPtzPresetScanStop;
        private System.Windows.Forms.GroupBox gbxPtzArea;
        private System.Windows.Forms.Label lblAn;
        private System.Windows.Forms.Label lblAhs;
        private System.Windows.Forms.Label lblAhe;
        private System.Windows.Forms.Label lblAhi;
        private System.Windows.Forms.Label lblAvs;
        private System.Windows.Forms.Label lblAve;
        private System.Windows.Forms.Label lblAvi;
        private System.Windows.Forms.Label lblAtm;
        private System.Windows.Forms.Button btnPtzAreaSetBound;
        private System.Windows.Forms.Button btnPtzAreaSetInterval;
        private System.Windows.Forms.Label lblAs;
        private System.Windows.Forms.Label lblAe;
        private System.Windows.Forms.Button btnPtzAreaScanStart;
        private System.Windows.Forms.Button btnPtzAreaScanStop;
        private System.Windows.Forms.GroupBox gbxPtzQuery;
        private System.Windows.Forms.Button btnPtzQueryStatus;
        private System.Windows.Forms.Button btnPtzQueryMode;
        private System.Windows.Forms.Label lblRti;
        private System.Windows.Forms.Button btnPtzRealtimeAngleOn;
        private System.Windows.Forms.Button btnPtzRealtimeAngleOff;
        private System.Windows.Forms.GroupBox gbxPtzRaw;
        private System.Windows.Forms.Button btnPtzSendRaw;
        private System.Windows.Forms.GroupBox gbxPtzSupplement;
        private System.Windows.Forms.GroupBox gbxPtzExtBasic;
        private System.Windows.Forms.GroupBox gbxPtzExtArea;
        private System.Windows.Forms.GroupBox gbxPtzExtPreset;
        private System.Windows.Forms.GroupBox gbxPtzExtQuery;
        private System.Windows.Forms.GroupBox gbxPtzExtMaintenance;
        private System.Windows.Forms.TabControl tabPtzSupplementCommands;
        private System.Windows.Forms.TabPage tabPtzSupplementBasic;
        private System.Windows.Forms.TabPage tabPtzSupplementArea;
        private System.Windows.Forms.TabPage tabPtzSupplementPreset;
        private System.Windows.Forms.TabPage tabPtzSupplementQuery;
        private System.Windows.Forms.TabPage tabPtzSupplementZero;
        private System.Windows.Forms.FlowLayoutPanel flpPtzSupplementBasic;
        private System.Windows.Forms.FlowLayoutPanel flpPtzSupplementArea;
        private System.Windows.Forms.FlowLayoutPanel flpPtzSupplementPreset;
        private System.Windows.Forms.FlowLayoutPanel flpPtzSupplementQuery;
        private System.Windows.Forms.FlowLayoutPanel flpPtzSupplementZero;
        private System.Windows.Forms.Button btnPtzSupDirUp;
        private System.Windows.Forms.Button btnPtzSupDirDown;
        private System.Windows.Forms.Button btnPtzSupDirLeft;
        private System.Windows.Forms.Button btnPtzSupDirRight;
        private System.Windows.Forms.Button btnPtzSupDirLeftUp;
        private System.Windows.Forms.Button btnPtzSupDirRightUp;
        private System.Windows.Forms.Button btnPtzSupDirLeftDown;
        private System.Windows.Forms.Button btnPtzSupDirRightDown;
        private System.Windows.Forms.Button btnPtzSupStop;
        private System.Windows.Forms.Button btnPtzSupLocateH;
        private System.Windows.Forms.Button btnPtzSupLocateV;
        private System.Windows.Forms.Button btnPtzSupQueryHAngle;
        private System.Windows.Forms.Button btnPtzSupQueryVAngle;
        private System.Windows.Forms.Button btnPtzSupPower1On;
        private System.Windows.Forms.Button btnPtzSupPower2On;
        private System.Windows.Forms.Button btnPtzSupPower1Off;
        private System.Windows.Forms.Button btnPtzSupPower2Off;
        private System.Windows.Forms.Button btnPtzSupReturnZero;
        private System.Windows.Forms.Button btnPtzSupAreaAngleHa;
        private System.Windows.Forms.Button btnPtzSupAreaAngleHb;
        private System.Windows.Forms.Button btnPtzSupAreaAngleVa;
        private System.Windows.Forms.Button btnPtzSupAreaAngleVb;
        private System.Windows.Forms.Button btnPtzSupAreaVideoHa;
        private System.Windows.Forms.Button btnPtzSupAreaVideoHb;
        private System.Windows.Forms.Button btnPtzSupAreaVideoVa;
        private System.Windows.Forms.Button btnPtzSupAreaVideoVb;
        private System.Windows.Forms.Button btnPtzSupAreaIntervalH;
        private System.Windows.Forms.Button btnPtzSupAreaIntervalV;
        private System.Windows.Forms.Button btnPtzSupAreaSetSpeed;
        private System.Windows.Forms.Button btnPtzSupAreaSetTime;
        private System.Windows.Forms.Button btnPtzSupAreaEnable;
        private System.Windows.Forms.Button btnPtzSupAreaDisable;
        private System.Windows.Forms.Button btnPtzSupAreaStartSingle;
        private System.Windows.Forms.Button btnPtzSupAreaStartMulti;
        private System.Windows.Forms.Button btnPtzSupAreaPause;
        private System.Windows.Forms.Button btnPtzSupAreaContinue;
        private System.Windows.Forms.Button btnPtzSupAreaClose;
        private System.Windows.Forms.Button btnPtzSupAreaModeStep;
        private System.Windows.Forms.Button btnPtzSupAreaModeContinuous;
        private System.Windows.Forms.Button btnPtzSupAreaSave;
        private System.Windows.Forms.Button btnPtzSupAreaQuery;
        private System.Windows.Forms.Button btnPtzSupAreaEndReturnOn;
        private System.Windows.Forms.Button btnPtzSupAreaEndReturnOff;
        private System.Windows.Forms.Button btnPtzSupAreaStepReturnOn;
        private System.Windows.Forms.Button btnPtzSupAreaStepReturnOff;
        private System.Windows.Forms.Button btnPtzSupPresetStandardSet;
        private System.Windows.Forms.Button btnPtzSupPresetStandardCall;
        private System.Windows.Forms.Button btnPtzSupPresetStandardDelete;
        private System.Windows.Forms.Button btnPtzSupPresetSetByAngle;
        private System.Windows.Forms.Button btnPtzSupPresetSetHAngle;
        private System.Windows.Forms.Button btnPtzSupPresetSetVAngle;
        private System.Windows.Forms.Button btnPtzSupPresetSetTime;
        private System.Windows.Forms.Button btnPtzSupPresetSetSpeed;
        private System.Windows.Forms.Button btnPtzSupPresetStart;
        private System.Windows.Forms.Button btnPtzSupPresetPause;
        private System.Windows.Forms.Button btnPtzSupPresetContinue;
        private System.Windows.Forms.Button btnPtzSupPresetClose;
        private System.Windows.Forms.Button btnPtzSupPresetEndReturnOn;
        private System.Windows.Forms.Button btnPtzSupPresetEndReturnOff;
        private System.Windows.Forms.Button btnPtzSupPresetArriveReturnOn;
        private System.Windows.Forms.Button btnPtzSupPresetArriveReturnOff;
        private System.Windows.Forms.Button btnPtzSupPresetCallReturnOn;
        private System.Windows.Forms.Button btnPtzSupPresetCallReturnOff;
        private System.Windows.Forms.Button btnPtzSupAckOn;
        private System.Windows.Forms.Button btnPtzSupAckOff;
        private System.Windows.Forms.Button btnPtzSupQueryMode;
        private System.Windows.Forms.Button btnPtzSupQueryStatus;
        private System.Windows.Forms.Button btnPtzSupQueryTemperature;
        private System.Windows.Forms.Button btnPtzSupQueryVoltage;
        private System.Windows.Forms.Button btnPtzSupQueryCurrent;
        private System.Windows.Forms.Button btnPtzSupAngleRealtimeOn;
        private System.Windows.Forms.Button btnPtzSupAngleRealtimeOff;
        private System.Windows.Forms.Button btnPtzSupQueryHSpeed;
        private System.Windows.Forms.Button btnPtzSupQueryVSpeed;
        private System.Windows.Forms.Button btnPtzSupQueryAllSpeed;
        private System.Windows.Forms.Button btnPtzSupSpeedRealtimeOn;
        private System.Windows.Forms.Button btnPtzSupSpeedRealtimeOff;
        private System.Windows.Forms.Button btnPtzSupLocateReturnOn;
        private System.Windows.Forms.Button btnPtzSupLocateReturnOff;
        private System.Windows.Forms.Button btnPtzSupZeroHCurrent;
        private System.Windows.Forms.Button btnPtzSupZeroVCurrent;
        private System.Windows.Forms.Button btnPtzSupZeroHvCurrent;
        private System.Windows.Forms.Button btnPtzSupZeroHAngle;
        private System.Windows.Forms.Button btnPtzSupZeroVAngle;
        private System.Windows.Forms.Button btnPtzSupZeroDelete;
        private System.Windows.Forms.Button btnPtzSupReboot;
        private System.Windows.Forms.Button btnPtzSupSelfCheck;
        private System.Windows.Forms.Button btn_UseZoom;
    }
}

