using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static あすよん月次帳票.Form1;

namespace あすよん月次帳票
{
    public partial class Form3 : Form
    {
        CommonData cm = new CommonData();
        FormActionMethod fam = new FormActionMethod();
        private string HIZTIM;

        private static List<string> runtimelog = new List<string>();

        private string TIM = DateTime.Now.ToString("HHmmss");
        private const int lockMinutes = 10; // ← ロック保持時間（10分）

        public Form3()
        {
            InitializeComponent();
            this.Load += Form3_Load;

            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));

            string mf = Path.Combine(cm.mfPath, $"Employee.csv");

            // Form3読込ログ
            fam.AddLog($"{HIZTIM} FormOpen 1 {cm.UserName} Form3");
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadRuntimeLog();

            this.MouseDown += Form3_MouseDown;
            this.MouseMove += Form3_MouseMove;
            this.MouseUp += Form3_MouseUp;

            ApplySnowManColors();
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
            string lockFilePath = Path.Combine(cm.LockPath, "LOCK_sim.txt");
            bool locked = false; // ← ロック取得済みフラグ

            //try
            //{
            // ロック確認と取得
            if (!fam.CheckAndLockSimulation(lockMinutes))
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

            await Task.Delay(100); // ちょっと待って anim が作られる

            // --- メインスレッドでシミュレーション実行 ---
            RunSimulation();

            // --- 終了したらアニメーション閉じる ---
            await Task.Delay(500);
            if (anim != null && !anim.IsDisposed)
            {
                anim.Invoke(new Action(() => anim.CloseForm()));
            }

            // アニメーションスレッド終了を待つ
            animThread.Join();
        }


        public void RunSimulation()
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";

            string monthlyFile = Path.Combine(cm.mfPath, "Monthly.txt");
            string firstLine = File.ReadLines(monthlyFile).FirstOrDefault();
            string sumirateYM = firstLine.Substring(0, 6); // 先頭6文字を取得(当月)
            FormActionMethod formActionMethod = new FormActionMethod();

            // 選択部門コード取得
            string selectedBumon = cmbxBumon.Invoke(new Func<string>(() =>
                cmbxBumon.SelectedItem != null ? cmbxBumon.SelectedItem.ToString().Split(':')[0] : null
            )) as string;

            // --- オーノ ---
            if (chkBxOhno.Checked)
            {
                if (string.IsNullOrEmpty(selectedBumon))
                {
                    // 部門未選択 → 全部門でシミュレーション
                    formActionMethod.SimulateIZAIKO_Ohno(cm.ohuid, cm.ohpass, sumirateYM);
                    AddLog("オーノ_シュミレーション実行");
                    fam.AddLog($"{HIZTIM} シュミレーション 0 {cm.UserName} オーノ全部門");
                    fam.AddLog2($"{HIZTIM} シュミレーション 0 【ユーザー:{cm.UserName}】 シュミレーション使用中です");
                }
                else
                {
                    // 部門1つ選択 → 部門指定シミュレーション
                    formActionMethod.SimulateIZAIKO_Ohno(cm.ohuid, cm.ohpass, sumirateYM, selectedBumon);
                    AddLog("オーノ({selectedBumon}) のシュミレーション実行");
                    fam.AddLog($"{HIZTIM} シュミレーション 0 {cm.UserName} 部門:{selectedBumon}");
                    fam.AddLog2($"{HIZTIM} シュミレーション 0 【ユーザー:{cm.UserName}】 シュミレーション使用中です");
                }
            }

            // --- サンミック(ダスコン) ---
            if (chkBxSundus.Checked)
            {
                formActionMethod.SimulateIZAIKO_Sun(cm.sundusuid, cm.sunduspass, sumirateYM, "SD");
                AddLog("サンミック(ダスコン)のシュミレーション実行");
                fam.AddLog($"{HIZTIM} シュミレーション 0 {cm.UserName} サンミック(ダスコン)");
                fam.AddLog2($"{HIZTIM} シュミレーション 0 【ユーザー:{cm.UserName}】 シュミレーション使用中です");
            }
            // --- サンミック(カーペット) ---
            if (chkBxSuncar.Checked)
            {
                formActionMethod.SimulateIZAIKO_Sun(cm.suncaruid, cm.suncarpass, sumirateYM, "SC");
                AddLog("サンミック(カーペット)のシュミレーション実行");
                fam.AddLog($"{HIZTIM} シュミレーション 0 {cm.UserName} サンミック(カーペット)");
                fam.AddLog2($"{HIZTIM} シュミレーション 0 【ユーザー:{cm.UserName}】 シュミレーション使用中です");
            }
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
            var selctedComp = fam.GetCompany(chkBxOhno, chkBxSundus, chkBxSuncar);

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
        /// Form3のログ削除
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

            //// TextBox の背景色や文字色
            //txtBxID.BackColor = Color.White;
            //txtBxPASS.BackColor = Color.White;
            //txtBxID.ForeColor = Color.Black;
            //txtBxPASS.ForeColor = Color.Black;

            StyleButton(btnSimulation, "saku", ColorManager.SakuBase, borderColor: ColorManager.SakuDark1, Color.White);
            StyleButton(btnForm1Back, "saku", ColorManager.SakuLight1, borderColor: ColorManager.SakuDark2, Color.White);
        }

        // フィールドに追加
        private Point mouseOffset;
        private bool isMouseDown = false;
        private FormAnimation1 animForm;

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