//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Excel = Microsoft.Office.Interop.Excel;
//using CMD = あすよん月次帳票.CommonData;
//using ENM = あすよん月次帳票.Enums;
//using DCN = あすよん月次帳票.Dictionaries;


//namespace あすよん月次帳票
//{
//    public partial class Form4 : Form
//    {
//        private string HIZTIM;
//        private string Hiz;
//        private string Tim;
//        private string startDate;
//        private string endDate;

//        FormAction fam = new FormAction();
//        ColorManager clrmg = new ColorManager();
//        Summarize summ = new Summarize();
//        Processor process = new Processor();

//        public Form4()
//        {
//            //InitializeComponent();

//            //grpBxData.Paint += GroupBoxCustomBorder;
//            //grpBxOrganization.Paint += GroupBoxCustomBorder;
//            //grpBxCondition.Paint += GroupBoxCustomBorder;

//            //this.Region = System.Drawing.Region.FromHrgn(
//            //    CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));
//        }

//        // Excelエクスポートボタンクリック
//        private async void btnExportExcel_Click(object sender, EventArgs e)
//        {
//            // チェック状態取得

//            // 月初日取得
//            string startDate = CMD.TYM + "01";
//            // 月末日取得
//            int year = int.Parse(CMD.TYM.Substring(0, 4));
//            int month = int.Parse(CMD.TYM.Substring(4, 2));
//            int lastDay = DateTime.DaysInMonth(year, month);
//            string endDate = CMD.TYM + lastDay.ToString("D2");

//            DataTable slprResult = new DataTable();
//            DataTable stockDtNow = new DataTable();
//            DataTable stockDtOld = new DataTable();
//            DataTable ohnoSales = new DataTable();
//            DataTable suncarSales = new DataTable();
//            DataTable sundusSales = new DataTable();
//            DataTable filtered = new DataTable();
//            DataTable ohnoPurchase = new DataTable();
//            DataTable suncarPurchase = new DataTable();
//            DataTable sundusPurchase = new DataTable();
//            DataTable ohnoDt = new DataTable();
//            DataTable suncarDt = new DataTable();
//            DataTable sundusDt = new DataTable();
//            DataTable ohnoStockNow = new DataTable();
//            DataTable ohnoStockOld = new DataTable();
//            DataTable suncarStockNow = new DataTable();
//            DataTable suncarStockOld = new DataTable();
//            DataTable sundusStockNow = new DataTable();
//            DataTable sundusStockOld = new DataTable();
//            Dictionary<string,string> classProduct = new Dictionary<string, string>();
//            // ★売上データ
//            if (DCN.selCategories.Contains("売上"))
//            {
//                foreach (var company in DCN.selCompanies)
//                {
//                    if (company == "オーノ") ohnoSales = fam.MakeReadData_SLPR(startDate, endDate, company, "SL");
//                    else if (company == "サンミックカーペット") suncarSales = fam.MakeReadData_SLPR(startDate, endDate, company, "SL");
//                    else if (company == "サンミックダスコン") sundusSales = fam.MakeReadData_SLPR(startDate, endDate, company, "SL");
//                }

//                var datasetsS = new[]
//                {
//                    new { Name = "オーノ", Table = ohnoSales },
//                    new { Name = "サンミックカーペット", Table = suncarSales },
//                    new { Name = "サンミックダスコン", Table = sundusSales }
//                };

//                // ▼▼条件フィルター
//                var salesList = new List<DataTable>();
//                foreach (var d in datasetsS)
//                {
//                    if (d.Table == null) continue;

//                    filtered = d.Table;
//                    // 販売先
//                    if (DCN.selSelleres.Count > 0)
//                    {
//                        filtered = process.CustFilter(filtered, d.Table, DCN.selSelleres, "URHBSC");
//                    }

//                    // 商品区分選択
//                    if (DCN.selSlPrProducts.Count < 5)
//                    {
//                        filtered = process.ProductFileter(filtered, classProduct, DCN.selSlPrProducts);
//                    }

//                    // 部門選択
//                    if (d.Name == "オーノ" && DCN.selBumons.Count > 0)
//                    {
//                        filtered = process.BumonFilter(filtered, DCN.selBumons, "URBMCD");
//                    }

//                    salesList.Add(d.Table);

