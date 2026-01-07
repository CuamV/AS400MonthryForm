namespace あすよん月次帳票
{
    partial class 郵便番号辞書インポートFm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(郵便番号辞書インポートFm));
            this.btnファイル選択 = new System.Windows.Forms.Button();
            this.txtBxファイル選択 = new System.Windows.Forms.TextBox();
            this.btnインポート = new System.Windows.Forms.Button();
            this.btn戻る = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblステータス = new System.Windows.Forms.Label();
            this.lbステータス = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnファイル選択
            // 
            this.btnファイル選択.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnファイル選択.Location = new System.Drawing.Point(265, 56);
            this.btnファイル選択.Name = "btnファイル選択";
            this.btnファイル選択.Size = new System.Drawing.Size(77, 26);
            this.btnファイル選択.TabIndex = 0;
            this.btnファイル選択.Text = "ファイル選択";
            this.btnファイル選択.UseVisualStyleBackColor = false;
            this.btnファイル選択.Click += new System.EventHandler(this.btnファイル選択_Click);
            // 
            // txtBxファイル選択
            // 
            this.txtBxファイル選択.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxファイル選択.Location = new System.Drawing.Point(44, 57);
            this.txtBxファイル選択.Name = "txtBxファイル選択";
            this.txtBxファイル選択.Size = new System.Drawing.Size(214, 23);
            this.txtBxファイル選択.TabIndex = 1;
            // 
            // btnインポート
            // 
            this.btnインポート.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnインポート.Location = new System.Drawing.Point(110, 127);
            this.btnインポート.Name = "btnインポート";
            this.btnインポート.Size = new System.Drawing.Size(75, 26);
            this.btnインポート.TabIndex = 2;
            this.btnインポート.Text = "インポート";
            this.btnインポート.UseVisualStyleBackColor = true;
            this.btnインポート.Click += new System.EventHandler(this.btnインポート_Click);
            // 
            // btn戻る
            // 
            this.btn戻る.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn戻る.Location = new System.Drawing.Point(206, 127);
            this.btn戻る.Name = "btn戻る";
            this.btn戻る.Size = new System.Drawing.Size(75, 26);
            this.btn戻る.TabIndex = 3;
            this.btn戻る.Text = "戻る";
            this.btn戻る.UseVisualStyleBackColor = true;
            this.btn戻る.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(44, 96);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(298, 16);
            this.progressBar.TabIndex = 4;
            this.progressBar.Visible = false;
            // 
            // lblステータス
            // 
            this.lblステータス.AutoSize = true;
            this.lblステータス.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lblステータス.Location = new System.Drawing.Point(41, 20);
            this.lblステータス.Name = "lblステータス";
            this.lblステータス.Size = new System.Drawing.Size(0, 15);
            this.lblステータス.TabIndex = 5;
            // 
            // lbステータス
            // 
            this.lbステータス.AutoSize = true;
            this.lbステータス.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.lbステータス.Location = new System.Drawing.Point(41, 79);
            this.lbステータス.Name = "lbステータス";
            this.lbステータス.Size = new System.Drawing.Size(0, 15);
            this.lbステータス.TabIndex = 6;
            // 
            // 郵便番号辞書インポートFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 182);
            this.Controls.Add(this.lbステータス);
            this.Controls.Add(this.lblステータス);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btn戻る);
            this.Controls.Add(this.btnインポート);
            this.Controls.Add(this.txtBxファイル選択);
            this.Controls.Add(this.btnファイル選択);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "郵便番号辞書インポートFm";
            this.Text = "郵便番号辞書インポート";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnファイル選択;
        private System.Windows.Forms.TextBox txtBxファイル選択;
        private System.Windows.Forms.Button btnインポート;
        private System.Windows.Forms.Button btn戻る;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblステータス;
        private System.Windows.Forms.Label lbステータス;
    }
}