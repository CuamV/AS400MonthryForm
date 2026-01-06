namespace あすよん月次帳票
{
    partial class WaitExcelExport
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.pictBx3 = new System.Windows.Forms.PictureBox();
            this.btnMin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictBx3)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.DimGray;
            this.lblMessage.Location = new System.Drawing.Point(0, 107);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(312, 52);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "Excelへデータをエクスポート中です…";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictBx3
            // 
            this.pictBx3.Image = global::あすよん月次帳票.Properties.Resources.Infiter_Small;
            this.pictBx3.Location = new System.Drawing.Point(76, 12);
            this.pictBx3.Name = "pictBx3";
            this.pictBx3.Size = new System.Drawing.Size(148, 100);
            this.pictBx3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBx3.TabIndex = 5;
            this.pictBx3.TabStop = false;
            // 
            // btnMin
            // 
            this.btnMin.BackColor = System.Drawing.Color.SeaShell;
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(274, 4);
            this.btnMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(33, 20);
            this.btnMin.TabIndex = 81;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = false;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // FormAnimation3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 159);
            this.ControlBox = false;
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.pictBx3);
            this.Controls.Add(this.lblMessage);
            this.Name = "FormAnimation3";
            this.Text = "FormAnimation";
            ((System.ComponentModel.ISupportInitialize)(this.pictBx3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBx3;
        private System.Windows.Forms.Button btnMin;
    }
}