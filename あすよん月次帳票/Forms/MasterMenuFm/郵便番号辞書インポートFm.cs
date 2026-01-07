using Ohno.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using DataTable = System.Data.DataTable;
using ENM = あすよん月次帳票.Enums;
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
        string mf;

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
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// ファイル選択ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnファイル選択_Click(object sender, EventArgs e)
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

                // データベース接続
                var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");

                // 既存データ削除
                lbステータス.Text = "既存データを削除中...";
                Application.DoEvents();
                string deleteSql = "TRUNCATE TABLE PostalCodes";
                dbManager.Execute(deleteSql, CommandType.Text);

                // CSVデータ読み込み
                lbステータス.Text = "CSVデータを読み込み中...";
                Application.DoEvents();

                var lines = File.ReadLines(csvFilePath, Encoding.GetEncoding("Shift_JIS")).ToList();
                int totalCount = lines.Count;
                int processedCount = 0;

                // バルクインサート用DataTable作成
                DataTable bulkData = new DataTable();
                foreach (var col in Enum.GetNames(typeof(ENM.POSTALCODES)))
                {
                    bulkData.Columns.Add(col, typeof(string));
                }

                foreach (var line in lines)
                {
                    var fields = line.Split(',').Select(f => f.Trim('"')).ToArray();

                    if (fields.Length >= 9)
                    {
                        DataRow row = bulkData.NewRow();
                        row["郵便番号"] = fields[0];
                        row["都道府県名ｶﾅ"] = fields[1];
                        row["市区町村名ｶﾅ"] = fields[2];
                        row["町域名ｶﾅ"] = fields[3];
                        row["都道府県名"] = fields[4];
                        row["市区町村名"] = fields[5];
                        row["町域名"] = fields[6];
                        bulkData.Rows.Add(row);
                    }

                    processedCount++;
                    if (processedCount % 1000 == 0)
                    {
                        progressBar.Value = (int)((double)processedCount / totalCount * 100);
                        lblステータス.Text = $"インポート中... {processedCount}/{totalCount}";
                        Application.DoEvents();
                    }
                }

                // バルクインサート実行
                lblステータス.Text = "データベースへ書き込み中...";
                Application.DoEvents();

                // Use parameterized INSERT via dbManager to avoid direct connection handling
                string insertSql = @"INSERT INTO PostalCodes(郵便番号, 都道府県名ｶﾅ, 市区町村名ｶﾅ, 町域名ｶﾅ, 都道府県名, 市区町村名, 町域名)
                                    VALUES(@Zip, @PrefKana, @CityKana, @TownKana, @Pref, @City, @Town)";

                int inserted = 0;
                foreach (DataRow dr in bulkData.Rows)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@Zip", dr["郵便番号"] ?? (object)DBNull.Value),
                        new SqlParameter("@PrefKana", dr["都道府県名ｶﾅ"] ?? (object)DBNull.Value),
                        new SqlParameter("@CityKana", dr["市区町村名ｶﾅ"] ?? (object)DBNull.Value),
                        new SqlParameter("@TownKana", dr["町域名ｶﾅ"] ?? (object)DBNull.Value),
                        new SqlParameter("@Pref", dr["都道府県名"] ?? (object)DBNull.Value),
                        new SqlParameter("@City", dr["市区町村名"] ?? (object)DBNull.Value),
                        new SqlParameter("@Town", dr["町域名"] ?? (object)DBNull.Value)
                    };
                    dbManager.Execute(insertSql, CommandType.Text, parameters);

                    inserted++;
                    if (inserted % 1000 == 0)
                    {
                        progressBar.Value = Math.Min(100, (int)((double)inserted / bulkData.Rows.Count * 100));
                        lblステータス.Text = $"データベースへ書き込み中... {inserted}/{bulkData.Rows.Count}";
                        Application.DoEvents();
                    }
                }

                progressBar.Value = 100;
                lblステータス.Text = "インポート完了！";

                MessageBox.Show($"{bulkData.Rows.Count}件のデータをインポートしました。", "完了",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"インポートに失敗しました。\n{ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
