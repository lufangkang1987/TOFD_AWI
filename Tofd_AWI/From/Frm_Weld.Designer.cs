
namespace Tofd_AWI.From
{
    partial class Frm_Weld
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Weld));
            this.Tab_Contrl = new System.Windows.Forms.TabControl();
            this.Input = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Bt_New = new System.Windows.Forms.Button();
            this.Cmb_Bh = new System.Windows.Forms.ComboBox();
            this.Txt_ReMark = new System.Windows.Forms.TextBox();
            this.Lb_Bz = new System.Windows.Forms.Label();
            this.Txt_ProbeSpacing = new System.Windows.Forms.TextBox();
            this.Lb_Ttjj = new System.Windows.Forms.Label();
            this.Txt_Thick = new System.Windows.Forms.TextBox();
            this.Lb_Hd = new System.Windows.Forms.Label();
            this.Cmb_DetectionSite = new System.Windows.Forms.ComboBox();
            this.Lb_NbWb = new System.Windows.Forms.Label();
            this.Lb_Bh = new System.Windows.Forms.Label();
            this.Lb_Weld_ID = new System.Windows.Forms.Label();
            this.Lb_ID = new System.Windows.Forms.Label();
            this.Bt_CanCel = new System.Windows.Forms.Button();
            this.Bt_Ok = new System.Windows.Forms.Button();
            this.History = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Dg_Alarm = new System.Windows.Forms.DataGridView();
            this.Dg_Part = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Lb_SubID_Val = new System.Windows.Forms.Label();
            this.Bt_Delete = new System.Windows.Forms.Button();
            this.Lb_Item = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Bt_Original = new System.Windows.Forms.Button();
            this.Lb_Alarm = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Tab_Contrl.SuspendLayout();
            this.Input.SuspendLayout();
            this.panel1.SuspendLayout();
            this.History.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dg_Alarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dg_Part)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tab_Contrl
            // 
            this.Tab_Contrl.Controls.Add(this.Input);
            this.Tab_Contrl.Controls.Add(this.History);
            this.Tab_Contrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Contrl.Location = new System.Drawing.Point(0, 0);
            this.Tab_Contrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Tab_Contrl.Name = "Tab_Contrl";
            this.Tab_Contrl.SelectedIndex = 0;
            this.Tab_Contrl.Size = new System.Drawing.Size(798, 539);
            this.Tab_Contrl.TabIndex = 0;
            // 
            // Input
            // 
            this.Input.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Input.Controls.Add(this.panel1);
            this.Input.Controls.Add(this.Bt_CanCel);
            this.Input.Controls.Add(this.Bt_Ok);
            this.Input.Location = new System.Drawing.Point(4, 30);
            this.Input.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Input.Name = "Input";
            this.Input.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Input.Size = new System.Drawing.Size(790, 505);
            this.Input.TabIndex = 0;
            this.Input.Text = "     新焊缝      ";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.Bt_New);
            this.panel1.Controls.Add(this.Cmb_Bh);
            this.panel1.Controls.Add(this.Txt_ReMark);
            this.panel1.Controls.Add(this.Lb_Bz);
            this.panel1.Controls.Add(this.Txt_ProbeSpacing);
            this.panel1.Controls.Add(this.Lb_Ttjj);
            this.panel1.Controls.Add(this.Txt_Thick);
            this.panel1.Controls.Add(this.Lb_Hd);
            this.panel1.Controls.Add(this.Cmb_DetectionSite);
            this.panel1.Controls.Add(this.Lb_NbWb);
            this.panel1.Controls.Add(this.Lb_Bh);
            this.panel1.Controls.Add(this.Lb_Weld_ID);
            this.panel1.Controls.Add(this.Lb_ID);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 458);
            this.panel1.TabIndex = 2;
            // 
            // Bt_New
            // 
            this.Bt_New.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_New.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Bt_New.Location = new System.Drawing.Point(261, 28);
            this.Bt_New.Name = "Bt_New";
            this.Bt_New.Size = new System.Drawing.Size(58, 31);
            this.Bt_New.TabIndex = 18;
            this.Bt_New.Text = "新建";
            this.Bt_New.UseVisualStyleBackColor = true;
            this.Bt_New.Click += new System.EventHandler(this.Bt_New_Click);
            // 
            // Cmb_Bh
            // 
            this.Cmb_Bh.FormattingEnabled = true;
            this.Cmb_Bh.Items.AddRange(new object[] {
            "外壁",
            "内壁"});
            this.Cmb_Bh.Location = new System.Drawing.Point(460, 30);
            this.Cmb_Bh.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Cmb_Bh.Name = "Cmb_Bh";
            this.Cmb_Bh.Size = new System.Drawing.Size(249, 28);
            this.Cmb_Bh.TabIndex = 12;
            // 
            // Txt_ReMark
            // 
            this.Txt_ReMark.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Txt_ReMark.Location = new System.Drawing.Point(152, 210);
            this.Txt_ReMark.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Txt_ReMark.Multiline = true;
            this.Txt_ReMark.Name = "Txt_ReMark";
            this.Txt_ReMark.Size = new System.Drawing.Size(634, 239);
            this.Txt_ReMark.TabIndex = 11;
            // 
            // Lb_Bz
            // 
            this.Lb_Bz.AutoSize = true;
            this.Lb_Bz.Location = new System.Drawing.Point(22, 219);
            this.Lb_Bz.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Bz.Name = "Lb_Bz";
            this.Lb_Bz.Size = new System.Drawing.Size(99, 20);
            this.Lb_Bz.TabIndex = 10;
            this.Lb_Bz.Text = "检测备注:";
            // 
            // Txt_ProbeSpacing
            // 
            this.Txt_ProbeSpacing.Location = new System.Drawing.Point(152, 143);
            this.Txt_ProbeSpacing.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Txt_ProbeSpacing.Name = "Txt_ProbeSpacing";
            this.Txt_ProbeSpacing.Size = new System.Drawing.Size(96, 30);
            this.Txt_ProbeSpacing.TabIndex = 9;
            // 
            // Lb_Ttjj
            // 
            this.Lb_Ttjj.AutoSize = true;
            this.Lb_Ttjj.Location = new System.Drawing.Point(22, 153);
            this.Lb_Ttjj.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Ttjj.Name = "Lb_Ttjj";
            this.Lb_Ttjj.Size = new System.Drawing.Size(119, 20);
            this.Lb_Ttjj.TabIndex = 8;
            this.Lb_Ttjj.Text = "探头间距mm:";
            // 
            // Txt_Thick
            // 
            this.Txt_Thick.Location = new System.Drawing.Point(460, 82);
            this.Txt_Thick.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Txt_Thick.Name = "Txt_Thick";
            this.Txt_Thick.Size = new System.Drawing.Size(96, 30);
            this.Txt_Thick.TabIndex = 7;
            // 
            // Lb_Hd
            // 
            this.Lb_Hd.AutoSize = true;
            this.Lb_Hd.Location = new System.Drawing.Point(335, 90);
            this.Lb_Hd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Hd.Name = "Lb_Hd";
            this.Lb_Hd.Size = new System.Drawing.Size(79, 20);
            this.Lb_Hd.TabIndex = 6;
            this.Lb_Hd.Text = "厚度mm:";
            // 
            // Cmb_DetectionSite
            // 
            this.Cmb_DetectionSite.FormattingEnabled = true;
            this.Cmb_DetectionSite.Items.AddRange(new object[] {
            "外壁",
            "内壁"});
            this.Cmb_DetectionSite.Location = new System.Drawing.Point(152, 88);
            this.Cmb_DetectionSite.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Cmb_DetectionSite.Name = "Cmb_DetectionSite";
            this.Cmb_DetectionSite.Size = new System.Drawing.Size(96, 28);
            this.Cmb_DetectionSite.TabIndex = 5;
            this.Cmb_DetectionSite.Text = "外壁";
            // 
            // Lb_NbWb
            // 
            this.Lb_NbWb.AutoSize = true;
            this.Lb_NbWb.Location = new System.Drawing.Point(22, 90);
            this.Lb_NbWb.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_NbWb.Name = "Lb_NbWb";
            this.Lb_NbWb.Size = new System.Drawing.Size(99, 20);
            this.Lb_NbWb.TabIndex = 4;
            this.Lb_NbWb.Text = "焊缝位置:";
            // 
            // Lb_Bh
            // 
            this.Lb_Bh.AutoSize = true;
            this.Lb_Bh.Location = new System.Drawing.Point(335, 33);
            this.Lb_Bh.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Bh.Name = "Lb_Bh";
            this.Lb_Bh.Size = new System.Drawing.Size(99, 20);
            this.Lb_Bh.TabIndex = 2;
            this.Lb_Bh.Text = "焊缝编号:";
            // 
            // Lb_Weld_ID
            // 
            this.Lb_Weld_ID.AutoSize = true;
            this.Lb_Weld_ID.ForeColor = System.Drawing.Color.Blue;
            this.Lb_Weld_ID.Location = new System.Drawing.Point(107, 33);
            this.Lb_Weld_ID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Weld_ID.Name = "Lb_Weld_ID";
            this.Lb_Weld_ID.Size = new System.Drawing.Size(149, 20);
            this.Lb_Weld_ID.TabIndex = 1;
            this.Lb_Weld_ID.Text = "20210819140500";
            this.toolTip1.SetToolTip(this.Lb_Weld_ID, "点击：建立当前焊缝唯一的索引号");
            this.Lb_Weld_ID.Click += new System.EventHandler(this.Lb_Weld_ID_Click);
            // 
            // Lb_ID
            // 
            this.Lb_ID.AutoSize = true;
            this.Lb_ID.Location = new System.Drawing.Point(22, 33);
            this.Lb_ID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_ID.Name = "Lb_ID";
            this.Lb_ID.Size = new System.Drawing.Size(79, 20);
            this.Lb_ID.TabIndex = 0;
            this.Lb_ID.Text = "焊缝ID:";
            // 
            // Bt_CanCel
            // 
            this.Bt_CanCel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_CanCel.Location = new System.Drawing.Point(643, 466);
            this.Bt_CanCel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Bt_CanCel.Name = "Bt_CanCel";
            this.Bt_CanCel.Size = new System.Drawing.Size(97, 37);
            this.Bt_CanCel.TabIndex = 1;
            this.Bt_CanCel.Text = "取消";
            this.Bt_CanCel.UseVisualStyleBackColor = true;
            this.Bt_CanCel.Click += new System.EventHandler(this.Bt_CanCel_Click);
            // 
            // Bt_Ok
            // 
            this.Bt_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_Ok.Location = new System.Drawing.Point(504, 466);
            this.Bt_Ok.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Bt_Ok.Name = "Bt_Ok";
            this.Bt_Ok.Size = new System.Drawing.Size(97, 37);
            this.Bt_Ok.TabIndex = 0;
            this.Bt_Ok.Text = "确认";
            this.Bt_Ok.UseVisualStyleBackColor = true;
            this.Bt_Ok.Click += new System.EventHandler(this.Bt_Ok_Click);
            // 
            // History
            // 
            this.History.Controls.Add(this.tableLayoutPanel1);
            this.History.Location = new System.Drawing.Point(4, 30);
            this.History.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.History.Name = "History";
            this.History.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.History.Size = new System.Drawing.Size(790, 505);
            this.History.TabIndex = 1;
            this.History.Text = "     已检     ";
            this.History.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Dg_Alarm, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Dg_Part, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 497);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // Dg_Alarm
            // 
            this.Dg_Alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dg_Alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dg_Alarm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dg_Alarm.Location = new System.Drawing.Point(3, 299);
            this.Dg_Alarm.MultiSelect = false;
            this.Dg_Alarm.Name = "Dg_Alarm";
            this.Dg_Alarm.RowHeadersWidth = 62;
            this.Dg_Alarm.RowTemplate.Height = 23;
            this.Dg_Alarm.Size = new System.Drawing.Size(778, 195);
            this.Dg_Alarm.TabIndex = 8;
            // 
            // Dg_Part
            // 
            this.Dg_Part.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dg_Part.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dg_Part.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dg_Part.Location = new System.Drawing.Point(3, 52);
            this.Dg_Part.MultiSelect = false;
            this.Dg_Part.Name = "Dg_Part";
            this.Dg_Part.RowHeadersWidth = 62;
            this.Dg_Part.RowTemplate.Height = 23;
            this.Dg_Part.Size = new System.Drawing.Size(778, 192);
            this.Dg_Part.TabIndex = 7;
            this.Dg_Part.Click += new System.EventHandler(this.Dg_Part_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Lb_SubID_Val);
            this.panel2.Controls.Add(this.Bt_Delete);
            this.panel2.Controls.Add(this.Lb_Item);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(780, 43);
            this.panel2.TabIndex = 0;
            // 
            // Lb_SubID_Val
            // 
            this.Lb_SubID_Val.AutoSize = true;
            this.Lb_SubID_Val.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lb_SubID_Val.Location = new System.Drawing.Point(104, 9);
            this.Lb_SubID_Val.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_SubID_Val.Name = "Lb_SubID_Val";
            this.Lb_SubID_Val.Size = new System.Drawing.Size(29, 20);
            this.Lb_SubID_Val.TabIndex = 4;
            this.Lb_SubID_Val.Text = "  ";
            // 
            // Bt_Delete
            // 
            this.Bt_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_Delete.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Delete.Location = new System.Drawing.Point(554, 3);
            this.Bt_Delete.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Bt_Delete.Name = "Bt_Delete";
            this.Bt_Delete.Size = new System.Drawing.Size(93, 37);
            this.Bt_Delete.TabIndex = 3;
            this.Bt_Delete.Text = "删除";
            this.Bt_Delete.UseVisualStyleBackColor = true;
            this.Bt_Delete.Click += new System.EventHandler(this.Bt_Delete_Click);
            // 
            // Lb_Item
            // 
            this.Lb_Item.AutoSize = true;
            this.Lb_Item.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lb_Item.Location = new System.Drawing.Point(11, 9);
            this.Lb_Item.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Item.Name = "Lb_Item";
            this.Lb_Item.Size = new System.Drawing.Size(89, 20);
            this.Lb_Item.TabIndex = 0;
            this.Lb_Item.Text = "1 焊缝：";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Bt_Original);
            this.panel3.Controls.Add(this.Lb_Alarm);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(2, 250);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(780, 43);
            this.panel3.TabIndex = 1;
            // 
            // Bt_Original
            // 
            this.Bt_Original.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bt_Original.Enabled = false;
            this.Bt_Original.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bt_Original.Location = new System.Drawing.Point(541, 3);
            this.Bt_Original.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Bt_Original.Name = "Bt_Original";
            this.Bt_Original.Size = new System.Drawing.Size(148, 37);
            this.Bt_Original.TabIndex = 2;
            this.Bt_Original.Text = "查看原始数据";
            this.Bt_Original.UseVisualStyleBackColor = true;
            this.Bt_Original.Click += new System.EventHandler(this.Bt_Original_Click);
            // 
            // Lb_Alarm
            // 
            this.Lb_Alarm.AutoSize = true;
            this.Lb_Alarm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lb_Alarm.Location = new System.Drawing.Point(11, 9);
            this.Lb_Alarm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lb_Alarm.Name = "Lb_Alarm";
            this.Lb_Alarm.Size = new System.Drawing.Size(129, 20);
            this.Lb_Alarm.TabIndex = 1;
            this.Lb_Alarm.Text = "2 缺陷数据：";
            // 
            // Frm_Weld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 539);
            this.Controls.Add(this.Tab_Contrl);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimizeBox = false;
            this.Name = "Frm_Weld";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "焊缝管理";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Weld_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Weld_Load);
            this.Tab_Contrl.ResumeLayout(false);
            this.Input.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.History.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dg_Alarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dg_Part)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tab_Contrl;
        private System.Windows.Forms.TabPage Input;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Bt_CanCel;
        private System.Windows.Forms.Button Bt_Ok;
        private System.Windows.Forms.TabPage History;
        private System.Windows.Forms.Label Lb_Weld_ID;
        private System.Windows.Forms.Label Lb_ID;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label Lb_Bh;
        private System.Windows.Forms.ComboBox Cmb_DetectionSite;
        private System.Windows.Forms.Label Lb_NbWb;
        private System.Windows.Forms.TextBox Txt_ProbeSpacing;
        private System.Windows.Forms.Label Lb_Ttjj;
        private System.Windows.Forms.TextBox Txt_Thick;
        private System.Windows.Forms.Label Lb_Hd;
        private System.Windows.Forms.TextBox Txt_ReMark;
        private System.Windows.Forms.Label Lb_Bz;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView Dg_Alarm;
        private System.Windows.Forms.DataGridView Dg_Part;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Lb_Item;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label Lb_Alarm;
        private System.Windows.Forms.Button Bt_Original;
        private System.Windows.Forms.ComboBox Cmb_Bh;
        private System.Windows.Forms.Button Bt_Delete;
        private System.Windows.Forms.Label Lb_SubID_Val;
        private System.Windows.Forms.Button Bt_New;
    }
}