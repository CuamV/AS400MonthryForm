using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class FormMainTop : Form
    {
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
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";

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
            CommonData.mfPath = @"\\ohnosv01\OhnoSys\099_sys\mf";  // マスターファイルパス
            CommonData.LockPath = @"\\ohnosv01\OhnoSys\099_sys\Lock"; // ロックファイルパス
            CommonData.LogPath = @"\\ohnosv01\OhnoSys\099_sys\LOG"; // ログファイルパス
            CommonData.UserID = idText;
            CommonData.Pass = passText;
            string mf = Path.Combine(CommonData.mfPath, $"Employee.csv");
            CommonData.UserName = FormActionMethod.GetUserName(idText, mf);
            CommonData.HIZ = DateTime.Now.ToString("yyyyMM");
            CommonData.uLog = Path.Combine(CommonData.LogPath, CommonData.HIZ, $"LOG.{CommonData.UserID}.txt");
            CommonData.conLog = Path.Combine(CommonData.LogPath, CommonData.HIZ, $"LOG_ControlAction.txt");

            // Form1を作成
            Form1 form1 = new Form1();
            // Form1を表示
            form1.Show();
            // ログイン情報をForm1に渡す
            if (Application.OpenForms["Form1"] is Form1 f1)
            {
                f1.AddLog($"{HIZTIM} ログイン 0 【ユーザー:{CommonData.UserName}】");
                f1.AddLog($"{HIZTIM} ログイン 1 【ユーザーID:{CommonData.UserID}/ユーザーPass:{CommonData.Pass}】");
            }
            // FormMainTopを非表示
            this.Hide();
        }
    }
}
