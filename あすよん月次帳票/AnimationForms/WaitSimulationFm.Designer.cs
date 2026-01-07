using System.Drawing;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class WaitSimulationFm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaitSimulationFm));
            this.pictBx1 = new System.Windows.Forms.PictureBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.btnMin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictBx1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictBx1
            // 
            this.pictBx1.ImageLocation = "\\\\ohnosv01\\OhnoSys\\099_sys\\Images\\Infiter_Big.gif";
            this.pictBx1.Location = new System.Drawing.Point(34, 35);
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
            this.progressBar1.Location = new System.Drawing.Point(55, 148);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(283, 11);
            this.progressBar1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Snow;
            this.button1.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(357, 1);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 20);
            this.button1.TabIndex = 83;
            this.button1.Text = "✕";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnMin
            // 
            this.btnMin.BackColor = System.Drawing.Color.Snow;
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(318, 1);
            this.btnMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(33, 20);
            this.btnMin.TabIndex = 82;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = false;
            // 
            // WaitSimulationFm
            // 
            this.ClientSize = new System.Drawing.Size(397, 243);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictBx1);
            this.Controls.Add(this.lblMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WaitSimulationFm";
            this.Opacity = 0.95D;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictBx1)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private ProgressBar progressBar1;
        private Button button1;
        private Button btnMin;
    }
}