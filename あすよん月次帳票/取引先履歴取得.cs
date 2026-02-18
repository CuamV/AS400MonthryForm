using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using DataTable = System.Data.DataTable;
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
        /// 環境依存文字を置換する辞書
        /// </summary>
        private readonly Dictionary<string, string> environmentCharMap = new Dictionary<string, string>
        {
            { "㈱", "(株)" },
            { "㈲", "(有)" },
            { "㈹", "(代)" },
            { "㈶", "(財)" },
            { "㈳", "(社)" },
            { "㈼", "(学)" },
            { "㈾", "(協)" },
            { "㈿", "(祭)" },
            { "㊀", "(企)" },
            { "㊁", "(資)" },
            { "㊂", "(団)" },
            { "㊃", "(労)" },
        };

        /// <summary>
        /// 文字列を正規化（環境依存文字の置換、タブとスペースの削除、全て大文字の英語をタイトルケースに変換）
        /// </summary>
        /// <param name="input">入力文字列</param>
        /// <param name="replaceEnvChars">環境依存文字を置換するか</param>
        /// <param name="convertToTitleCase">全て大文字の英語をタイトルケースに変換するか（住所用）</param>
        private string NormalizeString(string input, bool replaceEnvChars = false, bool convertToTitleCase = false)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string result = input;

            // 環境依存文字の置換（取引先名関連のみ）
            if (replaceEnvChars)
            {
                foreach (var kvp in environmentCharMap)
                {
                    result = result.Replace(kvp.Key, kvp.Value);
                }
            }

            // タブ文字を削除
            result = result.Replace("\t", "");

            // 全て大文字の英語単語をタイトルケースに変換（住所のみ、スペース削除前に実行）
            // 例: "NEW YORK CITY" → "New York City" → "NewYorkCity"
            if (convertToTitleCase)
            {
                result = ConvertAllCapsToTitleCase(result);
            }

            // すべてのスペース（半角・全角）を削除
            result = Regex.Replace(result, @"[ 　]+", "");

            return result;
        }

        /// <summary>
        /// 全て大文字の英語単語をタイトルケースに変換
        /// </summary>
        private string ConvertAllCapsToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // 英語の大文字とスペースのみで構成されているかチェック
            // 日本語が含まれている場合は処理しない
            var words = input.Split(new[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();
            var isFirstWord = true;

            foreach (var word in words)
            {
                // 英語のアルファベットのみで構成され、全て大文字の場合のみ変換
                if (Regex.IsMatch(word, @"^[A-Z]+$"))
                {
                    // タイトルケースに変換（先頭大文字、残りは小文字）
                    var titleCaseWord = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
                    if (!isFirstWord)
                    {
                        result.Append(" ");
                    }
                    result.Append(titleCaseWord);
                }
                else
                {
                    // そのまま追加
                    if (!isFirstWord)
                    {
                        result.Append(" ");
                    }
                    result.Append(word);
                }
                isFirstWord = false;
            }

            return result.ToString();
        }

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
            // ★AS400現行の取引先マスタを取得
            // ----------------------------------------------------
            GetData_AS400 gdAS400 = new GetData_AS400();
            DataTable dt = gdAS400.GetTorihikiMaster();
            DataTable dtBumon = gdAS400.GetTorihikiHistory(); // 売上履歴（販売先）

            // [newDt出力レイアウト]
            //  1:取引先CD    2:部門CD    3:取引先正式名  4:取引先名  5:取引先名カナ 6:取引先略名  7:取引先略名カナ 8:郵便番号   9:電話番号1  10:電話番号2
            // 11:FAX番号1  12:FAX番号2  13:住所1       14:住所1カナ 15:住所2     16:住所2カナ  17:商社区分     18:仕入先区分 19:販売先区分 20:得意先区分
            // 21:出荷先区分 22:預り先区分 23:運送便区分   24:倉庫区分  25:備考      26:登録者ID   27:登録日      28:登録時刻

            // dtBumon(1:取引先コード 2:部門コード 3:取引先名 4:カナ)から部門マップ作成: 取引先CD -> List<部門CD>
            var bumonMap = new Dictionary<string, List<string>>();
            foreach (DataRow brow in dtBumon.Rows)
            {
                var toriCD = brow[0]?.ToString() ?? "";   // 取引先コード
                var bumonCD = brow[1]?.ToString() ?? "";  // 部門コード
                
                if (string.IsNullOrWhiteSpace(toriCD) || string.IsNullOrWhiteSpace(bumonCD)) continue;
                
                if (!bumonMap.ContainsKey(toriCD))
                    bumonMap[toriCD] = new List<string>();
                if (!bumonMap[toriCD].Contains(bumonCD))
                    bumonMap[toriCD].Add(bumonCD);
            }

            // 部門マスタ(BUMONmf)の読み込み
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
            foreach (var kvp in bumonMap)
            {
                foreach (var bumonCD in kvp.Value)
                {
                    if (allowedBumons.Contains(bumonCD))
                    {
                        if (!filteredBumonMap.ContainsKey(kvp.Key))
                            filteredBumonMap[kvp.Key] = new List<string>();
                        if (!filteredBumonMap[kvp.Key].Contains(bumonCD))
                            filteredBumonMap[kvp.Key].Add(bumonCD);
                    }
                }
            }

            // dtをnewDtに変換（AS400マスタ → 新フォーマット）
            var newDt = new DataTable();
            
            // newDtの列を定義
            var newDtColumns = Enum.GetNames(typeof(TORIHIKI_MASTER_INOUT));
            foreach (var colName in newDtColumns)
            {
                newDt.Columns.Add(colName);
            }

            // dtの各行を処理して、newDtに変換
            foreach (DataRow origRow in dt.Rows)
            {
                var 取引先CD = origRow[(int)TORIHIKI_AS400_MASTER.取引先コード]?.ToString() ?? "";

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
                    var newRow = newDt.NewRow();

                    // マッピング処理（文字列正規化を適用）
                    newRow[(int)TORIHIKI_MASTER_INOUT.取引先CD] = NormalizeString(取引先CD);
                    newRow[(int)TORIHIKI_MASTER_INOUT.部門CD] = NormalizeString(bumonCD);
                    // 取引先名関連は元の表記を維持（パスカルケース変換なし）
                    newRow[(int)TORIHIKI_MASTER_INOUT.取引先正式名] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.取引先正式名]?.ToString() ?? "", replaceEnvChars: true, convertToTitleCase: false);
                    newRow[(int)TORIHIKI_MASTER_INOUT.取引先名] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.取引先名]?.ToString() ?? "", replaceEnvChars: true, convertToTitleCase: false);
                    newRow[(int)TORIHIKI_MASTER_INOUT.取引先名カナ] = ""; // null値
                    newRow[(int)TORIHIKI_MASTER_INOUT.取引先略名] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.略名]?.ToString() ?? "", replaceEnvChars: true, convertToTitleCase: false);
                    newRow[(int)TORIHIKI_MASTER_INOUT.取引先略名カナ] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.カナ名]?.ToString() ?? "", replaceEnvChars: true, convertToTitleCase: false);
                    newRow[(int)TORIHIKI_MASTER_INOUT.郵便番号] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.郵便番号]?.ToString() ?? "");
                    newRow[(int)TORIHIKI_MASTER_INOUT.電話番号1] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.電話番号]?.ToString() ?? "");
                    newRow[(int)TORIHIKI_MASTER_INOUT.電話番号2] = ""; // null値
                    newRow[(int)TORIHIKI_MASTER_INOUT.FAX番号1] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.FAX番号]?.ToString() ?? "");
                    newRow[(int)TORIHIKI_MASTER_INOUT.FAX番号2] = ""; // null値
                    // 住所のみパスカルケース変換を適用
                    newRow[(int)TORIHIKI_MASTER_INOUT.住所1] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.住所1]?.ToString() ?? "", replaceEnvChars: false, convertToTitleCase: true);
                    newRow[(int)TORIHIKI_MASTER_INOUT.住所1カナ] = ""; // null値
                    newRow[(int)TORIHIKI_MASTER_INOUT.住所2] = NormalizeString(origRow[(int)TORIHIKI_AS400_MASTER.住所2]?.ToString() ?? "", replaceEnvChars: false, convertToTitleCase: true);
                    newRow[(int)TORIHIKI_MASTER_INOUT.住所2カナ] = ""; // null値
                    
                    // 区分フィールド（"1"の場合のみ"1"、それ以外は空文字）
                    var 仕入先区分 = origRow[(int)TORIHIKI_AS400_MASTER.仕入先区分]?.ToString() ?? "";
                    var 販売先区分 = origRow[(int)TORIHIKI_AS400_MASTER.販売先区分]?.ToString() ?? "";
                    var 出荷先区分 = origRow[(int)TORIHIKI_AS400_MASTER.出荷先区分]?.ToString() ?? "";
                    var 運送便区分 = origRow[(int)TORIHIKI_AS400_MASTER.運送便区分]?.ToString() ?? "";
                    var 倉庫区分 = origRow[(int)TORIHIKI_AS400_MASTER.倉庫区分]?.ToString() ?? "";

                    newRow[(int)TORIHIKI_MASTER_INOUT.商社区分] = 仕入先区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.仕入先区分] = 仕入先区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.販売先区分] = 販売先区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.得意先区分] = 販売先区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.出荷先区分] = 出荷先区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.預り先区分] = 販売先区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.運送便区分] = 運送便区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.倉庫区分] = 倉庫区分 == "1" ? "1" : "";
                    newRow[(int)TORIHIKI_MASTER_INOUT.備考] = ""; // null値
                    newRow[(int)TORIHIKI_MASTER_INOUT.登録者] = CMD.UserID;
                    newRow[(int)TORIHIKI_MASTER_INOUT.登録日付] = CMD.HIZ;
                    newRow[(int)TORIHIKI_MASTER_INOUT.登録時刻] = DateTime.Now.ToString("HHmmss");

                    newDt.Rows.Add(newRow);
                }
            }

            // ----------------------------------------------------
            // ★取引先コードでソート
            // ----------------------------------------------------
            DataView dv = newDt.DefaultView;
            dv.Sort = "取引先CD ASC, 部門CD ASC";
            newDt = dv.ToTable();

            // ----------------------------------------------------
            // ★出力件数の入力
            // ----------------------------------------------------
            int outputCount = newDt.Rows.Count; // デフォルトは全件
            string inputValue = Interaction.InputBox(
                $"出力件数を入力してください。\n（全{newDt.Rows.Count}件）\n\n※空欄の場合は全件出力されます。",
                "出力件数指定",
                "",
                -1,
                -1);

            // 空欄の場合は全件出力
            if (string.IsNullOrEmpty(inputValue))
            {
                outputCount = newDt.Rows.Count;
            }
            else
            {
                // 数値として解析
                if (int.TryParse(inputValue, out int parsedCount))
                {
                    if (parsedCount > 0 && parsedCount <= newDt.Rows.Count)
                    {
                        outputCount = parsedCount;
                    }
                    else if (parsedCount > newDt.Rows.Count)
                    {
                        MessageBox.Show($"指定件数が全件数（{newDt.Rows.Count}件）を超えています。\n全件で出力します。",
                            "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        outputCount = newDt.Rows.Count;
                    }
                    else
                    {
                        MessageBox.Show("出力件数は1以上を指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("数値を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
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
                progressForm.SetMaximum(outputCount);
                progressForm.SetStatus($"0 / {outputCount} 件処理中...");

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

                    // データ行をnewDtから出力（outputCount件まで）
                    for (int r = 0; r < outputCount; r++)
                    {
                        for (int c = 0; c < newDt.Columns.Count; c++)
                        {
                            var val = newDt.Rows[r][c]?.ToString() ?? string.Empty;
                            // 全ての列に値を設定（NumberFormatで既にテキスト指定済み）
                            worksheet.Cells[r + 2, c + 1].Value = val;
                        }
                        
                        // プログレスバー更新
                        progressForm.SetValue(r + 1);
                        progressForm.SetStatus($"{r + 1} / {outputCount} 件処理中...");
                    }

                    // 列幅自動調整
                    progressForm.SetStatus("列幅を調整中...");
                    worksheet.Columns.AutoFit();

                    // 既存ファイルが存在する場合は削除
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (Exception ex)
                        {
                            progressForm.Close();
                            MessageBox.Show($"既存ファイルを削除できませんでした。\nファイルが開かれている可能性があります。\n\nエラー: {ex.Message}",
                                "保存エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // 保存
                    try
                    {
                        workbook.SaveAs(sfd.FileName, XlFileFormat.xlOpenXMLWorkbook);
                    }
                    catch (Exception ex)
                    {
                        progressForm.Close();
                        MessageBox.Show($"Excelファイルの保存に失敗しました。\n\nエラー: {ex.Message}",
                            "保存エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    progressForm.Close();

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

