
namespace Tofd_AWI.From
{
    partial class Frm_Print
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Bt_Item_Delet = new System.Windows.Forms.Button();
            this.Dg_Item = new System.Windows.Forms.DataGridView();
            this.Bt_Item_Add = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Dgr_Records = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Dgr_Record_Curr = new System.Windows.Forms.DataGridView();
            this.Dgr_Record_Alarm = new System.Windows.Forms.DataGridView();
            this.Bt_Ok = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Prg_Print_Bar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dg_Item)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgr_Records)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgr_Record_Curr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dgr_Record_Alarm)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.Bt_Item_Delet);
            this.groupBox1.Controls.Add(this.Dg_Item);
            this.groupBox1.Controls.Add(this.Bt_Item_Add);
            this.groupBox1.Location = new System.Drawing.Point(15, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(466, 371);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1 本报告包含的检测项目";
            // 
            // Bt_Item_Delet
            // 
            this.Bt_Item_Delet.Location = new System.Drawing.Point(239, 31);
            this.Bt_Item_Delet.Margin = new System.Windows.Forms.Padding(4);
            this.Bt_Item_Delet.Name = "Bt_Item_Delet";
            this.Bt_Item_Delet.Size = new System.Drawing.Size(154, 48);
            this.Bt_Item_Delet.TabIndex = 8;
            this.Bt_Item_Delet.Text = "删除项目";
            this.Bt_Item_Delet.UseVisualStyleBackColor = true;
            this.Bt_Item_Delet.Click += new System.EventHandler(this.Bt_Item_Delet_Click);
            // 
            // Dg_Item
            // 
            this.Dg_Item.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dg_Item.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dg_Item.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dg_Item.Location = new System.Drawing.Point(9, 88);
            this.Dg_Item.Margin = new System.Windows.Forms.Padding(5);
            this.Dg_Item.MultiSelect = false;
            this.Dg_Item.Name = "Dg_Item";
            this.Dg_Item.RowHeadersWidth = 62;
            this.Dg_Item.RowTemplate.Height = 23;
            this.Dg_Item.Size = new System.Drawing.Size(448, 283);
            this.Dg_Item.TabIndex = 7;
            this.Dg_Item.Click += new System.EventHandler(this.Dg_Item_Click);
            // 
            // Bt_Item_Add
            // 
            this.Bt_Item_Add.Location = new System.Drawing.Point(41, 31);
            this.Bt_Item_Add.Margin = new System.Windows.Forms.Padding(4);
            this.Bt_Item_Add.Name = "Bt_Item_Add";
            this.Bt_Item_Add.Size = new System.Drawing.Size(154, 48);
            this.Bt_Item_Add.TabIndex = 1;
            this.Bt_Item_Add.Text = "添加项目";
            this.Bt_Item_Add.UseVisualStyleBackColor = true;
            this.Bt_Item_Add.Click += new System.EventHandler(this.Bt_Item_Add_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.Dgr_Records);
            this.groupBox2.Location = new System.Drawing.Point(486, 16);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(477, 371);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2 选择项目对应的要打印的检测记录";
            // 
            // Dgr_Records
            // 
            this.Dgr_Records.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dgr_Records.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgr_Records.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dgr_Records.Location = new System.Drawing.Point(9, 32);
            this.Dgr_Records.Margin = new System.Windows.Forms.Padding(5);
            this.Dgr_Records.MultiSelect = false;
            this.Dgr_Records.Name = "Dgr_Records";
            this.Dgr_Records.RowHeadersWidth = 62;
            this.Dgr_Records.RowTemplate.Height = 23;
            this.Dgr_Records.Size = new System.Drawing.Size(459, 339);
            this.Dgr_Records.TabIndex = 7;
            this.Dgr_Records.Click += new System.EventHandler(this.Dgr_Records_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Location = new System.Drawing.Point(15, 395);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(948, 138);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3 选中检测记录的内容";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Dgr_Record_Curr, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Dgr_Record_Alarm, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(940, 107);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // Dgr_Record_Curr
            // 
            this.Dgr_Record_Curr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgr_Record_Curr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgr_Record_Curr.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dgr_Record_Curr.Location = new System.Drawing.Point(5, 5);
            this.Dgr_Record_Curr.Margin = new System.Windows.Forms.Padding(5);
            this.Dgr_Record_Curr.MultiSelect = false;
            this.Dgr_Record_Curr.Name = "Dgr_Record_Curr";
            this.Dgr_Record_Curr.RowHeadersWidth = 62;
            this.Dgr_Record_Curr.RowTemplate.Height = 23;
            this.Dgr_Record_Curr.Size = new System.Drawing.Size(460, 97);
            this.Dgr_Record_Curr.TabIndex = 7;
            // 
            // Dgr_Record_Alarm
            // 
            this.Dgr_Record_Alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgr_Record_Alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgr_Record_Alarm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.Dgr_Record_Alarm.Location = new System.Drawing.Point(475, 5);
            this.Dgr_Record_Alarm.Margin = new System.Windows.Forms.Padding(5);
            this.Dgr_Record_Alarm.MultiSelect = false;
            this.Dgr_Record_Alarm.Name = "Dgr_Record_Alarm";
            this.Dgr_Record_Alarm.RowHeadersWidth = 62;
            this.Dgr_Record_Alarm.RowTemplate.Height = 23;
            this.Dgr_Record_Alarm.Size = new System.Drawing.Size(460, 97);
            this.Dgr_Record_Alarm.TabIndex = 8;
            // 
            // Bt_Ok
            // 
            this.Bt_Ok.Location = new System.Drawing.Point(717, 541);
            this.Bt_Ok.Margin = new System.Windows.Forms.Padding(4);
            this.Bt_Ok.Name = "Bt_Ok";
            this.Bt_Ok.Size = new System.Drawing.Size(154, 48);
            this.Bt_Ok.TabIndex = 13;
            this.Bt_Ok.Text = "报表输出";
            this.Bt_Ok.UseVisualStyleBackColor = true;
            this.Bt_Ok.Click += new System.EventHandler(this.Bt_Ok_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.label4.Location = new System.Drawing.Point(11, 537);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(689, 40);
            this.label4.TabIndex = 40;
            this.label4.Text = "解释：  一个化工厂检测N个储罐，一个储罐，代表一个项目：对不同部位\r\n    焊缝的检测：罐顶、罐壁、罐底等；最后得到这个工厂的一个检测报告。\r\n";
            // 
            // Prg_Print_Bar
            // 
            this.Prg_Print_Bar.Location = new System.Drawing.Point(95, 279);
            this.Prg_Print_Bar.Margin = new System.Windows.Forms.Padding(4);
            this.Prg_Print_Bar.Name = "Prg_Print_Bar";
            this.Prg_Print_Bar.Size = new System.Drawing.Size(779, 42);
            this.Prg_Print_Bar.Step = 1;
            this.Prg_Print_Bar.TabIndex = 86;
            this.Prg_Print_Bar.Value = 2;
            this.Prg_Print_Bar.Visible = false;
            // 
            // Frm_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 600);
            this.Controls.Add(this.Prg_Print_Bar);
            this.Controls.Add(this.Bt_Ok);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Print";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择项目打印";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_Print_FormClosed);
            this.Load += new System.EventHandler(this.Frm_Print_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dg_Item)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgr_Records)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgr_Record_Curr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Dgr_Record_Alarm)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Bt_Item_Delet;
        private System.Windows.Forms.Button Bt_Item_Add;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView Dgr_Records;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView Dgr_Record_Curr;
        private System.Windows.Forms.DataGridView Dgr_Record_Alarm;
        private System.Windows.Forms.Button Bt_Ok;
        private System.Windows.Forms.DataGridView Dg_Item;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar Prg_Print_Bar;
    }
}