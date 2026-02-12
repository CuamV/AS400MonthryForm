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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
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
        string TIM;
        string BUMONmf = Path.Combine(CMD.mfPath, "BUMON.txt");
        string mfName = "TORIHIKI";
        string mf = Path.Combine(CMD.mfPath, "TORIHIKI.txt");
        string mf_bumonName = "TORIHIKI-BUMON";
        string mf_bumon = Path.Combine(CMD.mfPath, "TORIHIKI-BUMON.txt");
        string[] mf_toriroruNames = new[] { 
        "TROLE-SYOSYA", "TROLE-SIIRE", "TROLE-HANBAI", "TROLE-TOKUISAKI", "TROLE-SYUKKA", "TROLE-AZUKARI", "TROLE-UNSOU", "TROLE-SOUKO" };
        string[] mf_toriroruPaths = new[]
        {
            Path.Combine(CMD.mfPath, "TROLE-SYOSYA.txt"),
            Path.Combine(CMD.mfPath, "TROLE-SIIRE.txt"),
            Path.Combine(CMD.mfPath, "TROLE-HANBAI.txt"),
            Path.Combine(CMD.mfPath, "TROLE-TOKUISAKI.txt"),
            Path.Combine(CMD.mfPath, "TROLE-SYUKKA.txt"),
            Path.Combine(CMD.mfPath, "TROLE-AZUKARI.txt"),
            Path.Combine(CMD.mfPath, "TROLE-UNSOU.txt"),
            Path.Combine(CMD.mfPath, "TROLE-SOUKO.txt"),
        };

        string BUMON_mst = "部門マスタ";
        string mst = "取引先マスタ";
        string mst_bumon = "取引先部門マスタ";
        string mst_toriroru = "取引先ロール別マスタ";

        List<Control> _inputControls;
        private WaitExcelExport animForm;
        private Thread animThread;

        List<string> newLine_syosya;
        List<string> newLine_siire;
        List<string> newLine_hanbai;
        List<string> newLine_tokui;
        List<string> newLine_syukka;
        List<string> newLine_azukari;
        List<string> newLine_unsou;
        List<string> newLine_souko;
        List<List<string>> newLineRoruList;
        List<string> newLines_bumon;

        //========================================================
        // コンストラクタ
        //========================================================
        public 取引先マスタFm()
        {
            InitializeComponent();
            this.Load += 取引先マスタForm_Load;

            // 各リストを初期化
            newLine_syosya = new List<string>();
            newLine_siire = new List<string>();
            newLine_hanbai = new List<string>();
            newLine_tokui = new List<string>();
            newLine_syukka = new List<string>();
            newLine_azukari = new List<string>();
            newLine_unsou = new List<string>();
            newLine_souko = new List<string>();

            // newLineRoruListの初期化をここで行う
            newLineRoruList = new List<List<string>>()
            {
                newLine_syosya, newLine_siire, newLine_hanbai,
                newLine_tokui, newLine_syukka, newLine_azukari,
                newLine_unsou, newLine_souko
            };
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

            // 取引先CDが空白の場合未処理
            if (string.IsNullOrEmpty(inputTorihiki)) return;
            // 取引先CDが７桁未満の場合未処理
            if (inputTorihiki.Length != 7) return;
            
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

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
            //----------------------------------------------------
            // ★入力内容取得NO1
            //----------------------------------------------------
            Dictionary<string, string> CheacItemkDic = new Dictionary<string, string>
            {
                {"部門コード", cmbBx部門.SelectedItem.ToString() },
                {"取引先ロール選択数", chkListBx取引先ロール.SelectedItems.Count.ToString() },
            };
            int count = int.Parse(CheacItemkDic["取引先ロール選択数"]);
            // ----------------------------------------------------
            // ★入力内容チェックNO1
            // ----------------------------------------------------
            if (!fam.ValidateInput(VaridationPattern.取引先マスタ関連登録先チェック, MstMntPattern.登録, CheacItemkDic)) return;

            //----------------------------------------------------
            // ★入力内容取得NO2
            //----------------------------------------------------
            string bumonCD = cmbBx部門.SelectedItem?.ToString() ?? "";

            // 0:商社 1:仕入先 2:販売先 4:得意先 5:出荷先 6:預り先 7:運送便 8:倉庫
            // チェックボックス状態取得
            // チェックあり→"1", チェックなし→"0"　としてtoriRoll配列に格納
            string[] toriRorus = new string[chkListBx取引先ロール.Items.Count];
            for (int i = 0; i < chkListBx取引先ロール.Items.Count; i++)
                toriRorus[i] = chkListBx取引先ロール.GetItemChecked(i) ? "1" : "0";

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
            // ★入力内容チェックNO2
            // ----------------------------------------------------
            // 空白/取引先コード半角数字7桁/登録実行確認
            if (!fam.ValidateInput(VaridationPattern.必須項目登録前初期チェック, MstMntPattern.登録, ToriInTxtDic, 7, mst)) return;

            //----------------------------------------------------
            // ★入力内容取得NO3
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
                { "商社区分", toriRorus[0] },
                { "仕入先区分", toriRorus[1] },
                { "販売先区分", toriRorus[2] },
                { "得意先区分", toriRorus[3] },
                { "出荷先区分", toriRorus[4] },
                { "預り先区分", toriRorus[5] },
                { "運送便区分", toriRorus[6] },
                { "倉庫区分", toriRorus[7] },
                { "備考", txtBx備考.Text.Trim() }
            };
            foreach (var key in ToriInTxtDic2.Keys)
                ToriInTxtDic[key] = ToriInTxtDic2[key];

            // ----------------------------------------------------
            // ★入力内容チェックNO3
            // ----------------------------------------------------
            // 郵便/電話/FAX番号半角数字チェック(アルファベット不可)
            if (!fam.ValidateInput(VaridationPattern.入力値チェック, MstMntPattern.登録, ToriInTxtDic2)) return;

            // ----------------------------------------------------
            // ★マスタ登録レコード形成
            // ----------------------------------------------------
            // [取引先マスタ]（半角スペース区切り）
            //  1:取引先CD   2:取引先正式名称 3:取引先名    4:取引先名カナ 5:取引先略名  6:取引先略名カナ 7:郵便番号   8:電話番号1
            //  9:電話番号2  10:FAX番号1    11:FAX番号2  12:住所1      13:住所1カナ  14:住所2       15:住所2カナ 16:商社区分
            // 17:仕入先区分 18:販売先区分   19:得意先区分 20:出荷先区分  21:預り先区分 22:運送便区分   23:倉庫区分  24:備考
            // 25:登録者ID  26:登録日       27:登録時刻
            //----------------------------------------------------
            TIM = DateTime.Now.ToString("HHmmss");
            List<string> newLinesList = new List<string>();
            var newFields = new[] { ToriInTxtDic["取引先コード"], ToriInTxtDic["取引先正式名"], ToriInTxtDic["取引先名"], ToriInTxtDic["取引先名カナ"],
                                    ToriInTxtDic["取引先略名"], ToriInTxtDic["取引先略名カナ"], ToriInTxtDic2["郵便番号"],
                                    ToriInTxtDic2["電話番号1"], ToriInTxtDic2["電話番号2"], ToriInTxtDic2["FAX番号1"],ToriInTxtDic2["FAX番号2"],
                                    ToriInTxtDic2["住所1"], ToriInTxtDic2["住所1カナ"], ToriInTxtDic2["住所2"] ,ToriInTxtDic2["住所2カナ"],
                                    ToriInTxtDic2["商社区分"], ToriInTxtDic2["仕入先区分"], ToriInTxtDic2["販売先区分"], ToriInTxtDic2["得意先区分"],
                                    ToriInTxtDic2["出荷先区分"], ToriInTxtDic2["預り先区分"], ToriInTxtDic2["運送便区分"], ToriInTxtDic2["倉庫区分"],
                                    ToriInTxtDic2["備考"], CMD.UserID, CMD.HIZ, TIM };
            var newLine = string.Join(" ", newFields.Select(x => string.IsNullOrEmpty(x) ? "" : x));

            newLinesList.Add(newLine);

            if (!string.IsNullOrEmpty(bumonCD)) // 部門コードがある場合のみ登録可
            {
                // [取引先部門マスタ]
                //  1:取引先CD 2:部門CD
                //----------------------------------------------------
                newLines_bumon = new List<string>
                {
                    ($"{ToriInTxtDic["取引先コード"]} {bumonCD}")
                };

                if (count > 0) // 取引先ロールが1つ以上選択されている場合
                {
                    for (int i = 0; i < toriRorus.Length; i++)
                    {
                        if (toriRorus[i] == "1")
                        {
                            if (i == 3)
                                // [取引先ロール別マスタ] 得意先(newLines_tokui)の場合
                                // 1:取引先CD 2:部門CD 3:取引先名 4:取引先名カナ 5:適用開始日付 6:適用終了日付
                                //----------------------------------------------------
                                newLineRoruList[3].Add($"{ToriInTxtDic["取引先コード"]} {bumonCD} {ToriInTxtDic["取引先名"]} {ToriInTxtDic["取引先名カナ"]} 00000101 99991231 {CMD.HIZ} {TIM}");
                            else
                                // [取引先ロール別マスタ] 得意先以外の場合
                                // 1:取引先CD 2:部門CD 3:取引先名 4:取引先名カナ
                                //----------------------------------------------------
                                newLineRoruList[i].Add($"{ToriInTxtDic["取引先コード"]} {bumonCD} {ToriInTxtDic["取引先名"]} {ToriInTxtDic["取引先名カナ"]}");
                        }
                    }
                }
            }

            // ----------------------------------------------------
            // ★マスタ登録処理
            // ----------------------------------------------------
            // [取引先マスタ]
            // ----------------------------------------------------
            // ファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 0);
            // 新規・変更登録
            bool replaced1;
            (lines, replaced1) = fam.AddMasterFile(AddMasterPattern.Keyが1項目, lines, newLinesList);
            // バックアップ
            fam.BackupMaster(mf, mfName, "Add", mst);
            // ファイル書き込み
            File.WriteAllLines(mf, lines, Encoding.UTF8);
            // ログ登録
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} マスタ登録 1 {CMD.UserName} btn登録_Click {mst}");
            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst}が更新されました");

            if (!string.IsNullOrEmpty(bumonCD)) // 部門コードがある場合のみ登録可
            {
                // [取引先部門マスタ]
                // ----------------------------------------------------
                // 取引先部門マスタファイル有無チェック＆読込
                var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst_bumon, CMD.utf8, 0);
                bool replaced2;
                // 新規・変更登録
                (lines_bumon, replaced2) = fam.AddMasterFile(AddMasterPattern.Keyが1項目と2項目, lines_bumon, newLines_bumon);
                // バックアップ
                fam.BackupMaster(mf_bumon, mf_bumonName, "Add", mst_bumon);
                // ファイル書き込み
                File.WriteAllLines(mf_bumon, lines_bumon, Encoding.UTF8);
                // ログ登録
                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_bumon}が更新されました");

                if (count > 0) // 取引先ロールが1つ以上選択されている場合
                {
                    // [取引先ロール別マスタ]
                    // ----------------------------------------------------
                    // 取引先ロール別マスターファイル有無チェック＆読込
                    for (int i = 0; i < newLineRoruList.Count; i++)
                    {
                        string mf_toriroll = mf_toriroruPaths[i];

                        // 取引先ロール別マスターファイル有無チェック＆読込
                        var lines_toriroru = fam.CheckAndLoadMater(mf_toriroll, mst, CMD.utf8, 0);

                        // まず既存の該当レコードを削除（取引先CD + 部門CDで検索）
                        var existingLine = lines_toriroru.FirstOrDefault(x =>
                        {
                            if (string.IsNullOrWhiteSpace(x)) return false;
                            var parts = x.Split(' ');
                            return parts.Length >= 2 && parts[0] == ToriInTxtDic["取引先コード"] && parts[1] == bumonCD;
                        });

                        if (existingLine != null)
                        {
                            lines_toriroru.Remove(existingLine);
                        }

                        // チェックがONの場合のみ新規追加
                        if (newLineRoruList[i].Count > 0)
                        {
                            bool replaced_toriroru;

                            if (i == 3)
                                // 得意先の場合新規追加もしくは全列完全一致で上書き
                                (lines_toriroru, replaced_toriroru) = fam.AddMasterFile(AddMasterPattern.Keyなし, lines_toriroru, newLineRoruList[i]);
                            else
                                // 得意先以外は差分置換
                                (lines_toriroru, replaced_toriroru) = fam.AddMasterFile(AddMasterPattern.Keyが1項目と2項目, lines_toriroru, newLineRoruList[i]);
                        }
                        // バックアップ
                        fam.BackupMaster(mf_toriroruPaths[i], mf_toriroruNames[i], "Add", mst_toriroru);
                        // ファイル書き込み
                        File.WriteAllLines(mf_toriroruPaths[i], lines_toriroru, Encoding.UTF8);
                        // ログ登録
                        HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                        fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_toriroru}が更新されました");
                    }
                }
            }

            // 入力内容クリア
            fam.ClearInput(_inputControls);

            MessageBox.Show(replaced1 ? "変更登録が完了しました。" : "新規登録が完了しました。",
                $"{mst}登録", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        /// <summary>
        /// 削除ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn削除_Click(object sender, EventArgs e)
        {
            //----------------------------------------------------
            // ★入力内容取得　削除には取引先CDのみでOK
            //----------------------------------------------------
            string torihikisakiCD = txtBx取引先CD.Text.Trim();  // [取引先CD]
            Dictionary<string, string> ToriInTxtDic = new Dictionary<string, string>
            {
                { "取引先コード",txtBx取引先CD.Text.Trim() },
             };

            // 空白/取引先コード半角数字7桁/削除実行確認
            if (!fam.ValidateInput(VaridationPattern.必須項目登録前初期チェック, MstMntPattern.削除, ToriInTxtDic, 7, mst)) return;
            
            //----------------------------------------------------
            // 削除処理
            //----------------------------------------------------
            // 取引先マスタファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);
            // 取引先部門マスタファイル有無チェック＆読込
            var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst_bumon, CMD.utf8, 1);
            // 取引先ロール別マスターファイル有無チェック＆読込
            var lines_toriroru_list = new List<List<string>>();
            for (int i = 0; i < mf_toriroruPaths.Length; i++)
            {
                var lines_toriroru = fam.CheckAndLoadMater(mf_toriroruPaths[i], mst_toriroru, CMD.utf8, 1);
                lines_toriroru_list.Add(lines_toriroru);
            }

            // [取引先マスタ]
            // 該当レコード検索
            string target = lines.FirstOrDefault(x =>
            {
                var parts = x.Split(' ');
                return parts.Length > 0 && parts[0] == torihikisakiCD;
            });

            if (target == null)
            {
                MessageBox.Show("該当する取引先が取引先マスタに存在しません。",
                    "削除不可", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // [取引先マスタ] レコード削除
            lines.Remove(target);
            lines = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x.Split(' ')[0])
                .ToList();

            // [取引先部門マスタ]
            // 該当レコードの削除(取引先CDが一致するレコード全削除)
            lines_bumon = lines_bumon
                .Where(x =>
                {
                    if (string.IsNullOrWhiteSpace(x)) return false;
                    var parts = x.Split(' ');
                    return parts.Length < 1 || parts[0] != torihikisakiCD; // 取引先CD不一致のみ残す
                })
                .OrderBy(x => x.Split(' ')[0])
                .ToList();

            // [取引先ロール別マスタ]
            // 取引先CDが一致するレコード全削除
            for (int i = 0; i < lines_toriroru_list.Count; i++)
            {
                var lines_toriroru = lines_toriroru_list[i];
                
                // 取引先CDが一致しないレコードのみ残す
                lines_toriroru = lines_toriroru
                    .Where(x =>
                    {
                        if (string.IsNullOrWhiteSpace(x)) return false;
                        var parts = x.Split(' ');
                        return parts.Length < 1 || parts[0] != torihikisakiCD;
                    })
                    .OrderBy(x => x.Split(' ')[0])
                    .ToList();

                // バックアップ
                fam.BackupMaster(mf_toriroruPaths[i], mf_toriroruNames[i], "Add", mst_toriroru);
                // ファイル書き込み
                File.WriteAllLines(mf_toriroruPaths[i], lines_toriroru, CMD.utf8);
            }

            //------------------------------------------------
            // ★バックアップ＆ファイル書き込み
            //------------------------------------------------
            // バックアップ
            fam.BackupMaster(mf, mfName, "Add", mst);
            fam.BackupMaster(mf_bumon, mf_bumonName, "Add", mst_bumon);

            // ファイル書き込み
            File.WriteAllLines(mf, lines, CMD.utf8);
            File.WriteAllLines(mf_bumon, lines_bumon, CMD.utf8);

            // 入力内容クリア
            fam.ClearInput(_inputControls);

            MessageBox.Show("削除登録が完了しました。",
                $"{mst}削除", MessageBoxButtons.OK, MessageBoxIcon.None);


            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} マスタ削除 1 {CMD.UserName} btn削除_Click {mst}");
            fam.AddLog2($"{HIZTIM} マスタ削除 0 {CMD.UserName} btn削除_Click {mst}が更新されました");
            fam.AddLog2($"{HIZTIM} マスタ削除 0 {CMD.UserName} btn削除_Click {mst_bumon}が更新されました");
            fam.AddLog2($"{HIZTIM} マスタ削除 0 {CMD.UserName} btn削除_Click {mst_toriroru}が更新されました");
        }

        /// <summary>
        /// 照会ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn照会_Click(object sender, EventArgs e)
        {
            //マスターファイル有無チェック＆読込
            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8, 1);

            // 照会Formを開く（会社名を渡す）
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
                var bparts = bline.Split(' ');
                if (bparts.Length >= 2)
                {
                    var toriCD = bparts[0];
                    var bumonCD = bparts[1];
                    if (!bumonMap.ContainsKey(toriCD))
                        bumonMap[toriCD] = new List<string>();
                    bumonMap[toriCD].Add(bumonCD);
                }
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
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ');
                if (parts.Length < 17) continue; // 最低限のフィールド数チェック

                // 取引先マスタのレイアウト: 
                // 1:取引先CD 2:取引先正式名 3:取引先名 4:取引先名カナ 5:取引先略名 6:取引先略名カナ 7:郵便番号 8:電話番号1
                // 9:電話番号2 10:FAX番号1 11:FAX番号2 12:住所1 13:住所1カナ 14:住所2 15:住所2カナ 16:商社区分
                // 17:仕入先区分 18:販売先区分 19:得意先区分 20:出荷先区分 21:預り先区分 22:運送便区分 23:倉庫区分 24:備考
                // 25:登録者 26:登録日付 27:登録時刻

                var 取引先CD = parts[0];

                // この取引先に紐づく部門リストを取得
                if (filteredBumonMap.TryGetValue(取引先CD, out var bumonList) && bumonList.Count > 0)
                {
                    // 部門ごとに行を作成
                    foreach (var bumonCD in bumonList)
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
            }
            // Excel保存ダイアログ表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"{mst}.xlsx";
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
                        catch {
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
        /// 部門マスターから部門一覧を作成(cmbBx部門.Items, chkListBx部門.Items)
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

            var BUMON_lines = fam.CheckAndLoadMater(BUMONmf, BUMON_mst, CMD.utf8, 1);
            for (int j = 0; j < BUMON_lines.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(BUMON_lines[j])) continue;
                var BUMON_parts = BUMON_lines[j].Split(' ');
                if (BUMON_parts.Length > 0)
                {
                    list.Add(BUMON_parts[0]);
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
