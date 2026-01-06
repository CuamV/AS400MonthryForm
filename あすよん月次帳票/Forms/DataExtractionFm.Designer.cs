using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class DataExtractionFm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataExtractionFm));
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
            this.chkBx加工T = new System.Windows.Forms.CheckBox();
            this.chkBx預けT = new System.Windows.Forms.CheckBox();
            this.chkBx原材料 = new System.Windows.Forms.CheckBox();
            this.chkBx預りT = new System.Windows.Forms.CheckBox();
            this.chkBx半製品 = new System.Windows.Forms.CheckBox();
            this.chkBx製品 = new System.Windows.Forms.CheckBox();
            this.chkBx預け = new System.Windows.Forms.CheckBox();
            this.chkBxIv = new System.Windows.Forms.CheckBox();
            this.chkBxPr = new System.Windows.Forms.CheckBox();
            this.chkBxSl = new System.Windows.Forms.CheckBox();
            this.grpBxデータ区分 = new System.Windows.Forms.GroupBox();
            this.btnMin = new System.Windows.Forms.Button();
            this.linkLb販売先 = new System.Windows.Forms.LinkLabel();
            this.linkLb仕入先 = new System.Windows.Forms.LinkLabel();
            this.grpBx取引先 = new System.Windows.Forms.GroupBox();
            this.rdBtn売仕なし = new System.Windows.Forms.RadioButton();
            this.rdBtn売仕取引先 = new System.Windows.Forms.RadioButton();
            this.rdBtn売仕部門 = new System.Windows.Forms.RadioButton();
            this.grpBx売仕集計区分 = new System.Windows.Forms.GroupBox();
            this.rdBtn品目 = new System.Windows.Forms.RadioButton();
            this.txtBx名称 = new System.Windows.Forms.TextBox();
            this.lb名称 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpBx在集計区分 = new System.Windows.Forms.GroupBox();
            this.rdBtn在なし = new System.Windows.Forms.RadioButton();
            this.rdBtn在品種 = new System.Windows.Forms.RadioButton();
            this.grpBx在庫種別 = new System.Windows.Forms.GroupBox();
            this.chkBx自社 = new System.Windows.Forms.CheckBox();
            this.chkBx預り = new System.Windows.Forms.CheckBox();
            this.chkBx投入 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timrAnimation2 = new System.Windows.Forms.Timer(this.components);
            this.grpBx抽出期間.SuspendLayout();
            this.grpBxBtn.SuspendLayout();
            this.grpBx組織.SuspendLayout();
            this.grpBxクラス区分.SuspendLayout();
            this.grpBxデータ区分.SuspendLayout();
            this.grpBx取引先.SuspendLayout();
            this.grpBx売仕集計区分.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpBx在集計区分.SuspendLayout();
            this.grpBx在庫種別.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbTitleDisplay
            // 
            this.lbTitleDisplay.AutoSize = true;
            this.lbTitleDisplay.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitleDisplay.Location = new System.Drawing.Point(44, 10);
            this.lbTitleDisplay.Name = "lbTitleDisplay";
            this.lbTitleDisplay.Size = new System.Drawing.Size(92, 17);
            this.lbTitleDisplay.TabIndex = 1;
            this.lbTitleDisplay.Text = "< データ抽出>";
            // 
            // lb抽出期間
            // 
            this.lb抽出期間.AutoSize = true;
            this.lb抽出期間.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb抽出期間.Location = new System.Drawing.Point(6, 58);
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
            this.grpBx抽出期間.Location = new System.Drawing.Point(106, 105);
            this.grpBx抽出期間.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx抽出期間.Name = "grpBx抽出期間";
            this.grpBx抽出期間.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx抽出期間.Size = new System.Drawing.Size(279, 82);
            this.grpBx抽出期間.TabIndex = 1;
            this.grpBx抽出期間.TabStop = false;
            this.grpBx抽出期間.Text = "【抽出期間】";
            // 
            // txtBxEndYearMonth
            // 
            this.txtBxEndYearMonth.Location = new System.Drawing.Point(162, 22);
            this.txtBxEndYearMonth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBxEndYearMonth.Name = "txtBxEndYearMonth";
            this.txtBxEndYearMonth.Size = new System.Drawing.Size(106, 24);
            this.txtBxEndYearMonth.TabIndex = 1;
            this.txtBxEndYearMonth.TextChanged += new System.EventHandler(this.chkBxControl);
            // 
            // txtBxStrYearMonth
            // 
            this.txtBxStrYearMonth.Location = new System.Drawing.Point(14, 22);
            this.txtBxStrYearMonth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBxStrYearMonth.Name = "txtBxStrYearMonth";
            this.txtBxStrYearMonth.Size = new System.Drawing.Size(106, 24);
            this.txtBxStrYearMonth.TabIndex = 0;
            this.txtBxStrYearMonth.TextChanged += new System.EventHandler(this.chkBxControl);
            // 
            // lbSymbol1
            // 
            this.lbSymbol1.AutoSize = true;
            this.lbSymbol1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSymbol1.Location = new System.Drawing.Point(130, 25);
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
            this.grpBxBtn.Font = new System.Drawing.Font("MS UI Gothic", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxBtn.Location = new System.Drawing.Point(491, 54);
            this.grpBxBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBxBtn.Name = "grpBxBtn";
            this.grpBxBtn.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBxBtn.Size = new System.Drawing.Size(363, 62);
            this.grpBxBtn.TabIndex = 10;
            this.grpBxBtn.TabStop = false;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Font = new System.Drawing.Font("Meiryo UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnExportExcel.Location = new System.Drawing.Point(130, 14);
            this.btnExportExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(104, 34);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Excelエクスポート";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnDisplay
            // 
            this.btnDisplay.Font = new System.Drawing.Font("Meiryo UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDisplay.Location = new System.Drawing.Point(15, 14);
            this.btnDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(104, 34);
            this.btnDisplay.TabIndex = 0;
            this.btnDisplay.Text = "データ表示実行";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnForm1Back
            // 
            this.btnForm1Back.Font = new System.Drawing.Font("Meiryo UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnForm1Back.Location = new System.Drawing.Point(244, 14);
            this.btnForm1Back.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnForm1Back.Name = "btnForm1Back";
            this.btnForm1Back.Size = new System.Drawing.Size(104, 34);
            this.btnForm1Back.TabIndex = 2;
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
            this.grpBx組織.Location = new System.Drawing.Point(46, 201);
            this.grpBx組織.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx組織.Name = "grpBx組織";
            this.grpBx組織.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx組織.Size = new System.Drawing.Size(395, 177);
            this.grpBx組織.TabIndex = 2;
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
            this.linkLb部門.TabIndex = 3;
            this.linkLb部門.TabStop = true;
            this.linkLb部門.Text = "部門選択";
            this.linkLb部門.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLb部門_LinkClicked);
            // 
            // listBx部門
            // 
            this.listBx部門.FormattingEnabled = true;
            this.listBx部門.ItemHeight = 17;
            this.listBx部門.Location = new System.Drawing.Point(204, 54);
            this.listBx部門.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBx部門.Name = "listBx部門";
            this.listBx部門.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBx部門.Size = new System.Drawing.Size(170, 89);
            this.listBx部門.TabIndex = 59;
            // 
            // chkBxSundus
            // 
            this.chkBxSundus.AutoSize = true;
            this.chkBxSundus.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSundus.Location = new System.Drawing.Point(22, 90);
            this.chkBxSundus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBxSundus.Name = "chkBxSundus";
            this.chkBxSundus.Size = new System.Drawing.Size(122, 23);
            this.chkBxSundus.TabIndex = 1;
            this.chkBxSundus.Text = "サンミックダスコン";
            this.chkBxSundus.UseVisualStyleBackColor = true;
            // 
            // chkBxOhno
            // 
            this.chkBxOhno.AutoSize = true;
            this.chkBxOhno.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxOhno.Location = new System.Drawing.Point(22, 48);
            this.chkBxOhno.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBxOhno.Name = "chkBxOhno";
            this.chkBxOhno.Size = new System.Drawing.Size(61, 23);
            this.chkBxOhno.TabIndex = 0;
            this.chkBxOhno.Text = "オーノ";
            this.chkBxOhno.UseVisualStyleBackColor = true;
            // 
            // chkBxSuncar
            // 
            this.chkBxSuncar.AutoSize = true;
            this.chkBxSuncar.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSuncar.Location = new System.Drawing.Point(22, 132);
            this.chkBxSuncar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBxSuncar.Name = "chkBxSuncar";
            this.chkBxSuncar.Size = new System.Drawing.Size(134, 23);
            this.chkBxSuncar.TabIndex = 2;
            this.chkBxSuncar.Text = "サンミックカーペット";
            this.chkBxSuncar.UseVisualStyleBackColor = true;
            // 
            // listBx仕入先
            // 
            this.listBx仕入先.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBx仕入先.FormattingEnabled = true;
            this.listBx仕入先.ItemHeight = 15;
            this.listBx仕入先.Location = new System.Drawing.Point(227, 54);
            this.listBx仕入先.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBx仕入先.Name = "listBx仕入先";
            this.listBx仕入先.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBx仕入先.Size = new System.Drawing.Size(226, 94);
            this.listBx仕入先.TabIndex = 61;
            // 
            // listBx販売先
            // 
            this.listBx販売先.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBx販売先.FormattingEnabled = true;
            this.listBx販売先.ItemHeight = 15;
            this.listBx販売先.Location = new System.Drawing.Point(8, 54);
            this.listBx販売先.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBx販売先.Name = "listBx販売先";
            this.listBx販売先.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBx販売先.Size = new System.Drawing.Size(207, 94);
            this.listBx販売先.TabIndex = 60;
            // 
            // grpBxクラス区分
            // 
            this.grpBxクラス区分.Controls.Add(this.chkBx加工T);
            this.grpBxクラス区分.Controls.Add(this.chkBx預けT);
            this.grpBxクラス区分.Controls.Add(this.chkBx原材料);
            this.grpBxクラス区分.Controls.Add(this.chkBx預りT);
            this.grpBxクラス区分.Controls.Add(this.chkBx半製品);
            this.grpBxクラス区分.Controls.Add(this.chkBx製品);
            this.grpBxクラス区分.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxクラス区分.Location = new System.Drawing.Point(491, 206);
            this.grpBxクラス区分.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBxクラス区分.Name = "grpBxクラス区分";
            this.grpBxクラス区分.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBxクラス区分.Size = new System.Drawing.Size(363, 101);
            this.grpBxクラス区分.TabIndex = 5;
            this.grpBxクラス区分.TabStop = false;
            this.grpBxクラス区分.Text = "【クラス区分】";
            // 
            // chkBx加工T
            // 
            this.chkBx加工T.AutoSize = true;
            this.chkBx加工T.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx加工T.Location = new System.Drawing.Point(17, 71);
            this.chkBx加工T.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx加工T.Name = "chkBx加工T";
            this.chkBx加工T.Size = new System.Drawing.Size(81, 23);
            this.chkBx加工T.TabIndex = 3;
            this.chkBx加工T.Text = "加工(5)";
            this.chkBx加工T.UseVisualStyleBackColor = true;
            // 
            // chkBx預けT
            // 
            this.chkBx預けT.AutoSize = true;
            this.chkBx預けT.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx預けT.Location = new System.Drawing.Point(264, 71);
            this.chkBx預けT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx預けT.Name = "chkBx預けT";
            this.chkBx預けT.Size = new System.Drawing.Size(78, 23);
            this.chkBx預けT.TabIndex = 5;
            this.chkBx預けT.Text = "預け(7)";
            this.chkBx預けT.UseVisualStyleBackColor = true;
            this.chkBx預けT.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx原材料
            // 
            this.chkBx原材料.AutoSize = true;
            this.chkBx原材料.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx原材料.Location = new System.Drawing.Point(17, 32);
            this.chkBx原材料.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx原材料.Name = "chkBx原材料";
            this.chkBx原材料.Size = new System.Drawing.Size(96, 23);
            this.chkBx原材料.TabIndex = 0;
            this.chkBx原材料.Text = "原材料(1)";
            this.chkBx原材料.UseVisualStyleBackColor = true;
            this.chkBx原材料.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx預りT
            // 
            this.chkBx預りT.AutoSize = true;
            this.chkBx預りT.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx預りT.Location = new System.Drawing.Point(134, 71);
            this.chkBx預りT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx預りT.Name = "chkBx預りT";
            this.chkBx預りT.Size = new System.Drawing.Size(75, 23);
            this.chkBx預りT.TabIndex = 4;
            this.chkBx預りT.Text = "預り(6)";
            this.chkBx預りT.UseVisualStyleBackColor = true;
            this.chkBx預りT.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx半製品
            // 
            this.chkBx半製品.AutoSize = true;
            this.chkBx半製品.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx半製品.Location = new System.Drawing.Point(134, 32);
            this.chkBx半製品.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx半製品.Name = "chkBx半製品";
            this.chkBx半製品.Size = new System.Drawing.Size(110, 23);
            this.chkBx半製品.TabIndex = 1;
            this.chkBx半製品.Text = "半製品(2,3)";
            this.chkBx半製品.UseVisualStyleBackColor = true;
            this.chkBx半製品.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx製品
            // 
            this.chkBx製品.AutoSize = true;
            this.chkBx製品.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx製品.Location = new System.Drawing.Point(264, 32);
            this.chkBx製品.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx製品.Name = "chkBx製品";
            this.chkBx製品.Size = new System.Drawing.Size(81, 23);
            this.chkBx製品.TabIndex = 2;
            this.chkBx製品.Text = "製品(4)";
            this.chkBx製品.UseVisualStyleBackColor = true;
            this.chkBx製品.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx預け
            // 
            this.chkBx預け.AutoSize = true;
            this.chkBx預け.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx預け.Location = new System.Drawing.Point(99, 30);
            this.chkBx預け.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx預け.Name = "chkBx預け";
            this.chkBx預け.Size = new System.Drawing.Size(78, 23);
            this.chkBx預け.TabIndex = 1;
            this.chkBx預け.Text = "預け(1)";
            this.chkBx預け.UseVisualStyleBackColor = true;
            this.chkBx預け.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBxIv
            // 
            this.chkBxIv.AutoSize = true;
            this.chkBxIv.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxIv.Location = new System.Drawing.Point(198, 30);
            this.chkBxIv.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBxIv.Name = "chkBxIv";
            this.chkBxIv.Size = new System.Drawing.Size(58, 23);
            this.chkBxIv.TabIndex = 2;
            this.chkBxIv.Text = "在庫";
            this.chkBxIv.UseVisualStyleBackColor = true;
            this.chkBxIv.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBxPr
            // 
            this.chkBxPr.AutoSize = true;
            this.chkBxPr.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxPr.Location = new System.Drawing.Point(110, 30);
            this.chkBxPr.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBxPr.Name = "chkBxPr";
            this.chkBxPr.Size = new System.Drawing.Size(58, 23);
            this.chkBxPr.TabIndex = 1;
            this.chkBxPr.Text = "仕入";
            this.chkBxPr.UseVisualStyleBackColor = true;
            this.chkBxPr.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBxSl
            // 
            this.chkBxSl.AutoSize = true;
            this.chkBxSl.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSl.Location = new System.Drawing.Point(22, 30);
            this.chkBxSl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBxSl.Name = "chkBxSl";
            this.chkBxSl.Size = new System.Drawing.Size(58, 23);
            this.chkBxSl.TabIndex = 0;
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
            this.grpBxデータ区分.Location = new System.Drawing.Point(540, 135);
            this.grpBxデータ区分.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBxデータ区分.Name = "grpBxデータ区分";
            this.grpBxデータ区分.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBxデータ区分.Size = new System.Drawing.Size(279, 58);
            this.grpBxデータ区分.TabIndex = 4;
            this.grpBxデータ区分.TabStop = false;
            this.grpBxデータ区分.Text = "【データ区分】";
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(778, 6);
            this.btnMin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(33, 20);
            this.btnMin.TabIndex = 11;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // linkLb販売先
            // 
            this.linkLb販売先.AutoSize = true;
            this.linkLb販売先.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLb販売先.Location = new System.Drawing.Point(6, 28);
            this.linkLb販売先.Name = "linkLb販売先";
            this.linkLb販売先.Size = new System.Drawing.Size(84, 19);
            this.linkLb販売先.TabIndex = 0;
            this.linkLb販売先.TabStop = true;
            this.linkLb販売先.Text = "販売先選択";
            this.linkLb販売先.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLb販売先_LinkClicked);
            // 
            // linkLb仕入先
            // 
            this.linkLb仕入先.AutoSize = true;
            this.linkLb仕入先.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLb仕入先.Location = new System.Drawing.Point(226, 28);
            this.linkLb仕入先.Name = "linkLb仕入先";
            this.linkLb仕入先.Size = new System.Drawing.Size(84, 19);
            this.linkLb仕入先.TabIndex = 1;
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
            this.grpBx取引先.Location = new System.Drawing.Point(10, 396);
            this.grpBx取引先.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx取引先.Name = "grpBx取引先";
            this.grpBx取引先.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx取引先.Size = new System.Drawing.Size(460, 175);
            this.grpBx取引先.TabIndex = 3;
            this.grpBx取引先.TabStop = false;
            this.grpBx取引先.Text = "【取引先】";
            // 
            // rdBtn売仕なし
            // 
            this.rdBtn売仕なし.AutoSize = true;
            this.rdBtn売仕なし.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn売仕なし.Location = new System.Drawing.Point(16, 21);
            this.rdBtn売仕なし.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtn売仕なし.Name = "rdBtn売仕なし";
            this.rdBtn売仕なし.Size = new System.Drawing.Size(51, 23);
            this.rdBtn売仕なし.TabIndex = 0;
            this.rdBtn売仕なし.TabStop = true;
            this.rdBtn売仕なし.Tag = "NONE";
            this.rdBtn売仕なし.Text = "なし";
            this.rdBtn売仕なし.UseVisualStyleBackColor = true;
            // 
            // rdBtn売仕取引先
            // 
            this.rdBtn売仕取引先.AutoSize = true;
            this.rdBtn売仕取引先.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn売仕取引先.Location = new System.Drawing.Point(167, 21);
            this.rdBtn売仕取引先.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtn売仕取引先.Name = "rdBtn売仕取引先";
            this.rdBtn売仕取引先.Size = new System.Drawing.Size(72, 23);
            this.rdBtn売仕取引先.TabIndex = 2;
            this.rdBtn売仕取引先.TabStop = true;
            this.rdBtn売仕取引先.Tag = "取引先CD";
            this.rdBtn売仕取引先.Text = "取引先";
            this.rdBtn売仕取引先.UseVisualStyleBackColor = true;
            // 
            // rdBtn売仕部門
            // 
            this.rdBtn売仕部門.AutoSize = true;
            this.rdBtn売仕部門.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn売仕部門.Location = new System.Drawing.Point(261, 21);
            this.rdBtn売仕部門.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtn売仕部門.Name = "rdBtn売仕部門";
            this.rdBtn売仕部門.Size = new System.Drawing.Size(57, 23);
            this.rdBtn売仕部門.TabIndex = 3;
            this.rdBtn売仕部門.TabStop = true;
            this.rdBtn売仕部門.Tag = "部門CD";
            this.rdBtn売仕部門.Text = "部門";
            this.rdBtn売仕部門.UseVisualStyleBackColor = true;
            // 
            // grpBx売仕集計区分
            // 
            this.grpBx売仕集計区分.Controls.Add(this.rdBtn品目);
            this.grpBx売仕集計区分.Controls.Add(this.rdBtn売仕なし);
            this.grpBx売仕集計区分.Controls.Add(this.rdBtn売仕部門);
            this.grpBx売仕集計区分.Controls.Add(this.rdBtn売仕取引先);
            this.grpBx売仕集計区分.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx売仕集計区分.Location = new System.Drawing.Point(507, 450);
            this.grpBx売仕集計区分.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx売仕集計区分.Name = "grpBx売仕集計区分";
            this.grpBx売仕集計区分.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx売仕集計区分.Size = new System.Drawing.Size(335, 51);
            this.grpBx売仕集計区分.TabIndex = 8;
            this.grpBx売仕集計区分.TabStop = false;
            this.grpBx売仕集計区分.Text = "【売上・仕入集計区分】";
            // 
            // rdBtn品目
            // 
            this.rdBtn品目.AutoSize = true;
            this.rdBtn品目.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn品目.Location = new System.Drawing.Point(88, 21);
            this.rdBtn品目.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtn品目.Name = "rdBtn品目";
            this.rdBtn品目.Size = new System.Drawing.Size(57, 23);
            this.rdBtn品目.TabIndex = 1;
            this.rdBtn品目.TabStop = true;
            this.rdBtn品目.Tag = "品目CD";
            this.rdBtn品目.Text = "品目";
            this.rdBtn品目.UseVisualStyleBackColor = true;
            // 
            // txtBx名称
            // 
            this.txtBx名称.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBx名称.Location = new System.Drawing.Point(186, 70);
            this.txtBx名称.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBx名称.Name = "txtBx名称";
            this.txtBx名称.Size = new System.Drawing.Size(194, 24);
            this.txtBx名称.TabIndex = 0;
            // 
            // lb名称
            // 
            this.lb名称.AutoSize = true;
            this.lb名称.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb名称.Location = new System.Drawing.Point(112, 70);
            this.lb名称.Name = "lb名称";
            this.lb名称.Size = new System.Drawing.Size(73, 17);
            this.lb名称.TabIndex = 88;
            this.lb名称.Text = "帳票名称：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::あすよん月次帳票.Properties.Resources.ic_all_csm02;
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // grpBx在集計区分
            // 
            this.grpBx在集計区分.Controls.Add(this.rdBtn在なし);
            this.grpBx在集計区分.Controls.Add(this.rdBtn在品種);
            this.grpBx在集計区分.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx在集計区分.Location = new System.Drawing.Point(590, 520);
            this.grpBx在集計区分.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx在集計区分.Name = "grpBx在集計区分";
            this.grpBx在集計区分.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx在集計区分.Size = new System.Drawing.Size(154, 51);
            this.grpBx在集計区分.TabIndex = 9;
            this.grpBx在集計区分.TabStop = false;
            this.grpBx在集計区分.Text = "【在庫集計区分】";
            // 
            // rdBtn在なし
            // 
            this.rdBtn在なし.AutoSize = true;
            this.rdBtn在なし.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn在なし.Location = new System.Drawing.Point(15, 21);
            this.rdBtn在なし.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtn在なし.Name = "rdBtn在なし";
            this.rdBtn在なし.Size = new System.Drawing.Size(51, 23);
            this.rdBtn在なし.TabIndex = 0;
            this.rdBtn在なし.TabStop = true;
            this.rdBtn在なし.Tag = "NONE";
            this.rdBtn在なし.Text = "なし";
            this.rdBtn在なし.UseVisualStyleBackColor = true;
            // 
            // rdBtn在品種
            // 
            this.rdBtn在品種.AutoSize = true;
            this.rdBtn在品種.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdBtn在品種.Location = new System.Drawing.Point(89, 21);
            this.rdBtn在品種.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdBtn在品種.Name = "rdBtn在品種";
            this.rdBtn在品種.Size = new System.Drawing.Size(57, 23);
            this.rdBtn在品種.TabIndex = 1;
            this.rdBtn在品種.TabStop = true;
            this.rdBtn在品種.Tag = "品目CD";
            this.rdBtn在品種.Text = "品目";
            this.rdBtn在品種.UseVisualStyleBackColor = true;
            // 
            // grpBx在庫種別
            // 
            this.grpBx在庫種別.Controls.Add(this.chkBx自社);
            this.grpBx在庫種別.Controls.Add(this.chkBx預け);
            this.grpBx在庫種別.Controls.Add(this.chkBx預り);
            this.grpBx在庫種別.Controls.Add(this.chkBx投入);
            this.grpBx在庫種別.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx在庫種別.Location = new System.Drawing.Point(491, 328);
            this.grpBx在庫種別.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx在庫種別.Name = "grpBx在庫種別";
            this.grpBx在庫種別.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBx在庫種別.Size = new System.Drawing.Size(363, 73);
            this.grpBx在庫種別.TabIndex = 6;
            this.grpBx在庫種別.TabStop = false;
            this.grpBx在庫種別.Text = "【在庫種別】";
            // 
            // chkBx自社
            // 
            this.chkBx自社.AutoSize = true;
            this.chkBx自社.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx自社.Location = new System.Drawing.Point(9, 30);
            this.chkBx自社.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx自社.Name = "chkBx自社";
            this.chkBx自社.Size = new System.Drawing.Size(81, 23);
            this.chkBx自社.TabIndex = 0;
            this.chkBx自社.Text = "自社(0)";
            this.chkBx自社.UseVisualStyleBackColor = true;
            this.chkBx自社.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx預り
            // 
            this.chkBx預り.AutoSize = true;
            this.chkBx預り.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx預り.Location = new System.Drawing.Point(186, 30);
            this.chkBx預り.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx預り.Name = "chkBx預り";
            this.chkBx預り.Size = new System.Drawing.Size(75, 23);
            this.chkBx預り.TabIndex = 2;
            this.chkBx預り.Text = "預り(2)";
            this.chkBx預り.UseVisualStyleBackColor = true;
            this.chkBx預り.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // chkBx投入
            // 
            this.chkBx投入.AutoSize = true;
            this.chkBx投入.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBx投入.Location = new System.Drawing.Point(270, 30);
            this.chkBx投入.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBx投入.Name = "chkBx投入";
            this.chkBx投入.Size = new System.Drawing.Size(81, 23);
            this.chkBx投入.TabIndex = 3;
            this.chkBx投入.Text = "投入(9)";
            this.chkBx投入.UseVisualStyleBackColor = true;
            this.chkBx投入.CheckedChanged += new System.EventHandler(this.chkBxControl);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(362, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(8, 8);
            this.groupBox1.TabIndex = 91;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(548, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 92;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 600);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpBx在庫種別);
            this.Controls.Add(this.grpBx在集計区分);
            this.Controls.Add(this.lb名称);
            this.Controls.Add(this.txtBx名称);
            this.Controls.Add(this.grpBx売仕集計区分);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form2";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
            this.grpBx売仕集計区分.ResumeLayout(false);
            this.grpBx売仕集計区分.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpBx在集計区分.ResumeLayout(false);
            this.grpBx在集計区分.PerformLayout();
            this.grpBx在庫種別.ResumeLayout(false);
            this.grpBx在庫種別.PerformLayout();
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
        private System.Windows.Forms.CheckBox chkBx預け;
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
            btn.FlatAppearance.BorderColor = borderColor ?? clrmg.MemeDark1;

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
        private RadioButton rdBtn売仕なし;
        private RadioButton rdBtn売仕取引先;
        private RadioButton rdBtn売仕部門;
        private GroupBox grpBx売仕集計区分;
        private LinkLabel linkLb部門;
        private TextBox txtBx名称;
        private Label lb名称;
        private GroupBox grpBx在集計区分;
        private RadioButton rdBtn在なし;
        private RadioButton rdBtn在品種;
        private GroupBox grpBx在庫種別;
        private CheckBox chkBx自社;
        private CheckBox chkBx預り;
        private CheckBox chkBx投入;
        private GroupBox groupBox1;
        private CheckBox chkBx預けT;
        private CheckBox chkBx預りT;
        private Label label1;
        private CheckBox chkBx加工T;
        private RadioButton rdBtn品目;
        private Timer timrAnimation2;
    }
}