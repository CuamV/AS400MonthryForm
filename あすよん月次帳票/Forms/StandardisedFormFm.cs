using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using DCN = あすよん月次帳票.Dictionaries;
using ENM = あすよん月次帳票.Enums;


namespace あすよん月次帳票
{
    public partial class StandardisedFormFm : Form
    {
        private string HIZTIM;
        private string Hiz;
        private string Tim;
        private string startDate;
        private string endDate;

        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();
        Summarize summ = new Summarize();

        public StandardisedFormFm()
        {
            InitializeComponent();

            this.Load += StandardisedFormFm_Load;

            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private void StandardisedFormFm_Load(object sender, EventArgs e)
        {
            ApplySnowManColors();

            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// 月次帳票(未確定)リンククリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void 月次帳票未確定_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} 月次帳票未確定_LinkClicked");

            //----------------------------------------------------
            // ★開始日付・終了日付の取得
            //----------------------------------------------------
            if (string.IsNullOrEmpty(CMD.TYM) || CMD.TYM.Length != 6)
            {
                MessageBox.Show("対象年月が正しく設定されていません。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int year = int.Parse(CMD.TYM.Substring(0, 4));
            int month = int.Parse(CMD.TYM.Substring(4, 2));

            // 月初日取得
            startDate = CMD.TYM + "01";
            // 月末日取得
            int lastDay = DateTime.DaysInMonth(year, month);
            endDate = CMD.TYM + lastDay.ToString("D2");

            //----------------------------------------------------
            // ★データ抽出処理
            //----------------------------------------------------
            DataTable slprResult = null;
            DataTable stockDtNow = null;
            DataTable stockDtOld = null;

            try
            {
                (slprResult, stockDtNow, stockDtOld) = fam.FilterData(startDate, endDate,
                    DCN.selCompanies, DCN.selBumons, DCN.selSelleres, DCN.selSupplieres, 
                    DCN.selCategories, DCN.selSlPrProducts, DCN.selIvProducts, DCN.selIvTypes);
            }
            catch (Ohno.Db.ConnectionFailedException)
            {
                await Task.Delay(500);

                try
                {
                    (slprResult, stockDtNow, stockDtOld) = fam.FilterData(startDate, endDate,
                        DCN.selCompanies, DCN.selBumons, DCN.selSelleres, DCN.selSupplieres, 
                        DCN.selCategories, DCN.selSlPrProducts, DCN.selIvProducts, DCN.selIvTypes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("AS400への接続に失敗しました。\n" +
                        "ネットワークを確認して、再度お試しください。\n\n" +
                        $"【詳細】{ex.Message}", "接続エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //----------------------------------------------------
            // ★集計処理
            //----------------------------------------------------
            // 売上・仕入データの前処理
            DataTable preparedSlpr = PrepareForSummary(slprResult);
            
            // 在庫データの前処理
            DataTable preparedStockNow = PrepareForSummary(stockDtNow);
            
            // 分類・部門グループ別に集計（売上・仕入・在庫）
            DataTable summaryData = summ.SummarizeByCategoryTypeDept(preparedSlpr, preparedStockNow);

            if (summaryData == null || summaryData.Rows.Count == 0)
            {
                MessageBox.Show("集計するデータがありません。", "情報",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //----------------------------------------------------
            // ★Excelエクスポート処理
            //----------------------------------------------------
            Hiz = DateTime.Now.ToString("yyyyMMdd");
            Tim = DateTime.Now.ToString("HHmmss");
            string fileName = $"月次帳票_{CMD.TYM}_{Hiz}_{Tim}.xlsx";

            try
            {
                ExportToExcel(summaryData, fileName);

                MessageBox.Show("Excelエクスポートが完了しました。", "完了",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} 処理完了 1 {CMD.UserName} 月次帳票エクスポート成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excelエクスポート中にエラーが発生しました。\n{ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} エラー 1 {CMD.UserName} 月次帳票エクスポート失敗: {ex.Message}");
            }
        }

        /// <summary>
        /// 戻るボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForm1Back_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btnForm1Back_Click");
            // Form1 のインスタンスを取得して表示
            // 名前で探すと見つからない場合があるため、型で検索して取得する
            var form1 = Application.OpenForms.OfType<TopMenuFm>().FirstOrDefault();
            if (form1 != null)
            {
                form1.Show();
            }
            // Form4 を閉じる
            this.Close();
        }

        // フィールドに追加
        private Point mouseOffset;
        private bool isMouseDown = false;
        /// <summary>
        /// MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }
        /// <summary>
        /// MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                this.Location = mousePos;
            }
        }
        /// <summary>
        /// MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isMouseDown = false;
        }
        /// <summary>
        /// 最小化ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        //=================================================================
        // 処理メソッド
        //=================================================================
        /// <summary>
        /// クラス名、数量計、金額計の列を追加して返す
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable PrepareForSummary(DataTable dt)
        {
            // dt が null の場合は空テーブル返す
            if (dt == null) return new DataTable();

            // 行がないならクローン（構造だけ）を返す
            if (dt.Rows.Count == 0) return dt.Clone();

            DataTable clone = dt.Copy();

                        // クラス → クラス名
            if (!clone.Columns.Contains("クラス名"))
                clone.Columns.Add("クラス名", typeof(string));

            foreach (DataRow row in clone.Rows)
            {
                // 優先: 既にクラス名があるならそれを使う
                if (!string.IsNullOrWhiteSpace(row["クラス名"]?.ToString()))
                    continue;

                string classKey = clone.Columns.Contains("クラス") && row["クラス"] != DBNull.Value
                      ? row["クラス"].ToString()
                      : "";

                // 数字を日本語に変換
                if (DCN.classMap.ContainsKey(classKey))
                    row["クラス名"] = DCN.classMap[classKey];
                else
                    row["クラス名"] = classKey; // 元が空か文字列ならそのまま
            }

            // 数量 → 数量計
            if (!clone.Columns.Contains("数量計"))
                clone.Columns.Add("数量計", typeof(decimal));

            foreach (DataRow row in clone.Rows)
            {
                if (clone.Columns.Contains("数量") && row["数量"] != DBNull.Value)
                {
                    var s = row["数量"].ToString();
                    if (decimal.TryParse(s, out decimal v)) row["数量計"] = v;
                    else row["数量計"] = 0m;
                }
                else if (row["数量計"] == DBNull.Value || string.IsNullOrWhiteSpace(row["数量計"]?.ToString()))
                {
                    row["数量計"] = 0m;
                }
            }

            // 金額 → 金額計
            if (!clone.Columns.Contains("金額計"))
                clone.Columns.Add("金額計", typeof(decimal));

            foreach (DataRow row in clone.Rows)
            {
                if (clone.Columns.Contains("金額") && row["金額"] != DBNull.Value)
                {
                    var s = row["金額"].ToString();
                    if (decimal.TryParse(s, out decimal v)) row["金額計"] = v;
                    else row["金額計"] = 0m;
                }
                else if (row["金額計"] == DBNull.Value || string.IsNullOrWhiteSpace(row["金額計"]?.ToString()))
                {
                    row["金額計"] = 0m;
                }
            }

            return clone;
        }
        /// <summary>
        /// Excelへエクスポート
        /// </summary>
        private void ExportToExcel(DataTable data, string fileName)
        {
            // 保存ダイアログ
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excelファイル|*.xlsx",
                Title = "保存先を指定してください",
                FileName = fileName
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            string filePath = sfd.FileName;

            // Excelアプリケーション作成
            var xlApp = new Microsoft.Office.Interop.Excel.Application { Visible = false };
            var xlBook = xlApp.Workbooks.Add();
            var xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Sheets[1];

            try
            {
                xlSheet.Name = $"{CMD.TYM}月次帳票";

                // ==========================================
                // 分類ごとにオーノ・サンミックに分類
                // ==========================================
                var categoryOrder = new[] { "製品売上高", "原材料売上高", "製品仕入高", "原材料仕入高", "製品在庫", "原材料在庫", "仕掛品在庫" };

                // 分類ごとにグループ化
                var groupedData = new List<(string Category, List<DataRow> OhnoRows, List<DataRow> SanmicRows)>();

                foreach (var category in categoryOrder)
                {
                    var categoryRows = data.AsEnumerable()
                        .Where(r => r["分類"].ToString() == category)
                        .ToList();

                    if (categoryRows.Count == 0) continue;

                    var ohnoRows = categoryRows
                        .Where(r => r["部門グループ"].ToString().Contains("#110～") ||
                                   r["部門グループ"].ToString() == "#800")
                        .ToList();

                    var sanmicRows = categoryRows
                        .Where(r => r["部門グループ"].ToString() == "#900,950")
                        .ToList();

                    groupedData.Add((category, ohnoRows, sanmicRows));
                }

                // ==========================================
                // ヘッダー行（1行目）
                // ==========================================
                xlSheet.Cells[1, 1] = "オーノ";
                xlSheet.Cells[1, 4] = "サンミック";

                // ヘッダーのセル結合（A1:C1, D1:F1）
                xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[1, 3]].Merge();
                xlSheet.Range[xlSheet.Cells[1, 4], xlSheet.Cells[1, 6]].Merge();

                // ヘッダーの書式設定
                var headerRange = xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[1, 6]];
                headerRange.Font.Name = "Meiryo UI";
                headerRange.Font.Size = 12;
                headerRange.Font.Bold = true;
                headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                // ==========================================
                // データ書き込み
                // ==========================================
                int currentRow = 2;
                foreach (var (category, ohnoRows, sanmicRows) in groupedData)
                {
                    int maxRows = Math.Max(ohnoRows.Count, sanmicRows.Count);

                    for (int i = 0; i < maxRows; i++)
                    {
                        // --- オーノ側（列A～C） ---
                        if (i < ohnoRows.Count)
                        {
                            xlSheet.Cells[currentRow, 1] = ohnoRows[i]["分類"];
                            xlSheet.Cells[currentRow, 2] = ohnoRows[i]["部門グループ"];
                            xlSheet.Cells[currentRow, 3] = ohnoRows[i]["金額"];
                        }
                        else
                        {
                            // 空白行（セル結合）
                            xlSheet.Cells[currentRow, 1] = "";
                            xlSheet.Cells[currentRow, 2] = "";
                            xlSheet.Cells[currentRow, 3] = "";
                            xlSheet.Range[xlSheet.Cells[currentRow, 1], xlSheet.Cells[currentRow, 3]].Merge();
                            xlSheet.Range[xlSheet.Cells[currentRow, 1], xlSheet.Cells[currentRow, 3]].HorizontalAlignment =
                                Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        }

                        // --- サンミック側（列D～F） ---
                        if (i < sanmicRows.Count)
                        {
                            xlSheet.Cells[currentRow, 4] = sanmicRows[i]["分類"];
                            xlSheet.Cells[currentRow, 5] = sanmicRows[i]["部門グループ"];
                            xlSheet.Cells[currentRow, 6] = sanmicRows[i]["金額"];
                        }
                        else
                        {
                            // 空白行（セル結合）
                            xlSheet.Cells[currentRow, 4] = "";
                            xlSheet.Cells[currentRow, 5] = "";
                            xlSheet.Cells[currentRow, 6] = "";
                            xlSheet.Range[xlSheet.Cells[currentRow, 4], xlSheet.Cells[currentRow, 6]].Merge();
                            xlSheet.Range[xlSheet.Cells[currentRow, 4], xlSheet.Cells[currentRow, 6]].HorizontalAlignment =
                                Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        }

                        currentRow++;
                    }
                }

                // ==========================================
                // 全体の書式設定
                // ==========================================
                var dataRange = xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[currentRow - 1, 6]];

                // フォント設定
                dataRange.Font.Name = "Meiryo UI";
                dataRange.Font.Size = 12;

                // 罫線設定
                dataRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                dataRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                // 金額列の書式設定（C列とF列）
                var amountRangeC = xlSheet.Range[xlSheet.Cells[2, 3], xlSheet.Cells[currentRow - 1, 3]];
                amountRangeC.NumberFormat = "#,##0";
                amountRangeC.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                var amountRangeF = xlSheet.Range[xlSheet.Cells[2, 6], xlSheet.Cells[currentRow - 1, 6]];
                amountRangeF.NumberFormat = "#,##0";
                amountRangeF.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                // 分類・部門グループ列の中央揃え（A・B・D・E列）
                var categoryGroupRangeAB = xlSheet.Range[xlSheet.Cells[2, 1], xlSheet.Cells[currentRow - 1, 2]];
                categoryGroupRangeAB.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                var categoryGroupRangeDE = xlSheet.Range[xlSheet.Cells[2, 4], xlSheet.Cells[currentRow - 1, 5]];
                categoryGroupRangeDE.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                // 列幅自動調整
                xlSheet.Columns.AutoFit();

                // 保存
                xlBook.SaveAs(filePath);
                xlBook.Close(false);
                xlApp.Quit();

                // 保存後に開くか確認
                var result = MessageBox.Show("Excelを保存しました。\n開きますか? ", "保存完了",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(filePath);
                }
            }
            finally
            {
                // COMオブジェクト解放
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 分類の並び順を定義
        /// </summary>
        private int GetCategoryOrder(string category)
        {
            switch (category)
            {
                case "製品売上高": return 1;
                case "原材料売上高": return 2;
                case "製品仕入高": return 3;
                case "原材料仕入高": return 4;
                case "製品在庫": return 5;
                case "原材料在庫": return 6;
                case "仕掛品在庫": return 7;
                default: return 99;
            }
        }
        //=================================================================
        // デザイン関連メソッド
        //=================================================================
        private void ApplySnowManColors()
        {
            // フォーム全体の背景
            this.BackColor = clrmg.AbeLight2;

            //// 条件グループボックス
            //grpBxCondition.BackColor = clrmg.ShopyLight2;
            //grpBxCondition.ForeColor = clrmg.ShopyLight2;

            //// データグループボックス
            //grpBxData.BackColor = clrmg.ShopyLight2;
            //grpBxData.ForeColor = clrmg.ShopyLight2;

            //// 組織グループボックス
            //grpBxOrganization.BackColor = clrmg.ShopyLight2;
            //grpBxOrganization.ForeColor = clrmg.ShopyLight2;

            //// ラベル類
            //lbSituation.ForeColor = clrmg.MemeBase;
            //lbProductClass.ForeColor = clrmg.MemeBase;
            //lbSalesCategory.ForeColor = clrmg.MemeBase;
            //lbSupplier.ForeColor = clrmg.MemeBase;
            //lbSaller.ForeColor = clrmg.MemeBase;
            //lbBumon.ForeColor = clrmg.MemeBase;
            //lbCompany.ForeColor = clrmg.MemeBase;
            //lbYearMonth.ForeColor = clrmg.MemeBase;
            //label2.ForeColor = clrmg.MemeBase;

            //// ListBox 背景
            //listBxSituation.BackColor = clrmg.HikaruLight2;
            //listBxSituation.ForeColor = clrmg.MemeBase;

            //// ListBox 背景
            //listBxSaller.BackColor = clrmg.RauLight2;
            //listBxSaller.ForeColor = clrmg.MemeBase;
            //listBxSupplier.BackColor = clrmg.RauLight2;
            //listBxSupplier.ForeColor = clrmg.MemeBase;
            //listBxBumon.BackColor = clrmg.RauLight2;
            //listBxBumon.ForeColor = clrmg.MemeBase;

            //// CheckBox（データ・商品区分）
            //foreach (Control ctrl in grpBxData.Controls)
            //{
            //    if (ctrl is CheckBox cb)
            //    {
            //        cb.ForeColor = clrmg.ShopyDark2;
            //        cb.BackColor = clrmg.ShopyLight2;
            //    }
            //}

            //// CheckBox（会社選択）
            //foreach (Control ctrl in grpBxOrganization.Controls)
            //{
            //    if (ctrl is CheckBox cb)
            //    {
            //        cb.ForeColor = clrmg.ShopyDark2;
            //        cb.BackColor = clrmg.ShopyLight2;
            //    }
            //}
            //// ボタン類
            //StyleButton(btnExportExcel, clrmg.ShopyBase, Color.White, borderColor: Color.White);
            //StyleButton(btnForm1Back, clrmg.ShopyLight1, Color.White, borderColor: Color.White);
        }

        /// <summary>
        /// パネルのカスタム枠線描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelCustomBorder(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            e.Graphics.Clear(panel.BackColor);

            // アンチエイリアス無効（線をくっきり）
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // テキストを測定
            SizeF textSize = e.Graphics.MeasureString(panel.Text, panel.Font);

            // 枠線色を紺色で
            using (Pen pen = new Pen(Color.FromArgb(32, 55, 100), 1.5f))
            {
                int textPadding = 8;  // 左の余白
                int textWidth = (int)textSize.Width;

                // 枠線を描画（上の線だけタイトル部分を避ける）
                e.Graphics.DrawLine(pen, 1, (int)(textSize.Height / 2), textPadding - 2, (int)(textSize.Height / 2)); // 左上～文字前
                e.Graphics.DrawLine(pen, textPadding + textWidth + 2, (int)(textSize.Height / 2), panel.Width - 2, (int)(textSize.Height / 2)); // 文字後～右上
                e.Graphics.DrawLine(pen, 1, (int)(textSize.Height / 2), 1, panel.Height - 2); // 左線
                e.Graphics.DrawLine(pen, 1, panel.Height - 2, panel.Width - 2, panel.Height - 2); // 下線
                e.Graphics.DrawLine(pen, panel.Width - 2, (int)(textSize.Height / 2), panel.Width - 2, panel.Height - 2); // 右線

                // テキストを描画
                using (SolidBrush brush = new SolidBrush(clrmg.MemeDark1))
                {
                    e.Graphics.DrawString(panel.Text, panel.Font, brush, 8, 0);
                }
            }
        }
    }
}
