namespace あすよん月次帳票
{
    partial class StandardisedFormFm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StandardisedFormFm));
            this.linkLb = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbTitleSimulation = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBx月次帳票未確定 = new System.Windows.Forms.TextBox();
            this.listBx月次帳票未確定 = new System.Windows.Forms.ListBox();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnForm1Back = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLb
            // 
            this.linkLb.AutoSize = true;
            this.linkLb.BackColor = System.Drawing.SystemColors.Window;
            this.linkLb.Location = new System.Drawing.Point(10, 15);
            this.linkLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLb.Name = "linkLb";
            this.linkLb.Size = new System.Drawing.Size(102, 14);
            this.linkLb.TabIndex = 0;
            this.linkLb.TabStop = true;
            this.linkLb.Text = "◆ 月次帳票(未確定)";
            this.linkLb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.月次帳票未確定_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::あすよん月次帳票.Properties.Resources.ic_all_csm02;
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // lbTitleSimulation
            // 
            this.lbTitleSimulation.AutoSize = true;
            this.lbTitleSimulation.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitleSimulation.Location = new System.Drawing.Point(46, 14);
            this.lbTitleSimulation.Name = "lbTitleSimulation";
            this.lbTitleSimulation.Size = new System.Drawing.Size(126, 17);
            this.lbTitleSimulation.TabIndex = 73;
            this.lbTitleSimulation.Text = "< 定型帳票メニュー>";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtBx月次帳票未確定);
            this.panel1.Controls.Add(this.linkLb);
            this.panel1.Controls.Add(this.listBx月次帳票未確定);
            this.panel1.Location = new System.Drawing.Point(23, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 49);
            this.panel1.TabIndex = 74;
            // 
            // txtBx月次帳票未確定
            // 
            this.txtBx月次帳票未確定.BackColor = System.Drawing.SystemColors.Window;
            this.txtBx月次帳票未確定.Location = new System.Drawing.Point(155, 4);
            this.txtBx月次帳票未確定.Multiline = true;
            this.txtBx月次帳票未確定.Name = "txtBx月次帳票未確定";
            this.txtBx月次帳票未確定.ReadOnly = true;
            this.txtBx月次帳票未確定.Size = new System.Drawing.Size(287, 39);
            this.txtBx月次帳票未確定.TabIndex = 76;
            this.txtBx月次帳票未確定.Text = "月次処理前の、当月のデータ(売上・仕入・在庫)を\r\n分類・部門グループで集計した金額を取得";
            this.txtBx月次帳票未確定.WordWrap = false;
            // 
            // listBx月次帳票未確定
            // 
            this.listBx月次帳票未確定.FormattingEnabled = true;
            this.listBx月次帳票未確定.ItemHeight = 17;
            this.listBx月次帳票未確定.Location = new System.Drawing.Point(5, 5);
            this.listBx月次帳票未確定.Name = "listBx月次帳票未確定";
            this.listBx月次帳票未確定.Size = new System.Drawing.Size(149, 38);
            this.listBx月次帳票未確定.TabIndex = 75;
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(451, 10);
            this.btnMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(33, 20);
            this.btnMin.TabIndex = 75;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnForm1Back
            // 
            this.btnForm1Back.Font = new System.Drawing.Font("Meiryo UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnForm1Back.Location = new System.Drawing.Point(393, 282);
            this.btnForm1Back.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnForm1Back.Name = "btnForm1Back";
            this.btnForm1Back.Size = new System.Drawing.Size(80, 34);
            this.btnForm1Back.TabIndex = 77;
            this.btnForm1Back.Text = "戻る";
            this.btnForm1Back.UseVisualStyleBackColor = true;
            this.btnForm1Back.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // StandardisedFormFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 327);
            this.Controls.Add(this.btnForm1Back);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbTitleSimulation);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StandardisedFormFm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "定型帳票メニュー";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLb;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbTitleSimulation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBx月次帳票未確定;
        private System.Windows.Forms.TextBox txtBx月次帳票未確定;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnForm1Back;
    }
}