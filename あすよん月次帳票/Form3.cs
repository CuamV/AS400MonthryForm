using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static あすよん月次帳票.Form1;

namespace あすよん月次帳票
{
    public partial class Form3 : Form
    {
        private string HIZTIM;

        string ohuid;
        string ohpass;
        string suncaruid;
        string suncarpass;
        string sundusuid;
        string sunduspass;

        private static List<string> runtimelog = new List<string>();

        private string HIZ = DateTime.Now.ToString("yyyyMMdd");
        private string TIM = DateTime.Now.ToString("HHmmss");
        private const string LockFilePath = @"\\ohnosv01\OhnoSys\099_sys\Lock";
        private const string LogFilePath = @"\\ohnosv01\OhnoSys\099_sys\LOG";
        private const int lockMinutes = 10; // ← ロック保持時間（10分）

        public Form3()
        {
            InitializeComponent();
            this.Load += Form3_Load;

            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));
            
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private void Form3_Load(object sender, EventArgs e)
        {
            txtBxID.Text = Properties.Settings.Default.UserID;
            txtBxPASS.Text = Properties.Settings.Default.Password;

            LoadRuntimeLog();

            this.MouseDown += Form3_MouseDown;
            this.MouseMove += Form3_MouseMove;
            this.MouseUp += Form3_MouseUp;

            ApplySnowManColors();
            // LOG直下にHIZフォルダがなければ作成
            string logFilePath = Path.Combine(LogFilePath, $@"{HIZ}\sim_log.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
        }

        /// <summary>
        /// Form3のListBoxとメモリ上にログ追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddLog(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} {message}";
            runtimelog.Add(logMessage);
            listBxSituation.Items.Add(logMessage);
        }

        /// <summary>
        /// Form3を表示するたびに既存ログもListBoxに表示
        /// </summary>
        public void LoadRuntimeLog()
        {
            listBxSituation.Items.Clear();
            foreach (var log in runtimelog)
            {
                listBxSituation.Items.Add(log);
            }
        }

