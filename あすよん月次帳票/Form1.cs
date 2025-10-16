using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using Label = System.Windows.Forms.Label;

namespace あすよん月次帳票
{
    public partial class Form1 : Form
    {
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
            JsonLoader.LoadHanbai("オーノ",Path.Combine(mfPath ,"DLB01HANBAI.json"));
            JsonLoader.LoadShiire("オーノ", Path.Combine(mfPath, "DLB01SHIIRE.json"));
            JsonLoader.LoadHanbai("サンミックダスコン", Path.Combine(mfPath, "DLB02HANBAI.json"));
            JsonLoader.LoadShiire("サンミックダスコン", Path.Combine(mfPath, "DLB02SHIIRE.json"));
            JsonLoader.LoadHanbai("サンミックカーペット", Path.Combine(mfPath, "DLB03HANBAI.json"));
            JsonLoader.LoadShiire("サンミックカーペット", Path.Combine(mfPath, "DLB03SHIIRE.json"));

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

        // 過去3日間のログ読込
        private void LoadLogs()
        {
            if (!File.Exists(logPath)) return;

            var lines = File.ReadAllLines(logPath);
            DateTime threshold = DateTime.Now.AddDays(-3);

            foreach (var line in lines)
            {
                // ログの日付部分をバース
                if(DateTime.TryParse(line.Substring(0,16),out DateTime logDate))
                {
                    if (logDate > threshold)
                    {
                        listBxSituation.Items.Add(line);
                    }
                }
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
                    var hanbai = FormActionMethod.GetHanbai10Year(lib);
                    var hanbaiFile = $@"{mfPath}\{lib.Replace("SM1", "")}HANBAI.json";
                    FormActionMethod.SaveToJson(hanbaiFile, hanbai);


                    // 仕入先マスタ取得
                    var shiire = FormActionMethod.GetShiire10Year(lib);
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

                MessageBox.Show("マスタ作成が完了しました！", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBxSituation.Items.Add($"{DateTime.Now:yyyy/MM/dd　HH:mm:ss}　マスタ更新実行");
            }
            catch(Exception ex) {
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
            var form2 = new Form2();
            // Form2を表示
            form2.Show();
            // Form1を非表示
            this.Hide();
        }

        // Excelエクスポートリンク
        private void lnkLbExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form4を作成
            var form4 = new Form4();
            // Form4を表示
            form4.Show();
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
            this.BackColor = ColorManager.MemeDark1;  // 背景黒
            // ラベルの色
            lbMenu.ForeColor = ColorManager.RauBase;  // 白文字
            lbSituation.ForeColor = ColorManager.RauBase;  // 文字白
            // リストボックス
            listBxSituation.BackColor = ColorManager.HikaruLight1; // ひーくん黄色
            listBxSituation.ForeColor = Color.Black;
            // リンクラベルの色
            lnkLbSimulate.LinkColor = Color.FromArgb(255,102,255);  // さっくんピンク
            lnkLbExport.LinkColor = ColorManager.ShopyBase;    // しょっぴー青
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
            string logFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_Simulation.txt");
            string currentUserID = Properties.Settings.Default.UserID;

            if (File.Exists(lockFilePath))
            {
                // ロックファイルが存在する場合、ロックを解除+削除
                var flg = FormActionMethod.ReleaseSimulationLock(currentUserID, lockFilePath, logFilePath);
                if(flg)
                File.Delete(lockFilePath);
            }
            Application.Exit();
        }
    }
}
