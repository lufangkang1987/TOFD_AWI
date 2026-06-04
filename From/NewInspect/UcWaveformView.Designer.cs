namespace Tofd_AWI.From.NewInspect
{
    partial class UcWaveformView
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
            this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();

            this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayout.BackColor = System.Drawing.Color.FromArgb(15, 18, 25);
            this._tableLayout.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this._tableLayout.Location = new System.Drawing.Point(0, 0);
            this._tableLayout.Name = "_tableLayout";
            this._tableLayout.Size = new System.Drawing.Size(800, 600);
            this._tableLayout.TabIndex = 0;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayout);
            this.Name = "UcWaveformView";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel _tableLayout;
    }
}
