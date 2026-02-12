using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using Path = System.IO.Path;

namespace あすよん月次帳票
{
    internal class 取引先履歴取得
    {
        FormAction fam = new FormAction();

        string InMf = Path.Combine(CMD.mfPath, "AS400_TORIHIKI.csv");
        string mst = "AS400取引先マスタ";
        string ToriHis = Path.Combine(CMD.mfPath, "TORIHIKI_history.csv");
        string mst2 = "取引先履歴";
        string BUMONmf = Path.Combine(CMD.mfPath, "BUMON.txt");

        /// <summary>
        /// CSVファイルをTextFieldParserで読み込み（ダブルクォート対応）
        /// </summary>
        private List<string[]> LoadCsvWithParser(string filePath, string masterName, int skipLines = 1)
        {
            var result = new List<string[]>();

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"{masterName}ファイルが見つかりません。\n{filePath}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            using (TextFieldParser parser = new TextFieldParser(filePath, CMD.utf8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = true;

                // ヘッダー行をスキップ
                for (int i = 0; i < skipLines && !parser.EndOfData; i++)
                {
                    parser.ReadLine();
                }

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] fields = parser.ReadFields();
                        result.Add(fields);
                    }
                    catch (MalformedLineException)
                    {
                        // 不正な行はスキップ
                        continue;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 取引先データを部門コードで展開してDataTableを作成
        /// </summary>
        /// <returns></returns>
        internal void Create取引先部門展開Table()
        {
            // ----------------------------------------------------
            // ★ファイル取得
            // ----------------------------------------------------
            //ファイル有無チェック＆読込（TextFieldParserでダブルクォート対応）
            var lines = LoadCsvWithParser(InMf, mst, 1);
            var lines_bumon = LoadCsvWithParser(ToriHis, mst2, 1);

            // [出力レイアウト]
            //  1:取引先CD    2:部門CD    3:取引先正式名  4:取引先名  5:取引先名カナ 6:取引先略名  7:取引先略名カナ 8:郵便番号   9:電話番号1  10:電話番号2
            // 11:FAX番号1  12:FAX番号2  13:住所1       14:住所1カナ 15:住所2     16:住所2カナ  17:商社区分     18:仕入先区分 19:販売先区分 20:得意先区分
            // 21:出荷先区分 22:預り先区分 23:運送便区分   24:倉庫区分  25:備考      26:登録者ID   27:登録日      28:登録時刻
            //----------------------------------------------------
            // ★Excel出力処理 (取引先マスタ × 取引先部門マスタで部門分を展開)
            //----------------------------------------------------
            // 部門マップ作成: 取引先CD -> List<部門CD>
            var bumonMap = new Dictionary<string, List<string>>();
            foreach (var bparts in lines_bumon)
            {
                if (bparts == null || bparts.Length < 2) continue;
                var toriCD = bparts[0];
                var bumonCD = bparts[1];
                if (string.IsNullOrWhiteSpace(toriCD) || string.IsNullOrWhiteSpace(bumonCD)) continue;
                if (!bumonMap.ContainsKey(toriCD))
                    bumonMap[toriCD] = new List<string>();
                bumonMap[toriCD].Add(bumonCD);
            }

            // 部門マスタ(BUMONmf)の 4番目の項目が会社名
            var allowedBumons = new HashSet<string>();
            var bumonMasterLines = fam.CheckAndLoadMater(BUMONmf, "部門マスタ", CMD.utf8, 1);
            foreach (var bmLine in bumonMasterLines)
            {
                if (string.IsNullOrWhiteSpace(bmLine)) continue;
                var bmParts = bmLine.Split(' ');
                if (bmParts.Length >= 4)
                {
                    var bmCD = bmParts[0];
                    allowedBumons.Add(bmCD);
                }
            }

            // 指定会社に属する部門のみでフィルタリングした部門マップを作成
            var filteredBumonMap = new Dictionary<string, List<string>>();
            foreach (var b in allowedBumons)
            {
                foreach (var kvp in bumonMap)
                {
                    if (kvp.Value.Contains(b))
                    {
                        if (!filteredBumonMap.ContainsKey(kvp.Key))
                            filteredBumonMap[kvp.Key] = new List<string>();
                        filteredBumonMap[kvp.Key].Add(b);
                    }
                }
            }

            // 出力行リストを作成
            var outRows = new List<string[]>();

            // 取引先マスタの各行を処理
            foreach (var parts in lines)
            {
                if (parts == null || parts.Length < 17) continue; // 最低限のフィールド数チェック

                // 取引先マスタのレイアウト: 
                // 1:取引先CD 2:取引先正式名 3:取引先名 4:取引先名カナ 5:取引先略名 6:取引先略名カナ 7:郵便番号 8:電話番号1
                // 9:電話番号2 10:FAX番号1 11:FAX番号2 12:住所1 13:住所1カナ 14:住所2 15:住所2カナ 16:商社区分
                // 17:仕入先区分 18:販売先区分 19:得意先区分 20:出荷先区分 21:預り先区分 22:運送便区分 23:倉庫区分 24:備考
                // 25:登録者 26:登録日付 27:登録時刻

                var 取引先CD = parts[0];

                // この取引先に紐づく部門リストを取得（なければ"000"を設定）
                List<string> bumonCodesToProcess;
                if (filteredBumonMap.TryGetValue(取引先CD, out var bumonList) && bumonList.Count > 0)
                {
                    bumonCodesToProcess = bumonList;
                }
                else
                {
                    // 部門が見つからない場合は"000"として出力
                    bumonCodesToProcess = new List<string> { "000" };
                }

                // 部門ごとに行を作成
                foreach (var bumonCD in bumonCodesToProcess)
                {
                    var row = new string[Enum.GetNames(typeof(TORIHIKI_MASTER_INOUT)).Length];

                    // TORIHIKI_MASTER_OUT のレイアウトに合わせて配置
                    row[(int)TORIHIKI_MASTER_INOUT.取引先CD] = parts[0];
                    row[(int)TORIHIKI_MASTER_INOUT.部門CD] = bumonCD; // 部門CDを追加
                    row[(int)TORIHIKI_MASTER_INOUT.取引先正式名] = parts.Length > 1 ? parts[1] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.取引先名] = parts.Length > 2 ? parts[2] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.取引先名カナ] = parts.Length > 3 ? parts[3] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.取引先略名] = parts.Length > 4 ? parts[4] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.取引先略名カナ] = parts.Length > 5 ? parts[5] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.郵便番号] = parts.Length > 6 ? parts[6] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.電話番号1] = parts.Length > 7 ? parts[7] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.電話番号2] = parts.Length > 8 ? parts[8] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.FAX番号1] = parts.Length > 9 ? parts[9] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.FAX番号2] = parts.Length > 10 ? parts[10] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.住所1] = parts.Length > 11 ? parts[11] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.住所1カナ] = parts.Length > 12 ? parts[12] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.住所2] = parts.Length > 13 ? parts[13] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.住所2カナ] = parts.Length > 14 ? parts[14] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.商社区分] = parts.Length > 15 ? parts[15] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.仕入先区分] = parts.Length > 16 ? parts[16] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.販売先区分] = parts.Length > 17 ? parts[17] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.得意先区分] = parts.Length > 18 ? parts[18] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.出荷先区分] = parts.Length > 19 ? parts[19] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.預り先区分] = parts.Length > 20 ? parts[20] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.運送便区分] = parts.Length > 21 ? parts[21] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.倉庫区分] = parts.Length > 22 ? parts[22] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.備考] = parts.Length > 23 ? parts[23] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.登録者] = parts.Length > 24 ? parts[24] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.登録日付] = parts.Length > 25 ? parts[25] : "";
                    row[(int)TORIHIKI_MASTER_INOUT.登録時刻] = parts.Length > 26 ? parts[26] : "";

                    outRows.Add(row);
                }
            }

            // Excel保存ダイアログ表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"{mst}.xlsx";
                sfd.Filter = "Excelファイル (*.xlsx)|*.xlsx";
                sfd.Title = "保存先を指定してください";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                string filePath = sfd.FileName;

                // プログレスバー表示
                ProgressForm progressForm = new ProgressForm();
                progressForm.Show();
                progressForm.SetMaximum(outRows.Count);
                progressForm.SetStatus($"0 / {outRows.Count} 件処理中...");

                Microsoft.Office.Interop.Excel.Application excelApp = null;
                Workbook workbook = null;
                Worksheet worksheet = null;
                try
                {
                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    workbook = excelApp.Workbooks.Add();
                    worksheet = (Worksheet)workbook.Worksheets[1];

                    // ヘッダー出力
                    var headers = Enum.GetNames(typeof(TORIHIKI_MASTER_INOUT));
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    // 取引先CD、部門CD、郵便番号、電話、FAX は文字列として扱い先頭0を維持
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
                    // 各列をテキストフォーマットに設定してから値を書き込む
                    foreach (var idx in textCols)
                    {
                        try
                        {
                            ((Range)worksheet.Columns[idx + 1]).NumberFormat = "@"; // テキストフォーマット
                        }
                        catch
                        {
                            // 念のため例外は無視して続行
                        }
                    }

                    // データ行
                    for (int r = 0; r < outRows.Count; r++)
                    {
                        for (int c = 0; c < outRows[r].Length; c++)
                        {
                            var val = outRows[r][c] ?? string.Empty;
                            // テキスト扱いの列は先頭にアポストロフィを付けてExcel側の自動変換を防ぐ
                            if (textCols.Contains(c))
                                worksheet.Cells[r + 2, c + 1].Value = "'" + val;
                            else
                                worksheet.Cells[r + 2, c + 1].Value = val;
                        }
                        
                        // プログレスバー更新
                        progressForm.SetValue(r + 1);
                        progressForm.SetStatus($"{r + 1} / {outRows.Count} 件処理中...");
                    }

                    // 列幅自動調整
                    progressForm.SetStatus("列幅を調整中...");
                    worksheet.Columns.AutoFit();

                    // 保存
                    workbook.SaveAs(sfd.FileName, XlFileFormat.xlOpenXMLWorkbook);

                    // 保存後に開くか確認
                    var result = MessageBox.Show("Excelを保存しました。\n開きますか?", "保存完了", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(filePath);
                    }
                }
                finally
                {
                    // クリーンアップ
                    if (workbook != null)
                    {
                        workbook.Close(false);
                        Marshal.ReleaseComObject(workbook);
                    }
                    if (excelApp != null)
                    {
                        excelApp.Quit();
                        Marshal.ReleaseComObject(excelApp);
                    }
                    if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                    workbook = null;
                    worksheet = null;
                    excelApp = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }
    }
}
