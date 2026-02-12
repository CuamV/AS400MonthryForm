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
    public partial class 販売仕入先Fm : Form
    {
        private string mode;  // "HANBAI" or "SHIIRE"
        public 販売仕入先Fm(string mode)
        {
            InitializeComponent();

            this.mode = mode;
            if (mode == "HANBAI")
                this.Text = "販売先コード選択";
            else if (mode == "SHIIRE")
                this.Text = "仕入先コード選択";
            else
                throw new ArgumentException("Invalid mode specified. Use 'HANBAI' or 'SHIIRE'.");
        }
    }
}
