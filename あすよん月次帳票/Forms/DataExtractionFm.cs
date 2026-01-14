using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Excel = Microsoft.Office.Interop.Excel;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    //==========================================================
    // --------Form2(データ抽出)クラス--------
    //==========================================================
    internal partial class DataExtractionFm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        ColorManager clrmg = new ColorManager();
        Processor processor = new Processor();
        Summarize dsumm = new Summarize();
        FormAction fam = new FormAction();

        // フィールド変数
        private string HIZTIM;
        private string startDate;
        private string endDate;
        private string Hiz;
        private string Tim;
        private WaitExcelExport animForm;
        private Thread animThread;

        // 選択された会社と部門
        private List<string> selCompanies = new List<string>();
        private List<Department> selBumons = new List<Department>();
        private List<Torihiki> selSelleres = new List<Torihiki>();
        private List<Torihiki> selSupplieres = new List<Torihiki>();

        // 取引先コード -> 部門コード集合（販売／仕入で別管理）
        private Dictionary<string, HashSet<string>> salesDeptMap = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<string>> supplierDeptMap = new Dictionary<string, HashSet<string>>();

        private List<GroupBox> cBoxList;

        // Excel
        private Excel.Application xlApp = null;
        private Excel.Workbook xlBook = null;
        private Excel.Worksheet xlSheet = null;
        private Excel.Range cdRange = null;
        private Excel.Range start2 = null;
        private Excel.Range end2 = null;

        //=========================================================
        // コンストラクタ
        //=========================================================
        internal DataExtractionFm()
        {
            InitializeComponent();

            cBoxList = new List<GroupBox>
            {
                grpBx抽出期間,
                grpBx組織,
                grpBx取引先,
                grpBxBtn,
                grpBxデータ区分,
                grpBxクラス区分,
                grpBx在庫種別,
                grpBx売仕集計区分,
                grpBx在集計区分,
            };

            // RplForm2の全グループボックスを配列化して共通のPaintイベントを設定
            foreach (var gb in cBoxList)
                gb.Paint += GroupBoxCustomBorder;

            // Form2読込ログ
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} FormOpen 1 {CMD.UserName} Form2");

            this.Load += Form2_Load;

            //// DataGridViewスタイル設定
            //StyleDataGrid(dgvDataOhno, Color.DarkBlue, Color.White, Color.LightGray);
            //StyleDataGrid(dgvDataSdus, Color.DarkGreen, Color.White, Color.LightGray);
            //StyleDataGrid(dgvDataScar, Color.DarkRed, Color.White, Color.LightGray);
            //StyleDataGrid(dgvDataIV, Color.Gray, Color.White, Color.LightGray);

            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        /// <summary>
        /// Form2(データ抽出)読込時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void Form2_Load(object sender, EventArgs e)
        {
            ApplySnowManColors();

            this.MouseDown += RplForm2_MouseDown;
            this.MouseMove += RplForm2_MouseMove;
            this.MouseUp += RplForm2_MouseUp;

            // ★アニメーション登録
            SetButtonAnimation(btnDisplay);
            SetButtonAnimation(btnForm1Back);

            // デフォルトで明細にしておく
            rdBtn売仕なし.Checked = true;
            rdBtn在なし.Checked = true;
        }

        //=========================================================
        // 【コントロール実行メソッド】
        //=========================================================
        /// <summary>
        /// コントロール制御
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBxControl(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} chkBxControl");

            bool showSd = chkBxSundus.Checked;
            bool showSl = chkBxSl.Checked;
            bool showPr = chkBxPr.Checked;
            bool showIv = chkBxIv.Checked;

            // 売・仕・在の排他制御
            chkBxSl.Enabled = !showIv;
            chkBxPr.Enabled = !showIv;
            chkBxIv.Enabled = !showSl && !showPr;

            // 在庫関連の初期有効状態(売上・仕入が選ばれていない場合のみ)
            chkBx加工T.Enabled = !showSl && !showPr;
            chkBx預りT.Enabled = !showSl && !showPr;
            chkBx預けT.Enabled = !showSl && !showPr;
            foreach (Control cBox in grpBx在庫種別.Controls)
                cBox.Enabled = !showSl && !showPr;

            // 売・仕選択時は在庫集計無効
            foreach (Control cBox in grpBx在集計区分.Controls.OfType<RadioButton>())
                cBox.Enabled = !showSl && !showPr;

            // 在庫選択時は売仕集計無効
            foreach (Control cBox in grpBx売仕集計区分.Controls.OfType<RadioButton>())
                cBox.Enabled = !showIv;

            // 在庫選択＋年月入力内容に応じた制御
            if (showIv)
            {
                string inputYm = txtBxEndYearMonth.Text.Trim(); // 入力年月
                if (string.IsNullOrEmpty(inputYm)) return;

                string monthlyFile = @"\\ohnosv01\OhnoSys\099_sys\mf\Monthly.txt";
                if (!File.Exists(monthlyFile)) return;

                string firstLine = File.ReadLines(monthlyFile).FirstOrDefault();
                if (string.IsNullOrEmpty(firstLine)) return;

                string currentYm = firstLine.Substring(0, 6); // 先頭6文字を取得(当月)

                // 当月の場合 → grpBx在庫種別無効、預りT/預けTは有効
                if (inputYm == currentYm) // 当月
                                          // 在庫選択＋サンミックダスコン、預りT/預けTを無効化
                {
                    if (showSd)
                    {
                        foreach (Control cBox in grpBx在庫種別.Controls)
                            cBox.Enabled = false;
                        chkBx加工T.Enabled = true;
                        chkBx預りT.Enabled = false;
                        chkBx預けT.Enabled = false;
                    }
                    else
                    {
                        foreach (Control cBox in grpBx在庫種別.Controls)
                        {
                            cBox.Enabled = false;
                            chkBx加工T.Enabled = true;
                            chkBx預りT.Enabled = true;
                            chkBx預けT.Enabled = true;
                        }
                    }
                }
                // 過去月の場合 → grpBx在庫種別有効、預りT/預けTは無効
                else if (string.Compare(inputYm, currentYm) < 0) // 過去月
                {
                    foreach (Control cBox in grpBx在庫種別.Controls)
                        cBox.Enabled = true;
                    chkBx加工T.Enabled = false;
                    chkBx預りT.Enabled = false;
                    chkBx預けT.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 部門選択リンククリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLb部門_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} linkLb部門_LinkClicked");

            // 部門Formを作成
            using (var BuForm = new 部門Form(selBumons))
            {
                // モーダル表示
                if (BuForm.ShowDialog() == DialogResult.OK)
                {
                    // returnされた部門リストに更新
                    selBumons = BuForm.GetSelectedBumons();

                    // BuFormで選択された部門リストを取得して listBx部門 に反映
                    UpdateCompanyCheckboxesFromBumon(selBumons);
                    RefreshListBx(listBx部門, selBumons.Select(b => new Torihiki
                    {
                        Code = b.Code,
                        Name = b.Name,
                        Company = b.Company,
                        DeptCode = b.Code,
                        DeptName = b.Name
                    }).ToList());
                }
            }
        }

        /// <summary>
        /// 販売先選択リンクをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLb販売先_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} linkLb販売先_LinkClicked");

            using (var frm = new 販売仕入先Form("HANBAI", selSelleres, salesDeptMap))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // 戻り値は「チェックされた全取引先×部門」リスト（同コードでも複数行ある）
                    var returned = frm.GetSelectedItems();

                    // 選択なしなら deptMap もクリア
                    if (returned.Count == 0)
                    {
                        salesDeptMap.Clear(); // SHIIREなら supplierDeptMap.Clear()
                    }
                    else
                    {
                        // salesDeptMap を追加で更新（既存を上書きせずにマージ）
                        AddDeptMapRange(salesDeptMap, returned);
                    }

                    // selSelleres は listBox 表示用に "コード毎に1行" を維持
                    selSelleres = returned
                        .GroupBy(t => t.Code + "|" + t.Company)
                        .Select(g => g.First()) // 表示用は1行
                        .ToList();

                    // 会社チェックと部門一覧更新（追加）
                    UpdateBumonAndCompanyFromTorihiki(returned);

                    // listbox 表示（コード＋名称）
                    RefreshListBx(listBx販売先, selSelleres);
                }
            }
        }

        /// <summary>
        /// 仕入先選択リンククリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLb仕入先_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} linkLb仕入先_LinkClicked");

            using (var frm = new 販売仕入先Form("SHIIRE", selSupplieres, supplierDeptMap))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var returned = frm.GetSelectedItems();
                    AddDeptMapRange(supplierDeptMap, returned);

                    selSupplieres = returned
                        .GroupBy(t => t.Code + "|" + t.Company)
                        .Select(g => g.First())
                        .ToList();

                    UpdateBumonAndCompanyFromTorihiki(returned);

                    RefreshListBx(listBx仕入先, selSupplieres);
                }
            }
        }

        /// <summary>
        /// データ表示実行ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnReadData_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btnReadData_Click");

            //---------------------------------------------------- 
            // ★ エラーチェック
            //----------------------------------------------------
            if (ErrCheck())
                return;

            //----------------------------------------------------
            // ★アニメーションフォーム表示
            //----------------------------------------------------
            // --- FormAnimation スレッド ---
            WaitSnowMan anim = null;
            Thread animThread = new Thread(() =>
            {
                using (WaitSnowMan a = new WaitSnowMan())
                {
                    anim = a; // 外部参照用
                    a.Shown += (s, i) =>
                    {
                        a.Invoke((Action)(() =>
                        {
                            anim.lblMessage.Text = "表示用データ作成中です…\r\n";
                            anim.BackColor = clrmg.KojiDark1;
                        }));
                    };
                    Application.Run(a); // GIF表示
                }
            });
            animThread.SetApartmentState(ApartmentState.STA);
            animThread.Start();

            await Task.Delay(100); // ちょっと待って anim が作られる

            //----------------------------------------------------
            // ★表示＆Excelのデータ抽出(明細)
            //----------------------------------------------------
            // メインデータ作成処理(メインスレッドでデータ抽出) 
            (DataTable slprResult, DataTable stockDtNow, DataTable stockDtOld,
                List<string> selDatas, string selAggregte, string selBookName, Dictionary<string, List<string>> conList) result;

            try
            {
                // 初回トライ
                result = MakeMainData();
            }
            catch (Ohno.Db.ConnectionFailedException)
            {
                // 「Ohno.Db.ConnectionFailedException:
                // '以下のエラーが発生したため、データベースに接続できませんでした。 ネットワークに問題があるため、データベースに接続できません。
                await Task.Delay(500);

                try
                {
                    // リトライ
                    result = MakeMainData();
                }
                catch (Exception ex) 
                {
                    // 2回とも失敗→アニメーションを閉じてユーザー通知
                    CloseAnimation(anim, animThread);

                    MessageBox.Show("AS400への接続に失敗しました。\n" +
                        "ネットワークを確認して、再度お試しください。\n\n" +
                        $"【詳細】{ex.Message}", "接続エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
            // ----------------------------------------------------
            // ★集計処理
            // ----------------------------------------------------
            var displayData = DoSummary(result.slprResult, result.stockDtNow, result.stockDtOld, result.selDatas, result.selAggregte);

            // ----------------------------------------------------
            // ★表示用フォーム起動
            // ----------------------------------------------------
            if (displayData != null)
            {
                // Form2_DataView をモードレスで開く場合（閉じてもForm2が残る）
                DataEtlViewFm view = new DataEtlViewFm();
                view.DisplayData = displayData;
                view.SelectedConditions = result.conList ?? new Dictionary<string, List<string>>();
                view.Show();
            }
            else
            {
                MessageBox.Show("該当データがありませんでした。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //----------------------------------------------------
            // ★アニメーションフォーム閉じる
            //----------------------------------------------------
            CloseAnimation(anim, animThread);

            await Task.Delay(300);
            if (anim != null && !anim.IsDisposed)
                anim.Invoke(new Action(() => anim.CloseForm()));

            // アニメーションスレッド終了を待つ
            animThread.Join();
        }

        /// <summary>
        /// Excelエクスポートボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnExportExcel_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btnExportExcel_Click");

            Processor processor = new Processor();
            Hiz = DateTime.Now.ToString("yyyyMMdd");
            Tim = DateTime.Now.ToString("HHmmss");

            // ----------------------------------------------------
            // ★ エラーチェック
            // ----------------------------------------------------
            if (ErrCheck())
                return;

            // ----------------------------------------------------
            // ★アニメーションフォーム表示
            // ----------------------------------------------------
            StartEndAnimationThread(AnimationPattern.開く);

            //----------------------------------------------------
            // ★メインデータ作成処理(メインスレッドでデータ抽出)
            //----------------------------------------------------
            var (slprResult, stockDtNow, stockDtOld, selDatas, selAggregte, selBookName, conList) = MakeMainData();

            //----------------------------------------------------
            // ★集計処理
            //----------------------------------------------------
            var ExcelData = DoSummary(slprResult, stockDtNow, stockDtOld, selDatas, selAggregte);

            //----------------------------------------------------
            // ★データ有無チェック
            //----------------------------------------------------
            if (ExcelData == null || ExcelData.Rows.Count == 0)
            {
                MessageBox.Show("エクスポートするデータがありません。");
                return;
            }

            //----------------------------------------------------
            // ★アニメーションフォーム閉じる
            //----------------------------------------------------
            StartEndAnimationThread(AnimationPattern.閉じる);

            //----------------------------------------------------
            // ★Excelエクスポート処理
            // ----------------------------------------------------
            OutputExcel(ExcelData, selBookName);

            // ログ追加             
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} Excelエクスポート 1 {CMD.UserName}");
            // ↑↑----------- Excelエクスポート -----------↑↑

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
            // Form1 のインスタンスを取得して表示
            // 名前で探すと見つからない場合があるため、型で検索して取得する
            var form1 = Application.OpenForms.OfType<TopMenuFm>().FirstOrDefault();
            if (form1 != null)
            {
                form1.Show();
            }
            // Form2 を閉じる
            this.Close();
        }

        // フィールドに追加
        private Point mouseOffset;
        private bool isMouseDown = false;

        /// <summary>
        /// マウスダウンでForm2を掴む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // MouseDown
        private void RplForm2_MouseDown(object sender, MouseEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} RplForm2_MouseDown");

            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }

        /// <summary>
        /// マウスドラッグでForm2を動かす
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // MouseMove
        private void RplForm2_MouseMove(object sender, MouseEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} RplForm2_MouseMove");

            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                this.Location = mousePos;
            }
        }

        /// <summary>
        /// マウスアップでForm2を離す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // MouseUp
        private void RplForm2_MouseUp(object sender, MouseEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} RplForm2_MouseUp");

            if (e.Button == MouseButtons.Left)
                isMouseDown = false;
        }

        /// <summary>
        /// 最小化ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btnMin_Click");

            this.WindowState = FormWindowState.Minimized;
        }

        //=================================================================
        // 処理メソッド
        //=================================================================
        /// <summary>
        /// 部門選択から取得した会社チェックを更新
        /// </summary>
        /// <param name="bumons"></param>
        private void UpdateCompanyCheckboxesFromBumon(List<Department> bumons)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} UpdateCompanyCheckboxesFromBumon");

            // 部門に基づいて会社のチェックボックスを更新
            chkBxOhno.Checked = bumons.Any(d => d.Company == "オーノ");
            chkBxSuncar.Checked = bumons.Any(d => d.Company == "サンミックカーペット");
            chkBxSundus.Checked = bumons.Any(d => d.Company == "サンミックダスコン");
        }

        /// <summary>
        /// 取引先選択から取得した部門と会社を選択
        /// </summary>
        /// <param name="torihikis"></param>
        private void UpdateBumonAndCompanyFromTorihiki(List<Torihiki> torihikis)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} UpdateBumonAndCompanyFromTorihiki");

            // 取引先が空なら会社チェック・部門リストもクリア
            if (torihikis == null || torihikis.Count == 0)
            {
                chkBxOhno.Checked = false;
                chkBxSuncar.Checked = false;
                chkBxSundus.Checked = false;

                selBumons.Clear();
                listBx部門.Items.Clear();
                return;
            }

            // 取引先に基づいて会社のチェックボックスを更新
            chkBxOhno.Checked |= torihikis.Any(t => t.Company == "オーノ");
            chkBxSuncar.Checked |= torihikis.Any(t => t.Company == "サンミックカーペット");
            chkBxSundus.Checked |= torihikis.Any(t => t.Company == "サンミックダスコン");
            // 取引先に基づいて部門リストを更新
            var newBumons = torihikis
                .Select(t => new Department { Code = t.DeptCode, Name = t.DeptName, Company = t.Company })
                .ToList();

            foreach (var nb in newBumons)
            {
                if (!selBumons.Any(b => b.Code == nb.Code && b.Company == nb.Company))
                    selBumons.Add(nb);
            }
            // listBox 表示更新（コード+名称）
            RefreshListBx(listBx部門, selBumons.Select(b => new Torihiki
            {
                Code = b.Code,
                Name = b.Name,
                Company = b.Company,
                DeptCode = b.Code,
                DeptName = b.Name
            }).ToList());
        }

        /// <summary>
        /// 取引先選択から取得した部門を追加
        /// </summary>
        /// <param name="deptmap"></param>
        /// <param name="ts"></param>
        private void AddDeptMap(Dictionary<string, HashSet<string>> deptmap, Torihiki ts)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} AddDeptMap");

            if (!deptmap.TryGetValue(ts.Code,out var set))
            {
                set = new HashSet<string>();
                deptmap[ts.Code] = set;
            }
            set.Add(ts.DeptCode);
        }

        /// <summary>
        /// 取引先選択から取得した部門を追加
        /// </summary>
        /// <param name="deptmap"></param>
        /// <param name="ts"></param>
        private void AddDeptMapRange(Dictionary<string, HashSet<string>> deptmap, IEnumerable<Torihiki> ts)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} AddDeptMapRange");

            foreach (var t in ts)
            {
                AddDeptMap(deptmap, t);
            }
        }

        /// <summary>
        /// 部門・販売・仕入の選択用リストボックス更新
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="items"></param>
        private void RefreshListBx(ListBox listBox, List<Torihiki> items)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} RefreshListBx");

            listBox.Items.Clear();

            // 選択アイテムがなければ終了
            if (items.Count == 0) return;

            foreach (var item in items)
            {
                // 同じ Code + Company の Torihiki がなければ追加
                bool exists = listBox.Items.Cast<Torihiki>()
                    .Any(t => t.Code == item.Code && t.Company == item.Company);

                if (!exists)
                    listBox.Items.Add(item);
            }
        }

        /// <summary>
        /// 表示＆Excelの選択条件エラーチェック
        /// </summary>
        /// <returns></returns>
        private bool ErrCheck()
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} ErrCheck");

            // =====================================================================================
            // ★ エラーチェック
            // =====================================================================================
            //  ＊年月入力チェック
            if (!fam.TryParseYearMonth(txtBxStrYearMonth, out int strY, out int strM) ||
                !fam.TryParseYearMonth(txtBxEndYearMonth, out int endY, out int endM))
            {
                MessageBox.Show("年月は6桁の数字(yyyyMM)で入力してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            //  ＊組織未選択NG
            if (!chkBxOhno.Checked && !chkBxSuncar.Checked && !chkBxSundus.Checked
                && listBx販売先.Items.Count == 0 && listBx仕入先.Items.Count == 0
                && listBx部門.Items.Count == 0)
            {
                MessageBox.Show("会社～取引先のいずれかを選択してください。",
                                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            //  ＊データ区分未選択NG
            if (!chkBxSl.Checked && !chkBxPr.Checked && !chkBxIv.Checked)
            {
                MessageBox.Show("データ区分を選択してください。",
                                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            string sym = txtBxStrYearMonth.Text.Trim();
            string eym = txtBxEndYearMonth.Text.Trim();
            string nowym = DateTime.Now.ToString("yyyyMM");
            //  ＊データ区分"在庫"選択の場合未来月NG
            if (chkBxIv.Checked)
            {
                if (string.Compare(sym, nowym) > 0 || string.Compare(eym, nowym) > 0)
                {
                    MessageBox.Show("データ区分在庫を選択する場合、未来月は指定できません。",
                                    "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }
            // データ区分"在庫"選択と、年月複数月(過去月の複数月はOK,過去月と当月はNG)の選択NG
            if (chkBxIv.Checked && sym != eym && (sym == nowym || eym == nowym))
            {
                MessageBox.Show("データ区分在庫を選択する場合、当月の場合は単月で指定してください。",
                                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 表示＆Excelのデータ抽出(明細)
        /// </summary>
        /// <returns></returns>
        private (
            DataTable slprResult, DataTable stockDtNow, DataTable stockDtOld, List<string> selDatas,
            string selAggregte, string selBookName, Dictionary<string, List<string>> conList) MakeMainData()
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} MakeMainData");

            // =====================================================================================
            // ★メインデータ作成処理
            // =====================================================================================
            // 条件選択内容取得
            fam.TryParseYearMonth(txtBxStrYearMonth, out int strY, out int strM);
            fam.TryParseYearMonth(txtBxEndYearMonth, out int endY, out int endM);

            string selBookName = fam.GetBookName(txtBx名称);  // 帳票名称
            (startDate, endDate) = fam.GetStartEndDate(strY, strM, endY, endM);　// 開始・終了日付
            var selCompanies = fam.GetCompany(chkBxOhno, chkBxSundus, chkBxSuncar); // 会社
            var selBumons = fam.GetSelectedBumons(listBx部門);  // 部門（先頭空白行は無視して取得）
            var selSelleres = fam.GetSallerOrSupplier(listBx販売先);  // 販売先 （先頭空白行は無視）
            var selSupplieres = fam.GetSallerOrSupplier(listBx仕入先);  // 仕入先 （先頭空白行は無視）
            var selDatas = fam.GetSalseProduct(chkBxSl, chkBxPr, chkBxIv);  // データ区分
            List<string> selSlPrProducts = null;
            List<string> selIvProducts = null;
            string selAggregte = null;
            Dictionary<string, string> selIvTypes = null;

            if (chkBxSl.Checked || chkBxPr.Checked)
            {
                selSlPrProducts = fam.GetProduct(chkBx原材料, chkBx半製品, chkBx製品,
                                                                chkBxOhno, chkBxSundus, chkBxSuncar);  // 売・仕クラス区分
                selAggregte = fam.GetAggregte(grpBx売仕集計区分);  // 売仕集計区分
            }
            if (chkBxIv.Checked)
            {
                selIvProducts = fam.GetProduct(chkBx原材料, chkBx半製品, chkBx製品,
                                                            chkBx加工T, chkBx預りT, chkBx預けT,
                                                            chkBxOhno, chkBxSundus, chkBxSuncar);  // 在クラス区分
                selIvTypes = fam.GetIvType(chkBx自社, chkBx預け, chkBx預り, chkBx投入);  // 在庫種別
                selAggregte = fam.GetAggregte(grpBx在集計区分);  // 在庫集計区分
            }


            // 条件フィルター
            // データ取得・加工処理
            var (slprResult, stockDtNow, stockDtOld) = fam.FilterData(startDate, endDate,
                                                                                   selCompanies, selBumons,
                                                                                   selSelleres, selSupplieres,
                                                                                   selDatas, selSlPrProducts, selIvProducts,
                                                                                   selIvTypes);
            // 選択条件のリスト
            // conListには選択されてる条件を格納→Form2_DataViewのListBxに表示
            Dictionary<string, List<string>> conList = new Dictionary<string, List<string>>();
            List<string> selectCondition = null;

            // 帳票名
            if (!string.IsNullOrWhiteSpace(selBookName))
            {
                selectCondition = selBookName.Split(',')
                    .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                conList.Add("帳票名称", selectCondition);
            }

            // strY+strM(yyyyMM形式)と endY+endM(yyyyMM形式)をList<string> YearMonthsに格納
            selectCondition = new List<string>
            {
                $"{strY}{strM:D2}", $"{endY}{endM:D2}"
            };
            conList.Add("年月", selectCondition);

            // 販売先・仕入先－部門－会社
            if (selSelleres.Count > 0 && selSelleres.Count > 0)
            {
                // 会社
                conList.Add("会社", selCompanies);
                // 部門
                conList.Add("部門", selBumons);
                // 販売先
                conList.Add("販売先", selSelleres);
                // 仕入先
                conList.Add("仕入先", selSupplieres);
            }
            else if (selSupplieres.Count == 0 && selSelleres.Count > 0)
            {
                // 会社
                conList.Add("会社", selCompanies);
                // 部門
                conList.Add("部門", selBumons);
                // 販売先
                conList.Add("販売先", selSelleres);
            }
            else if (selSelleres.Count == 0 && selSupplieres.Count > 0)
            {
                // 会社
                conList.Add("会社", selCompanies);
                // 部門
                conList.Add("部門", selBumons);
                // 仕入先
                conList.Add("仕入先", selSupplieres);
            }
            else if (selBumons.Count > 0)
            {
                // 会社
                conList.Add("会社", selCompanies);
                // 部門
                conList.Add("部門", selBumons);
            }
            else
            {
                // 会社
                conList.Add("会社", selCompanies);
            }

            // データ区分
            conList.Add("データ区分", selDatas);

            // 売仕クラス区分
            if (selSlPrProducts != null && selSlPrProducts.Count > 0) 
                conList.Add("売仕クラス区分", selSlPrProducts);

            // 在クラス区分
            if(selIvProducts != null && selIvProducts.Count > 0)
                conList.Add("在クラス区分", selIvProducts);

            // 在庫種別
            if(selIvTypes != null && selIvTypes.Count > 0)
            {
                selectCondition = selIvTypes.Values.ToList();
                conList.Add("在庫種別", selectCondition);
            }

            // 集計区分
            selectCondition = selAggregte.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            conList.Add("集計区分", selectCondition);

            return (slprResult, stockDtNow, stockDtOld, selDatas, selAggregte, selBookName, conList);
        }

        /// <summary>
        /// 表示＆Excelの集計
        /// </summary>
        /// <param name="slprResult"></param>
        /// <param name="stockDtNow"></param>
        /// <param name="stockDtOld"></param>
        /// <param name="selDatas"></param>
        /// <param name="selAggregte"></param>
        /// <returns></returns>
        private DataTable DoSummary(DataTable slprResult, DataTable stockDtNow, DataTable stockDtOld, List<string> selDatas, string selAggregte)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} DoSummary");

            // =====================================================================================
            // ★集計処理
            // =====================================================================================
            DataTable displayData = null;
            // --- 集計キー列を決定 ---
            List<string> groupKeys = new List<string>();
            string[] sortcols = null;
            string ptn = null;
            if (selDatas.Contains("在庫"))
            {

                if (stockDtNow != null)
                    displayData = stockDtNow;
                else if (stockDtOld != null)
                {
                    if (selAggregte == "NONE")
                        displayData = stockDtOld;
                    else
                    {
                        switch (selAggregte)
                        {
                            case "品目CD":
                                groupKeys = new List<string> { "年月", "クラス", "在庫種別", "部門CD", "品目CD" };
                                sortcols = new string[] { "年月", "部門CD", "クラス", "品目CD" };
                                ptn = "1";
                                break;
                                //case "倉庫CD":
                                //    groupKeys = new List<string> { "年月", "クラス", "在庫種別", "部門CD", "倉庫CD" };
                                //    sortcols = new string[] { "年月", "部門CD", "クラス", "倉庫CD" };
                                //    ptn = "2";
                                //    break;
                                //case "部門CD":
                                //    groupKeys = new List<string> { "年月", "クラス", "在庫種別", "部門CD" };
                                //    sortcols = new string[] { "年月", "部門CD", "クラス" };
                                //    ptn = "3";
                                //    break;
                        }
                        displayData = dsumm.SumData(stockDtOld, groupKeys, sortcols, "在庫", ptn);
                    }
                }
            }
            else
                if (slprResult != null)
            {
                if (selAggregte == "NONE")
                    displayData = slprResult;
                else
                {
                    switch (selAggregte)
                    {
                        case "品目CD":
                            groupKeys = new List<string> { "年月", "SbSys区分", "クラス", "部門CD", "品部門CD", "品名CD", "品種CD", "色CD" };
                            sortcols = new string[] { "年月", "SbSys区分", "部門CD", "クラス", "品部門CD", "品名CD", "品種CD", "色CD" };
                            ptn = "1";
                            break;
                        case "取引先CD":
                            groupKeys = new List<string> { "年月", "SbSys区分", "クラス", "部門CD", "取引先CD" };
                            sortcols = new string[] { "年月", "SbSys区分", "部門CD", "クラス", "取引先CD" };
                            ptn = "2";
                            break;
                        case "部門CD":
                            groupKeys = new List<string> { "年月", "SbSys区分", "クラス", "部門CD" };
                            sortcols = new string[] { "年月", "SbSys区分", "部門CD", "クラス" };
                            ptn = "3";
                            break;
                    }
                    displayData = dsumm.SumData(slprResult, groupKeys, sortcols, "売仕", ptn);
                }
            }
            return displayData;
        }

        /// <summary>
        /// Excelへエクスポート
        /// </summary>
        /// <param name="ExcelData"></param>
        /// <param name="selBookName"></param>
        private void OutputExcel(DataTable ExcelData, string selBookName)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} OutputExcel");

            //----------------------------------------------------
            // ★エクセルへのエクスポート処理
            //----------------------------------------------------
            if (string.IsNullOrEmpty(selBookName))
                selBookName = $"月次データ.{Hiz}.{Tim}.xlsm";
            else
                selBookName = $"{selBookName}.xlsm";

            try
            {
                // 保存ダイアログ
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "マクロ有効Excelファイル|*.xlsm";
                sfd.Title = "保存先を指定してください";
                sfd.FileName = selBookName;

                if (sfd.ShowDialog() != DialogResult.OK) return;

                string filePath = sfd.FileName;

                // Excelアプリケーション作成
                xlApp = new Excel.Application { Visible = false };

                // 新規ブック作成
                xlBook = xlApp.Workbooks.Add();

                //----------------------------------------------------
                // ★アニメーションフォーム表示
                //----------------------------------------------------
                StartEndAnimationThread(AnimationPattern.開く);

                //==================== Dataシート =====================
                xlSheet = (Excel.Worksheet)xlBook.Sheets[1];
                xlSheet.Name = "Data";

                // ヘッダー
                for (int c = 0; c < ExcelData.Columns.Count; c++)
                {
                    xlSheet.Cells[1, c + 1] = ExcelData.Columns[c].ColumnName;
                }

                // データ
                int rows = ExcelData.Rows.Count;
                int cols = ExcelData.Columns.Count;

                //==============================================================
                // 売仕
                //--------------------------------------------------------------
                // 集計[1]
                //  1:年月     2:SbSys区分 3:部門CD  4:クラス 5:取引先CD 6:取引先名
                //  7:品部門CD 8:品名CD    9:品種CD 10:色CD  11:品名    12:品種名
                // 13:色名    14:数量     15:単位CD 16:単価  17:金額
                //--------------------------------------------------------------
                // 集計[2]
                //  1:年月 2:SbSys区分 3:部門CD 4:クラス 5:取引先CD 6:取引先名
                //  7:数量 8:金額
                //--------------------------------------------------------------
                // 集計[3]
                //  1:年月 2:SbSys区分 3:部門CD 4:クラス 5:数量 6:金額
                //--------------------------------------------------------------
                // 集計なし
                //  1:伝票No  2:枝番    3:SbSys区分 4:取引区分  5:伝票日付
                //  6:部門CD  7:クラス  8:取引先CD  9:取引先名 10:品部門CD
                // 11:品名CD 12:品種CD 13:色CD     14:品名     15:品種名
                // 16:色名   17:数量   18:単位CD   19:単価     20:金額
                //==============================================================
                // 在庫
                //--------------------------------------------------------------
                // 集計[1]
                // 1:年月 2:部門CD 3:在庫種別 4:クラス 5:品名 6:品目CD 7:残数量 8:残金額
                //--------------------------------------------------------------
                // 集計なし
                //  1:年月    2:部門CD   3:在庫種別 4:クラス 5:倉庫CD
                //  6:倉庫名  7:預り先CD 8:預り先名 9:品名  10:品目CD
                // 11:残数量 12:残金額 
                //==============================================================
                //
                // 0埋めが必要な列と桁数（この4種類のみ対象）
                var zeroPadTargets = new Dictionary<string, int>
                {
                    { "伝票No",   8 },
                    { "取引先CD", 7 },
                    { "倉庫CD",   7 },
                    { "預り先CD", 7 }
                };
                // 全ての CD 列（文字列扱いにしたい列）
                string[] cdColNames = { "伝票No", "枝番", "部門CD", "取引先CD",
                                        "品部門CD", "品名CD", "品種CD", "色CD",
                                        "単位CD", "SbSys区分", "取引区分", "倉庫CD",
                                        "預り先CD", "在庫種別", "品目CD" };

                foreach (var cdColName in cdColNames)
                {
                    // DataTable に列が無ければスキップ
                    if (!ExcelData.Columns.Contains(cdColName)) continue;
                    // Excel 列番号（1-based）
                    int cdIndex = ExcelData.Columns.IndexOf(ExcelData.Columns[cdColName]) + 1; // Excel列番号

                    // CD列を文字列形式に設定
                    cdRange = xlSheet.Columns[cdIndex];
                    cdRange.NumberFormat = "@"; // Excelで文字列扱い


                    // CD列を個別に書き込む（ゼロ埋め）
                    for (int r = 0; r < rows; r++)
                    {
                        string cdValue = ExcelData.Rows[r][cdColName]?.ToString() ?? "";

                        if (zeroPadTargets.ContainsKey(cdColName))
                        {
                            int fixLen = zeroPadTargets[cdColName];

                            // 数値でも文字でもとにかくゼロ埋め
                            cdValue = cdValue.PadLeft(fixLen, '0');
                        }

                        // Excel のセルに書き込み
                        xlSheet.Cells[r + 2, cdIndex] = cdValue;
                    }
                }

                // CD列以外を配列に格納
                object[,] dataArray = new object[rows, cols - 1];
                int dataCol = 0;
                for (int c = 0; c < cols; c++)
                {
                    string colName = ExcelData.Columns[c].ColumnName;

                    // CD列はスキップ
                    if (cdColNames.Contains(colName)) continue;

                    for (int r = 0; r < rows; r++)
                    {
                        var val = ExcelData.Rows[r][c];

                        // 数量計・金額計は数値形式に変換
                        if ((colName == "数量計" || colName == "金額計") && val != null && val != DBNull.Value)
                        {
                            if (decimal.TryParse(val.ToString(), out decimal num))
                            {
                                dataArray[r, dataCol] = num.ToString("#,0");
                            }
                            else
                            {
                                dataArray[r, dataCol] = "0";
                            }
                        }
                        // 年月は文字列形式で
                        else if (colName == "年月")
                        {
                            string ymd = val?.ToString() ?? "";
                            dataArray[r, dataCol] = "'" + ymd;
                            //dataArray[r, dataCol] = ymd;
                        }
                        else
                        {
                            dataArray[r, dataCol] = val?.ToString() ?? "";
                        }
                    }
                    // Excel上で文字列扱いにしたい列は NumberFormat = "@"
                    if (colName == "年月")
                    {
                        int excelColIndex = c + 1; // Excel列番号
                        Excel.Range yearRange = xlSheet.Columns[excelColIndex];
                        yearRange.NumberFormat = "@"; // 文字列扱い
                        Marshal.ReleaseComObject(yearRange);
                    }
                    dataCol++;
                }

                // 一括代入（CD列以外）
                dataCol = 0;
                for (int c = 0; c < cols; c++)
                {
                    string colName = ExcelData.Columns[c].ColumnName;
                    if (cdColNames.Contains(colName)) continue;

                    Excel.Range colRange = xlSheet.Range[xlSheet.Cells[2, c + 1], xlSheet.Cells[rows + 1, c + 1]];

                    colRange.Value = GetColumnData(dataArray, dataCol, rows);
                    dataCol++;
                }

                // カンマ付き表示をExcel上でも設定（念のため）
                for (int c = 0; c < cols; c++)
                {
                    string colName = ExcelData.Columns[c].ColumnName;
                    if (colName == "数量計" || colName == "金額計")
                    {
                        Excel.Range col = xlSheet.Range[xlSheet.Cells[2, c + 1], xlSheet.Cells[rows + 1, c + 1]];
                        col.NumberFormat = "#,##0";
                        Marshal.ReleaseComObject(col);
                    }
                }
                // ------------------- ヘルパー関数 -------------------
                object[,] GetColumnData(object[,] source, int colIndex, int rowCount)
                {
                    object[,] colData = new object[rowCount, 1];
                    for (int r = 0; r < rowCount; r++)
                        colData[r, 0] = source[r, colIndex];
                    return colData;
                }

                // 列幅自動調整
                xlSheet.Columns.AutoFit();

                // 既存のフィルターをクリア
                if (xlSheet.AutoFilterMode) xlSheet.AutoFilterMode = false;

                // ヘッダー範囲を取得（1行目）
                Excel.Range headerRange = xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[1, cols]];
                // 太文字
                headerRange.Font.Bold = true;
                // 下線＋枠線（外枠＋内部の罫線）
                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                // 背景色（任意）
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                // フィルターを有効化
                headerRange.AutoFilter(1);
                // COMオブジェクト解放
                Marshal.ReleaseComObject(headerRange);

                //----------------------------------------------------
                // ★アニメーションフォーム閉じる
                //----------------------------------------------------
                StartEndAnimationThread(AnimationPattern.閉じる);

                // マクロ有効形式で保存
                xlBook.SaveAs(filePath, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
                xlBook.Close(false);
                xlApp.Quit();

                // 保存後に開くか確認
                var result = MessageBox.Show("Excelを保存しました。\n開きますか?", "保存完了", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                StartEndAnimationThread(AnimationPattern.閉じる);
                MessageBox.Show("Excelエクスポート中にエラーが発生しました: " + ex.Message);
            }

            finally
            {
                // COMオブジェクト解放
                if (cdRange != null) { Marshal.ReleaseComObject(cdRange); cdRange = null; }
                if (start2 != null) { Marshal.ReleaseComObject(start2); start2 = null; }
                if (end2 != null) { Marshal.ReleaseComObject(end2); end2 = null; }
                if (xlSheet != null) { Marshal.ReleaseComObject(xlSheet); xlSheet = null; }
                if (xlBook != null) { Marshal.ReleaseComObject(xlBook); xlBook = null; }
                if (xlApp != null) { Marshal.ReleaseComObject(xlApp); xlApp = null; }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        ///<summary>
        /// アニメーションを閉じる(FormAnimation2)
        ///</summary>
        ///
        private void CloseAnimation(WaitSnowMan anim, Thread animThread)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} 処理メソッド 1 {CMD.UserName} CloseAnimation");

            try
            {
                if (anim != null && !anim.IsDisposed)
                    anim.CloseForm();
            }
            catch { }

            try
            {
                if (animThread != null && animThread.IsAlive)
                    animThread.Join();
            }
            catch { }
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

        //==============================================================
        // デザイン関連メソッド
        //==============================================================
        /// <summary>
        /// Form2にスノーマンカラーを適用
        /// </summary>
        private void ApplySnowManColors()
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} デザイン関連メソッド 1 {CMD.UserName} ApplySnowManColors");

            // フォーム全体の背景
            this.BackColor = Color.FromArgb(255, 220, 150);

            //DataGridView[] grids = { dgvDataOhno, dgvDataSdus, dgvDataScar, dgvDataIV };
            //foreach (var dgv in grids)
            //{
            //    dgv.BackgroundColor = clrmg.RauDark1;
            //}

            // ラベル類
            lb名称.ForeColor = clrmg.MemeBase;

            // DataGridView に色を適用
            //StyleDataGrid(dgvDataOhno, clrmg.KojiDark1, Color.FromArgb(245, 240, 220), clrmg.KojiLight2);
            //StyleDataGrid(dgvDataSdus, Color.FromArgb(255, 153, 51), Color.FromArgb(245, 240, 220), clrmg.HikaruLight1);
            //StyleDataGrid(dgvDataScar, Color.FromArgb(255, 170, 50), Color.FromArgb(255, 240, 200), Color.FromArgb(255, 230, 180));
            //StyleDataGrid(dgvDataIV, Color.FromArgb(160, 120, 60), Color.FromArgb(245, 220, 170), Color.FromArgb(200, 160, 100));

            // チェックボックスなどは直接色指定でもOK
            foreach (var gb in cBoxList)
            {
                // グループボックスの背景
                gb.BackColor = Color.FromArgb(255, 220, 150);

                foreach (Control ctrl in gb.Controls)
                {
                    // チェックボックスの色
                    if (ctrl is CheckBox chk)
                    {
                        chk.ForeColor = clrmg.KojiDark1;
                        chk.BackColor = Color.FromArgb(255, 220, 150);
                    }
                    // ラジオボタンの色
                    else if (ctrl is RadioButton rdo)
                    {
                        rdo.ForeColor = Color.FromArgb(246, 83, 10);
                        rdo.BackColor = Color.FromArgb(255, 220, 150);
                    }
                }
            }

            // ボタンの色
            StyleButton(btnDisplay, clrmg.KojiBase, Color.White, borderColor: clrmg.KojiDark1);
            StyleButton(btnExportExcel, clrmg.KojiBase, Color.White, borderColor: clrmg.KojiDark1);
            StyleButton(btnForm1Back, clrmg.KojiLight1, Color.White, borderColor: clrmg.KojiDark1);
        }

        /// <summary>
        /// グループボックスのカスタム枠線描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBoxCustomBorder(object sender, PaintEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} デザイン関連メソッド 1 {CMD.UserName} GroupBoxCustomBorder");

            GroupBox box = (GroupBox)sender;
            e.Graphics.Clear(box.BackColor);

            // アンチエイリアス無効（線をくっきり）
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // テキストを測定
            SizeF textSize = e.Graphics.MeasureString(box.Text, box.Font);

            // 枠線色を赤茶色でで
            using (Pen pen = new Pen(Color.FromArgb(177, 65, 3), 1.5f))
            {
                int textPadding = 8;  // 左の余白
                int textWidth = (int)textSize.Width;

                // 枠線を描画（上の線だけタイトル部分を避ける）
                e.Graphics.DrawLine(pen, 1, (int)(textSize.Height / 2), textPadding - 2, (int)(textSize.Height / 2)); // 左上～文字前
                e.Graphics.DrawLine(pen, textPadding + textWidth + 2, (int)(textSize.Height / 2), box.Width - 2, (int)(textSize.Height / 2)); // 文字後～右上
                e.Graphics.DrawLine(pen, 1, (int)(textSize.Height / 2), 1, box.Height - 2); // 左線
                e.Graphics.DrawLine(pen, 1, box.Height - 2, box.Width - 2, box.Height - 2); // 下線
                e.Graphics.DrawLine(pen, box.Width - 2, (int)(textSize.Height / 2), box.Width - 2, box.Height - 2); // 右線

                // テキストを描画
                using (SolidBrush brush = new SolidBrush(clrmg.MemeDark1))
                {
                    e.Graphics.DrawString(box.Text, box.Font, brush, 8, 0);
                }
            }
        }
    }
}
