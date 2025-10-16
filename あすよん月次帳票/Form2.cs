using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class Form2 : Form
    {
        private string HIZTIM;
        private string startDate;
        private string endDate;

        FormActionMethod formActionMethod = new FormActionMethod();

        public Form2()
        {
            InitializeComponent();

            groupBox1.Paint += GroupBoxCustomBorder;
            groupBox2.Paint += GroupBoxCustomBorder;
            groupBox3.Paint += GroupBoxCustomBorder;
            grpBxBtn.Paint += GroupBoxCustomBorder;

            this.Load += Form2_Load;

            // DataGridViewスタイル設定
            StyleDataGrid(dgvDataOhno, Color.DarkBlue, Color.White, Color.LightGray);
            StyleDataGrid(dgvDataSdus, Color.DarkGreen, Color.White, Color.LightGray);
            StyleDataGrid(dgvDataScar, Color.DarkRed, Color.White, Color.LightGray);
            StyleDataGrid(dgvDataIV, Color.Gray, Color.White, Color.LightGray);

            

            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private void Form2_Load(object sender, EventArgs e)
        {
            ApplySnowManColors();

            //// デフォルト値は前月
            //formActionMethod.UpdateStartEndDate(txtBxStrYearMonth, txtBxEndYearMonth);

            // ★アニメーション登録
            SetButtonAnimation(btnDisplay);
            SetButtonAnimation(btnForm1Back);
        }


        /// <summary>
        /// データ表示実行ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // 
        private async void btnReadData_Click(object sender, EventArgs e)
        {
            DataProcessor processor = new DataProcessor();
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";

            // 年月入力チェック
            if (!FormActionMethod.TryParseYearMonth(txtBxStrYearMonth, out int strY, out int strM) ||
                !FormActionMethod.TryParseYearMonth(txtBxEndYearMonth, out int endY, out int endM))
            {
                MessageBox.Show("年月は6桁の数字(yyyyMM)で入力してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 会社未選択NG
            if (!chkBxOhno.Checked && !chkBxSuncar.Checked && !chkBxSundus.Checked)
            {
                MessageBox.Show("会社を選択してください。",
                                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 販売区分未選択NG
            if (!chkBxSalesAll.Checked && !chkBxSl.Checked && !chkBxPr.Checked && !chkBxIv.Checked)
            {
                MessageBox.Show("販売区分を選択してください。",
                                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FormAnimation2 anim = null;

            // --- FormAnimation スレッド ---
            Thread animThread = new Thread(() =>
            {
                using (FormAnimation2 a = new FormAnimation2())
                {

                    anim = a; // 外部参照用

                    a.Shown += (s, i) =>
                    {
                        a.Invoke((Action)(() =>
                        { 
                            anim.lblMessage.Text = "表示用データ作成中です…\r\n";
                            anim.BackColor = ColorManager.KojiDark1;
                        }));
                    };
                    Application.Run(a); // GIF表示
                }
            });
            animThread.SetApartmentState(ApartmentState.STA);
            animThread.Start();

            // --- メインスレッドでシミュレーション実行 ---
            await Task.Delay(100); // ちょっと待って anim が作られる

            // チェック状態取得
            var selCompanies = FormActionMethod.GetCompany(chkBxOhno, chkBxSundus, chkBxSuncar); // 会社
            var selBumons = FormActionMethod.GetSelectedBumons(listBxBumon);  // 部門（先頭空白行は無視して取得）
            var selSelleres = FormActionMethod.GetSallerOrSupplier(listBxSaller);  // 販売先 （先頭空白行は無視）
            var selSupplieres = FormActionMethod.GetSallerOrSupplier(listBxSupplier);  // 仕入先 （先頭空白行は無視）
            var selSlCategories = FormActionMethod.GetSalseProduct(chkBxSl, chkBxPr, chkBxIv, chkBxSalesAll);  // 販売区分
            var (selSlPrProducts, selIvProducts) = FormActionMethod.GetProduct(chkBxRawMaterials, chkBxSemiFinProducts,
                                                          chkBxProduct, chkBxProcess, chkBxProAll,
                                                          chkBxOhno, chkBxSundus, chkBxSuncar);  // 商品区分(在庫)

            // 開始・終了日付取得
            (string startDate, string endDate) = FormActionMethod.GetStartEndDate(strY, strM);
            (string _, string end) = FormActionMethod.GetStartEndDate(endY, endM);

            // 各データ取得(売上,仕入)
            DataTable ohnoSales = null, ohnoPurchase = null, ohnoStock = null;
            DataTable suncarSales = null, suncarPurchase = null, suncarStock = null;
            DataTable sundusSales = null, sundusPurchase = null, sundusStock = null;

            var classProduct = new Dictionary<string, string>
                            {
                                { "1", "原材料" },
                                { "2", "半製品" },
                                { "3", "半製品" },
                                { "4", "製品" },
                                { "5", "加工" }
                            };

            DataTable ohnoDt = null;
            DataTable suncarDt = null;
            DataTable sundusDt = null;
            DataTable stockDt = null;

            // ★売上データ
            if (selSlCategories.Contains("売上"))
            {
                foreach (var company in selCompanies)
                {
                    if (company == "オーノ") ohnoSales = FormActionMethod.MakeReadData_SLPR(startDate, endDate, company, "SL");
                    else if (company == "サンミックカーペット") suncarSales = FormActionMethod.MakeReadData_SLPR(startDate, endDate, company, "SL");
                    else if (company == "サンミックダスコン") sundusSales = FormActionMethod.MakeReadData_SLPR(startDate, endDate, company, "SL");
                }

                var datasetsS = new[]
                {
                    new { Name = "オーノ", Table = ohnoSales },
                    new { Name = "サンミックカーペット", Table = suncarSales },
                    new { Name = "サンミックダスコン", Table = sundusSales }
                };

                // ▼▼条件フィルター

                foreach (var d in datasetsS)
                {
                    if (d.Table == null) continue;

                    DataTable filtered = d.Table;
                    // 販売先
                    if (selSelleres.Count > 0)
                    {
                        filtered = processor.CustFilter(filtered, d.Table, selSelleres, "URHBSC");
                    }

                    // 商品区分選択
                    if (selSlPrProducts.Count > 0)
                    {
                        filtered = processor.ProductFileter(filtered, classProduct, selSlPrProducts);
                    }

                    // 部門選択
                    if (d.Name == "オーノ" && selBumons.Count > 0)
                    {
                        filtered = processor.BumonFilter(filtered, selBumons, "URBMCD");
                    }

                    var salesList = new List<DataTable>();
                    if (ohnoSales != null) salesList.Add(ohnoSales);
                    if (suncarSales != null) salesList.Add(suncarSales);
                    if (sundusSales != null) salesList.Add(sundusSales);

                    if (d.Name == "オーノ") ohnoSales = filtered;
                    else if (d.Name == "サンミックカーペット") suncarSales = filtered;
                    else if (d.Name == "サンミックダスコン") sundusSales = filtered;
                }
            }

            // ★仕入データ
            if (selSlCategories.Contains("仕入"))
            {
                foreach (var company in selCompanies)
                {
                    if (company == "オーノ") ohnoPurchase = FormActionMethod.MakeReadData_SLPR(startDate, endDate, company, "PR");
                    else if (company == "サンミックカーペット") suncarPurchase = FormActionMethod.MakeReadData_SLPR(startDate, endDate, company, "PR");
                    else if (company == "サンミックダスコン") sundusPurchase = FormActionMethod.MakeReadData_SLPR(startDate, endDate, company, "PR");
                }

                var datasetsP = new[]
                {
                    new { Name = "オーノ", Table = ohnoPurchase },
                    new { Name = "サンミックカーペット", Table = suncarPurchase },
                    new { Name = "サンミックダスコン", Table = sundusPurchase }
                };

                // ▼▼条件フィルター
                foreach (var d in datasetsP)
                {
                    if (d.Table == null) continue;

                    DataTable filtered = d.Table;
                    // 仕入先選択
                    if (selSupplieres.Count > 0)
                    {
                        filtered = processor.CustFilter(filtered, d.Table, selSupplieres, "SRSRCD");
                    }

                    // 商品区分選択
                    if (selSlPrProducts.Count > 0)
                    {
                        filtered = processor.ProductFileter(filtered, classProduct, selSlPrProducts);
                    }

                    // 部門選択
                    if (d.Name == "オーノ" && selBumons.Count > 0)
                    {
                        filtered = processor.BumonFilter(filtered, selBumons, "SRBMCD");
                    }

                    var purchaseList = new List<DataTable>();
                    if (ohnoPurchase != null) purchaseList.Add(ohnoPurchase);
                    if (suncarPurchase != null) purchaseList.Add(suncarPurchase);
                    if (sundusPurchase != null) purchaseList.Add(sundusPurchase);

                    if (d.Name == "オーノ") ohnoPurchase = filtered;
                    else if (d.Name == "サンミックカーペット") suncarPurchase = filtered;
                    else if (d.Name == "サンミックダスコン") sundusPurchase = filtered;
                }
            }

            // 売上・仕入データ結合
            var datasetsSP = new[]
            {
                    new { Name = "オーノ", Sales = ohnoSales, Purchase = ohnoPurchase },
                    new { Name = "サンミックカーペット", Sales = suncarSales, Purchase = suncarPurchase },
                    new { Name = "サンミックダスコン", Sales = sundusSales, Purchase = sundusPurchase }
                };
            foreach (var d in datasetsSP)
            {
                DataTable result = null;

                if (d.Sales != null && d.Purchase != null)
                {
                    result = processor.MergeSalesPurchase(d.Sales, d.Purchase);
                }
                else if (d.Sales != null)
                {
                    var dt = processor.NormalizeColumnNames(d.Sales, "Sales");
                    result = processor.SortData(dt);
                }
                else if (d.Purchase != null)
                {
                    var dt = processor.NormalizeColumnNames(d.Purchase, "Purchase");
                    result = processor.SortData(dt);
                }
                if (result != null)
                {
                    if (d.Name == "オーノ") ohnoDt = result;
                    else if (d.Name == "サンミックカーペット") suncarDt = result;
                    else if (d.Name == "サンミックダスコン") sundusDt = result;
                }
            }

            //在庫
            if (selSlCategories.Contains("在庫"))
            {
                foreach (var company in selCompanies)
                {
                    if (company == "オーノ") ohnoStock = FormActionMethod.MakeReadData_IV(startDate, endDate, company);
                    else if (company == "サンミックカーペット") suncarStock = FormActionMethod.MakeReadData_IV(startDate, endDate, company);
                    else if (company == "サンミックダスコン") sundusStock = FormActionMethod.MakeReadData_IV(startDate, endDate, company);
                }

                var datasetsI = new[]
                {
                    new { Name = "オーノ", Table = ohnoStock },
                    new { Name = "サンミックカーペット", Table = suncarStock },
                    new { Name = "サンミックダスコン", Table = sundusStock }
                };

                // ▼▼条件フィルター
                foreach (var d in datasetsI)
                {
                    var current = d.Table;
                    if (current == null) continue;
                    DataTable filtered = current;

                    // 商品区分選択
                    if (selIvProducts.Count > 0)
                    {
                        filtered = processor.ProductFileter(filtered, selIvProducts, selIvProducts);
                    }
                    // 部門選択
                    if (d.Name == "オーノ" && selBumons.Count > 0)
                    {
                        filtered = processor.BumonFilter(filtered, selBumons, "ZHBMCD");
                    }

                    if (d.Name == "オーノ") ohnoStock = filtered;
                    else if (d.Name == "サンミックカーペット") suncarStock = filtered;
                    else if (d.Name == "サンミックダスコン") sundusStock = filtered;
                }
            }
            var stockList = new List<DataTable>();
            if (ohnoStock != null) stockList.Add(ohnoStock);
            if (suncarStock != null) stockList.Add(suncarStock);
            if (sundusStock != null) stockList.Add(sundusStock);

            if (stockList.Count > 0)
            {
                stockDt = processor.MergeStockData(stockList.ToArray());
                stockDt = processor.FormatStockTable(stockDt);
            }

            // 売上・仕入・在庫データをDataGridViewにバインド
            dgvDataOhno.DataSource = ohnoDt;
            dgvDataSdus.DataSource = sundusDt;
            dgvDataScar.DataSource = suncarDt;
            dgvDataIV.DataSource = stockDt;

            
            // Form1のlistBxSituationに追記
            if (Application.OpenForms["Form1"] is Form1 form1)
            {
                form1.AddLog($"{HIZTIM} データ表示実行完了");
            }

            // --- 終了したらアニメーション閉じる ---
            await Task.Delay(500);
            if (anim != null && !anim.IsDisposed)
            {
                anim.Invoke(new Action(() => anim.CloseForm()));
            }

            // アニメーションスレッド終了を待つ
            animThread.Join();

        }

        /// <summary>
        /// Form1へ戻る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForm1Back_Click(object sender, EventArgs e)
        {
            // Form1 のインスタンスを取得して表示
            if (Application.OpenForms["Form1"] is Form1 form1)
            {
                form1.Show();
            }
            // Form2 を閉じる
            this.Close();
        }

        private void Company_CheckedChanged(object sender, EventArgs e)
        {
            FormActionMethod.SelectCompany_Bumon(chkBxOhno, chkBxSundus, chkBxSuncar, listBxBumon);
        }
               

        private void listBxBumon_selectedIndexChanged(object sender, EventArgs e)
        {
            FormActionMethod.ShowBumon(this, listBxBumon, listBxSaller, listBxSupplier, chkBxOhno, chkBxSundus, chkBxSuncar);
        }

        private void chkDataType(object sender, EventArgs e)
        {
            // チェックボックス制御
            bool showAll = chkBxSalesAll.Checked;
            bool showSl = chkBxSl.Checked || showAll;
            bool showPr = chkBxPr.Checked || showAll;
            bool showIv = chkBxIv.Checked || showAll;

            // ALL選択時は他チェック無効化
            chkBxSl.Enabled = !showAll;
            chkBxPr.Enabled = !showAll;
            chkBxIv.Enabled = !showAll;
        }
        private void chkProductType(object sender, EventArgs e)
        {
            // チェックボックス制御
            bool showAll = chkBxProAll.Checked;
            bool showRaw = chkBxRawMaterials.Checked || showAll;
            bool showSemi = chkBxSemiFinProducts.Checked || showAll;
            bool showProd = chkBxProduct.Checked || showAll;
            bool showProc = chkBxProcess.Checked || showAll;
            // ALL選択時は他チェック無効化
            chkBxRawMaterials.Enabled = !showAll;
            chkBxSemiFinProducts.Enabled = !showAll;
            chkBxProduct.Enabled = !showAll;
            chkBxProcess.Enabled = !showAll;
        }

        private void TxtBxYearMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 数字とバックスペースのみ許可
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void ApplySnowManColors()
        {
            // フォーム全体の背景
            this.BackColor = Color.FromArgb(255, 220, 150);

            DataGridView[] grids = { dgvDataOhno, dgvDataSdus, dgvDataScar, dgvDataIV };
            foreach (var dgv in grids)
            {
                dgv.BackgroundColor = ColorManager.RauDark1;
            }

            // ラベル類
            lbProductClass.ForeColor = ColorManager.MemeBase;
            lbSalesCategory.ForeColor = ColorManager.MemeBase;
            lbSupplier.ForeColor = ColorManager.MemeBase;
            lbSaller.ForeColor = ColorManager.MemeBase;
            lbBumon.ForeColor = ColorManager.MemeBase;
            lbCompany.ForeColor = ColorManager.MemeBase;
            lbSymbol1.ForeColor = ColorManager.MemeBase;
            label2.ForeColor = ColorManager.MemeBase;

            // DataGridView に色を適用
            StyleDataGrid(dgvDataOhno, ColorManager.KojiDark1, Color.FromArgb(245, 240, 220), ColorManager.KojiLight2);   
            StyleDataGrid(dgvDataSdus, Color.FromArgb(255, 153, 51), Color.FromArgb(245, 240, 220), ColorManager.HikaruLight1); 
            StyleDataGrid(dgvDataScar, Color.FromArgb(255, 170, 50), Color.FromArgb(255, 240, 200), Color.FromArgb(255, 230, 180));   
            StyleDataGrid(dgvDataIV, Color.FromArgb(160, 120, 60), Color.FromArgb(245, 220, 170), Color.FromArgb(200, 160, 100));     

            // チェックボックスなどは直接色指定でもOK
            foreach (Control ctrl in groupBox2.Controls)
            {
                if (ctrl is CheckBox chk)
                {
                    chk.ForeColor = ColorManager.MemeDark1;
                    chk.BackColor = Color.FromArgb(255, 220, 150);
                    ;
                }
            }

            foreach (Control ctrl in groupBox3.Controls)
            {
                if (ctrl is CheckBox chk)
                {
                    chk.ForeColor = ColorManager.MemeDark1;
                    chk.BackColor = Color.FromArgb(255, 220, 150);
                }
            }
            // CheckBox（データ・商品区分）
            foreach (Control ctrl in groupBox2.Controls)
            {
                if (ctrl is CheckBox cb)
                {
                    cb.ForeColor = ColorManager.KojiDark2;
                    cb.BackColor = Color.FromArgb(255, 220, 150);
                }
            }

            // CheckBox（会社選択）
            foreach (Control ctrl in groupBox3.Controls)
            {
                if (ctrl is CheckBox cb)
                {
                    cb.ForeColor = ColorManager.KojiDark2;
                    cb.BackColor = Color.FromArgb(255, 220, 150);
                }
            }

            // グループボックスの背景
            groupBox1.BackColor = Color.FromArgb(255, 220, 150);
            groupBox2.BackColor = Color.FromArgb(255, 220, 150);
            groupBox3.BackColor = Color.FromArgb(255, 220, 150);
            grpBxBtn.BackColor = Color.FromArgb(255, 220, 150);

            // ボタンの色
            StyleButton(btnDisplay, ColorManager.KojiBase, Color.White, borderColor: ColorManager.KojiDark1);
            StyleButton(btnForm1Back, ColorManager.KojiLight1, Color.White, borderColor: ColorManager.KojiDark1);
        }

        private void GroupBoxCustomBorder(object sender, PaintEventArgs e)
        {
            GroupBox box = (GroupBox)sender;
            e.Graphics.Clear(box.BackColor);

            // アンチエイリアス無効（線をくっきり）
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // テキストを測定
            SizeF textSize = e.Graphics.MeasureString(box.Text, box.Font);

            // 枠線色を赤茶色でで
            using (Pen pen = new Pen(Color.FromArgb(177, 65, 3), 1.5f))
            {
                int textPadding = 8;  // 左の余白
                int textWidth = (int)textSize.Width;

                // 枠線を描画（上の線だけタイトル部分を避ける）
                e.Graphics.DrawLine(pen, 1, (int)(textSize.Height / 2), textPadding - 2, (int)(textSize.Height / 2)); // 左上～文字前
                e.Graphics.DrawLine(pen, textPadding + textWidth + 2, (int)(textSize.Height / 2), box.Width - 2, (int)(textSize.Height / 2)); // 文字後～右上
                e.Graphics.DrawLine(pen, 1, (int)(textSize.Height / 2), 1, box.Height - 2); // 左線
                e.Graphics.DrawLine(pen, 1, box.Height - 2, box.Width - 2, box.Height - 2); // 下線
                e.Graphics.DrawLine(pen, box.Width - 2, (int)(textSize.Height / 2), box.Width - 2, box.Height - 2); // 右線

                // テキストを描画
                using (SolidBrush brush = new SolidBrush(ColorManager.MemeDark1))
                {
                    e.Graphics.DrawString(box.Text, box.Font, brush, 8, 0);
                }
            }


        }


    }
}