//                    if (d.Name == "オーノ") ohnoSales = filtered;
//                    else if (d.Name == "サンミックカーペット") suncarSales = filtered;
//                    else if (d.Name == "サンミックダスコン") sundusSales = filtered;
//                }
//            }

//            // ★仕入データ
//            if (DCN.selCategories.Contains("仕入"))
//            {
//                foreach (var company in DCN.selCompanies)
//                {
//                    if (company == "オーノ") ohnoPurchase = fam.MakeReadData_SLPR(startDate, endDate, company, "PR");
//                    else if (company == "サンミックカーペット") suncarPurchase = fam.MakeReadData_SLPR(startDate, endDate, company, "PR");
//                    else if (company == "サンミックダスコン") sundusPurchase = fam.MakeReadData_SLPR(startDate, endDate, company, "PR");
//                }

//                var datasetsP = new[]
//                {
//                    new { Name = "オーノ", Table = ohnoPurchase },
//                    new { Name = "サンミックカーペット", Table = suncarPurchase },
//                    new { Name = "サンミックダスコン", Table = sundusPurchase }
//                };

//                // ▼▼条件フィルター
//                var purchaseList = new List<DataTable>();
//                foreach (var d in datasetsP)
//                {
//                    if (d.Table == null) continue;

//                    filtered = d.Table;
//                    // 仕入先選択
//                    if (DCN.selSupplieres.Count > 0)
//                    {
//                        filtered = process.CustFilter(filtered, d.Table, DCN.selSupplieres, "SRSRCD");
//                    }

//                    // 商品区分選択
//                    if (DCN.selSlPrProducts.Count < 5)
//                    {
//                        filtered = process.ProductFileter(filtered, classProduct, DCN.selSlPrProducts);
//                    }

//                    // 部門選択
//                    if (d.Name == "オーノ" && DCN.selBumons.Count > 0)
//                    {
//                        filtered = process.BumonFilter(filtered, DCN.selBumons, "SRBMCD");
//                    }

//                    purchaseList.Add(d.Table);

//                    if (d.Name == "オーノ") ohnoPurchase = filtered;
//                    else if (d.Name == "サンミックカーペット") suncarPurchase = filtered;
//                    else if (d.Name == "サンミックダスコン") sundusPurchase = filtered;
//                }
//            }

//            // 売上・仕入データ結合
//            var datasetsSP = new[]
//            {
//                    new { Name = "オーノ", Sales = ohnoSales, Purchase = ohnoPurchase },
//                    new { Name = "サンミックカーペット", Sales = suncarSales, Purchase = suncarPurchase },
//                    new { Name = "サンミックダスコン", Sales = sundusSales, Purchase = sundusPurchase }
//                };
//            foreach (var d in datasetsSP)
//            {
//                DataTable result = null;

//                if (d.Sales != null && d.Purchase != null)
//                {
//                    result = process.MergeSalesPurchase(d.Sales, d.Purchase, true);
//                }
//                else if (d.Sales != null)
//                {
//                    var dt = process.NormalizeColumnNames(d.Sales, "Sales");
//                    result = process.SortData(dt);
//                }
//                else if (d.Purchase != null)
//                {
//                    var dt = process.NormalizeColumnNames(d.Purchase, "Purchase");
//                    result = process.SortData(dt);
//                }
//                if (result != null)
//                {
//                    if (d.Name == "オーノ") ohnoDt = result;
//                    else if (d.Name == "サンミックカーペット") suncarDt = result;
//                    else if (d.Name == "サンミックダスコン") sundusDt = result;
//                }
//            }

//            // ★在庫データ
//            if (DCN.selCategories.Contains("在庫"))
//            {
//                foreach (var company in DCN.selCompanies)
//                {
//                    if (company == "オーノ") (ohnoStockNow, ohnoStockOld) = fam.MakeReadData_IV(startDate, endDate, company, DCN.selIvProducts);
//                    else if (company == "サンミックカーペット") (suncarStockNow, suncarStockOld) = fam.MakeReadData_IV(startDate, endDate, company, DCN.selIvProducts);
//                    else if (company == "サンミックダスコン") (sundusStockNow, sundusStockOld) = fam.MakeReadData_IV(startDate, endDate, company, DCN.selIvProducts);
//                }

//                var datasetsI = new[]
//                {
//                    new { Name = "オーノ", Table = ohnoStock },
//                    new { Name = "サンミックカーペット", Table = suncarStock },
//                    new { Name = "サンミックダスコン", Table = sundusStock }
//                };

