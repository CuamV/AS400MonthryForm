using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = System.Action;
using Application = System.Windows.Forms.Application;
using Label = System.Windows.Forms.Label;

namespace あすよん月次帳票
{
    public partial class Form1 : Form
    {
        private string HIZTIM;
        private string HIZ = DateTime.Now.ToString("yyyyMMdd");
        private string TIM = DateTime.Now.ToString("HHmmss");
        private Label lbSituation;

        private string mfPath = @"\\ohnosv01\OhnoSys\099_sys\mf";
        private readonly string logPath =
            Path.Combine(Application.UserAppDataPath, "log.txt");
        private const string LockFilePath = @"\\ohnosv01\OhnoSys\099_sys\Lock";
        private const string LogFilePath = @"\\ohnosv01\OhnoSys\099_sys\LOG";

        public Form1()
        {
            InitializeComponent();


            JsonLoader.LoadBumon(mfPath + @"\BUMON.json");
            JsonLoader.LoadHanbai("オーノ", Path.Combine(mfPath, "DLB01HANBAI.json"));
            JsonLoader.LoadShiire("オーノ", Path.Combine(mfPath, "DLB01SHIIRE.json"));
            JsonLoader.LoadHanbai("サンミックダスコン", Path.Combine(mfPath, "DLB02HANBAI.json"));
            JsonLoader.LoadShiire("サンミックダスコン", Path.Combine(mfPath, "DLB02SHIIRE.json"));
            JsonLoader.LoadHanbai("サンミックカーペット", Path.Combine(mfPath, "DLB03HANBAI.json"));
            JsonLoader.LoadShiire("サンミックカーペット", Path.Combine(mfPath, "DLB03SHIIRE.json"));

            grpBxMenu.Paint += GroupBoxCustomBorder;

            LoadLogs();

            // フェードインイベント追加
            this.Shown += Form1_Shown;
        }

        ///<summary>
        ///ログを追加する専用メソッド
        ///</summary>
        public void AddLog(string message)
        {
            // Form1のリストに追加
            listBxSituation.Items.Add($"{message}");
            // ファイルに保存
            File.AppendAllText(logPath, message + Environment.NewLine);
        }

        public void AddLog2(string message)
        {
            string logFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_AllSimulation.txt");
            // Form1のリストに追加
            listBxSituation.Items.Add($"{message}");
            // ファイルに保存
            File.AppendAllText(logFilePath, message + Environment.NewLine);
        }

        // 過去3日間のログ読込
        private void LoadLogs()
        {
            listBxSituation.Items.Clear();

            string AllLogFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_AllSimulation.txt");
            // --- 個人ログの読込 ---
            if (!File.Exists(logPath)) return;

            var lines = File.ReadAllLines(logPath);
            DateTime threshold = DateTime.Now.AddDays(-3);

            foreach (var line in lines)
            {
                // ログの日付部分をバース
                if (DateTime.TryParse(line.Substring(0, 10), out DateTime logDate))
                {
                    if (logDate > threshold)
                    {
                        listBxSituation.Items.Add(line);
                    }
                }
            }
            // --- 全体ログの読込 ---
            if (!File.Exists(AllLogFilePath)) return;
            var allLines = File.ReadAllLines(AllLogFilePath);
            foreach (var line in allLines)
            {
                // ログの日付部分をバース
                if (DateTime.TryParse(line.Substring(0, 10), out DateTime logDate))
                {
                    if (logDate > threshold)
                    {
                        listBxSituation.Items.Add(line);
                    }
                }
            }

            var sorted = listBxSituation.Items.Cast<string>()
                .OrderByDescending(line =>
                {
                    if (DateTime.TryParse(line.Substring(line.IndexOf('2'), 16), out var dt)) return dt;
                    return DateTime.MinValue;
                })
                .ToList();

            listBxSituation.Items.Clear();
            foreach (var s in sorted)
                listBxSituation.Items.Add(s);

            // listBxSituation.Itemsが0件の場合、ログファイルをリネームして空ファイル再作成
            if (listBxSituation.Items.Count == 0)
            {
                string backupLogPath = logPath.Replace("log.txt", $"log_backup_{HIZ}_{TIM}.txt");
                File.Move(logPath, backupLogPath);
                File.Create(logPath).Close();
            }
        }

        /// <summary>
        /// フォームが開いたときに呼ばれる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // 
        private void Form1_Load(object sender, EventArgs e)
        {
            ApplySnowManColors();

            // ロック状況確認タイマー起動
            timrReleaseLock.Tick += timerReleaseLock_Tick;
            timrReleaseLock.Start();

            // ログ更新タイマー起動
            timrLogRenewal.Tick += timrLogRenewal_Tick;
            timrLogRenewal.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form3.ClearRuntimeLog();
        }

        // マスター更新ボタンクリック
        private async void lnkLbMaster_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                FormAnimation2 anim = null;

                // --- FormAnimation スレッド ---
                Thread animThread = new Thread(() =>
                {
                    using (FormAnimation2 a = new FormAnimation2())
                    {

                        anim = a; // 外部参照用

                        a.Shown += (s, i) =>
                        {
                            a.Invoke((Action)(() =>
                            {
                                anim.lblMessage.Text = "あすよん月次帳票専用\r\nマスタ更新中です…\r\n";
                                anim.BackColor = ColorManager.FukaLight1;
                            }));
                        };
                        Application.Run(a); // GIF表示
                    }
                });
                animThread.SetApartmentState(ApartmentState.STA);
                animThread.Start();

                // --- メインスレッドでマスタ更新実行 ---
                await Task.Delay(100); // ちょっと待って anim が作られる

                // ライブラリごとに処理
                var libs = new[] { "SM1DLB01", "SM1DLB02", "SM1DLB03" };

