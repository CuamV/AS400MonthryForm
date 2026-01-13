using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    public partial class 取引先マスタインポートFm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        private string selectedFilePath;  // 選択されたファイルパスを保持
        string HIZTIM;
        string mf;
        string mfName;
        string BUMONmf = Path.Combine(CMD.mfPath, "BUMON.txt");
        string[] mfTxtNames = new[] { "DLB01TORIHIKI", "DLB02TORIHIKI", "DLB03TORIHIKI" };
        string[] mfTxtPaths = new[]
        {
            Path.Combine(CMD.mfPath, "DLB01TORIHIKI.txt"),
            Path.Combine(CMD.mfPath, "DLB02TORIHIKI.txt"),
            Path.Combine(CMD.mfPath, "DLB03TORIHIKI.txt")
        };

        string mf_bumon = Path.Combine(CMD.mfPath, "TORIHIKI-BUMON.txt");
        string mf_bumonName = "TORIHIKI-BUMON";
        string[] mf_torirollTxtNames = new[] { "SYOSYA", "SIIRE", "HANBAI", "TOKUISAKI", "SYUKKA", "AZUKARI", "UNSOU", "SOUKO" };
        string[] mf_torirollTxtPaths = new[]
        {
            Path.Combine(CMD.mfPath, "SYOSYA.txt"),
            Path.Combine(CMD.mfPath, "SIIRE.txt"),
            Path.Combine(CMD.mfPath, "HANBAI.txt"),
            Path.Combine(CMD.mfPath, "TOKUISAKI.txt"),
            Path.Combine(CMD.mfPath, "SYUKKA.txt"),
            Path.Combine(CMD.mfPath, "AZUKARI.txt"),
            Path.Combine(CMD.mfPath, "UNSOU.txt"),
            Path.Combine(CMD.mfPath, "SOUKO.txt"),
        };
        string mst = "取引先マスタ";
        string mst_bumon = "取引先部門マスタ";
        string mst_torirole = "取引先ロール別マスタ";

        //=========================================================
        // コンストラクタ
        //=========================================================
        public 取引先マスタインポートFm()
        {
            InitializeComponent();
            // デザイナ上のコントロールにイベントを動的に紐付け
            try
            {
                this.pnlCSVインポート.AllowDrop = true;
                this.pictBxCSVインポート.AllowDrop = true;
                this.pnlCSVインポート.DragEnter += PnlCSVインポート_DragEnter;
                this.pnlCSVインポート.DragDrop += PnlCSVインポート_DragDrop;
                this.pictBxCSVインポート.DragEnter += PnlCSVインポート_DragEnter;
                this.pictBxCSVインポート.DragDrop += PnlCSVインポート_DragDrop;
                this.linkLbファイル選択.LinkClicked += LinkLbファイル選択_LinkClicked;
                // 会社が選択されるまでファイル操作を無効化
                this.linkLbファイル選択.Enabled = false;
                this.pictBxCSVインポート.Enabled = false;
                this.cmbBx会社.SelectedIndexChanged += CmbBx会社_SelectedIndexChanged;
            }
            catch
            {
                // デザイナ状態によってはコントロールが見つからないことがあるため静かに無視
            }
        }

        //=========================================================
        // 【コントロール実行メソッド】
        //=========================================================
        /// <summary>
        /// ドラッグ＆ドロップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PnlCSVインポート_DragEnter(object sender, DragEventArgs e)
        {
            // 会社が選択されていない場合は受け付けない
            if (cmbBx会社.SelectedItem == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        /// <summary>
        /// ドラッグ＆ドロップ時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PnlCSVインポート_DragDrop(object sender, DragEventArgs e)
        {
            // 会社選択がない場合は取り込ませない
            if (cmbBx会社.SelectedItem == null)
            {
                MessageBox.Show("会社を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                var file = files[0];
                selectedFilePath = file;
                ShowPreview(file);
            }
        }

        /// <summary>
        /// ファイル選択リンククリックでファイルダイアログ表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkLbファイル選択_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 会社選択がない場合はファイル選択不可
            if (cmbBx会社.SelectedItem == null)
            {
                MessageBox.Show("先に会社を選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = ofd.FileName;
                    ShowPreview(ofd.FileName);
                }
            }
        }

        // 会社選択変更時のハンドラ
        private void CmbBx会社_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabled = cmbBx会社.SelectedItem != null;
            this.linkLbファイル選択.Enabled = enabled;
            this.pictBxCSVインポート.Enabled = enabled;
        }

        private void ImportSelectedFile(string[] lines)
        {
            
            // 簡易インポート処理の例: CSV内容を読み込んで行数を確認し、確認メッセージを表示
            try
            {
                Encoding usedEnc;

                if (MessageBox.Show($"{lines.Length - 1} 件のデータを選択会社({cmbBx会社.SelectedItem})へインポートします。よろしいですか？", "インポート確認", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                // rawLines を読み直してヘッダ含めて渡す
                var raw = ReadAllLinesDetectEncoding(selectedFilePath, out usedEnc);
                var (ok, msg) = fam.ImportMaster(raw, cmbBx会社.SelectedItem.ToString(), BUMONmf,
                    mfTxtNames, mfTxtPaths, mf_bumon, mf_bumonName, mf_torirollTxtNames, mf_torirollTxtPaths,
                    mst, mst_bumon, mst_torirole);
                if (!ok)
                {
                    MessageBox.Show($"インポートに失敗しました: {msg}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("インポート処理を完了しました。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} マスタインポート 1 {CMD.UserName} btn登録_Click {mst}");
                fam.AddLog2($"{HIZTIM} マスタインポート 0 {CMD.UserName} btn登録_Click {mst}が更新されました");
                fam.AddLog2($"{HIZTIM} マスタインポート 0 {CMD.UserName} btn登録_Click {mst_bumon}が登録されました");
                fam.AddLog2($"{HIZTIM} マスタインポート 0 {CMD.UserName} btn登録_Click {mst_torirole}が登録されました");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"インポートに失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// プレビュー表示をモーダルで行う
        /// </summary>
        /// <param name="path"></param>
        private void ShowPreview(string path)
        {
            //----------------------------------------------------
            // ★エラーチェック
            //----------------------------------------------------
            path = selectedFilePath?.Trim();

            // ファイル存在チェック
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
            {
                MessageBox.Show("有効なCSVファイルを指定してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // データ行有無チェック（エンコーディング自動判定: UTF-8 -> Shift_JIS）
                Encoding usedEnc;
                var rawLines = ReadAllLinesDetectEncoding(path, out usedEnc);
                var lines = rawLines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

                if (lines.Length <= 1)
                {
                    MessageBox.Show("CSVにデータがありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ヘッダチェック（最低限のカラムが存在するか）
                //  1:取引先CD  2:部門CD      3:取引正式名称 4:取引先名    5:取引先名カナ 6:取引先略名  7:取引先略名カナ 8:郵便番号
                //  9:電話番号1 10:電話番号2  11:FAX番号1   12:FAX番号2  13:住所1      14:住所1カナ  15:住所2       16:住所2カナ
                //  17:商社区分 18:仕入先区分 19:販売先区分  20:得意先区分 21:出荷先区分  22:預り先区分 23:運送便区分   24:倉庫区分  25:備考
                //----------------------------------------------------
                var headers = SplitCsvLine(lines[0]);
                string[] reqCols = Enum.GetNames(typeof(TORIHIKI_MASTER_IN));

                foreach (var col in reqCols)
                {
                    if (!headers.Contains(col))
                    {
                        MessageBox.Show($"項目「{col}」の列が存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 行単位の入力値チェック
                // 以下、行単位でエラーがあれば、 errorMessages に追加していく
                // 更に、エラー列を追加し、エラー内容を記載してエラー行のみプレビューに表示
                // ----------------------------------------------------
                // 部門マスタファイル読込
                var bumonLines = fam.CheckAndLoadMater(BUMONmf, "部門マスタ", CMD.utf8, 1);
                var bumonSet = new HashSet<string>(bumonLines
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => l.Split(' ')[0].Trim()));

                // 行単位チェック
                var errorRows = new List<string[]>();
                var errorMessages = new List<string>();

                // ヘッダー名→インデックスマップ
                var headerIndex = new Dictionary<string, int>();
                for (int h = 0; h < headers.Length; h++) headerIndex[headers[h]] = h;

                for (int i = 1; i < lines.Length; i++)
                {
                    var fields = SplitCsvLine(lines[i]);
                    // フィールド数が不足している場合は空文字で補完
                    var rowFields = new string[headers.Length];
                    for (int j = 0; j < headers.Length; j++)
                    {
                        if (j < fields.Length) rowFields[j] = fields[j].Trim();
                        else rowFields[j] = string.Empty;
                    }

                    var errs = new List<string>();

                    // 1. 項目数エラー
                    if (fields.Length != reqCols.Length)
                        errs.Add("[項目数エラー] 項目数が17未満または18以上です");

                    // ヘルパー関数
                    Func<string, bool> isDigits = s => !string.IsNullOrEmpty(s) && s.All(char.IsDigit);

                    // 2. 取引先CD
                    var tori = rowFields[headerIndex["取引先CD"]];
                    if (string.IsNullOrEmpty(tori)) errs.Add("[入力・文字数エラー] 取引先CDが入力なし");
                    else if (!Regex.IsMatch(tori, "^[0-9]{7}$")) errs.Add("[入力・文字数エラー] 取引先CDは半角数字7桁で入力してください");

                    // 3. 部門CD
                    var bumon = rowFields[headerIndex["部門CD"]];
                    if (string.IsNullOrEmpty(bumon)) errs.Add("[入力・文字数エラー] 部門CDが入力なし");
                    else if (!Regex.IsMatch(bumon, "^[0-9]{3}$")) errs.Add("[入力・文字数エラー] 部門CDは半角数字3桁で入力してください");

                    // 4. 部門マスタ存在チェック
                    if (!string.IsNullOrEmpty(bumon) && !bumonSet.Contains(bumon)) errs.Add("[部門コード未登録エラー] 部門CDが部門マスタに存在しません");

                    // 5-9 文字数チェック
                    var col = headerIndex["取引先正式名称"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 51) errs.Add("[文字数エラー] 取引先正式名称が51文字以上です");
                    col = headerIndex["取引先名"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 31) errs.Add("[文字数エラー] 取引先名が31文字以上です");
                    col = headerIndex["取引先名カナ"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 41) errs.Add("[文字数エラー] 取引先名カナが41文字以上です");
                    col = headerIndex["取引先略名"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 21) errs.Add("[文字数エラー] 取引先略名が21文字以上です");
                    col = headerIndex["取引先略名カナ"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 31) errs.Add("[文字数エラー] 取引先略名カナが31文字以上です");

                    // 10 郵便番号
                    col = headerIndex["郵便番号"]; var zip = rowFields[col];
                    if (!string.IsNullOrEmpty(zip) && zip != "_")
                    {
                        var zipOnly = zip.Replace("-", "");
                        if (!zipOnly.All(char.IsDigit) || zipOnly.Length >= 8) errs.Add("[入力・文字数エラー] 郵便番号が不正です（半角数字で7桁以下）");
                    }

                    // 11 電話番号1,2
                    string[] tels = { "電話番号1", "電話番号2" };
                    foreach (var tel in tels)
                    {
                        col = headerIndex[tel]; var telValue = rowFields[col];
                        if (!string.IsNullOrEmpty(telValue) && telValue != "_")
                        {
                            var telOnly = telValue.Replace("-", "").Replace("(", "").Replace(")", "");
                            if (!telOnly.All(char.IsDigit) || telOnly.Length >= 15) errs.Add($"[入力・文字数エラー] {tel}が不正です（半角数字で14桁以下）");
                        }
                    }

                    // 12 FAX番号1,2
                    string[] faxes = { "FAX番号1", "FAX番号2" };
                    foreach (var fax in faxes)
                    {
                        col = headerIndex[fax]; var faxValue = rowFields[col];
                        if (!string.IsNullOrEmpty(faxValue) && faxValue != "_")
                        {
                            var faxOnly = faxValue.Replace("-", "").Replace("(", "").Replace(")", "");
                            if (!faxOnly.All(char.IsDigit) || faxOnly.Length >= 15) errs.Add($"[入力・文字数エラー] {fax}が不正です（半角数字で14桁以下）");
                        }
                    }

                    // 13 住所長さ
                    col = headerIndex["住所1"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 35) errs.Add("[文字数エラー] 住所1が35文字以上です");
                    col = headerIndex["住所2"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 35) errs.Add("[文字数エラー] 住所2が35文字以上です");

                    // 14 住所カナ長さ
                    col = headerIndex["住所1カナ"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 37) errs.Add("[文字数エラー] 住所1カナが37文字以上です");
                    col = headerIndex["住所2カナ"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 37) errs.Add("[文字数エラー] 住所2カナが37文字以上です");

                    if (errs.Count > 0)
                    {
                        // 最終列にエラー内容を追加、行番号も追加してエラー行リストに登録
                        var rowWithErr = new string[headers.Length + 2]; // 行番号 + 元データ + エラー内容
                        rowWithErr[0] = (i + 1).ToString(); // 行番号
                        for (int k = 0; k < headers.Length; k++) rowWithErr[k + 1] = rowFields[k];
                        rowWithErr[headers.Length + 1] = string.Join("; ", errs);
                        errorRows.Add(rowWithErr);
                        errorMessages.Add(string.Join("; ", errs));
                    }

                    // 備考長さ
                    col = headerIndex["備考"]; if (!string.IsNullOrEmpty(rowFields[col]) && rowFields[col].Length >= 47) errs.Add("[文字数エラー] 備考が47文字以上です");
                }

                // プレビュー表示
                var previewForm = new Form();
                previewForm.Text = System.IO.Path.GetFileName(path) + " - プレビュー" + $" ({usedEnc.WebName})";
                previewForm.StartPosition = FormStartPosition.CenterParent;
                previewForm.Size = new Size(900, 600);

                var dgv = new DataGridView();
                dgv.Dock = DockStyle.Fill;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;

                if (errorRows.Count > 0)
                {
                    // エラーあり: エラー行のみ表示し、インポート不可（ボタンは表示しない）
                    dgv.Columns.Add("行番号", "行番号");
                    foreach (var h in headers) dgv.Columns.Add(h, h);
                    dgv.Columns.Add("エラー内容", "エラー内容");
                    foreach (var r in errorRows) dgv.Rows.Add(r);

                    previewForm.Controls.Add(dgv);
                    var btnClose = new Button();
                    btnClose.Text = "閉じる";
                    btnClose.Dock = DockStyle.Bottom;
                    btnClose.Height = 36;
                    btnClose.Click += (s, e) => { previewForm.Close(); };
                    previewForm.Controls.Add(btnClose);

                    // show modal - do not allow import
                    previewForm.ShowDialog(this);
                    return;
                }
                else
                {
                    // エラーなし: CSV全行を表示してインポート可能
                    // show header columns
                    foreach (var h in headers) dgv.Columns.Add(h, h);
                    // add all data rows (from lines)
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var rowFields = SplitCsvLine(lines[i]);
                        var row = new string[headers.Length];
                        for (int j = 0; j < headers.Length && j < rowFields.Length; j++) row[j] = rowFields[j];
                        dgv.Rows.Add(row);
                    }

                    previewForm.Controls.Add(dgv);
                    var btn = new Button();
                    btn.Text = "インポート";
                    btn.Dock = DockStyle.Bottom;
                    btn.Height = 36;
                    btn.Click += (s, e) => { previewForm.DialogResult = DialogResult.OK; previewForm.Close(); };
                    previewForm.Controls.Add(btn);

                    if (previewForm.ShowDialog(this) == DialogResult.OK)
                    {
                        // ユーザーがプレビューからインポートを押した -> 実際のインポート処理を呼ぶ
                        selectedFilePath = path;
                        ImportSelectedFile(lines);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV読み込みに失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string[] SplitCsvLine(string line)
        {
            var list = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++; // skip escaped quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            list.Add(sb.ToString());
            return list.ToArray();
        }

        private string[] ReadAllLinesDetectEncoding(string path, out Encoding usedEncoding)
        {
            // Try UTF-8
            try
            {
                var utf8 = new UTF8Encoding(false, true);
                var text = File.ReadAllText(path, utf8);
                // If read succeeded without exception, use UTF-8
                usedEncoding = utf8;
                return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            }
            catch
            {
                // fallthrough to try Shift_JIS
            }

            try
            {
                var sjis = Encoding.GetEncoding(932);
                var text = File.ReadAllText(path, sjis);
                usedEncoding = sjis;
                return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            }
            catch
            {
                // As a last resort, read with system default
                usedEncoding = Encoding.Default;
                return File.ReadAllLines(path, usedEncoding);
            }
        }
    }
}
