using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    //==========================================================
    // --------部門マスタ照会Formクラス--------
    //==========================================================
    internal partial class 部門マスタ照会Form : Form
    {
        //=========================================================
        // インスタンス
        //=========================================================
        FormAction fam = new FormAction();
        ColorManager clrmg = new ColorManager();

        string HIZTIM;
        //=========================================================
        // コンストラクタ
        //=========================================================
        internal 部門マスタ照会Form()
        {
            InitializeComponent();
        }

        //=================================================================
        // 処理メソッド
        //=================================================================
        /// <summary>
        /// マスタ表示
        /// </summary>
        /// <param name="lines"></param>
        internal void SetData(List<string> lines)
        {
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
                if(parts.Length >= 4)
                {
                    dt.Rows.Add(parts[0], parts[1], parts[2], parts[3]);
                }
            }
            dgv部門マスタ.DataSource = dt;
        }
    }
}
