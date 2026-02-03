namespace RadarConnect
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_SetCoordinate = new System.Windows.Forms.Button();
            this.cbx_Coordinate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_StopSample = new System.Windows.Forms.Button();
            this.btn_StartSample = new System.Windows.Forms.Button();
            this.btn_SetMode = new System.Windows.Forms.Button();
            this.cbx_WorkMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbl_SampleState = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox2.Size = new System.Drawing.Size(200, 320);
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
            this.cbx_Coordinate.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "坐标系：";
            // 
            // btn_StopSample
            // 
            this.btn_StopSample.Location = new System.Drawing.Point(17, 156);
            this.btn_StopSample.Name = "btn_StopSample";
            this.btn_StopSample.Size = new System.Drawing.Size(160, 30);
            this.btn_StopSample.TabIndex = 4;
            this.btn_StopSample.Text = "停止采样";
            this.btn_StopSample.UseVisualStyleBackColor = true;
            this.btn_StopSample.Click += new System.EventHandler(this.btn_StopSample_Click_1);
            // 
            // btn_StartSample
            // 
            this.btn_StartSample.Location = new System.Drawing.Point(17, 120);
            this.btn_StartSample.Name = "btn_StartSample";
            this.btn_StartSample.Size = new System.Drawing.Size(160, 30);
            this.btn_StartSample.TabIndex = 3;
            this.btn_StartSample.Text = "开始采样";
            this.btn_StartSample.UseVisualStyleBackColor = true;
            this.btn_StartSample.Click += new System.EventHandler(this.btn_StartSample_Click_1);
            // 
            // btn_SetMode
            // 
            this.btn_SetMode.Location = new System.Drawing.Point(17, 75);
            this.btn_SetMode.Name = "btn_SetMode";
            this.btn_SetMode.Size = new System.Drawing.Size(160, 30);
            this.btn_SetMode.TabIndex = 2;
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
            this.cbx_WorkMode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "工作模式：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox_Log);
            this.groupBox3.Location = new System.Drawing.Point(179, 218);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(560, 300);
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
            this.listBox_Log.Size = new System.Drawing.Size(554, 280);
            this.listBox_Log.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_SampleState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 540);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1334, 22);
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
            this.ClientSize = new System.Drawing.Size(1334, 562);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Livox Radar Demo (Refined)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.ListBox listBox_Log; // 已修改为 ListBox
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_SampleState;
        private System.Windows.Forms.Button btn_SetCoordinate;
        private System.Windows.Forms.ComboBox cbx_Coordinate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}