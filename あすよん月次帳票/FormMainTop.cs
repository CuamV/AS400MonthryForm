using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class FormMainTop : Form
    {
        CommonData cm = new CommonData();
        FormActionMethod fam = new FormActionMethod();

        private string HIZTIM;
        public FormMainTop()
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
        private void btnStart_Clicked(object sender, EventArgs e)
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
            cm.mfPath = @"\\ohnosv01\OhnoSys\099_sys\mf";  // マスターファイルパス
            cm.LockPath = @"\\ohnosv01\OhnoSys\099_sys\Lock"; // ロックファイルパス
            cm.LogPath = @"\\ohnosv01\OhnoSys\099_sys\LOG"; // ログファイルパス
            cm.UserID = idText;
            cm.Pass = passText;
            string mf = Path.Combine(cm.mfPath, "Employee.csv");
            cm.UserName = 
                fam.GetUserName(idText, mf);
            if (string.IsNullOrEmpty(cm.UserName))
            {
                cm.UserName = "ゲストユーザー";
                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                fam.AddLog($"{HIZTIM} ユーザー名 1 {cm.UserName} ユーザー名取得不可");
            }
            cm.HIZ = DateTime.Now.ToString("yyyyMMdd");
            cm.uLog = Path.Combine(cm.LogPath, cm.HIZ, $"LOG.{cm.UserID}.txt");
            cm.conLog = Path.Combine(cm.LogPath, cm.HIZ, "LOG_ControlAction.txt");
            cm.sLog = Path.Combine(cm.LogPath, cm.HIZ, "LOG_Simulation");
            cm.ohuid = "X" + cm.UserID;
            cm.ohpass = "X" + cm.Pass;
            cm.suncaruid = "A" + cm.UserID;
            cm.suncarpass = "A" + cm.Pass;
            cm.sundusuid = "S" + cm.UserID;
            cm.sunduspass = "S" + cm.Pass;

            // ログイン情報をForm1に渡す
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} ログイン 0 {cm.UserName}");
            fam.AddLog($"{HIZTIM} ログイン 1 【ユーザーID:{cm.UserID}/ユーザーPass:{cm.Pass}】");

            // Form1を作成
            Form1 form1New = new Form1();
            // Form1を表示
            form1New.Show();

            // FormMainTopを非表示
            this.Hide();
        }
    }
}
