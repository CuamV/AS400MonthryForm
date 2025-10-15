using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class FormAnimation2 : Form
    {
        public FormAnimation2()
        {
            InitializeComponent();

            pictBx2.Image = Image.FromFile(@"\\ohnosv01\OhnoSys\099_sys\Images\free_snowman.gif");
            pictBx2.SizeMode = PictureBoxSizeMode.Zoom; // PictureBoxに合わせて拡大縮小
            // 画面中央に表示
            this.StartPosition = FormStartPosition.CenterScreen;

            // フォーム設定
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ColorManager.RauBase;
            this.Opacity = 0.95;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, this.Width, this.Height, 40, 40));
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public void CloseForm()
        {
            this.Invoke((Action)(() => this.Close()));
        }

        // lblMessage フィールドのアクセス修飾子を public に変更
        public Label lblMessage;
    }
}
