namespace Tofd_AWI.From.NewInspect
{
    partial class UcMotionParams
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._tabMotion = new System.Windows.Forms.TabControl();
            this._tabPageAxis = new System.Windows.Forms.TabPage();
            this._tabPageJog = new System.Windows.Forms.TabPage();
            this._tabPageTeach = new System.Windows.Forms.TabPage();
            this.SuspendLayout();
            // 
            // _tabMotion
            // 
            this._tabMotion.Alignment = System.Windows.Forms.TabAlignment.Top;
            this._tabMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabMotion.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this._tabMotion.ItemSize = new System.Drawing.Size(48, 20);
            this._tabMotion.Location = new System.Drawing.Point(0, 0);
            this._tabMotion.Name = "_tabMotion";
            this._tabMotion.SelectedIndex = 0;
            this._tabMotion.Size = new System.Drawing.Size(210, 600);
            this._tabMotion.TabIndex = 0;
            // 
            // _tabPageAxis
            // 
            this._tabPageAxis.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._tabPageAxis.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._tabPageAxis.Text = "轴参数";
            // 
            // _tabPageJog
            // 
            this._tabPageJog.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._tabPageJog.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._tabPageJog.Text = "JOG";
            // 
            // _tabPageTeach
            // 
            this._tabPageTeach.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._tabPageTeach.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._tabPageTeach.Text = "示教";
            // 
            // UcMotionParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tabMotion);
            this.Name = "UcMotionParams";
            this._tabMotion.Controls.Add(this._tabPageAxis);
            this._tabMotion.Controls.Add(this._tabPageJog);
            this._tabMotion.Controls.Add(this._tabPageTeach);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl _tabMotion;
        private System.Windows.Forms.TabPage _tabPageAxis;
        private System.Windows.Forms.TabPage _tabPageJog;
        private System.Windows.Forms.TabPage _tabPageTeach;
    }
}
