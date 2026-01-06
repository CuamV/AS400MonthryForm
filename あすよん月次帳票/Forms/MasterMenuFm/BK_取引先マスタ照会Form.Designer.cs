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
            this.dgv取引先マスタ = new System.Windows.Forms.DataGridView();
            this.grpBx検索 = new System.Windows.Forms.GroupBox();
            this.txtBx名称 = new System.Windows.Forms.TextBox();
            this.lb名称 = new System.Windows.Forms.Label();
            this.lbコード = new System.Windows.Forms.Label();
            this.txtBxコード = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv取引先マスタ)).BeginInit();
            this.grpBx検索.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv取引先マスタ
            // 
            this.dgv取引先マスタ.AllowUserToAddRows = false;
            this.dgv取引先マスタ.AllowUserToDeleteRows = false;
            this.dgv取引先マスタ.AllowUserToOrderColumns = true;
            this.dgv取引先マスタ.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv取引先マスタ.Location = new System.Drawing.Point(7, 126);
            this.dgv取引先マスタ.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dgv取引先マスタ.Name = "dgv取引先マスタ";
            this.dgv取引先マスタ.ReadOnly = true;
            this.dgv取引先マスタ.RowTemplate.Height = 21;
            this.dgv取引先マスタ.Size = new System.Drawing.Size(1160, 481);
            this.dgv取引先マスタ.TabIndex = 1;
            // 
            // grpBx検索
            // 
            this.grpBx検索.Controls.Add(this.txtBx名称);
            this.grpBx検索.Controls.Add(this.lb名称);
            this.grpBx検索.Controls.Add(this.lbコード);
            this.grpBx検索.Controls.Add(this.txtBxコード);
            this.grpBx検索.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx検索.Location = new System.Drawing.Point(12, 17);
            this.grpBx検索.Name = "grpBx検索";
            this.grpBx検索.Size = new System.Drawing.Size(370, 101);
            this.grpBx検索.TabIndex = 2;
            this.grpBx検索.TabStop = false;
            this.grpBx検索.Text = "< 検索 >";
            // 
            // txtBx名称
            // 
            this.txtBx名称.Location = new System.Drawing.Point(99, 64);
            this.txtBx名称.Name = "txtBx名称";
            this.txtBx名称.Size = new System.Drawing.Size(247, 23);
            this.txtBx名称.TabIndex = 1;
            // 
            // lb名称
            // 
            this.lb名称.AutoSize = true;
            this.lb名称.Location = new System.Drawing.Point(21, 67);
            this.lb名称.Name = "lb名称";
            this.lb名称.Size = new System.Drawing.Size(72, 15);
            this.lb名称.TabIndex = 17;
            this.lb名称.Text = "取引先名称:";
            // 
            // lbコード
            // 
            this.lbコード.AutoSize = true;
            this.lbコード.Location = new System.Drawing.Point(28, 27);
            this.lbコード.Name = "lbコード";
            this.lbコード.Size = new System.Drawing.Size(65, 15);
            this.lbコード.TabIndex = 16;
            this.lbコード.Text = "取引先CD:";
            // 
            // txtBxコード
            // 
            this.txtBxコード.Location = new System.Drawing.Point(99, 23);
            this.txtBxコード.Name = "txtBxコード";
            this.txtBxコード.Size = new System.Drawing.Size(107, 23);
            this.txtBxコード.TabIndex = 0;
            // 
            // 仕入先マスタ照会Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 624);
            this.Controls.Add(this.grpBx検索);
            this.Controls.Add(this.dgv取引先マスタ);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "仕入先マスタ照会Form";
            this.Text = "仕入先マスタ照会Form";
            ((System.ComponentModel.ISupportInitialize)(this.dgv取引先マスタ)).EndInit();
            this.grpBx検索.ResumeLayout(false);
            this.grpBx検索.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv取引先マスタ;
        private System.Windows.Forms.GroupBox grpBx検索;
        private System.Windows.Forms.TextBox txtBx名称;
        private System.Windows.Forms.Label lb名称;
        private System.Windows.Forms.Label lbコード;
        private System.Windows.Forms.TextBox txtBxコード;
    }
}