//                // ▼▼条件フィルター
//                var stockList = new List<DataTable>();
//                foreach (var d in datasetsI)
//                {
//                    if (d.Table == null) continue;
//                    filtered = d.Table;

//                    // 商品区分選択
//                    if (selIvProducts.Count < 5)
//                    {
//                        filtered = processor.ProductFileter(filtered, selIvProducts, selIvProducts);
//                    }
//                    // 部門選択
//                    if (d.Name == "オーノ" && selBumons.Count > 0)
//                    {
//                        filtered = processor.BumonFilter(filtered, selBumons, "ZHBMCD");
//                    }
//                    if (d.Name == "オーノ") ohnoStock = filtered;
//                    else if (d.Name == "サンミックカーペット") suncarStock = filtered;
//                    else if (d.Name == "サンミックダスコン") sundusStock = filtered;

//                    stockList.Add(d.Table);
//                }

//                bool flg = true;
//                if (stockList.Count > 1)
//                {
//                    stockDt = processor.MergeData(stockList.ToArray());
//                    stockDt = processor.FormatStockTable(stockDt, flg);
//                }
//                else
//                {
//                    stockDt = processor.FormatStockTable(filtered, flg);
//                }
//            }

//            // 売上・仕入の集計
//            DataTable ohnoSummary = summ.SummarizeSalesPurchase(PrepareForSummary(ohnoDt));
//            DataTable suncarSummary = summ.SummarizeSalesPurchase(PrepareForSummary(suncarDt));
//            DataTable sundusSummary = summ.SummarizeSalesPurchase(PrepareForSummary(sundusDt));

//            // 全データ結合
//            List<DataTable> allTables = new List<DataTable>();

//            if (stockDt != null && stockDt.Rows.Count > 0)
//                allTables.Add(stockDt);

//            if (ohnoSummary != null && ohnoSummary.Rows.Count > 0)
//                allTables.Add(ohnoSummary);

//            if (suncarSummary != null && suncarSummary.Rows.Count > 0)
//                allTables.Add(suncarSummary);

//            if (sundusSummary != null && sundusSummary.Rows.Count > 0)
//                allTables.Add(sundusSummary);

//            // 全データをマージして取引先/品種でサマリ 
//            DataTable mergedSummary = summ.MergedSummary(allTables);

//            // さらにクラス名＋取引区分＋部門でサマリ
//            DataTable summary2 = summ.SummarizeByCategoryTypeDept(mergedSummary);

//            if (mergedSummary == null || mergedSummary.Rows.Count == 0)
//            {
//                MessageBox.Show("エクスポートするデータがありません。");
//                return;
//            }

//            Excel.Application xlApp = null;
//            Excel.Workbook xlBook = null;
//            Excel.Worksheet xlSheet = null;
//            Excel.Worksheet xlSheet2 = null;

//            Excel.Range cdRange = null;
//            Excel.Range colRange2 = null;
//            Excel.Range start2 = null;
//            Excel.Range end2 = null;
//            // ↑↑------- メインスレッドでデータ抽出 -------↑↑

//            // ↓↓---------- アニメーション閉じる ----------↓↓
//            await Task.Delay(500);
//            if (anim != null && !anim.IsDisposed)
//            {
//                anim.Invoke(new Action(() => anim.CloseForm()));
//            }

//            // アニメーションスレッド終了を待つ
//            animThread.Join();

//            // ↑↑---------- アニメーション閉じる ----------↑↑


//            // ↓↓----------- Excelエクスポート -----------↓↓
//            try
//            {
//                // 保存ダイアログ
//                SaveFileDialog sfd = new SaveFileDialog();
//                sfd.Filter = "マクロ有効Excelファイル|*.xlsm";
//                sfd.Title = "保存先を指定してください";
//                sfd.FileName = $"月次データ.{Hiz}.{Tim}.xlsm";

//                if (sfd.ShowDialog() != DialogResult.OK) return;

//                string filePath = sfd.FileName;

//                // Excelアプリケーション作成
//                xlApp = new Excel.Application { Visible = false };

//                // 新規ブック作成
//                xlBook = xlApp.Workbooks.Add();

//                //==================== Dataシート =====================
//                xlSheet = (Excel.Worksheet)xlBook.Sheets[1];
//                xlSheet.Name = "Data";

