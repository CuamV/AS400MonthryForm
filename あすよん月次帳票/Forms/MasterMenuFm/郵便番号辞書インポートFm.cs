using Ohno.Db;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using DataTable = System.Data.DataTable;
using Path = System.IO.Path;

namespace あすよん月次帳票
{
    public partial class 郵便番号辞書インポートFm : Form
    {
        //========================================================
        // インスタンス
        //========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        private string csvFilePath = "";
        string HIZTIM;
        string mst = "郵便番号辞書インポート";

        //========================================================
        // コンストラクタ
        //========================================================
        public 郵便番号辞書インポートFm()
        {
            InitializeComponent();

            this.Load += 郵便番号辞書インポートFm_Load;
        }

        private void 郵便番号辞書インポートFm_Load(object sender, EventArgs e)
        {
            // 初期設定
            lblステータス.Text = "準備完了";
            progressBar.Visible = false;
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        ///参照ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn参照_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*";
                ofd.Title = "郵便番号辞書CSVファイルを選択してください";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    csvFilePath = ofd.FileName;
                    txtBxファイル選択.Text = csvFilePath;
                }
            }
        }

        /// <summary>
        /// インポートボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnインポート_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btnインポート_Click");

            if (string.IsNullOrEmpty(csvFilePath))
            {
                MessageBox.Show("CSVファイルを選択してください。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "現行の郵便番号辞書を削除して、新しいデータへ差替えます。\n実行してよろしいですか？",
                "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                // プログレスバー表示
                progressBar.Visible = true;
                progressBar.Value = 0;
                lblステータス.Text = "インポート中...";
                Application.DoEvents();

                // タイムスタンプ
                string timestamp = DateTime.Now.ToString("yyyyMMdd.HHmmss");

                // --------------------------------------------
                // 1. 既存データをCSVでバックアップ
                // --------------------------------------------
                lblステータス.Text = "既存データをバックアップ中...";
                Application.DoEvents();
                BackupExistingData(timestamp);
                progressBar.Value = 20;

                // --------------------------------------------
                // 2. データベース接続
                // --------------------------------------------
                var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");

                // --------------------------------------------
                // 3. CSVデータ読み込み
                // --------------------------------------------
                lblステータス.Text = "CSVデータを読み込み中...";
                Application.DoEvents();

                var lines = File.ReadLines(csvFilePath, Encoding.GetEncoding("Shift_JIS")).ToList();
                int totalCount = lines.Count;
                int processedCount = 0;

                // バルクインサート用DataTable作成
                DataTable bulkData = new DataTable();
                foreach (var col in Enum.GetNames(typeof(POSTALCODES)))
                {
                    bulkData.Columns.Add(col, typeof(string));
                }

                foreach (var line in lines)
                {
                    var fields = line.Split(',').Select(f => f.Trim('"')).ToArray();

                    // 15項目あることを確認
                    if (fields.Length >= 15)
                    {
                        DataRow row = bulkData.NewRow();
                        row["郵便番号"] = fields[2];           // 3項目目:  郵便番号(7桁)
                        row["都道府県名ｶﾅ"] = fields[3];       // 4項目目
                        row["市区町村名ｶﾅ"] = fields[4];       // 5項目目
                        row["町域名ｶﾅ"] = fields[5];           // 6項目目
                        row["都道府県名"] = fields[6];          // 7項目目
                        row["市区町村名"] = fields[7];          // 8項目目
                        row["町域名"] = fields[8];              // 9項目目
                        bulkData.Rows.Add(row);
                    }

                    processedCount++;
                    if (processedCount % 5000 == 0)
                    {
                        int progress = 30 + (int)((double)processedCount / totalCount * 40);
                        progressBar.Value = progress;
                        lblステータス.Text = $"CSVデータ読み込み中... {processedCount:N0}/{totalCount:N0}";
                        Application.DoEvents();
                    }
                }

                progressBar.Value = 70;

                // --------------------------------------------
                // 4. バルクインサート実行
                // --------------------------------------------
                lblステータス.Text = "データベースへ書き込み中...";
                Application.DoEvents();

                using (SqlConnection conn = (SqlConnection)dbManager.Connection)
                {
                    conn.Open();
                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 現行削除
                            var delcommand = new SqlCommand("DELETE FROM PostalCodes", conn, trans);
                            delcommand.ExecuteNonQuery();

                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                            {
                                bulkCopy.DestinationTableName = "PostalCodes";
                                bulkCopy.BatchSize = 10000;
                                bulkCopy.BulkCopyTimeout = 300; // 5分

                                // 列マッピング
                                bulkCopy.ColumnMappings.Add("郵便番号", "郵便番号");
                                bulkCopy.ColumnMappings.Add("都道府県名ｶﾅ", "都道府県名ｶﾅ");
                                bulkCopy.ColumnMappings.Add("市区町村名ｶﾅ", "市区町村名ｶﾅ");
                                bulkCopy.ColumnMappings.Add("町域名ｶﾅ", "町域名ｶﾅ");
                                bulkCopy.ColumnMappings.Add("都道府県名", "都道府県名");
                                bulkCopy.ColumnMappings.Add("市区町村名", "市区町村名");
                                bulkCopy.ColumnMappings.Add("町域名", "町域名");

                                bulkCopy.WriteToServer(bulkData);
                            }
                            trans.Commit();
                        }
                        //catch(InvalidOperationException ioe)
                        //{
                        //}
                        catch (Exception ex1)
                        {
                            // エラー時はロールバック(動作取り消し)
                            trans.Rollback();
                            // エラーメッセージを出力
                            CatchErr(ex1, "インポートに失敗しました。", "エラーが発生しました。");
                            lblステータス.Text = "DBへの差替え中にエラーのためロールバックしました";
                            // プログレスバー非表示
                            progressBar.Visible = false;
                            return;
                        }
                    }
                }
                progressBar.Value = 90;

                // --------------------------------------------
                // 5. アップロードCSVをリネームして保存
                // --------------------------------------------
                lblステータス.Text = "アップロードCSVを保存中...";
                Application.DoEvents();
                SaveUploadCsv(csvFilePath);

                progressBar.Value = 100;
                lblステータス.Text = "インポート完了！";

                // ログ出力
                string HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm: ss}";
                fam.AddLog($"{HIZTIM} 郵便番号辞書インポート 1 {CMD.UserName} {bulkData.Rows.Count}件");

                MessageBox.Show($"{bulkData.Rows.Count:N0}件のデータをインポートしました。", "完了",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex2)
            {
                CatchErr(ex2, "インポートに失敗しました。", "エラーが発生しました。");
                lblステータス.Text = "エラーが発生しました。";
            }
            finally
            {
                progressBar.Visible = false;
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
            // Form5 のインスタンスを取得して表示
            // 名前で探すと見つからない場合があるため、型で検索して取得する
            var form5 = Application.OpenForms.OfType<MasterMenuFm>().FirstOrDefault();
            if (form5 != null)
            {
                form5.Show();
            }
            // 取引先マスタForm を閉じる
            this.Close();
        }

        //=================================================================
        // 処理メソッド
        //=================================================================
        private void CatchErr(Exception err, string errMsg, string lblMsg)
        {
            MessageBox.Show($"{errMsg}\n{err.Message}", "エラー",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblステータス.Text = lblMsg;
            // {ioe.StackTrace} 後ほどログへ出力
            // エラーメッセージを出力
            fam.ErrLog(err, mst);

            // エラーログ出力
            string HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} {mst}エラー 1 {CMD.UserName} {err.Message}");
        }

        /// <summary>
        /// 既存データをCSVでバックアップ（ZIP圧縮）
        /// </summary>
        /// <param name="timestamp"></param>
        private void BackupExistingData(string timestamp)
        {
            try
            {
                // データベース接続
                var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");

                // 既存データ取得
                string sql = "SELECT 郵便番号, 都道府県名ｶﾅ, 市区町村名ｶﾅ, 町域名ｶﾅ, 都道府県名, 市区町村名, 町域名 FROM PostalCodes";
                DataTable dt = dbManager.GetDataTable(sql, CommandType.Text);

                if (dt.Rows.Count == 0)
                {
                    lblステータス.Text = "既存データなし（バックアップスキップ）";
                    return;
                }

                // バックアップフォルダ検証
                if (string.IsNullOrWhiteSpace(CMD.mfBkPath))
                {
                    MessageBox.Show("バックアップフォルダが設定されていません。処理を続行します。", "警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 一時CSVファイル作成
                string tempCsvPath = Path.Combine(Path.GetTempPath(), $"PostalCodes_backup_{timestamp}.csv");

                using (StreamWriter sw = new StreamWriter(tempCsvPath, false, Encoding.GetEncoding("Shift_JIS")))
                {
                    // ヘッダー
                    sw.WriteLine("郵便番号,都道府県名ｶﾅ,市区町村名ｶﾅ,町域名ｶﾅ,都道府県名,市区町村名,町域名");

                    // データ
                    foreach (DataRow row in dt.Rows)
                    {
                        string line = string.Join(",",
                            $"\"{row["郵便番号"]}\"",
                            $"\"{row["都道府県名ｶﾅ"]}\"",
                            $"\"{row["市区町村名ｶﾅ"]}\"",
                            $"\"{row["町域名ｶﾅ"]}\"",
                            $"\"{row["都道府県名"]}\"",
                            $"\"{row["市区町村名"]}\"",
                            $"\"{row["町域名"]}\""
                        );
                        sw.WriteLine(line);
                    }
                }

                // ZIP圧縮してバックアップフォルダに保存
                string zipFileName = $"PostalCodes_backup_{timestamp}.zip";
                string zipPath = Path.Combine(CMD.mfBkPath, zipFileName);

                // バックアップフォルダ作成（存在しなければ）
                if (!Directory.Exists(CMD.mfBkPath))
                    Directory.CreateDirectory(CMD.mfBkPath);

                // 一時ディレクトリを作成しCSVをコピーしてからディレクトリごとZIP化
                string tempDir = Path.Combine(Path.GetTempPath(), $"PostalCodes_backup_{timestamp}");
                if (Directory.Exists(tempDir))
                {
                    try { Directory.Delete(tempDir, true); } catch { }
                }
                Directory.CreateDirectory(tempDir);
                string tempCsvName = Path.GetFileName(tempCsvPath);
                string tempCsvCopyPath = Path.Combine(tempDir, tempCsvName);
                File.Copy(tempCsvPath, tempCsvCopyPath, true);

                if (File.Exists(zipPath))
                {
                    try { File.Delete(zipPath); } catch { }
                }
                ZipFile.CreateFromDirectory(tempDir, zipPath, CompressionLevel.Optimal, includeBaseDirectory: false, entryNameEncoding: Encoding.GetEncoding("Shift_JIS"));

                // 一時ファイル・ディレクトリ削除
                File.Delete(tempCsvPath);
                try { Directory.Delete(tempDir, true); } catch { }

                lblステータス.Text = $"バックアップ完了: {zipFileName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"バックアップ作成に失敗しました。\n{ex.Message}\n\n処理を続行します。", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 前回CSVのバックアップ＋アップロードCSV保存
        /// </summary>
        /// <param name="originalPath"></param>
        private void SaveUploadCsv(string originalPath)
        {
            try
            {
                // 前回CSVバックアップ
                string backupDir = CMD.mfBkPath;
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);
                string extension = Path.GetExtension(originalPath);
                string timestamp = DateTime.Now.ToString("yyyyMMdd.HHmmss");
                string backupFileName = $"KEN_ALL.{timestamp}.{extension}";
                string backupPath = Path.Combine(backupDir, backupFileName);
                File.Copy(originalPath, backupPath, true);
                // バックアップCSVは容量が大きいためZIP圧縮
                string zipBackupPath = Path.Combine(backupDir, $"KEN_ALL.{timestamp}.zip");

                // アップロードCSV保存
                string fileName = Path.GetFileNameWithoutExtension(originalPath);
                string newFileName = $"{fileName}.{extension}";
                string newPath = Path.Combine(CMD.mfPath, newFileName);
                

                // mfPathフォルダ作成（存在しなければ）
                if (!Directory.Exists(CMD.mfPath))
                    Directory.CreateDirectory(CMD.mfPath);

                // ファイルコピー
                File.Copy(originalPath, newPath, true);

                lblステータス.Text = $"CSV保存完了: {newFileName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSVの保存に失敗しました。\n{ex.Message}", "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
