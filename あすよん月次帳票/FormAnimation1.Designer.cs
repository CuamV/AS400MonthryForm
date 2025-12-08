using System.Drawing;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class FormAnimation1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictBx1;
        private Label lblMessage;

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
            this.pictBx1 = new System.Windows.Forms.PictureBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictBx1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictBx1
            // 
            this.pictBx1.ImageLocation = "\\\\ohnosv01\\OhnoSys\\099_sys\\Images\\Infiter_Big.gif";
            this.pictBx1.Location = new System.Drawing.Point(34, 12);
            this.pictBx1.Name = "pictBx1";
            this.pictBx1.Size = new System.Drawing.Size(328, 79);
            this.pictBx1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBx1.TabIndex = 0;
            this.pictBx1.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.DimGray;
            this.lblMessage.Location = new System.Drawing.Point(0, 183);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(397, 60);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "シュミレーション処理を行い、\r\nライブラリに在庫ファイルを作成中です...\r\n";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(55, 138);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(283, 11);
            this.progressBar1.TabIndex = 2;
            // 
            // FormAnimation1
            // 
            this.ClientSize = new System.Drawing.Size(397, 243);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictBx1);
            this.Controls.Add(this.lblMessage);
            this.Name = "FormAnimation1";
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictBx1)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private ProgressBar progressBar1;
    }
}