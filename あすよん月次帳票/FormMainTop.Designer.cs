namespace あすよん月次帳票
{
    partial class FormMainTop
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
            this.lbPASS = new System.Windows.Forms.Label();
            this.txtBxPASS = new System.Windows.Forms.TextBox();
            this.txtBxID = new System.Windows.Forms.TextBox();
            this.IbID = new System.Windows.Forms.Label();
            this.btn開始 = new System.Windows.Forms.Button();
            this.pctBxあすよん = new System.Windows.Forms.PictureBox();
            this.lbログイン = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxあすよん)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPASS
            // 
            this.lbPASS.AutoSize = true;
            this.lbPASS.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbPASS.Location = new System.Drawing.Point(25, 187);
            this.lbPASS.Name = "lbPASS";
            this.lbPASS.Size = new System.Drawing.Size(54, 19);
            this.lbPASS.TabIndex = 35;
            this.lbPASS.Text = "PASS:";
            // 
            // txtBxPASS
            // 
            this.txtBxPASS.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxPASS.Location = new System.Drawing.Point(81, 187);
            this.txtBxPASS.Name = "txtBxPASS";
            this.txtBxPASS.Size = new System.Drawing.Size(115, 24);
            this.txtBxPASS.TabIndex = 1;
            // 
            // txtBxID
            // 
            this.txtBxID.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxID.Location = new System.Drawing.Point(81, 154);
            this.txtBxID.Name = "txtBxID";
            this.txtBxID.Size = new System.Drawing.Size(115, 24);
            this.txtBxID.TabIndex = 0;
            // 
            // IbID
            // 
            this.IbID.AutoSize = true;
            this.IbID.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.IbID.Location = new System.Drawing.Point(47, 155);
            this.IbID.Name = "IbID";
            this.IbID.Size = new System.Drawing.Size(32, 19);
            this.IbID.TabIndex = 33;
            this.IbID.Text = "ID:";
            // 
            // btn開始
            // 
            this.btn開始.BackColor = System.Drawing.SystemColors.Control;
            this.btn開始.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn開始.Location = new System.Drawing.Point(229, 170);
            this.btn開始.Name = "btn開始";
            this.btn開始.Size = new System.Drawing.Size(86, 28);
            this.btn開始.TabIndex = 2;
            this.btn開始.Text = "開始";
            this.btn開始.UseVisualStyleBackColor = false;
            this.btn開始.Click += new System.EventHandler(this.btnStart_Clicked);
            // 
            // pctBxあすよん
            // 
            this.pctBxあすよん.Image = global::あすよん月次帳票.Properties.Resources.あすよん月次帳票;
            this.pctBxあすよん.Location = new System.Drawing.Point(22, 12);
            this.pctBxあすよん.Name = "pctBxあすよん";
            this.pctBxあすよん.Size = new System.Drawing.Size(313, 87);
            this.pctBxあすよん.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pctBxあすよん.TabIndex = 40;
            this.pctBxあすよん.TabStop = false;
            // 
            // lbログイン
            // 
            this.lbログイン.AutoSize = true;
            this.lbログイン.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbログイン.Location = new System.Drawing.Point(25, 123);
            this.lbログイン.Name = "lbログイン";
            this.lbログイン.Size = new System.Drawing.Size(80, 19);
            this.lbログイン.TabIndex = 41;
            this.lbログイン.Text = "【 ログイン 】";
            // 
            // FormMainTop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 227);
            this.Controls.Add(this.lbログイン);
            this.Controls.Add(this.pctBxあすよん);
            this.Controls.Add(this.btn開始);
            this.Controls.Add(this.lbPASS);
            this.Controls.Add(this.txtBxPASS);
            this.Controls.Add(this.txtBxID);
            this.Controls.Add(this.IbID);
            this.Name = "FormMainTop";
            this.Text = "FormMainTop";
            ((System.ComponentModel.ISupportInitialize)(this.pctBxあすよん)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbPASS;
        private System.Windows.Forms.TextBox txtBxPASS;
        private System.Windows.Forms.TextBox txtBxID;
        private System.Windows.Forms.Label IbID;
        private System.Windows.Forms.Button btn開始;
        private System.Windows.Forms.PictureBox pctBxあすよん;
        private System.Windows.Forms.Label lbログイン;
    }
}