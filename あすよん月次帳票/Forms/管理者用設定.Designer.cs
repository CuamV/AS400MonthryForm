namespace あすよん月次帳票
{
    partial class 管理者用設定
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
            this.btnAS400取引先マスタ取得 = new System.Windows.Forms.Button();
            this.btnAS400取引先マスタエクスポート = new System.Windows.Forms.Button();
            this.btnAS400取引先マスタ展開 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAS400取引先マスタ取得
            // 
            this.btnAS400取引先マスタ取得.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAS400取引先マスタ取得.Location = new System.Drawing.Point(51, 74);
            this.btnAS400取引先マスタ取得.Name = "btnAS400取引先マスタ取得";
            this.btnAS400取引先マスタ取得.Size = new System.Drawing.Size(173, 36);
            this.btnAS400取引先マスタ取得.TabIndex = 16;
            this.btnAS400取引先マスタ取得.Text = "AS400取引先マスタ取得";
            this.btnAS400取引先マスタ取得.UseVisualStyleBackColor = true;
            this.btnAS400取引先マスタ取得.Click += new System.EventHandler(this.btnAS400取引先マスタ取得_Click);
            // 
            // btnAS400取引先マスタエクスポート
            // 
            this.btnAS400取引先マスタエクスポート.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAS400取引先マスタエクスポート.Location = new System.Drawing.Point(51, 131);
            this.btnAS400取引先マスタエクスポート.Name = "btnAS400取引先マスタエクスポート";
            this.btnAS400取引先マスタエクスポート.Size = new System.Drawing.Size(173, 36);
            this.btnAS400取引先マスタエクスポート.TabIndex = 17;
            this.btnAS400取引先マスタエクスポート.Text = "AS400取引先マスタエクスポート";
            this.btnAS400取引先マスタエクスポート.UseVisualStyleBackColor = true;
            this.btnAS400取引先マスタエクスポート.Click += new System.EventHandler(this.btnAS400取引先マスタエクスポート_Click);
            // 
            // btnAS400取引先マスタ展開
            // 
            this.btnAS400取引先マスタ展開.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAS400取引先マスタ展開.Location = new System.Drawing.Point(51, 190);
            this.btnAS400取引先マスタ展開.Name = "btnAS400取引先マスタ展開";
            this.btnAS400取引先マスタ展開.Size = new System.Drawing.Size(173, 36);
            this.btnAS400取引先マスタ展開.TabIndex = 18;
            this.btnAS400取引先マスタ展開.Text = "AS400取引先マスタ展開";
            this.btnAS400取引先マスタ展開.UseVisualStyleBackColor = true;
            this.btnAS400取引先マスタ展開.Click += new System.EventHandler(this.btnAS400取引先マスタ展開_Click);
            // 
            // 管理者用設定
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAS400取引先マスタ展開);
            this.Controls.Add(this.btnAS400取引先マスタエクスポート);
            this.Controls.Add(this.btnAS400取引先マスタ取得);
            this.Name = "管理者用設定";
            this.Text = "メンテナンス";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAS400取引先マスタ取得;
        private System.Windows.Forms.Button btnAS400取引先マスタエクスポート;
        private System.Windows.Forms.Button btnAS400取引先マスタ展開;
    }
}