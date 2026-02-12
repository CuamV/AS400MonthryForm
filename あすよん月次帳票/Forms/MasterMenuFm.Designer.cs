using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    partial class MasterMenuFm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterMenuFm));
            this.btn郵便番号辞書 = new System.Windows.Forms.Button();
            this.btn取引先マスタ = new System.Windows.Forms.Button();
            this.btn取引先ロール別マスタ = new System.Windows.Forms.Button();
            this.btn部門マスタ = new System.Windows.Forms.Button();
            this.btn戻る = new System.Windows.Forms.Button();
            this.listBx部門マスタ = new System.Windows.Forms.ListBox();
            this.grpBx組織 = new System.Windows.Forms.GroupBox();
            this.listBx取引先ロール別マスタ = new System.Windows.Forms.ListBox();
            this.listBx取引先マスタ = new System.Windows.Forms.ListBox();
            this.grpBx効率化 = new System.Windows.Forms.GroupBox();
            this.listBx郵便番号辞書 = new System.Windows.Forms.ListBox();
            this.grpBx権限 = new System.Windows.Forms.GroupBox();
            this.listBxユーザーマスタ = new System.Windows.Forms.ListBox();
            this.btnユーザーマスタ = new System.Windows.Forms.Button();
            this.lb掲題 = new System.Windows.Forms.Label();
            this.btnAS400取引先マスタ = new System.Windows.Forms.Button();
            this.grpBx組織.SuspendLayout();
            this.grpBx効率化.SuspendLayout();
            this.grpBx権限.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn郵便番号辞書
            // 
            this.btn郵便番号辞書.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn郵便番号辞書.Location = new System.Drawing.Point(9, 23);
            this.btn郵便番号辞書.Name = "btn郵便番号辞書";
            this.btn郵便番号辞書.Size = new System.Drawing.Size(115, 36);
            this.btn郵便番号辞書.TabIndex = 1;
            this.btn郵便番号辞書.Text = "郵便番号辞書";
            this.btn郵便番号辞書.UseVisualStyleBackColor = true;
            this.btn郵便番号辞書.Click += new System.EventHandler(this.btn郵便番号辞書_Click);
            // 
            // btn取引先マスタ
            // 
            this.btn取引先マスタ.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn取引先マスタ.Location = new System.Drawing.Point(9, 77);
            this.btn取引先マスタ.Name = "btn取引先マスタ";
            this.btn取引先マスタ.Size = new System.Drawing.Size(115, 36);
            this.btn取引先マスタ.TabIndex = 2;
            this.btn取引先マスタ.Text = "取引先マスタ";
            this.btn取引先マスタ.UseVisualStyleBackColor = true;
            this.btn取引先マスタ.Click += new System.EventHandler(this.btn取引先マスタ_Click);
            // 
            // btn取引先ロール別マスタ
            // 
            this.btn取引先ロール別マスタ.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn取引先ロール別マスタ.Location = new System.Drawing.Point(9, 145);
            this.btn取引先ロール別マスタ.Name = "btn取引先ロール別マスタ";
            this.btn取引先ロール別マスタ.Size = new System.Drawing.Size(115, 36);
            this.btn取引先ロール別マスタ.TabIndex = 3;
            this.btn取引先ロール別マスタ.Text = "取引先ロール別マスタ";
            this.btn取引先ロール別マスタ.UseVisualStyleBackColor = true;
            // 
            // btn部門マスタ
            // 
            this.btn部門マスタ.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn部門マスタ.Location = new System.Drawing.Point(9, 16);
            this.btn部門マスタ.Name = "btn部門マスタ";
            this.btn部門マスタ.Size = new System.Drawing.Size(115, 36);
            this.btn部門マスタ.TabIndex = 8;
            this.btn部門マスタ.Text = "部門マスタ";
            this.btn部門マスタ.UseVisualStyleBackColor = true;
            this.btn部門マスタ.Click += new System.EventHandler(this.btn部門マスタ_Click);
            // 
            // btn戻る
            // 
            this.btn戻る.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn戻る.Location = new System.Drawing.Point(191, 475);
            this.btn戻る.Margin = new System.Windows.Forms.Padding(6);
            this.btn戻る.Name = "btn戻る";
            this.btn戻る.Size = new System.Drawing.Size(85, 32);
            this.btn戻る.TabIndex = 9;
            this.btn戻る.Text = "戻る";
            this.btn戻る.UseVisualStyleBackColor = true;
            this.btn戻る.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // listBx部門マスタ
            // 
            this.listBx部門マスタ.FormattingEnabled = true;
            this.listBx部門マスタ.ItemHeight = 15;
            this.listBx部門マスタ.Items.AddRange(new object[] {
            "部門の追加登録,変更登録,削除登録や",
            "参照が行えます。"});
            this.listBx部門マスタ.Location = new System.Drawing.Point(130, 17);
            this.listBx部門マスタ.Name = "listBx部門マスタ";
            this.listBx部門マスタ.Size = new System.Drawing.Size(238, 34);
            this.listBx部門マスタ.TabIndex = 10;
            // 
            // grpBx組織
            // 
            this.grpBx組織.Controls.Add(this.listBx取引先ロール別マスタ);
            this.grpBx組織.Controls.Add(this.listBx取引先マスタ);
            this.grpBx組織.Controls.Add(this.btn部門マスタ);
            this.grpBx組織.Controls.Add(this.btn取引先ロール別マスタ);
            this.grpBx組織.Controls.Add(this.listBx部門マスタ);
            this.grpBx組織.Controls.Add(this.btn取引先マスタ);
            this.grpBx組織.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx組織.Location = new System.Drawing.Point(41, 42);
            this.grpBx組織.Name = "grpBx組織";
            this.grpBx組織.Size = new System.Drawing.Size(379, 197);
            this.grpBx組織.TabIndex = 11;
            this.grpBx組織.TabStop = false;
            this.grpBx組織.Text = "●";
            // 
            // listBx取引先ロール別マスタ
            // 
            this.listBx取引先ロール別マスタ.FormattingEnabled = true;
            this.listBx取引先ロール別マスタ.ItemHeight = 15;
            this.listBx取引先ロール別マスタ.Items.AddRange(new object[] {
            "取引先のロール別特有の内容を",
            "編集,削除登録,参照が行えます。",
            "※取引先マスタとのリレーション"});
            this.listBx取引先ロール別マスタ.Location = new System.Drawing.Point(130, 138);
            this.listBx取引先ロール別マスタ.Name = "listBx取引先ロール別マスタ";
            this.listBx取引先ロール別マスタ.Size = new System.Drawing.Size(238, 49);
            this.listBx取引先ロール別マスタ.TabIndex = 13;
            // 
            // listBx取引先マスタ
            // 
            this.listBx取引先マスタ.FormattingEnabled = true;
            this.listBx取引先マスタ.ItemHeight = 15;
            this.listBx取引先マスタ.Items.AddRange(new object[] {
            "取引先の基本情報の追加登録,変更登録,",
            "削除登録や参照が行えます。",
            "一括登録用のダウンロード・インポートも",
            "こちらから行えます。"});
            this.listBx取引先マスタ.Location = new System.Drawing.Point(130, 62);
            this.listBx取引先マスタ.Name = "listBx取引先マスタ";
            this.listBx取引先マスタ.Size = new System.Drawing.Size(238, 64);
            this.listBx取引先マスタ.TabIndex = 12;
            // 
            // grpBx効率化
            // 
            this.grpBx効率化.Controls.Add(this.listBx郵便番号辞書);
            this.grpBx効率化.Controls.Add(this.btn郵便番号辞書);
            this.grpBx効率化.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx効率化.Location = new System.Drawing.Point(41, 249);
            this.grpBx効率化.Name = "grpBx効率化";
            this.grpBx効率化.Size = new System.Drawing.Size(379, 73);
            this.grpBx効率化.TabIndex = 12;
            this.grpBx効率化.TabStop = false;
            this.grpBx効率化.Text = "●";
            // 
            // listBx郵便番号辞書
            // 
            this.listBx郵便番号辞書.FormattingEnabled = true;
            this.listBx郵便番号辞書.ItemHeight = 15;
            this.listBx郵便番号辞書.Items.AddRange(new object[] {
            "日本郵政公式の住所録アップロードが行えます。",
            "取引先マスタなどで、郵便番号入力で",
            "住所とフリガナが自動入力できるようになります。"});
            this.listBx郵便番号辞書.Location = new System.Drawing.Point(130, 15);
            this.listBx郵便番号辞書.Name = "listBx郵便番号辞書";
            this.listBx郵便番号辞書.Size = new System.Drawing.Size(238, 49);
            this.listBx郵便番号辞書.TabIndex = 14;
            // 
            // grpBx権限
            // 
            this.grpBx権限.Controls.Add(this.listBxユーザーマスタ);
            this.grpBx権限.Controls.Add(this.btnユーザーマスタ);
            this.grpBx権限.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpBx権限.Location = new System.Drawing.Point(41, 330);
            this.grpBx権限.Name = "grpBx権限";
            this.grpBx権限.Size = new System.Drawing.Size(379, 62);
            this.grpBx権限.TabIndex = 13;
            this.grpBx権限.TabStop = false;
            this.grpBx権限.Text = "●";
            // 
            // listBxユーザーマスタ
            // 
            this.listBxユーザーマスタ.FormattingEnabled = true;
            this.listBxユーザーマスタ.ItemHeight = 15;
            this.listBxユーザーマスタ.Items.AddRange(new object[] {
            "ユーザー管理が行えます。"});
            this.listBxユーザーマスタ.Location = new System.Drawing.Point(130, 17);
            this.listBxユーザーマスタ.Name = "listBxユーザーマスタ";
            this.listBxユーザーマスタ.Size = new System.Drawing.Size(238, 34);
            this.listBxユーザーマスタ.TabIndex = 14;
            // 
            // btnユーザーマスタ
            // 
            this.btnユーザーマスタ.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnユーザーマスタ.Location = new System.Drawing.Point(9, 15);
            this.btnユーザーマスタ.Name = "btnユーザーマスタ";
            this.btnユーザーマスタ.Size = new System.Drawing.Size(115, 36);
            this.btnユーザーマスタ.TabIndex = 1;
            this.btnユーザーマスタ.Text = "ユーザーマスタ";
            this.btnユーザーマスタ.UseVisualStyleBackColor = true;
            this.btnユーザーマスタ.Click += new System.EventHandler(this.btnユーザーマスタ_Click);
            // 
            // lb掲題
            // 
            this.lb掲題.AutoSize = true;
            this.lb掲題.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lb掲題.Location = new System.Drawing.Point(82, 11);
            this.lb掲題.Name = "lb掲題";
            this.lb掲題.Size = new System.Drawing.Size(310, 17);
            this.lb掲題.TabIndex = 14;
            this.lb掲題.Text = "★あすよん月次帳票内で使用可能な各種マスタメニュー★";
            // 
            // btnAS400取引先マスタ
            // 
            this.btnAS400取引先マスタ.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAS400取引先マスタ.Location = new System.Drawing.Point(50, 407);
            this.btnAS400取引先マスタ.Name = "btnAS400取引先マスタ";
            this.btnAS400取引先マスタ.Size = new System.Drawing.Size(115, 36);
            this.btnAS400取引先マスタ.TabIndex = 15;
            this.btnAS400取引先マスタ.Text = "AS400取引先マスタ";
            this.btnAS400取引先マスタ.UseVisualStyleBackColor = true;
            this.btnAS400取引先マスタ.Click += new System.EventHandler(this.btnAS400取引先マスタ_Click);
            // 
            // MasterMenuFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 525);
            this.Controls.Add(this.btnAS400取引先マスタ);
            this.Controls.Add(this.lb掲題);
            this.Controls.Add(this.grpBx権限);
            this.Controls.Add(this.grpBx効率化);
            this.Controls.Add(this.grpBx組織);
            this.Controls.Add(this.btn戻る);
            this.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MasterMenuFm";
            this.Opacity = 0.93D;
            this.Text = "マスタメニュー";
            this.grpBx組織.ResumeLayout(false);
            this.grpBx効率化.ResumeLayout(false);
            this.grpBx権限.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private void StyleButton(Button btn, Color backColor, Color foreColor, Color? borderColor = null, int radius = 12)
        {
            // -- ボタンのスタイル設定 --
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0; // 枠線はPaintで描画
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Meiryo UI", 9F, FontStyle.Bold);

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
            SetButtonAnimation(btn);

            // ★ 初期色を記録
            if (!originalColors.ContainsKey(btn))
                originalColors[btn] = btn.BackColor;
            // ★ 初期位置を記録
            if (!originalYPositions.ContainsKey(btn))
                originalYPositions[btn] = btn.Location.Y;

            // ★ クリック時アニメーション
            btn.MouseDown += (s, e) =>
            {
                float scale = 0.95f;
                var info = (btn.Size, btn.Location);
                btn.Tag = info;

                int newW = (int)(btn.Width * scale);
                int newH = (int)(btn.Height * scale);
                btn.Location = new Point(btn.Left + (btn.Width - newW) / 2, btn.Top + (btn.Height - newH) / 2);
                btn.Size = new Size(newW, newH);
            };

            btn.MouseUp += (s, e) =>
            {
                if (btn.Tag is ValueTuple<Size, Point> info)
                {
                    btn.Size = info.Item1;
                    btn.Location = info.Item2;
                }
            };
        }
        // ボタンごとの位置を保持（上昇防止用）
        private readonly Dictionary<Button, int> originalYPositions = new Dictionary<Button, int>();
        private readonly Dictionary<Button, bool> animatingButtons = new Dictionary<Button, bool>();
        private readonly Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        private async Task AnimateButton(Button btn, bool enter)
        {
            // ボタンの基本色取得
            Color baseColor = originalColors.ContainsKey(btn) ? originalColors[btn] : btn.BackColor; ;
            Color baseBorder = btn.FlatAppearance.BorderColor;

            // フェード先を少し明るくした色にする
            Color hoverColor = ControlPaint.Light(baseColor, 0.3f);
            Color hoverBorder = ControlPaint.Light(baseBorder, 0.3f);

            Color startColor = enter ? baseColor : hoverColor;
            Color endColor = enter ? hoverColor : baseColor;

            Color startBorder = enter ? baseBorder : hoverBorder;
            Color endBorder = enter ? hoverBorder : baseBorder;

            int steps = 10;
            int jumpHeight = enter ? 5 : 0; // 軽くジャンプ(出るときはジャンプしない)
            // 「元の位置」を辞書から取得する！
            int originalY = originalYPositions.ContainsKey(btn)
                ? originalYPositions[btn]
                : btn.Location.Y;

            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;

                // 背景色の補間
                btn.BackColor = Color.FromArgb(
                    (int)(startColor.R + (endColor.R - startColor.R) * t),
                    (int)(startColor.G + (endColor.G - startColor.G) * t),
                    (int)(startColor.B + (endColor.B - startColor.B) * t));

                // 枠線色の補間
                btn.FlatAppearance.BorderColor = Color.FromArgb(
                    (int)(startBorder.R + (endBorder.R - startBorder.R) * t),
                    (int)(startBorder.G + (endBorder.G - startBorder.G) * t),
                    (int)(startBorder.B + (endBorder.B - startBorder.B) * t));

                // ======== ジャンプはEnter時のみ ========
                if (enter)
                {
                    // 上に軽く持ち上げて戻る（sin波）
                    btn.Location = new Point(
                        btn.Location.X,
                        originalY - (int)(jumpHeight * Math.Sin(Math.PI * t))
                    );
                }
                else
                {
                    // Leave 時は確実に元の位置に戻す
                    btn.Location = new Point(btn.Location.X, originalY);
                }

                await Task.Delay(15); // アニメーション速度調整
            }
            btn.BackColor = endColor;
            btn.FlatAppearance.BorderColor = endBorder;
            btn.Location = new Point(btn.Location.X, originalY);
        }

        // 各ボタンにイベント登録
        private void SetButtonAnimation(Button btn)
        {
            btn.MouseEnter += async (s, e) =>
            {
                bool isAnimating;
                if (!animatingButtons.TryGetValue(btn, out isAnimating))
                {
                    isAnimating = false; // デフォルト値
                }
                animatingButtons[btn] = true;
                await AnimateButton(btn, true);
                animatingButtons[btn] = false;
            };

            btn.MouseLeave += async (s, e) =>
            {
                bool isAnimating;
                if (!animatingButtons.TryGetValue(btn, out isAnimating))
                {
                    isAnimating = false; // デフォルト値
                }
                animatingButtons[btn] = true;
                await AnimateButton(btn, false);
                animatingButtons[btn] = false;
            };

            btn.MouseDown += (s, e) => btn.FlatAppearance.BorderSize = 4;
            btn.MouseUp += (s, e) => btn.FlatAppearance.BorderSize = 2;
        }
        private System.Windows.Forms.Button btn郵便番号辞書;
        private System.Windows.Forms.Button btn取引先マスタ;
        private System.Windows.Forms.Button btn取引先ロール別マスタ;
        private System.Windows.Forms.Button btn部門マスタ;
        private System.Windows.Forms.Button btn戻る;
        private System.Windows.Forms.ListBox listBx部門マスタ;
        private System.Windows.Forms.GroupBox grpBx組織;
        private System.Windows.Forms.ListBox listBx取引先ロール別マスタ;
        private System.Windows.Forms.ListBox listBx取引先マスタ;
        private System.Windows.Forms.GroupBox grpBx効率化;
        private System.Windows.Forms.ListBox listBx郵便番号辞書;
        private System.Windows.Forms.GroupBox grpBx権限;
        private System.Windows.Forms.ListBox listBxユーザーマスタ;
        private System.Windows.Forms.Button btnユーザーマスタ;
        private System.Windows.Forms.Label lb掲題;
        private System.Windows.Forms.Button btnAS400取引先マスタ;
    }
}