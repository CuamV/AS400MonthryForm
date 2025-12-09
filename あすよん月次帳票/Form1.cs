using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = System.Action;
using Application = System.Windows.Forms.Application;
using Label = System.Windows.Forms.Label;

namespace あすよん月次帳票
{
    //==========================================================
    // --------Form1(トップフォーム)クラス--------
    //==========================================================
    public partial class Form1 : Form
    {
        //=========================================================
        // フィールド変数
        //=========================================================
        private string HIZTIM;
        private Label lb操作履歴;

        //=========================================================
        // コンストラクタ
        //=========================================================
        public Form1()
        {
            InitializeComponent();

            JsonLoader.LoadBumon(CommonData.mfPath + @"\BUMON.json");
            JsonLoader.LoadHanbai("オーノ", Path.Combine(CommonData.mfPath, "DLB01HANBAI.json"));
            JsonLoader.LoadShiire("オーノ", Path.Combine(CommonData.mfPath, "DLB01SHIIRE.json"));
            JsonLoader.LoadHanbai("サンミックダスコン", Path.Combine(CommonData.mfPath, "DLB02HANBAI.json"));
            JsonLoader.LoadShiire("サンミックダスコン", Path.Combine(CommonData.mfPath, "DLB02SHIIRE.json"));
            JsonLoader.LoadHanbai("サンミックカーペット", Path.Combine(CommonData.mfPath, "DLB03HANBAI.json"));
            JsonLoader.LoadShiire("サンミックカーペット", Path.Combine(CommonData.mfPath, "DLB03SHIIRE.json"));

            grpBxメニュー.Paint += GroupBoxCustomBorder;

            if (!Directory.Exists(Path.Combine(CommonData.LogPath, CommonData.HIZ)))
                Directory.CreateDirectory(Path.Combine(CommonData.LogPath, CommonData.HIZ));
            // 個人用ログファイルパス
            if (!File.Exists(CommonData.uLog))
                File.Create(CommonData.uLog).Close();
            // 全体用ログファイルパス
            if (!File.Exists(CommonData.conLog))
                File.Create(CommonData.conLog).Close();

            // Form1読込ログ
            AddLog($"{HIZTIM} FormOpen 1 {CommonData.UserName} Form1");

            // フェードインイベント追加
            this.Shown += Form1_Shown;
            
            // ログ読込　　　　　
            LoadLogs();
        }

        /// <summary>
        /// Form1(トップフォーム)読込時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// マスタ更新リンククリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    var hanbaiFile = $@"{CommonData.mfPath}\{lib.Replace("SM1", "")}HANBAI.json";
                    FormActionMethod.SaveToJson(hanbaiFile, hanbai);


                    // 仕入先マスタ取得
                    var shiire = FormActionMethod.GetShiireAll(lib);
                    var shiireFile = $@"{CommonData.mfPath}\{lib.Replace("SM1", "")}SHIIRE.json";
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

                MessageBox.Show("マスタ作成が完了しました！", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddLog($"{HIZTIM} マスタ更新 0 {CommonData.UserName}");
                AddLog2($"{HIZTIM} マスタ更新 0 {CommonData.UserName} マスタ更新が完了しました。");
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// シュミレーションリンククリック(Form3)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// データ抽出リンククリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkLbDisplay_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form2を作成
            var form2 = new Form2();
            // Form2を表示
            form2.Show();
            // Form1を非表示
            this.Hide();
        }

        /// <summary>
        /// 定型帳票リンククリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkLbStandard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form5を作成
            var form5 = new Form5();
            // Form5を表示
            form5.Show();
            // Form1を非表示
            this.Hide();
        }

        /// <summary>
        /// アプリ終了ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            string lockFilePath = Path.Combine(CommonData.LockPath, "LOCK_sim.txt");
            string currentUserID = Properties.Settings.Default.UserID;

