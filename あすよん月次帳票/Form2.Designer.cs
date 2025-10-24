using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.DataGridView dgvPurchase;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridView dgvSummary;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.dgvDataOhno = new System.Windows.Forms.DataGridView();
            this.dgvDataScar = new System.Windows.Forms.DataGridView();
            this.dgvDataSdus = new System.Windows.Forms.DataGridView();
            this.dgvDataIV = new System.Windows.Forms.DataGridView();
            this.chkBxSuncar = new System.Windows.Forms.CheckBox();
            this.chkBxOhno = new System.Windows.Forms.CheckBox();
            this.chkBxSundus = new System.Windows.Forms.CheckBox();
            this.lbCompany = new System.Windows.Forms.Label();
            this.lbBumon = new System.Windows.Forms.Label();
            this.lbSymbol1 = new System.Windows.Forms.Label();
            this.chkBxProAll = new System.Windows.Forms.CheckBox();
            this.chkBxSemiFinProducts = new System.Windows.Forms.CheckBox();
            this.chkBxRawMaterials = new System.Windows.Forms.CheckBox();
            this.chkBxProduct = new System.Windows.Forms.CheckBox();
            this.lbProductClass = new System.Windows.Forms.Label();
            this.lbSalesCategory = new System.Windows.Forms.Label();
            this.chkBxSl = new System.Windows.Forms.CheckBox();
            this.chkBxPr = new System.Windows.Forms.CheckBox();
            this.chkBxIv = new System.Windows.Forms.CheckBox();
            this.chkBxSalesAll = new System.Windows.Forms.CheckBox();
            this.lbSupplier = new System.Windows.Forms.Label();
            this.lbSaller = new System.Windows.Forms.Label();
            this.lbDataOhno = new System.Windows.Forms.Label();
            this.lbDataSuncar = new System.Windows.Forms.Label();
            this.lbDataSundus = new System.Windows.Forms.Label();
            this.lbDataIV = new System.Windows.Forms.Label();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.btnForm1Back = new System.Windows.Forms.Button();
            this.grpBxBtn = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBxEndYearMonth = new System.Windows.Forms.TextBox();
            this.txtBxStrYearMonth = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBxSupplier = new System.Windows.Forms.ListBox();
            this.listBxSaller = new System.Windows.Forms.ListBox();
            this.listBxBumon = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkBxProcess = new System.Windows.Forms.CheckBox();
            this.lbTitleDisplay = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataOhno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataScar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSdus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataIV)).BeginInit();
            this.grpBxBtn.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDataOhno
            // 
            this.dgvDataOhno.AllowDrop = true;
            this.dgvDataOhno.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvDataOhno.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataOhno.Location = new System.Drawing.Point(503, 30);
            this.dgvDataOhno.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDataOhno.Name = "dgvDataOhno";
            this.dgvDataOhno.RowTemplate.Height = 21;
            this.dgvDataOhno.Size = new System.Drawing.Size(909, 165);
            this.dgvDataOhno.TabIndex = 2;
            // 
            // dgvDataScar
            // 
            this.dgvDataScar.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvDataScar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataScar.Location = new System.Drawing.Point(503, 423);
            this.dgvDataScar.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDataScar.Name = "dgvDataScar";
            this.dgvDataScar.RowTemplate.Height = 21;
            this.dgvDataScar.Size = new System.Drawing.Size(909, 165);
            this.dgvDataScar.TabIndex = 13;
            // 
            // dgvDataSdus
            // 
            this.dgvDataSdus.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvDataSdus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataSdus.Location = new System.Drawing.Point(503, 228);
            this.dgvDataSdus.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDataSdus.Name = "dgvDataSdus";
            this.dgvDataSdus.RowTemplate.Height = 21;
            this.dgvDataSdus.Size = new System.Drawing.Size(909, 165);
            this.dgvDataSdus.TabIndex = 14;
            // 
            // dgvDataIV
            // 
            this.dgvDataIV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvDataIV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataIV.Location = new System.Drawing.Point(503, 613);
            this.dgvDataIV.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDataIV.Name = "dgvDataIV";
            this.dgvDataIV.RowTemplate.Height = 21;
            this.dgvDataIV.Size = new System.Drawing.Size(909, 165);
            this.dgvDataIV.TabIndex = 17;
            // 
            // chkBxSuncar
            // 
            this.chkBxSuncar.AutoSize = true;
            this.chkBxSuncar.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSuncar.Location = new System.Drawing.Point(11, 158);
            this.chkBxSuncar.Name = "chkBxSuncar";
            this.chkBxSuncar.Size = new System.Drawing.Size(134, 23);
            this.chkBxSuncar.TabIndex = 35;
            this.chkBxSuncar.Text = "サンミックカーペット";
            this.chkBxSuncar.UseVisualStyleBackColor = true;
            this.chkBxSuncar.CheckedChanged += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // chkBxOhno
            // 
            this.chkBxOhno.AutoSize = true;
            this.chkBxOhno.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxOhno.Location = new System.Drawing.Point(11, 75);
            this.chkBxOhno.Name = "chkBxOhno";
            this.chkBxOhno.Size = new System.Drawing.Size(61, 23);
            this.chkBxOhno.TabIndex = 32;
            this.chkBxOhno.Text = "オーノ";
            this.chkBxOhno.UseVisualStyleBackColor = true;
            this.chkBxOhno.CheckedChanged += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // chkBxSundus
            // 
            this.chkBxSundus.AutoSize = true;
            this.chkBxSundus.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSundus.Location = new System.Drawing.Point(11, 116);
            this.chkBxSundus.Name = "chkBxSundus";
            this.chkBxSundus.Size = new System.Drawing.Size(122, 23);
            this.chkBxSundus.TabIndex = 33;
            this.chkBxSundus.Text = "サンミックダスコン";
            this.chkBxSundus.UseVisualStyleBackColor = true;
            this.chkBxSundus.CheckedChanged += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbCompany.Location = new System.Drawing.Point(6, 42);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(48, 17);
            this.lbCompany.TabIndex = 34;
            this.lbCompany.Text = "【会社】";
            // 
            // lbBumon
            // 
            this.lbBumon.AutoSize = true;
            this.lbBumon.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbBumon.Location = new System.Drawing.Point(232, 42);
            this.lbBumon.Name = "lbBumon";
            this.lbBumon.Size = new System.Drawing.Size(48, 17);
            this.lbBumon.TabIndex = 36;
            this.lbBumon.Text = "【部門】";
            // 
            // lbSymbol1
            // 
            this.lbSymbol1.AutoSize = true;
            this.lbSymbol1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSymbol1.Location = new System.Drawing.Point(135, 32);
            this.lbSymbol1.Name = "lbSymbol1";
            this.lbSymbol1.Size = new System.Drawing.Size(24, 19);
            this.lbSymbol1.TabIndex = 40;
            this.lbSymbol1.Text = "～";
            // 
            // chkBxProAll
            // 
            this.chkBxProAll.AutoSize = true;
            this.chkBxProAll.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxProAll.Location = new System.Drawing.Point(359, 89);
            this.chkBxProAll.Name = "chkBxProAll";
            this.chkBxProAll.Size = new System.Drawing.Size(54, 23);
            this.chkBxProAll.TabIndex = 51;
            this.chkBxProAll.Text = "ALL";
            this.chkBxProAll.UseVisualStyleBackColor = true;
            this.chkBxProAll.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // chkBxSemiFinProducts
            // 
            this.chkBxSemiFinProducts.AutoSize = true;
            this.chkBxSemiFinProducts.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSemiFinProducts.Location = new System.Drawing.Point(245, 89);
            this.chkBxSemiFinProducts.Name = "chkBxSemiFinProducts";
            this.chkBxSemiFinProducts.Size = new System.Drawing.Size(73, 23);
            this.chkBxSemiFinProducts.TabIndex = 50;
            this.chkBxSemiFinProducts.Text = "半製品";
            this.chkBxSemiFinProducts.UseVisualStyleBackColor = true;
            this.chkBxSemiFinProducts.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxRawMaterials
            // 
            this.chkBxRawMaterials.AutoSize = true;
            this.chkBxRawMaterials.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxRawMaterials.Location = new System.Drawing.Point(245, 59);
            this.chkBxRawMaterials.Name = "chkBxRawMaterials";
            this.chkBxRawMaterials.Size = new System.Drawing.Size(73, 23);
            this.chkBxRawMaterials.TabIndex = 49;
            this.chkBxRawMaterials.Text = "原材料";
            this.chkBxRawMaterials.UseVisualStyleBackColor = true;
            this.chkBxRawMaterials.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // chkBxProduct
            // 
            this.chkBxProduct.AutoSize = true;
            this.chkBxProduct.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxProduct.Location = new System.Drawing.Point(245, 118);
            this.chkBxProduct.Name = "chkBxProduct";
            this.chkBxProduct.Size = new System.Drawing.Size(58, 23);
            this.chkBxProduct.TabIndex = 48;
            this.chkBxProduct.Text = "製品";
            this.chkBxProduct.UseVisualStyleBackColor = true;
            this.chkBxProduct.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // lbProductClass
            // 
            this.lbProductClass.AutoSize = true;
            this.lbProductClass.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbProductClass.Location = new System.Drawing.Point(241, 32);
            this.lbProductClass.Name = "lbProductClass";
            this.lbProductClass.Size = new System.Drawing.Size(74, 17);
            this.lbProductClass.TabIndex = 47;
            this.lbProductClass.Text = "【商品区分】";
            // 
            // lbSalesCategory
            // 
            this.lbSalesCategory.AutoSize = true;
            this.lbSalesCategory.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSalesCategory.Location = new System.Drawing.Point(33, 32);
            this.lbSalesCategory.Name = "lbSalesCategory";
            this.lbSalesCategory.Size = new System.Drawing.Size(74, 17);
            this.lbSalesCategory.TabIndex = 46;
            this.lbSalesCategory.Text = "【販売区分】";
            // 
            // chkBxSl
            // 
            this.chkBxSl.AutoSize = true;
            this.chkBxSl.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSl.Location = new System.Drawing.Point(36, 59);
            this.chkBxSl.Name = "chkBxSl";
            this.chkBxSl.Size = new System.Drawing.Size(58, 23);
            this.chkBxSl.TabIndex = 42;
            this.chkBxSl.Text = "売上";
            this.chkBxSl.UseVisualStyleBackColor = true;
            this.chkBxSl.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxPr
            // 
            this.chkBxPr.AutoSize = true;
            this.chkBxPr.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxPr.Location = new System.Drawing.Point(36, 89);
            this.chkBxPr.Name = "chkBxPr";
            this.chkBxPr.Size = new System.Drawing.Size(58, 23);
            this.chkBxPr.TabIndex = 43;
            this.chkBxPr.Text = "仕入";
            this.chkBxPr.UseVisualStyleBackColor = true;
            this.chkBxPr.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxIv
            // 
            this.chkBxIv.AutoSize = true;
            this.chkBxIv.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxIv.Location = new System.Drawing.Point(36, 118);
            this.chkBxIv.Name = "chkBxIv";
            this.chkBxIv.Size = new System.Drawing.Size(58, 23);
            this.chkBxIv.TabIndex = 44;
            this.chkBxIv.Text = "在庫";
            this.chkBxIv.UseVisualStyleBackColor = true;
            this.chkBxIv.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxSalesAll
            // 
            this.chkBxSalesAll.AutoSize = true;
            this.chkBxSalesAll.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSalesAll.Location = new System.Drawing.Point(117, 89);
            this.chkBxSalesAll.Name = "chkBxSalesAll";
            this.chkBxSalesAll.Size = new System.Drawing.Size(54, 23);
            this.chkBxSalesAll.TabIndex = 45;
            this.chkBxSalesAll.Text = "ALL";
            this.chkBxSalesAll.UseVisualStyleBackColor = true;
            this.chkBxSalesAll.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // lbSupplier
            // 
            this.lbSupplier.AutoSize = true;
            this.lbSupplier.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSupplier.Location = new System.Drawing.Point(241, 215);
            this.lbSupplier.Name = "lbSupplier";
            this.lbSupplier.Size = new System.Drawing.Size(61, 17);
            this.lbSupplier.TabIndex = 56;
            this.lbSupplier.Text = "【仕入先】";
            // 
            // lbSaller
            // 
            this.lbSaller.AutoSize = true;
            this.lbSaller.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSaller.Location = new System.Drawing.Point(3, 215);
            this.lbSaller.Name = "lbSaller";
            this.lbSaller.Size = new System.Drawing.Size(61, 17);
            this.lbSaller.TabIndex = 55;
            this.lbSaller.Text = "【販売先】";
            // 
            // lbDataOhno
            // 
            this.lbDataOhno.AutoSize = true;
            this.lbDataOhno.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbDataOhno.Location = new System.Drawing.Point(500, 9);
            this.lbDataOhno.Name = "lbDataOhno";
            this.lbDataOhno.Size = new System.Drawing.Size(103, 17);
            this.lbDataOhno.TabIndex = 59;
            this.lbDataOhno.Text = "<オーノ(売・仕)>";
            // 
            // lbDataSuncar
            // 
            this.lbDataSuncar.AutoSize = true;
            this.lbDataSuncar.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbDataSuncar.Location = new System.Drawing.Point(500, 402);
            this.lbDataSuncar.Name = "lbDataSuncar";
            this.lbDataSuncar.Size = new System.Drawing.Size(167, 17);
            this.lbDataSuncar.TabIndex = 60;
            this.lbDataSuncar.Text = "<サンミックカーペット(売・仕)>";
            // 
            // lbDataSundus
            // 
            this.lbDataSundus.AutoSize = true;
            this.lbDataSundus.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbDataSundus.Location = new System.Drawing.Point(500, 206);
            this.lbDataSundus.Name = "lbDataSundus";
            this.lbDataSundus.Size = new System.Drawing.Size(157, 17);
            this.lbDataSundus.TabIndex = 61;
            this.lbDataSundus.Text = "<サンミックダスコン(売・仕)>";
            // 
            // lbDataIV
            // 
            this.lbDataIV.AutoSize = true;
            this.lbDataIV.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbDataIV.Location = new System.Drawing.Point(500, 592);
            this.lbDataIV.Name = "lbDataIV";
            this.lbDataIV.Size = new System.Drawing.Size(54, 17);
            this.lbDataIV.TabIndex = 62;
            this.lbDataIV.Text = "<在庫>";
            // 
            // btnDisplay
            // 
            this.btnDisplay.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDisplay.Location = new System.Drawing.Point(12, 13);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(108, 35);
            this.btnDisplay.TabIndex = 63;
            this.btnDisplay.Text = "データ表示実行";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnForm1Back
            // 
            this.btnForm1Back.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnForm1Back.Location = new System.Drawing.Point(138, 13);
            this.btnForm1Back.Name = "btnForm1Back";
            this.btnForm1Back.Size = new System.Drawing.Size(108, 35);
            this.btnForm1Back.TabIndex = 64;
            this.btnForm1Back.Text = "戻る";
            this.btnForm1Back.UseVisualStyleBackColor = true;
            this.btnForm1Back.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // grpBxBtn
            // 
            this.grpBxBtn.Controls.Add(this.btnDisplay);
            this.grpBxBtn.Controls.Add(this.btnForm1Back);
            this.grpBxBtn.Location = new System.Drawing.Point(218, 26);
            this.grpBxBtn.Name = "grpBxBtn";
            this.grpBxBtn.Size = new System.Drawing.Size(258, 55);
            this.grpBxBtn.TabIndex = 65;
            this.grpBxBtn.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBxEndYearMonth);
            this.groupBox1.Controls.Add(this.txtBxStrYearMonth);
            this.groupBox1.Controls.Add(this.lbSymbol1);
            this.groupBox1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(95, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 62);
            this.groupBox1.TabIndex = 66;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "〚 表示期間 〛";
            // 
            // txtBxEndYearMonth
            // 
            this.txtBxEndYearMonth.Location = new System.Drawing.Point(170, 26);
            this.txtBxEndYearMonth.Name = "txtBxEndYearMonth";
            this.txtBxEndYearMonth.Size = new System.Drawing.Size(106, 27);
            this.txtBxEndYearMonth.TabIndex = 72;
            this.txtBxEndYearMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBxYearMonth_KeyPress);
            // 
            // txtBxStrYearMonth
            // 
            this.txtBxStrYearMonth.Location = new System.Drawing.Point(15, 26);
            this.txtBxStrYearMonth.Name = "txtBxStrYearMonth";
            this.txtBxStrYearMonth.Size = new System.Drawing.Size(106, 27);
            this.txtBxStrYearMonth.TabIndex = 71;
            this.txtBxStrYearMonth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBxYearMonth_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBxSupplier);
            this.groupBox2.Controls.Add(this.listBxSaller);
            this.groupBox2.Controls.Add(this.listBxBumon);
            this.groupBox2.Controls.Add(this.lbCompany);
            this.groupBox2.Controls.Add(this.chkBxSundus);
            this.groupBox2.Controls.Add(this.chkBxOhno);
            this.groupBox2.Controls.Add(this.chkBxSuncar);
            this.groupBox2.Controls.Add(this.lbBumon);
            this.groupBox2.Controls.Add(this.lbSaller);
            this.groupBox2.Controls.Add(this.lbSupplier);
            this.groupBox2.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox2.Location = new System.Drawing.Point(12, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(474, 428);
            this.groupBox2.TabIndex = 67;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "〚 組織 〛";
            // 
            // listBxSupplier
            // 
            this.listBxSupplier.FormattingEnabled = true;
            this.listBxSupplier.ItemHeight = 19;
            this.listBxSupplier.Location = new System.Drawing.Point(244, 245);
            this.listBxSupplier.Name = "listBxSupplier";
            this.listBxSupplier.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBxSupplier.Size = new System.Drawing.Size(219, 175);
            this.listBxSupplier.TabIndex = 61;
            // 
            // listBxSaller
            // 
            this.listBxSaller.FormattingEnabled = true;
            this.listBxSaller.ItemHeight = 19;
            this.listBxSaller.Location = new System.Drawing.Point(9, 245);
            this.listBxSaller.Name = "listBxSaller";
            this.listBxSaller.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBxSaller.Size = new System.Drawing.Size(220, 175);
            this.listBxSaller.TabIndex = 60;
            // 
            // listBxBumon
            // 
            this.listBxBumon.FormattingEnabled = true;
            this.listBxBumon.ItemHeight = 19;
            this.listBxBumon.Location = new System.Drawing.Point(218, 75);
            this.listBxBumon.Name = "listBxBumon";
            this.listBxBumon.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBxBumon.Size = new System.Drawing.Size(221, 118);
            this.listBxBumon.TabIndex = 59;
            this.listBxBumon.SelectedIndexChanged += new System.EventHandler(this.listBxBumon_selectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkBxProcess);
            this.groupBox3.Controls.Add(this.lbSalesCategory);
            this.groupBox3.Controls.Add(this.chkBxSalesAll);
            this.groupBox3.Controls.Add(this.chkBxIv);
            this.groupBox3.Controls.Add(this.chkBxPr);
            this.groupBox3.Controls.Add(this.chkBxSl);
            this.groupBox3.Controls.Add(this.lbProductClass);
            this.groupBox3.Controls.Add(this.chkBxProduct);
            this.groupBox3.Controls.Add(this.chkBxRawMaterials);
            this.groupBox3.Controls.Add(this.chkBxSemiFinProducts);
            this.groupBox3.Controls.Add(this.chkBxProAll);
            this.groupBox3.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox3.Location = new System.Drawing.Point(12, 598);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(474, 180);
            this.groupBox3.TabIndex = 68;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "〚 データ 〛";
            // 
            // chkBxProcess
            // 
            this.chkBxProcess.AutoSize = true;
            this.chkBxProcess.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxProcess.Location = new System.Drawing.Point(245, 147);
            this.chkBxProcess.Name = "chkBxProcess";
            this.chkBxProcess.Size = new System.Drawing.Size(58, 23);
            this.chkBxProcess.TabIndex = 55;
            this.chkBxProcess.Text = "加工";
            this.chkBxProcess.UseVisualStyleBackColor = true;
            this.chkBxProcess.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // lbTitleDisplay
            // 
            this.lbTitleDisplay.AutoSize = true;
            this.lbTitleDisplay.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitleDisplay.Location = new System.Drawing.Point(48, 16);
            this.lbTitleDisplay.Name = "lbTitleDisplay";
            this.lbTitleDisplay.Size = new System.Drawing.Size(96, 17);
            this.lbTitleDisplay.TabIndex = 69;
            this.lbTitleDisplay.Text = "< データ表示 >";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::あすよん月次帳票.Properties.Resources.ic_all_csm02;
            this.pictureBox1.Location = new System.Drawing.Point(15, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 70;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(104, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(267, 15);
            this.label2.TabIndex = 71;
            this.label2.Text = "●表示期間はyyyymmの6桁年月で指定してください";
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(1358, 6);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(54, 20);
            this.btnMin.TabIndex = 72;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 793);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbTitleDisplay);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpBxBtn);
            this.Controls.Add(this.lbDataIV);
            this.Controls.Add(this.lbDataSundus);
            this.Controls.Add(this.lbDataSuncar);
            this.Controls.Add(this.lbDataOhno);
            this.Controls.Add(this.dgvDataIV);
            this.Controls.Add(this.dgvDataSdus);
            this.Controls.Add(this.dgvDataScar);
            this.Controls.Add(this.dgvDataOhno);
            this.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form2";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "データ表示";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataOhno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataScar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSdus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataIV)).EndInit();
            this.grpBxBtn.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDataOhno;
        private System.Windows.Forms.DataGridView dgvDataScar;
        private System.Windows.Forms.DataGridView dgvDataSdus;
        private System.Windows.Forms.DataGridView dgvDataIV;

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



        private CheckBox chkBxSuncar;
        private CheckBox chkBxOhno;
        private CheckBox chkBxSundus;
        private Label lbCompany;
        private Label lbBumon;
        private Label lbSymbol1;
        private CheckBox chkBxProAll;
        private CheckBox chkBxSemiFinProducts;
        private CheckBox chkBxRawMaterials;
        private CheckBox chkBxProduct;
        private Label lbProductClass;
        private Label lbSalesCategory;
        private CheckBox chkBxSl;
        private CheckBox chkBxPr;
        private CheckBox chkBxIv;
        private CheckBox chkBxSalesAll;
        private Label lbSupplier;
        private Label lbSaller;
        private Label lbDataOhno;
        private Label lbDataSuncar;
        private Label lbDataSundus;
        private Label lbDataIV;
        private Button btnDisplay;
        private Button btnForm1Back;
        private GroupBox grpBxBtn;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private CheckBox chkBxProcess;

        private void StyleButton(Button btn, Color backColor, Color foreColor, Color? borderColor = null, int radius = 12)
        {
            // -- ボタンのスタイル設定 --
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // 枠線はPaintで描画
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Meiryo UI", 9.75F, FontStyle.Bold);

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

        private Label lbTitleDisplay;
        private PictureBox pictureBox1;
        private ListBox listBxBumon;
        private ListBox listBxSupplier;
        private ListBox listBxSaller;
        private TextBox txtBxStrYearMonth;
        private TextBox txtBxEndYearMonth;
        private Label label2;
        private Button btnMin;
    }
}