//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace あすよん月次帳票
//{
//    internal class XXX
//    {// Excelエクスポートボタンクリック
//        private async void btnExportExcel_Click(object sender, EventArgs e)
//        {
//            if (mergedSummary == null || mergedSummary.Rows.Count == 0)
//            {
//                MessageBox.Show("エクスポートするデータがありません。");
//                return;
//            }
//            try
//            {
//                // 保存ダイアログ
//                SaveFileDialog sfd = new SaveFileDialog();
//                sfd.Filter = "マクロ有効Excelファイル|*.xlsm";
//                sfd.Title = "保存先を指定してください";
//                sfd.FileName = "月次データ.xlsm";

//                if (sfd.ShowDialog() != DialogResult.OK) return;
//                string filePath = sfd.FileName;
//                // Excelアプリケーション作成
//                var xlApp = new Excel.Application { Visible = false };

//                // 新規ブック作成
//                Excel.Workbook xlBook = xlApp.Workbooks.Add();
//                Excel.Worksheet xlSheet = (Excel.Worksheet)xlBook.Sheets[1];
//                xlSheet.Name = "Data";

//                // ヘッダー
//                string[] Headers = { "カテゴリ", "取引区分", "部門CD", "販売先CD", "販売先名", "数量計", "金額計" };
//                for (int i = 0; i < Headers.Length; i++) xlSheet.Cells[1, i + 1] = Headers[i];

//                // CD列桁数ルール
//                var cdPadding = new Dictionary<string, int>
//                {
//                    ["販売先CD"] = 7,
//                };

//                // CD列文字列形式に先に設定
//                foreach (var colName in cdPadding.Keys)
//                {
//                    if (mergedSummary.Columns.Contains(colName))
//                    {
//                        int colIndex = mergedSummary.Columns[colName].Ordinal + 1;
//                        Excel.Range colRange = xlSheet.Columns[colIndex];
//                        colRange.NumberFormat = "@";  // 文字列形式に先に設定
//                    }
//                }

//                // データ
//                int rows = mergedSummary.Rows.Count;
//                int cols = mergedSummary.Columns.Count;

//                // 二次元配列に変換
//                object[,] dataArray = new object[rows, cols];
//                for (int r = 0; r < rows; r++)
//                {
//                    for (int c = 0; c < cols; c++)
//                    {
//                        string colName = mergedSummary.Columns[c].ColumnName;
//                        string value = mergedSummary.Rows[r][c]?.ToString() ?? "";

//                        // CD列は左ゼロ埋め
//                        if (cdPadding.ContainsKey(colName))
//                        {
//                            value = value.PadLeft(cdPadding[colName], '0');
//                            value = "'" + value;  // 文字列としてExcelに渡す
//                        }
//                        dataArray[r, c] = value;
//                    }
//                }

//                // CD列だけ文字列形式に設定（存在する列のみ）
//                foreach (var colName in cdPadding.Keys)
//                {
//                    if (mergedSummary.Columns.Contains(colName))
//                    {
//                        int colIndex = mergedSummary.Columns[colName].Ordinal + 1;
//                        Excel.Range colRange = xlSheet.Columns[colIndex];
//                        colRange.NumberFormat = "@";  // 文字列形式
//                    }
//                }

//                // 範囲に一括代入
//                Excel.Range startCell = (Excel.Range)xlSheet.Cells[2, 1];
//                Excel.Range endCell = xlSheet.Cells[rows + 1, cols];
//                Excel.Range writeRange = xlSheet.Range[startCell, endCell];
//                writeRange.Value = dataArray;

//                //// テスト用(dgvDataOhno/dgvDataScar/dgvDataSdus)
//                //if (dgvDataOhno.DataSource is DataTable dtOhno)
//                //{ 
//                // Excel.Worksheet sheet2 = xlBook.Sheets.Add(After: xlBook.Sheets[xlBook.Sheets.Count]); 
//                // sheet2.Name = "オーノ"; // int rCount = dtOhno.Rows.Count; // int cCount = dtOhno.Columns.Count;
//                // // ヘッダー 
//                // for (int c = 0; c < cCount; c++) sheet2.Cells[1, c + 1] = dtOhno.Columns[c].ColumnName; 
//                // // データ 
//                // object[,] arr = new object[rCount, cCount]; 
//                // for (int r = 0; r < rCount; r++) 
//                // for (int c = 0; c < cCount; c++) 
//                // arr[r, c] = dtOhno.Rows[r][c]?.ToString() ?? ""; 
//                // Excel.Range start = sheet2.Cells[2, 1]; 
//                // Excel.Range end = sheet2.Cells[rCount + 1, cCount]; 
//                // sheet2.Range[start, end].Value = arr;
//                // }
//                //if (dgvDataScar.DataSource is DataTable dtScar) 
//                //{ 
//                // Excel.Worksheet sheet3 = xlBook.Sheets.Add(After: xlBook.Sheets[xlBook.Sheets.Count]); 
//                // sheet3.Name = "サンミックカーペット"; 
//                // int rCount = dtScar.Rows.Count; 
//                // int cCount = dtScar.Columns.Count; 
//                // // ヘッダー 
//                // for (int c = 0; c < cCount; c++) sheet3.Cells[1, c + 1] = dtScar.Columns[c].ColumnName; 
//                // // データ 
//                // object[,] arr = new object[rCount, cCount]; 
//                // for (int r = 0; r < rCount; r++) 
//                // for (int c = 0; c < cCount; c++) 
//                // arr[r, c] = dtScar.Rows[r][c]?.ToString() ?? ""; 
//                // Excel.Range start = sheet3.Cells[2, 1]; 
//                // Excel.Range end = sheet3.Cells[rCount + 1, cCount]; 
//                // sheet3.Range[start, end].Value = arr; 
//                //} 
//                //if (dgvDataSdus.DataSource is DataTable dtSdus) 
//                //{ 
//                // Excel.Worksheet sheet4 = xlBook.Sheets.Add(After: xlBook.Sheets[xlBook.Sheets.Count]); 
//                // sheet4.Name = "サンミックダスコン"; 
//                // int rCount = dtSdus.Rows.Count; 
//                // int cCount = dtSdus.Columns.Count; 
//                // // ヘッダー 
//                // for (int c = 0; c < cCount; c++) sheet4.Cells[1, c + 1] = dtSdus.Columns[c].ColumnName; 
//                // // データ 
//                // object[,] arr = new object[rCount, cCount]; 
//                // for (int r = 0; r < rCount; r++) 
//                // for (int c = 0; c < cCount; c++) 
//                // arr[r, c] = dtSdus.Rows[r][c]?.ToString() ?? ""; 
//                // Excel.Range start = sheet4.Cells[2, 1]; 
//                // Excel.Range end = sheet4.Cells[rCount + 1, cCount]; 
//                // sheet4.Range[start, end].Value = arr; 
//                //} 

//                //マクロ有効形式で保存
//                xlBook.SaveAs(filePath, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
//                xlBook.Close(false); xlApp.Quit();

//                // 保存後に開くか確認
//                var result = MessageBox.Show(
//                "Excelを保存しました。\n開きますか?", "保存完了", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (result == DialogResult.Yes)
//                {
//                    System.Diagnostics.Process.Start(filePath);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Excelエクスポート中にエラーが発生しました: " + ex.Message);
//            }
//        }
//    }
//}
