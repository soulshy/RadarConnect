namespace RadarConnect
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_SaveImage = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker_Query = new System.Windows.Forms.DateTimePicker();
            this.btn_Reconstruct = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_CameraIp = new System.Windows.Forms.TextBox();
            this.btn_PlayCamera = new System.Windows.Forms.Button();
            this.panel_Video = new System.Windows.Forms.Panel();
            this.renderWindowControl1 = new Kitware.VTK.RenderWindowControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_ExecuteFusion = new System.Windows.Forms.Button();
            this.btn_SelectVideo = new System.Windows.Forms.Button();
            this.txt_VideoPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker_Fusion = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox_FusionResult = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbl_SampleState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FusionResult)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1969, 800);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1961, 774);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "控制与日志";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.listBox_Log);
            this.groupBox3.Location = new System.Drawing.Point(12, 218);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(864, 535);
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
            this.listBox_Log.Size = new System.Drawing.Size(858, 515);
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
            this.groupBox2.Location = new System.Drawing.Point(882, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 280);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "雷达控制";
            // 
            // btn_SetCoordinate
            // 
            this.btn_SetCoordinate.Location = new System.Drawing.Point(135, 240);
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
            this.cbx_Coordinate.Location = new System.Drawing.Point(17, 242);
            this.cbx_Coordinate.Name = "cbx_Coordinate";
            this.cbx_Coordinate.Size = new System.Drawing.Size(110, 20);
            this.cbx_Coordinate.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 227);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.panel_Video);
            this.tabPage2.Controls.Add(this.renderWindowControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1961, 774);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "可视化视图";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btn_SaveImage);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.dateTimePicker_Query);
            this.groupBox5.Controls.Add(this.btn_Reconstruct);
            this.groupBox5.Location = new System.Drawing.Point(6, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(922, 60);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "点云还原查询";
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
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txt_CameraIp);
            this.groupBox4.Controls.Add(this.btn_PlayCamera);
            this.groupBox4.Location = new System.Drawing.Point(1022, 9);
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
            // panel_Video
            // 
            this.panel_Video.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Video.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Video.Location = new System.Drawing.Point(1022, 75);
            this.panel_Video.Name = "panel_Video";
            this.panel_Video.Size = new System.Drawing.Size(922, 685);
            this.panel_Video.TabIndex = 6;
            // 
            // renderWindowControl1
            // 
            this.renderWindowControl1.AddTestActors = false;
            this.renderWindowControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderWindowControl1.Location = new System.Drawing.Point(6, 75);
            this.renderWindowControl1.Name = "renderWindowControl1";
            this.renderWindowControl1.Size = new System.Drawing.Size(922, 685);
            this.renderWindowControl1.TabIndex = 4;
            this.renderWindowControl1.TestText = null;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox_FusionResult);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1961, 774);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "点云CCD融合";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            this.groupBox6.Location = new System.Drawing.Point(8, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1945, 60);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "离线融合控制";
            // 
            // btn_ExecuteFusion
            // 
            this.btn_ExecuteFusion.Location = new System.Drawing.Point(750, 18);
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
            // pictureBox_FusionResult
            // 
            this.pictureBox_FusionResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_FusionResult.BackColor = System.Drawing.Color.Black;
            this.pictureBox_FusionResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_FusionResult.Location = new System.Drawing.Point(8, 72);
            this.pictureBox_FusionResult.Name = "pictureBox_FusionResult";
            this.pictureBox_FusionResult.Size = new System.Drawing.Size(1945, 694);
            this.pictureBox_FusionResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_FusionResult.TabIndex = 1;
            this.pictureBox_FusionResult.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_SampleState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 800);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1969, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbl_SampleState
            // 
            this.lbl_SampleState.Name = "lbl_SampleState";
            this.lbl_SampleState.Size = new System.Drawing.Size(56, 17);
            this.lbl_SampleState.Text = "系统就绪";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1969, 822);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Livox Radar Connect";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FusionResult)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Disconnect;
        private System.Windows.Forms.Button btn_HandShake;
        private System.Windows.Forms.Button btn_StopListen;
        private System.Windows.Forms.Button btn_StartListen;
        private System.Windows.Forms.ListView listView_Devices;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_StopSample;
        private System.Windows.Forms.Button btn_StartSample;
        private System.Windows.Forms.Button btn_SetMode;
        private System.Windows.Forms.ComboBox cbx_WorkMode;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox_Log;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_SampleState;
        private System.Windows.Forms.Button btn_SetCoordinate;
        private System.Windows.Forms.ComboBox cbx_Coordinate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_Reconstruct;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Query;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_SaveImage;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_CameraIp;
        private System.Windows.Forms.Button btn_PlayCamera;
        private Kitware.VTK.RenderWindowControl renderWindowControl1;
        private System.Windows.Forms.Panel panel_Video;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_ExecuteFusion;
        private System.Windows.Forms.Button btn_SelectVideo;
        private System.Windows.Forms.TextBox txt_VideoPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Fusion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox_FusionResult;
    }
}