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
using DCN = あすよん月次帳票.Dictionaries;
using DataTable = System.Data.DataTable;
using Path = System.IO.Path;
using Application = System.Windows.Forms.Application;
using TextBox = System.Windows.Forms.TextBox;

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
        Dictionaries2 dic2 = new Dictionaries2();

        // フィールド変数
        string HIZTIM;
        string mf;
        string mfName;
        string mf_bumonName = "TORIHIKI-BUMON";
        string mf1 = Path.Combine(CMD.mfPath, "DLB01TORIHIKI.txt"); //オーノ
        string mf2 = Path.Combine(CMD.mfPath, "DLB02TORIHIKI.txt"); // サンミックダスコン
        string mf3 = Path.Combine(CMD.mfPath, "DLB03TORIHIKI.txt"); //サンミックカーペット
        string mf_bumon = Path.Combine(CMD.mfPath, "TORIHIKI-BUMON.txt");
        string mst = "取引先マスタ";
        string mst_bumon = "取引先部門マスタ";
        string mst_toriroru = "取引先ロール別マスタ";
        List<Control> _inputControls;

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
            _inputControls = GetTextInputControl(this);
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
        /// 郵便番号辞書検索
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
        /// 登録ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn登録_Click(object sender, EventArgs e)
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
            // ★入力内容取得NO1
            //----------------------------------------------------
            string company = cmbBx会社.SelectedItem?.ToString() ?? "";
            string bumonCD = cmbBx部門.SelectedItem?.ToString() ?? "";
            // 0:商社 1:仕入先 2:販売先 4:得意先 5:出荷先 6:預り先 7:運送便 8:倉庫
            // チェックボックス状態取得
            // チェックあり→"1", チェックなし→"0"　としてtoriRoll配列に格納
            string[] toriRoll = new string[chkListBx取引先ロール.Items.Count];
            for(int i = 0; i < chkListBx取引先ロール.Items.Count; i++)
            {
                toriRoll[i] = chkListBx取引先ロール.GetItemChecked(i) ? "1" : "0";
            }

            Dictionary<string, string> ToriInTxtDic = new Dictionary<string, string> 
            {
                { "取引先コード",txtBx取引先CD.Text.Trim() },
                { "取引先正式名", txtBx取引先正式名.Text.Trim() },
                { "取引先名", txtBx取引先名.Text.Trim() }, 
                { "取引先名カナ", txtBx取引先名カナ.Text.Trim() },
                { "取引先略名", txtBx取引先略名.Text.Trim() },
                { "取引先略名カナ", txtBx取引先略名カナ.Text.Trim() },
             };
            // ----------------------------------------------------
            // ★入力内容チェックNO1
            // ----------------------------------------------------
            // 空白チェック
            if (!fam.CheckedNullOrWhiteSpace(ToriInTxtDic)) return;
            // 取引先コード半角数字7桁チェック
            if (!fam.CheckedErrLength("取引先コード", ToriInTxtDic["取引先コード"], 7)) return;
            // 登録実行確認
            if (!fam.CheckedAddYesNo(mst)) return;

            //----------------------------------------------------
            // ★入力内容取得NO2
            //----------------------------------------------------
            var ToriInTxtDic2 = new Dictionary<string, string>
            {
                { "郵便番号", txtBx郵便番号.Text.Trim() },
                { "電話番号1", txtBx電話番号1.Text.Trim() },
                { "電話番号2", txtBx電話番号2.Text.Trim() },
                { "FAX番号1", txtBxFAX番号1.Text.Trim() },
                { "FAX番号2", txtBxFAX番号2.Text.Trim() },
                { "住所1", txtBx住所1.Text.Trim() },
                { "住所1カナ", txtBx住所1カナ.Text.Trim() },
                { "住所2", txtBx住所2.Text.Trim() },
                { "住所2カナ", txtBx住所2カナ.Text.Trim() },
                { "商社区分", toriRoll[0] },
                { "仕入先区分", toriRoll[1] },
                { "販売先区分", toriRoll[2] },
                { "得意先区分", toriRoll[3] },
                { "出荷先区分", toriRoll[4] },
                { "預り先区分", toriRoll[5] },
                { "運送便区分", toriRoll[6] },
                { "倉庫区分", toriRoll[7] },
                { "備考", txtBx備考.Text.Trim() }
            };
            foreach(var key in ToriInTxtDic2.Keys)
            {
                ToriInTxtDic[key] = ToriInTxtDic2[key];
            }

            // ----------------------------------------------------
            // ★入力内容チェックNO2
            // ----------------------------------------------------
            // 郵便番号半角数字チェック(アルファベット不可)
            if (!fam.CheckedHalfNum("郵便番号", ToriInTxtDic2["郵便番号"], allowHyphen: true)) return;
            // 電話番号1半角数字チェック(アルファベット不可)
            if (!fam.CheckedHalfNum("電話番号1", ToriInTxtDic2["電話番号1"], allowHyphen: true)) return;
            // 電話番号2半角数字チェック(アルファベット不可)
            if (!fam.CheckedHalfNum("電話番号2", ToriInTxtDic2["電話番号2"], allowHyphen: true)) return;
            // FAX番号1半角数字チェック(アルファベット不可)
            if (!fam.CheckedHalfNum("FAX番号1", ToriInTxtDic2["FAX番号1"], allowHyphen: true)) return;
            // FAX番号2半角数字チェック(アルファベット不可)
            if (!fam.CheckedHalfNum("FAX番号2", ToriInTxtDic2["FAX番号2"], allowHyphen: true)) return;

            // ----------------------------------------------------
            // ★マスタ登録レコード形成
            // ----------------------------------------------------
            // [取引先マスタ]（半角スペース区切り）
            //  1:取引先CD   2:取引先正式名称 3:取引先名    4:取引先名カナ 5:取引先略名  6:取引先略名カナ 7:郵便番号   8:電話番号1
            //  9:電話番号2  10:FAX番号1    11:FAX番号2  12:住所1      13:住所1カナ  14:住所2       15:住所2カナ 16:商社区分
            // 17:仕入先区分 18:販売先区分   19:得意先区分 20:出荷先区分  21:預り先区分 22:運送便区分   23:倉庫区分  24:備考
            // 25:登録者ID  26:登録日       27:登録時刻
            //----------------------------------------------------
            List<string> newLineList = new List<string>
            {
                ToriInTxtDic["取引先コード"], ToriInTxtDic["取引先正式名"],
                ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"],
                ToriInTxtDic["取引先略名"],ToriInTxtDic["取引先略名カナ"],
                ToriInTxtDic2["郵便番号"], ToriInTxtDic2["電話番号1"], ToriInTxtDic2["電話番号2"],
                ToriInTxtDic2["FAX番号1"], ToriInTxtDic2["FAX番号2"],
                ToriInTxtDic2["住所1"], ToriInTxtDic2["住所1カナ"],
                ToriInTxtDic2["住所2"], ToriInTxtDic2["住所2カナ"],
                ToriInTxtDic2["商社区分"], ToriInTxtDic2["仕入先区分"],
                ToriInTxtDic2["販売先区分"], ToriInTxtDic2["得意先区分"],
                ToriInTxtDic2["出荷先区分"], ToriInTxtDic2["預り先区分"],
                ToriInTxtDic2["運送便区分"], ToriInTxtDic2["倉庫区分"],
                ToriInTxtDic2["備考"], CMD.UserID, CMD.HIZ, DateTime.Now.ToString("HHmmss")
            };

            // [取引先部門マスタ]（半角スペース区切り）
            //  1:取引先CD 2:部門CD
            List<string> newLine_bumon = new List<string>
            {
                ToriInTxtDic["取引先コード"], bumonCD
            };

            // 取引先ロール別登録処理
            List<string> newLine_syosya = new List<string>();
            List<string> newLine_siire = new List<string>();
            List<string> newLine_hanbai = new List<string>();
            List<string> newLine_tokui = new List<string>();
            List<string> newLine_syukka = new List<string>();
            List<string> newLine_azukari = new List<string>();
            List<string> newLine_unsou = new List<string>();
            List<string> newLine_souko = new List<string>();
            List<List<string>> newLineRollList = new List<List<string>>()
            {
                newLine_syosya, newLine_siire, newLine_hanbai,
                newLine_tokui, newLine_syukka, newLine_azukari,
                newLine_unsou, newLine_souko
            };
            // 1:取引先CD 2:部門CD 3:取引先名 4:取引先名カナ
            // ----------------------------------------------------
            // [商社マスタ]
            if (toriRoll[0] == "1") // 商社区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_syosya);
            // [仕入先マスタ]
            if (toriRoll[1] == "1") // 仕入先区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_syosya);
            // [販売先マスタ]
            if (toriRoll[2] == "1") // 販売先区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_hanbai);
            // [得意先マスタ]
            if (toriRoll[3] == "1") // 得意先区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_tokui);
            // [出荷先マスタ]
            if (toriRoll[4] == "1") // 出荷先区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_syukka);
            // [預り先マスタ]
            if (toriRoll[5] == "1") // 預り先区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_azukari);
            // [運送便マスタ]
            if (toriRoll[6] == "1") // 運送便区分が"1"の場合のみ登録
                MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLine_unsou);

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

            // ----------------------------------------------------
            // ★マスタ登録処理[取引先マスタ/取引先部門マスタ]
            // ----------------------------------------------------
            // 取引先マスタファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 0);
            // 取引先部門マスタファイル有無チェック＆読込
            var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst_bumon, CMD.utf8, 0);
            // 新規・変更登録
            bool replaced1;
            bool replaced2;
            (lines, replaced1) = fam.AddMasterFile(lines, newLineList);
            (lines_bumon, replaced2) = fam.AddMasterFile2(lines_bumon, newLine_bumon);
            // 取引先コードでソート
            lines = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x.Split(' ')[0])
                .ToList();
            lines_bumon = lines_bumon
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x.Split(' ')[0])
                .ToList();
            // バックアップ
            fam.BackupMaster(mf, mfName, "Add", mst);
            fam.BackupMaster(mf_bumon, mf_bumonName, "Add", mst_bumon);
            // ファイル書き込み
            File.WriteAllLines(mf, lines, Encoding.UTF8);
            File.WriteAllLines(mf_bumon, lines_bumon, Encoding.UTF8);

            // ----------------------------------------------------
            // ★マスタ登録処理[取引先ロール別マスタ]
            // ----------------------------------------------------
            // 取引先ロール別マスターファイル有無チェック＆読込
            foreach (var newLineRoll in newLineRollList)
            {
                if (newLineRoll.Count == 0) continue; // 登録なしの場合スキップ
                string mf_toriroll = null;
                if (ReferenceEquals(newLineRoll, newLine_syosya))
                    mf_toriroll = Path.Combine(CMD.mfPath, "SYOSYA-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_siire))
                    mf_toriroll = Path.Combine(CMD.mfPath, "SIIRE-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_hanbai))
                    mf_toriroll = Path.Combine(CMD.mfPath, "HANBAI-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_tokui))
                    mf_toriroll = Path.Combine(CMD.mfPath, "TOKUISAKI-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_syukka))
                    mf_toriroll = Path.Combine(CMD.mfPath, "SYUKKA-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_azukari))
                    mf_toriroll = Path.Combine(CMD.mfPath, "AZUKARI-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_unsou))
                    mf_toriroll = Path.Combine(CMD.mfPath, "UNSOU-MASTER.txt");
                else if (ReferenceEquals(newLineRoll, newLine_souko))
                    mf_toriroll = Path.Combine(CMD.mfPath, "SOUKO-MASTER.txt");

                // 取引先ロール別マスターファイル有無チェック＆読込
                var lines_toriroru = fam.CheckAndLoadMater(mf_toriroll, mst, CMD.utf8, 0);
                bool replaced_toriroru;
                // 新規・変更登録
                (lines_toriroru, replaced_toriroru) = fam.AddMasterFile(lines_toriroru, newLineRoll);
                // 取引先ロール別マスターファイル書き込み
                File.WriteAllLines(mf_toriroll, lines_toriroru, Encoding.UTF8);
                // 取引先コードでソート
                lines_toriroru = lines_toriroru
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .OrderBy(x => x.Split(' ')[0])
                    .ToList();
                // バックアップ
                fam.BackupMaster(mf_bumon, mf_bumonName, "Add", mst_bumon);
                // ファイル書き込み
                File.WriteAllLines(mf_toriroll, lines_toriroru, Encoding.UTF8);
            }

            // 入力内容クリア
            fam.ClearInput(_inputControls);

            MessageBox.Show(replaced1 ? "変更登録が完了しました。" : "新規登録が完了しました。",
                $"{mst}登録", MessageBoxButtons.OK, MessageBoxIcon.None);

            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} マスタ登録 1 {CMD.UserName} btn登録_Click {mst}");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst}が更新されました");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_bumon}が登録されました");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_toriroru}が登録されました");
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
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

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
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 0);

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
        private List<Control> GetTextInputControl(Control parent)
        {
            var list = new List<Control>();

            foreach (Control ctrl in parent.Controls)
            {
                // 自分自身が対象なら追加
                if (ctrl is TextBox || ctrl is ComboBox || ctrl is CheckedListBox)
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
        private List<string> MakingNewLineToriRoll(string col1, string col2, string col3, string col4, List<string> list)
        {
            list = new List<string>
            {
                col1, col2, col3, col4
            };
            
            return list;
        }

    }
}
