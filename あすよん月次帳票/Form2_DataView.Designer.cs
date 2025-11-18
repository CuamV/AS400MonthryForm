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
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowDrop = true;
            this.dgvData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(12, 177);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 21;
            this.dgvData.Size = new System.Drawing.Size(1162, 529);
            this.dgvData.TabIndex = 63;
            // 
            // Form2_DataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 718);
            this.Controls.Add(this.dgvData);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form2_DataView";
            this.Text = "Form2_DataView";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

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
    }
}