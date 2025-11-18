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
        public DataTable DisplayData { get; set; }

        public Form2_DataView()
        {
            InitializeComponent();
            // DataGridViewスタイル設定
            StyleDataGrid(dgvData, Color.DarkBlue, Color.White, Color.LightGray);
            
        }
        private void Form2_DataView_Load(object sender, EventArgs e)
        {
            if (DisplayData != null)
            {
                dgvData.DataSource = DisplayData;
            }
        }
    }
}
