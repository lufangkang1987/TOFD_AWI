namespace ClimbVideo
{
    partial class Frm_Video
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Video));
            this.Pic_L = new System.Windows.Forms.PictureBox();
            this.Time_Exit = new System.Windows.Forms.Timer(this.components);
            this.Img_LR = new System.Windows.Forms.ImageList(this.components);
            this.Time_Show = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.butt4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Pic_Video = new System.Windows.Forms.PictureBox();
            this.Lb_RunMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Tab_Yay_Pan_1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Lab_Video_3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.StateLabel = new System.Windows.Forms.Label();
            this.buttonSnap = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.Bt_Wave_Show = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Pic_Big = new System.Windows.Forms.PictureBox();
            this.Lb_Err_2 = new System.Windows.Forms.Label();
            this.Lb_Err = new System.Windows.Forms.Label();
            this.Bt_Video = new System.Windows.Forms.Button();
            this.Time_RunTime = new System.Windows.Forms.Timer(this.components);
            this.Time_ReLink = new System.Windows.Forms.Timer(this.components);
            this.Time_ReLink_2 = new System.Windows.Forms.Timer(this.components);
            this.Time_R = new System.Windows.Forms.Timer(this.components);
            this.Tb_Lay_Pan = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Lab_Video_4 = new System.Windows.Forms.Label();
            this.Pic_R = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Tab_Main = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.Tab_U = new System.Windows.Forms.TableLayoutPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.Lab_Video_1 = new System.Windows.Forms.Label();
            this.Pic_Q = new System.Windows.Forms.PictureBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.Lab_Video_2 = new System.Windows.Forms.Label();
            this.Pic_H = new System.Windows.Forms.PictureBox();
            this.panel6 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Video)).BeginInit();
            this.panel1.SuspendLayout();
            this.Tab_Yay_Pan_1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Big)).BeginInit();
            this.Tb_Lay_Pan.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_R)).BeginInit();
            this.Tab_Main.SuspendLayout();
            this.panel5.SuspendLayout();
            this.Tab_U.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Q)).BeginInit();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_H)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pic_L
            // 
            this.Pic_L.BackColor = System.Drawing.Color.Black;
            this.Pic_L.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pic_L.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_L.Location = new System.Drawing.Point(0, 0);
            this.Pic_L.Margin = new System.Windows.Forms.Padding(0);
            this.Pic_L.Name = "Pic_L";
            this.Pic_L.Size = new System.Drawing.Size(553, 151);
            this.Pic_L.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_L.TabIndex = 1;
            this.Pic_L.TabStop = false;
            this.Pic_L.Click += new System.EventHandler(this.Pho_Video_Usb_Click);
            this.Pic_L.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Pho_Video_Usb_MouseMove);
            // 
            // Time_Exit
            // 
            this.Time_Exit.Interval = 10;
            this.Time_Exit.Tick += new System.EventHandler(this.Time_Exit_Tick);
            // 
            // Img_LR
            // 
            this.Img_LR.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Img_LR.ImageStream")));
            this.Img_LR.TransparentColor = System.Drawing.Color.Transparent;
            this.Img_LR.Images.SetKeyName(0, "csszdk.png");
            this.Img_LR.Images.SetKeyName(1, "csszsq.png");
            // 
            // Time_Show
            // 
            this.Time_Show.Interval = 1;
            this.Time_Show.Tick += new System.EventHandler(this.Time_Show_Tick);
            // 
            // butt4
            // 
            this.butt4.Location = new System.Drawing.Point(0, 160);
            this.butt4.Name = "butt4";
            this.butt4.Size = new System.Drawing.Size(132, 64);
            this.butt4.TabIndex = 56;
            this.butt4.Text = "button1";
            this.butt4.UseVisualStyleBackColor = true;
            this.butt4.Visible = false;
            this.butt4.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(244, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 30);
            this.textBox1.TabIndex = 57;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(373, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 30);
            this.textBox2.TabIndex = 58;
            this.textBox2.Visible = false;
            // 
            // Pic_Video
            // 
            this.Pic_Video.BackColor = System.Drawing.Color.Transparent;
            this.Pic_Video.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Pic_Video.BackgroundImage")));
            this.Pic_Video.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pic_Video.Location = new System.Drawing.Point(-13, 47);
            this.Pic_Video.Name = "Pic_Video";
            this.Pic_Video.Size = new System.Drawing.Size(12, 12);
            this.Pic_Video.TabIndex = 59;
            this.Pic_Video.TabStop = false;
            this.Pic_Video.Visible = false;
            // 
            // Lb_RunMsg
            // 
            this.Lb_RunMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Lb_RunMsg.AutoSize = true;
            this.Lb_RunMsg.BackColor = System.Drawing.Color.Transparent;
            this.Lb_RunMsg.Font = new System.Drawing.Font("黑体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lb_RunMsg.ForeColor = System.Drawing.Color.Transparent;
            this.Lb_RunMsg.Location = new System.Drawing.Point(0, -205);
            this.Lb_RunMsg.Name = "Lb_RunMsg";
            this.Lb_RunMsg.Size = new System.Drawing.Size(0, 24);
            this.Lb_RunMsg.TabIndex = 60;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.Tab_Yay_Pan_1);
            this.panel1.Controls.Add(this.butt4);
            this.panel1.Controls.Add(this.Bt_Wave_Show);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.Pic_Big);
            this.panel1.Controls.Add(this.Lb_Err_2);
            this.panel1.Controls.Add(this.Lb_Err);
            this.panel1.Controls.Add(this.Bt_Video);
            this.panel1.Controls.Add(this.Lb_RunMsg);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 221);
            this.panel1.TabIndex = 61;
            // 
            // Tab_Yay_Pan_1
            // 
            this.Tab_Yay_Pan_1.ColumnCount = 1;
            this.Tab_Yay_Pan_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Tab_Yay_Pan_1.Controls.Add(this.panel3, 0, 1);
            this.Tab_Yay_Pan_1.Controls.Add(this.panel4, 0, 0);
            this.Tab_Yay_Pan_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Yay_Pan_1.Location = new System.Drawing.Point(0, 0);
            this.Tab_Yay_Pan_1.Margin = new System.Windows.Forms.Padding(0);
            this.Tab_Yay_Pan_1.Name = "Tab_Yay_Pan_1";
            this.Tab_Yay_Pan_1.RowCount = 2;
            this.Tab_Yay_Pan_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.Tab_Yay_Pan_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Tab_Yay_Pan_1.Size = new System.Drawing.Size(553, 221);
            this.Tab_Yay_Pan_1.TabIndex = 67;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Lab_Video_3);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.Pic_L);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 70);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(553, 151);
            this.panel3.TabIndex = 0;
            // 
            // Lab_Video_3
            // 
            this.Lab_Video_3.AutoSize = true;
            this.Lab_Video_3.BackColor = System.Drawing.Color.Black;
            this.Lab_Video_3.Font = new System.Drawing.Font("黑体", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Video_3.ForeColor = System.Drawing.Color.Red;
            this.Lab_Video_3.Location = new System.Drawing.Point(17, 28);
            this.Lab_Video_3.Name = "Lab_Video_3";
            this.Lab_Video_3.Size = new System.Drawing.Size(364, 43);
            this.Lab_Video_3.TabIndex = 4;
            this.Lab_Video_3.Text = "Start recording";
            this.Lab_Video_3.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(391, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 38);
            this.button1.TabIndex = 2;
            this.button1.Text = "录像";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.StateLabel);
            this.panel4.Controls.Add(this.buttonSnap);
            this.panel4.Controls.Add(this.buttonStop);
            this.panel4.Controls.Add(this.buttonPlay);
            this.panel4.Controls.Add(this.buttonSettings);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(547, 64);
            this.panel4.TabIndex = 1;
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.ForeColor = System.Drawing.Color.White;
            this.StateLabel.Location = new System.Drawing.Point(7, 46);
            this.StateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(69, 20);
            this.StateLabel.TabIndex = 9;
            this.StateLabel.Text = "状态栏";
            // 
            // buttonSnap
            // 
            this.buttonSnap.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSnap.Location = new System.Drawing.Point(401, 6);
            this.buttonSnap.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSnap.Name = "buttonSnap";
            this.buttonSnap.Size = new System.Drawing.Size(68, 29);
            this.buttonSnap.TabIndex = 8;
            this.buttonSnap.Text = "抓图";
            this.buttonSnap.UseVisualStyleBackColor = true;
            this.buttonSnap.Visible = false;
            this.buttonSnap.Click += new System.EventHandler(this.buttonSnap_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonStop.Location = new System.Drawing.Point(305, 6);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(68, 29);
            this.buttonStop.TabIndex = 7;
            this.buttonStop.Text = "停止";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Visible = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPlay.Location = new System.Drawing.Point(208, 6);
            this.buttonPlay.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(68, 29);
            this.buttonPlay.TabIndex = 6;
            this.buttonPlay.Text = "播放";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Visible = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonSettings.Location = new System.Drawing.Point(65, 6);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(100, 29);
            this.buttonSettings.TabIndex = 5;
            this.buttonSettings.Text = "相机设置";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Visible = false;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // Bt_Wave_Show
            // 
            this.Bt_Wave_Show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_Wave_Show.BackColor = System.Drawing.Color.Black;
            this.Bt_Wave_Show.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Bt_Wave_Show.BackgroundImage")));
            this.Bt_Wave_Show.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bt_Wave_Show.FlatAppearance.BorderSize = 0;
            this.Bt_Wave_Show.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bt_Wave_Show.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Wave_Show.ForeColor = System.Drawing.Color.Transparent;
            this.Bt_Wave_Show.Location = new System.Drawing.Point(508, 167);
            this.Bt_Wave_Show.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Bt_Wave_Show.Name = "Bt_Wave_Show";
            this.Bt_Wave_Show.Size = new System.Drawing.Size(40, 54);
            this.Bt_Wave_Show.TabIndex = 66;
            this.Bt_Wave_Show.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bt_Wave_Show.UseVisualStyleBackColor = false;
            this.Bt_Wave_Show.Visible = false;
            this.Bt_Wave_Show.Click += new System.EventHandler(this.Bt_Wave_Show_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(916, 199);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 65;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Pic_Big
            // 
            this.Pic_Big.Location = new System.Drawing.Point(812, 4);
            this.Pic_Big.Name = "Pic_Big";
            this.Pic_Big.Size = new System.Drawing.Size(174, 164);
            this.Pic_Big.TabIndex = 64;
            this.Pic_Big.TabStop = false;
            this.Pic_Big.Visible = false;
            // 
            // Lb_Err_2
            // 
            this.Lb_Err_2.AutoSize = true;
            this.Lb_Err_2.Font = new System.Drawing.Font("黑体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lb_Err_2.ForeColor = System.Drawing.Color.Red;
            this.Lb_Err_2.Location = new System.Drawing.Point(288, 227);
            this.Lb_Err_2.Name = "Lb_Err_2";
            this.Lb_Err_2.Size = new System.Drawing.Size(130, 24);
            this.Lb_Err_2.TabIndex = 63;
            this.Lb_Err_2.Text = "视频2 异常";
            this.Lb_Err_2.Visible = false;
            // 
            // Lb_Err
            // 
            this.Lb_Err.AutoSize = true;
            this.Lb_Err.Font = new System.Drawing.Font("黑体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lb_Err.ForeColor = System.Drawing.Color.Red;
            this.Lb_Err.Location = new System.Drawing.Point(279, 160);
            this.Lb_Err.Name = "Lb_Err";
            this.Lb_Err.Size = new System.Drawing.Size(142, 24);
            this.Lb_Err.TabIndex = 62;
            this.Lb_Err.Text = "视频 1 异常";
            this.Lb_Err.Visible = false;
            // 
            // Bt_Video
            // 
            this.Bt_Video.BackColor = System.Drawing.Color.Transparent;
            this.Bt_Video.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Bt_Video.BackgroundImage")));
            this.Bt_Video.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Bt_Video.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Bt_Video.ForeColor = System.Drawing.Color.Transparent;
            this.Bt_Video.Location = new System.Drawing.Point(3, 29);
            this.Bt_Video.Name = "Bt_Video";
            this.Bt_Video.Size = new System.Drawing.Size(18, 20);
            this.Bt_Video.TabIndex = 61;
            this.Bt_Video.UseVisualStyleBackColor = false;
            this.Bt_Video.Visible = false;
            // 
            // Time_RunTime
            // 
            this.Time_RunTime.Interval = 20;
            this.Time_RunTime.Tick += new System.EventHandler(this.Time_RunTime_Tick);
            // 
            // Time_R
            // 
            this.Time_R.Tick += new System.EventHandler(this.Time_R_Tick);
            // 
            // Tb_Lay_Pan
            // 
            this.Tb_Lay_Pan.ColumnCount = 2;
            this.Tb_Lay_Pan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.09025F));
            this.Tb_Lay_Pan.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.90975F));
            this.Tb_Lay_Pan.Controls.Add(this.panel1, 0, 0);
            this.Tb_Lay_Pan.Controls.Add(this.panel2, 1, 0);
            this.Tb_Lay_Pan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tb_Lay_Pan.Location = new System.Drawing.Point(0, 0);
            this.Tb_Lay_Pan.Margin = new System.Windows.Forms.Padding(0, 1, 1, 1);
            this.Tb_Lay_Pan.Name = "Tb_Lay_Pan";
            this.Tb_Lay_Pan.RowCount = 1;
            this.Tb_Lay_Pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tb_Lay_Pan.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 221F));
            this.Tb_Lay_Pan.Size = new System.Drawing.Size(1108, 221);
            this.Tb_Lay_Pan.TabIndex = 62;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Lab_Video_4);
            this.panel2.Controls.Add(this.Pic_R);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(554, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(554, 221);
            this.panel2.TabIndex = 62;
            // 
            // Lab_Video_4
            // 
            this.Lab_Video_4.AutoSize = true;
            this.Lab_Video_4.BackColor = System.Drawing.Color.Black;
            this.Lab_Video_4.Font = new System.Drawing.Font("黑体", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Video_4.ForeColor = System.Drawing.Color.Red;
            this.Lab_Video_4.Location = new System.Drawing.Point(54, 90);
            this.Lab_Video_4.Name = "Lab_Video_4";
            this.Lab_Video_4.Size = new System.Drawing.Size(364, 43);
            this.Lab_Video_4.TabIndex = 3;
            this.Lab_Video_4.Text = "Start recording";
            this.Lab_Video_4.Visible = false;
            this.Lab_Video_4.Click += new System.EventHandler(this.Lab_Video_4_Click);
            // 
            // Pic_R
            // 
            this.Pic_R.BackColor = System.Drawing.Color.Black;
            this.Pic_R.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pic_R.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_R.Location = new System.Drawing.Point(0, 0);
            this.Pic_R.Margin = new System.Windows.Forms.Padding(0);
            this.Pic_R.Name = "Pic_R";
            this.Pic_R.Size = new System.Drawing.Size(554, 221);
            this.Pic_R.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_R.TabIndex = 2;
            this.Pic_R.TabStop = false;
            this.Pic_R.Click += new System.EventHandler(this.Pic_R_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Tab_Main
            // 
            this.Tab_Main.ColumnCount = 1;
            this.Tab_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tab_Main.Controls.Add(this.panel5, 0, 0);
            this.Tab_Main.Controls.Add(this.panel6, 0, 1);
            this.Tab_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Main.Location = new System.Drawing.Point(0, 0);
            this.Tab_Main.Margin = new System.Windows.Forms.Padding(1);
            this.Tab_Main.Name = "Tab_Main";
            this.Tab_Main.RowCount = 2;
            this.Tab_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tab_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tab_Main.Size = new System.Drawing.Size(1108, 441);
            this.Tab_Main.TabIndex = 63;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.Tab_U);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1108, 219);
            this.panel5.TabIndex = 0;
            // 
            // Tab_U
            // 
            this.Tab_U.ColumnCount = 2;
            this.Tab_U.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tab_U.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tab_U.Controls.Add(this.panel7, 0, 0);
            this.Tab_U.Controls.Add(this.panel8, 1, 0);
            this.Tab_U.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_U.Location = new System.Drawing.Point(0, 0);
            this.Tab_U.Margin = new System.Windows.Forms.Padding(0, 1, 1, 1);
            this.Tab_U.Name = "Tab_U";
            this.Tab_U.RowCount = 1;
            this.Tab_U.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Tab_U.Size = new System.Drawing.Size(1108, 219);
            this.Tab_U.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.button3);
            this.panel7.Controls.Add(this.Lab_Video_1);
            this.panel7.Controls.Add(this.Pic_Q);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(553, 219);
            this.panel7.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(465, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Lab_Video_1
            // 
            this.Lab_Video_1.AutoSize = true;
            this.Lab_Video_1.BackColor = System.Drawing.Color.Black;
            this.Lab_Video_1.Font = new System.Drawing.Font("黑体", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Video_1.ForeColor = System.Drawing.Color.Red;
            this.Lab_Video_1.Location = new System.Drawing.Point(70, 72);
            this.Lab_Video_1.Name = "Lab_Video_1";
            this.Lab_Video_1.Size = new System.Drawing.Size(364, 43);
            this.Lab_Video_1.TabIndex = 4;
            this.Lab_Video_1.Text = "Start recording";
            this.Lab_Video_1.Visible = false;
            // 
            // Pic_Q
            // 
            this.Pic_Q.BackColor = System.Drawing.Color.Black;
            this.Pic_Q.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pic_Q.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_Q.Location = new System.Drawing.Point(0, 0);
            this.Pic_Q.Margin = new System.Windows.Forms.Padding(0);
            this.Pic_Q.Name = "Pic_Q";
            this.Pic_Q.Size = new System.Drawing.Size(553, 219);
            this.Pic_Q.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_Q.TabIndex = 3;
            this.Pic_Q.TabStop = false;
            this.Pic_Q.Click += new System.EventHandler(this.Pic_Q_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.Lab_Video_2);
            this.panel8.Controls.Add(this.Pic_H);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(554, 0);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(554, 219);
            this.panel8.TabIndex = 1;
            // 
            // Lab_Video_2
            // 
            this.Lab_Video_2.AutoSize = true;
            this.Lab_Video_2.BackColor = System.Drawing.Color.Black;
            this.Lab_Video_2.Font = new System.Drawing.Font("黑体", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Video_2.ForeColor = System.Drawing.Color.Red;
            this.Lab_Video_2.Location = new System.Drawing.Point(70, 72);
            this.Lab_Video_2.Name = "Lab_Video_2";
            this.Lab_Video_2.Size = new System.Drawing.Size(364, 43);
            this.Lab_Video_2.TabIndex = 6;
            this.Lab_Video_2.Text = "Start recording";
            this.Lab_Video_2.Visible = false;
            // 
            // Pic_H
            // 
            this.Pic_H.BackColor = System.Drawing.Color.Black;
            this.Pic_H.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pic_H.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pic_H.Location = new System.Drawing.Point(0, 0);
            this.Pic_H.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Pic_H.Name = "Pic_H";
            this.Pic_H.Size = new System.Drawing.Size(554, 219);
            this.Pic_H.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_H.TabIndex = 5;
            this.Pic_H.TabStop = false;
            this.Pic_H.Click += new System.EventHandler(this.Pic_H_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.Tb_Lay_Pan);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 220);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1108, 221);
            this.panel6.TabIndex = 1;
            // 
            // Frm_Video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1108, 441);
            this.ControlBox = false;
            this.Controls.Add(this.Tab_Main);
            this.Controls.Add(this.Pic_Video);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Video";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Video_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Video_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pic_L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Video)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.Tab_Yay_Pan_1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Big)).EndInit();
            this.Tb_Lay_Pan.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_R)).EndInit();
            this.Tab_Main.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.Tab_U.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Q)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_H)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Pic_L;
        private System.Windows.Forms.Timer Time_Exit;
        private System.Windows.Forms.ImageList Img_LR;
        private System.Windows.Forms.Timer Time_Show;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button butt4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox Pic_Video;
        private System.Windows.Forms.Label Lb_RunMsg;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Bt_Video;
        private System.Windows.Forms.Timer Time_RunTime;
        private System.Windows.Forms.Label Lb_Err;
        private System.Windows.Forms.Label Lb_Err_2;
        private System.Windows.Forms.Timer Time_ReLink;
        private System.Windows.Forms.Timer Time_ReLink_2;
        private System.Windows.Forms.PictureBox Pic_Big;
     //   private FrameWork.Class.CrystalButton crystalButton1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Bt_Wave_Show;
        private System.Windows.Forms.Timer Time_R;
        private System.Windows.Forms.TableLayoutPanel Tb_Lay_Pan;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox Pic_R;
        private System.Windows.Forms.TableLayoutPanel Tab_Yay_Pan_1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonSnap;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TableLayoutPanel Tab_Main;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox Pic_Q;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label Lab_Video_3;
        private System.Windows.Forms.Label Lab_Video_4;
        private System.Windows.Forms.Label Lab_Video_1;
        private System.Windows.Forms.TableLayoutPanel Tab_U;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label Lab_Video_2;
        private System.Windows.Forms.PictureBox Pic_H;
        private System.Windows.Forms.Button button3;
    }
}

