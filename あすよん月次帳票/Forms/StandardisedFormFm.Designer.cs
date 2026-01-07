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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.linkLb.Size = new System.Drawing.Size(124, 17);
            this.linkLb.TabIndex = 0;
            this.linkLb.TabStop = true;
            this.linkLb.Text = "◆月次帳票(未確定)";
            this.linkLb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLb_LinkClicked);
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
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.linkLb);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Location = new System.Drawing.Point(23, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 49);
            this.panel1.TabIndex = 74;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(5, 5);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(149, 38);
            this.listBox1.TabIndex = 75;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(155, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(287, 39);
            this.textBox1.TabIndex = 76;
            this.textBox1.Text = "月次処理前の、当月のデータ(売上・仕入・在庫)を\r\n分類・部門グループで集計した金額を取得";
            this.textBox1.WordWrap = false;
            // 
            // StandardisedFormFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 327);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbTitleSimulation);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StandardisedFormFm";
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
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}