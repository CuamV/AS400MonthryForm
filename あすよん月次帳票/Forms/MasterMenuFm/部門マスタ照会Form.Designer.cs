namespace あすよん月次帳票
{
    partial class 部門マスタ照会Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(部門マスタ照会Form));
            this.dgv部門マスタ = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv部門マスタ)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv部門マスタ
            // 
            this.dgv部門マスタ.AllowUserToAddRows = false;
            this.dgv部門マスタ.AllowUserToDeleteRows = false;
            this.dgv部門マスタ.AllowUserToOrderColumns = true;
            this.dgv部門マスタ.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv部門マスタ.Location = new System.Drawing.Point(14, 36);
            this.dgv部門マスタ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgv部門マスタ.Name = "dgv部門マスタ";
            this.dgv部門マスタ.ReadOnly = true;
            this.dgv部門マスタ.RowTemplate.Height = 21;
            this.dgv部門マスタ.Size = new System.Drawing.Size(444, 244);
            this.dgv部門マスタ.TabIndex = 0;
            // 
            // 部門マスタ照会Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 294);
            this.Controls.Add(this.dgv部門マスタ);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "部門マスタ照会Form";
            this.Text = "部門マスタ照会Form";
            ((System.ComponentModel.ISupportInitialize)(this.dgv部門マスタ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv部門マスタ;
    }
}