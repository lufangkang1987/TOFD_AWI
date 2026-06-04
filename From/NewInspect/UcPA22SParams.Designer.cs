namespace Tofd_AWI.From.NewInspect
{
    partial class UcPA22SParams
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
            this._tabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();

            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._tabControl.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this._tabControl.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this._tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this._tabControl.ItemSize = new System.Drawing.Size(56, 24);
            this._tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this._tabControl.Alignment = System.Windows.Forms.TabAlignment.Top;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tabControl);
            this.Name = "UcPA22SParams";
            this.Size = new System.Drawing.Size(210, 500);
            this.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl _tabControl;
    }
}
