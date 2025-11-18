using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class RplForm2
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
            this.lbTitleDisplay = new System.Windows.Forms.Label();
            this.lb抽出期間 = new System.Windows.Forms.Label();
            this.grpBx抽出期間 = new System.Windows.Forms.GroupBox();
            this.txtBxEndYearMonth = new System.Windows.Forms.TextBox();
            this.txtBxStrYearMonth = new System.Windows.Forms.TextBox();
            this.lbSymbol1 = new System.Windows.Forms.Label();
            this.grpBxBtn = new System.Windows.Forms.GroupBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.btnForm1Back = new System.Windows.Forms.Button();
            this.grpBx組織 = new System.Windows.Forms.GroupBox();
            this.linkLb部門 = new System.Windows.Forms.LinkLabel();
            this.listBx部門 = new System.Windows.Forms.ListBox();
            this.chkBxSundus = new System.Windows.Forms.CheckBox();
            this.chkBxOhno = new System.Windows.Forms.CheckBox();
            this.chkBxSuncar = new System.Windows.Forms.CheckBox();
            this.listBx仕入先 = new System.Windows.Forms.ListBox();
            this.listBx販売先 = new System.Windows.Forms.ListBox();
            this.grpBxクラス区分 = new System.Windows.Forms.GroupBox();
            this.chkBx加工 = new System.Windows.Forms.CheckBox();
            this.chkBx預り = new System.Windows.Forms.CheckBox();
            this.chkBx原材料 = new System.Windows.Forms.CheckBox();
            this.chkBx半製品 = new System.Windows.Forms.CheckBox();
            this.chkBx製品 = new System.Windows.Forms.CheckBox();
            this.chkBxIv = new System.Windows.Forms.CheckBox();
            this.chkBxPr = new System.Windows.Forms.CheckBox();
            this.chkBxSl = new System.Windows.Forms.CheckBox();
            this.grpBxデータ区分 = new System.Windows.Forms.GroupBox();
            this.btnMin = new System.Windows.Forms.Button();
            this.linkLb販売先 = new System.Windows.Forms.LinkLabel();
            this.linkLb仕入先 = new System.Windows.Forms.LinkLabel();
            this.grpBx取引先 = new System.Windows.Forms.GroupBox();
            this.rdBtnなし = new System.Windows.Forms.RadioButton();
            this.rdBtn取引先 = new System.Windows.Forms.RadioButton();
            this.rdBtn部門 = new System.Windows.Forms.RadioButton();
            this.grpBx集計区分 = new System.Windows.Forms.GroupBox();
            this.txtBx名称 = new System.Windows.Forms.TextBox();
            this.lb名称 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpBx抽出期間.SuspendLayout();
            this.grpBxBtn.SuspendLayout();
            this.grpBx組織.SuspendLayout();
            this.grpBxクラス区分.SuspendLayout();
            this.grpBxデータ区分.SuspendLayout();
            this.grpBx取引先.SuspendLayout();
            this.grpBx集計区分.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTitleDisplay
            // 
            this.lbTitleDisplay.AutoSize = true;
            this.lbTitleDisplay.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitleDisplay.Location = new System.Drawing.Point(43, 11);
            this.lbTitleDisplay.Name = "lbTitleDisplay";
            this.lbTitleDisplay.Size = new System.Drawing.Size(92, 17);
            this.lbTitleDisplay.TabIndex = 71;
            this.lbTitleDisplay.Text = "< データ抽出>";
            // 
            // lb抽出期間
            // 
            this.lb抽出期間.AutoSize = true;
            this.lb抽出期間.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb抽出期間.Location = new System.Drawing.Point(6, 57);
            this.lb抽出期間.Name = "lb抽出期間";
            this.lb抽出期間.Size = new System.Drawing.Size(267, 15);
            this.lb抽出期間.TabIndex = 75;
            this.lb抽出期間.Text = "●抽出期間はyyyymmの6桁年月で指定してください";
            // 
            // grpBx抽出期間
            // 
            this.grpBx抽出期間.Controls.Add(this.txtBxEndYearMonth);
            this.grpBx抽出期間.Controls.Add(this.txtBxStrYearMonth);
            this.grpBx抽出期間.Controls.Add(this.lbSymbol1);
            this.grpBx抽出期間.Controls.Add(this.lb抽出期間);
            this.grpBx抽出期間.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx抽出期間.Location = new System.Drawing.Point(106, 87);
            this.grpBx抽出期間.Name = "grpBx抽出期間";
            this.grpBx抽出期間.Size = new System.Drawing.Size(279, 81);
            this.grpBx抽出期間.TabIndex = 74;
            this.grpBx抽出期間.TabStop = false;
            this.grpBx抽出期間.Text = "【抽出期間】";
            // 
            // txtBxEndYearMonth
            // 
            this.txtBxEndYearMonth.Location = new System.Drawing.Point(162, 23);
            this.txtBxEndYearMonth.Name = "txtBxEndYearMonth";
            this.txtBxEndYearMonth.Size = new System.Drawing.Size(106, 24);
            this.txtBxEndYearMonth.TabIndex = 72;
            // 
            // txtBxStrYearMonth
            // 
            this.txtBxStrYearMonth.Location = new System.Drawing.Point(13, 23);
            this.txtBxStrYearMonth.Name = "txtBxStrYearMonth";
            this.txtBxStrYearMonth.Size = new System.Drawing.Size(106, 24);
            this.txtBxStrYearMonth.TabIndex = 71;
            // 
            // lbSymbol1
            // 
            this.lbSymbol1.AutoSize = true;
            this.lbSymbol1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSymbol1.Location = new System.Drawing.Point(128, 27);
            this.lbSymbol1.Name = "lbSymbol1";
            this.lbSymbol1.Size = new System.Drawing.Size(24, 19);
            this.lbSymbol1.TabIndex = 40;
            this.lbSymbol1.Text = "～";
            // 
            // grpBxBtn
            // 
            this.grpBxBtn.Controls.Add(this.btnExportExcel);
            this.grpBxBtn.Controls.Add(this.btnDisplay);
            this.grpBxBtn.Controls.Add(this.btnForm1Back);
            this.grpBxBtn.Location = new System.Drawing.Point(458, 77);
            this.grpBxBtn.Name = "grpBxBtn";
            this.grpBxBtn.Size = new System.Drawing.Size(394, 56);
            this.grpBxBtn.TabIndex = 73;
            this.grpBxBtn.TabStop = false;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnExportExcel.Location = new System.Drawing.Point(147, 13);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(103, 35);
            this.btnExportExcel.TabIndex = 79;
            this.btnExportExcel.Text = "Excelエクスポート";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnDisplay
            // 
            this.btnDisplay.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDisplay.Location = new System.Drawing.Point(18, 13);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(103, 35);
            this.btnDisplay.TabIndex = 63;
            this.btnDisplay.Text = "データ表示実行";
            this.btnDisplay.UseVisualStyleBackColor = true;
            // 
            // btnForm1Back
            // 
            this.btnForm1Back.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnForm1Back.Location = new System.Drawing.Point(274, 13);
            this.btnForm1Back.Name = "btnForm1Back";
            this.btnForm1Back.Size = new System.Drawing.Size(103, 35);
            this.btnForm1Back.TabIndex = 64;
            this.btnForm1Back.Text = "戻る";
            this.btnForm1Back.UseVisualStyleBackColor = true;
            this.btnForm1Back.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // grpBx組織
            // 
            this.grpBx組織.Controls.Add(this.linkLb部門);
            this.grpBx組織.Controls.Add(this.listBx部門);
            this.grpBx組織.Controls.Add(this.chkBxSundus);
            this.grpBx組織.Controls.Add(this.chkBxOhno);
            this.grpBx組織.Controls.Add(this.chkBxSuncar);
            this.grpBx組織.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx組織.Location = new System.Drawing.Point(47, 174);
            this.grpBx組織.Name = "grpBx組織";
            this.grpBx組織.Size = new System.Drawing.Size(401, 190);
            this.grpBx組織.TabIndex = 76;
            this.grpBx組織.TabStop = false;
            this.grpBx組織.Text = "【組織】";
            // 
            // linkLb部門
            // 
            this.linkLb部門.AutoSize = true;
            this.linkLb部門.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLb部門.Location = new System.Drawing.Point(200, 32);
            this.linkLb部門.Name = "linkLb部門";
            this.linkLb部門.Size = new System.Drawing.Size(69, 19);
            this.linkLb部門.TabIndex = 87;
            this.linkLb部門.TabStop = true;
            this.linkLb部門.Text = "部門選択";
            this.linkLb部門.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLb部門_LinkClicked);
            // 
            // listBx部門
            // 
            this.listBx部門.FormattingEnabled = true;
            this.listBx部門.ItemHeight = 17;
            this.listBx部門.Location = new System.Drawing.Point(204, 54);
            this.listBx部門.Name = "listBx部門";
            this.listBx部門.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBx部門.Size = new System.Drawing.Size(171, 106);
            this.listBx部門.TabIndex = 59;
            // 
            // chkBxSundus
            // 
            this.chkBxSundus.AutoSize = true;
            this.chkBxSundus.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSundus.Location = new System.Drawing.Point(23, 89);
            this.chkBxSundus.Name = "chkBxSundus";
            this.chkBxSundus.Size = new System.Drawing.Size(122, 23);
            this.chkBxSundus.TabIndex = 33;
            this.chkBxSundus.Text = "サンミックダスコン";
            this.chkBxSundus.UseVisualStyleBackColor = true;
            // 
            // chkBxOhno
            // 
            this.chkBxOhno.AutoSize = true;
            this.chkBxOhno.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxOhno.Location = new System.Drawing.Point(23, 48);
            this.chkBxOhno.Name = "chkBxOhno";
            this.chkBxOhno.Size = new System.Drawing.Size(61, 23);
            this.chkBxOhno.TabIndex = 32;
            this.chkBxOhno.Text = "オーノ";
            this.chkBxOhno.UseVisualStyleBackColor = true;
            // 
            // chkBxSuncar
            // 
            this.chkBxSuncar.AutoSize = true;
            this.chkBxSuncar.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSuncar.Location = new System.Drawing.Point(23, 132);
            this.chkBxSuncar.Name = "chkBxSuncar";
            this.chkBxSuncar.Size = new System.Drawing.Size(134, 23);
            this.chkBxSuncar.TabIndex = 35;
            this.chkBxSuncar.Text = "サンミックカーペット";
            this.chkBxSuncar.UseVisualStyleBackColor = true;
            // 
            // listBx仕入先
            // 
            this.listBx仕入先.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBx仕入先.FormattingEnabled = true;
            this.listBx仕入先.ItemHeight = 15;
            this.listBx仕入先.Location = new System.Drawing.Point(249, 54);
            this.listBx仕入先.Name = "listBx仕入先";
            this.listBx仕入先.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBx仕入先.Size = new System.Drawing.Size(222, 109);
            this.listBx仕入先.TabIndex = 61;
            // 
            // listBx販売先
            // 
            this.listBx販売先.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBx販売先.FormattingEnabled = true;
            this.listBx販売先.ItemHeight = 15;
            this.listBx販売先.Location = new System.Drawing.Point(14, 54);
            this.listBx販売先.Name = "listBx販売先";
            this.listBx販売先.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBx販売先.Size = new System.Drawing.Size(203, 109);
            this.listBx販売先.TabIndex = 60;
            // 
            // grpBxクラス区分
            // 
            this.grpBxクラス区分.Controls.Add(this.chkBx加工);
            this.grpBxクラス区分.Controls.Add(this.chkBx預り);
            this.grpBxクラス区分.Controls.Add(this.chkBx原材料);
            this.grpBxクラス区分.Controls.Add(this.chkBx半製品);
            this.grpBxクラス区分.Controls.Add(this.chkBx製品);
            this.grpBxクラス区分.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxクラス区分.Location = new System.Drawing.Point(550, 263);
            this.grpBxクラス区分.Name = "grpBxクラス区分";
            this.grpBxクラス区分.Size = new System.Drawing.Size(232, 141);
            this.grpBxクラス区分.TabIndex = 77;
            this.grpBxクラス区分.TabStop = false;
            this.grpBxクラス区分.Text = "【クラス区分】";
            // 
            // chkBx加工
            // 
            this.chkBx加工.AutoSize = true;
            this.chkBx加工.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx加工.Location = new System.Drawing.Point(134, 66);
            this.chkBx加工.Name = "chkBx加工";
            this.chkBx加工.Size = new System.Drawing.Size(58, 23);
            this.chkBx加工.TabIndex = 55;
            this.chkBx加工.Text = "加工";
            this.chkBx加工.UseVisualStyleBackColor = true;
            this.chkBx加工.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx預り
            // 
            this.chkBx預り.AutoSize = true;
            this.chkBx預り.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx預り.Location = new System.Drawing.Point(25, 99);
            this.chkBx預り.Name = "chkBx預り";
            this.chkBx預り.Size = new System.Drawing.Size(52, 23);
            this.chkBx預り.TabIndex = 56;
            this.chkBx預り.Text = "預り";
            this.chkBx預り.UseVisualStyleBackColor = true;
            this.chkBx預り.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx原材料
            // 
            this.chkBx原材料.AutoSize = true;
            this.chkBx原材料.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx原材料.Location = new System.Drawing.Point(25, 32);
            this.chkBx原材料.Name = "chkBx原材料";
            this.chkBx原材料.Size = new System.Drawing.Size(73, 23);
            this.chkBx原材料.TabIndex = 49;
            this.chkBx原材料.Text = "原材料";
            this.chkBx原材料.UseVisualStyleBackColor = true;
            this.chkBx原材料.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx半製品
            // 
            this.chkBx半製品.AutoSize = true;
            this.chkBx半製品.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx半製品.Location = new System.Drawing.Point(134, 32);
            this.chkBx半製品.Name = "chkBx半製品";
            this.chkBx半製品.Size = new System.Drawing.Size(73, 23);
            this.chkBx半製品.TabIndex = 50;
            this.chkBx半製品.Text = "半製品";
            this.chkBx半製品.UseVisualStyleBackColor = true;
            this.chkBx半製品.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx製品
            // 
            this.chkBx製品.AutoSize = true;
            this.chkBx製品.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx製品.Location = new System.Drawing.Point(25, 66);
            this.chkBx製品.Name = "chkBx製品";
            this.chkBx製品.Size = new System.Drawing.Size(58, 23);
            this.chkBx製品.TabIndex = 48;
            this.chkBx製品.Text = "製品";
            this.chkBx製品.UseVisualStyleBackColor = true;
            this.chkBx製品.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBxIv
            // 
            this.chkBxIv.AutoSize = true;
            this.chkBxIv.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxIv.Location = new System.Drawing.Point(198, 30);
            this.chkBxIv.Name = "chkBxIv";
            this.chkBxIv.Size = new System.Drawing.Size(58, 23);
            this.chkBxIv.TabIndex = 44;
            this.chkBxIv.Text = "在庫";
            this.chkBxIv.UseVisualStyleBackColor = true;
            this.chkBxIv.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBxPr
            // 
            this.chkBxPr.AutoSize = true;
            this.chkBxPr.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxPr.Location = new System.Drawing.Point(110, 30);
            this.chkBxPr.Name = "chkBxPr";
            this.chkBxPr.Size = new System.Drawing.Size(58, 23);
            this.chkBxPr.TabIndex = 43;
            this.chkBxPr.Text = "仕入";
            this.chkBxPr.UseVisualStyleBackColor = true;
            this.chkBxPr.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBxSl
            // 
            this.chkBxSl.AutoSize = true;
            this.chkBxSl.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSl.Location = new System.Drawing.Point(22, 30);
            this.chkBxSl.Name = "chkBxSl";
            this.chkBxSl.Size = new System.Drawing.Size(58, 23);
            this.chkBxSl.TabIndex = 42;
            this.chkBxSl.Text = "売上";
            this.chkBxSl.UseVisualStyleBackColor = true;
            this.chkBxSl.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // grpBxデータ区分
            // 
            this.grpBxデータ区分.Controls.Add(this.chkBxSl);
            this.grpBxデータ区分.Controls.Add(this.chkBxPr);
            this.grpBxデータ区分.Controls.Add(this.chkBxIv);
            this.grpBxデータ区分.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxデータ区分.Location = new System.Drawing.Point(526, 174);
            this.grpBxデータ区分.Name = "grpBxデータ区分";
            this.grpBxデータ区分.Size = new System.Drawing.Size(279, 59);
            this.grpBxデータ区分.TabIndex = 78;
            this.grpBxデータ区分.TabStop = false;
            this.grpBxデータ区分.Text = "【データ区分】";
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(796, 6);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(33, 20);
            this.btnMin.TabIndex = 79;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // linkLb販売先
            // 
            this.linkLb販売先.AutoSize = true;
            this.linkLb販売先.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLb販売先.Location = new System.Drawing.Point(10, 28);
            this.linkLb販売先.Name = "linkLb販売先";
            this.linkLb販売先.Size = new System.Drawing.Size(84, 19);
            this.linkLb販売先.TabIndex = 80;
            this.linkLb販売先.TabStop = true;
            this.linkLb販売先.Text = "販売先選択";
            this.linkLb販売先.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLb販売先_LinkClicked);
            // 
            // linkLb仕入先
            // 
            this.linkLb仕入先.AutoSize = true;
            this.linkLb仕入先.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLb仕入先.Location = new System.Drawing.Point(249, 28);
            this.linkLb仕入先.Name = "linkLb仕入先";
            this.linkLb仕入先.Size = new System.Drawing.Size(84, 19);
            this.linkLb仕入先.TabIndex = 81;
            this.linkLb仕入先.TabStop = true;
            this.linkLb仕入先.Text = "仕入先選択";
            this.linkLb仕入先.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLb仕入先_LinkClicked);
            // 
            // grpBx取引先
            // 
            this.grpBx取引先.Controls.Add(this.linkLb販売先);
            this.grpBx取引先.Controls.Add(this.listBx仕入先);
            this.grpBx取引先.Controls.Add(this.listBx販売先);
            this.grpBx取引先.Controls.Add(this.linkLb仕入先);
            this.grpBx取引先.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx取引先.Location = new System.Drawing.Point(8, 376);
            this.grpBx取引先.Name = "grpBx取引先";
            this.grpBx取引先.Size = new System.Drawing.Size(486, 190);
            this.grpBx取引先.TabIndex = 82;
            this.grpBx取引先.TabStop = false;
            this.grpBx取引先.Text = "【取引先】";
            // 
            // rdBtnなし
            // 
            this.rdBtnなし.AutoSize = true;
            this.rdBtnなし.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtnなし.Location = new System.Drawing.Point(10, 27);
            this.rdBtnなし.Name = "rdBtnなし";
            this.rdBtnなし.Size = new System.Drawing.Size(51, 23);
            this.rdBtnなし.TabIndex = 83;
            this.rdBtnなし.TabStop = true;
            this.rdBtnなし.Text = "なし";
            this.rdBtnなし.UseVisualStyleBackColor = true;
            // 
            // rdBtn取引先
            // 
            this.rdBtn取引先.AutoSize = true;
            this.rdBtn取引先.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn取引先.Location = new System.Drawing.Point(80, 27);
            this.rdBtn取引先.Name = "rdBtn取引先";
            this.rdBtn取引先.Size = new System.Drawing.Size(72, 23);
            this.rdBtn取引先.TabIndex = 84;
            this.rdBtn取引先.TabStop = true;
            this.rdBtn取引先.Text = "取引先";
            this.rdBtn取引先.UseVisualStyleBackColor = true;
            // 
            // rdBtn部門
            // 
            this.rdBtn部門.AutoSize = true;
            this.rdBtn部門.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn部門.Location = new System.Drawing.Point(175, 27);
            this.rdBtn部門.Name = "rdBtn部門";
            this.rdBtn部門.Size = new System.Drawing.Size(57, 23);
            this.rdBtn部門.TabIndex = 85;
            this.rdBtn部門.TabStop = true;
            this.rdBtn部門.Text = "部門";
            this.rdBtn部門.UseVisualStyleBackColor = true;
            // 
            // grpBx集計区分
            // 
            this.grpBx集計区分.Controls.Add(this.rdBtnなし);
            this.grpBx集計区分.Controls.Add(this.rdBtn部門);
            this.grpBx集計区分.Controls.Add(this.rdBtn取引先);
            this.grpBx集計区分.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx集計区分.Location = new System.Drawing.Point(545, 449);
            this.grpBx集計区分.Name = "grpBx集計区分";
            this.grpBx集計区分.Size = new System.Drawing.Size(247, 58);
            this.grpBx集計区分.TabIndex = 86;
            this.grpBx集計区分.TabStop = false;
            this.grpBx集計区分.Text = "【集計区分】";
            // 
            // txtBx名称
            // 
            this.txtBx名称.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx名称.Location = new System.Drawing.Point(161, 57);
            this.txtBx名称.Name = "txtBx名称";
            this.txtBx名称.Size = new System.Drawing.Size(195, 24);
            this.txtBx名称.TabIndex = 87;
            // 
            // lb名称
            // 
            this.lb名称.AutoSize = true;
            this.lb名称.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb名称.Location = new System.Drawing.Point(112, 61);
            this.lb名称.Name = "lb名称";
            this.lb名称.Size = new System.Drawing.Size(47, 17);
            this.lb名称.TabIndex = 88;
            this.lb名称.Text = "名称：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::あすよん月次帳票.Properties.Resources.ic_all_csm02;
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // RplForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 586);
            this.Controls.Add(this.lb名称);
            this.Controls.Add(this.txtBx名称);
            this.Controls.Add(this.grpBx集計区分);
            this.Controls.Add(this.grpBx取引先);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.grpBxデータ区分);
            this.Controls.Add(this.grpBxクラス区分);
            this.Controls.Add(this.grpBx組織);
            this.Controls.Add(this.grpBx抽出期間);
            this.Controls.Add(this.grpBxBtn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbTitleDisplay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RplForm2";
            this.Opacity = 0.95D;
            this.Text = "RplForm2";
            this.grpBx抽出期間.ResumeLayout(false);
            this.grpBx抽出期間.PerformLayout();
            this.grpBxBtn.ResumeLayout(false);
            this.grpBx組織.ResumeLayout(false);
            this.grpBx組織.PerformLayout();
            this.grpBxクラス区分.ResumeLayout(false);
            this.grpBxクラス区分.PerformLayout();
            this.grpBxデータ区分.ResumeLayout(false);
            this.grpBxデータ区分.PerformLayout();
            this.grpBx取引先.ResumeLayout(false);
            this.grpBx取引先.PerformLayout();
            this.grpBx集計区分.ResumeLayout(false);
            this.grpBx集計区分.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbTitleDisplay;
        private System.Windows.Forms.Label lb抽出期間;
        private System.Windows.Forms.GroupBox grpBx抽出期間;
        private System.Windows.Forms.TextBox txtBxEndYearMonth;
        private System.Windows.Forms.TextBox txtBxStrYearMonth;
        private System.Windows.Forms.Label lbSymbol1;
        private System.Windows.Forms.GroupBox grpBxBtn;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Button btnForm1Back;
        private System.Windows.Forms.GroupBox grpBx組織;
        private System.Windows.Forms.ListBox listBx仕入先;
        private System.Windows.Forms.ListBox listBx販売先;
        private System.Windows.Forms.ListBox listBx部門;
        private System.Windows.Forms.CheckBox chkBxSundus;
        private System.Windows.Forms.CheckBox chkBxOhno;
        private System.Windows.Forms.CheckBox chkBxSuncar;
        private System.Windows.Forms.GroupBox grpBxクラス区分;
        private System.Windows.Forms.CheckBox chkBx預り;
        private System.Windows.Forms.CheckBox chkBx加工;
        private System.Windows.Forms.CheckBox chkBxIv;
        private System.Windows.Forms.CheckBox chkBxPr;
        private System.Windows.Forms.CheckBox chkBxSl;
        private System.Windows.Forms.CheckBox chkBx製品;
        private System.Windows.Forms.CheckBox chkBx原材料;
        private System.Windows.Forms.CheckBox chkBx半製品;
        private System.Windows.Forms.GroupBox grpBxデータ区分;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnMin;

        private void StyleButton(Button btn, Color backColor, Color foreColor, Color? borderColor = null, int radius = 12)
        {
            // -- ボタンのスタイル設定 --
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // 枠線はPaintで描画
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Meiryo UI", 9F, FontStyle.Bold);

            // ボタン初期色を設定
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatAppearance.BorderColor = borderColor ?? ColorManager.MemeDark1;

            // 角丸設定
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);
            }

            // Paintイベントで背景・枠線・文字を描画
            btn.Paint += (s, e) =>
            {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                using (var brush = new SolidBrush(btn.BackColor))
                using (var pen = new Pen(btn.FlatAppearance.BorderColor, 2))
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 背景
                    e.Graphics.FillPath(brush, path);

                    // 枠線
                    e.Graphics.DrawPath(pen, path);

                    // 文字
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor,
                                          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
            SetButtonAnimation(btn);

            // ★ 初期色を記録
            if (!originalColors.ContainsKey(btn))
                originalColors[btn] = btn.BackColor;
            // ★ 初期位置を記録
            if (!originalYPositions.ContainsKey(btn))
                originalYPositions[btn] = btn.Location.Y;

            // ★ クリック時アニメーション
            btn.MouseDown += (s, e) =>
            {
                float scale = 0.95f;
                var info = (btn.Size, btn.Location);
                btn.Tag = info;

                int newW = (int)(btn.Width * scale);
                int newH = (int)(btn.Height * scale);
                btn.Location = new Point(btn.Left + (btn.Width - newW) / 2, btn.Top + (btn.Height - newH) / 2);
                btn.Size = new Size(newW, newH);
            };

            btn.MouseUp += (s, e) =>
            {
                if (btn.Tag is ValueTuple<Size, Point> info)
                {
                    btn.Size = info.Item1;
                    btn.Location = info.Item2;
                }
            };
        }
        // ボタンごとの位置を保持（上昇防止用）
        private readonly Dictionary<Button, int> originalYPositions = new Dictionary<Button, int>();
        private readonly Dictionary<Button, bool> animatingButtons = new Dictionary<Button, bool>();
        private readonly Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        private async Task AnimateButton(Button btn, bool enter)
        {
            // ボタンの基本色取得
            Color baseColor = originalColors.ContainsKey(btn) ? originalColors[btn] : btn.BackColor; ;
            Color baseBorder = btn.FlatAppearance.BorderColor;

            // フェード先を少し明るくした色にする
            Color hoverColor = ControlPaint.Light(baseColor, 0.3f);
            Color hoverBorder = ControlPaint.Light(baseBorder, 0.3f);

            Color startColor = enter ? baseColor : hoverColor;
            Color endColor = enter ? hoverColor : baseColor;

            Color startBorder = enter ? baseBorder : hoverBorder;
            Color endBorder = enter ? hoverBorder : baseBorder;

            int steps = 10;
            int jumpHeight = enter ? 5 : 0; // 軽くジャンプ(出るときはジャンプしない)
            // 「元の位置」を辞書から取得する！
            int originalY = originalYPositions.ContainsKey(btn)
                ? originalYPositions[btn]
                : btn.Location.Y;

            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;

                // 背景色の補間
                btn.BackColor = Color.FromArgb(
                    (int)(startColor.R + (endColor.R - startColor.R) * t),
                    (int)(startColor.G + (endColor.G - startColor.G) * t),
                    (int)(startColor.B + (endColor.B - startColor.B) * t));

                // 枠線色の補間
                btn.FlatAppearance.BorderColor = Color.FromArgb(
                    (int)(startBorder.R + (endBorder.R - startBorder.R) * t),
                    (int)(startBorder.G + (endBorder.G - startBorder.G) * t),
                    (int)(startBorder.B + (endBorder.B - startBorder.B) * t));

                // ======== ジャンプはEnter時のみ ========
                if (enter)
                {
                    // 上に軽く持ち上げて戻る（sin波）
                    btn.Location = new Point(
                        btn.Location.X,
                        originalY - (int)(jumpHeight * Math.Sin(Math.PI * t))
                    );
                }
                else
                {
                    // Leave 時は確実に元の位置に戻す
                    btn.Location = new Point(btn.Location.X, originalY);
                }

                await Task.Delay(15); // アニメーション速度調整
            }
            btn.BackColor = endColor;
            btn.FlatAppearance.BorderColor = endBorder;
            btn.Location = new Point(btn.Location.X, originalY);
        }

        // 各ボタンにイベント登録
        private void SetButtonAnimation(Button btn)
        {
            btn.MouseEnter += async (s, e) =>
            {
                bool isAnimating;
                if (!animatingButtons.TryGetValue(btn, out isAnimating))
                {
                    isAnimating = false; // デフォルト値
                }
                animatingButtons[btn] = true;
                await AnimateButton(btn, true);
                animatingButtons[btn] = false;
            };

            btn.MouseLeave += async (s, e) =>
            {
                bool isAnimating;
                if (!animatingButtons.TryGetValue(btn, out isAnimating))
                {
                    isAnimating = false; // デフォルト値
                }
                animatingButtons[btn] = true;
                await AnimateButton(btn, false);
                animatingButtons[btn] = false;
            };

            btn.MouseDown += (s, e) => btn.FlatAppearance.BorderSize = 4;
            btn.MouseUp += (s, e) => btn.FlatAppearance.BorderSize = 2;
        }

        private LinkLabel linkLb販売先;
        private LinkLabel linkLb仕入先;
        private GroupBox grpBx取引先;
        private RadioButton rdBtnなし;
        private RadioButton rdBtn取引先;
        private RadioButton rdBtn部門;
        private GroupBox grpBx集計区分;
        private LinkLabel linkLb部門;
        private TextBox txtBx名称;
        private Label lb名称;
    }
}