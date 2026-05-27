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
            // 状态显示区控件
            this.Lb_PowerState = new System.Windows.Forms.Label();
            this.Lb_CameraState = new System.Windows.Forms.Label();
            this.Lb_MotionState = new System.Windows.Forms.Label();
            this.Lb_TofdState = new System.Windows.Forms.Label();
            this.Lb_Distance = new System.Windows.Forms.Label();
            this.Lb_Speed = new System.Windows.Forms.Label();
            
            // 显示区控件
            this.Pic_Video = new System.Windows.Forms.PictureBox();
            this.Pic_Waveform = new System.Windows.Forms.PictureBox();
            
            // 按钮区控件
            this.Btn_Settings = new System.Windows.Forms.Button();
            this.Btn_MotionControl = new System.Windows.Forms.Button();
            this.Btn_EmergencyStop = new System.Windows.Forms.Button();
            this.Btn_StartScan = new System.Windows.Forms.Button();
            this.Btn_StopScan = new System.Windows.Forms.Button();
            this.Btn_DataQuery = new System.Windows.Forms.Button();
            this.Btn_Close = new System.Windows.Forms.Button();

            // 初始化控件
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Video)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Waveform)).BeginInit();
            this.SuspendLayout();
            
            // ==========================================
            // 状态显示区 (顶部，高度 40px)
            // ==========================================
            
            // Lb_PowerState - 电源状态
            this.Lb_PowerState.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Lb_PowerState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lb_PowerState.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.Lb_PowerState.ForeColor = System.Drawing.Color.Red;
            this.Lb_PowerState.Location = new System.Drawing.Point(10, 10);
            this.Lb_PowerState.Name = "Lb_PowerState";
            this.Lb_PowerState.Size = new System.Drawing.Size(120, 30);
            this.Lb_PowerState.TabIndex = 0;
            this.Lb_PowerState.Text = "● 电源";
            this.Lb_PowerState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // Lb_CameraState - 相机状态
            this.Lb_CameraState.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Lb_CameraState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lb_CameraState.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.Lb_CameraState.ForeColor = System.Drawing.Color.Red;
            this.Lb_CameraState.Location = new System.Drawing.Point(140, 10);
            this.Lb_CameraState.Name = "Lb_CameraState";
            this.Lb_CameraState.Size = new System.Drawing.Size(120, 30);
            this.Lb_CameraState.TabIndex = 1;
            this.Lb_CameraState.Text = "● 相机";
            this.Lb_CameraState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // Lb_MotionState - 小车状态
            this.Lb_MotionState.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Lb_MotionState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lb_MotionState.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.Lb_MotionState.ForeColor = System.Drawing.Color.Red;
            this.Lb_MotionState.Location = new System.Drawing.Point(270, 10);
            this.Lb_MotionState.Name = "Lb_MotionState";
            this.Lb_MotionState.Size = new System.Drawing.Size(120, 30);
            this.Lb_MotionState.TabIndex = 2;
            this.Lb_MotionState.Text = "● 小车";
            this.Lb_MotionState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // Lb_TofdState - TOFD 状态
            this.Lb_TofdState.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Lb_TofdState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lb_TofdState.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.Lb_TofdState.ForeColor = System.Drawing.Color.Red;
            this.Lb_TofdState.Location = new System.Drawing.Point(400, 10);
            this.Lb_TofdState.Name = "Lb_TofdState";
            this.Lb_TofdState.Size = new System.Drawing.Size(120, 30);
            this.Lb_TofdState.TabIndex = 3;
            this.Lb_TofdState.Text = "● TOFD";
            this.Lb_TofdState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // Lb_Distance - 距离显示
            this.Lb_Distance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Lb_Distance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lb_Distance.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Lb_Distance.Location = new System.Drawing.Point(530, 10);
            this.Lb_Distance.Name = "Lb_Distance";
            this.Lb_Distance.Size = new System.Drawing.Size(150, 30);
            this.Lb_Distance.TabIndex = 4;
            this.Lb_Distance.Text = "距离：0.0 mm";
            this.Lb_Distance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // Lb_Speed - 速度显示
            this.Lb_Speed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Lb_Speed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lb_Speed.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Lb_Speed.Location = new System.Drawing.Point(690, 10);
            this.Lb_Speed.Name = "Lb_Speed";
            this.Lb_Speed.Size = new System.Drawing.Size(120, 30);
            this.Lb_Speed.TabIndex = 5;
            this.Lb_Speed.Text = "速度：0%";
            this.Lb_Speed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // ==========================================
            // 显示区 (中部，视频和波形)
            // ==========================================
            
            // Pic_Video - 相机视频显示
            this.Pic_Video.BackColor = System.Drawing.Color.Black;
            this.Pic_Video.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_Video.Location = new System.Drawing.Point(10, 50);
            this.Pic_Video.Name = "Pic_Video";
            this.Pic_Video.Size = new System.Drawing.Size(600, 400);
            this.Pic_Video.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_Video.TabIndex = 10;
            this.Pic_Video.TabStop = false;
            
            // Pic_Waveform - 超声波形显示
            this.Pic_Waveform.BackColor = System.Drawing.Color.Black;
            this.Pic_Waveform.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pic_Waveform.Location = new System.Drawing.Point(620, 50);
            this.Pic_Waveform.Name = "Pic_Waveform";
            this.Pic_Waveform.Size = new System.Drawing.Size(470, 400);
            this.Pic_Waveform.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pic_Waveform.TabIndex = 11;
            this.Pic_Waveform.TabStop = false;
            
            // ==========================================
            // 按钮区 (底部，高度 50px)
            // ==========================================
            
            // Btn_Settings - 参数设置
            this.Btn_Settings.BackColor = System.Drawing.Color.FromArgb(64, 128, 255);
            this.Btn_Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Settings.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_Settings.ForeColor = System.Drawing.Color.White;
            this.Btn_Settings.Location = new System.Drawing.Point(10, 460);
            this.Btn_Settings.Name = "Btn_Settings";
            this.Btn_Settings.Size = new System.Drawing.Size(120, 45);
            this.Btn_Settings.TabIndex = 20;
            this.Btn_Settings.Text = "参数设置";
            this.Btn_Settings.UseVisualStyleBackColor = false;
            this.Btn_Settings.Click += new System.EventHandler(this.Btn_Settings_Click);
            
            // Btn_MotionControl - 运动控制
            this.Btn_MotionControl.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
            this.Btn_MotionControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_MotionControl.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_MotionControl.ForeColor = System.Drawing.Color.White;
            this.Btn_MotionControl.Location = new System.Drawing.Point(140, 460);
            this.Btn_MotionControl.Name = "Btn_MotionControl";
            this.Btn_MotionControl.Size = new System.Drawing.Size(120, 45);
            this.Btn_MotionControl.TabIndex = 21;
            this.Btn_MotionControl.Text = "运动控制";
            this.Btn_MotionControl.UseVisualStyleBackColor = false;
            this.Btn_MotionControl.Click += new System.EventHandler(this.Btn_MotionControl_Click);
            
            // Btn_EmergencyStop - 紧急停止 (红色，突出显示)
            this.Btn_EmergencyStop.BackColor = System.Drawing.Color.Red;
            this.Btn_EmergencyStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_EmergencyStop.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_EmergencyStop.ForeColor = System.Drawing.Color.White;
            this.Btn_EmergencyStop.Location = new System.Drawing.Point(270, 460);
            this.Btn_EmergencyStop.Name = "Btn_EmergencyStop";
            this.Btn_EmergencyStop.Size = new System.Drawing.Size(120, 45);
            this.Btn_EmergencyStop.TabIndex = 22;
            this.Btn_EmergencyStop.Text = "紧急停止";
            this.Btn_EmergencyStop.UseVisualStyleBackColor = false;
            this.Btn_EmergencyStop.Click += new System.EventHandler(this.Btn_EmergencyStop_Click);
            
            // Btn_StartScan - 开始检测
            this.Btn_StartScan.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
            this.Btn_StartScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_StartScan.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_StartScan.ForeColor = System.Drawing.Color.White;
            this.Btn_StartScan.Location = new System.Drawing.Point(400, 460);
            this.Btn_StartScan.Name = "Btn_StartScan";
            this.Btn_StartScan.Size = new System.Drawing.Size(120, 45);
            this.Btn_StartScan.TabIndex = 23;
            this.Btn_StartScan.Text = "开始检测";
            this.Btn_StartScan.UseVisualStyleBackColor = false;
            this.Btn_StartScan.Click += new System.EventHandler(this.Btn_StartScan_Click);
            
            // Btn_StopScan - 停止检测
            this.Btn_StopScan.BackColor = System.Drawing.Color.Orange;
            this.Btn_StopScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_StopScan.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_StopScan.ForeColor = System.Drawing.Color.White;
            this.Btn_StopScan.Location = new System.Drawing.Point(530, 460);
            this.Btn_StopScan.Name = "Btn_StopScan";
            this.Btn_StopScan.Size = new System.Drawing.Size(120, 45);
            this.Btn_StopScan.TabIndex = 24;
            this.Btn_StopScan.Text = "停止检测";
            this.Btn_StopScan.UseVisualStyleBackColor = false;
            this.Btn_StopScan.Click += new System.EventHandler(this.Btn_StopScan_Click);
            
            // Btn_DataQuery - 数据查询
            this.Btn_DataQuery.BackColor = System.Drawing.Color.FromArgb(64, 128, 255);
            this.Btn_DataQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_DataQuery.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_DataQuery.ForeColor = System.Drawing.Color.White;
            this.Btn_DataQuery.Location = new System.Drawing.Point(660, 460);
            this.Btn_DataQuery.Name = "Btn_DataQuery";
            this.Btn_DataQuery.Size = new System.Drawing.Size(120, 45);
            this.Btn_DataQuery.TabIndex = 25;
            this.Btn_DataQuery.Text = "数据查询";
            this.Btn_DataQuery.UseVisualStyleBackColor = false;
            this.Btn_DataQuery.Click += new System.EventHandler(this.Btn_DataQuery_Click);
            
            // Btn_Close - 关闭
            this.Btn_Close.BackColor = System.Drawing.Color.Gray;
            this.Btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Close.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.Btn_Close.ForeColor = System.Drawing.Color.White;
            this.Btn_Close.Location = new System.Drawing.Point(790, 460);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(120, 45);
            this.Btn_Close.TabIndex = 26;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = false;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            
            // ==========================================
            // 窗体属性
            // ==========================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 520);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_DataQuery);
            this.Controls.Add(this.Btn_StopScan);
            this.Controls.Add(this.Btn_StartScan);
            this.Controls.Add(this.Btn_EmergencyStop);
            this.Controls.Add(this.Btn_MotionControl);
            this.Controls.Add(this.Btn_Settings);
            this.Controls.Add(this.Pic_Waveform);
            this.Controls.Add(this.Pic_Video);
            this.Controls.Add(this.Lb_Speed);
            this.Controls.Add(this.Lb_Distance);
            this.Controls.Add(this.Lb_TofdState);
            this.Controls.Add(this.Lb_MotionState);
            this.Controls.Add(this.Lb_CameraState);
            this.Controls.Add(this.Lb_PowerState);
            this.Name = "Frm_Main_C_New";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "4 轮车体控制系统";
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Video)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Waveform)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        // ==========================================
        // 控件字段声明
        // ==========================================
        
        // 状态显示区
        private System.Windows.Forms.Label Lb_PowerState;
        private System.Windows.Forms.Label Lb_CameraState;
        private System.Windows.Forms.Label Lb_MotionState;
        private System.Windows.Forms.Label Lb_TofdState;
        private System.Windows.Forms.Label Lb_Distance;
        private System.Windows.Forms.Label Lb_Speed;
        
        // 显示区
        private System.Windows.Forms.PictureBox Pic_Video;
        private System.Windows.Forms.PictureBox Pic_Waveform;
        
        // 按钮区
        private System.Windows.Forms.Button Btn_Settings;
        private System.Windows.Forms.Button Btn_MotionControl;
        private System.Windows.Forms.Button Btn_EmergencyStop;
        private System.Windows.Forms.Button Btn_StartScan;
        private System.Windows.Forms.Button Btn_StopScan;
        private System.Windows.Forms.Button Btn_DataQuery;
        private System.Windows.Forms.Button Btn_Close;
        
        // 定时器
        private System.Windows.Forms.Timer _statusTimer;
    }
}