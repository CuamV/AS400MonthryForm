using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    internal partial class LoginFm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        private string HIZTIM;

        //=========================================================
        // コンストラクタ
        //=========================================================
        internal LoginFm()
        {
            InitializeComponent();
            
            CMD.sjis = Encoding.GetEncoding("Shift_JIS");
            CMD.utf8 = Encoding.GetEncoding("UTF-8");

            this.Load += FormMainTop_Load;
        }

        private void FormMainTop_Load(object sender, EventArgs e)
        {
            txtBxID.Text = Properties.Settings.Default.UserID;
            txtBxPASS.Text = Properties.Settings.Default.Password;
        }

        //=========================================================
        // コントロール実行メソッド
        //=========================================================
        /// <summary>
        /// Form1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal async void btnStart_Clicked(object sender, EventArgs e)
        {
            string idText = txtBxID.Text.Trim();
            string passText = txtBxPASS.Text.Trim();

            if (string.IsNullOrEmpty(idText) || string.IsNullOrEmpty(passText))
            {
                MessageBox.Show(
                "IDとPasswordの入力がありません。", "確認",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                            anim.lblMessage.Text = "ログイン中…\r\n";
                            anim.BackColor = clrmg.ShopyLight1;
                        }));
                    };
                    Application.Run(a); // GIF表示
                }
            });
            animThread.SetApartmentState(ApartmentState.STA);
            animThread.Start();

            await Task.Delay(100); // ちょっと待って anim が作られる

            // 設定保存
            Properties.Settings.Default.UserID = idText;
            Properties.Settings.Default.Password = passText;
            Properties.Settings.Default.Save();

            // 共通クラスにセット
            CMD.HIZ = DateTime.Now.ToString("yyyyMMdd");
            CMD.mfPath = @"\\ohnosv01\OhnoSys\099_sys\mf";  // マスターファイルパス
            CMD.mfBkPath = @"\\ohnosv01\OhnoSys\099_sys\mf.BK"; // マスターファイルバックアップパス
            CMD.LockPath = @"\\ohnosv01\OhnoSys\099_sys\Lock"; // ロックファイルパス
            CMD.LogPath = $@"\\ohnosv01\OhnoSys\099_sys\LOG\{CMD.HIZ}"; // ログファイルパス
            CMD.UserID = idText;
            CMD.UserID = idText;
            CMD.Pass = passText;
            string mf = Path.Combine(CMD.mfPath, "Usermf.csv");
            CMD.UserName = 
                fam.GetUserName(idText, mf);
            if (string.IsNullOrEmpty(CMD.UserName))
            {
                CMD.UserName = "ゲストユーザー";
                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} ユーザー名 1 {CMD.UserName} ユーザー名取得不可");
            }
            CMD.uLog = Path.Combine(CMD.LogPath, $"LOG.{CMD.UserID}.txt");
            CMD.conLog = Path.Combine(CMD.LogPath, "LOG_ControlAction.txt");
            CMD.sLog = Path.Combine(CMD.LogPath, "LOG_Simulation.txt");
            CMD.ohuid = "X" + CMD.UserID;
            CMD.ohpass = "X" + CMD.Pass;
            CMD.suncaruid = "A" + CMD.UserID;
            CMD.suncarpass = "A" + CMD.Pass;
            CMD.sundusuid = "S" + CMD.UserID;
            CMD.sunduspass = "S" + CMD.Pass;

            // ログイン情報をForm1に渡す
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} ログイン 0 {CMD.UserName}");
            fam.AddLog($"{HIZTIM} ログイン 1 ユーザーID:{CMD.UserID}/ユーザーPass:{CMD.Pass} btnStart_Clicked");

            // Form1を作成
            TopMenuFm form1New = new TopMenuFm();
            // Form1を表示
            form1New.Show();

            //----------------------------------------------------
            // ★アニメーションフォーム閉じる
            //----------------------------------------------------
            CloseAnimation(anim, animThread);

            await Task.Delay(300);
            if (anim != null && !anim.IsDisposed)
                anim.Invoke(new Action(() => anim.CloseForm()));

            // アニメーションスレッド終了を待つ
            animThread.Join();

            // FormMainTopを非表示
            this.Hide();
        }

        //=========================================================
        // 処理メソッド
        //=========================================================
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
    }
}
