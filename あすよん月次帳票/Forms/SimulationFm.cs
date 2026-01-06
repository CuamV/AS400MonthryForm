using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Action = System.Action;
using Application = System.Windows.Forms.Application;
using CheckBox = System.Windows.Forms.CheckBox;
using GroupBox = System.Windows.Forms.GroupBox;
using Point = System.Drawing.Point;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    //==========================================================
    // --------Form3(シュミレーション)クラス--------
    //==========================================================
    internal partial class SimulationFm : Form
    {
        //==========================================================
        // インスタンス
        //==========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        private Point mouseOffset;
        private bool isMouseDown = false;
        private WaitSimulationFm animForm;
        private string HIZTIM;
        private const int lockMinutes = 10; // ← ロック保持時間（10分）

        private static List<string> runtimelog = new List<string>();

        //=========================================================
        // コンストラクタ
        //=========================================================
        internal SimulationFm()
        {
            InitializeComponent();

            this.Load += Form3_Load;

            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));

            string mf = Path.Combine(CMD.mfPath, $"Employee.csv");

            // Form3読込ログ
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} FormOpen 1 {CMD.UserName} Form3");
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        internal void Form3_Load(object sender, EventArgs e)
        {
            LoadRuntimeLog();

            this.MouseDown += Form3_MouseDown;
            this.MouseMove += Form3_MouseMove;
            this.MouseUp += Form3_MouseUp;

            ApplySnowManColors();
        }

        //=========================================================
        // 【コントロール実行メソッド】
        //=========================================================
        /// <summary>
        /// シュミレーションボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSimulation_Click(object sender, EventArgs e)
        {
            string lockFilePath = Path.Combine(CMD.LockPath, "LOCK_sim.txt");
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

            WaitSimulationFm anim = null;

            // --- FormAnimation スレッド ---
            Thread animThread = new Thread(() =>
            {
                using (WaitSimulationFm a = new WaitSimulationFm())
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

        /// <summary>
        /// 戻るボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForm1Back_Click(object sender, EventArgs e)
        {
            // Form1 のインスタンスを取得して表示
            if (Application.OpenForms["Form1"] is TopMenuFm form1)
            {
                form1.Show();
            }
            // Form3 を閉じる
            this.Close();
        }

        /// <summary>
        /// 会社選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// マウスダウンでForm3を掴む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }

        /// <summary>
        /// マウスドラッグでForm3を動かす
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// マウスアップでForm3を離す
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // MouseUp
        private void Form3_MouseUp(object sender, MouseEventArgs e)
        {
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
            this.WindowState = FormWindowState.Minimized;
        }

        //=================================================================
        // 処理メソッド
        //=================================================================
        /// <summary>
        /// Form3のListBoxとメモリ上にログ追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void AddLog(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} {message}";
            runtimelog.Add(logMessage);
            listBxSituation.Items.Add(logMessage);
        }

        /// <summary>
        /// Form3を表示するたびに既存ログもListBoxに表示
        /// </summary>
        internal void LoadRuntimeLog()
        {
            listBxSituation.Items.Clear();
            foreach (var log in runtimelog)
            {
                listBxSituation.Items.Add(log);
            }
        }

        /// <summary>
        /// Form3のログ削除
        /// </summary>
        internal static void ClearRuntimeLog()
        {
            runtimelog.Clear();
        }

        /// <summary>
        /// シュミレーション処理
        /// </summary>
        private void RunSimulation()
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";

            string monthlyFile = Path.Combine(CMD.mfPath, "Monthly.txt");
            string firstLine = File.ReadLines(monthlyFile).FirstOrDefault();
            string sumirateYM = firstLine.Substring(0, 6); // 先頭6文字を取得(当月)

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
                    fam.SimulateIZAIKO_Ohno(CMD.ohuid, CMD.ohpass, sumirateYM);
                    AddLog("オーノ_シュミレーション実行");
                    fam.AddLog($"{HIZTIM} シュミレーション 0 {CMD.UserName} RunSimulation オーノ全部門");
                    fam.AddLog2($"{HIZTIM} シュミレーション 0 ユーザー:{CMD.UserName} RunSimulation シュミレーション使用中です");
                }
                else
                {
                    // 部門1つ選択 → 部門指定シミュレーション
                    fam.SimulateIZAIKO_Ohno(CMD.ohuid, CMD.ohpass, sumirateYM, selectedBumon);
                    AddLog("オーノ({selectedBumon}) のシュミレーション実行");
                    fam.AddLog($"{HIZTIM} シュミレーション 0 {CMD.UserName} RunSimulation 部門:{selectedBumon}");
                    fam.AddLog2($"{HIZTIM} シュミレーション 0 ユーザー:{CMD.UserName} RunSimulation シュミレーション使用中です");
                }
            }

            // --- サンミック(ダスコン) ---
            if (chkBxSundus.Checked)
            {
                fam.SimulateIZAIKO_Sun(CMD.sundusuid, CMD.sunduspass, sumirateYM, "SD");
                AddLog("サンミック(ダスコン)のシュミレーション実行");
                fam.AddLog($"{HIZTIM} シュミレーション 0 {CMD.UserName} RunSimulation サンミック(ダスコン)");
                fam.AddLog2($"{HIZTIM} シュミレーション 0 ユーザー:{CMD.UserName} RunSimulation シュミレーション使用中です");
            }
            // --- サンミック(カーペット) ---
            if (chkBxSuncar.Checked)
            {
                fam.SimulateIZAIKO_Sun(CMD.suncaruid, CMD.suncarpass, sumirateYM, "SC");
                AddLog("サンミック(カーペット)のシュミレーション実行");
                fam.AddLog($"{HIZTIM} シュミレーション 0 {CMD.UserName} RunSimulation サンミック(カーペット)");
                fam.AddLog2($"{HIZTIM} シュミレーション 0 ユーザー:{CMD.UserName} RunSimulation シュミレーション使用中です");
            }
        }

        //==============================================================
        // デザイン関連メソッド
        //==============================================================
        /// <summary>
        /// Form3にスノーマンカラーを適用
        /// </summary>
        private void ApplySnowManColors()
        {
            // フォーム全体の背景
            this.BackColor = clrmg.SakuLight2;

            // ボタンに色を適用
            StyleButton(btnSimulation, clrmg.SakuBase, Color.White);
            StyleButton(btnForm1Back, clrmg.SakuLight1, Color.White);

            // ラベル類
            lbBumon.ForeColor = clrmg.MemeBase;
            lbCompany.ForeColor = clrmg.MemeBase;
            lbSituation.ForeColor = clrmg.MemeBase;

            // ListBox やチェックボックスの背景と文字色
            listBxSituation.BackColor = clrmg.HikaruLight2;
            listBxSituation.ForeColor = clrmg.MemeDark1;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is CheckBox chk)
                {
                    chk.BackColor = clrmg.SakuLight2;
                    chk.ForeColor = clrmg.SakuDark2;
                }
            }

            // GroupBox 背景と囲い線
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is GroupBox gb)
                {
                    gb.BackColor = clrmg.SakuLight2;
                    gb.ForeColor = clrmg.SakuDark1; // 囲い線やラベルに濃いピンク
                }
            }

            //// TextBox の背景色や文字色
            //txtBxID.BackColor = Color.White;
            //txtBxPASS.BackColor = Color.White;
            //txtBxID.ForeColor = Color.Black;
            //txtBxPASS.ForeColor = Color.Black;

            StyleButton(btnSimulation, "saku", clrmg.SakuBase, borderColor: clrmg.SakuDark1, Color.White);
            StyleButton(btnForm1Back, "saku", clrmg.SakuLight1, borderColor: clrmg.SakuDark2, Color.White);
        }

        

        
        

        

    }
}