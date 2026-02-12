namespace あすよん月次帳票
{
    partial class ユーザーマスタ照会Fm
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.lb部門 = new System.Windows.Forms.Label();
            this.listBx部門 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxコード検索 = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.Margin = new System.Windows.Forms.Padding(4);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(834, 595);
            this.dgv.TabIndex = 1;
            // 
            // lb部門
            // 
            this.lb部門.AutoSize = true;
            this.lb部門.Location = new System.Drawing.Point(599, 614);
            this.lb部門.Name = "lb部門";
            this.lb部門.Size = new System.Drawing.Size(53, 15);
            this.lb部門.TabIndex = 10;
            this.lb部門.Text = "<部門>";
            // 
            // listBx部門
            // 
            this.listBx部門.FormattingEnabled = true;
            this.listBx部門.ItemHeight = 15;
            this.listBx部門.Items.AddRange(new object[] {
            "コード検索した取引先が紐づく部門を表示します"});
            this.listBx部門.Location = new System.Drawing.Point(602, 643);
            this.listBx部門.Name = "listBx部門";
            this.listBx部門.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBx部門.Size = new System.Drawing.Size(189, 94);
            this.listBx部門.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 614);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "<コード検索>";
            // 
            // txtBxコード検索
            // 
            this.txtBxコード検索.AcceptsReturn = true;
            this.txtBxコード検索.Location = new System.Drawing.Point(12, 643);
            this.txtBxコード検索.Multiline = true;
            this.txtBxコード検索.Name = "txtBxコード検索";
            this.txtBxコード検索.Size = new System.Drawing.Size(288, 94);
            this.txtBxコード検索.TabIndex = 7;
            this.txtBxコード検索.Text = "複数コード検索時は、カンマ/スペース/読点/改行のいずれかで区切ってください";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.Location = new System.Drawing.Point(363, 682);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(109, 35);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ユーザーマスタ照会Fm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 748);
            this.Controls.Add(this.lb部門);
            this.Controls.Add(this.listBx部門);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBxコード検索);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgv);
            this.Name = "ユーザーマスタ照会Fm";
            this.Text = "ユーザーマスタ照会";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label lb部門;
        private System.Windows.Forms.ListBox listBx部門;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBxコード検索;
        private System.Windows.Forms.Button btnClose;
    }
}