//                // ヘッダー
//                for (int c = 0; c < mergedSummary.Columns.Count; c++)
//                {
//                    xlSheet.Cells[1, c + 1] = mergedSummary.Columns[c].ColumnName;
//                }

//                // データ
//                int rows = mergedSummary.Rows.Count;
//                int cols = mergedSummary.Columns.Count;

//                // CD列は個別に書き込み
//                string cdColName = "取引先/品種CD";
//                int cdIndex = mergedSummary.Columns[cdColName].Ordinal + 1;

//                // CD列を文字列形式に設定
//                cdRange = xlSheet.Columns[cdIndex];
//                cdRange.NumberFormat = "@"; // Excelで文字列扱い

//                // CD列を個別に書き込む（ゼロ埋め）
//                for (int r = 0; r < rows; r++)
//                {
//                    string cdValue = mergedSummary.Rows[r][cdColName]?.ToString() ?? "";

//                    // 取引区分をチェック
//                    string transactionType = mergedSummary.Columns.Contains("取引区分")
//                        ? mergedSummary.Rows[r]["取引区分"]?.ToString() ?? ""
//                        : "";
//                    // 売上高・仕入高だけゼロ埋め
//                    if (transactionType == "売上高" && transactionType == "仕入高")
//                    {
//                        cdValue = cdValue.PadLeft(7, '0'); // 7桁にゼロ埋め
//                    }
//                    xlSheet.Cells[r + 2, cdIndex] = cdValue;
//                }

//                // CD列以外を配列に格納
//                object[,] dataArray = new object[rows, cols - 1];
//                int dataCol = 0;
//                for (int c = 0; c < cols; c++)
//                {
//                    if (mergedSummary.Columns[c].ColumnName == cdColName) continue; // CD列はスキップ
//                    string colName = mergedSummary.Columns[c].ColumnName;
//                    for (int r = 0; r < rows; r++)
//                    {
//                        var val = mergedSummary.Rows[r][c];

//                        // 数量計・金額計は数値形式に変換
//                        if ((colName == "数量計" || colName == "金額計") && val != null && val != DBNull.Value)
//                        {
//                            if (decimal.TryParse(val.ToString(), out decimal num))
//                            {
//                                dataArray[r, dataCol] = num.ToString("#,0");
//                            }
//                            else
//                            {
//                                dataArray[r, dataCol] = "0";
//                            }
//                        }
//                        // 年月は文字列形式で
//                        else if (colName == "年月")
//                        {
//                            string ymd = val?.ToString() ?? "";
//                            dataArray[r, dataCol] = "'" + ymd;
//                            //dataArray[r, dataCol] = ymd;
//                        }
//                        else
//                        {
//                            dataArray[r, dataCol] = val?.ToString() ?? "";
//                        }
//                    }
//                    // Excel上で文字列扱いにしたい列は NumberFormat = "@"
//                    if (colName == "年月")
//                    {
//                        int excelColIndex = c + 1; // Excel列番号
//                        Excel.Range yearRange = xlSheet.Columns[excelColIndex];
//                        yearRange.NumberFormat = "@"; // 文字列扱い
//                        Marshal.ReleaseComObject(yearRange);
//                    }
//                    dataCol++;
//                }

//                // 一括代入（CD列以外）
//                dataCol = 0;
//                for (int c = 0; c < cols; c++)
//                {
//                    if (mergedSummary.Columns[c].ColumnName == cdColName) continue;
//                    Excel.Range colRange = xlSheet.Range[xlSheet.Cells[2, c + 1], xlSheet.Cells[rows + 1, c + 1]];

//                    colRange.Value = GetColumnData(dataArray, dataCol, rows);
//                    dataCol++;
//                }

//                // カンマ付き表示をExcel上でも設定（念のため）
//                for (int c = 0; c < cols; c++)
//                {
//                    string colName = mergedSummary.Columns[c].ColumnName;
//                    if (colName == "数量計" || colName == "金額計")
//                    {
//                        Excel.Range col = xlSheet.Range[xlSheet.Cells[2, c + 1], xlSheet.Cells[rows + 1, c + 1]];
//                        col.NumberFormat = "#,##0";
//                        Marshal.ReleaseComObject(col);
//                    }
//                }
//                // ------------------- ヘルパー関数 -------------------
//                object[,] GetColumnData(object[,] source, int colIndex, int rowCount)
//                {
//                    object[,] colData = new object[rowCount, 1];
//                    for (int r = 0; r < rowCount; r++)
//                        colData[r, 0] = source[r, colIndex];
//                    return colData;
//                }

