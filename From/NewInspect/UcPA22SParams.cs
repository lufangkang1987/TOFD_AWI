using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: UcPA22SParams.cs
    // 职责: CTSPA22S 超声板卡参数配置面板
    //       6个标签页: 发射/接收、聚焦法则、闸门/报警、楔块/声速、扫查、校准
    //       使用 Panel+Button 自制标签栏(替代 TabControl)
    // ============================================================

    public partial class UcPA22SParams : UserControl
    {
        // 截图色系 — 更暗的深蓝黑色调
        private static readonly Color BG_PANEL   = Color.FromArgb(16, 22, 36);   // #101624 面板
        private static readonly Color BG_CELL    = Color.FromArgb(22, 30, 48);   // #161e30 单元格
        private static readonly Color BG_INPUT   = Color.FromArgb(30, 40, 62);   // #1e283e 输入框
        private static readonly Color CLR_TEXT   = Color.FromArgb(200, 210, 225); // #c8d2e1 主文字
        private static readonly Color CLR_MUTED  = Color.FromArgb(100, 115, 140); // #64738c 次要
        private static readonly Color CLR_BLUE   = Color.FromArgb(56, 130, 246);  // #3882f6 蓝
        private static readonly Color CLR_GREEN  = Color.FromArgb(34, 197, 94);   // #22c55e 绿
        private static readonly Color CLR_BORDER = Color.FromArgb(30, 40, 60);    // #1e283c 边框
        private static readonly Font FONT_LABEL  = new Font("微软雅黑", 8F);
        private static readonly Font FONT_TAB    = new Font("微软雅黑", 8F, FontStyle.Bold);

        private Panel _tabBar;
        private Panel _contentPanel;
        private Button[] _tabButtons;
        private Panel[] _tabContents;
        private int _activeTabIndex = 5; // 默认显示"校准"标签(跟截图一致)

        public UcPA22SParams()
        {
            InitializeComponent();
            BuildCustomTabs();
        }

        private void BuildCustomTabs()
        {
            this.Controls.Clear();
            this.BackColor = BG_PANEL;

            // ========== 标签栏 ==========
            _tabBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = BG_PANEL
            };

            var tabNames = new[] { "发射/接收", "聚焦法则", "闸门/报警", "楔块/声速", "扫查", "校准" };
            _tabButtons = new Button[tabNames.Length];

            int btnWidth = 55; // 紧凑排列
            for (int i = 0; i < tabNames.Length; i++)
            {
                var btn = new Button
                {
                    Text = tabNames[i],
                    FlatStyle = FlatStyle.Flat,
                    Font = FONT_TAB,
                    Height = 24,
                    Width = btnWidth,
                    Left = 4 + i * (btnWidth + 2),
                    Top = 3,
                    Tag = i
                };
                btn.FlatAppearance.BorderSize = 0;
                SetTabStyle(btn, i == _activeTabIndex);
                btn.Click += TabButton_Click;
                _tabBar.Controls.Add(btn);
                _tabButtons[i] = btn;
            }

            // ========== 内容区 ==========
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BG_PANEL,
                Padding = new Padding(4)
            };

            _tabContents = new Panel[]
            {
                MakeTxRxPanel(),
                MakeFocalLawPanel(),
                MakeGateAlarmPanel(),
                MakeWedgeVelocityPanel(),
                MakeScanPanel(),
                MakeCalibrationPanel()
            };

            foreach (var p in _tabContents)
            {
                p.Dock = DockStyle.Fill;
                p.Visible = false;
                _contentPanel.Controls.Add(p);
            }
            _tabContents[_activeTabIndex].Visible = true;

            this.Controls.Add(_contentPanel);
            this.Controls.Add(_tabBar);
        }

        private void SetTabStyle(Button btn, bool active)
        {
            if (active)
            {
                btn.BackColor = CLR_BLUE;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderSize = 0;
                // 底部画一条高亮边
            }
            else
            {
                btn.BackColor = BG_PANEL;
                btn.ForeColor = CLR_MUTED;
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        private void TabButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int idx)
            {
                SwitchTab(idx);
            }
        }

        private void SwitchTab(int index)
        {
            if (index < 0 || index >= _tabContents.Length) return;
            if (index == _activeTabIndex) return;

            _tabContents[_activeTabIndex].Visible = false;
            SetTabStyle(_tabButtons[_activeTabIndex], false);

            _activeTabIndex = index;
            _tabContents[_activeTabIndex].Visible = true;
            SetTabStyle(_tabButtons[_activeTabIndex], true);
        }

        // ==========================================
        // 发射/接收面板
        // ==========================================
        private Panel MakeTxRxPanel()
        {
            var panel = new Panel { AutoScroll = true, BackColor = BG_PANEL };
            int y = 6;
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
        private Panel MakeFocalLawPanel()
        {
            var panel = new Panel { AutoScroll = true, BackColor = BG_PANEL };
            int y = 6;
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
        private Panel MakeGateAlarmPanel()
        {
            var panel = new Panel { AutoScroll = true, BackColor = BG_PANEL };
            int y = 6;
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
        private Panel MakeWedgeVelocityPanel()
        {
            var panel = new Panel { AutoScroll = true, BackColor = BG_PANEL };
            int y = 6;
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
        private Panel MakeScanPanel()
        {
            var panel = new Panel { AutoScroll = true, BackColor = BG_PANEL };
            int y = 6;
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
        // 校准面板 — 截图风格：卡片式按钮 + 分组
        // ==========================================
        private Panel MakeCalibrationPanel()
        {
            var panel = new Panel { AutoScroll = true, BackColor = BG_PANEL };
            int y = 8;

            // 校准向导 — 卡片式大按钮
            AddGroup(panel, ref y, "校准向导");

            var steps = new[] { "1. 声速校准", "2. 延迟校准", "3. 灵敏度校准", "4. TCG 校准" };
            for (int i = 0; i < steps.Length; i++)
            {
                var stepPanel = new Panel
                {
                    Location = new Point(4, y),
                    Size = new Size(192, 30),
                    BackColor = BG_CELL,
                    Padding = new Padding(1)
                };

                var btn = new Button
                {
                    Text = steps[i],
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BG_CELL,
                    ForeColor = CLR_TEXT,
                    Font = FONT_LABEL,
                    Dock = DockStyle.Left,
                    Width = 110
                };
                btn.FlatAppearance.BorderSize = 0;

                var lblStatus = new Label
                {
                    Text = "未完成",
                    ForeColor = CLR_MUTED,
                    Font = FONT_LABEL,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(0, 0, 6, 0)
                };

                stepPanel.Controls.Add(lblStatus);
                stepPanel.Controls.Add(btn);
                panel.Controls.Add(stepPanel);
                y += 34;
            }

            // 校准状态
            AddGroup(panel, ref y, "校准状态");
            var statusPanel = new Panel
            {
                Location = new Point(4, y),
                Size = new Size(192, 52),
                BackColor = BG_CELL
            };
            var statusLabel = new Label
            {
                Text = "声速: --\n延迟: --\n灵敏度: --\nTCG: --",
                ForeColor = CLR_MUTED,
                Font = FONT_LABEL,
                Dock = DockStyle.Fill,
                Padding = new Padding(6, 4, 0, 0)
            };
            statusPanel.Controls.Add(statusLabel);
            panel.Controls.Add(statusPanel);
            y += 60;

            // 试块选择
            AddGroup(panel, ref y, "试块");
            var cbBlock = MakeCombo("IIW试块", "CSK-IA", "CSK-IIA", "CSK-III", "自定义");
            cbBlock.Location = new Point(4, y);
            cbBlock.Size = new Size(192, 22);
            panel.Controls.Add(cbBlock);

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
                Location = new Point(4, y + 16),
                Size = new Size(192, 1)
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(line);
            y += 22;
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
                    Location = new Point(6, y + 3),
                    AutoSize = true
                };
                parent.Controls.Add(lbl);
            }

            input.Location = new Point(80, y);
            input.Size = new Size(118, 22);
            if (input is NumericUpDown nud) nud.Width = 80;
            parent.Controls.Add(input);
            y += 26;
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
                Font = FONT_LABEL,
                BorderStyle = BorderStyle.FixedSingle
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
                BackColor = BG_CELL,
                ForeColor = CLR_TEXT,
                Font = FONT_LABEL,
                Size = new Size(118, 22)
            };
            btn.FlatAppearance.BorderColor = CLR_BORDER;
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }
    }
}
