using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace あすよん月次帳票
{

    internal partial class WaitSimulationFm : Form
    {
        private Timer closeTimer;

        internal WaitSimulationFm()
        {
            InitializeComponent();

            this.TopMost = false; // 最前面表示を一時的に解除

            pictBx1.Image = Image.FromFile(@"\\ohnosv01\OhnoSys\099_sys\Images\Infiter_Big.gif");
            pictBx1.SizeMode = PictureBoxSizeMode.Zoom; // PictureBoxに合わせて拡大縮小
            // 画面中央に表示
            this.StartPosition = FormStartPosition.CenterScreen;

            // フォーム設定
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.Opacity = 0.95;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));

            progressBar1.Style = ProgressBarStyle.Marquee; // ％表示なしで動き続ける
            progressBar1.MarqueeAnimationSpeed = 30;

            this.AutoScaleMode = AutoScaleMode.None;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        internal void CloseForm()
        {
            this.Invoke((Action)(() => this.Close()));
        }
    }
}


