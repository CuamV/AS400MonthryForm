using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.lbSituation = new System.Windows.Forms.Label();
            this.listBxSituation = new System.Windows.Forms.ListBox();
            this.grpBxCondition = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBxEndYearMonth = new System.Windows.Forms.TextBox();
            this.txtBxStrYearMonth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBxData = new System.Windows.Forms.GroupBox();
            this.chkBxProcess = new System.Windows.Forms.CheckBox();
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
            this.grpBxOrganization = new System.Windows.Forms.GroupBox();
            this.listBxSupplier = new System.Windows.Forms.ListBox();
            this.listBxSaller = new System.Windows.Forms.ListBox();
            this.listBxBumon = new System.Windows.Forms.ListBox();
            this.chkBxSuncar = new System.Windows.Forms.CheckBox();
            this.lbSupplier = new System.Windows.Forms.Label();
            this.lbSaller = new System.Windows.Forms.Label();
            this.lbBumon = new System.Windows.Forms.Label();
            this.chkBxOhno = new System.Windows.Forms.CheckBox();
            this.chkBxSundus = new System.Windows.Forms.CheckBox();
            this.lbCompany = new System.Windows.Forms.Label();
            this.lbYearMonth = new System.Windows.Forms.Label();
            this.btnForm1Back = new System.Windows.Forms.Button();
            this.lbTitleExportExcel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMin = new System.Windows.Forms.Button();
            this.chkBxCustody = new System.Windows.Forms.CheckBox();
            this.grpBxCondition.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpBxData.SuspendLayout();
            this.grpBxOrganization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnExportExcel.Location = new System.Drawing.Point(562, 85);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(121, 35);
            this.btnExportExcel.TabIndex = 26;
            this.btnExportExcel.Text = "Excelエクスポート";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // lbSituation
            // 
            this.lbSituation.AutoSize = true;
            this.lbSituation.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSituation.Location = new System.Drawing.Point(146, 35);
            this.lbSituation.Name = "lbSituation";
            this.lbSituation.Size = new System.Drawing.Size(125, 17);
            this.lbSituation.TabIndex = 28;
            this.lbSituation.Text = "【シュミレーション状況】";
            // 
            // listBxSituation
            // 
            this.listBxSituation.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxSituation.FormattingEnabled = true;
            this.listBxSituation.ItemHeight = 15;
            this.listBxSituation.Location = new System.Drawing.Point(149, 55);
            this.listBxSituation.Name = "listBxSituation";
            this.listBxSituation.Size = new System.Drawing.Size(369, 94);
            this.listBxSituation.TabIndex = 27;
            // 
            // grpBxCondition
            // 
            this.grpBxCondition.Controls.Add(this.label2);
            this.grpBxCondition.Controls.Add(this.groupBox1);
            this.grpBxCondition.Controls.Add(this.grpBxData);
            this.grpBxCondition.Controls.Add(this.grpBxOrganization);
            this.grpBxCondition.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxCondition.Location = new System.Drawing.Point(15, 155);
            this.grpBxCondition.Name = "grpBxCondition";
            this.grpBxCondition.Size = new System.Drawing.Size(917, 513);
            this.grpBxCondition.TabIndex = 29;
            this.grpBxCondition.TabStop = false;
            this.grpBxCondition.Text = "条件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(568, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(287, 17);
            this.label2.TabIndex = 68;
            this.label2.Text = "●取得期間はyyyymmの6桁年月で指定してください";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBxEndYearMonth);
            this.groupBox1.Controls.Add(this.txtBxStrYearMonth);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(571, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 62);
            this.groupBox1.TabIndex = 67;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "〚 取得期間 〛";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(135, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 19);
            this.label1.TabIndex = 40;
            this.label1.Text = "～";
            // 
            // grpBxData
            // 
            this.grpBxData.Controls.Add(this.chkBxCustody);
            this.grpBxData.Controls.Add(this.chkBxProcess);
            this.grpBxData.Controls.Add(this.chkBxProAll);
            this.grpBxData.Controls.Add(this.chkBxSemiFinProducts);
            this.grpBxData.Controls.Add(this.chkBxRawMaterials);
            this.grpBxData.Controls.Add(this.chkBxProduct);
            this.grpBxData.Controls.Add(this.lbProductClass);
            this.grpBxData.Controls.Add(this.lbSalesCategory);
            this.grpBxData.Controls.Add(this.chkBxSl);
            this.grpBxData.Controls.Add(this.chkBxPr);
            this.grpBxData.Controls.Add(this.chkBxIv);
            this.grpBxData.Controls.Add(this.chkBxSalesAll);
            this.grpBxData.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxData.Location = new System.Drawing.Point(538, 218);
            this.grpBxData.Name = "grpBxData";
            this.grpBxData.Size = new System.Drawing.Size(364, 278);
            this.grpBxData.TabIndex = 29;
            this.grpBxData.TabStop = false;
            this.grpBxData.Text = "データ";
            // 
            // chkBxProcess
            // 
            this.chkBxProcess.AutoSize = true;
            this.chkBxProcess.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxProcess.Location = new System.Drawing.Point(292, 195);
            this.chkBxProcess.Name = "chkBxProcess";
            this.chkBxProcess.Size = new System.Drawing.Size(58, 23);
            this.chkBxProcess.TabIndex = 56;
            this.chkBxProcess.Text = "加工";
            this.chkBxProcess.UseVisualStyleBackColor = true;
            this.chkBxProcess.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // chkBxProAll
            // 
            this.chkBxProAll.AutoSize = true;
            this.chkBxProAll.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxProAll.Location = new System.Drawing.Point(120, 242);
            this.chkBxProAll.Name = "chkBxProAll";
            this.chkBxProAll.Size = new System.Drawing.Size(54, 23);
            this.chkBxProAll.TabIndex = 29;
            this.chkBxProAll.Text = "ALL";
            this.chkBxProAll.UseVisualStyleBackColor = true;
            this.chkBxProAll.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // chkBxSemiFinProducts
            // 
            this.chkBxSemiFinProducts.AutoSize = true;
            this.chkBxSemiFinProducts.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSemiFinProducts.Location = new System.Drawing.Point(120, 195);
            this.chkBxSemiFinProducts.Name = "chkBxSemiFinProducts";
            this.chkBxSemiFinProducts.Size = new System.Drawing.Size(73, 23);
            this.chkBxSemiFinProducts.TabIndex = 28;
            this.chkBxSemiFinProducts.Text = "半製品";
            this.chkBxSemiFinProducts.UseVisualStyleBackColor = true;
            this.chkBxSemiFinProducts.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // chkBxRawMaterials
            // 
            this.chkBxRawMaterials.AutoSize = true;
            this.chkBxRawMaterials.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxRawMaterials.Location = new System.Drawing.Point(33, 195);
            this.chkBxRawMaterials.Name = "chkBxRawMaterials";
            this.chkBxRawMaterials.Size = new System.Drawing.Size(73, 23);
            this.chkBxRawMaterials.TabIndex = 27;
            this.chkBxRawMaterials.Text = "原材料";
            this.chkBxRawMaterials.UseVisualStyleBackColor = true;
            this.chkBxRawMaterials.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // chkBxProduct
            // 
            this.chkBxProduct.AutoSize = true;
            this.chkBxProduct.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxProduct.Location = new System.Drawing.Point(211, 195);
            this.chkBxProduct.Name = "chkBxProduct";
            this.chkBxProduct.Size = new System.Drawing.Size(58, 23);
            this.chkBxProduct.TabIndex = 26;
            this.chkBxProduct.Text = "製品";
            this.chkBxProduct.UseVisualStyleBackColor = true;
            this.chkBxProduct.CheckedChanged += new System.EventHandler(this.chkProductType);
            // 
            // lbProductClass
            // 
            this.lbProductClass.AutoSize = true;
            this.lbProductClass.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbProductClass.Location = new System.Drawing.Point(30, 162);
            this.lbProductClass.Name = "lbProductClass";
            this.lbProductClass.Size = new System.Drawing.Size(74, 17);
            this.lbProductClass.TabIndex = 25;
            this.lbProductClass.Text = "【商品区分】";
            // 
            // lbSalesCategory
            // 
            this.lbSalesCategory.AutoSize = true;
            this.lbSalesCategory.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSalesCategory.Location = new System.Drawing.Point(30, 33);
            this.lbSalesCategory.Name = "lbSalesCategory";
            this.lbSalesCategory.Size = new System.Drawing.Size(74, 17);
            this.lbSalesCategory.TabIndex = 24;
            this.lbSalesCategory.Text = "【販売区分】";
            // 
            // chkBxSl
            // 
            this.chkBxSl.AutoSize = true;
            this.chkBxSl.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSl.Location = new System.Drawing.Point(36, 66);
            this.chkBxSl.Name = "chkBxSl";
            this.chkBxSl.Size = new System.Drawing.Size(58, 23);
            this.chkBxSl.TabIndex = 17;
            this.chkBxSl.Text = "売上";
            this.chkBxSl.UseVisualStyleBackColor = true;
            this.chkBxSl.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxPr
            // 
            this.chkBxPr.AutoSize = true;
            this.chkBxPr.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxPr.Location = new System.Drawing.Point(119, 66);
            this.chkBxPr.Name = "chkBxPr";
            this.chkBxPr.Size = new System.Drawing.Size(58, 23);
            this.chkBxPr.TabIndex = 18;
            this.chkBxPr.Text = "仕入";
            this.chkBxPr.UseVisualStyleBackColor = true;
            this.chkBxPr.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxIv
            // 
            this.chkBxIv.AutoSize = true;
            this.chkBxIv.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxIv.Location = new System.Drawing.Point(200, 66);
            this.chkBxIv.Name = "chkBxIv";
            this.chkBxIv.Size = new System.Drawing.Size(58, 23);
            this.chkBxIv.TabIndex = 19;
            this.chkBxIv.Text = "在庫";
            this.chkBxIv.UseVisualStyleBackColor = true;
            this.chkBxIv.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // chkBxSalesAll
            // 
            this.chkBxSalesAll.AutoSize = true;
            this.chkBxSalesAll.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSalesAll.Location = new System.Drawing.Point(36, 104);
            this.chkBxSalesAll.Name = "chkBxSalesAll";
            this.chkBxSalesAll.Size = new System.Drawing.Size(54, 23);
            this.chkBxSalesAll.TabIndex = 20;
            this.chkBxSalesAll.Text = "ALL";
            this.chkBxSalesAll.UseVisualStyleBackColor = true;
            this.chkBxSalesAll.CheckedChanged += new System.EventHandler(this.chkDataType);
            // 
            // grpBxOrganization
            // 
            this.grpBxOrganization.Controls.Add(this.listBxSupplier);
            this.grpBxOrganization.Controls.Add(this.listBxSaller);
            this.grpBxOrganization.Controls.Add(this.listBxBumon);
            this.grpBxOrganization.Controls.Add(this.chkBxSuncar);
            this.grpBxOrganization.Controls.Add(this.lbSupplier);
            this.grpBxOrganization.Controls.Add(this.lbSaller);
            this.grpBxOrganization.Controls.Add(this.lbBumon);
            this.grpBxOrganization.Controls.Add(this.chkBxOhno);
            this.grpBxOrganization.Controls.Add(this.chkBxSundus);
            this.grpBxOrganization.Controls.Add(this.lbCompany);
            this.grpBxOrganization.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBxOrganization.Location = new System.Drawing.Point(20, 36);
            this.grpBxOrganization.Name = "grpBxOrganization";
            this.grpBxOrganization.Size = new System.Drawing.Size(492, 460);
            this.grpBxOrganization.TabIndex = 28;
            this.grpBxOrganization.TabStop = false;
            this.grpBxOrganization.Text = "組織";
            // 
            // listBxSupplier
            // 
            this.listBxSupplier.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxSupplier.FormattingEnabled = true;
            this.listBxSupplier.ItemHeight = 17;
            this.listBxSupplier.Location = new System.Drawing.Point(256, 307);
            this.listBxSupplier.Name = "listBxSupplier";
            this.listBxSupplier.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBxSupplier.Size = new System.Drawing.Size(219, 140);
            this.listBxSupplier.TabIndex = 63;
            // 
            // listBxSaller
            // 
            this.listBxSaller.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxSaller.FormattingEnabled = true;
            this.listBxSaller.ItemHeight = 17;
            this.listBxSaller.Location = new System.Drawing.Point(16, 307);
            this.listBxSaller.Name = "listBxSaller";
            this.listBxSaller.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBxSaller.Size = new System.Drawing.Size(220, 140);
            this.listBxSaller.TabIndex = 62;
            // 
            // listBxBumon
            // 
            this.listBxBumon.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxBumon.FormattingEnabled = true;
            this.listBxBumon.ItemHeight = 17;
            this.listBxBumon.Location = new System.Drawing.Point(20, 148);
            this.listBxBumon.Name = "listBxBumon";
            this.listBxBumon.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBxBumon.Size = new System.Drawing.Size(221, 106);
            this.listBxBumon.TabIndex = 60;
            this.listBxBumon.SelectedIndexChanged += new System.EventHandler(this.listBoxBumon_SelectedIndexChanged);
            // 
            // chkBxSuncar
            // 
            this.chkBxSuncar.AutoSize = true;
            this.chkBxSuncar.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSuncar.Location = new System.Drawing.Point(247, 60);
            this.chkBxSuncar.Name = "chkBxSuncar";
            this.chkBxSuncar.Size = new System.Drawing.Size(134, 23);
            this.chkBxSuncar.TabIndex = 31;
            this.chkBxSuncar.Text = "サンミックカーペット";
            this.chkBxSuncar.UseVisualStyleBackColor = true;
            this.chkBxSuncar.Click += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // lbSupplier
            // 
            this.lbSupplier.AutoSize = true;
            this.lbSupplier.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSupplier.Location = new System.Drawing.Point(253, 283);
            this.lbSupplier.Name = "lbSupplier";
            this.lbSupplier.Size = new System.Drawing.Size(61, 17);
            this.lbSupplier.TabIndex = 28;
            this.lbSupplier.Text = "【仕入先】";
            // 
            // lbSaller
            // 
            this.lbSaller.AutoSize = true;
            this.lbSaller.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSaller.Location = new System.Drawing.Point(17, 283);
            this.lbSaller.Name = "lbSaller";
            this.lbSaller.Size = new System.Drawing.Size(61, 17);
            this.lbSaller.TabIndex = 26;
            this.lbSaller.Text = "【販売先】";
            // 
            // lbBumon
            // 
            this.lbBumon.AutoSize = true;
            this.lbBumon.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbBumon.Location = new System.Drawing.Point(17, 128);
            this.lbBumon.Name = "lbBumon";
            this.lbBumon.Size = new System.Drawing.Size(48, 17);
            this.lbBumon.TabIndex = 17;
            this.lbBumon.Text = "【部門】";
            // 
            // chkBxOhno
            // 
            this.chkBxOhno.AutoSize = true;
            this.chkBxOhno.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxOhno.Location = new System.Drawing.Point(20, 60);
            this.chkBxOhno.Name = "chkBxOhno";
            this.chkBxOhno.Size = new System.Drawing.Size(61, 23);
            this.chkBxOhno.TabIndex = 11;
            this.chkBxOhno.Text = "オーノ";
            this.chkBxOhno.UseVisualStyleBackColor = true;
            this.chkBxOhno.Click += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // chkBxSundus
            // 
            this.chkBxSundus.AutoSize = true;
            this.chkBxSundus.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSundus.Location = new System.Drawing.Point(108, 60);
            this.chkBxSundus.Name = "chkBxSundus";
            this.chkBxSundus.Size = new System.Drawing.Size(122, 23);
            this.chkBxSundus.TabIndex = 12;
            this.chkBxSundus.Text = "サンミックダスコン";
            this.chkBxSundus.UseVisualStyleBackColor = true;
            this.chkBxSundus.Click += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbCompany.Location = new System.Drawing.Point(17, 40);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(48, 17);
            this.lbCompany.TabIndex = 22;
            this.lbCompany.Text = "【会社】";
            // 
            // lbYearMonth
            // 
            this.lbYearMonth.AutoSize = true;
            this.lbYearMonth.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbYearMonth.Location = new System.Drawing.Point(558, 55);
            this.lbYearMonth.Name = "lbYearMonth";
            this.lbYearMonth.Size = new System.Drawing.Size(61, 20);
            this.lbYearMonth.TabIndex = 9;
            this.lbYearMonth.Text = "【年月】";
            // 
            // btnForm1Back
            // 
            this.btnForm1Back.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnForm1Back.Location = new System.Drawing.Point(703, 85);
            this.btnForm1Back.Name = "btnForm1Back";
            this.btnForm1Back.Size = new System.Drawing.Size(108, 35);
            this.btnForm1Back.TabIndex = 66;
            this.btnForm1Back.Text = "戻る";
            this.btnForm1Back.UseVisualStyleBackColor = true;
            this.btnForm1Back.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // lbTitleExportExcel
            // 
            this.lbTitleExportExcel.AutoSize = true;
            this.lbTitleExportExcel.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitleExportExcel.Location = new System.Drawing.Point(48, 17);
            this.lbTitleExportExcel.Name = "lbTitleExportExcel";
            this.lbTitleExportExcel.Size = new System.Drawing.Size(102, 17);
            this.lbTitleExportExcel.TabIndex = 73;
            this.lbTitleExportExcel.Text = "< エクスポート >";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::あすよん月次帳票.Properties.Resources.ic_all_csm02;
            this.pictureBox1.Location = new System.Drawing.Point(13, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 72;
            this.pictureBox1.TabStop = false;
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(899, 12);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(39, 20);
            this.btnMin.TabIndex = 74;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // chkBxCustody
            // 
            this.chkBxCustody.AutoSize = true;
            this.chkBxCustody.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxCustody.Location = new System.Drawing.Point(33, 242);
            this.chkBxCustody.Name = "chkBxCustody";
            this.chkBxCustody.Size = new System.Drawing.Size(52, 23);
            this.chkBxCustody.TabIndex = 57;
            this.chkBxCustody.Text = "預り";
            this.chkBxCustody.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 680);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.lbTitleExportExcel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbYearMonth);
            this.Controls.Add(this.btnForm1Back);
            this.Controls.Add(this.grpBxCondition);
            this.Controls.Add(this.lbSituation);
            this.Controls.Add(this.listBxSituation);
            this.Controls.Add(this.btnExportExcel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form4";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "エクスポート";
            this.grpBxCondition.ResumeLayout(false);
            this.grpBxCondition.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpBxData.ResumeLayout(false);
            this.grpBxData.PerformLayout();
            this.grpBxOrganization.ResumeLayout(false);
            this.grpBxOrganization.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Label lbSituation;
        private System.Windows.Forms.ListBox listBxSituation;
        private System.Windows.Forms.GroupBox grpBxCondition;
        private System.Windows.Forms.GroupBox grpBxData;
        private System.Windows.Forms.CheckBox chkBxProAll;
        private System.Windows.Forms.CheckBox chkBxSemiFinProducts;
        private System.Windows.Forms.CheckBox chkBxRawMaterials;
        private System.Windows.Forms.CheckBox chkBxProduct;
        private System.Windows.Forms.Label lbProductClass;
        private System.Windows.Forms.Label lbSalesCategory;
        private System.Windows.Forms.CheckBox chkBxSl;
        private System.Windows.Forms.CheckBox chkBxPr;
        private System.Windows.Forms.CheckBox chkBxIv;
        private System.Windows.Forms.CheckBox chkBxSalesAll;
        private System.Windows.Forms.GroupBox grpBxOrganization;
        private System.Windows.Forms.CheckBox chkBxSuncar;
        private System.Windows.Forms.Label lbSupplier;
        private System.Windows.Forms.Label lbSaller;
        private System.Windows.Forms.Label lbBumon;
        private System.Windows.Forms.CheckBox chkBxOhno;
        private System.Windows.Forms.CheckBox chkBxSundus;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.Label lbYearMonth;
        private System.Windows.Forms.Button btnForm1Back;
        private System.Windows.Forms.CheckBox chkBxProcess;

        private void StyleButton(Button btn, Color backColor, Color foreColor, Color? borderColor = null, int radius = 12)
        {
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

        private PictureBox pictureBox1;
        private Label lbTitleExportExcel;
        private ListBox listBxBumon;
        private ListBox listBxSupplier;
        private ListBox listBxSaller;
        private GroupBox groupBox1;
        private TextBox txtBxEndYearMonth;
        private TextBox txtBxStrYearMonth;
        private Label label1;
        private Label label2;
        private Button btnMin;
        private CheckBox chkBxCustody;
    }
}