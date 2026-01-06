namespace あすよん月次帳票
{
    partial class 部門マスタForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(部門マスタForm));
            this.btn登録 = new System.Windows.Forms.Button();
            this.btn削除 = new System.Windows.Forms.Button();
            this.btn照会 = new System.Windows.Forms.Button();
            this.btn戻る = new System.Windows.Forms.Button();
            this.txtBx部門名 = new System.Windows.Forms.TextBox();
            this.lb部門名 = new System.Windows.Forms.Label();
            this.txtBx部門CD = new System.Windows.Forms.TextBox();
            this.lb部門CD = new System.Windows.Forms.Label();
            this.cmbBx会社 = new System.Windows.Forms.ComboBox();
            this.lb会社 = new System.Windows.Forms.Label();
            this.txtBx部門名カナ = new System.Windows.Forms.TextBox();
            this.lb部門名カナ = new System.Windows.Forms.Label();
            this.btnダウンロード = new System.Windows.Forms.Button();
            this.btnメニュー = new System.Windows.Forms.GroupBox();
            this.btnメニュー.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn登録
            // 
            this.btn登録.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn登録.Location = new System.Drawing.Point(105, 260);
            this.btn登録.Margin = new System.Windows.Forms.Padding(4);
            this.btn登録.Name = "btn登録";
            this.btn登録.Size = new System.Drawing.Size(72, 28);
            this.btn登録.TabIndex = 4;
            this.btn登録.Text = "登録";
            this.btn登録.UseVisualStyleBackColor = true;
            this.btn登録.Click += new System.EventHandler(this.btn登録_Click);
            // 
            // btn削除
            // 
            this.btn削除.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn削除.Location = new System.Drawing.Point(231, 260);
            this.btn削除.Margin = new System.Windows.Forms.Padding(4);
            this.btn削除.Name = "btn削除";
            this.btn削除.Size = new System.Drawing.Size(72, 28);
            this.btn削除.TabIndex = 5;
            this.btn削除.Text = "削除";
            this.btn削除.UseVisualStyleBackColor = true;
            this.btn削除.Click += new System.EventHandler(this.btn削除_Click);
            // 
            // btn照会
            // 
            this.btn照会.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn照会.Location = new System.Drawing.Point(8, 24);
            this.btn照会.Margin = new System.Windows.Forms.Padding(4);
            this.btn照会.Name = "btn照会";
            this.btn照会.Size = new System.Drawing.Size(80, 24);
            this.btn照会.TabIndex = 0;
            this.btn照会.Text = "照会";
            this.btn照会.UseVisualStyleBackColor = true;
            this.btn照会.Click += new System.EventHandler(this.btn照会_Click);
            // 
            // btn戻る
            // 
            this.btn戻る.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn戻る.Location = new System.Drawing.Point(208, 24);
            this.btn戻る.Margin = new System.Windows.Forms.Padding(4);
            this.btn戻る.Name = "btn戻る";
            this.btn戻る.Size = new System.Drawing.Size(80, 24);
            this.btn戻る.TabIndex = 2;
            this.btn戻る.Text = "戻る";
            this.btn戻る.UseVisualStyleBackColor = true;
            this.btn戻る.Click += new System.EventHandler(this.btn戻る_Click);
            // 
            // txtBx部門名
            // 
            this.txtBx部門名.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx部門名.Location = new System.Drawing.Point(78, 146);
            this.txtBx部門名.Margin = new System.Windows.Forms.Padding(4);
            this.txtBx部門名.MaxLength = 30;
            this.txtBx部門名.Name = "txtBx部門名";
            this.txtBx部門名.Size = new System.Drawing.Size(99, 25);
            this.txtBx部門名.TabIndex = 2;
            // 
            // lb部門名
            // 
            this.lb部門名.AutoSize = true;
            this.lb部門名.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb部門名.Location = new System.Drawing.Point(25, 149);
            this.lb部門名.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb部門名.Name = "lb部門名";
            this.lb部門名.Size = new System.Drawing.Size(50, 18);
            this.lb部門名.TabIndex = 9;
            this.lb部門名.Text = "部門名";
            // 
            // txtBx部門CD
            // 
            this.txtBx部門CD.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx部門CD.Location = new System.Drawing.Point(78, 105);
            this.txtBx部門CD.Margin = new System.Windows.Forms.Padding(4);
            this.txtBx部門CD.MaxLength = 3;
            this.txtBx部門CD.Name = "txtBx部門CD";
            this.txtBx部門CD.Size = new System.Drawing.Size(99, 25);
            this.txtBx部門CD.TabIndex = 1;
            this.txtBx部門CD.TextChanged += new System.EventHandler(this.txtBoxControl);
            // 
            // lb部門CD
            // 
            this.lb部門CD.AutoSize = true;
            this.lb部門CD.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb部門CD.Location = new System.Drawing.Point(20, 107);
            this.lb部門CD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb部門CD.Name = "lb部門CD";
            this.lb部門CD.Size = new System.Drawing.Size(55, 18);
            this.lb部門CD.TabIndex = 8;
            this.lb部門CD.Text = "部門CD";
            // 
            // cmbBx会社
            // 
            this.cmbBx会社.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbBx会社.FormattingEnabled = true;
            this.cmbBx会社.Items.AddRange(new object[] {
            "オーノ",
            "サンミックダスコン",
            "サンミックカーペット"});
            this.cmbBx会社.Location = new System.Drawing.Point(78, 191);
            this.cmbBx会社.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBx会社.Name = "cmbBx会社";
            this.cmbBx会社.Size = new System.Drawing.Size(99, 26);
            this.cmbBx会社.TabIndex = 0;
            // 
            // lb会社
            // 
            this.lb会社.AutoSize = true;
            this.lb会社.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb会社.Location = new System.Drawing.Point(39, 196);
            this.lb会社.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb会社.Name = "lb会社";
            this.lb会社.Size = new System.Drawing.Size(36, 18);
            this.lb会社.TabIndex = 7;
            this.lb会社.Text = "会社";
            // 
            // txtBx部門名カナ
            // 
            this.txtBx部門名カナ.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx部門名カナ.Location = new System.Drawing.Point(267, 146);
            this.txtBx部門名カナ.Margin = new System.Windows.Forms.Padding(4);
            this.txtBx部門名カナ.MaxLength = 32;
            this.txtBx部門名カナ.Name = "txtBx部門名カナ";
            this.txtBx部門名カナ.Size = new System.Drawing.Size(111, 25);
            this.txtBx部門名カナ.TabIndex = 3;
            // 
            // lb部門名カナ
            // 
            this.lb部門名カナ.AutoSize = true;
            this.lb部門名カナ.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb部門名カナ.Location = new System.Drawing.Point(195, 149);
            this.lb部門名カナ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb部門名カナ.Name = "lb部門名カナ";
            this.lb部門名カナ.Size = new System.Drawing.Size(71, 18);
            this.lb部門名カナ.TabIndex = 10;
            this.lb部門名カナ.Text = "部門名カナ";
            // 
            // btnダウンロード
            // 
            this.btnダウンロード.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnダウンロード.Location = new System.Drawing.Point(109, 24);
            this.btnダウンロード.Margin = new System.Windows.Forms.Padding(4);
            this.btnダウンロード.Name = "btnダウンロード";
            this.btnダウンロード.Size = new System.Drawing.Size(80, 24);
            this.btnダウンロード.TabIndex = 1;
            this.btnダウンロード.Text = "ダウンロード";
            this.btnダウンロード.UseVisualStyleBackColor = true;
            this.btnダウンロード.Click += new System.EventHandler(this.btnダウンロード_Click);
            // 
            // btnメニュー
            // 
            this.btnメニュー.Controls.Add(this.btn照会);
            this.btnメニュー.Controls.Add(this.btnダウンロード);
            this.btnメニュー.Controls.Add(this.btn戻る);
            this.btnメニュー.Location = new System.Drawing.Point(62, 20);
            this.btnメニュー.Name = "btnメニュー";
            this.btnメニュー.Size = new System.Drawing.Size(297, 59);
            this.btnメニュー.TabIndex = 6;
            this.btnメニュー.TabStop = false;
            // 
            // 部門マスタForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 319);
            this.Controls.Add(this.btnメニュー);
            this.Controls.Add(this.txtBx部門名カナ);
            this.Controls.Add(this.lb部門名カナ);
            this.Controls.Add(this.lb会社);
            this.Controls.Add(this.cmbBx会社);
            this.Controls.Add(this.txtBx部門名);
            this.Controls.Add(this.lb部門名);
            this.Controls.Add(this.txtBx部門CD);
            this.Controls.Add(this.lb部門CD);
            this.Controls.Add(this.btn削除);
            this.Controls.Add(this.btn登録);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "部門マスタForm";
            this.Text = "部門マスタForm";
            this.btnメニュー.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn登録;
        private System.Windows.Forms.Button btn削除;
        private System.Windows.Forms.Button btn照会;
        private System.Windows.Forms.Button btn戻る;
        private System.Windows.Forms.TextBox txtBx部門名;
        private System.Windows.Forms.Label lb部門名;
        private System.Windows.Forms.TextBox txtBx部門CD;
        private System.Windows.Forms.Label lb部門CD;
        private System.Windows.Forms.ComboBox cmbBx会社;
        private System.Windows.Forms.Label lb会社;
        private System.Windows.Forms.TextBox txtBx部門名カナ;
        private System.Windows.Forms.Label lb部門名カナ;
        private System.Windows.Forms.Button btnダウンロード;
        private System.Windows.Forms.GroupBox btnメニュー;
    }
}