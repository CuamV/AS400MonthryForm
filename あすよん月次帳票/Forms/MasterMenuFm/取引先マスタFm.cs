using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Ohno.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using CMD = あすよん月次帳票.CommonData;
using ENM = あすよん月次帳票.Enums;
using DataTable = System.Data.DataTable;
using Path = System.IO.Path;

namespace あすよん月次帳票
{
    //==========================================================
    // --------取引先マスタFormクラス--------
    //==========================================================
    public partial class 取引先マスタFm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        string HIZTIM;
        string mf;
        string mfName;
        string mf1 = Path.Combine(CMD.mfPath, "DLB01SHIIRE.txt"); //オーノ
        string mf2 = Path.Combine(CMD.mfPath, "DLB02SHIIRE.txt"); // サンミックダスコン
        string mf3 = Path.Combine(CMD.mfPath, "DLB03SHIIRE.txt"); //サンミックカーペット
        string mf_bumon = Path.Combine(CMD.mfPath, "SHIIRE-BUMON.txt");
        string mst = "取引先マスタ";
        string mst_bumon = "取引先部門マスタ";

        //========================================================
        // コンストラクタ
        //========================================================
        public 取引先マスタFm()
        {
            InitializeComponent();
            this.Load += 取引先マスタForm_Load;
        }

        private void 取引先マスタForm_Load(object sender, EventArgs e)
        {
            List<Control> inputControls = GetTextBoxAndComboBox(this);
        }
        //=========================================================
        // コントロール実行メソッド
        //=========================================================
            /// <summary>
            /// 会社セレクトチェンジ
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void cmboBx会社_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string mf;
            // 会社選択確認(stringへ変換)
            var selComp = cmbBx会社.SelectedItem?.ToString();

            var bumons = fam.SelectCompany_Bumon(selComp, mst);

            cmbBx部門.Items.Clear();
            foreach (var bumon in bumons)
                cmbBx部門.Items.Add(bumon);

            if (cmbBx部門.Items.Count > 0)
                cmbBx部門.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBx郵便番号_TextChanged(object sender, EventArgs e)
        {
            string pCD = txtBx郵便番号.Text.Trim(); // 部門CD
            if (string.IsNullOrEmpty(pCD)) return;

            string zip = pCD.Replace("-", "");
            if (zip.Length != 7) return;

            // 住所取得
            var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");
            string sql = $@"SELECT 都道府県名ｶﾅ+市区町村名ｶﾅ+町域名ｶﾅ as 住所カナ,
                                       都道府県名+市区町村名+町域名 as 住所
                                FROM PostalCodes
                                WHERE 郵便番号 = '{zip}'";
            var dt = dbManager.GetDataTable(sql, CommandType.Text);

            if (dt.Rows.Count > 0)
            {
                txtBx住所1.Text = dt.Rows[0]["住所"].ToString();
                txtBx住所1カナ.Text = dt.Rows[0]["住所カナ"].ToString();
            }
            else
            {
                txtBx住所1.Text = "住所が見つかりません";
                txtBx住所1カナ.Text = "";
            }
        }

