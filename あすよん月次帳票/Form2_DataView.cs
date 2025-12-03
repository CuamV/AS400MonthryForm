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
    public partial class Form2_DataView : Form
    {
        private DataTable _displayData;

        // 選択された条件を格納するプロパティ
        public Dictionary<string, List<string>> SelectedConditions { get; set; }

        // 表示するデータを格納するプロパティ
        public DataTable DisplayData
        {
            get => _displayData;
            set
            {
                _displayData = value;
                if (dgvData != null)
                {
                    dgvData.DataSource = _displayData;
                    dgvData.AutoResizeColumns();
                }
            }
        }

        public Form2_DataView()
        {
            InitializeComponent();
            // DataGridViewスタイル設定
            StyleDataGrid(dgvData, Color.DarkBlue, Color.White, Color.LightGray);

            this.Load += Form2_DataView_Load;
        }
        private void Form2_DataView_Load(object sender, EventArgs e)
        {
            // 表示データの反映
            if (DisplayData != null)
            {
                dgvData.DataSource = _displayData;
            }

            // 条件の表示
            if (SelectedConditions != null)
            {
                txtBx条件.Text = BuildConditionText(SelectedConditions);
            }
        }

        private string BuildConditionText(Dictionary<string, List<string>> conditions)
        {
            
            var sb = new StringBuilder();

            foreach (var kv in conditions)
            {
                string key = kv.Key;
                // ListがnullまたはCount == 0の場合
                string values = (kv.Value != null && kv.Value.Count > 0)
                    ? string.Join(", ", kv.Value)
                    : "なし";
                sb.AppendLine($"{key}: {values}");
            }

            return sb.ToString();
        }

        
    }

}
