using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        private List<GroupBox> cBoxList;

        public MasterMenuFm()
        {
            InitializeComponent();

            cBoxList = new List<GroupBox>
            {
                grpBx組織,
                grpBx効率化,
                grpBx権限,
            };

            // RplForm2の全グループボックスを配列化して共通のPaintイベントを設定
            foreach (var gb in cBoxList)
                gb.Paint += GroupBoxCustomBorder;

            this.Load += MasterMenuFm_Load;
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public void MasterMenuFm_Load(object sender, EventArgs e)
        {
            // フォームにスノーマンカラーを適用
            ApplySnowManColors();
        }
        //=========================================================
        // 【コントロール実行メソッド】
        //=========================================================


        /// <summary>
        /// 戻るボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForm1Back_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btnForm1Back_Click");
            // Form1 のインスタンスを取得して表示
            // 名前で探すと見つからない場合があるため、型で検索して取得する
            var form1 = Application.OpenForms.OfType<TopMenuFm>().FirstOrDefault();
            if (form1 != null)
            {
                form1.Show();   
            }
            // マスタメニューForm を閉じる
            this.Close();
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

        private void btn取引先マスタ_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btn取引先マスタ_Click");
            // 取引先マスタFormを作成
            var form = new 取引先マスタFm();
            // 取引先マスタFormを表示
            form.Show();
        }

        private void btn郵便番号辞書_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btn郵便番号辞書_Click");
            // 郵便番号辞書インポートFormを作成
            var form = new 郵便番号辞書インポートFm();
            // 郵便番号辞書インポートFormを表示
            form.Show();
        }
        private void btnユーザーマスタ_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btn郵便番号辞書_Click");
            // ユーザーマスタFormを作成
            var form = new ユーザーマスタFm();
            // ユーザーマスタFormを表示
            form.Show();
        }

        private void btn管理者用設定_Click(object sender, EventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} コントロール 1 {CMD.UserName} btn管理者用設定_Click");
            // 管理者用設定Formを作成
            var form = new 管理者用設定();
            // 管理者用設定Formを表示
            form.Show();
        }
        //==============================================================
        // デザイン関連メソッド
        //==============================================================
        /// <summary>
        /// Form2にスノーマンカラーを適用
        /// </summary>
        private void ApplySnowManColors()
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} デザイン関連メソッド 1 {CMD.UserName} ApplySnowManColors");

            // フォーム全体の背景
            this.BackColor = clrmg.FukaLight3;

            //DataGridView[] grids = { dgvDataOhno, dgvDataSdus, dgvDataScar, dgvDataIV };
            //foreach (var dgv in grids)
            //{
            //    dgv.BackgroundColor = clrmg.RauDark1;
            //}

            // ラベル類
            lb掲題.ForeColor = clrmg.FukaDark1;

            // ボタンの色
            StyleButton(btn部門マスタ, clrmg.FukaLight1, Color.White, borderColor: clrmg.FukaBase);
            StyleButton(btn取引先マスタ, clrmg.FukaLight2, Color.White, borderColor: clrmg.FukaBase);
            StyleButton(btn取引先ロール別マスタ, clrmg.FukaLight1, Color.White, borderColor: clrmg.FukaBase);
            StyleButton(btn郵便番号辞書, clrmg.FukaLight2, Color.White, borderColor: clrmg.FukaBase);
            StyleButton(btnユーザーマスタ, clrmg.FukaLight1, Color.White, borderColor: clrmg.FukaBase);
            StyleButton(btn戻る, clrmg.FukaBase, Color.White, borderColor: clrmg.FukaDark2);
        }

        /// <summary>
        /// グループボックスのカスタム枠線描画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBoxCustomBorder(object sender, PaintEventArgs e)
        {
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            fam.AddLog($"{HIZTIM} デザイン関連メソッド 1 {CMD.UserName} GroupBoxCustomBorder");

            GroupBox box = (GroupBox)sender;
            e.Graphics.Clear(box.BackColor);

            // アンチエイリアス無効（線をくっきり）
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // テキストを測定
            SizeF textSize = e.Graphics.MeasureString(box.Text, box.Font);

            // 枠線色を濃い紫色で
            using (Pen pen = new Pen(clrmg.FukaDark2, 1.5f))
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
                using (SolidBrush brush = new SolidBrush(clrmg.MemeDark1))
                {
                    e.Graphics.DrawString(box.Text, box.Font, brush, 8, 0);
                }
            }
        }
    }
}