        /// <summary>
        /// 次へボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn次へ_Click(object sender, EventArgs e)
        {
            // 会社選択チェック
            if (cmbBx会社.SelectedItem == null)
            {
                MessageBox.Show("会社を選択して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 部門選択チェック
            if (cmbBx部門.SelectedItem == null)
            {
                MessageBox.Show("部門を選択して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //----------------------------------------------------
            // ★入力内容チェック
            //----------------------------------------------------
            string company   = cmbBx会社.SelectedItem?.ToString() ?? "";
            string bumonCD   = cmbBx部門.SelectedItem?.ToString() ?? "";
            string toriCD    = txtBx取引先CD.Text.Trim();
            string toriNAME  = txtBx取引先正式名.Text.Trim();
            string toriNAME1 = txtBx取引先名.Text.Trim();
            string toriKANA1 = txtBx取引先名カナ.Text.Trim();
            string toriNAME2 = txtBx取引先略名.Text.Trim();
            string toriKANA2 = txtBx取引先略名カナ.Text.Trim();

            Dictionary<string, string> InputTxtList = new Dictionary<string, string>();
            InputTxtList.Add("取引先コード", toriCD);
            InputTxtList.Add("取引先正式名", toriNAME);
            InputTxtList.Add("取引先名", toriNAME1);
            InputTxtList.Add("取引先名カナ", toriKANA1);
            InputTxtList.Add("取引先略名", toriNAME2);
            InputTxtList.Add("取引先略名カナ", toriKANA2);

            // 空白チェック
            foreach (var input in InputTxtList)
            {
                if (string.IsNullOrWhiteSpace(input.Value))
                {
                    MessageBox.Show($"{input.Key}を入力して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // 取引先コード半角数字7桁チェック
            if (!Regex.IsMatch(toriCD, @"^[0-9]{7}$"))
            {
                MessageBox.Show("取引先コードは半角数字7桁で入力して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"{mst}登録を行います。\n", "【取引先登録】",
                MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.No) return;
            //----------------------------------------------------
            // ★取引先マスタ登録処理
            //----------------------------------------------------
            string pstCD    = txtBx郵便番号.Text.Trim();
            string telNUM1  = txtBx電話番号1.Text.Trim();
            string telNUM2  = txtBx電話番号2.Text.Trim();
            string faxNUM1  = txtBxFAX番号1.Text.Trim();
            string faxNUM2  = txtBxFAX番号2.Text.Trim();
            string adr1     = txtBx住所1.Text.Trim();
            string adr1KANA = txtBx住所1カナ.Text.Trim();
            string adr2     = txtBx住所2.Text.Trim();
            string adr2KANA = txtBx住所2カナ.Text.Trim();
            string bikou    = txtBx備考.Text.Trim();

            // 書き込む行[仕入先マスタ]（半角スペース区切り）
            //  1:仕入先CD   2:仕入先正式名称 3:仕入先名   4:仕入先名カナ 5:仕入先略名 6:仕入先略名カナ 7:郵便番号 8:電話番号1
            //  9:電話番号2 10:FAX番号1      11:FAX番号2  12:住所1       13:住所1カナ 14:住所2         15:住所2カナ 16:備考
            // 17:登録者ID  18:登録日        19:登録時刻
            //----------------------------------------------------
            string[] newLines = new string[]
            {
                $"{toriCD} {toriNAME} {toriNAME1} {toriKANA1} {toriNAME2} {toriKANA2}",
                $"{pstCD} {telNUM1} {telNUM2} {faxNUM1} {faxNUM2} {adr1} {adr1KANA} {adr2} {adr2KANA} {bikou} {CMD.UserID} {CMD.HIZ} {DateTime.Now.ToString("HHmmss")}"
            };

            // 書き込む行[仕入先部門マスタ]（半角スペース区切り）
            //  1:仕入先CD 2:部門CD
            string[] newLines_bumon = new string[]
            {
                $"{toriCD} {bumonCD}"
            };

            // mf判定
            switch (company)
            {
                case "オーノ":
                    mf = mf1;
                    mfName = "BLB01SHIIRE";
                    break;
                case "サンミックダスコン":
                    mf = mf2;
                    mfName = "BLB02SHIIRE";
                    break;
                case "サンミックカーペット":
                    mf = mf3;
                    mfName = "BLB03SHIIRE";
                    break;
            }

            //// newLines, newLines_bumonを取引先マスタFm-2.csのXXXメソッドへ渡す
            //var frm2 = new 取引先マスタ_2Fm();
            //frm2.SetData(newLines, newLines_bumon, mf, mfName);
            //frm2.Show();
        }
        private void btn削除_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 照会ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn照会_Click(object sender, EventArgs e)
        {
            // 会社選択チェック
            if (cmbBx会社.SelectedItem == null)
            {
                MessageBox.Show("会社を選択して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string company = cmbBx会社.SelectedItem?.ToString() ?? "";
            // ----------------------------------------------------
            // ★表示用フォーム起動
            // ----------------------------------------------------
            // mf判定
            switch (company)
            {
                case "オーノ":
                    mf = mf1;
                    break;
                case "サンミックダスコン":
                    mf = mf2;
                    break;
                case "サンミックカーペット":
                    mf = mf3;
                    break;
            }
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8);

            // 照会Formを開く
            var frm = new 取引先マスタ照会Fm();
            frm.SetData(lines);
            frm.Show();
        }


        private void btnインポート_Click(object sender, EventArgs e)
        {
            // ----------------------------------------------------
            // ★インポート用フォーム起動
            // ----------------------------------------------------
            // インポートFormを開く
            var frm = new 取引先マスタインポートFm();
            frm.Show();
        }

        private void btnダウンロード_Click(object sender, EventArgs e)
        {
            // ----------------------------------------------------
            // ★ダウンロード用マスターデータ取得
            // ----------------------------------------------------
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8);

            // DataTableの作成
            //  1:取引先CD 2:部門CD    3:取引先正式名称 4:取引先名  5:取引先名カナ 6:取引先略名 7:取引先略名カナ
            //  8:郵便番号 9:電話番号1 10:電話番号2    11:FAX番号1 12:FAX番号2  13:住所1    14:住所1カナ 15:住所2 16:住所2カナ 17:備考
            //----------------------------------------------------
            DataTable dt = new DataTable();
            foreach (var col in Enum.GetNames(typeof(ENM.TORIHIKI_MASTER_OUT)))
            {
                dt.Columns.Add(col);
            }

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ');
                if (parts.Length >= 17)
                {
                    dt.Rows.Add(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6], parts[7], parts[8],
                        parts[9], parts[10], parts[11], parts[12], parts[13], parts[14], parts[15], parts[16]);
                }
            }

            //----------------------------------------------------
            // ★CSV出力処理
            //----------------------------------------------------
            // CSV保存ダイアログ表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"{mst}_{DateTime.Now:yyyyMMdd.HHmmss}.csv";
                sfd.Filter = "CSVファイル (*.csv)|*.csv";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                // CSV 出力処理
                var sb = new StringBuilder();

                // ヘッダー
                var header = string.Join(",", dt.Columns.Cast<DataColumn>().Select(col => EscapeCsv(col.ColumnName)));
                sb.AppendLine(header);

                // データ行
                foreach (DataRow row in dt.Rows)
                {
                    var fields = dt.Columns.Cast<DataColumn>()
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
            if (System.Windows.Forms.Application.OpenForms["Form5"] is MasterMenuFm form5)
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

        /// <summary>
        /// テキストボックスとコンボボックスを再帰的に取得
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<Control> GetTextBoxAndComboBox(Control parent)
        {
            var list = new List<Control>();

            foreach (Control ctrl in parent.Controls)
            {
                // 自分自身が対象なら追加
                if (ctrl is System.Windows.Forms.TextBox || ctrl is ComboBox)
                {
                    list.Add(ctrl);
                }

                // 子コントロールがある場合は再帰
                if (ctrl.HasChildren)
                {
                    list.AddRange(GetTextBoxAndComboBox(ctrl));
                }
            }

            return list;
        }

    }

    public class ShiireItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kana { get; set; }
    }
}
