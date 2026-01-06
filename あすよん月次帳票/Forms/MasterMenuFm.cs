using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    public partial class MasterMenuFm : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        // フィールド変数
        private string HIZTIM;
    
        public MasterMenuFm()
        {
            InitializeComponent();
        }

        //=========================================================
        // 【コントロール実行メソッド】
        //=========================================================
        /// <summary>
        /// 取引先ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AAA(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} lnkLbMaster_LinkClicked");
            try
            {
                WaitSnowMan anim = null;

                // --- FormAnimation スレッド ---
                Thread animThread = new Thread(() =>
                {
                    using (WaitSnowMan a = new WaitSnowMan())
                    {

                        anim = a; // 外部参照用

                        a.Shown += (s, i) =>
                        {
                            a.Invoke((Action)(() =>
                            {
                                anim.lblMessage.Text = "あすよん月次帳票専用\r\nマスタ更新中です…\r\n";
                                anim.BackColor = clrmg.FukaLight1;
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
                    var hanbai = fam.GetHanbaiAll(lib);
                    var hanbaiFile = $@"{CMD.mfPath}\{lib.Replace("SM1", "")}HANBAI.json";
                    fam.SaveToJson(hanbaiFile, hanbai);


                    // 仕入先マスタ取得
                    var shiire = fam.GetShiireAll(lib);
                    var shiireFile = $@"{CMD.mfPath}\{lib.Replace("SM1", "")}SHIIRE.json";
                    fam.SaveToJson(shiireFile, shiire);
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
                fam.AddLog($"{HIZTIM} マスタ更新 0 {CMD.UserName}");
                fam.AddLog2($"{HIZTIM} マスタ更新 0 {CMD.UserName} マスタ更新が完了しました。");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 部門マスタボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn部門マスタ_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} lnkLbStandard_LinkClicked");
            // 部門マスタFormを作成
            var form = new 部門マスタForm();
            // 部門マスタFormを表示
            form.Show();
        }
        /// <summary>
        /// 戻るボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn戻る_Click(object sender, EventArgs e)
        {
            // Form1 のインスタンスを取得して表示
            if (Application.OpenForms["Form1"] is TopMenuFm form1)
            {
                form1.Show();
            }
            // 部門マスタForm を閉じる
            this.Close();
        }

        private void btn仕入先マスタ_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btn仕入先マスタ_Click");
            // 仕入先マスタFormを作成
            var form = new 取引先マスタFm();
            // 仕入先マスタFormを表示
            form.Show();
        }
    }
}