//                // ==================== 集計シート =====================
//                xlSheet2 = (Excel.Worksheet)xlBook.Sheets.Add(After: xlBook.Sheets[xlBook.Sheets.Count]);
//                xlSheet2.Name = "集計";

//                int rows2 = summary2.Rows.Count;
//                int cols2 = summary2.Columns.Count;

//                // ヘッダー
//                for (int i = 0; i < cols2; i++) xlSheet2.Cells[1, i + 1] = summary2.Columns[i].ColumnName;

//                // データ
//                object[,] dataArray2 = new object[rows2, cols2];
//                for (int r = 0; r < rows2; r++)
//                {
//                    for (int c = 0; c < cols2; c++)
//                    {
//                        var val = summary2.Rows[r][c];
//                        string colName = summary2.Columns[c].ColumnName;

//                        if ((colName == "数量計" || colName == "金額計") && val != null && val != DBNull.Value)
//                        {
//                            if (decimal.TryParse(val.ToString(), out decimal num))
//                            {
//                                dataArray2[r, c] = num;  // 数値そのまま
//                            }
//                            else
//                            {
//                                dataArray2[r, c] = 0m;
//                            }
//                        }
//                        else if (colName == "年月")
//                        {
//                            string ymd = val?.ToString() ?? "";
//                            dataArray2[r, c] = "'" + ymd; // 文字列として扱うため先頭にシングルクォート追加
//                                                          //dataArray2[r, dataCol] = ymd;
//                        }
//                        else
//                        {
//                            dataArray2[r, c] = val?.ToString() ?? "";
//                        }
//                    }
//                }

//                // 一括代入
//                start2 = xlSheet2.Cells[2, 1];
//                end2 = xlSheet2.Cells[rows2 + 1, cols2];
//                xlSheet2.Range[start2, end2].Value = dataArray2;

//                // 年月列だけ文字列扱いに設定
//                for (int c = 0; c < cols2; c++)
//                {
//                    string colName = summary2.Columns[c].ColumnName;
//                    if (colName == "年月")
//                    {
//                        int excelColIndex = c + 1;
//                        Excel.Range yearRange = xlSheet2.Columns[excelColIndex];
//                        yearRange.NumberFormat = "@"; // 文字列扱い
//                        Marshal.ReleaseComObject(yearRange);
//                    }
//                }

//                // カンマ付き表示をExcel上でも設定
//                for (int c = 0; c < cols2; c++)
//                {
//                    string colName = summary2.Columns[c].ColumnName;
//                    if (colName == "数量計" || colName == "金額計")
//                    {
//                        Excel.Range col = xlSheet2.Range[xlSheet2.Cells[2, c + 1], xlSheet2.Cells[rows2 + 1, c + 1]];
//                        col.NumberFormat = "#,##0"; // 数値＋カンマ表示 
//                        Marshal.ReleaseComObject(col);
//                    }
//                }

//                xlSheet.Columns.AutoFit();
//                xlSheet2.Columns.AutoFit();
//                xlSheet.Select();

//                // 既存のフィルターをクリア
//                if (xlSheet.AutoFilterMode) xlSheet.AutoFilterMode = false;

//                // ヘッダー範囲を取得（1行目）
//                Excel.Range headerRange = xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[1, cols]];
//                // 太文字
//                headerRange.Font.Bold = true;
//                // 下線＋枠線（外枠＋内部の罫線）
//                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
//                // 背景色（任意）
//                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
//                // フィルターを有効化
//                headerRange.AutoFilter(1);
//                // COMオブジェクト解放
//                Marshal.ReleaseComObject(headerRange);

//                // マクロ有効形式で保存
//                xlBook.SaveAs(filePath, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
//                xlBook.Close(false);
//                xlApp.Quit();

//                // 保存後に開くか確認
//                var result = MessageBox.Show("Excelを保存しました。\n開きますか?", "保存完了", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (result == DialogResult.Yes)
//                {
//                    System.Diagnostics.Process.Start(filePath);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Excelエクスポート中にエラーが発生しました: " + ex.Message);
//            }

