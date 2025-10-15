using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace あすよん月次帳票
{
    public static class ColorManager
    {
        // ラウール（白系）
        public static Color RauDark1 = Color.FromArgb(240, 240, 240);
        public static Color RauDark2 = Color.FromArgb(233, 238, 242);
        public static Color RauBase = Color.White;
        public static Color RauLight1 = Color.FromArgb(247, 249, 241);
        public static Color RauLight2 = Color.FromArgb(255, 255, 235);

        // めめ（黒系）
        public static Color MemeDark1 = Color.FromArgb(41, 41, 41);
        public static Color MemeDark2 = Color.FromArgb(28, 28, 28);
        public static Color MemeBase = Color.Black;
        public static Color MemeLight1 = Color.FromArgb(92, 92, 92);
        public static Color MemeLight2 = Color.FromArgb(169, 169, 169);

        // しょっぴー（青系）
        public static Color ShopyDark1 = Color.FromArgb(0, 50, 150);
        public static Color ShopyDark2 = Color.FromArgb(0, 70, 200);
        public static Color ShopyBase = Color.FromArgb(0, 120, 215);
        public static Color ShopyLight1 = Color.FromArgb(100, 180, 255);
        public static Color ShopyLight2 = Color.FromArgb(220, 240, 255);

        // 阿部ちゃん（緑系）
        public static Color AbeDark1 = Color.FromArgb(0, 100, 0);
        public static Color AbeDark2 = Color.FromArgb(0, 140, 0);
        public static Color AbeBase = Color.FromArgb(0, 180, 0);
        public static Color AbeLight1 = Color.FromArgb(100, 220, 100);
        public static Color AbeLight2 = Color.FromArgb(150, 250, 150);

        // ひーくん（黄色系）
        public static Color HikaruDark1 = Color.FromArgb(200, 180, 0);
        public static Color HikaruDark2 = Color.FromArgb(220, 200, 0);
        public static Color HikaruBase = Color.FromArgb(255, 245, 100);
        public static Color HikaruLight1 = Color.FromArgb(255, 255, 150);
        public static Color HikaruLight2 = Color.FromArgb(255, 255, 200);

        // 康二オレンジ
        public static Color KojiDark1 = Color.FromArgb(255, 102, 0);
        public static Color KojiDark2 = Color.FromArgb(246, 94, 0);
        public static Color KojiBase = Color.FromArgb(255, 140, 0);
        public static Color KojiLight1 = Color.FromArgb(255, 180, 80);
        public static Color KojiLight2 = Color.FromArgb(255, 204, 102);

        // さっくんピンク
        public static Color SakuDark1 = Color.FromArgb(160, 50, 120);   // 少し落ち着いた濃いめ紫
        public static Color SakuDark2 = Color.FromArgb(180, 70, 140);
        public static Color SakuBase = Color.FromArgb(230, 150, 220); // メイン操作色（優しいラベンダー寄りピンク）
        public static Color SakuLight1 = Color.FromArgb(246, 176, 231); // 背景寄りの薄ピンクラベンダー
        public static Color SakuLight2 = Color.FromArgb(255, 220, 255); // さらに淡めの背景補助

        // ふっか紫
        public static Color FukaDark1 = Color.FromArgb(80, 0, 120);
        public static Color FukaDark2 = Color.FromArgb(100, 0, 150);
        public static Color FukaBase = Color.FromArgb(150, 0, 200);
        public static Color FukaLight1 = Color.FromArgb(180, 100, 220);
        public static Color FukaLight2 = Color.FromArgb(200, 150, 230);

        // 舘様赤
        public static Color DateDark1 = Color.FromArgb(150, 0, 0);
        public static Color DateDark2 = Color.FromArgb(180, 0, 0);
        public static Color DateBase = Color.FromArgb(220, 0, 0);
        public static Color DateLight1 = Color.FromArgb(255, 50, 50);
        public static Color DateLight2 = Color.FromArgb(255, 100, 100);

    }
}
