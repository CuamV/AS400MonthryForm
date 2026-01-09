using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    //==========================================================
    // --------部門マスタFormクラス--------
    //==========================================================
    internal partial class 部門マスタForm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        string HIZTIM;
        List<Control> _inputControls;
        string mf = Path.Combine(CMD.mfPath, "BUMON.txt");
        string mfName = "BUMON";
        string mst = "部門マスタ";
        //=========================================================
        // コンストラクタ
        //=========================================================
        internal 部門マスタForm()
        {
            InitializeComponent();
            this.Load += 部門マスタForm_Load;
        }

        internal void 部門マスタForm_Load(object sender, EventArgs e)
        {
            _inputControls = GetTextInputControl(this);
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// 部門CD入力チェンジ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBoxControl(object sender, EventArgs e)
        {
            string inputBumon = txtBx部門CD.Text.Trim(); // 部門CD
            if (string.IsNullOrEmpty(inputBumon)) return;

            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

            // 既存データチェック
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(' ');
                // 1:部門コード 2:部門名 3:部門名カナ 4:会社
                if (parts.Length > 0 && parts[0] == inputBumon)
                {
                    // マスタに存在する場合、各項目にセット
                    txtBx部門名.Text = parts.Length > 1 ? parts[1] : "";
                    txtBx部門名カナ.Text = parts.Length > 2 ? parts[2] : "";
                    cmbBx会社.SelectedItem = parts.Length > 3 ? parts[3] : null;
                }
            }
        }

        /// <summary>
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn登録_Click(object sender, EventArgs e)
        {
            // 会社選択チェック
            if(cmbBx会社.SelectedItem == null)
            {
                MessageBox.Show("会社を選択して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //----------------------------------------------------
            // ★入力内容チェック
            //----------------------------------------------------
            string company = cmbBx会社.SelectedItem?.ToString() ?? ""; // 会社名
            string bumonCD = txtBx部門CD.Text.Trim();   //[部門コード] 数字3桁コード
            string bumonNAME = txtBx部門名.Text.Trim();  // [部門名]
            string bumonKANA = txtBx部門名カナ.Text.Trim();  // [部門名カナ]

            Dictionary<string, string> InputTxtList = new Dictionary<string, string>();
            InputTxtList.Add("部門コード", bumonCD);
            InputTxtList.Add("部門名", bumonNAME);
            InputTxtList.Add("部門名カナ", bumonKANA);
            
            // 空白チェック
            foreach (var input in InputTxtList)
            {
                if (string.IsNullOrWhiteSpace(input.Value))
                {
                    MessageBox.Show($"{input.Key}を入力して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // 部門コード半角数字3桁チェック
            if (!Regex.IsMatch(bumonCD, @"^[0-9]{3}$"))
            {
                MessageBox.Show("部門コードは半角数字3桁で入力して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("部門マスタ登録を行います。\n", "【部門登録】",
                MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No) return;
            //----------------------------------------------------
            // ★部門マスタ登録処理
            //----------------------------------------------------

            // 書き込む行（半角スペース区切り）
            // 1:部門コード 2:部門名 3:部門名カナ 4:会社
            List<string> newLineList = new List<string>
            {
                bumonCD, bumonNAME, bumonKANA, company
            };

            // マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 0);

            // 新規・変更登録
            bool replaced;
            (lines, replaced) = fam.AddMasterFile(lines, newLineList);

            // 部門コードでソート
            lines = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy( x => x.Split(' ')[0])
                .ToList();

            //------------------------------------------------
            // ★バックアップ＆ファイル書き込み
            //------------------------------------------------
            // バックアップ
            fam.BackupMaster(mf, mfName, "Add" , mst);

            // ファイル書き込み
            File.WriteAllLines(mf,lines, Encoding.UTF8);

            // 入力内容クリア
            fam.ClearInput(_inputControls);

            MessageBox.Show(replaced ? "変更登録が完了しました。" : "新規登録が完了しました。",
                $"{mst}登録", MessageBoxButtons.OK, MessageBoxIcon.None);
            
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} マスタ登録 1 {CMD.UserName} btn登録_Click {mst}");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst}が更新されました");
        }

        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn削除_Click(object sender, EventArgs e)
        {
            //----------------------------------------------------
            // ★入力内容チェック
            //----------------------------------------------------
            string company = cmbBx会社.SelectedItem?.ToString() ?? ""; // 会社名
            string bumonCD = txtBx部門CD.Text.Trim();   //[部門コード] 数字3桁コード
            string bumonNAME = txtBx部門名.Text.Trim();  // [部門名]
            string bumonKANA = txtBx部門名カナ.Text.Trim();  // [部門名カナ]

            // 空白チェック
            if (string.IsNullOrWhiteSpace(bumonCD))
            {
                MessageBox.Show("部門コードを入力して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 部門コード半角数字3桁チェック
            if (!Regex.IsMatch(bumonCD, @"^[0-9]{3}$"))
            {
                MessageBox.Show("部門コードは半角数字3桁で入力して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("部門マスタ削除を行います。\n", "【部門登録】",
                MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No) return;

            //----------------------------------------------------
            // ★部門マスタ削除処理
            //----------------------------------------------------
            // ファイル存在チェック→なければエラー、あれば既存データ読み込み
            List<string> lines = new List<string>();
            if (!File.Exists(mf))
            {
                MessageBox.Show("部門マスタファイルが存在しません。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} マスタ削除 1 {CMD.UserName} btn削除_Click {mst}");
                fam.AddLog2($"{HIZTIM} マスタ削除 0 {CMD.UserName} btn削除_Click {mst}ファイルなし");
                return;
            }
            else lines = File.ReadAllLines(mf, CMD.utf8)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            // 該当レコード検索
            // 1:部門コード 2:部門名 3:部門名カナ 4:会社
            string target = lines.FirstOrDefault(x =>
            {
                var parts = x.Split(' ');
                return parts.Length >= 4 && parts[0] == bumonCD && parts[3] == company;
            });

            if(target == null)
            {
                MessageBox.Show("該当する部門が部門マスタに存在しません。",
                    "削除不可", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // レコード削除
            lines.Remove(target);

            // 部門コードでソート
            lines = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x.Split(' ')[0])
                .ToList();

            //------------------------------------------------
            // ★バックアップ＆ファイル書き込み
            //------------------------------------------------
            // バックアップ
            fam.BackupMaster(mf, mfName, "Add", mst);

            // ファイル書き込み
            File.WriteAllLines(mf, lines, CMD.utf8);

            // 入力内容クリア
            cmbBx会社.SelectedItem = null;
            txtBx部門CD.Clear();
            txtBx部門名.Clear();
            txtBx部門名カナ.Clear();
            txtBx部門CD.Focus();

            MessageBox.Show("削除登録が完了しました。",
                $"{mst}削除", MessageBoxButtons.OK, MessageBoxIcon.None);


            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} マスタ削除 1 {CMD.UserName} btn削除_Click {mst}");
            fam.AddLog2($"{HIZTIM} マスタ削除 0 {CMD.UserName} btn削除_Click {mst}が更新されました");
        }

        /// <summary>
        /// 照会ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn照会_Click(object sender, EventArgs e)
        {
            // ----------------------------------------------------
            // ★表示用フォーム起動
            // ----------------------------------------------------
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

            // 照会Formを開く
            var frm = new 部門マスタ照会Form();
            frm.SetData(lines);
            frm.Show();
        }

        /// <summary>
        /// ダウンロードボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnダウンロード_Click(object sender, EventArgs e)
        {
            // ----------------------------------------------------
            // ★ダウンロード用マスターデータ取得
            // ----------------------------------------------------
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

            // DataTableの作成
            DataTable dt = new DataTable();
            dt.Columns.Add("部門コード");
            dt.Columns.Add("部門名");
            dt.Columns.Add("部門名カナ");
            dt.Columns.Add("会社コード");

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ');
                if (parts.Length >= 4)
                {
                    dt.Rows.Add(parts[0], parts[1], parts[2], parts[3]);
                }
            }

            //----------------------------------------------------
            // ★CSV出力処理
            //----------------------------------------------------
            // CSV保存ダイアログ表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"部門マスタ_{DateTime.Now:yyyyMMdd.HHmmss}.csv";
                sfd.Filter = "CSVファイル (*.csv)|*.csv";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                // CSV 出力処理
                var sb = new StringBuilder();

                // ヘッダー
                var header = string.Join(",", dt.Columns.Cast<DataColumn>().Select(col => EscapeCsv(col.ColumnName)));
                sb.AppendLine(header);

                // データ行
                foreach(DataRow row in dt.Rows)
                {
                    var fields =dt.Columns.Cast<DataColumn>()
                        .Select(col => EscapeCsv(row[col].ToString()));

                    sb.AppendLine(string.Join(",", fields));
                }
                // UTF-8 BOM付きで保存
                File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
            }
        }

        /// <summary>
        /// 戻るボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn戻る_Click(object sender, EventArgs e)
        {
            // Form5 のインスタンスを取得して表示
            if (Application.OpenForms["Form5"] is MasterMenuFm form5)
            {
                form5.Show();
            }
            // 部門マスタForm を閉じる
            this.Close();
        }
        //=========================================================
        // 処理メソッド
        //=========================================================
        /// <summary>
        /// CSV
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private string EscapeCsv(string field)
        {
            if (field.Contains("\""))
                field = field.Replace("\"", "\"\"");

            if (field.Contains(",") || field.Contains("\""))
                field = $"\"{field}\"";

            return field;
        }
        private List<Control> GetTextInputControl(Control parent)
        {
            var list = new List<Control>();

            foreach (Control ctrl in parent.Controls)
            {
                // 自分自身が対象なら追加
                if (ctrl is TextBox || ctrl is ComboBox)
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
    }
    
}
