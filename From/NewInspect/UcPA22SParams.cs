using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: UcPA22SParams.cs
    // 职责: CTSPA22S 超声板卡参数配置面板
    //       6个标签页: 发射/接收、聚焦法则、闸门/报警、楔块/声速、扫查、校准
    //       所有参数基于 CTS-PA22S 真实硬件规格
    // ============================================================

    public partial class UcPA22SParams : UserControl
    {
        private static readonly Color BG_PANEL   = Color.FromArgb(30, 41, 59);
        private static readonly Color BG_INPUT   = Color.FromArgb(40, 52, 72);
        private static readonly Color CLR_TEXT   = Color.FromArgb(226, 232, 240);
        private static readonly Color CLR_MUTED  = Color.FromArgb(148, 163, 184);
        private static readonly Color CLR_BLUE   = Color.FromArgb(59, 130, 246);
        private static readonly Color CLR_BORDER = Color.FromArgb(51, 65, 85);
        private static readonly Font FONT_LABEL  = new Font("微软雅黑", 8F);
        private static readonly Font FONT_TAB    = new Font("微软雅黑", 8F);

        public UcPA22SParams()
        {
            InitializeComponent();
            SetupTabControlTheme();
            BuildParamTabs();
        }

        private void SetupTabControlTheme()
        {
            _tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            _tabControl.DrawItem += (sender, e) =>
            {
                var tab = _tabControl.TabPages[e.Index];
                bool isSelected = (e.Index == _tabControl.SelectedIndex);
                var rect = e.Bounds;

                // 填充标签背景
                using (var bg = new SolidBrush(isSelected ? CLR_BLUE : BG_PANEL))
                {
                    e.Graphics.FillRectangle(bg, rect);
                }

                // 底部边框线
                if (!isSelected)
                {
                    using (var pen = new Pen(CLR_BORDER, 1))
                    {
                        e.Graphics.DrawLine(pen, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                    }
                }

                // 文字
                using (var text = new SolidBrush(isSelected ? Color.White : CLR_MUTED))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(tab.Text, FONT_TAB, text, rect, sf);
                }
            };
            _tabControl.BackColor = BG_PANEL;
        }

        private void BuildParamTabs()
        {
            _tabControl.TabPages.Clear();

            _tabControl.TabPages.Add(MakeTab("发射/接收", MakeTxRxPanel()));
            _tabControl.TabPages.Add(MakeTab("聚焦法则", MakeFocalLawPanel()));
            _tabControl.TabPages.Add(MakeTab("闸门/报警", MakeGateAlarmPanel()));
            _tabControl.TabPages.Add(MakeTab("楔块/声速", MakeWedgeVelocityPanel()));
            _tabControl.TabPages.Add(MakeTab("扫查", MakeScanPanel()));
            _tabControl.TabPages.Add(MakeTab("校准", MakeCalibrationPanel()));
        }

        private TabPage MakeTab(string title, Control content)
        {
            var page = new TabPage(title)
            {
                BackColor = BG_PANEL,
                ForeColor = CLR_TEXT,
                Padding = new Padding(4)
            };
            content.Dock = DockStyle.Fill;
            content.BackColor = BG_PANEL;
            page.Controls.Add(content);
            return page;
        }

        // ==========================================
        // 发射/接收面板
        // ==========================================
        private Control MakeTxRxPanel()
        {
            var panel = new Panel { AutoScroll = true };

            int y = 4;
            AddGroup(panel, ref y, "发射参数");
            AddField(panel, ref y, "电压", MakeCombo("20V", "40V", "100V"));
            AddField(panel, ref y, "脉冲宽度", MakeNumeric(50, 500, 10, "ns"));
            AddField(panel, ref y, "PRF", MakeNumeric(10, 10000, 100, "Hz"));
            AddField(panel, ref y, "阻尼", MakeCombo("50Ω", "100Ω", "200Ω", "500Ω"));
            AddField(panel, ref y, "脉冲极性", MakeCombo("正极性", "负极性", "双极性"));

            AddGroup(panel, ref y, "接收参数");
            AddField(panel, ref y, "增益", MakeNumeric(0, 80, 1, "dB"));
            AddField(panel, ref y, "高通滤波", MakeCombo("Off", "1MHz", "2.5MHz", "5MHz"));
            AddField(panel, ref y, "低通滤波", MakeCombo("Off", "3MHz", "5MHz", "10MHz", "18MHz"));
            AddField(panel, ref y, "检波", MakeCombo("全波", "正向", "负向", "RF"));
            AddField(panel, ref y, "抑制", MakeNumeric(0, 100, 1, "%"));

            AddGroup(panel, ref y, "探头配置");
            AddField(panel, ref y, "频率", MakeNumeric(1, 18, 0.5m, "MHz"));
            AddField(panel, ref y, "晶片数", MakeNumeric(1, 64, 1, ""));
            AddField(panel, ref y, "间距", MakeNumeric(0.1m, 5, 0.1m, "mm"));

            return panel;
        }

        // ==========================================
        // 聚焦法则面板
        // ==========================================
        private Control MakeFocalLawPanel()
        {
            var panel = new Panel { AutoScroll = true };

            int y = 4;
            AddGroup(panel, ref y, "聚焦类型");
            AddField(panel, ref y, "类型", MakeCombo("扇扫", "线扫", "复合扫查"));

            AddGroup(panel, ref y, "扇扫参数");
            AddField(panel, ref y, "起始角度", MakeNumeric(-90, 90, 1, "°"));
            AddField(panel, ref y, "终止角度", MakeNumeric(-90, 90, 1, "°"));
            AddField(panel, ref y, "角度步进", MakeNumeric(0.5m, 10, 0.5m, "°"));
            AddField(panel, ref y, "聚焦深度", MakeNumeric(1, 500, 1, "mm"));

            AddGroup(panel, ref y, "孔径配置");
            AddField(panel, ref y, "起始阵元", MakeNumeric(1, 64, 1, ""));
            AddField(panel, ref y, "阵元数", MakeNumeric(1, 16, 1, ""));
            AddField(panel, ref y, "法则总数", new Label { Text = "41", ForeColor = CLR_MUTED, Font = FONT_LABEL, AutoSize = true });

            AddGroup(panel, ref y, "延迟法则");
            var btnImport = MakeButton("导入 .utlaw");
            var btnExport = MakeButton("导出 .utlaw");
            AddField(panel, ref y, "法则文件", btnImport);
            AddField(panel, ref y, "", btnExport);

            return panel;
        }

        // ==========================================
        // 闸门/报警面板
        // ==========================================
        private Control MakeGateAlarmPanel()
        {
            var panel = new Panel { AutoScroll = true };

            int y = 4;
            AddGroup(panel, ref y, "闸门 1");
            AddField(panel, ref y, "起始", MakeNumeric(0, 1000, 1, "mm"));
            AddField(panel, ref y, "宽度", MakeNumeric(1, 1000, 1, "mm"));
            AddField(panel, ref y, "阈值", MakeNumeric(0, 100, 1, "%"));

            AddGroup(panel, ref y, "闸门 2");
            AddField(panel, ref y, "起始", MakeNumeric(0, 1000, 1, "mm"));
            AddField(panel, ref y, "宽度", MakeNumeric(1, 1000, 1, "mm"));
            AddField(panel, ref y, "阈值", MakeNumeric(0, 100, 1, "%"));

            AddGroup(panel, ref y, "报警设置");
            AddField(panel, ref y, "G1 超阈值", MakeCombo("蜂鸣器", "指示灯", "蜂鸣+灯", "关闭"));
            AddField(panel, ref y, "G2 超阈值", MakeCombo("蜂鸣器", "指示灯", "蜂鸣+灯", "关闭"));
            AddField(panel, ref y, "丢失波", MakeCombo("蜂鸣器", "指示灯", "蜂鸣+灯", "关闭"));

            return panel;
        }

        // ==========================================
        // 楔块/声速面板
        // ==========================================
        private Control MakeWedgeVelocityPanel()
        {
            var panel = new Panel { AutoScroll = true };

            int y = 4;
            AddGroup(panel, ref y, "楔块参数");
            AddField(panel, ref y, "楔块型号", MakeCombo("I4型", "I5型", "S1型", "自定义"));
            AddField(panel, ref y, "楔块角度", MakeNumeric(0, 90, 0.5m, "°"));
            AddField(panel, ref y, "楔块声速", MakeNumeric(1000, 5000, 10, "m/s"));
            AddField(panel, ref y, "楔块高度", MakeNumeric(0, 50, 0.1m, "mm"));
            AddField(panel, ref y, "前沿距离", MakeNumeric(0, 100, 0.1m, "mm"));

            AddGroup(panel, ref y, "工件声速");
            AddField(panel, ref y, "纵波声速", MakeNumeric(1000, 10000, 10, "m/s"));
            AddField(panel, ref y, "横波声速", MakeNumeric(1000, 7000, 10, "m/s"));
            AddField(panel, ref y, "工件厚度", MakeNumeric(1, 500, 1, "mm"));

            return panel;
        }

        // ==========================================
        // 扫查面板
        // ==========================================
        private Control MakeScanPanel()
        {
            var panel = new Panel { AutoScroll = true };

            int y = 4;
            AddGroup(panel, ref y, "扫查模式");
            AddField(panel, ref y, "模式", MakeCombo("扇扫", "线扫", "复合扫查"));
            AddField(panel, ref y, "显示模式", MakeCombo("A+S", "A+L", "A+S+C"));

            AddGroup(panel, ref y, "编码器");
            AddField(panel, ref y, "编码器类型", MakeCombo("增量式", "绝对式"));
            AddField(panel, ref y, "分辨率", MakeNumeric(0.01m, 1, 0.01m, "mm/pulse"));
            AddField(panel, ref y, "方向", MakeCombo("正向", "反向"));

            AddGroup(panel, ref y, "PA22S APP");
            AddField(panel, ref y, "APP模式", MakeCombo("面阵", "螺栓", "常规PA"));

            return panel;
        }

        // ==========================================
        // 校准面板
        // ==========================================
        private Control MakeCalibrationPanel()
        {
            var panel = new Panel { AutoScroll = true };

            int y = 4;
            AddGroup(panel, ref y, "校准向导");

            var steps = new[] { "1. 声速校准", "2. 延迟校准", "3. 灵敏度校准", "4. TCG 校准" };
            foreach (var step in steps)
            {
                var btn = new Button
                {
                    Text = step,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BG_INPUT,
                    ForeColor = CLR_TEXT,
                    Font = FONT_TAB,
                    Width = 180,
                    Height = 28,
                    Margin = new Padding(4)
                };
                btn.FlatAppearance.BorderColor = CLR_BORDER;
                var lbl = new Label { Location = new Point(192, y + 4), Text = "未完成", ForeColor = CLR_MUTED, Font = FONT_LABEL, AutoSize = true };
                panel.Controls.Add(btn);
                panel.Controls.Add(lbl);
                y += 34;
            }

            AddGroup(panel, ref y, "试块选择");
            AddField(panel, ref y, "试块", MakeCombo("IIW试块", "CSK-IA", "CSK-IIA", "CSK-III", "自定义"));

            AddGroup(panel, ref y, "校准状态");
            var statusLabel = new Label
            {
                Text = "声速: --  延迟: --  灵敏度: --  TCG: --",
                ForeColor = CLR_MUTED,
                Font = FONT_LABEL,
                AutoSize = true,
                Location = new Point(8, y)
            };
            panel.Controls.Add(statusLabel);

            return panel;
        }

        // ==========================================
        // 辅助方法
        // ==========================================
        private void AddGroup(Panel parent, ref int y, string title)
        {
            var lbl = new Label
            {
                Text = title,
                ForeColor = CLR_BLUE,
                Font = new Font("微软雅黑", 8.5F, FontStyle.Bold),
                Location = new Point(4, y),
                AutoSize = true
            };
            var line = new Panel
            {
                BackColor = CLR_BORDER,
                Location = new Point(4, y + 18),
                Size = new Size(190, 1)
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(line);
            y += 24;
        }

        private void AddField(Panel parent, ref int y, string label, Control input)
        {
            if (!string.IsNullOrEmpty(label))
            {
                var lbl = new Label
                {
                    Text = label,
                    ForeColor = CLR_MUTED,
                    Font = FONT_LABEL,
                    Location = new Point(8, y + 3),
                    AutoSize = true
                };
                parent.Controls.Add(lbl);
            }

            input.Location = new Point(82, y);
            input.Size = new Size(120, 22);
            parent.Controls.Add(input);
            y += 28;
        }

        private ComboBox MakeCombo(params string[] items)
        {
            var cb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = BG_INPUT,
                ForeColor = CLR_TEXT,
                Font = FONT_LABEL,
                FlatStyle = FlatStyle.Flat,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 20
            };
            cb.Items.AddRange(items);
            if (items.Length > 0) cb.SelectedIndex = 0;

            // 自定义下拉列表绘制
            cb.DrawItem += (sender, e) =>
            {
                e.DrawBackground();
                if (e.Index < 0) return;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using (var bg = new SolidBrush(selected ? CLR_BLUE : BG_INPUT))
                {
                    e.Graphics.FillRectangle(bg, e.Bounds);
                }
                using (var text = new SolidBrush(CLR_TEXT))
                {
                    e.Graphics.DrawString(cb.Items[e.Index].ToString(), FONT_LABEL, text, e.Bounds.X + 2, e.Bounds.Y + 2);
                }
                e.DrawFocusRectangle();
            };

            return cb;
        }

        private NumericUpDown MakeNumeric(decimal min, decimal max, decimal step, string unit)
        {
            var nud = new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Increment = step,
                DecimalPlaces = (step < 1) ? 1 : 0,
                BackColor = BG_INPUT,
                ForeColor = CLR_TEXT,
                Font = FONT_LABEL
            };
            nud.Value = min;
            return nud;
        }

        private Button MakeButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = BG_INPUT,
                ForeColor = CLR_TEXT,
                Font = FONT_LABEL,
                Size = new Size(120, 22)
            };
            btn.FlatAppearance.BorderColor = CLR_BORDER;
            return btn;
        }
    }
}