        private async void btnSimulation_Click(object sender, EventArgs e)
        {

            string currentUID = txtBxID.Text.Trim();
            string idText = txtBxID.Text.Trim();
            string passText = txtBxPASS.Text.Trim();

            string lockFilePath = Path.Combine(LockFilePath, "LOCK_sim.txt");

            bool locked = false; // ← ロック取得済みフラグ

            //try
            //{
                // ロック確認と取得
                if (!FormActionMethod.CheckAndLockSimulation(currentUID, lockFilePath, LogFilePath, lockMinutes))
                {
                    return; // 他のユーザーが実行中なら処理を中止
                }

                locked = true; // ← ここでロック成功を記録

                // 確認ダイアログは UI スレッドで表示
                var result = MessageBox.Show(
                    "AS/400のセッションウインドウを全て閉じてから実行してください。\n\n実行しますか？",
                    "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Cancel)
                    return;

                if (string.IsNullOrEmpty(idText) || string.IsNullOrEmpty(passText))
                {
                    MessageBox.Show(
                    "IDとPasswordの入力がありません。", "確認",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!chkBxOhno.Checked && !chkBxSundus.Checked && !chkBxSuncar.Checked)
                {
                    MessageBox.Show(
                    "会社の選択を行って下さい。", "確認",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FormAnimation1 anim = null;

                // --- FormAnimation スレッド ---
                Thread animThread = new Thread(() =>
                {
                    using (FormAnimation1 a = new FormAnimation1())
                    {
                        anim = a; // 外部参照用
                        Application.Run(a); // GIF表示
                    }
                });
                animThread.SetApartmentState(ApartmentState.STA);
                animThread.Start();

                // --- メインスレッドでシミュレーション実行 ---
                await Task.Delay(100); // ちょっと待って anim が作られる
                RunSimulation(idText, passText, currentUID);

                // --- 終了したらアニメーション閉じる ---
                await Task.Delay(500);
                if (anim != null && !anim.IsDisposed)
                {
                    anim.Invoke(new Action(() => anim.CloseForm()));
                }

                // アニメーションスレッド終了を待つ
                animThread.Join();

                //// アニメーション表示は別スレッドで実行
                //FormAnimation anim = new FormAnimation();
                //Thread animThread = new Thread(() =>
                //{
                    
                //        Application.Run(anim); // GIF専用スレッド
                //});
                //animThread.SetApartmentState(ApartmentState.STA);
                //animThread.Start();

                //RunSimulation(idText, passText);

                //// 終了したらアニメーション閉じる
                //if (animThread.IsAlive)
                //    anim.Invoke(new Action(() => anim.CloseForm()));
            //}
            //finally
            //{
            //    // ロック解除
            //    if (locked)
            //    {
            //        FormActionMethod.ReleaseSimulationLock(currentUID, lockFilePath, logFilePath);
            //    }
            //}
        }
        

        public void RunSimulation(string idText, string passText, string currentUID)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";

            ohuid = "X" + idText;
            ohpass = "X" + passText;
            suncaruid = "A" + idText;
            suncarpass = "A" + passText;
            sundusuid = "S" + idText;
            sunduspass = "S" + passText;

            string sumirateYM = DateTime.Now.ToString("yyyyMM");
            FormActionMethod formActionMethod = new FormActionMethod();

            // 選択部門コード取得
            string selectedBumon = cmbxBumon.Invoke(new Func<string>(() =>
                cmbxBumon.SelectedItem != null ? cmbxBumon.SelectedItem.ToString().Split(':')[0] : null
            )) as string;

            int progress = 0;

            // --- オーノ ---
            if (chkBxOhno.Checked)
            {
                if (string.IsNullOrEmpty(selectedBumon))
                {
                    // 部門未選択 → 全部門でシミュレーション
                    formActionMethod.SimulateIZAIKO_Ohno(ohuid, ohpass, sumirateYM);
                    formActionMethod.AddLog("オーノ_シュミレーション実行", listBxSituation);
                    if (Application.OpenForms["Form1"] is Form1 form1)
                    {
                        form1.AddLog($"{HIZTIM} オーノ_シュミレーション実行 実行者ID:{currentUID}");
                        form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} が使用中です");
                    }
                }
                else
                {
                    // 部門1つ選択 → 部門指定シミュレーション
                    formActionMethod.SimulateIZAIKO_Ohno(ohuid, ohpass, sumirateYM, selectedBumon);
                    formActionMethod.AddLog($"オーノ({selectedBumon}) のシュミレーション実行", listBxSituation);
                    if (Application.OpenForms["Form1"] is Form1 form1)
                    {
                        form1.AddLog($"{HIZTIM} オーノ({selectedBumon})_シュミレーション実行 実行者ID:{currentUID}");
                        form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} が使用中です");
                    }
                }
            }

            // --- サンミック(ダスコン) ---
            if (chkBxSundus.Checked)
            {
                formActionMethod.SimulateIZAIKO_Sun(sundusuid, sunduspass, sumirateYM, "SD");
                formActionMethod.AddLog("サンミック(ダスコン)のシュミレーション実行", listBxSituation);
                if (Application.OpenForms["Form1"] is Form1 form1)
                {
                    form1.AddLog($"{HIZTIM} サンミック(ダスコン)_シュミレーション実行 実行者ID:{currentUID}");
                    form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} が使用中です");
                }

            }
            // --- サンミック(カーペット) ---
            if (chkBxSuncar.Checked)
            {
                formActionMethod.SimulateIZAIKO_Sun(suncaruid, suncarpass, sumirateYM, "SC");
                formActionMethod.AddLog("サンミック(カーペット)のシュミレーション実行", listBxSituation);
                if (Application.OpenForms["Form1"] is Form1 form1)
                {
                    form1.AddLog($"{HIZTIM} サンミック(カーペット)_シュミレーション実行 実行者ID:{currentUID}");
                    form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} が使用中です");
                }

            }
                // 設定保存
                Properties.Settings.Default.UserID = idText;
                Properties.Settings.Default.Password = passText;
                Properties.Settings.Default.Save();
        }


        private void btnForm1Back_Click(object sender, EventArgs e)
        {
            // Form1 のインスタンスを取得して表示
            if (Application.OpenForms["Form1"] is Form1 form1)
            {
                form1.Show();
            }
            // Form3 を閉じる
            this.Close();
        }

        private void Company_CheckedChanged(object sender, EventArgs e)
        {
            cmbxBumon.Items.Clear();

            // 会社選択確認
            var selctedComp = FormActionMethod.GetCompany(chkBxOhno, chkBxSundus, chkBxSuncar);

            // 空のアイテムを追加(部門未選択用)
            cmbxBumon.Items.Add("");

            // 選択された会社の部門をchkLBxBumonに追加
            foreach (var bumon in JsonLoader.GetBUMONs(selctedComp.ToArray()))
            {
                cmbxBumon.Items.Add($"{bumon.Code}:{bumon.Name}");
            }

            cmbxBumon.SelectedIndex = 0;
        }

        /// <summary>
        /// アプリ終了時に呼ぶとForm3のログも消える
        /// </summary>
        public static void ClearRuntimeLog()
        {
            runtimelog.Clear();
        }
        private void ApplySnowManColors()
        {
            // フォーム全体の背景
            this.BackColor = ColorManager.SakuLight2;

            // ボタンに色を適用
            StyleButton(btnSimulation, ColorManager.SakuBase, Color.White);
            StyleButton(btnForm1Back, ColorManager.SakuLight1, Color.White);

            // ラベル類
            lbBumon.ForeColor = ColorManager.MemeBase;
            lbCompany.ForeColor = ColorManager.MemeBase;
            IbID.ForeColor = ColorManager.MemeBase;
            lbPASS.ForeColor = ColorManager.MemeBase;
            lbSituation.ForeColor = ColorManager.MemeBase;

            // ListBox やチェックボックスの背景と文字色
            listBxSituation.BackColor = ColorManager.HikaruLight2;
            listBxSituation.ForeColor = ColorManager.MemeDark1;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is CheckBox chk)
                {
                    chk.BackColor = ColorManager.SakuLight2;
                    chk.ForeColor = ColorManager.SakuDark2;
                }
            }

            // GroupBox 背景と囲い線
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is GroupBox gb)
                {
                    gb.BackColor = ColorManager.SakuLight2;
                    gb.ForeColor = ColorManager.SakuDark1; // 囲い線やラベルに濃いピンク
                }
            }

            // TextBox の背景色や文字色
            txtBxID.BackColor = Color.White;
            txtBxPASS.BackColor = Color.White;
            txtBxID.ForeColor = Color.Black;
            txtBxPASS.ForeColor = Color.Black;

            StyleButton(btnSimulation, "saku", ColorManager.SakuBase, borderColor: ColorManager.SakuDark1, Color.White);
            StyleButton(btnForm1Back, "saku", ColorManager.SakuLight1, borderColor: ColorManager.SakuDark2, Color.White);
        }

        // フィールドに追加
        private Point mouseOffset;
        private bool isMouseDown = false;

        // MouseDown
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }

        // MouseMove
        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                this.Location = mousePos;
            }
        }

        // MouseUp
        private void Form3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isMouseDown = false;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
