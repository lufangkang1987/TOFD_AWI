using System.Windows.Forms;

namespace Tofd_AWI.From
{
    partial class Frm_Move_New
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
            this.components = new System.ComponentModel.Container();
            this.Pan_Connect = new System.Windows.Forms.Panel();
            this.Lb_LinkState = new System.Windows.Forms.Label();
            this.Ck_Com_Can = new System.Windows.Forms.CheckBox();
            this.Cmb_Port = new System.Windows.Forms.ComboBox();
            this.Btn_Link = new System.Windows.Forms.Button();
            this.Btn_DisLink = new System.Windows.Forms.Button();
            this.Grp_Direction = new System.Windows.Forms.GroupBox();
            this.Btn_Forward = new System.Windows.Forms.Button();
            this.Btn_Backward = new System.Windows.Forms.Button();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.Btn_Left = new System.Windows.Forms.Button();
            this.Btn_Right = new System.Windows.Forms.Button();
            this.Grp_Speed = new System.Windows.Forms.GroupBox();
            this.Lb_SpeedVal = new System.Windows.Forms.Label();
            this.Track_Speed = new System.Windows.Forms.TrackBar();
            this.Btn_SpeedAdd = new System.Windows.Forms.Button();
            this.Btn_SpeedDec = new System.Windows.Forms.Button();
            this.Grp_Grating = new System.Windows.Forms.GroupBox();
            this.Btn_GratingUp = new System.Windows.Forms.Button();
            this.Btn_GratingDown = new System.Windows.Forms.Button();
            this.Lb_GratingState = new System.Windows.Forms.Label();
            this.Grp_MarkLight = new System.Windows.Forms.GroupBox();
            this.Btn_Mark = new System.Windows.Forms.Button();
            this.Ck_MarkEnabled = new System.Windows.Forms.CheckBox();
            this.Btn_LightFront = new System.Windows.Forms.Button();
            this.Btn_LightBack = new System.Windows.Forms.Button();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Lb_StatusDistance = new System.Windows.Forms.ToolStripStatusLabel();
            this.Lb_StatusSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.Lb_StatusDirection = new System.Windows.Forms.ToolStripStatusLabel();
            this.Timer_Refresh = new System.Windows.Forms.Timer(this.components);
            this.Pan_Connect.SuspendLayout();
            this.Grp_Direction.SuspendLayout();
            this.Grp_Speed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Track_Speed)).BeginInit();
            this.Grp_Grating.SuspendLayout();
            this.Grp_MarkLight.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pan_Connect
            // 
            this.Pan_Connect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pan_Connect.Controls.Add(this.Lb_LinkState);
            this.Pan_Connect.Controls.Add(this.Ck_Com_Can);
            this.Pan_Connect.Controls.Add(this.Cmb_Port);
            this.Pan_Connect.Controls.Add(this.Btn_Link);
            this.Pan_Connect.Controls.Add(this.Btn_DisLink);
            this.Pan_Connect.Location = new System.Drawing.Point(12, 12);
            this.Pan_Connect.Name = "Pan_Connect";
            this.Pan_Connect.Size = new System.Drawing.Size(556, 50);
            this.Pan_Connect.TabIndex = 4;
            // 
            // Lb_LinkState
            // 
            this.Lb_LinkState.AutoSize = true;
            this.Lb_LinkState.Location = new System.Drawing.Point(10, 16);
            this.Lb_LinkState.Name = "Lb_LinkState";
            this.Lb_LinkState.Size = new System.Drawing.Size(75, 17);
            this.Lb_LinkState.TabIndex = 0;
            this.Lb_LinkState.Text = "联机: 未连接";
            // 
            // Ck_Com_Can
            // 
            this.Ck_Com_Can.AutoSize = true;
            this.Ck_Com_Can.Location = new System.Drawing.Point(120, 15);
            this.Ck_Com_Can.Name = "Ck_Com_Can";
            this.Ck_Com_Can.Size = new System.Drawing.Size(81, 21);
            this.Ck_Com_Can.TabIndex = 1;
            this.Ck_Com_Can.Text = "COM模式";
            this.Ck_Com_Can.CheckedChanged += new System.EventHandler(this.Ck_Com_Can_CheckedChanged);
            // 
            // Cmb_Port
            // 
            this.Cmb_Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmb_Port.Location = new System.Drawing.Point(210, 13);
            this.Cmb_Port.Name = "Cmb_Port";
            this.Cmb_Port.Size = new System.Drawing.Size(100, 25);
            this.Cmb_Port.TabIndex = 2;
            // 
            // Btn_Link
            // 
            this.Btn_Link.Location = new System.Drawing.Point(320, 11);
            this.Btn_Link.Name = "Btn_Link";
            this.Btn_Link.Size = new System.Drawing.Size(80, 28);
            this.Btn_Link.TabIndex = 3;
            this.Btn_Link.Text = "连接";
            this.Btn_Link.Click += new System.EventHandler(this.Btn_Link_Click);
            // 
            // Btn_DisLink
            // 
            this.Btn_DisLink.Location = new System.Drawing.Point(410, 11);
            this.Btn_DisLink.Name = "Btn_DisLink";
            this.Btn_DisLink.Size = new System.Drawing.Size(80, 28);
            this.Btn_DisLink.TabIndex = 4;
            this.Btn_DisLink.Text = "断开";
            this.Btn_DisLink.Click += new System.EventHandler(this.Btn_DisLink_Click);
            // 
            // Grp_Direction
            // 
            this.Grp_Direction.Controls.Add(this.Btn_Forward);
            this.Grp_Direction.Controls.Add(this.Btn_Backward);
            this.Grp_Direction.Controls.Add(this.Btn_Stop);
            this.Grp_Direction.Controls.Add(this.Btn_Left);
            this.Grp_Direction.Controls.Add(this.Btn_Right);
            this.Grp_Direction.Location = new System.Drawing.Point(12, 75);
            this.Grp_Direction.Name = "Grp_Direction";
            this.Grp_Direction.Size = new System.Drawing.Size(556, 163);
            this.Grp_Direction.TabIndex = 3;
            this.Grp_Direction.TabStop = false;
            this.Grp_Direction.Text = "运动方向控制";
            // 
            // Btn_Forward
            // 
            this.Btn_Forward.Location = new System.Drawing.Point(231, 13);
            this.Btn_Forward.Name = "Btn_Forward";
            this.Btn_Forward.Size = new System.Drawing.Size(80, 50);
            this.Btn_Forward.TabIndex = 0;
            this.Btn_Forward.Text = "↑ 前进";
            this.Btn_Forward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Forward_MouseDown);
            this.Btn_Forward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Stop_MouseUp);
            // 
            // Btn_Backward
            // 
            this.Btn_Backward.Location = new System.Drawing.Point(230, 111);
            this.Btn_Backward.Name = "Btn_Backward";
            this.Btn_Backward.Size = new System.Drawing.Size(80, 50);
            this.Btn_Backward.TabIndex = 1;
            this.Btn_Backward.Text = "↓ 后退";
            this.Btn_Backward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Backward_MouseDown);
            this.Btn_Backward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Stop_MouseUp);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.BackColor = System.Drawing.Color.Red;
            this.Btn_Stop.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.Btn_Stop.ForeColor = System.Drawing.Color.White;
            this.Btn_Stop.Location = new System.Drawing.Point(231, 63);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(80, 50);
            this.Btn_Stop.TabIndex = 2;
            this.Btn_Stop.Text = "停止";
            this.Btn_Stop.UseVisualStyleBackColor = false;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // Btn_Left
            // 
            this.Btn_Left.Location = new System.Drawing.Point(135, 62);
            this.Btn_Left.Name = "Btn_Left";
            this.Btn_Left.Size = new System.Drawing.Size(80, 50);
            this.Btn_Left.TabIndex = 3;
            this.Btn_Left.Text = "← 左转";
            this.Btn_Left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Left_MouseDown);
            this.Btn_Left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Stop_MouseUp);
            // 
            // Btn_Right
            // 
            this.Btn_Right.Location = new System.Drawing.Point(329, 61);
            this.Btn_Right.Name = "Btn_Right";
            this.Btn_Right.Size = new System.Drawing.Size(80, 50);
            this.Btn_Right.TabIndex = 4;
            this.Btn_Right.Text = "右转 →";
            this.Btn_Right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_Right_MouseDown);
            this.Btn_Right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_Stop_MouseUp);
            // 
            // Grp_Speed
            // 
            this.Grp_Speed.Controls.Add(this.Lb_SpeedVal);
            this.Grp_Speed.Controls.Add(this.Track_Speed);
            this.Grp_Speed.Controls.Add(this.Btn_SpeedAdd);
            this.Grp_Speed.Controls.Add(this.Btn_SpeedDec);
            this.Grp_Speed.Location = new System.Drawing.Point(12, 244);
            this.Grp_Speed.Name = "Grp_Speed";
            this.Grp_Speed.Size = new System.Drawing.Size(270, 90);
            this.Grp_Speed.TabIndex = 2;
            this.Grp_Speed.TabStop = false;
            this.Grp_Speed.Text = "速度控制";
            // 
            // Lb_SpeedVal
            // 
            this.Lb_SpeedVal.AutoSize = true;
            this.Lb_SpeedVal.Location = new System.Drawing.Point(100, 55);
            this.Lb_SpeedVal.Name = "Lb_SpeedVal";
            this.Lb_SpeedVal.Size = new System.Drawing.Size(33, 17);
            this.Lb_SpeedVal.TabIndex = 0;
            this.Lb_SpeedVal.Text = "50%";
            // 
            // Track_Speed
            // 
            this.Track_Speed.Location = new System.Drawing.Point(10, 20);
            this.Track_Speed.Maximum = 100;
            this.Track_Speed.Minimum = 1;
            this.Track_Speed.Name = "Track_Speed";
            this.Track_Speed.Size = new System.Drawing.Size(180, 45);
            this.Track_Speed.TabIndex = 1;
            this.Track_Speed.Value = 50;
            this.Track_Speed.Scroll += new System.EventHandler(this.Track_Speed_Scroll);
            // 
            // Btn_SpeedAdd
            // 
            this.Btn_SpeedAdd.Location = new System.Drawing.Point(200, 15);
            this.Btn_SpeedAdd.Name = "Btn_SpeedAdd";
            this.Btn_SpeedAdd.Size = new System.Drawing.Size(55, 25);
            this.Btn_SpeedAdd.TabIndex = 2;
            this.Btn_SpeedAdd.Text = "+";
            this.Btn_SpeedAdd.Click += new System.EventHandler(this.Btn_SpeedAdd_Click);
            // 
            // Btn_SpeedDec
            // 
            this.Btn_SpeedDec.Location = new System.Drawing.Point(200, 45);
            this.Btn_SpeedDec.Name = "Btn_SpeedDec";
            this.Btn_SpeedDec.Size = new System.Drawing.Size(55, 25);
            this.Btn_SpeedDec.TabIndex = 3;
            this.Btn_SpeedDec.Text = "-";
            this.Btn_SpeedDec.Click += new System.EventHandler(this.Btn_SpeedDec_Click);
            // 
            // Grp_Grating
            // 
            this.Grp_Grating.Controls.Add(this.Btn_GratingUp);
            this.Grp_Grating.Controls.Add(this.Btn_GratingDown);
            this.Grp_Grating.Controls.Add(this.Lb_GratingState);
            this.Grp_Grating.Location = new System.Drawing.Point(298, 244);
            this.Grp_Grating.Name = "Grp_Grating";
            this.Grp_Grating.Size = new System.Drawing.Size(270, 90);
            this.Grp_Grating.TabIndex = 1;
            this.Grp_Grating.TabStop = false;
            this.Grp_Grating.Text = "光栅臂控制";
            // 
            // Btn_GratingUp
            // 
            this.Btn_GratingUp.Location = new System.Drawing.Point(15, 30);
            this.Btn_GratingUp.Name = "Btn_GratingUp";
            this.Btn_GratingUp.Size = new System.Drawing.Size(100, 35);
            this.Btn_GratingUp.TabIndex = 0;
            this.Btn_GratingUp.Text = "抬起";
            this.Btn_GratingUp.Click += new System.EventHandler(this.Btn_GratingUp_Click);
            // 
            // Btn_GratingDown
            // 
            this.Btn_GratingDown.Location = new System.Drawing.Point(125, 30);
            this.Btn_GratingDown.Name = "Btn_GratingDown";
            this.Btn_GratingDown.Size = new System.Drawing.Size(100, 35);
            this.Btn_GratingDown.TabIndex = 1;
            this.Btn_GratingDown.Text = "落下";
            this.Btn_GratingDown.Click += new System.EventHandler(this.Btn_GratingDown_Click);
            // 
            // Lb_GratingState
            // 
            this.Lb_GratingState.AutoSize = true;
            this.Lb_GratingState.Location = new System.Drawing.Point(15, 70);
            this.Lb_GratingState.Name = "Lb_GratingState";
            this.Lb_GratingState.Size = new System.Drawing.Size(63, 17);
            this.Lb_GratingState.TabIndex = 2;
            this.Lb_GratingState.Text = "状态: 落下";
            // 
            // Grp_MarkLight
            // 
            this.Grp_MarkLight.Controls.Add(this.Btn_Mark);
            this.Grp_MarkLight.Controls.Add(this.Ck_MarkEnabled);
            this.Grp_MarkLight.Controls.Add(this.Btn_LightFront);
            this.Grp_MarkLight.Controls.Add(this.Btn_LightBack);
            this.Grp_MarkLight.Location = new System.Drawing.Point(12, 340);
            this.Grp_MarkLight.Name = "Grp_MarkLight";
            this.Grp_MarkLight.Size = new System.Drawing.Size(556, 70);
            this.Grp_MarkLight.TabIndex = 0;
            this.Grp_MarkLight.TabStop = false;
            this.Grp_MarkLight.Text = "打标 & 灯光";
            // 
            // Btn_Mark
            // 
            this.Btn_Mark.Location = new System.Drawing.Point(15, 25);
            this.Btn_Mark.Name = "Btn_Mark";
            this.Btn_Mark.Size = new System.Drawing.Size(100, 30);
            this.Btn_Mark.TabIndex = 0;
            this.Btn_Mark.Text = "缺陷打标";
            this.Btn_Mark.Click += new System.EventHandler(this.Btn_Mark_Click);
            // 
            // Ck_MarkEnabled
            // 
            this.Ck_MarkEnabled.AutoSize = true;
            this.Ck_MarkEnabled.Location = new System.Drawing.Point(130, 30);
            this.Ck_MarkEnabled.Name = "Ck_MarkEnabled";
            this.Ck_MarkEnabled.Size = new System.Drawing.Size(75, 21);
            this.Ck_MarkEnabled.TabIndex = 1;
            this.Ck_MarkEnabled.Text = "启用打标";
            this.Ck_MarkEnabled.CheckedChanged += new System.EventHandler(this.Ck_MarkEnabled_CheckedChanged);
            // 
            // Btn_LightFront
            // 
            this.Btn_LightFront.Location = new System.Drawing.Point(290, 25);
            this.Btn_LightFront.Name = "Btn_LightFront";
            this.Btn_LightFront.Size = new System.Drawing.Size(100, 30);
            this.Btn_LightFront.TabIndex = 2;
            this.Btn_LightFront.Text = "前灯";
            this.Btn_LightFront.Click += new System.EventHandler(this.Btn_LightFront_Click);
            // 
            // Btn_LightBack
            // 
            this.Btn_LightBack.Location = new System.Drawing.Point(400, 25);
            this.Btn_LightBack.Name = "Btn_LightBack";
            this.Btn_LightBack.Size = new System.Drawing.Size(100, 30);
            this.Btn_LightBack.TabIndex = 3;
            this.Btn_LightBack.Text = "后灯";
            this.Btn_LightBack.Click += new System.EventHandler(this.Btn_LightBack_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Lb_StatusDistance,
            this.Lb_StatusSpeed,
            this.Lb_StatusDirection});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 419);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(584, 22);
            this.StatusStrip1.TabIndex = 5;
            // 
            // Lb_StatusDistance
            // 
            this.Lb_StatusDistance.Name = "Lb_StatusDistance";
            this.Lb_StatusDistance.Size = new System.Drawing.Size(82, 17);
            this.Lb_StatusDistance.Text = "距离: 0.0 mm";
            // 
            // Lb_StatusSpeed
            // 
            this.Lb_StatusSpeed.Name = "Lb_StatusSpeed";
            this.Lb_StatusSpeed.Size = new System.Drawing.Size(64, 17);
            this.Lb_StatusSpeed.Text = "速度: 50%";
            // 
            // Lb_StatusDirection
            // 
            this.Lb_StatusDirection.Name = "Lb_StatusDirection";
            this.Lb_StatusDirection.Size = new System.Drawing.Size(63, 17);
            this.Lb_StatusDirection.Text = "方向: 停止";
            // 
            // Timer_Refresh
            // 
            this.Timer_Refresh.Interval = 200;
            this.Timer_Refresh.Tick += new System.EventHandler(this.Timer_Refresh_Tick);
            // 
            // Frm_Move_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.Grp_MarkLight);
            this.Controls.Add(this.Grp_Grating);
            this.Controls.Add(this.Grp_Speed);
            this.Controls.Add(this.Grp_Direction);
            this.Controls.Add(this.Pan_Connect);
            this.Controls.Add(this.StatusStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Frm_Move_New";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "运动控制";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Move_New_FormClosing);
            this.Pan_Connect.ResumeLayout(false);
            this.Pan_Connect.PerformLayout();
            this.Grp_Direction.ResumeLayout(false);
            this.Grp_Speed.ResumeLayout(false);
            this.Grp_Speed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Track_Speed)).EndInit();
            this.Grp_Grating.ResumeLayout(false);
            this.Grp_Grating.PerformLayout();
            this.Grp_MarkLight.ResumeLayout(false);
            this.Grp_MarkLight.PerformLayout();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // 控件声明
        private Panel Pan_Connect;
        private Label Lb_LinkState;
        private CheckBox Ck_Com_Can;
        private ComboBox Cmb_Port;
        private Button Btn_Link;
        private Button Btn_DisLink;

        private GroupBox Grp_Direction;
        private Button Btn_Forward;
        private Button Btn_Backward;
        private Button Btn_Stop;
        private Button Btn_Left;
        private Button Btn_Right;

        private GroupBox Grp_Speed;
        private Label Lb_SpeedVal;
        private TrackBar Track_Speed;
        private Button Btn_SpeedAdd;
        private Button Btn_SpeedDec;

        private GroupBox Grp_Grating;
        private Button Btn_GratingUp;
        private Button Btn_GratingDown;
        private Label Lb_GratingState;

        private GroupBox Grp_MarkLight;
        private Button Btn_Mark;
        private CheckBox Ck_MarkEnabled;
        private Button Btn_LightFront;
        private Button Btn_LightBack;

        private StatusStrip StatusStrip1;
        private ToolStripStatusLabel Lb_StatusDistance;
        private ToolStripStatusLabel Lb_StatusSpeed;
        private ToolStripStatusLabel Lb_StatusDirection;

        private Timer Timer_Refresh;
    }
}