                foreach (var lib in libs)
                {
                    // 販売先マスタ取得
                    var hanbai = FormActionMethod.GetHanbaiAll(lib);
                    var hanbaiFile = $@"{mfPath}\{lib.Replace("SM1", "")}HANBAI.json";
                    FormActionMethod.SaveToJson(hanbaiFile, hanbai);


                    // 仕入先マスタ取得
                    var shiire = FormActionMethod.GetShiireAll(lib);
                    var shiireFile = $@"{mfPath}\{lib.Replace("SM1", "")}SHIIRE.json";
                    FormActionMethod.SaveToJson(shiireFile, shiire);
                }

                // --- 終了したらアニメーション閉じる ---
                await Task.Delay(500);
                if (anim != null && !anim.IsDisposed)
                {
                    anim.Invoke(new Action(() => anim.CloseForm()));
                }

                // アニメーションスレッド終了を待つ
                animThread.Join();

                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                string currentUserID = Properties.Settings.Default.UserID;

                MessageBox.Show("マスタ作成が完了しました！", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBxSituation.Items.Add($"{DateTime.Now:yyyy/MM/dd　HH:mm:ss}　マスタ更新実行");
                AddLog2($"{HIZTIM} 実行者ID:{currentUserID} がマスタ更新実行しました");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// シュミレーションリンク
        private void lnkLbSimulate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form3を作成
            var form3 = new Form3();
            // Form3を表示
            form3.Show();
            // Form1を非表示
            this.Hide();
        }

        /// <summary>
        /// データ表示ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // 
        private void lnkLbDisplay_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form2を作成
            var form2 = new RplForm2();
            // Form2を表示
            form2.Show();
            // Form1を非表示
            this.Hide();
        }

        private void lnkLbStandard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("現在開発中です。", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ApplySnowManColors()
        {
            // フォーム全体の背景色
            this.BackColor = ColorManager.RauLight1;  // 背景黒
            // ラベルの色
            lbMenu.ForeColor = ColorManager.MemeBase;  // 白文字
            lbSituation.ForeColor = ColorManager.MemeBase;  // 文字白
            // リストボックス
            listBxSituation.BackColor = ColorManager.HikaruLight1; // ひーくん黄色
            listBxSituation.ForeColor = Color.Black;
            // グループボックスの枠線
            grpBxMenu.ForeColor = ColorManager.ShopyBase;

            // リンクラベルの色
            lnkLbSimulate.LinkColor = Color.FromArgb(255, 102, 255);  // さっくんピンク
            lnkLbDisplay.LinkColor = ColorManager.KojiBase;    // 康二オレンジ
            lnkLbStandard.LinkColor = ColorManager.AbeBase;   // 阿部ちゃん緑
            lnkLbMaster.LinkColor = ColorManager.FukaLight1;    // ふっか紫

            StyleButton(btnEnd, ColorManager.RauBase, ColorManager.DateBase, ColorManager.DateLight1);
            // マウスイベント追加
            btnEnd.MouseEnter += Btn_MouseEnter;
            btnEnd.MouseLeave += Btn_MouseLeave;
            btnEnd.MouseDown += Btn_MouseDown;
            btnEnd.MouseUp += Btn_MouseUp;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            string lockFilePath = Path.Combine(LockFilePath, "LOCK_sim.txt");
            string currentUserID = Properties.Settings.Default.UserID;

            if (File.Exists(lockFilePath))
            {
                // ロックファイルが存在する場合、ロックを解除+削除
                var flg = FormActionMethod.ReleaseSimulationLock(currentUserID, lockFilePath, LogFilePath);
                if (flg)
                    File.Delete(lockFilePath);
            }
            Application.Exit();
        }
        private void timrLogRenewal_Tick(object sender, EventArgs e)
        {
            LoadLogs();
        }

        // タイマーの起動
        private void timerReleaseLock_Tick(object sender, EventArgs e)
        {
            string lockFilePath = Path.Combine(LockFilePath, "LOCK_sim.txt");
            string currentUserID = Properties.Settings.Default.UserID;

            if (File.Exists(lockFilePath))
            {
                var lines = File.ReadAllLines(lockFilePath);
                string lockUser = lines.FirstOrDefault(l => l.StartsWith("UserID="))?.Split('=')[1];
                string timeLine = lines.FirstOrDefault(l => l.StartsWith("Time="))?.Split('=')[1];

                // "Time=" の行に書かれている時刻を取得
                if (!DateTime.TryParse(timeLine, out DateTime lockTime))
                    lockTime = DateTime.Now;
                // ロックファイル作成（または更新）から10分経過しているか判定
                bool isExpired = (DateTime.Now - lockTime) >= TimeSpan.FromMinutes(10);

                // 最終起動から10分経過していたらロック解除
                if (isExpired)
                {
                    var flg = FormActionMethod.ReleaseSimulationLock(currentUserID, lockFilePath, LogFilePath);
                    if (flg)
                        File.Delete(lockFilePath);
                }
            }
        }

        private void GroupBoxCustomBorder(object sender, PaintEventArgs e)
        {
            GroupBox box = (GroupBox)sender;
            e.Graphics.Clear(box.BackColor);

            // アンチエイリアス無効（線をくっきり）
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // テキストを測定
            SizeF textSize = e.Graphics.MeasureString(box.Text, box.Font);

            // 枠線色
            using (Pen pen = new Pen(ColorManager.ShopyBase, 1.5f))
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
                using (SolidBrush brush = new SolidBrush(ColorManager.MemeDark1))
                {
                    e.Graphics.DrawString(box.Text, box.Font, brush, 8, 0);
                }
            }
        }
    }
}
