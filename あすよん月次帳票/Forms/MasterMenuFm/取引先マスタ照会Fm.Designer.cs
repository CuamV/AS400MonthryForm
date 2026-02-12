namespace あすよん月次帳票
{
    partial class 取引先マスタ照会Fm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(取引先マスタ照会Fm));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtBxコード検索 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBx部門 = new System.Windows.Forms.ListBox();
            this.lb部門 = new System.Windows.Forms.Label();
            this.btn検索 = new System.Windows.Forms.Button();
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
            this.dgv.Size = new System.Drawing.Size(989, 595);
            this.dgv.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.Location = new System.Drawing.Point(440, 707);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(109, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txtBxコード検索
            // 
            this.txtBxコード検索.AcceptsReturn = true;
            this.txtBxコード検索.Location = new System.Drawing.Point(42, 636);
            this.txtBxコード検索.Multiline = true;
            this.txtBxコード検索.Name = "txtBxコード検索";
            this.txtBxコード検索.Size = new System.Drawing.Size(255, 106);
            this.txtBxコード検索.TabIndex = 2;
            this.txtBxコード検索.Text = "複数コード検索時は、カンマ/スペース/読点/改行のいずれかで区切ってください";
            this.txtBxコード検索.TextChanged += new System.EventHandler(this.txtBxコード検索_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 616);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "<コード検索>";
            // 
            // listBx部門
            // 
            this.listBx部門.FormattingEnabled = true;
            this.listBx部門.ItemHeight = 17;
            this.listBx部門.Items.AddRange(new object[] {
            "コード検索した取引先が紐づく部門を表示します"});
            this.listBx部門.Location = new System.Drawing.Point(759, 636);
            this.listBx部門.Name = "listBx部門";
            this.listBx部門.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBx部門.Size = new System.Drawing.Size(186, 140);
            this.listBx部門.TabIndex = 4;
            // 
            // lb部門
            // 
            this.lb部門.AutoSize = true;
            this.lb部門.Location = new System.Drawing.Point(756, 616);
            this.lb部門.Name = "lb部門";
            this.lb部門.Size = new System.Drawing.Size(54, 17);
            this.lb部門.TabIndex = 5;
            this.lb部門.Text = "<部門>";
            // 
            // btn検索
            // 
            this.btn検索.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn検索.Location = new System.Drawing.Point(205, 749);
            this.btn検索.Margin = new System.Windows.Forms.Padding(4);
            this.btn検索.Name = "btn検索";
            this.btn検索.Size = new System.Drawing.Size(73, 27);
            this.btn検索.TabIndex = 6;
            this.btn検索.Text = "検索";
            this.btn検索.UseVisualStyleBackColor = true;
            this.btn検索.Click += new System.EventHandler(this.btn検索_Click);
            // 
            // 取引先マスタ照会Fm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 785);
            this.Controls.Add(this.btn検索);
            this.Controls.Add(this.lb部門);
            this.Controls.Add(this.listBx部門);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBxコード検索);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "取引先マスタ照会Fm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "取引先マスタ照会";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBx検索;
        private System.Windows.Forms.TextBox txtBxコード検索;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBx部門;
        private System.Windows.Forms.Label lb部門;
        private System.Windows.Forms.Button btn検索;
    }
}