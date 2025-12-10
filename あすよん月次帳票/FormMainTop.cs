using System;
using System.IO;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    internal partial class FormMainTop : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormActionMethod fam = new FormActionMethod();

        // フィールド変数
        private string HIZTIM;

        //=========================================================
        // コンストラクタ
        //=========================================================
        internal FormMainTop()
        {
            InitializeComponent();

            this.Load += FormMainTop_Load;
        }

        private void FormMainTop_Load(object sender, EventArgs e)
        {
            txtBxID.Text = Properties.Settings.Default.UserID;
            txtBxPASS.Text = Properties.Settings.Default.Password;
        }

        /// <summary>
        /// Form1ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void btnStart_Clicked(object sender, EventArgs e)
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

            // 設定保存
            Properties.Settings.Default.UserID = idText;
            Properties.Settings.Default.Password = passText;
            Properties.Settings.Default.Save();

            // 共通クラスにセット
            CMD.HIZ = DateTime.Now.ToString("yyyyMMdd");
            CMD.mfPath = @"\\ohnosv01\OhnoSys\099_sys\mf";  // マスターファイルパス
            CMD.LockPath = @"\\ohnosv01\OhnoSys\099_sys\Lock"; // ロックファイルパス
            CMD.LogPath = $@"\\ohnosv01\OhnoSys\099_sys\LOG\{CMD.HIZ}"; // ログファイルパス
            CMD.UserID = idText;
            CMD.UserID = idText;
            CMD.Pass = passText;
            string mf = Path.Combine(CMD.mfPath, "Employee.csv");
            CMD.UserName = 
                fam.GetUserName(idText, mf);
            if (string.IsNullOrEmpty(CMD.UserName))
            {
                CMD.UserName = "ゲストユーザー";
                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} ユーザー名 1 {CMD.UserName} ユーザー名取得不可");
            }
            CMD.uLog = Path.Combine(CMD.LogPath, CMD.HIZ, $"LOG.{CMD.UserID}.txt");
            CMD.conLog = Path.Combine(CMD.LogPath, CMD.HIZ, "LOG_ControlAction.txt");
            CMD.sLog = Path.Combine(CMD.LogPath, CMD.HIZ, "LOG_Simulation.txt");
            CMD.ohuid = "X" + CMD.UserID;
            CMD.ohpass = "X" + CMD.Pass;
            CMD.suncaruid = "A" + CMD.UserID;
            CMD.suncarpass = "A" + CMD.Pass;
            CMD.sundusuid = "S" + CMD.UserID;
            CMD.sunduspass = "S" + CMD.Pass;

            // ログイン情報をForm1に渡す
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} ログイン 0 {CMD.UserName}");
            fam.AddLog($"{HIZTIM} ログイン 1 【ユーザーID:{CMD.UserID}/ユーザーPass:{CMD.Pass}】");

            // Form1を作成
            Form1 form1New = new Form1();
            // Form1を表示
            form1New.Show();

            // FormMainTopを非表示
            this.Hide();
        }
    }
}
