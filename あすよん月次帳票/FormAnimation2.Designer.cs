namespace あすよん月次帳票
{
    partial class FormAnimation2
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
            this.pictBx2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictBx2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Yu Gothic UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.DimGray;
            this.lblMessage.Location = new System.Drawing.Point(0, 86);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(228, 52);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictBx2
            // 
            this.pictBx2.Image = global::あすよん月次帳票.Properties.Resources.free_snowman;
            this.pictBx2.Location = new System.Drawing.Point(12, 12);
            this.pictBx2.Name = "pictBx2";
            this.pictBx2.Size = new System.Drawing.Size(70, 71);
            this.pictBx2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBx2.TabIndex = 5;
            this.pictBx2.TabStop = false;
            // 
            // FormAnimation2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 138);
            this.Controls.Add(this.pictBx2);
            this.Controls.Add(this.lblMessage);
            this.Name = "FormAnimation2";
            this.Text = "FormAnimatuin2";
            ((System.ComponentModel.ISupportInitialize)(this.pictBx2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictBx2;
    }
}