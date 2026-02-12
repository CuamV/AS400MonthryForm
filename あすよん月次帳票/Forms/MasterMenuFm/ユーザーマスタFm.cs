using Ohno.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = System.Action;
using Application = System.Windows.Forms.Application;
using CMD = あすよん月次帳票.CommonData;
using DataTable = System.Data.DataTable;
using Path = System.IO.Path;
using TextBox = System.Windows.Forms.TextBox;

namespace あすよん月次帳票
{
    //==========================================================
    // --------ユーザーマスタFormクラス--------
    //==========================================================
    public partial class ユーザーマスタFm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        string HIZTIM;
        string TIM;
        string mfName = "UserMaster";
        string mf = Path.Combine(CMD.mfPath, "UserMaster.txt");
        string mst = "ユーザーマスタ";

        List<Control> _inputControls;
        private WaitExcelExport animForm;
        private Thread animThread;

        //========================================================
        // コンストラクタ
        //========================================================
        public ユーザーマスタFm()
        {
            InitializeComponent();

            this.Load += ユーザーマスタFm_Load;
        }

        public void ユーザーマスタFm_Load(object sender, EventArgs e)
        {
            _inputControls = GetTextInputControl(this);
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// 社員番号入力チェンジ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBxUserCD_TextChanged(object sender, EventArgs e)
        {
            // 社員番号,ユーザーID,ドメインユーザー全てが空白の場合未処理
         if (string.IsNullOrEmpty(txtBx社員番号.Text.Trim()) 
                && string.IsNullOrEmpty(txtBxユーザーID.Text.Trim())
                && string.IsNullOrEmpty(txtBxドメインユーザー.Text.Trim())) return;

            // テキストチェンジされたテキストボックスの種類を取得
            int targetIndex = 0;
            if ((TextBox)sender == txtBx社員番号)
            {
                // 5桁未満の場合未処理
                if (((TextBox)sender).Text.Trim().Length != 5) return;

                targetIndex = 0;
            }
            else if ((TextBox)sender == txtBxユーザーID)
            {
                // 5桁未満の場合未処理
                if (((TextBox)sender).Text.Trim().Length != 5) return;

                targetIndex = 1;
            }
            else
                targetIndex = 2;

            // テキストチェンジされたテキストボックスの値を取得
            string inputTxt = ((TextBox)sender).Text.Trim();

            // 既存マスター内容取得
            var dt = GetDataSQL(targetIndex, inputTxt);

            // 既存データチェック
            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                // ヘルパー（列名があれば列名、なければインデックスで取得）
                string GetVal(string colName, int idx)
                {
                    if (dt.Columns.Contains(colName))
                    {
                        var v = row[colName];
                        return v == DBNull.Value ? "" : v.ToString();
                    }
                    if (dt.Columns.Count > idx)
                    {
                        var v = row[idx];
                        return v == DBNull.Value ? "" : v.ToString();
                    }
                    return "";
                }

                // 社員番号入力時以外は社員番号も設定
                if (targetIndex != 0)
                    txtBx社員番号.Text = GetVal("社員番号", 0);
                
                // ユーザーID入力時以外はユーザーIDを設定
                if (targetIndex != 1)
                    txtBxユーザーID.Text = GetVal("ユーザーID", 1);
                
                // ドメインユーザー入力時以外はドメインユーザーを設定
                if (targetIndex != 2)
                    txtBxドメインユーザー.Text = GetVal("ドメインユーザー", 2);
                
                // その他の項目は常に設定
                txtBx氏名.Text = GetVal("氏名", 3);
                txtBx氏名ｶﾅ.Text = GetVal("氏名ｶﾅ", 4);
                txtBx部門コード.Text = GetVal("部門コード", 5);
                txtBxメールアドレス1.Text = GetVal("メールアドレス1", 6);
                txtBxメールアドレス2.Text = GetVal("メールアドレス2", 7);
                txtBx社用電話番号.Text = GetVal("社用電話番号", 8);
                txtBx内線番号.Text = GetVal("内線番号", 9);
                txtBx機器管理番号.Text = GetVal("機器管理番号", 10);
                txtBxバックアップパス1.Text = GetVal("バックアップパス1", 11);
                txtBxバックアップパス2.Text = GetVal("バックアップパス2", 12);
            }
        }

        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn登録_Click(object sender, EventArgs e)
        {
            //----------------------------------------------------
            // ★入力内容取得NO1
            //----------------------------------------------------
            Dictionary<string, string> InTxtDic = new Dictionary<string, string>
            {
                { "社員番号",txtBx社員番号.Text.Trim() },
                { "ユーザーID",txtBxユーザーID.Text.Trim() },
                { "氏名",txtBx氏名.Text.Trim() },
                { "氏名ｶﾅ", txtBx氏名ｶﾅ.Text.Trim() },
            };
            // ----------------------------------------------------
            // ★入力内容チェックNO1
            // ----------------------------------------------------
            // 空白/ユーザーコード半角数字5桁
            if (!fam.ValidateInput(VaridationPattern.ユーザーマスタ登録前チェック1, MstMntPattern.登録, InTxtDic, 5, mst)) return;

            //----------------------------------------------------
            // ★入力内容取得NO2
            //----------------------------------------------------
            Dictionary<string, string> InTxtDic2 = new Dictionary<string, string>
            {
                { "部門コード", txtBx部門コード.Text.Trim() },
            };
            // ----------------------------------------------------
            // ★入力内容チェックNO2
            // ----------------------------------------------------
            // 空白/部門コード半角数字3桁
            if (!fam.ValidateInput(VaridationPattern.ユーザーマスタ登録前チェック1, MstMntPattern.登録, InTxtDic2, 3, mst)) return;

            // 登録削除実行確認
            if (!fam.CheckedAddYesNo(MstMntPattern.登録, mst)) return;

            //----------------------------------------------------
            // ★入力内容取得NO3
            //----------------------------------------------------
            var InTxtDic3 = new Dictionary<string, string>
            {
                { "社員番号",txtBx社員番号.Text.Trim() },
                { "ユーザーID",txtBxユーザーID.Text.Trim() },
                { "ドメインユーザー",txtBxドメインユーザー.Text.Trim() },
                { "氏名",txtBx氏名.Text.Trim() },
                { "氏名ｶﾅ", txtBx氏名ｶﾅ.Text.Trim() },
                { "部門コード", txtBx部門コード.Text.Trim() },
                { "メールアドレス1", txtBxメールアドレス1.Text.Trim() },
                { "メールアドレス2", txtBxメールアドレス2.Text.Trim() },
                { "社用電話番号", txtBx社用電話番号.Text.Trim() },
                { "内線番号", txtBx内線番号.Text.Trim() },
                { "機器管理番号", txtBx機器管理番号.Text.Trim() },
                { "バックアップパス1", txtBxバックアップパス1.Text.Trim() },
                { "バックアップパス2", txtBxバックアップパス2.Text.Trim() },
            };

            try
            {
                // タイムスタンプ
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // --------------------------------------------
                // 1. 既存データをバックアップ
                // --------------------------------------------
                BackupExistingData(timestamp);

                // --------------------------------------------
                // 2. データベース接続
                // --------------------------------------------
                var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");

                // --------------------------------------------
                // 3. 既存データの確認
                // --------------------------------------------
                string chkSql = "SELECT COUNT(*) FROM UserMaster WHERE 社員番号 = @社員番号";
                var dtCheck = dbManager.GetDataTable(chkSql, CommandType.Text, 
                    new SqlParameter("@社員番号", InTxtDic3["社員番号"]));
                
                bool isUpdate = dtCheck != null && dtCheck.Rows.Count > 0 && 
                                Convert.ToInt32(dtCheck.Rows[0][0]) > 0;

                // --------------------------------------------
                // 4. INSERT または UPDATE 実行
                // --------------------------------------------
                string sql;
                if (isUpdate)
                {
                    // UPDATE
                    sql = @"UPDATE UserMaster SET
                                ユーザーID = @ユーザーID,
                                ドメインユーザー = @ドメインユーザー,
                                氏名 = @氏名,
                                氏名ｶﾅ = @氏名ｶﾅ,
                                部門コード = @部門コード,
                                メールアドレス1 = @メールアドレス1,
                                メールアドレス2 = @メールアドレス2,
                                社用電話番号 = @社用電話番号,
                                内線番号 = @内線番号,
                                機器管理番号 = @機器管理番号,
                                バックアップパス1 = @バックアップパス1,
                                バックアップパス2 = @バックアップパス2,
                                更新日付 = @日付,
                                更新時刻 = @時刻
                            WHERE 社員番号 = @社員番号";
                }
                else
                {
                    // INSERT
                    sql = @"INSERT INTO UserMaster
                            (社員番号, ユーザーID, ドメインユーザー, 氏名, 氏名ｶﾅ, 部門コード,
                             メールアドレス1, メールアドレス2, 社用電話番号, 内線番号,
                             機器管理番号, バックアップパス1, バックアップパス2, 登録日付, 登録時刻)
                            VALUES
                            (@社員番号, @ユーザーID, @ドメインユーザー, @氏名, @氏名ｶﾅ, @部門コード,
                             @メールアドレス1, @メールアドレス2, @社用電話番号, @内線番号,
                             @機器管理番号, @バックアップパス1, @バックアップパス2, @日付, @時刻)";
                }

                using (SqlConnection conn = (SqlConnection)dbManager.Connection)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // パラメータ設定
                        foreach (var kvp in InTxtDic3)
                        {
                            cmd.Parameters.AddWithValue($"@{kvp.Key}",
                                string.IsNullOrEmpty(kvp.Value) ? (object)DBNull.Value : kvp.Value);
                        }

                        TIM = DateTime.Now.ToString("HHmmss");
                        cmd.Parameters.AddWithValue("@日付", CMD.HIZ);
                        cmd.Parameters.AddWithValue("@時刻", TIM);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                            fam.AddLog($"{HIZTIM} ユーザーマスタ登録 1 {CMD.UserName} {(isUpdate ? "更新" : "新規")}");
                            fam.AddLog2($"{HIZTIM} ユーザーマスタ登録 0 {CMD.UserName} {mst}が{(isUpdate ? "更新" : "登録")}されました");

                            // 入力内容クリア
                            fam.ClearInput(_inputControls);

                            MessageBox.Show(
                                isUpdate ? "変更登録が完了しました。" : "新規登録が完了しました。",
                                $"{mst}登録",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登録に失敗しました。\n{ex.Message}", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        /// <summary>
        /// 照会ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn照会_Click(object sender, EventArgs e)
        {
            ////マスターファイル有無チェック＆読込
            //var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

            //// 照会Formを開く（会社名を渡す）
            //var frm = new ユーザーマスタFm();
            //frm.SetData(lines);
            //frm.Show();
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
        //=========================================================
        // 処理メソッド
        //=========================================================
        /// <summary>
        /// テキストボックスを再帰的に取得
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<Control> GetTextInputControl(Control parent)
        {
            var list = new List<Control>();

            foreach (Control ctrl in parent.Controls)
            {
                // 自分自身が対象なら追加
                if (ctrl is TextBox)
                {
                    list.Add(ctrl);
                }

                // 子コントロールがある場合は再帰
                if (ctrl.HasChildren)
                {
                    list.AddRange(GetTextInputControl(ctrl));
                }
            }
            return list;
        }

        private DataTable GetDataSQL(int targetIndex, string inputTxt)
        {
            string selKey = null;
            if (targetIndex == 0)
                selKey = "社員番号";
            else if (targetIndex == 1)
                selKey = "ユーザーID";
            else
                selKey = "ドメインユーザー";

            var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");
            string sql = $@"SELECT *
                                FROM UserMaster
                                WHERE {selKey} = '{inputTxt}'";
            var dt = dbManager.GetDataTable(sql, CommandType.Text);
            return dt;
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
                string sql = "SELECT * FROM UserMaster";
                DataTable dt = dbManager.GetDataTable(sql, CommandType.Text);

                if (dt.Rows.Count == 0)
                {
                    return;
                }

                // バックアップフォルダ検証
                if (string.IsNullOrWhiteSpace(CMD.mfBkPath))
                {
                    return;
                }

                // バックアップフォルダ作成（存在しなければ）
                if (!Directory.Exists(CMD.mfBkPath))
                    Directory.CreateDirectory(CMD.mfBkPath);

                // 一時CSVファイル作成
                string tempCsvPath = Path.Combine(Path.GetTempPath(), $"UserMaster_backup_{timestamp}.csv");

                using (StreamWriter sw = new StreamWriter(tempCsvPath, false, CMD.utf8))
                {
                    // ヘッダー
                    var headers = new List<string>();
                    foreach (DataColumn col in dt.Columns)
                        headers.Add(col.ColumnName);
                    sw.WriteLine(string.Join(",", headers.Select(h => $"\"{h}\"")));

                    // データ
                    foreach (DataRow row in dt.Rows)
                    {
                        var values = new List<string>();
                        foreach (var item in row.ItemArray)
                        {
                            var val = item == DBNull.Value ? "" : item.ToString();
                            values.Add($"\"{val}\"");
                        }
                        sw.WriteLine(string.Join(",", values));
                    }
                }

                // ZIP圧縮してバックアップフォルダに保存
                string zipFileName = $"UserMaster_backup_{timestamp}.zip";
                string zipPath = Path.Combine(CMD.mfBkPath, zipFileName);

                // 一時ディレクトリを作成しCSVをコピー
                string tempDir = Path.Combine(Path.GetTempPath(), $"UserMaster_backup_{timestamp}");
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
                
                ZipFile.CreateFromDirectory(tempDir, zipPath, CompressionLevel.Optimal, 
                    includeBaseDirectory: false, entryNameEncoding: CMD.utf8);

                // 一時ファイル・ディレクトリ削除
                File.Delete(tempCsvPath);
                try { Directory.Delete(tempDir, true); } catch { }
            }
            catch
            {
                // バックアップ失敗時も処理を継続
            }
        }
    }
}
