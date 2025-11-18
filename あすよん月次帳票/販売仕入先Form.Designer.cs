namespace あすよん月次帳票
{
    partial class 販売仕入先Form
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btn削除 = new System.Windows.Forms.Button();
            this.btn追加 = new System.Windows.Forms.Button();
            this.listBx販売仕入 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView販売仕入 = new System.Windows.Forms.TreeView();
            this.grpBx検索 = new System.Windows.Forms.GroupBox();
            this.btn検索 = new System.Windows.Forms.Button();
            this.txtBx名称 = new System.Windows.Forms.TextBox();
            this.lb名称 = new System.Windows.Forms.Label();
            this.lbコード = new System.Windows.Forms.Label();
            this.txtBxコード = new System.Windows.Forms.TextBox();
            this.grpBx検索.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.Location = new System.Drawing.Point(495, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 38);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOK.Location = new System.Drawing.Point(495, 37);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 38);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btn削除
            // 
            this.btn削除.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn削除.Location = new System.Drawing.Point(343, 145);
            this.btn削除.Name = "btn削除";
            this.btn削除.Size = new System.Drawing.Size(67, 29);
            this.btn削除.TabIndex = 11;
            this.btn削除.Text = "削除";
            this.btn削除.UseVisualStyleBackColor = true;
            this.btn削除.Click += new System.EventHandler(this.btn削除_Click);
            // 
            // btn追加
            // 
            this.btn追加.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn追加.Location = new System.Drawing.Point(256, 145);
            this.btn追加.Name = "btn追加";
            this.btn追加.Size = new System.Drawing.Size(67, 29);
            this.btn追加.TabIndex = 10;
            this.btn追加.Text = "追加";
            this.btn追加.UseVisualStyleBackColor = true;
            this.btn追加.Click += new System.EventHandler(this.btn追加_Click);
            // 
            // listBx販売仕入
            // 
            this.listBx販売仕入.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBx販売仕入.FormattingEnabled = true;
            this.listBx販売仕入.ItemHeight = 15;
            this.listBx販売仕入.Location = new System.Drawing.Point(363, 180);
            this.listBx販売仕入.Name = "listBx販売仕入";
            this.listBx販売仕入.Size = new System.Drawing.Size(275, 259);
            this.listBx販売仕入.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(318, 301);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "→";
            // 
            // treeView販売仕入
            // 
            this.treeView販売仕入.CheckBoxes = true;
            this.treeView販売仕入.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.treeView販売仕入.Location = new System.Drawing.Point(16, 180);
            this.treeView販売仕入.Name = "treeView販売仕入";
            this.treeView販売仕入.Size = new System.Drawing.Size(285, 259);
            this.treeView販売仕入.TabIndex = 7;
            this.treeView販売仕入.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView販売仕入_AfterCheck);
            // 
            // grpBx検索
            // 
            this.grpBx検索.Controls.Add(this.btn検索);
            this.grpBx検索.Controls.Add(this.txtBx名称);
            this.grpBx検索.Controls.Add(this.lb名称);
            this.grpBx検索.Controls.Add(this.lbコード);
            this.grpBx検索.Controls.Add(this.txtBxコード);
            this.grpBx検索.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx検索.Location = new System.Drawing.Point(91, 27);
            this.grpBx検索.Name = "grpBx検索";
            this.grpBx検索.Size = new System.Drawing.Size(370, 101);
            this.grpBx検索.TabIndex = 14;
            this.grpBx検索.TabStop = false;
            this.grpBx検索.Text = "< 検索 >";
            // 
            // btn検索
            // 
            this.btn検索.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn検索.Location = new System.Drawing.Point(260, 19);
            this.btn検索.Name = "btn検索";
            this.btn検索.Size = new System.Drawing.Size(67, 29);
            this.btn検索.TabIndex = 19;
            this.btn検索.Text = "検索";
            this.btn検索.UseVisualStyleBackColor = true;
            this.btn検索.Click += new System.EventHandler(this.btn検索_Click);
            // 
            // txtBx名称
            // 
            this.txtBx名称.Location = new System.Drawing.Point(99, 64);
            this.txtBx名称.Name = "txtBx名称";
            this.txtBx名称.Size = new System.Drawing.Size(247, 23);
            this.txtBx名称.TabIndex = 18;
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
            this.txtBxコード.TabIndex = 15;
            // 
            // 販売仕入先Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 453);
            this.Controls.Add(this.grpBx検索);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btn削除);
            this.Controls.Add(this.btn追加);
            this.Controls.Add(this.listBx販売仕入);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView販売仕入);
            this.Name = "販売仕入先Form";
            this.Text = "販売仕入先Form";
            this.grpBx検索.ResumeLayout(false);
            this.grpBx検索.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btn削除;
        private System.Windows.Forms.Button btn追加;
        private System.Windows.Forms.ListBox listBx販売仕入;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView販売仕入;
        private System.Windows.Forms.GroupBox grpBx検索;
        private System.Windows.Forms.Button btn検索;
        private System.Windows.Forms.TextBox txtBx名称;
        private System.Windows.Forms.Label lb名称;
        private System.Windows.Forms.Label lbコード;
        private System.Windows.Forms.TextBox txtBxコード;
    }
}