//            finally
//            {
//                // COMオブジェクト解放
//                if (cdRange != null) { Marshal.ReleaseComObject(cdRange); cdRange = null; }
//                if (colRange2 != null) { Marshal.ReleaseComObject(colRange2); colRange2 = null; }
//                if (start2 != null) { Marshal.ReleaseComObject(start2); start2 = null; }
//                if (end2 != null) { Marshal.ReleaseComObject(end2); end2 = null; }
//                if (xlSheet != null) { Marshal.ReleaseComObject(xlSheet); xlSheet = null; }
//                if (xlSheet2 != null) { Marshal.ReleaseComObject(xlSheet2); xlSheet2 = null; }
//                if (xlBook != null) { Marshal.ReleaseComObject(xlBook); xlBook = null; }
//                if (xlApp != null) { Marshal.ReleaseComObject(xlApp); xlApp = null; }

//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//            }

//            // ログ追加             
//            fam.AddLog("Excelエクスポート完了", listBxSituation);
//            if (Application.OpenForms["Form1"] is Form1 form1) form1.AddLog($"{HIZTIM}　Excelエクスポート完了");
//            // ↑↑----------- Excelエクスポート -----------↑↑

//        }
//        private void btnForm1Back_Click(object sender, EventArgs e)
//        {
//            // Form1 のインスタンスを取得して表示
//            if (Application.OpenForms["Form1"] is Form1 form1)
//            {
//                form1.Show();
//            }
//            // Form4 を閉じる
//            this.Close();
//        }

//        private void TxtBxYearMonth_KeyPress(object sender, KeyPressEventArgs e)
//        {
//            // 数字とバックスペースのみ許可
//            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
//            {
//                e.Handled = true;
//            }
//        }

//        private DataTable PrepareForSummary(DataTable dt)
//        {
//            // dt が null の場合は空テーブル返す
//            if (dt == null) return new DataTable();

//            // 行がないならクローン（構造だけ）を返す
//            if (dt.Rows.Count == 0) return dt.Clone();

//            DataTable clone = dt.Copy();

//            var classDict = new Dictionary<string, string>
//            {
//                { "1", "原材料" },
//                { "2", "半製品" },
//                { "3", "半製品" },
//                { "4", "製品" },
//                { "5", "加工" }
//            };


//            // クラス → クラス名
//            if (!clone.Columns.Contains("クラス名"))
//                clone.Columns.Add("クラス名", typeof(string));

//            foreach (DataRow row in clone.Rows)
//            {
//                // 優先: 既にクラス名があるならそれを使う
//                if (!string.IsNullOrWhiteSpace(row["クラス名"]?.ToString()))
//                    continue;

//                string classKey = clone.Columns.Contains("クラス") && row["クラス"] != DBNull.Value
//                      ? row["クラス"].ToString()
//                      : "";

//                // 数字を日本語に変換
//                if (classDict.ContainsKey(classKey))
//                    row["クラス名"] = classDict[classKey];
//                else
//                    row["クラス名"] = classKey; // 元が空か文字列ならそのまま
//            }

//            // 数量 → 数量計
//            if (!clone.Columns.Contains("数量計"))
//                clone.Columns.Add("数量計", typeof(decimal));

//            foreach (DataRow row in clone.Rows)
//            {
//                if (clone.Columns.Contains("数量") && row["数量"] != DBNull.Value)
//                {
//                    var s = row["数量"].ToString();
//                    if (decimal.TryParse(s, out decimal v)) row["数量計"] = v;
//                    else row["数量計"] = 0m;
//                }
//                else if (row["数量計"] == DBNull.Value || string.IsNullOrWhiteSpace(row["数量計"]?.ToString()))
//                {
//                    row["数量計"] = 0m;
//                }
//            }

//            // 金額 → 金額計
//            if (!clone.Columns.Contains("金額計"))
//                clone.Columns.Add("金額計", typeof(decimal));

//            foreach (DataRow row in clone.Rows)
//            {
//                if (clone.Columns.Contains("金額") && row["金額"] != DBNull.Value)
//                {
//                    var s = row["金額"].ToString();
//                    if (decimal.TryParse(s, out decimal v)) row["金額計"] = v;
//                    else row["金額計"] = 0m;
//                }
//                else if (row["金額計"] == DBNull.Value || string.IsNullOrWhiteSpace(row["金額計"]?.ToString()))
//                {
//                    row["金額計"] = 0m;
//                }
//            }

//            return clone;
//        }


//    }
//}




