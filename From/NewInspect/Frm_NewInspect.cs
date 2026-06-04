using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: Frm_NewInspect.cs
    // 位置: Tofd_AWI/From/NewInspect/
    // 命名空间: Tofd_AWI.From.NewInspect
    // 职责: NewInspect 主窗体 — 深色主题三栏布局
    //       顶栏: 工作台标签(检测/设置/分析/运动/报告/数据) + 通信状态
    //       左栏: 参数面板(210px)
    //       中栏: 波形视图(弹性填充)
    //       右栏: 控制面板(190px)
    //       底栏: 运动状态栏(36px)
    // ============================================================

    public partial class Frm_NewInspect : Form
    {
        // 深色主题配色
        private static readonly Color BG_DARK      = Color.FromArgb(15, 18, 25);   // #0f1219
        private static readonly Color BG_PANEL     = Color.FromArgb(30, 41, 59);   // #1e293b
        private static readonly Color BG_TOOLBAR   = Color.FromArgb(22, 28, 38);   // #161c26
        private static readonly Color BG_INPUT     = Color.FromArgb(40, 52, 72);   // #283448
        private static readonly Color CLR_TEXT     = Color.FromArgb(226, 232, 240); // #e2e8f0
        private static readonly Color CLR_MUTED    = Color.FromArgb(148, 163, 184); // #94a3b8
        private static readonly Color CLR_BLUE     = Color.FromArgb(59, 130, 246);  // #3b82f6
        private static readonly Color CLR_GREEN    = Color.FromArgb(34, 197, 94);   // #22c55e
        private static readonly Color CLR_RED      = Color.FromArgb(239, 68, 68);   // #ef4444
        private static readonly Color CLR_AMBER    = Color.FromArgb(245, 158, 11);  // #f59e0b
        private static readonly Color CLR_BORDER   = Color.FromArgb(51, 65, 85);   // #334155

        private string _activeWorkspace = "inspect";

        public Frm_NewInspect()
        {
            InitializeComponent();
            ApplyDarkTheme(this);
            SwitchWorkspace("inspect");
        }

        // ==========================================
        // 工作台切换
        // ==========================================
        private void SwitchWorkspace(string name)
        {
            _activeWorkspace = name;

            // 更新标签高亮
            foreach (Control c in _tabBar.Controls)
            {
                if (c is Button btn && btn.Tag is string tag)
                {
                    btn.BackColor = (tag == name) ? CLR_BLUE : BG_TOOLBAR;
                    btn.ForeColor = (tag == name) ? Color.White : CLR_MUTED;
                }
            }

            // 根据工作台切换左侧面板内容
            _paramPanel.Controls.Clear();
            switch (name)
            {
                case "inspect":
                    _paramPanel.Controls.Add(new UcPA22SParams { Dock = DockStyle.Fill });
                    break;
                case "settings":
                    var lblSettings = new Label { Text = "配方管理\n(待实现)", Dock = DockStyle.Fill,
                        ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 12F), TextAlign = ContentAlignment.MiddleCenter };
                    _paramPanel.Controls.Add(lblSettings);
                    break;
                case "analysis":
                    var lblAnalysis = new Label { Text = "离线分析\n(待实现)", Dock = DockStyle.Fill,
                        ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 12F), TextAlign = ContentAlignment.MiddleCenter };
                    _paramPanel.Controls.Add(lblAnalysis);
                    break;
                case "motion":
                    _paramPanel.Controls.Add(new UcMotionParams { Dock = DockStyle.Fill });
                    break;
                case "report":
                    var lblReport = new Label { Text = "报告模板\n(待实现)", Dock = DockStyle.Fill,
                        ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 12F), TextAlign = ContentAlignment.MiddleCenter };
                    _paramPanel.Controls.Add(lblReport);
                    break;
                case "data":
                    var lblData = new Label { Text = "数据管理\n(待实现)", Dock = DockStyle.Fill,
                        ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 12F), TextAlign = ContentAlignment.MiddleCenter };
                    _paramPanel.Controls.Add(lblData);
                    break;
            }

            // 更新标题
            var titles = new System.Collections.Generic.Dictionary<string, string>
            {
                {"inspect","检测"},{"settings","设置"},{"analysis","分析"},
                {"motion","运动"},{"report","报告"},{"data","数据"}
            };
            _lblTitle.Text = titles.ContainsKey(name) ? titles[name] : "检测";
        }

        // ==========================================
        // 深色主题递归应用
        // ==========================================
        public static void ApplyDarkTheme(Control control)
        {
            if (control is Form frm)
            {
                frm.BackColor = BG_DARK;
                frm.ForeColor = CLR_TEXT;
            }
            else if (control is SplitContainer sc)
            {
                sc.BackColor = BG_DARK;
                sc.ForeColor = CLR_TEXT;
            }
            else if (control is Panel pnl)
            {
                pnl.BackColor = BG_PANEL;
                pnl.ForeColor = CLR_TEXT;
            }
            else if (control is TabControl tc)
            {
                tc.BackColor = BG_PANEL;
                tc.ForeColor = CLR_TEXT;
            }
            else if (control is TabPage tp)
            {
                tp.BackColor = BG_PANEL;
                tp.ForeColor = CLR_TEXT;
            }
            else if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = BG_INPUT;
                btn.ForeColor = CLR_TEXT;
                btn.FlatAppearance.BorderColor = CLR_BORDER;
                btn.FlatAppearance.MouseOverBackColor = CLR_BLUE;
            }
            else if (control is Label lbl)
            {
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = CLR_TEXT;
            }
            else if (control is TextBox tb)
            {
                tb.BackColor = BG_INPUT;
                tb.ForeColor = CLR_TEXT;
                tb.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox cb)
            {
                cb.BackColor = BG_INPUT;
                cb.ForeColor = CLR_TEXT;
                cb.FlatStyle = FlatStyle.Flat;
            }
            else if (control is NumericUpDown nud)
            {
                nud.BackColor = BG_INPUT;
                nud.ForeColor = CLR_TEXT;
            }
            else if (control is TrackBar tk)
            {
                tk.BackColor = BG_PANEL;
            }
            else if (control is GroupBox gb)
            {
                gb.BackColor = Color.Transparent;
                gb.ForeColor = CLR_BLUE;
            }
            else if (control is PictureBox pb)
            {
                pb.BackColor = Color.Black;
            }
            else if (control is ProgressBar prg)
            {
                prg.BackColor = BG_INPUT;
                prg.ForeColor = CLR_BLUE;
            }

            foreach (Control child in control.Controls)
            {
                ApplyDarkTheme(child);
            }
        }

        // ==========================================
        // 事件处理
        // ==========================================
        private void TabBtn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                SwitchWorkspace(tag);
            }
        }
    }
}
