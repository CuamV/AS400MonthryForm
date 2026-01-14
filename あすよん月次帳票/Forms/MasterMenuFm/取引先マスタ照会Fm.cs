using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class 取引先マスタ照会Fm : Form
    {
        private DataGridView dgv;
        private Button btnClose;
        private readonly string _placeholderText = "複数コード検索時は、カンマ/スペース/読点/改行のいずれかで区切ってください";
        private Color _normalForeColor;
        private readonly Color _placeholderColor = SystemColors.GrayText;

        public 取引先マスタ照会Fm()
        {
            InitializeComponent();
            // placeholder 初期設定
            _normalForeColor = txtBxコード検索.ForeColor;
            if (string.IsNullOrEmpty(txtBxコード検索.Text))
            {
                txtBxコード検索.Text = _placeholderText;
                txtBxコード検索.ForeColor = _placeholderColor;
            }
            else if (txtBxコード検索.Text == _placeholderText)
            {
                txtBxコード検索.ForeColor = _placeholderColor;
            }

            txtBxコード検索.Enter += TxtBxコード検索_Enter;
            txtBxコード検索.Leave += TxtBxコード検索_Leave;
        }

        public void SetData(List<string> lines)
        {
            if (lines == null) return;
            dgv.Columns.Clear();

            //  1:取引先CD   2:取引先正式名称 3:取引先名   4:取引先名カナ 5:取引先略名 6:取引先略名カナ 7:郵便番号 8:電話番号1   9:電話番号2
            // 10:FAX番号1  11:FAX番号2    12:住所1     13:住所1カナ  14:住所2    15:住所2カナ    16:商社区分 17:仕入先区分 18:販売先区分
            // 19:得意先区分 20:出荷先区分   21:預り先区分 22:運送便区分 23:倉庫区分  24:備考        25:登録者  26:登録日付   27:登録時刻
            var cols = Enum.GetNames(typeof(TORIHIKI_MASTER_OUT));
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 入力された複数コードでフィルタリングを行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBxコード検索_TextChanged(object sender, EventArgs e)
        {
            // プレースホルダ表示中はフィルタしない（全行表示）
            var current = txtBxコード検索.Text;
            if (string.IsNullOrWhiteSpace(current) || current == _placeholderText)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.Visible = true;
                }
                return;
            }

            // カンマ、スペース、読点、改行で区切られた複数コードを取得してトリム
            var codes = current
                .Split(new char[] { ',', '、', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToArray();

            // 部分一致でマッチした行のみ表示、その他は非表示にする
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var cellValue = row.Cells[(int)TORIHIKI_MASTER_OUT.取引先CD].Value?.ToString();
                if (string.IsNullOrEmpty(cellValue))
                {
                    row.Visible = false;
                    continue;
                }

                // いずれかのコードがセル値に含まれていれば表示
                row.Visible = codes.Any(code => cellValue.Contains(code));
            }

        }

        private void TxtBxコード検索_Enter(object sender, EventArgs e)
        {
            if (txtBxコード検索.Text == _placeholderText)
            {
                txtBxコード検索.Text = string.Empty;
                txtBxコード検索.ForeColor = _normalForeColor;
            }
        }

        private void TxtBxコード検索_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBxコード検索.Text))
            {
                txtBxコード検索.Text = _placeholderText;
                txtBxコード検索.ForeColor = _placeholderColor;
            }
        }
    }
}

