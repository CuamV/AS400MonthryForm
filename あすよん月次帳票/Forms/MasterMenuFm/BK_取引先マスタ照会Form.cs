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
    public partial class 取引先マスタ照会Form : Form
    {
        public 取引先マスタ照会Form()
        {
            InitializeComponent();
        }
        /// <summary>
        /// マスタ表示
        /// </summary>
        /// <param name="lines"></param>
        internal void SetData(List<string> lines)
        {
            // テキストファイルにヘッダーをいれておいて、そのヘッダーでDataTablrのカラムを追加できないか？
            // DataTableの作成
            DataTable dt = new DataTable();
            dt.Columns.Add("部門コード");
            dt.Columns.Add("部門名");
            dt.Columns.Add("部門名カナ");
            dt.Columns.Add("会社");

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ');
                if (parts.Length >= 4)
                {
                    dt.Rows.Add(parts[0], parts[1], parts[2], parts[3]);
                }
            }
            dgv取引先マスタ.DataSource = dt;
        }
    }
}
