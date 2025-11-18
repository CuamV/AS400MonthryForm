namespace あすよん月次帳票
{
    partial class 部門Form
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
            this.treeView部門 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.listBx部門 = new System.Windows.Forms.ListBox();
            this.btn追加 = new System.Windows.Forms.Button();
            this.btn削除 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView部門
            // 
            this.treeView部門.CheckBoxes = true;
            this.treeView部門.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.treeView部門.Location = new System.Drawing.Point(26, 110);
            this.treeView部門.Name = "treeView部門";
            this.treeView部門.Size = new System.Drawing.Size(180, 229);
            this.treeView部門.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(226, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "→";
            // 
            // listBx部門
            // 
            this.listBx部門.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBx部門.FormattingEnabled = true;
            this.listBx部門.ItemHeight = 15;
            this.listBx部門.Location = new System.Drawing.Point(276, 110);
            this.listBx部門.Name = "listBx部門";
            this.listBx部門.Size = new System.Drawing.Size(180, 229);
            this.listBx部門.TabIndex = 2;
            // 
            // btn追加
            // 
            this.btn追加.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn追加.Location = new System.Drawing.Point(171, 75);
            this.btn追加.Name = "btn追加";
            this.btn追加.Size = new System.Drawing.Size(67, 29);
            this.btn追加.TabIndex = 3;
            this.btn追加.Text = "追加";
            this.btn追加.UseVisualStyleBackColor = true;
            this.btn追加.Click += new System.EventHandler(this.btn追加_Click);
            // 
            // btn削除
            // 
            this.btn削除.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn削除.Location = new System.Drawing.Point(249, 75);
            this.btn削除.Name = "btn削除";
            this.btn削除.Size = new System.Drawing.Size(67, 29);
            this.btn削除.TabIndex = 4;
            this.btn削除.Text = "削除";
            this.btn削除.UseVisualStyleBackColor = true;
            this.btn削除.Click += new System.EventHandler(this.btn削除_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOK.Location = new System.Drawing.Point(374, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 38);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.Location = new System.Drawing.Point(374, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // 部門Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 362);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btn削除);
            this.Controls.Add(this.btn追加);
            this.Controls.Add(this.listBx部門);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView部門);
            this.Name = "部門Form";
            this.Text = "部門Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView部門;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBx部門;
        private System.Windows.Forms.Button btn追加;
        private System.Windows.Forms.Button btn削除;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}