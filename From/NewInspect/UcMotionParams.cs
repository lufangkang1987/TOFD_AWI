using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    /// <summary>
    /// 运动控制参数面板（运动工作台左侧面板）
    /// </summary>
    public partial class UcMotionParams : UserControl
    {
        public UcMotionParams()
        {
            InitializeComponent();
            ApplyDarkTheme(this);
        }

        private void ApplyDarkTheme(Control control)
        {
            Frm_NewInspect.ApplyDarkTheme(control);
        }
    }
}
