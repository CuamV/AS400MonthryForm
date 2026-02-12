using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class ユーザーマスタ照会Fm : Form
    {
        public ユーザーマスタ照会Fm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// データと会社名を設定
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="company"></param>
        public void SetData(List<string> lines)
        {
            if (lines == null) return;

            dgv.Columns.Clear();

            //  1:取引先CD   2:取引先正式名称 3:取引先名   4:取引先名カナ 5:取引先略名 6:取引先略名カナ 7:郵便番号 8:電話番号1   9:電話番号2
            // 10:FAX番号1  11:FAX番号2    12:住所1     13:住所1カナ  14:住所2    15:住所2カナ    16:商社区分 17:仕入先区分 18:販売先区分
            // 19:得意先区分 20:出荷先区分   21:預り先区分 22:運送便区分 23:倉庫区分  24:備考        25:登録者  26:登録日付   27:登録時刻
            var cols = Enum.GetNames(typeof(TORIHIKI_MASTER));
            foreach (var c in cols) dgv.Columns.Add(c, c);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ');
                var row = new string[cols.Length];
                for (int i = 0; i < cols.Length; i++) row[i] = i < parts.Length ? parts[i] : string.Empty;
                dgv.Rows.Add(row);
            }
        }
    }
}
