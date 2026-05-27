namespace Tofd_AWI
{
    partial class Frm_Main_C_New
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
            this.Lb_LinkState = new System.Windows.Forms.Label();
            this.Lb_Distance = new System.Windows.Forms.Label();
            this.Lb_Speed = new System.Windows.Forms.Label();
            this.Btn_Link = new System.Windows.Forms.Button();
            this.Btn_Forward = new System.Windows.Forms.Button();
            this.Btn_Backward = new System.Windows.Forms.Button();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.Btn_Mark = new System.Windows.Forms.Button();
            this.Btn_StartScan = new System.Windows.Forms.Button();
            this.Btn_StopScan = new System.Windows.Forms.Button();
            this.Pic_AScan = new System.Windows.Forms.PictureBox();
            this.Pic_BScan = new System.Windows.Forms.PictureBox();
            this.Pic_CScan = new System.Windows.Forms.PictureBox();
            this.Pic_DScan = new System.Windows.Forms.PictureBox();
            this.Pic_InspectVideo = new System.Windows.Forms.PictureBox();
            this.Pic_OperatorVideo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_AScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_BScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_CScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_DScan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_InspectVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_OperatorVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // Lb_LinkState
            // 
            this.Lb_LinkState.Location = new System.Drawing.Point(10, 10);
            this.Lb_LinkState.Name = "Lb_LinkState";
            this.Lb_LinkState.Size = new System.Drawing.Size(150, 25);
            this.Lb_LinkState.TabIndex = 0;
            this.Lb_LinkState.Text = "未连接";
            // 
            // Lb_Distance
            // 
            this.Lb_Distance.Location = new System.Drawing.Point(200, 10);
            this.Lb_Distance.Name = "Lb_Distance";
            this.Lb_Distance.Size = new System.Drawing.Size(150, 25);
            this.Lb_Distance.TabIndex = 1;
            this.Lb_Distance.Text = "0.0 mm";
            // 
            // Lb_Speed
            // 
            this.Lb_Speed.Location = new System.Drawing.Point(400, 10);
            this.Lb_Speed.Name = "Lb_Speed";
            this.Lb_Speed.Size = new System.Drawing.Size(100, 25);
            this.Lb_Speed.TabIndex = 2;
            this.Lb_Speed.Text = "0%";
            // 
            // Btn_Link
            // 
            this.Btn_Link.Location = new System.Drawing.Point(10, 50);
            this.Btn_Link.Name = "Btn_Link";
            this.Btn_Link.Size = new System.Drawing.Size(100, 35);
            this.Btn_Link.TabIndex = 10;
            this.Btn_Link.Text = "连接";
            this.Btn_Link.UseVisualStyleBackColor = true;
            this.Btn_Link.Click += new System.EventHandler(this.Btn_Link_Click);
            // 
            // Btn_Forward
            // 
            this.Btn_Forward.Location = new System.Drawing.Point(120, 50);
            this.Btn_Forward.Name = "Btn_Forward";
            this.Btn_Forward.Size = new System.Drawing.Size(80, 35);
            this.Btn_Forward.TabIndex = 11;
            this.Btn_Forward.Text = "前进";
            this.Btn_Forward.UseVisualStyleBackColor = true;
            this.Btn_Forward.Click += new System.EventHandler(this.Btn_Forward_Click);
            // 
            // Btn_Backward
            // 
            this.Btn_Backward.Location = new System.Drawing.Point(210, 50);
            this.Btn_Backward.Name = "Btn_Backward";
            this.Btn_Backward.Size = new System.Drawing.Size(80, 35);
            this.Btn_Backward.TabIndex = 12;
            this.Btn_Backward.Text = "后退";
            this.Btn_Backward.UseVisualStyleBackColor = true;
            this.Btn_Backward.Click += new System.EventHandler(this.Btn_Backward_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Location = new System.Drawing.Point(300, 50);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(80, 35);
            this.Btn_Stop.TabIndex = 13;
            this.Btn_Stop.Text = "停止";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // Btn_Mark
            // 
            this.Btn_Mark.Location = new System.Drawing.Point(390, 50);
            this.Btn_Mark.Name = "Btn_Mark";
            this.Btn_Mark.Size = new System.Drawing.Size(80, 35);
            this.Btn_Mark.TabIndex = 14;
            this.Btn_Mark.Text = "打标";
            this.Btn_Mark.UseVisualStyleBackColor = true;
            this.Btn_Mark.Click += new System.EventHandler(this.Btn_Mark_Click);
            // 
            // Btn_StartScan
            // 
            this.Btn_StartScan.Location = new System.Drawing.Point(500, 50);
            this.Btn_StartScan.Name = "Btn_StartScan";
            this.Btn_StartScan.Size = new System.Drawing.Size(100, 35);
            this.Btn_StartScan.TabIndex = 15;
            this.Btn_StartScan.Text = "开始检测";
            this.Btn_StartScan.UseVisualStyleBackColor = true;
            this.Btn_StartScan.Click += new System.EventHandler(this.Btn_StartScan_Click);
            // 
            // Btn_StopScan
            // 
            this.Btn_StopScan.Location = new System.Drawing.Point(610, 50);
            this.Btn_StopScan.Name = "Btn_StopScan";
            this.Btn_StopScan.Size = new System.Drawing.Size(100, 35);
            this.Btn_StopScan.TabIndex = 16;
            this.Btn_StopScan.Text = "停止检测";
            this.Btn_StopScan.UseVisualStyleBackColor = true;
            this.Btn_StopScan.Click += new System.EventHandler(this.Btn_StopScan_Click);
            // 
            // Pic_AScan
            // 
            this.Pic_AScan.BackColor = System.Drawing.Color.Black;
            this.Pic_AScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_AScan.Location = new System.Drawing.Point(10, 100);
            this.Pic_AScan.Name = "Pic_AScan";
            this.Pic_AScan.Size = new System.Drawing.Size(422, 333);
            this.Pic_AScan.TabIndex = 20;
            this.Pic_AScan.TabStop = false;
            // 
            // Pic_BScan
            // 
            this.Pic_BScan.BackColor = System.Drawing.Color.Black;
            this.Pic_BScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_BScan.Location = new System.Drawing.Point(447, 100);
            this.Pic_BScan.Name = "Pic_BScan";
            this.Pic_BScan.Size = new System.Drawing.Size(439, 333);
            this.Pic_BScan.TabIndex = 21;
            this.Pic_BScan.TabStop = false;
            // 
            // Pic_CScan
            // 
            this.Pic_CScan.BackColor = System.Drawing.Color.Black;
            this.Pic_CScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_CScan.Location = new System.Drawing.Point(10, 439);
            this.Pic_CScan.Name = "Pic_CScan";
            this.Pic_CScan.Size = new System.Drawing.Size(422, 349);
            this.Pic_CScan.TabIndex = 22;
            this.Pic_CScan.TabStop = false;
            // 
            // Pic_DScan
            // 
            this.Pic_DScan.BackColor = System.Drawing.Color.Black;
            this.Pic_DScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_DScan.Location = new System.Drawing.Point(447, 439);
            this.Pic_DScan.Name = "Pic_DScan";
            this.Pic_DScan.Size = new System.Drawing.Size(439, 349);
            this.Pic_DScan.TabIndex = 23;
            this.Pic_DScan.TabStop = false;
            // 
            // Pic_InspectVideo
            // 
            this.Pic_InspectVideo.BackColor = System.Drawing.Color.Black;
            this.Pic_InspectVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_InspectVideo.Location = new System.Drawing.Point(900, 100);
            this.Pic_InspectVideo.Name = "Pic_InspectVideo";
            this.Pic_InspectVideo.Size = new System.Drawing.Size(389, 333);
            this.Pic_InspectVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pic_InspectVideo.TabIndex = 24;
            this.Pic_InspectVideo.TabStop = false;
            // 
            // Pic_OperatorVideo
            // 
            this.Pic_OperatorVideo.BackColor = System.Drawing.Color.Black;
            this.Pic_OperatorVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_OperatorVideo.Location = new System.Drawing.Point(900, 439);
            this.Pic_OperatorVideo.Name = "Pic_OperatorVideo";
            this.Pic_OperatorVideo.Size = new System.Drawing.Size(389, 349);
            this.Pic_OperatorVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pic_OperatorVideo.TabIndex = 25;
            this.Pic_OperatorVideo.TabStop = false;
            // 
            // Frm_Main_C_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 800);
            this.Controls.Add(this.Pic_OperatorVideo);
            this.Controls.Add(this.Pic_InspectVideo);
            this.Controls.Add(this.Pic_DScan);
            this.Controls.Add(this.Pic_CScan);
            this.Controls.Add(this.Pic_BScan);
            this.Controls.Add(this.Pic_AScan);
            this.Controls.Add(this.Btn_StopScan);
            this.Controls.Add(this.Btn_StartScan);
            this.Controls.Add(this.Btn_Mark);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_Backward);
            this.Controls.Add(this.Btn_Forward);
            this.Controls.Add(this.Btn_Link);
            this.Controls.Add(this.Lb_Speed);
            this.Controls.Add(this.Lb_Distance);
            this.Controls.Add(this.Lb_LinkState);
            this.Name = "Frm_Main_C_New";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "4轮车体控制系统";
            ((System.ComponentModel.ISupportInitialize)(this.Pic_AScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_BScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_CScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_DScan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_InspectVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_OperatorVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // 控件字段声明（VS 设计器标准位置：region 之后、类结尾之前）
        private System.Windows.Forms.Label Lb_LinkState;
        private System.Windows.Forms.Label Lb_Distance;
        private System.Windows.Forms.Label Lb_Speed;

        private System.Windows.Forms.Button Btn_Link;
        private System.Windows.Forms.Button Btn_Forward;
        private System.Windows.Forms.Button Btn_Backward;
        private System.Windows.Forms.Button Btn_Stop;
        private System.Windows.Forms.Button Btn_Mark;
        private System.Windows.Forms.Button Btn_StartScan;
        private System.Windows.Forms.Button Btn_StopScan;

        private System.Windows.Forms.PictureBox Pic_AScan;
        private System.Windows.Forms.PictureBox Pic_BScan;
        private System.Windows.Forms.PictureBox Pic_CScan;
        private System.Windows.Forms.PictureBox Pic_DScan;
        private System.Windows.Forms.PictureBox Pic_InspectVideo;
        private System.Windows.Forms.PictureBox Pic_OperatorVideo;

        // _statusTimer 在 Frm_Main_C_New.cs 的 InitializeControls() 中创建，
        // 此处仅声明，不在 InitializeComponent 中实例化（设计器不需要它）
        private System.Windows.Forms.Timer _statusTimer;
    }
}
