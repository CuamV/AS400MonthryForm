namespace あすよん月次帳票
{
    partial class 取引先マスタFm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(取引先マスタFm));
            this.btnメニュー = new System.Windows.Forms.GroupBox();
            this.btnインポート = new System.Windows.Forms.Button();
            this.btn照会 = new System.Windows.Forms.Button();
            this.btnダウンロード = new System.Windows.Forms.Button();
            this.btn戻る = new System.Windows.Forms.Button();
            this.lb部門 = new System.Windows.Forms.Label();
            this.btn削除 = new System.Windows.Forms.Button();
            this.btn登録 = new System.Windows.Forms.Button();
            this.txtBx取引先名 = new System.Windows.Forms.TextBox();
            this.txtBx取引先正式名 = new System.Windows.Forms.TextBox();
            this.txtBx取引先CD = new System.Windows.Forms.TextBox();
            this.lb取引先名 = new System.Windows.Forms.Label();
            this.lb取引先正式名 = new System.Windows.Forms.Label();
            this.lb取引先CD = new System.Windows.Forms.Label();
            this.lb取引先略名 = new System.Windows.Forms.Label();
            this.txtBx取引先略名 = new System.Windows.Forms.TextBox();
            this.cmbBx部門 = new System.Windows.Forms.ComboBox();
            this.lb取引先名カナ = new System.Windows.Forms.Label();
            this.txtBx取引先名カナ = new System.Windows.Forms.TextBox();
            this.lb取引先略名カナ = new System.Windows.Forms.Label();
            this.txtBx取引先略名カナ = new System.Windows.Forms.TextBox();
            this.lb郵便番号 = new System.Windows.Forms.Label();
            this.txtBx郵便番号 = new System.Windows.Forms.TextBox();
            this.lb電話番号1 = new System.Windows.Forms.Label();
            this.txtBx電話番号1 = new System.Windows.Forms.TextBox();
            this.lb住所1 = new System.Windows.Forms.Label();
            this.txtBx住所1 = new System.Windows.Forms.TextBox();
            this.lbFAX番号1 = new System.Windows.Forms.Label();
            this.txtBxFAX番号1 = new System.Windows.Forms.TextBox();
            this.lb住所2 = new System.Windows.Forms.Label();
            this.txtBx住所2 = new System.Windows.Forms.TextBox();
            this.lb住所1カナ = new System.Windows.Forms.Label();
            this.txtBx住所1カナ = new System.Windows.Forms.TextBox();
            this.lb住所2カナ = new System.Windows.Forms.Label();
            this.txtBx住所2カナ = new System.Windows.Forms.TextBox();
            this.lb電話番号2 = new System.Windows.Forms.Label();
            this.txtBx電話番号2 = new System.Windows.Forms.TextBox();
            this.lbFAX番号2 = new System.Windows.Forms.Label();
            this.txtBxFAX番号2 = new System.Windows.Forms.TextBox();
            this.txtBx備考 = new System.Windows.Forms.TextBox();
            this.lb備考 = new System.Windows.Forms.Label();
            this.chkListBx取引先ロール = new System.Windows.Forms.CheckedListBox();
            this.lb取引先ロール = new System.Windows.Forms.Label();
            this.btnメニュー.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnメニュー
            // 
            this.btnメニュー.Controls.Add(this.btnインポート);
            this.btnメニュー.Controls.Add(this.btn照会);
            this.btnメニュー.Controls.Add(this.btnダウンロード);
            this.btnメニュー.Controls.Add(this.btn戻る);
            this.btnメニュー.Location = new System.Drawing.Point(289, 13);
            this.btnメニュー.Margin = new System.Windows.Forms.Padding(4);
            this.btnメニュー.Name = "btnメニュー";
            this.btnメニュー.Padding = new System.Windows.Forms.Padding(4);
            this.btnメニュー.Size = new System.Drawing.Size(464, 59);
            this.btnメニュー.TabIndex = 0;
            this.btnメニュー.TabStop = false;
            // 
            // btnインポート
            // 
            this.btnインポート.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnインポート.Location = new System.Drawing.Point(248, 19);
            this.btnインポート.Margin = new System.Windows.Forms.Padding(5);
            this.btnインポート.Name = "btnインポート";
            this.btnインポート.Size = new System.Drawing.Size(90, 29);
            this.btnインポート.TabIndex = 2;
            this.btnインポート.Text = "インポート";
            this.btnインポート.UseVisualStyleBackColor = true;
            this.btnインポート.Click += new System.EventHandler(this.btnインポート_Click);
            // 
            // btn照会
            // 
            this.btn照会.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn照会.Location = new System.Drawing.Point(11, 19);
            this.btn照会.Margin = new System.Windows.Forms.Padding(5);
            this.btn照会.Name = "btn照会";
            this.btn照会.Size = new System.Drawing.Size(90, 29);
            this.btn照会.TabIndex = 0;
            this.btn照会.Text = "照会";
            this.btn照会.UseVisualStyleBackColor = true;
            this.btn照会.Click += new System.EventHandler(this.btn照会_Click);
            // 
            // btnダウンロード
            // 
            this.btnダウンロード.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnダウンロード.Location = new System.Drawing.Point(129, 19);
            this.btnダウンロード.Margin = new System.Windows.Forms.Padding(5);
            this.btnダウンロード.Name = "btnダウンロード";
            this.btnダウンロード.Size = new System.Drawing.Size(90, 29);
            this.btnダウンロード.TabIndex = 1;
            this.btnダウンロード.Text = "ダウンロード";
            this.btnダウンロード.UseVisualStyleBackColor = true;
            this.btnダウンロード.Click += new System.EventHandler(this.btnダウンロード_Click);
            // 
            // btn戻る
            // 
            this.btn戻る.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn戻る.Location = new System.Drawing.Point(366, 19);
            this.btn戻る.Margin = new System.Windows.Forms.Padding(5);
            this.btn戻る.Name = "btn戻る";
            this.btn戻る.Size = new System.Drawing.Size(90, 29);
            this.btn戻る.TabIndex = 3;
            this.btn戻る.Text = "戻る";
            this.btn戻る.UseVisualStyleBackColor = true;
            this.btn戻る.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // lb部門
            // 
            this.lb部門.AutoSize = true;
            this.lb部門.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb部門.Location = new System.Drawing.Point(48, 111);
            this.lb部門.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb部門.Name = "lb部門";
            this.lb部門.Size = new System.Drawing.Size(47, 17);
            this.lb部門.TabIndex = 25;
            this.lb部門.Text = "部門：";
            // 
            // btn削除
            // 
            this.btn削除.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn削除.Location = new System.Drawing.Point(543, 539);
            this.btn削除.Margin = new System.Windows.Forms.Padding(5);
            this.btn削除.Name = "btn削除";
            this.btn削除.Size = new System.Drawing.Size(80, 30);
            this.btn削除.TabIndex = 17;
            this.btn削除.Text = "削除";
            this.btn削除.UseVisualStyleBackColor = true;
            // 
            // btn登録
            // 
            this.btn登録.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn登録.Location = new System.Drawing.Point(415, 539);
            this.btn登録.Margin = new System.Windows.Forms.Padding(5);
            this.btn登録.Name = "btn登録";
            this.btn登録.Size = new System.Drawing.Size(80, 30);
            this.btn登録.TabIndex = 16;
            this.btn登録.Text = "登録";
            this.btn登録.UseVisualStyleBackColor = true;
            this.btn登録.Click += new System.EventHandler(this.btn登録_Click);
            // 
            // txtBx取引先名
            // 
            this.txtBx取引先名.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx取引先名.Location = new System.Drawing.Point(365, 151);
            this.txtBx取引先名.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx取引先名.MaxLength = 30;
            this.txtBx取引先名.Name = "txtBx取引先名";
            this.txtBx取引先名.Size = new System.Drawing.Size(210, 25);
            this.txtBx取引先名.TabIndex = 3;
            // 
            // txtBx取引先正式名
            // 
            this.txtBx取引先正式名.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx取引先正式名.Location = new System.Drawing.Point(719, 101);
            this.txtBx取引先正式名.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx取引先正式名.MaxLength = 50;
            this.txtBx取引先正式名.Name = "txtBx取引先正式名";
            this.txtBx取引先正式名.Size = new System.Drawing.Size(249, 25);
            this.txtBx取引先正式名.TabIndex = 2;
            // 
            // txtBx取引先CD
            // 
            this.txtBx取引先CD.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx取引先CD.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtBx取引先CD.Location = new System.Drawing.Point(365, 101);
            this.txtBx取引先CD.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx取引先CD.MaxLength = 7;
            this.txtBx取引先CD.Name = "txtBx取引先CD";
            this.txtBx取引先CD.Size = new System.Drawing.Size(104, 25);
            this.txtBx取引先CD.TabIndex = 1;
            this.txtBx取引先CD.TextChanged += new System.EventHandler(this.txtBx取引先CD_TextChanged);
            // 
            // lb取引先名
            // 
            this.lb取引先名.AutoSize = true;
            this.lb取引先名.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb取引先名.Location = new System.Drawing.Point(292, 155);
            this.lb取引先名.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb取引先名.Name = "lb取引先名";
            this.lb取引先名.Size = new System.Drawing.Size(73, 17);
            this.lb取引先名.TabIndex = 20;
            this.lb取引先名.Text = "取引先名：";
            // 
            // lb取引先正式名
            // 
            this.lb取引先正式名.AutoSize = true;
            this.lb取引先正式名.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb取引先正式名.Location = new System.Drawing.Point(621, 107);
            this.lb取引先正式名.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb取引先正式名.Name = "lb取引先正式名";
            this.lb取引先正式名.Size = new System.Drawing.Size(99, 17);
            this.lb取引先正式名.TabIndex = 19;
            this.lb取引先正式名.Text = "取引先正式名：";
            // 
            // lb取引先CD
            // 
            this.lb取引先CD.AutoSize = true;
            this.lb取引先CD.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb取引先CD.Location = new System.Drawing.Point(286, 107);
            this.lb取引先CD.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb取引先CD.Name = "lb取引先CD";
            this.lb取引先CD.Size = new System.Drawing.Size(79, 17);
            this.lb取引先CD.TabIndex = 18;
            this.lb取引先CD.Text = "取引先CD：";
            // 
            // lb取引先略名
            // 
            this.lb取引先略名.AutoSize = true;
            this.lb取引先略名.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb取引先略名.Location = new System.Drawing.Point(279, 205);
            this.lb取引先略名.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb取引先略名.Name = "lb取引先略名";
            this.lb取引先略名.Size = new System.Drawing.Size(86, 17);
            this.lb取引先略名.TabIndex = 22;
            this.lb取引先略名.Text = "取引先略名：";
            // 
            // txtBx取引先略名
            // 
            this.txtBx取引先略名.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx取引先略名.Location = new System.Drawing.Point(365, 201);
            this.txtBx取引先略名.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx取引先略名.MaxLength = 20;
            this.txtBx取引先略名.Name = "txtBx取引先略名";
            this.txtBx取引先略名.Size = new System.Drawing.Size(193, 25);
            this.txtBx取引先略名.TabIndex = 5;
            // 
            // cmbBx部門
            // 
            this.cmbBx部門.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbBx部門.FormattingEnabled = true;
            this.cmbBx部門.Items.AddRange(new object[] {
            "オーノ",
            "サンミックダスコン",
            "サンミックカーペット"});
            this.cmbBx部門.Location = new System.Drawing.Point(97, 107);
            this.cmbBx部門.Margin = new System.Windows.Forms.Padding(5);
            this.cmbBx部門.Name = "cmbBx部門";
            this.cmbBx部門.Size = new System.Drawing.Size(120, 26);
            this.cmbBx部門.TabIndex = 8;
            this.cmbBx部門.SelectionChangeCommitted += new System.EventHandler(this.txtBx取引先CD_TextChanged);
            // 
            // lb取引先名カナ
            // 
            this.lb取引先名カナ.AutoSize = true;
            this.lb取引先名カナ.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb取引先名カナ.Location = new System.Drawing.Point(628, 156);
            this.lb取引先名カナ.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb取引先名カナ.Name = "lb取引先名カナ";
            this.lb取引先名カナ.Size = new System.Drawing.Size(92, 17);
            this.lb取引先名カナ.TabIndex = 21;
            this.lb取引先名カナ.Text = "取引先名カナ：";
            // 
            // txtBx取引先名カナ
            // 
            this.txtBx取引先名カナ.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx取引先名カナ.Location = new System.Drawing.Point(719, 151);
            this.txtBx取引先名カナ.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx取引先名カナ.MaxLength = 40;
            this.txtBx取引先名カナ.Name = "txtBx取引先名カナ";
            this.txtBx取引先名カナ.Size = new System.Drawing.Size(249, 25);
            this.txtBx取引先名カナ.TabIndex = 4;
            // 
            // lb取引先略名カナ
            // 
            this.lb取引先略名カナ.AutoSize = true;
            this.lb取引先略名カナ.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb取引先略名カナ.Location = new System.Drawing.Point(615, 207);
            this.lb取引先略名カナ.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb取引先略名カナ.Name = "lb取引先略名カナ";
            this.lb取引先略名カナ.Size = new System.Drawing.Size(105, 17);
            this.lb取引先略名カナ.TabIndex = 23;
            this.lb取引先略名カナ.Text = "取引先略名カナ：";
            // 
            // txtBx取引先略名カナ
            // 
            this.txtBx取引先略名カナ.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx取引先略名カナ.Location = new System.Drawing.Point(719, 201);
            this.txtBx取引先略名カナ.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx取引先略名カナ.MaxLength = 30;
            this.txtBx取引先略名カナ.Name = "txtBx取引先略名カナ";
            this.txtBx取引先略名カナ.Size = new System.Drawing.Size(228, 25);
            this.txtBx取引先略名カナ.TabIndex = 6;
            // 
            // lb郵便番号
            // 
            this.lb郵便番号.AutoSize = true;
            this.lb郵便番号.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb郵便番号.Location = new System.Drawing.Point(52, 395);
            this.lb郵便番号.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb郵便番号.Name = "lb郵便番号";
            this.lb郵便番号.Size = new System.Drawing.Size(73, 17);
            this.lb郵便番号.TabIndex = 26;
            this.lb郵便番号.Text = "郵便番号：";
            // 
            // txtBx郵便番号
            // 
            this.txtBx郵便番号.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx郵便番号.Location = new System.Drawing.Point(127, 391);
            this.txtBx郵便番号.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx郵便番号.MaxLength = 8;
            this.txtBx郵便番号.Name = "txtBx郵便番号";
            this.txtBx郵便番号.Size = new System.Drawing.Size(108, 25);
            this.txtBx郵便番号.TabIndex = 9;
            this.txtBx郵便番号.TextChanged += new System.EventHandler(this.txtBx郵便番号_TextChanged);
            // 
            // lb電話番号1
            // 
            this.lb電話番号1.AutoSize = true;
            this.lb電話番号1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb電話番号1.Location = new System.Drawing.Point(284, 264);
            this.lb電話番号1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb電話番号1.Name = "lb電話番号1";
            this.lb電話番号1.Size = new System.Drawing.Size(81, 17);
            this.lb電話番号1.TabIndex = 31;
            this.lb電話番号1.Text = "電話番号1：";
            // 
            // txtBx電話番号1
            // 
            this.txtBx電話番号1.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx電話番号1.Location = new System.Drawing.Point(366, 259);
            this.txtBx電話番号1.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx電話番号1.MaxLength = 15;
            this.txtBx電話番号1.Name = "txtBx電話番号1";
            this.txtBx電話番号1.Size = new System.Drawing.Size(119, 25);
            this.txtBx電話番号1.TabIndex = 14;
            // 
            // lb住所1
            // 
            this.lb住所1.AutoSize = true;
            this.lb住所1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb住所1.Location = new System.Drawing.Point(70, 445);
            this.lb住所1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb住所1.Name = "lb住所1";
            this.lb住所1.Size = new System.Drawing.Size(55, 17);
            this.lb住所1.TabIndex = 27;
            this.lb住所1.Text = "住所1：";
            // 
            // txtBx住所1
            // 
            this.txtBx住所1.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx住所1.Location = new System.Drawing.Point(127, 440);
            this.txtBx住所1.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx住所1.MaxLength = 75;
            this.txtBx住所1.Name = "txtBx住所1";
            this.txtBx住所1.Size = new System.Drawing.Size(287, 25);
            this.txtBx住所1.TabIndex = 10;
            // 
            // lbFAX番号1
            // 
            this.lbFAX番号1.AutoSize = true;
            this.lbFAX番号1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbFAX番号1.Location = new System.Drawing.Point(641, 264);
            this.lbFAX番号1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbFAX番号1.Name = "lbFAX番号1";
            this.lbFAX番号1.Size = new System.Drawing.Size(79, 17);
            this.lbFAX番号1.TabIndex = 32;
            this.lbFAX番号1.Text = "FAX番号1：";
            // 
            // txtBxFAX番号1
            // 
            this.txtBxFAX番号1.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxFAX番号1.Location = new System.Drawing.Point(719, 259);
            this.txtBxFAX番号1.Margin = new System.Windows.Forms.Padding(5);
            this.txtBxFAX番号1.MaxLength = 15;
            this.txtBxFAX番号1.Name = "txtBxFAX番号1";
            this.txtBxFAX番号1.Size = new System.Drawing.Size(120, 25);
            this.txtBxFAX番号1.TabIndex = 15;
            // 
            // lb住所2
            // 
            this.lb住所2.AutoSize = true;
            this.lb住所2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb住所2.Location = new System.Drawing.Point(70, 490);
            this.lb住所2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb住所2.Name = "lb住所2";
            this.lb住所2.Size = new System.Drawing.Size(55, 17);
            this.lb住所2.TabIndex = 28;
            this.lb住所2.Text = "住所2：";
            // 
            // txtBx住所2
            // 
            this.txtBx住所2.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx住所2.Location = new System.Drawing.Point(127, 484);
            this.txtBx住所2.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx住所2.MaxLength = 40;
            this.txtBx住所2.Name = "txtBx住所2";
            this.txtBx住所2.Size = new System.Drawing.Size(287, 25);
            this.txtBx住所2.TabIndex = 11;
            // 
            // lb住所1カナ
            // 
            this.lb住所1カナ.AutoSize = true;
            this.lb住所1カナ.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb住所1カナ.Location = new System.Drawing.Point(513, 442);
            this.lb住所1カナ.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb住所1カナ.Name = "lb住所1カナ";
            this.lb住所1カナ.Size = new System.Drawing.Size(74, 17);
            this.lb住所1カナ.TabIndex = 29;
            this.lb住所1カナ.Text = "住所1カナ：";
            // 
            // txtBx住所1カナ
            // 
            this.txtBx住所1カナ.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx住所1カナ.Location = new System.Drawing.Point(589, 437);
            this.txtBx住所1カナ.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx住所1カナ.MaxLength = 145;
            this.txtBx住所1カナ.Name = "txtBx住所1カナ";
            this.txtBx住所1カナ.Size = new System.Drawing.Size(343, 25);
            this.txtBx住所1カナ.TabIndex = 12;
            // 
            // lb住所2カナ
            // 
            this.lb住所2カナ.AutoSize = true;
            this.lb住所2カナ.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb住所2カナ.Location = new System.Drawing.Point(513, 488);
            this.lb住所2カナ.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb住所2カナ.Name = "lb住所2カナ";
            this.lb住所2カナ.Size = new System.Drawing.Size(74, 17);
            this.lb住所2カナ.TabIndex = 30;
            this.lb住所2カナ.Text = "住所2カナ：";
            // 
            // txtBx住所2カナ
            // 
            this.txtBx住所2カナ.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx住所2カナ.Location = new System.Drawing.Point(589, 484);
            this.txtBx住所2カナ.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx住所2カナ.MaxLength = 80;
            this.txtBx住所2カナ.Name = "txtBx住所2カナ";
            this.txtBx住所2カナ.Size = new System.Drawing.Size(343, 25);
            this.txtBx住所2カナ.TabIndex = 13;
            // 
            // lb電話番号2
            // 
            this.lb電話番号2.AutoSize = true;
            this.lb電話番号2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb電話番号2.Location = new System.Drawing.Point(286, 310);
            this.lb電話番号2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb電話番号2.Name = "lb電話番号2";
            this.lb電話番号2.Size = new System.Drawing.Size(81, 17);
            this.lb電話番号2.TabIndex = 34;
            this.lb電話番号2.Text = "電話番号2：";
            // 
            // txtBx電話番号2
            // 
            this.txtBx電話番号2.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx電話番号2.Location = new System.Drawing.Point(365, 306);
            this.txtBx電話番号2.Margin = new System.Windows.Forms.Padding(5);
            this.txtBx電話番号2.MaxLength = 15;
            this.txtBx電話番号2.Name = "txtBx電話番号2";
            this.txtBx電話番号2.Size = new System.Drawing.Size(120, 25);
            this.txtBx電話番号2.TabIndex = 33;
            // 
            // lbFAX番号2
            // 
            this.lbFAX番号2.AutoSize = true;
            this.lbFAX番号2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbFAX番号2.Location = new System.Drawing.Point(641, 311);
            this.lbFAX番号2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lbFAX番号2.Name = "lbFAX番号2";
            this.lbFAX番号2.Size = new System.Drawing.Size(79, 17);
            this.lbFAX番号2.TabIndex = 36;
            this.lbFAX番号2.Text = "FAX番号2：";
            // 
            // txtBxFAX番号2
            // 
            this.txtBxFAX番号2.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxFAX番号2.Location = new System.Drawing.Point(719, 307);
            this.txtBxFAX番号2.Margin = new System.Windows.Forms.Padding(5);
            this.txtBxFAX番号2.MaxLength = 15;
            this.txtBxFAX番号2.Name = "txtBxFAX番号2";
            this.txtBxFAX番号2.Size = new System.Drawing.Size(120, 25);
            this.txtBxFAX番号2.TabIndex = 35;
            // 
            // txtBx備考
            // 
            this.txtBx備考.Location = new System.Drawing.Point(589, 388);
            this.txtBx備考.MaxLength = 46;
            this.txtBx備考.Name = "txtBx備考";
            this.txtBx備考.Size = new System.Drawing.Size(342, 24);
            this.txtBx備考.TabIndex = 37;
            // 
            // lb備考
            // 
            this.lb備考.AutoSize = true;
            this.lb備考.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb備考.Location = new System.Drawing.Point(539, 394);
            this.lb備考.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lb備考.Name = "lb備考";
            this.lb備考.Size = new System.Drawing.Size(47, 17);
            this.lb備考.TabIndex = 38;
            this.lb備考.Text = "備考：";
            // 
            // chkListBx取引先ロール
            // 
            this.chkListBx取引先ロール.CheckOnClick = true;
            this.chkListBx取引先ロール.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkListBx取引先ロール.FormattingEnabled = true;
            this.chkListBx取引先ロール.Items.AddRange(new object[] {
            "商社",
            "仕入先",
            "販売先",
            "得意先",
            "出荷先",
            "預り先",
            "運送便",
            "倉庫"});
            this.chkListBx取引先ロール.Location = new System.Drawing.Point(97, 189);
            this.chkListBx取引先ロール.Name = "chkListBx取引先ロール";
            this.chkListBx取引先ロール.Size = new System.Drawing.Size(104, 156);
            this.chkListBx取引先ロール.TabIndex = 39;
            // 
            // lb取引先ロール
            // 
            this.lb取引先ロール.AutoSize = true;
            this.lb取引先ロール.Location = new System.Drawing.Point(48, 169);
            this.lb取引先ロール.Name = "lb取引先ロール";
            this.lb取引先ロール.Size = new System.Drawing.Size(91, 17);
            this.lb取引先ロール.TabIndex = 40;
            this.lb取引先ロール.Text = "取引先ロール：";
            // 
            // 取引先マスタFm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1034, 602);
            this.Controls.Add(this.lb取引先ロール);
            this.Controls.Add(this.chkListBx取引先ロール);
            this.Controls.Add(this.lb備考);
            this.Controls.Add(this.txtBx備考);
            this.Controls.Add(this.lbFAX番号2);
            this.Controls.Add(this.txtBxFAX番号2);
            this.Controls.Add(this.lb電話番号2);
            this.Controls.Add(this.txtBx電話番号2);
            this.Controls.Add(this.lb住所2カナ);
            this.Controls.Add(this.txtBx住所2カナ);
            this.Controls.Add(this.lb住所1カナ);
            this.Controls.Add(this.txtBx住所1カナ);
            this.Controls.Add(this.lb住所2);
            this.Controls.Add(this.txtBx住所2);
            this.Controls.Add(this.lbFAX番号1);
            this.Controls.Add(this.txtBxFAX番号1);
            this.Controls.Add(this.lb住所1);
            this.Controls.Add(this.txtBx住所1);
            this.Controls.Add(this.lb電話番号1);
            this.Controls.Add(this.txtBx電話番号1);
            this.Controls.Add(this.lb郵便番号);
            this.Controls.Add(this.txtBx郵便番号);
            this.Controls.Add(this.lb取引先略名カナ);
            this.Controls.Add(this.txtBx取引先略名カナ);
            this.Controls.Add(this.lb取引先名カナ);
            this.Controls.Add(this.txtBx取引先名カナ);
            this.Controls.Add(this.cmbBx部門);
            this.Controls.Add(this.lb取引先略名);
            this.Controls.Add(this.txtBx取引先略名);
            this.Controls.Add(this.lb取引先CD);
            this.Controls.Add(this.lb取引先正式名);
            this.Controls.Add(this.lb取引先名);
            this.Controls.Add(this.txtBx取引先CD);
            this.Controls.Add(this.txtBx取引先正式名);
            this.Controls.Add(this.txtBx取引先名);
            this.Controls.Add(this.btnメニュー);
            this.Controls.Add(this.lb部門);
            this.Controls.Add(this.btn削除);
            this.Controls.Add(this.btn登録);
            this.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "取引先マスタFm";
            this.Text = "取引先マスタForm";
            this.btnメニュー.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox btnメニュー;
        private System.Windows.Forms.Button btn照会;
        private System.Windows.Forms.Button btnダウンロード;
        private System.Windows.Forms.Button btn戻る;
        private System.Windows.Forms.Label lb部門;
        private System.Windows.Forms.Button btn削除;
        private System.Windows.Forms.Button btn登録;
        private System.Windows.Forms.Button btnインポート;
        private System.Windows.Forms.TextBox txtBx取引先名;
        private System.Windows.Forms.TextBox txtBx取引先正式名;
        private System.Windows.Forms.TextBox txtBx取引先CD;
        private System.Windows.Forms.Label lb取引先名;
        private System.Windows.Forms.Label lb取引先正式名;
        private System.Windows.Forms.Label lb取引先CD;
        private System.Windows.Forms.Label lb取引先略名;
        private System.Windows.Forms.TextBox txtBx取引先略名;
        private System.Windows.Forms.ComboBox cmbBx部門;
        private System.Windows.Forms.Label lb取引先名カナ;
        private System.Windows.Forms.TextBox txtBx取引先名カナ;
        private System.Windows.Forms.Label lb取引先略名カナ;
        private System.Windows.Forms.TextBox txtBx取引先略名カナ;
        private System.Windows.Forms.Label lb郵便番号;
        private System.Windows.Forms.TextBox txtBx郵便番号;
        private System.Windows.Forms.Label lb電話番号1;
        private System.Windows.Forms.TextBox txtBx電話番号1;
        private System.Windows.Forms.Label lb住所1;
        private System.Windows.Forms.TextBox txtBx住所1;
        private System.Windows.Forms.Label lbFAX番号1;
        private System.Windows.Forms.TextBox txtBxFAX番号1;
        private System.Windows.Forms.Label lb住所2;
        private System.Windows.Forms.TextBox txtBx住所2;
        private System.Windows.Forms.Label lb住所1カナ;
        private System.Windows.Forms.TextBox txtBx住所1カナ;
        private System.Windows.Forms.Label lb住所2カナ;
        private System.Windows.Forms.TextBox txtBx住所2カナ;
        private System.Windows.Forms.Label lb電話番号2;
        private System.Windows.Forms.TextBox txtBx電話番号2;
        private System.Windows.Forms.Label lbFAX番号2;
        private System.Windows.Forms.TextBox txtBxFAX番号2;
        private System.Windows.Forms.TextBox txtBx備考;
        private System.Windows.Forms.Label lb備考;
        private System.Windows.Forms.CheckedListBox chkListBx取引先ロール;
        private System.Windows.Forms.Label lb取引先ロール;
    }
}