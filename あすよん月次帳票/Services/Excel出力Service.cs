using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using あすよん月次帳票.Models;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace あすよん月次帳票.Services
{
    /// <summary>
    /// Excel出力に関する処理を担当
    /// </summary>
    public class Excel出力Service
    {
        /// <summary>
        /// 部門展開データをExcelファイルに出力する
        /// </summary>
        /// <param name="データリスト">出力するデータ</param>
        /// <param name="fName">デフォルトのファイル名</param>
        /// <param name="出力件数">出力する件数(nullの場合は全件)</param>
        public void Export部門展開データToExcel(List<取引先部門展開> データリスト,
                                                string ファイル名 = "AS400取引先マスタ.xlsx", int? 出力件数 = null)
        {
            if (データリスト == null || データリスト.Count == 0)
            {
                MessageBox.Show("出力するデータがありません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 出力件数の確定
            int outputCount = 出力件数 ?? データリスト.Count;
            if (outputCount > データリスト.Count)
            {
                outputCount = データリスト.Count;
            }

            // 保存ダイアログの表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = ファイル名;
                sfd.Filter = "Excelファイル (*.xlsx)|*.xlsx";
                sfd.Title = "Excelファイルの保存先を選択してください";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                // プログレスバー表示
                using (var progressForm = new ProgressForm())
                {
                    progressForm.Show();
                    progressForm.SetMaximum(outputCount);
                    progressForm.SetStatus($"0 / {outputCount} 件出力中...");

                    // Excel出力実行
                    try
                    {
                        Excel出力Create(データリスト, sfd.FileName, outputCount, progressForm);

                        progressForm.Close();

                        // 保存後に開くか確認
                        var result = MessageBox.Show(
                            "Excelを保存しました。\n開きますか？",
                            "保存完了",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        progressForm.Close();
                        throw new Exception($"Excel出力中にエラーが発生しました。\n{ex.Message}", ex);
                    }
                }
            }
        }


        /// <summary>
        /// Excelファイルを作成
        /// </summary>
        private void Excel出力Create(List<取引先部門展開> データリスト, string ファイルパス, int 出力件数, ProgressForm progressForm)
        {
            Application excelApp = null;
            Workbook wkbook = null;
            Worksheet wksheet = null;

            try
            {
                excelApp = new Application();
                wkbook = excelApp.Workbooks.Add();
                wksheet = (Worksheet)wkbook.Worksheets[1];

                // ヘッダー出力
                WriteHeaders(wksheet);

                // 列のフォーマット設定
                SetColumnFormats(wksheet);

                // データ出力
                WriteData(wksheet, データリスト, 出力件数, progressForm);

                // 列幅自動調整
                progressForm.SetStatus("列幅を調整中...");
                wksheet.Columns.AutoFit();

                // 既存ファイルが存在する場合は削除
                DeleteExistingFile(ファイルパス);

                // ファイル保存
                wkbook.SaveAs(ファイルパス, XlFileFormat.xlOpenXMLWorkbook);
            }
            finally
            {
                // クリーンアップ
                CleanupExcelObjects(wkbook, wksheet, excelApp);
            }
        }

        /// <summary>
        /// ヘッダー行を書き込む
        /// </summary>
        private  void WriteHeaders(Worksheet wksheet)
        {
            var headers = Enum.GetNames(typeof(TORIHIKI_MASTER_INOUT));
            for(int i =0; i < headers.Length; i++)
            {
                wksheet.Cells[1, i + 1] = headers[i];
            }
        }

        /// <summary>
        /// 列のフォーマットを設定
        /// </summary>
        /// <param name="worksheet"></param>
        private void SetColumnFormats(Worksheet worksheet)
        {
            // テキスト形式にする列(先頭0を保持)
            var textCols = new[] {
                (int)TORIHIKI_MASTER_INOUT.取引先CD,
                (int)TORIHIKI_MASTER_INOUT.部門CD,
                (int)TORIHIKI_MASTER_INOUT.郵便番号,
                (int)TORIHIKI_MASTER_INOUT.電話番号1,
                (int)TORIHIKI_MASTER_INOUT.電話番号2,
                (int)TORIHIKI_MASTER_INOUT.FAX番号1,
                (int)TORIHIKI_MASTER_INOUT.FAX番号2,
                (int)TORIHIKI_MASTER_INOUT.登録者,
            };

            foreach(var idx in textCols)
            {
                try
                {
                    ((Range)worksheet.Columns[idx + 1]).NumberFormat = "@";
                }
                catch
                {
                    // 例外は無視
                }
            }
        }

        /// <summary>
        /// データを書き込む
        /// </summary>
        private void WriteData(Worksheet wksheet, List<取引先部門展開> データリスト, int 出力件数, ProgressForm progressForm)
        {
            for (int r =0; r < 出力件数 && r < データリスト.Count; r++)
            {
                var item = データリスト[r];
                int rowIdx = r + 2; // データは2行目から

                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.取引先CD + 1].Value = item.取引先CD;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.部門CD + 1].Value = item.部門CD;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.取引先正式名 + 1].Value = item.取引先正式名;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.取引先名 + 1].Value = item.取引先名;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.取引先名カナ + 1].Value = item.取引先名カナ;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.取引先略名 + 1].Value = item.取引先略名;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.取引先略名カナ + 1].Value = item.取引先略名カナ;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.郵便番号 + 1].Value = item.郵便番号;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.電話番号1 + 1].Value = item.電話番号1;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.電話番号2 + 1].Value = item.電話番号2;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.FAX番号1 + 1].Value = item.FAX番号1;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.FAX番号2 + 1].Value = item.FAX番号2;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.住所1 + 1].Value = item.住所1;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.住所1カナ + 1].Value = item.住所1カナ;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.住所2 + 1].Value = item.住所2;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.住所2カナ + 1].Value = item.住所2カナ;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.商社区分 + 1].Value = item.商社区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.仕入先区分 + 1].Value = item.仕入先区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.販売先区分 + 1].Value = item.販売先区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.得意先区分 + 1].Value = item.得意先区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.出荷先区分 + 1].Value = item.出荷先区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.預り先区分 + 1].Value = item.預り先区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.運送便区分 + 1].Value = item.運送便区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.倉庫区分 + 1].Value = item.倉庫区分;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.備考 + 1].Value = item.備考;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.登録者 + 1].Value = item.登録者;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.登録日付 + 1].Value = item.登録日付;
                wksheet.Cells[rowIdx, (int)TORIHIKI_MASTER_INOUT.登録時刻 + 1].Value = item.登録時刻;

                // プログレスバー更新
                progressForm.SetValue(r + 1);
                progressForm.SetStatus($"{r + 1} / {出力件数} 件出力中...");
            }
        }

        /// <summary>
        /// 既存ファイルを削除
        /// </summary>
        private void DeleteExistingFile(string ファイルパス)
        {
            if (File.Exists(ファイルパス))
            {
                try
                {
                    File.Delete(ファイルパス);

                }
                catch (Exception ex)
                {
                    throw new Exception($"既存のファイルを削除できませんでした。\n" +
                        $"ファイルが開かれている可能性があります。\n\nエラー：{ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Excelオブジェクトをクリーンアップ
        /// </summary>
        private void CleanupExcelObjects(Workbook wkbook, Worksheet wksheet, Application excelApp)
        {
            if(wkbook != null)
            {
                wkbook.Close(false);
                Marshal.ReleaseComObject(wkbook);
            }
            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }
            if (wksheet != null)
            {
                Marshal.ReleaseComObject(wksheet);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
