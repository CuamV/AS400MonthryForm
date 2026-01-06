using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ENM = あすよん月次帳票.Enums;

namespace あすよん月次帳票
{
    public partial class 取引先マスタ照会Fm : Form
    {
        private DataGridView dgv;
        private Button btnClose;

        public 取引先マスタ照会Fm()
        {
            InitializeComponent();
        }

        public void SetData(List<string> lines)
        {
            if (lines == null) return;
            dgv.Columns.Clear();

            //  1:取引先CD  2:取引先正式名称 3:取引先名 4:取引先名カナ 5:取引先略名 6:取引先略名カナ 7:郵便番号 8:電話番号1 9:電話番号2
            // 10:FAX番号1 11:FAX番号2    12:住所1  13:住所1カナ   14:住所2    15:住所2カナ    16:備考   17:登録者   18:登録日付 19:登録時刻
            var cols = Enum.GetNames(typeof(ENM.TORIHIKI_MASTER_OUT));
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
    }
}
