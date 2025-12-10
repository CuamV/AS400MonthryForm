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
        /// <summary>
        /// フォームの初期化(コンストラクタ)   
        /// </summary>
        private void InitializeComponent()  
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lb操作履歴 = new System.Windows.Forms.Label();
            this.listBxログ表示 = new System.Windows.Forms.ListBox();
            this.btn終了 = new System.Windows.Forms.Button();
            this.lnkLbシュミレーション = new System.Windows.Forms.LinkLabel();
            this.lnkLb定型帳票 = new System.Windows.Forms.LinkLabel();
            this.lnkLbExport = new System.Windows.Forms.LinkLabel();
            this.lnkLbデータ抽出 = new System.Windows.Forms.LinkLabel();
            this.lnkLbマスタ更新 = new System.Windows.Forms.LinkLabel();
            this.timrReleaseLock = new System.Windows.Forms.Timer(this.components);
            this.grpBxメニュー = new System.Windows.Forms.GroupBox();
            this.lbメニュー = new System.Windows.Forms.Label();
            this.timrLogRenewal = new System.Windows.Forms.Timer(this.components);
            this.grpBxメニュー.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb操作履歴
            // 
            this.lb操作履歴.AutoSize = true;
            this.lb操作履歴.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb操作履歴.Location = new System.Drawing.Point(29, 21);
            this.lb操作履歴.Name = "lb操作履歴";
            this.lb操作履歴.Size = new System.Drawing.Size(89, 20);
            this.lb操作履歴.TabIndex = 23;
            this.lb操作履歴.Text = "【操作履歴】";
            // 
            // listBxログ表示
            // 
            this.listBxログ表示.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxログ表示.FormattingEnabled = true;
            this.listBxログ表示.ItemHeight = 15;
            this.listBxログ表示.Location = new System.Drawing.Point(33, 43);
            this.listBxログ表示.Name = "listBxログ表示";
            this.listBxログ表示.Size = new System.Drawing.Size(563, 184);
            this.listBxログ表示.TabIndex = 0;
            // 
            // btn終了
            // 
            this.btn終了.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn終了.Location = new System.Drawing.Point(476, 421);
            this.btn終了.Name = "btn終了";
            this.btn終了.Size = new System.Drawing.Size(102, 35);
            this.btn終了.TabIndex = 2;
            this.btn終了.Text = "終了";
            this.btn終了.UseVisualStyleBackColor = true;
            this.btn終了.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // lnkLbシュミレーション
            // 
            this.lnkLbシュミレーション.AutoSize = true;
            this.lnkLbシュミレーション.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbシュミレーション.Location = new System.Drawing.Point(266, 26);
            this.lnkLbシュミレーション.Name = "lnkLbシュミレーション";
            this.lnkLbシュミレーション.Size = new System.Drawing.Size(122, 20);
            this.lnkLbシュミレーション.TabIndex = 1;
            this.lnkLbシュミレーション.TabStop = true;
            this.lnkLbシュミレーション.Text = "● シュミレーション";
            this.lnkLbシュミレーション.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbSimulate_LinkClicked);
            // 
            // lnkLb定型帳票
            // 
            this.lnkLb定型帳票.AutoSize = true;
            this.lnkLb定型帳票.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLb定型帳票.Location = new System.Drawing.Point(81, 76);
            this.lnkLb定型帳票.Name = "lnkLb定型帳票";
            this.lnkLb定型帳票.Size = new System.Drawing.Size(94, 20);
            this.lnkLb定型帳票.TabIndex = 2;
            this.lnkLb定型帳票.TabStop = true;
            this.lnkLb定型帳票.Text = "● 定型帳票";
            this.lnkLb定型帳票.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbStandard_LinkClicked);
            // 
            // lnkLbExport
            // 
            this.lnkLbExport.Location = new System.Drawing.Point(0, 0);
            this.lnkLbExport.Name = "lnkLbExport";
            this.lnkLbExport.Size = new System.Drawing.Size(100, 23);
            this.lnkLbExport.TabIndex = 75;
            // 
            // lnkLbデータ抽出
            // 
            this.lnkLbデータ抽出.AutoSize = true;
            this.lnkLbデータ抽出.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbデータ抽出.Location = new System.Drawing.Point(266, 76);
            this.lnkLbデータ抽出.Name = "lnkLbデータ抽出";
            this.lnkLbデータ抽出.Size = new System.Drawing.Size(99, 20);
            this.lnkLbデータ抽出.TabIndex = 3;
            this.lnkLbデータ抽出.TabStop = true;
            this.lnkLbデータ抽出.Text = "● データ抽出";
            this.lnkLbデータ抽出.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbDisplay_LinkClicked);
            // 
            // lnkLbマスタ更新
            // 
            this.lnkLbマスタ更新.AutoSize = true;
            this.lnkLbマスタ更新.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lnkLbマスタ更新.Location = new System.Drawing.Point(81, 26);
            this.lnkLbマスタ更新.Name = "lnkLbマスタ更新";
            this.lnkLbマスタ更新.Size = new System.Drawing.Size(97, 20);
            this.lnkLbマスタ更新.TabIndex = 0;
            this.lnkLbマスタ更新.TabStop = true;
            this.lnkLbマスタ更新.Text = "● マスタ更新";
            this.lnkLbマスタ更新.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLbMaster_LinkClicked);
            // 
            // timrReleaseLock
            // 
            this.timrReleaseLock.Interval = 60000;
            // 
            // grpBxメニュー
            // 
            this.grpBxメニュー.Controls.Add(this.lnkLbマスタ更新);
            this.grpBxメニュー.Controls.Add(this.lnkLbデータ抽出);
            this.grpBxメニュー.Controls.Add(this.lnkLbシュミレーション);
            this.grpBxメニュー.Controls.Add(this.lnkLb定型帳票);
            this.grpBxメニュー.Location = new System.Drawing.Point(83, 284);
            this.grpBxメニュー.Name = "grpBxメニュー";
            this.grpBxメニュー.Size = new System.Drawing.Size(461, 118);
            this.grpBxメニュー.TabIndex = 1;
            this.grpBxメニュー.TabStop = false;
            // 
            // lbメニュー
            // 
            this.lbメニュー.AutoSize = true;
            this.lbメニュー.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbメニュー.Location = new System.Drawing.Point(89, 272);
            this.lbメニュー.Name = "lbメニュー";
            this.lbメニュー.Size = new System.Drawing.Size(72, 20);
            this.lbメニュー.TabIndex = 77;
            this.lbメニュー.Text = "【メニュー】";
            // 
            // timrLogRenewal
            // 
            this.timrLogRenewal.Interval = 15000;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(629, 468);
            this.Controls.Add(this.lbメニュー);
            this.Controls.Add(this.grpBxメニュー);
            this.Controls.Add(this.lnkLbExport);
            this.Controls.Add(this.btn終了);
            this.Controls.Add(this.listBxログ表示);
            this.Controls.Add(this.lb操作履歴);
            this.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "あすよん月次帳票";
            this.Activated += new System.EventHandler(this.timerReleaseLock_Tick);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpBxメニュー.ResumeLayout(false);
            this.grpBxメニュー.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        /// <summary>
        /// Form1内ボタンの共通スタイル設定メソッド
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="backColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="radius"></param>
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
            btn.FlatAppearance.BorderColor = borderColor ?? clrmg.MemeDark1;

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

        /// <summary>
        /// Form1のShownイベントハンドラ(フェードイン)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_Shown(object sender, EventArgs e)
        {
            this.Opacity = 0; // 初期状態で透明に設定
            while (this.Opacity < 1)
            {
                await Task.Delay(20); // 20ミリ秒待機,速度調整（小さくすると早く出る)
                this.Opacity += 0.05; // 徐々に不透明に
            }
            LoadLogs(); // ログの初期読み込み
        }

        /// <summary>
        /// ボタンの初期位置と色を保持(伸び防止)
        /// </summary>
        private readonly Dictionary<Button, int> originalYPositions = new Dictionary<Button, int>();
        private readonly Dictionary<Button, bool> animatingButtons = new Dictionary<Button, bool>();
        private readonly Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        /// <summary>
        /// ボタンのクリックアニメーション
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// マウスエンター時のアニメーション(ホバー効果)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = clrmg.MemeLight2; // ホバー時の色
                await AnimateButton(btn);
            }
        }

        /// <summary>
        /// マウスリーブ時のアニメーション(ホバー効果解除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.White; // 元に戻す
                await AnimateButton(btn);
            }
        }

        /// <summary>
        /// マウスダウン時のアニメーション(クリック効果)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.FlatAppearance.BorderSize = 4; // クリック時に枠線太く
                await AnimateButton(btn);
            }
        }

        /// <summary>
        /// マウスアップ時のアニメーション(クリック効果解除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.FlatAppearance.BorderSize = 2; // 元に戻す
                await AnimateButton(btn);
            }
        }

        private ListBox listBxログ表示;
        private Button btn終了;
        private LinkLabel lnkLbシュミレーション;
        private LinkLabel lnkLb定型帳票;
        private LinkLabel lnkLbExport;
        private LinkLabel lnkLbデータ抽出;
        private LinkLabel lnkLbマスタ更新;
        private Timer timrReleaseLock;
        private System.ComponentModel.IContainer components;
        private GroupBox grpBxメニュー;
        private Label lbメニュー;
        private Timer timrLogRenewal;
    }
}
