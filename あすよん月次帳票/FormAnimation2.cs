using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    internal partial class FormAnimation2 : Form
    {
        ColorManager clrmg = new ColorManager();
        internal FormAnimation2()
        {
            InitializeComponent();

            pictBx2.Image = Image.FromFile(@"\\ohnosv01\OhnoSys\099_sys\Images\free_snowman.gif");
            pictBx2.SizeMode = PictureBoxSizeMode.Zoom; // PictureBoxに合わせて拡大縮小
            // 画面中央に表示
            this.StartPosition = FormStartPosition.CenterScreen;

            // フォーム設定
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = clrmg.RauBase;
            this.Opacity = 0.95;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));

            this.MouseDown += FormAnimation2_MouseDown;
            this.MouseMove += FormAnimation2_MouseMove;
            this.MouseUp += FormAnimation2_MouseUp;
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        internal void CloseForm()
        {
            this.Invoke((Action)(() => this.Close()));
        }

        // lblMessage フィールドのアクセス修飾子を internal に変更
        internal Label lblMessage;


        // フィールドに追加
        private Point mouseOffset;
        private bool isMouseDown = false;
        private void FormAnimation2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = new Point(-e.X, -e.Y);
            }
        }

        // MouseMove
        private void FormAnimation2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                this.Location = mousePos;
            }
        }

        // MouseUp
        private void FormAnimation2_MouseUp(object sender, MouseEventArgs e)
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
    }
}
