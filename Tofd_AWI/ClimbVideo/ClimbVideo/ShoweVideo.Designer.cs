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
            this.Pho_Video_Usb = new System.Windows.Forms.PictureBox();
            this.Time_Exit = new System.Windows.Forms.Timer(this.components);
            this.Img_LR = new System.Windows.Forms.ImageList(this.components);
            this.Time_Show = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Pic_Video = new System.Windows.Forms.PictureBox();
            this.Lb_RunMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Bt_Wave_Show = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Pic_Big = new System.Windows.Forms.PictureBox();
            this.Lb_Err_2 = new System.Windows.Forms.Label();
            this.Lb_Err = new System.Windows.Forms.Label();
            this.Bt_Video = new System.Windows.Forms.Button();
            this.Time_RunTime = new System.Windows.Forms.Timer(this.components);
            this.Time_ReLink = new System.Windows.Forms.Timer(this.components);
            this.Time_ReLink_2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Pho_Video_Usb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Video)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Big)).BeginInit();
            this.SuspendLayout();
            // 
            // Pho_Video_Usb
            // 
            this.Pho_Video_Usb.BackColor = System.Drawing.Color.Black;
            this.Pho_Video_Usb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pho_Video_Usb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Pho_Video_Usb.Location = new System.Drawing.Point(0, 0);
            this.Pho_Video_Usb.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Pho_Video_Usb.Name = "Pho_Video_Usb";
            this.Pho_Video_Usb.Size = new System.Drawing.Size(1108, 441);
            this.Pho_Video_Usb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pho_Video_Usb.TabIndex = 1;
            this.Pho_Video_Usb.TabStop = false;
            this.Pho_Video_Usb.Click += new System.EventHandler(this.Pho_Video_Usb_Click);
            this.Pho_Video_Usb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Pho_Video_Usb_MouseMove);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(504, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 64);
            this.button1.TabIndex = 56;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
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
            this.Lb_RunMsg.Location = new System.Drawing.Point(0, 15);
            this.Lb_RunMsg.Name = "Lb_RunMsg";
            this.Lb_RunMsg.Size = new System.Drawing.Size(0, 24);
            this.Lb_RunMsg.TabIndex = 60;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.Bt_Wave_Show);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.Pic_Big);
            this.panel1.Controls.Add(this.Lb_Err_2);
            this.panel1.Controls.Add(this.Lb_Err);
            this.panel1.Controls.Add(this.Bt_Video);
            this.panel1.Controls.Add(this.Lb_RunMsg);
            this.panel1.Controls.Add(this.Pho_Video_Usb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1108, 441);
            this.panel1.TabIndex = 61;
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
            this.Bt_Wave_Show.Location = new System.Drawing.Point(1063, 387);
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
            // Frm_Video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1108, 441);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
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
            ((System.ComponentModel.ISupportInitialize)(this.Pho_Video_Usb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Video)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Big)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Pho_Video_Usb;
        private System.Windows.Forms.Timer Time_Exit;
        private System.Windows.Forms.ImageList Img_LR;
        private System.Windows.Forms.Timer Time_Show;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
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
    }
}

