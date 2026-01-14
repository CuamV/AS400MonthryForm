using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Ohno.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        string BUMON_mst = "部門マスタ";
        string mst = "取引先マスタ";
        string mst_bumon = "取引先部門マスタ";
        string mst_torirole = "取引先ロール別マスタ";

        List<Control> _inputControls;
        private WaitExcelExport animForm;
        private Thread animThread;

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

            // cmbBx部門のデフォルト値設定
            cmbBx部門.Items.AddRange(GetBumonDefault());
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// 取引先CD入力チェンジ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBx取引先CD_TextChanged(object sender, EventArgs e)
        {
            string inputTorihiki = txtBx取引先CD.Text.Trim(); // 取引先CD
            string inputBumon = cmbBx部門.SelectedItem?.ToString() ?? ""; // 部門CD

            // 部門CDが空白の場合未処理
            if (string.IsNullOrEmpty(inputBumon)) return;
            // 取引先CDが空白の場合未処理
            if (string.IsNullOrEmpty(inputTorihiki)) return;
            // 取引先CDが７桁未満の場合未処理
            if (inputTorihiki.Length != 7) return;
            
            // mf判定
            (mf, mfName) = JudgeMF(JudgeMfPattern.パスとファイル名, GetCompanyFromBumonCD(inputBumon));
            
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);
            var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst_bumon, CMD.utf8, 1);

            // 既存データチェック
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(' ');
                //  1:取引先CD   2:取引先正式名称 3:取引先名    4:取引先名カナ 5:取引先略名  6:取引先略名カナ 7:郵便番号   8:電話番号1
                //  9:電話番号2  10:FAX番号1    11:FAX番号2  12:住所1      13:住所1カナ  14:住所2       15:住所2カナ 16:商社区分
                // 17:仕入先区分 18:販売先区分   19:得意先区分 20:出荷先区分  21:預り先区分 22:運送便区分   23:倉庫区分  24:備考
                // 25:登録者ID  26:登録日       27:登録時刻
                //----------------------------------------------------
                if (parts.Length > 0 && parts[0] == inputTorihiki)
                {
                    // マスタに存在する場合、各項目にセット
                    txtBx取引先正式名.Text = parts.Length > 1 ? parts[1] : "";  // 2:取引先正式名称
                    txtBx取引先名.Text = parts.Length > 2 ? parts[2] : "";      // 3:取引先名
                    txtBx取引先名カナ.Text = parts.Length > 3 ? parts[3] : "";   // 4:取引先名カナ
                    txtBx取引先略名.Text = parts.Length > 4 ? parts[4] : "";      // 5:取引先略名
                    txtBx取引先略名カナ.Text = parts.Length > 5 ? parts[5] : "";   // 6:取引先略名カナ
                    txtBx郵便番号.Text = parts.Length > 6 ? parts[6] : "";        // 7:郵便番号
                    txtBx電話番号1.Text = parts.Length > 7 ? parts[7] : "";       // 8:電話番号1
                    txtBx電話番号2.Text = parts.Length > 8 ? parts[8] : "";       // 9:電話番号2
                    txtBxFAX番号1.Text = parts.Length > 9 ? parts[9] : "";        // 10:FAX番号1
                    txtBxFAX番号2.Text = parts.Length > 10 ? parts[10] : "";      // 11:FAX番号2
                    txtBx住所1.Text = parts.Length > 11 ? parts[11] : "";         // 12:住所1
                    txtBx住所1カナ.Text = parts.Length > 12 ? parts[12] : "";      // 13:住所1カナ
                    txtBx住所2.Text = parts.Length > 13 ? parts[13] : "";         // 14:住所2
                    txtBx住所2カナ.Text = parts.Length > 14 ? parts[14] : "";      // 15:住所2カナ
                    // 取引先ロールチェックボックス設定
                    // 入力値が"1"の場合チェックあり、""の場合チェックなし
                    chkListBx取引先ロール.SetItemChecked(0, parts.Length > 15 && parts[15] == "1"); // 16:商社区分 
                    chkListBx取引先ロール.SetItemChecked(1, parts.Length > 16 && parts[16] == "1"); // 17:仕入先区分
                    chkListBx取引先ロール.SetItemChecked(2, parts.Length > 17 && parts[17] == "1"); // 18:販売先区分
                    chkListBx取引先ロール.SetItemChecked(3, parts.Length > 18 && parts[18] == "1"); // 19:得意先区分
                    chkListBx取引先ロール.SetItemChecked(4, parts.Length > 19 && parts[19] == "1"); // 20:出荷先区分
                    chkListBx取引先ロール.SetItemChecked(5, parts.Length > 20 && parts[20] == "1"); // 21:預り先区分
                    chkListBx取引先ロール.SetItemChecked(6, parts.Length > 21 && parts[21] == "1"); // 22:運送便区分
                    chkListBx取引先ロール.SetItemChecked(7, parts.Length > 22 && parts[22] == "1"); // 23:倉庫区分
                    txtBx備考.Text = parts.Length > 23 ? parts[23] : "";          // 24:備考
                }
            }
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
            // 部門選択チェック
            if (cmbBx部門.SelectedItem == null)
            {
                MessageBox.Show("部門を選択して下さい。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //----------------------------------------------------
            // ★入力内容取得NO1
            //----------------------------------------------------
            string bumonCD = cmbBx部門.SelectedItem?.ToString() ?? "";

            // 0:商社 1:仕入先 2:販売先 4:得意先 5:出荷先 6:預り先 7:運送便 8:倉庫
            // チェックボックス状態取得
            // チェックあり→"1", チェックなし→"0"　としてtoriRoll配列に格納
            string[] toriRolls = new string[chkListBx取引先ロール.Items.Count];
            for (int i = 0; i < chkListBx取引先ロール.Items.Count; i++)
                toriRolls[i] = chkListBx取引先ロール.GetItemChecked(i) ? "1" : "0";

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
            // 空白/取引先コード半角数字7桁/登録実行確認
            if (!fam.ValidateInput(VaridationPattern.登録前初期チェック, ToriInTxtDic, 7, mst)) return;

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
                { "商社区分", toriRolls[0] },
                { "仕入先区分", toriRolls[1] },
                { "販売先区分", toriRolls[2] },
                { "得意先区分", toriRolls[3] },
                { "出荷先区分", toriRolls[4] },
                { "預り先区分", toriRolls[5] },
                { "運送便区分", toriRolls[6] },
                { "倉庫区分", toriRolls[7] },
                { "備考", txtBx備考.Text.Trim() }
            };
            foreach (var key in ToriInTxtDic2.Keys)
                ToriInTxtDic[key] = ToriInTxtDic2[key];

            // ----------------------------------------------------
            // ★入力内容チェックNO2
            // ----------------------------------------------------
            // 郵便/電話/FAX番号半角数字チェック(アルファベット不可)
            if (!fam.ValidateInput(VaridationPattern.入力値チェック, ToriInTxtDic2)) return;

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
            for (int i = 0; i < newLineRollList.Count; i++)
            {
                if (toriRolls[i] == "1")
                    fam.MakingNewLineToriRoll(ToriInTxtDic["取引先コード"], bumonCD, ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"], newLineRollList[i]);
            }

            // mf判定
            (mf, mfName) = JudgeMF(JudgeMfPattern.パスとファイル名, GetCompanyFromBumonCD(bumonCD));

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
            (lines, replaced1) = fam.AddMasterFile(AddMasterPattern.Keyが1項目, lines, newLineList);
            (lines_bumon, replaced2) = fam.AddMasterFile(AddMasterPattern.Keyが1項目と2項目, lines_bumon, newLine_bumon);
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
            for (int i = 0; i <= newLineRollList.Count; i++)
            {
                if (newLineRollList[i].Count == 0) continue; // 登録なしの場合スキップ
                {
                    string mf_toriroll = null;
                    mf_toriroll = mf_torirollTxtPaths[i];

                    // 取引先ロール別マスターファイル有無チェック＆読込
                    var lines_toriroru = fam.CheckAndLoadMater(mf_toriroll, mst, CMD.utf8, 0);
                    bool replaced_toriroru;
                    // 新規・変更登録
                    (lines_toriroru, replaced_toriroru) = fam.AddMasterFile(AddMasterPattern.Keyが1項目と2項目, lines_toriroru, newLineRollList[i]);
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
            }

            // 入力内容クリア
            fam.ClearInput(_inputControls);

            MessageBox.Show(replaced1 ? "変更登録が完了しました。" : "新規登録が完了しました。",
                $"{mst}登録", MessageBoxButtons.OK, MessageBoxIcon.None);

            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} マスタ登録 1 {CMD.UserName} btn登録_Click {mst}");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst}が更新されました");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_bumon}が登録されました");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_torirole}が登録されました");
        }

        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            // メッセージボックスでテキスト入力を表示
            string input = Interaction.InputBox(
                "オーノ / ダスコン / カーペット", // プロンプト
                "データ領域(会社)を選択", // タイトル
                "", // デフォルト値（省略可）
                -1, // X座標（-1で中央）
                -1  // Y座標（-1で中央）
            );

            // 入力がキャンセルまたは空白の場合、処理を中止
            if (string.IsNullOrWhiteSpace(input)) return;
            string company = input;
            // ----------------------------------------------------
            // ★表示用フォーム起動
            // ----------------------------------------------------
            // mf判定
            switch (input)
            {
                case "ダスコン":
                    company = "サンミックダスコン";
                    break;
                case "カーペット":
                    company = "サンミックカーペット";
                    break;
                default:
                    company = "オーノ";
                    break;
            }
            (mf, mfName) = JudgeMF(JudgeMfPattern.パスのみ,company);

            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

            // 照会Formを開く
            var frm = new 取引先マスタ照会Fm();
            frm.SetData(lines);
            frm.Show();
        }

        /// <summary>
        /// インポートボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnインポート_Click(object sender, EventArgs e)
        {
            // ----------------------------------------------------
            // ★インポート用フォーム起動
            // ----------------------------------------------------
            // インポートFormを開く
            var frm = new 取引先マスタインポートFm();
            frm.Show();
        }

        /// <summary>
        /// ダウンロードボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnダウンロード_Click(object sender, EventArgs e)
        {
            // メッセージボックスでテキスト入力を表示
            string input = Interaction.InputBox(
                "オーノ / ダスコン / カーペット", // プロンプト
                "データ領域(会社)を選択", // タイトル
                "", // デフォルト値（省略可）
                -1, // X座標（-1で中央）
                -1  // Y座標（-1で中央）
            );

            // 入力がキャンセルまたは空白の場合、処理を中止
            if (string.IsNullOrWhiteSpace(input)) return;
            string company = input;
            // ----------------------------------------------------
            // ★表示用フォーム起動
            // ----------------------------------------------------
            // mf判定
            switch (input)
            {
                case "ダスコン":
                    company = "サンミックダスコン";
                    break;
                case "カーペット":
                    company = "サンミックカーペット";
                    break;
                default:
                    company = "オーノ";
                    break;
            }
            (mf, mfName) = JudgeMF(JudgeMfPattern.パスのみ, company);

            // ----------------------------------------------------
            // ★ダウンロード用マスターデータ取得
            // ----------------------------------------------------
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);
            var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst_bumon, CMD.utf8, 1);

            // [出力レイアウト]
            //  1:取引先CD    2:部門CD    3:取引先正式名  4:取引先名  5:取引先名カナ 6:取引先略名  7:取引先略名カナ 8:郵便番号   9:電話番号1  10:電話番号2
            // 11:FAX番号1  12:FAX番号2  13:住所1       14:住所1カナ 15:住所2     16:住所2カナ  17:商社区分     18:仕入先区分 19:販売先区分 20:得意先区分
            // 21:出荷先区分 22:預り先区分 23:運送便区分   24:倉庫区分  25:備考      26:登録者ID   27:登録日      28:登録時刻
            //----------------------------------------------------
            // ★Excel出力処理 (取引先マスタ × 取引先部門マスタで部門分を展開)
            //----------------------------------------------------
            // 部門マップ作成: 取引先CD -> List<部門CD>
            var bumonMap = new Dictionary<string, List<string>>();
            foreach (var bline in lines_bumon)
            {
                if (string.IsNullOrWhiteSpace(bline)) continue;
                var p = bline.Split(' ');
                if (p.Length < 2) continue;
                var tcode = p[0];
                var bcode = p[1];
                if (!bumonMap.ContainsKey(tcode)) bumonMap[tcode] = new List<string>();
                if (!bumonMap[tcode].Contains(bcode)) bumonMap[tcode].Add(bcode);
            }

            // 指定した company に属する部門のみ出力するための許可部門セットを作成
            // 部門マスタ(BUMONmf)の 4番目の項目が会社名
            var allowedBumons = new HashSet<string>();
            var bumonLines = fam.CheckAndLoadMater(BUMONmf, BUMON_mst, CMD.utf8, 1);
            foreach (var bl in bumonLines)
            {
                if (string.IsNullOrWhiteSpace(bl)) continue;
                var bp = bl.Split(' ');
                if (bp.Length > 3)
                {
                    var bcode = bp[0];
                    var bcompany = bp[3];
                    if (!string.IsNullOrEmpty(bcode) && bcompany == company)
                        allowedBumons.Add(bcode);
                }
            }

            // lines_bumonをフィルタリングして、allowedBumonsに含まれる部門のみを保持
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

            
            //// 出力行リストを作成
            //var cols = Enum.GetNames(typeof(TORIHIKI_MASTER_OUT));
            //var outRows = new List<string[]>();
            //foreach (var line in lines)
            //{
            //    if (string.IsNullOrWhiteSpace(line)) continue;
            //    var parts = line.Split(' ');
            //    var baseRow = new string[cols.Length];
            //    for (int i = 0; i < baseRow.Length; i++) baseRow[i] = string.Empty;

            //    // parts は 部門CD を含まない取引先マスタの列順(TORIHIKI_MASTER)
            //    // 出力レイアウト(TORIHIKI_MASTER_OUT)は 部門CD が index=1 に挿入されているため
            //    // マッピングは以下のように行う: parts[0] -> out[取引先CD], parts[1] -> out[取引先正式名] (out index 2)
            //    if (parts.Length > 0) baseRow[(int)TORIHIKI_MASTER_OUT.取引先CD] = parts[0];
            //    for (int pi = 1; pi < parts.Length; pi++)
            //    {
            //        int outIdx = pi + 1; // shift because 部門CD inserted at index 1
            //        if (outIdx < baseRow.Length) baseRow[outIdx] = parts[pi];
            //    }

            //    // 部門リストがあれば部門ごとに行を追加、なければ部門空で1行追加
            //    var key = baseRow[(int)TORIHIKI_MASTER_OUT.取引先CD];
            //    if (!string.IsNullOrEmpty(key) && bumonMap.TryGetValue(key, out var bList) && bList.Count > 0)
            //    {
            //        foreach (var b in bList)
            //        {
            //            var row = (string[])baseRow.Clone();
            //            row[(int)TORIHIKI_MASTER_OUT.部門CD] = b;
            //            outRows.Add(row);
            //        }
            //    }
            //    else
            //    {
            //        // 部門なしは空の部門CDで出力
            //        outRows.Add(baseRow);
            //    }
            //}

            // Excel保存ダイアログ表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"{mst}_{company}.xlsx";
                sfd.Filter = "Excelファイル (*.xlsx)|*.xlsx";
                sfd.Title = "保存先を指定してください";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                string filePath = sfd.FileName;

                Microsoft.Office.Interop.Excel.Application excelApp = null;
                Workbook workbook = null;
                Worksheet worksheet = null;
                try
                {
                    // ----------------------------------------------------
                    // ★アニメーションフォーム表示
                    // ----------------------------------------------------
                    StartEndAnimationThread(AnimationPattern.開く);

                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    workbook = excelApp.Workbooks.Add();
                    worksheet = (Worksheet)workbook.Worksheets[1];

                    // ヘッダー出力
                    for (int c = 0; c < cols.Length; c++)
                    {
                        worksheet.Cells[1, c + 1] = cols[c];
                    }

                    // 取引先CD、部門CD、郵便番号、電話、FAX は文字列として扱い先頭0を維持
                    var textCols = new[] {
                        (int)TORIHIKI_MASTER_OUT.取引先CD,
                        (int)TORIHIKI_MASTER_OUT.部門CD,
                        (int)TORIHIKI_MASTER_OUT.郵便番号,
                        (int)TORIHIKI_MASTER_OUT.電話番号1,
                        (int)TORIHIKI_MASTER_OUT.電話番号2,
                        (int)TORIHIKI_MASTER_OUT.FAX番号1,
                        (int)TORIHIKI_MASTER_OUT.FAX番号2
                    };
                    foreach (var idx in textCols)
                    {
                        try
                        {
                            ((Range)worksheet.Columns[idx + 1]).NumberFormat = "@"; // テキストフォーマット
                        }
                        catch {
                            // 念のため例外は無視して続行
                        }
                    }

                    // データ出力
                    for (int r = 0; r < outRows.Count; r++)
                    {
                        var row = outRows[r];
                        for (int c = 0; c < cols.Length; c++)
                        {
                            // 明示的に文字列を代入してExcelが数値に変換するのを抑制
                            worksheet.Cells[r + 2, c + 1] = row[c] ?? string.Empty;
                        }
                    }

                    // 列幅自動調整
                    worksheet.Columns.AutoFit();

                    // 保存
                    workbook.SaveAs(sfd.FileName, XlFileFormat.xlOpenXMLWorkbook);

                    //----------------------------------------------------
                    // ★アニメーションフォーム閉じる
                    //----------------------------------------------------
                    StartEndAnimationThread(AnimationPattern.閉じる);

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

        /// <summary>
        /// 部門マスターから部門一覧を作成(cmbBx部門.Items)
        /// </summary>
        /// <returns></returns>
        private string[] GetBumonDefault()
        {
            // コンボボックスクリア
            cmbBx部門.Items.Clear();

            var list = new List<string>
            {
                "", // 空白行追加
            };

            var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst_bumon, CMD.utf8, 1);
            for (int j = 0; j < lines_bumon.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(lines_bumon[j])) continue;
                var parts_bumon = lines_bumon[j].Split(' ');
                if (parts_bumon.Length > 1)
                {
                    list.Add(parts_bumon[1]);
                }
            }
            list = list
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x.Split(' ')[0])
                .ToList();
            return list.Distinct().ToArray();
        }

        /// <summary>
        /// bumonCDで部門マスターから会社名取得
        /// </summary>
        /// <param name="bumonCD"></param>
        /// <returns></returns>
        private string GetCompanyFromBumonCD(string bumonCD)
        {
            // bumonCDで部門マスター(Path.Combine(CMD.mfPath,BUMON.txt))から会社名取得
            // 1:部門CD 2:部門名 3:部門名カナ 4:会社名
            string company = "";
            var BUMON_lines= fam.CheckAndLoadMater(BUMONmf, BUMON_mst, CMD.utf8, 1);
            for (int j = 0; j < BUMON_lines.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(BUMON_lines[j])) continue;
                var parts_bumon = BUMON_lines[j].Split(' ');
                if (parts_bumon.Length > 3 && parts_bumon[0] == bumonCD)
                {
                    company = parts_bumon[3];
                    break;
                }
            }

            return company;
        }

        private (string, string) JudgeMF(JudgeMfPattern pattern, string company)
        {
            if (pattern == JudgeMfPattern.パスのみ)
            {
                switch (company)
                {
                    case "オーノ":
                        mf = mfTxtPaths[0];
                        break;
                    case "サンミックダスコン":
                        mf = mfTxtPaths[1];
                        break;
                    case "サンミックカーペット":
                        mf = mfTxtPaths[2];
                        break;
                }
                return (mf,null);
            }
            else if (pattern == JudgeMfPattern.パスとファイル名)
            {
                switch (company)
                {
                    case "オーノ":
                        mf = mfTxtPaths[0];
                        mfName = mfTxtNames[0];
                        break;
                    case "サンミックダスコン":
                        mf = mfTxtPaths[1];
                        mfName = mfTxtNames[1];
                        break;
                    case "サンミックカーペット":
                        mf = mfTxtPaths[2];
                        mfName = mfTxtNames[2];
                        break;
                }
            }
            return (mf, mfName);
        }

        /// <summary>
        /// アニメーション表示・非表示(FormAnimation3)
        /// </summary>
        /// <param name="ocFlg"></param>
        private async void StartEndAnimationThread(AnimationPattern pattern)
        {
            WaitExcelExport anim = null;
            if (pattern == AnimationPattern.開く)
            {
                // ----------------------------------------------------
                // ★アニメーションフォーム表示
                // ----------------------------------------------------
                animThread = new Thread(() =>
                {
                    using (WaitExcelExport a = new WaitExcelExport())
                    {
                        animForm = a; // 外部参照用
                        Application.Run(a); // GIF表示
                    }
                });
                animThread.SetApartmentState(ApartmentState.STA);
                animThread.Start();

                await Task.Delay(100); // ちょっと待って anim が作られる
            }
            else if (pattern == AnimationPattern.閉じる)
            {
                //----------------------------------------------------
                // ★アニメーションフォーム閉じる
                //----------------------------------------------------
                if (animForm != null && !animForm.IsDisposed)
                    animForm.Invoke(new Action(() => animForm.CloseForm()));

                // アニメーションスレッド終了を待つ
                if (animThread != null && animThread.IsAlive)
                    animThread.Join();
            }
        }
    }
}
