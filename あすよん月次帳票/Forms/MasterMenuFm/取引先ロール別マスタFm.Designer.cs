namespace あすよん月次帳票
{
    partial class 取引先ロール別マスタFm
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
            this.lb取引先ロール = new System.Windows.Forms.Label();
            this.chkListBx取引先ロール = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // lb取引先ロール
            // 
            this.lb取引先ロール.AutoSize = true;
            this.lb取引先ロール.Location = new System.Drawing.Point(94, 159);
            this.lb取引先ロール.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb取引先ロール.Name = "lb取引先ロール";
            this.lb取引先ロール.Size = new System.Drawing.Size(91, 17);
            this.lb取引先ロール.TabIndex = 42;
            this.lb取引先ロール.Text = "取引先ロール：";
            // 
            // chkListBx取引先ロール
            // 
            this.chkListBx取引先ロール.CheckOnClick = true;
            this.chkListBx取引先ロール.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkListBx取引先ロール.FormattingEnabled = true;
            this.chkListBx取引先ロール.Items.AddRange(new object[] {
            "商社",
            "仕入先",
            "販売先",
            "得意先",
            "出荷先",
            "預り先",
            "運送便",
            "倉庫"});
            this.chkListBx取引先ロール.Location = new System.Drawing.Point(98, 187);
            this.chkListBx取引先ロール.Margin = new System.Windows.Forms.Padding(4);
            this.chkListBx取引先ロール.Name = "chkListBx取引先ロール";
            this.chkListBx取引先ロール.Size = new System.Drawing.Size(137, 156);
            this.chkListBx取引先ロール.TabIndex = 41;
            // 
            // 取引先ロール別マスタFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 466);
            this.Controls.Add(this.lb取引先ロール);
            this.Controls.Add(this.chkListBx取引先ロール);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "取引先ロール別マスタFm";
            this.Text = "取引先ロール別マスタFm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb取引先ロール;
        private System.Windows.Forms.CheckedListBox chkListBx取引先ロール;
    }
}