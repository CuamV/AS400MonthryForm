using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace あすよん月次帳票
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.lbSituation = new System.Windows.Forms.Label();
            this.listBxSituation = new System.Windows.Forms.ListBox();
            this.lbPASS = new System.Windows.Forms.Label();
            this.txtBxPASS = new System.Windows.Forms.TextBox();
            this.txtBxID = new System.Windows.Forms.TextBox();
            this.IbID = new System.Windows.Forms.Label();
            this.chkBxSuncar = new System.Windows.Forms.CheckBox();
            this.lbBumon = new System.Windows.Forms.Label();
            this.chkBxOhno = new System.Windows.Forms.CheckBox();
            this.chkBxSundus = new System.Windows.Forms.CheckBox();
            this.lbCompany = new System.Windows.Forms.Label();
            this.btnSimulation = new System.Windows.Forms.Button();
            this.btnForm1Back = new System.Windows.Forms.Button();
            this.cmbxBumon = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbTitleSimulation = new System.Windows.Forms.Label();
            this.btnMin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSituation
            // 
            this.lbSituation.AutoSize = true;
            this.lbSituation.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbSituation.Location = new System.Drawing.Point(12, 163);
            this.lbSituation.Name = "lbSituation";
            this.lbSituation.Size = new System.Drawing.Size(125, 17);
            this.lbSituation.TabIndex = 25;
            this.lbSituation.Text = "【シュミレーション状況】";
            // 
            // listBxSituation
            // 
            this.listBxSituation.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBxSituation.FormattingEnabled = true;
            this.listBxSituation.ItemHeight = 15;
            this.listBxSituation.Location = new System.Drawing.Point(15, 192);
            this.listBxSituation.Name = "listBxSituation";
            this.listBxSituation.Size = new System.Drawing.Size(378, 124);
            this.listBxSituation.TabIndex = 24;
            // 
            // lbPASS
            // 
            this.lbPASS.AutoSize = true;
            this.lbPASS.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbPASS.Location = new System.Drawing.Point(32, 102);
            this.lbPASS.Name = "lbPASS";
            this.lbPASS.Size = new System.Drawing.Size(54, 19);
            this.lbPASS.TabIndex = 31;
            this.lbPASS.Text = "PASS:";
            // 
            // txtBxPASS
            // 
            this.txtBxPASS.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxPASS.Location = new System.Drawing.Point(88, 102);
            this.txtBxPASS.Name = "txtBxPASS";
            this.txtBxPASS.Size = new System.Drawing.Size(115, 24);
            this.txtBxPASS.TabIndex = 30;
            // 
            // txtBxID
            // 
            this.txtBxID.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxID.Location = new System.Drawing.Point(88, 69);
            this.txtBxID.Name = "txtBxID";
            this.txtBxID.Size = new System.Drawing.Size(115, 24);
            this.txtBxID.TabIndex = 28;
            // 
            // IbID
            // 
            this.IbID.AutoSize = true;
            this.IbID.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.IbID.Location = new System.Drawing.Point(54, 70);
            this.IbID.Name = "IbID";
            this.IbID.Size = new System.Drawing.Size(32, 19);
            this.IbID.TabIndex = 29;
            this.IbID.Text = "ID:";
            // 
            // chkBxSuncar
            // 
            this.chkBxSuncar.AutoSize = true;
            this.chkBxSuncar.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSuncar.Location = new System.Drawing.Point(429, 251);
            this.chkBxSuncar.Name = "chkBxSuncar";
            this.chkBxSuncar.Size = new System.Drawing.Size(134, 23);
            this.chkBxSuncar.TabIndex = 37;
            this.chkBxSuncar.Text = "サンミックカーペット";
            this.chkBxSuncar.UseVisualStyleBackColor = true;
            this.chkBxSuncar.Click += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // lbBumon
            // 
            this.lbBumon.AutoSize = true;
            this.lbBumon.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbBumon.Location = new System.Drawing.Point(426, 299);
            this.lbBumon.Name = "lbBumon";
            this.lbBumon.Size = new System.Drawing.Size(48, 17);
            this.lbBumon.TabIndex = 34;
            this.lbBumon.Text = "【部門】";
            // 
            // chkBxOhno
            // 
            this.chkBxOhno.AutoSize = true;
            this.chkBxOhno.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxOhno.Location = new System.Drawing.Point(429, 193);
            this.chkBxOhno.Name = "chkBxOhno";
            this.chkBxOhno.Size = new System.Drawing.Size(61, 23);
            this.chkBxOhno.TabIndex = 32;
            this.chkBxOhno.Text = "オーノ";
            this.chkBxOhno.UseVisualStyleBackColor = true;
            this.chkBxOhno.Click += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // chkBxSundus
            // 
            this.chkBxSundus.AutoSize = true;
            this.chkBxSundus.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkBxSundus.Location = new System.Drawing.Point(429, 222);
            this.chkBxSundus.Name = "chkBxSundus";
            this.chkBxSundus.Size = new System.Drawing.Size(122, 23);
            this.chkBxSundus.TabIndex = 33;
            this.chkBxSundus.Text = "サンミックダスコン";
            this.chkBxSundus.UseVisualStyleBackColor = true;
            this.chkBxSundus.Click += new System.EventHandler(this.Company_CheckedChanged);
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbCompany.Location = new System.Drawing.Point(426, 163);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(48, 17);
            this.lbCompany.TabIndex = 35;
            this.lbCompany.Text = "【会社】";
            // 
            // btnSimulation
            // 
            this.btnSimulation.BackColor = System.Drawing.SystemColors.Control;
            this.btnSimulation.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSimulation.Location = new System.Drawing.Point(337, 67);
            this.btnSimulation.Name = "btnSimulation";
            this.btnSimulation.Size = new System.Drawing.Size(121, 35);
            this.btnSimulation.TabIndex = 38;
            this.btnSimulation.Text = "シュミレーション実行";
            this.btnSimulation.UseVisualStyleBackColor = false;
            this.btnSimulation.Click += new System.EventHandler(this.btnSimulation_Click);
            // 
            // btnForm1Back
            // 
            this.btnForm1Back.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnForm1Back.Location = new System.Drawing.Point(468, 67);
            this.btnForm1Back.Name = "btnForm1Back";
            this.btnForm1Back.Size = new System.Drawing.Size(108, 35);
            this.btnForm1Back.TabIndex = 65;
            this.btnForm1Back.Text = "戻る";
            this.btnForm1Back.UseVisualStyleBackColor = true;
            this.btnForm1Back.Click += new System.EventHandler(this.btnForm1Back_Click);
            // 
            // cmbxBumon
            // 
            this.cmbxBumon.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbxBumon.FormattingEnabled = true;
            this.cmbxBumon.Location = new System.Drawing.Point(429, 333);
            this.cmbxBumon.Name = "cmbxBumon";
            this.cmbxBumon.Size = new System.Drawing.Size(155, 25);
            this.cmbxBumon.TabIndex = 66;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::あすよん月次帳票.Properties.Resources.ic_all_csm02;
            this.pictureBox1.Location = new System.Drawing.Point(15, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 71;
            this.pictureBox1.TabStop = false;
            // 
            // lbTitleSimulation
            // 
            this.lbTitleSimulation.AutoSize = true;
            this.lbTitleSimulation.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitleSimulation.Location = new System.Drawing.Point(51, 15);
            this.lbTitleSimulation.Name = "lbTitleSimulation";
            this.lbTitleSimulation.Size = new System.Drawing.Size(117, 17);
            this.lbTitleSimulation.TabIndex = 72;
            this.lbTitleSimulation.Text = "< シュミレーション >";
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMin.Location = new System.Drawing.Point(554, 10);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(39, 20);
            this.btnMin.TabIndex = 73;
            this.btnMin.Text = "―";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 386);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.lbTitleSimulation);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cmbxBumon);
            this.Controls.Add(this.btnForm1Back);
            this.Controls.Add(this.btnSimulation);
            this.Controls.Add(this.chkBxSuncar);
            this.Controls.Add(this.lbBumon);
            this.Controls.Add(this.chkBxOhno);
            this.Controls.Add(this.chkBxSundus);
            this.Controls.Add(this.lbCompany);
            this.Controls.Add(this.lbPASS);
            this.Controls.Add(this.txtBxPASS);
            this.Controls.Add(this.txtBxID);
            this.Controls.Add(this.IbID);
            this.Controls.Add(this.lbSituation);
            this.Controls.Add(this.listBxSituation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "シュミレーション";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private System.Windows.Forms.Label lbSituation;
        private System.Windows.Forms.ListBox listBxSituation;
        private System.Windows.Forms.Label lbPASS;
        private System.Windows.Forms.TextBox txtBxPASS;
        private System.Windows.Forms.TextBox txtBxID;
        private System.Windows.Forms.Label IbID;
        private System.Windows.Forms.CheckBox chkBxSuncar;
        private System.Windows.Forms.Label lbBumon;
        private System.Windows.Forms.CheckBox chkBxOhno;
        private System.Windows.Forms.CheckBox chkBxSundus;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.Button btnSimulation;
        private System.Windows.Forms.Button btnForm1Back;
        private System.Windows.Forms.ComboBox cmbxBumon;


        /// <summary>
        /// ボタンにさっくんカラーなど任意の色を適用する
        /// </summary>
        /// <param name="btn">対象ボタン</param>
        /// <param name="backColor">背景色</param>
        /// <param name="foreColor">文字色</param>
        private void StyleButton(Button btn, Color backColor, Color foreColor)
        {
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = ColorManager.SakuDark2; // 枠線は濃いめピンク
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = new System.Drawing.Font("Meiryo UI", 9.75F, FontStyle.Regular);
        }

        private void StyleButton(Button btn, string snowManName = null, Color? backColor = null, Color? borderColor = null, Color? foreColor = null)
        {
            // -- ボタンのスタイル設定 --
            Color baseColor = backColor ?? Color.White;
            Color bdColor = borderColor ?? ColorManager.MemeDark1;
            Color ftColor = foreColor ?? Color.Black;

            btn.BackColor = baseColor;
            btn.ForeColor = ftColor;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.BorderColor = bdColor;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Meiryo UI", 9.75F, FontStyle.Bold);

            // SnowMan名による枠線のデフォルト値（必要ならここで上書き）
            if (!string.IsNullOrEmpty(snowManName))
            {
                string name = snowManName.ToLower();
                if (name == "fuka") borderColor = borderColor ?? ColorManager.FukaLight2;
                else if (name == "meme") borderColor = borderColor ?? ColorManager.MemeBase;
                else if (name == "shopy") borderColor = borderColor ?? ColorManager.ShopyBase;
                else if (name == "abe") borderColor = borderColor ?? ColorManager.AbeBase;
                else if (name == "hikaru") borderColor = borderColor ?? ColorManager.HikaruBase;
                else if (name == "koji") borderColor = borderColor ?? ColorManager.KojiBase;
                else if (name == "saku") borderColor = borderColor ?? ColorManager.SakuBase;
                else if (name == "rau") borderColor = borderColor ?? ColorManager.RauLight2;
                // 他のSnowMan名も必要に応じて追加
            }
            
            // 角丸設定
            int radius = 12;
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                btn.Region = new Region(path);
            }

            // Paintイベントで枠線描写
            btn.Paint += (s, e) =>
            {
                int borderSize = 3;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                using (var brush = new SolidBrush(btn.BackColor))
                using (var pen = new Pen(btn.FlatAppearance.BorderColor, borderSize))
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 背景を塗る
                    e.Graphics.FillPath(brush, path);

                    // 枠線を描く
                    e.Graphics.DrawPath(pen, path);

                    // 文字色
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, ftColor,
                                          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                ;
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
            Color baseColor = originalColors.ContainsKey(btn) ? originalColors[btn] : btn.BackColor;
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
        #endregion

        private PictureBox pictureBox1;
        private Label lbTitleSimulation;
        private Button btnMin;
    }
}