            if (File.Exists(lockFilePath))
            {
                // ロックファイルが存在する場合、ロックを解除+削除
                var flg = FormActionMethod.ReleaseSimulationLock(currentUserID, lockFilePath, CommonData.LogPath);
                if (flg)
                    File.Delete(lockFilePath);
            }
            Form1_FormClosing(sender, new FormClosingEventArgs(CloseReason.UserClosing, false));
            Application.Exit();

            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            AddLog($"{HIZTIM} ログアウト 1 【ユーザー:{CommonData.UserName}】");
        }

        /// <summary>
        /// シュミレーションロック解除タイマー(1分)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerReleaseLock_Tick(object sender, EventArgs e)
        {
            string lockFilePath = Path.Combine(CommonData.LockPath, "LOCK_sim.txt");
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
                    var flg = FormActionMethod.ReleaseSimulationLock(currentUserID, lockFilePath, CommonData.LogPath);
                    if (flg)
                        File.Delete(lockFilePath);
                }
            }
        }

        /// <summary>
        /// ログ表示更新タイマー(30秒)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timrLogRenewal_Tick(object sender, EventArgs e)
        {
            LoadLogs();
        }

        //=================================================================
        // 処理メソッド
        //=================================================================
        ///<summary>
        /// 個人用ログを追加&ログファイル保存
        /// </summary>
        /// <param name="message"></param>
        public void AddLog(string message)
        {
            //  ログファイルに保存
            File.AppendAllText(CommonData.uLog, message + Environment.NewLine);
        }

        ///<summary>
        /// 全体用ログを追加&ログファイル保存
        /// </summary>
        /// <param name="message"></param>
        public void AddLog2(string message)
        {
            // ログファイルに保存
            File.AppendAllText(CommonData.conLog, message + Environment.NewLine);
        }

        /// <summary>
        /// ログファイル読込&過去3日分表示
        /// </summary>
        private void LoadLogs()
        {
            if (!File.Exists(CommonData.uLog)) return;
            if (!File.Exists(CommonData.conLog)) return;

            List<string> dispList = new List<string>(); // 表示用リスト
            string display;

            // ログ(uLog,conLog)レイアウト [スペース区切り]
            // 1:年月日 2:時分秒 3:処理ステータス 4:表示フラグ 5:ユーザー 6:備考
            //=======================================================================
            // --- 個人ログの読込 ---
            var lines = File.ReadLines(CommonData.uLog, Encoding.UTF8);

            foreach (var line in lines)
            {
                // 空行スキップ
                if (string.IsNullOrWhiteSpace(line)) continue;
                // スペース区切りで分割(項目を配列に)
                var parts = line.Split(' ');
                // 項目数不足スキップ
                if (parts.Length < 5) continue;

                if (parts[3] == "0")
                {
                    // 必要項目のみ再構築(1,2,3,5(,6))
                    if (parts.Length < 6)
                        display = $"{parts[0]} {parts[1]} 【{parts[2]}:{parts[4]}】";
                    else
                        display = $"{parts[0]} {parts[1]} 【{parts[2]}:{parts[4]}】 <{parts[5]}>";

                    dispList.Add(display);
                }

            }
            // --- 全体ログの読込 ---
            var allLines = File.ReadLines(CommonData.conLog,Encoding.UTF8);

            foreach (var line in allLines)
            {
                // 空行スキップ
                if (string.IsNullOrWhiteSpace(line)) continue;
                // スペース区切りで分割(項目を配列に)
                var parts = line.Split(' ');
                // 項目不足スキップ
                if (parts.Length < 5) continue;

                if (parts[3] == "0")
                {
                    // 必要項目のみ再構築(1,2,3,5(,6))
                    if (parts.Length < 6)
                        display = $"{parts[0]} {parts[1]} 【{parts[2]}:{parts[4]}】";
                    else
                        display = $"{parts[0]} {parts[1]} 【{parts[2]}:{parts[4]}】 <{parts[5]}>";

                    dispList.Add(display);
                }
            }
            // --- ソート(年月日＋時分秒で降順) ---
            var sorted = dispList.OrderByDescending(line =>
                {
                    // 例: "2025/02/03 12:34:56 【OK:USER】 <備考>"
                    if (DateTime.TryParse(line.Substring(0, 19), out var dt)) return dt;
                    return DateTime.MinValue;
                })
                .ToList();

            listBxログ表示.Items.Clear();
            foreach (var s in sorted)
                listBxログ表示.Items.Add(s);
        }

        /// <summary>
        /// Form3のログ削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form3.ClearRuntimeLog();
        }
        
        //=================================================================
        // デザイン関連メソッド
        //=================================================================
        /// <summary>
        /// Form1にスノーマンの色を適用
        /// </summary>
        public void ApplySnowManColors()
        {
            // フォーム全体の背景色
            this.BackColor = ColorManager.RauLight1;  // 背景黒
            // ラベルの色
            lbメニュー.ForeColor = ColorManager.MemeBase;  // 白文字
            lb操作履歴.ForeColor = ColorManager.MemeBase;  // 文字白
            // リストボックス
            listBxログ表示.BackColor = ColorManager.HikaruLight1; // ひーくん黄色
            listBxログ表示.ForeColor = Color.Black;
            // グループボックスの枠線
            grpBxメニュー.ForeColor = ColorManager.ShopyBase;

            // リンクラベルの色
            lnkLbシュミレーション.LinkColor = Color.FromArgb(255, 102, 255);  // さっくんピンク
            lnkLbデータ抽出.LinkColor = ColorManager.KojiBase;    // 康二オレンジ
            lnkLb定型帳票.LinkColor = ColorManager.AbeBase;   // 阿部ちゃん緑
            lnkLbマスタ更新.LinkColor = ColorManager.FukaLight1;    // ふっか紫

            StyleButton(btn終了, ColorManager.RauBase, ColorManager.DateBase, ColorManager.DateLight1);
            // マウスイベント追加
            btn終了.MouseEnter += Btn_MouseEnter;
            btn終了.MouseLeave += Btn_MouseLeave;
            btn終了.MouseDown += Btn_MouseDown;
            btn終了.MouseUp += Btn_MouseUp;
        }
        /// <summary>
        /// グループボックスのカスタム枠線描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
