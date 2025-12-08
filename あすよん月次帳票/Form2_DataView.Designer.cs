using System.Drawing;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class Form2_DataView
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
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.lb条件 = new System.Windows.Forms.Label();
            this.txtBx条件 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowDrop = true;
            this.dgvData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(14, 181);
            this.dgvData.Margin = new System.Windows.Forms.Padding(4);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 21;
            this.dgvData.Size = new System.Drawing.Size(1441, 600);
            this.dgvData.TabIndex = 1;
            // 
            // lb条件
            // 
            this.lb条件.AutoSize = true;
            this.lb条件.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb条件.Location = new System.Drawing.Point(22, 7);
            this.lb条件.Name = "lb条件";
            this.lb条件.Size = new System.Drawing.Size(47, 17);
            this.lb条件.TabIndex = 64;
            this.lb条件.Text = "条件：";
            // 
            // txtBx条件
            // 
            this.txtBx条件.Location = new System.Drawing.Point(25, 27);
            this.txtBx条件.Multiline = true;
            this.txtBx条件.Name = "txtBx条件";
            this.txtBx条件.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBx条件.Size = new System.Drawing.Size(344, 143);
            this.txtBx条件.TabIndex = 0;
            // 
            // Form2_DataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1469, 788);
            this.Controls.Add(this.txtBx条件);
            this.Controls.Add(this.lb条件);
            this.Controls.Add(this.dgvData);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form2_DataView";
            this.Text = "Form2_DataView";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // DataGridView のスタイル
        private void StyleDataGrid(DataGridView dgv, Color header, Color row, Color altRow)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = header;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9.75F, FontStyle.Bold);

            dgv.RowsDefaultCellStyle.BackColor = row;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = altRow;
            dgv.RowsDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private System.Windows.Forms.DataGridView dgvData;
        private Label lb条件;
        private TextBox txtBx条件;
    }
}