using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class Form1
    {
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbSituation = new System.Windows.Forms.Label();
            this.listBxSituation = new System.Windows.Forms.ListBox();
            this.btnEnd = new System.Windows.Forms.Button();
            this.lnkLbSimulate = new System.Windows.Forms.LinkLabel();
            this.lnkLbStandard = new System.Windows.Forms.LinkLabel();
            this.lbMenu = new System.Windows.Forms.Label();
            this.lnkLbExport = new System.Windows.Forms.LinkLabel();
            this.lnkLbDisplay = new System.Windows.Forms.LinkLabel();
            this.lnkLbMaster = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lbSituation
            // 
            this.lbSituation.AutoSize = true;
            this.lbSituation.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSituation.Location = new System.Drawing.Point(29, 21);
            this.lbSituation.Name = "lbSituation";
            this.lbSituation.Size = new System.Drawing.Size(89, 20);
            this.lbSituation.TabIndex = 23;
            this.lbSituation.Text = "【操作履歴】";
            // 
            // listBxSituation
            // 
            this.listBxSituation.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxSituation.FormattingEnabled = true;
            this.listBxSituation.ItemHeight = 15;
            this.listBxSituation.Location = new System.Drawing.Point(33, 43);
            this.listBxSituation.Name = "listBxSituation";
            this.listBxSituation.Size = new System.Drawing.Size(563, 184);
            this.listBxSituation.TabIndex = 28;
            // 
            // btnEnd
            // 
            this.btnEnd.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEnd.Location = new System.Drawing.Point(476, 421);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(102, 35);
            this.btnEnd.TabIndex = 68;
            this.btnEnd.Text = "終了";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // lnkLbSimulate
            // 
            this.lnkLbSimulate.AutoSize = true;
            this.lnkLbSimulate.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbSimulate.Location = new System.Drawing.Point(244, 313);
            this.lnkLbSimulate.Name = "lnkLbSimulate";
            this.lnkLbSimulate.Size = new System.Drawing.Size(114, 19);
            this.lnkLbSimulate.TabIndex = 69;
            this.lnkLbSimulate.TabStop = true;
            this.lnkLbSimulate.Text = "● シュミレーション";
            this.lnkLbSimulate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbSimulate_LinkClicked);
            // 
            // lnkLbStandard
            // 
            this.lnkLbStandard.AutoSize = true;
            this.lnkLbStandard.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbStandard.Location = new System.Drawing.Point(161, 362);
            this.lnkLbStandard.Name = "lnkLbStandard";
            this.lnkLbStandard.Size = new System.Drawing.Size(89, 19);
            this.lnkLbStandard.TabIndex = 70;
            this.lnkLbStandard.TabStop = true;
            this.lnkLbStandard.Text = "● 定型帳票";
            this.lnkLbStandard.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbStandard_LinkClicked);
            // 
            // lbMenu
            // 
            this.lbMenu.AutoSize = true;
            this.lbMenu.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbMenu.Location = new System.Drawing.Point(29, 274);
            this.lbMenu.Name = "lbMenu";
            this.lbMenu.Size = new System.Drawing.Size(72, 20);
            this.lbMenu.TabIndex = 71;
            this.lbMenu.Text = "【メニュー】";
            // 
            // lnkLbExport
            // 
            this.lnkLbExport.AutoSize = true;
            this.lnkLbExport.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbExport.Location = new System.Drawing.Point(424, 313);
            this.lnkLbExport.Name = "lnkLbExport";
            this.lnkLbExport.Size = new System.Drawing.Size(96, 19);
            this.lnkLbExport.TabIndex = 72;
            this.lnkLbExport.TabStop = true;
            this.lnkLbExport.Text = "● エクスポート";
            this.lnkLbExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbExport_LinkClicked);
            // 
            // lnkLbDisplay
            // 
            this.lnkLbDisplay.AutoSize = true;
            this.lnkLbDisplay.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbDisplay.Location = new System.Drawing.Point(317, 362);
            this.lnkLbDisplay.Name = "lnkLbDisplay";
            this.lnkLbDisplay.Size = new System.Drawing.Size(93, 19);
            this.lnkLbDisplay.TabIndex = 73;
            this.lnkLbDisplay.TabStop = true;
            this.lnkLbDisplay.Text = "● データ表示";
            this.lnkLbDisplay.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbDisplay_LinkClicked);
            // 
            // lnkLbMaster
            // 
            this.lnkLbMaster.AutoSize = true;
            this.lnkLbMaster.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbMaster.Location = new System.Drawing.Point(92, 313);
            this.lnkLbMaster.Name = "lnkLbMaster";
            this.lnkLbMaster.Size = new System.Drawing.Size(91, 19);
            this.lnkLbMaster.TabIndex = 74;
            this.lnkLbMaster.TabStop = true;
            this.lnkLbMaster.Text = "● マスタ更新";
            this.lnkLbMaster.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbMaster_LinkClicked);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(629, 468);
            this.Controls.Add(this.lnkLbMaster);
            this.Controls.Add(this.lnkLbDisplay);
            this.Controls.Add(this.lnkLbExport);
            this.Controls.Add(this.lbMenu);
            this.Controls.Add(this.lnkLbStandard);
            this.Controls.Add(this.lnkLbSimulate);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.listBxSituation);
            this.Controls.Add(this.lbSituation);
            this.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "あすよん月次帳票";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // ボタン共通スタイルメソッド
        private void StyleButton(Button btn, Color backColor, Color foreColor, Color? borderColor = null, int radius = 12)
        {
            // -- ボタンのスタイル設定 --
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // 枠線はPaintで描画
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Meiryo UI", 9.75F, FontStyle.Bold);

            // ボタン初期色を設定
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatAppearance.BorderColor = borderColor ?? ColorManager.MemeDark1;

            // 角丸設定
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);
            }

            // Paintイベントで背景・枠線・文字を描画
            btn.Paint += (s, e) =>
            {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                using (var brush = new SolidBrush(btn.BackColor))
                using (var pen = new Pen(btn.FlatAppearance.BorderColor, 2))
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 背景
                    e.Graphics.FillPath(brush, path);

                    // 枠線
                    e.Graphics.DrawPath(pen, path);

                    // 文字
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor,
                                          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
        }


        // フォーム読み込み時にふわっと表示（フェードイン）
        private async void Form1_Shown(object sender, EventArgs e)
        {
            this.Opacity = 0; // 初期状態で透明に設定
            while (this.Opacity < 1)
            {
                await Task.Delay(20); // 20ミリ秒待機,速度調整（小さくすると早く出る)
                this.Opacity += 0.05; // 徐々に不透明に
            }
        }

        // ボタンごとの位置を保持（伸び防止用）
        private readonly Dictionary<Button, int> originalYPositions = new Dictionary<Button, int>();
        private readonly Dictionary<Button, bool> animatingButtons = new Dictionary<Button, bool>();
        private readonly Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        // クリックアニメーション
        private async Task AnimateButton(Button btn)
        {
            var originalSize = btn.Size;
            var originalLocation = btn.Location; // ★ 元の位置を記録
            var enlargedSize = new Size((int)(btn.Width * 1.1), (int)(btn.Height * 1.1));
            int radius = 16;

            // 拡大アニメーション
            for (int i = 0; i < 5; i++)
            {
                int newWidth = btn.Size.Width + (enlargedSize.Width - originalSize.Width) / 5;
                int newHeight = btn.Size.Height + (enlargedSize.Height - originalSize.Height) / 5;

                int deltaX = (newWidth - btn.Size.Width) / 2;
                int deltaY = (newHeight - btn.Size.Height) / 2;

                btn.Location = new Point(btn.Location.X - deltaX, btn.Location.Y - deltaY);
                btn.Size = new Size(newWidth, newHeight);

                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    btn.Region = new Region(path);
                }
                await Task.Delay(15);
            }

            // 縮小アニメーション
            for (int i = 0; i < 5; i++)
            {
                int newWidth = btn.Size.Width - (enlargedSize.Width - originalSize.Width) / 5;
                int newHeight = btn.Size.Height - (enlargedSize.Height - originalSize.Height) / 5;

                int deltaX = (btn.Size.Width - newWidth) / 2;
                int deltaY = (btn.Size.Height - newHeight) / 2;

                btn.Location = new Point(btn.Location.X + deltaX, btn.Location.Y + deltaY);
                btn.Size = new Size(newWidth, newHeight);

                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    btn.Region = new Region(path);
                }
                await Task.Delay(15);
            }

            // ★ 最後に完全リセット
            btn.Size = originalSize;
            btn.Location = originalLocation;

            // 最後に Region を元に戻す
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);
            }
        }

        private async void Btn_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = ColorManager.MemeLight2; // ホバー時の色
                await AnimateButton(btn);
            }
                
        }

        private async void Btn_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.White; // 元に戻す
                await AnimateButton(btn);
            }
                
        }

        private async void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.FlatAppearance.BorderSize = 4; // クリック時に枠線太く
                await AnimateButton(btn);
            }
                
        }

        private async void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.FlatAppearance.BorderSize = 2; // 元に戻す
                await AnimateButton(btn);
            }
                
        }
        private ListBox listBxSituation;
        private Button btnEnd;
        private LinkLabel lnkLbSimulate;
        private LinkLabel lnkLbStandard;
        private Label lbMenu;
        private LinkLabel lnkLbExport;
        private LinkLabel lnkLbDisplay;
        private LinkLabel lnkLbMaster;
    }
}
