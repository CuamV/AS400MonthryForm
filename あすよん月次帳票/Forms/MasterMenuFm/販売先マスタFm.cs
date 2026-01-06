using Newtonsoft.Json;
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
    public partial class 販売先マスタFm : Form
    {
        public 販売先マスタFm()
        {
            InitializeComponent();
        }

        private void btnダウンロード_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    // ==============================
            //    // JSONパス
            //    // ==============================
            //    string jsonPath = Path.Combine(CMD.mfPath, "DLB01HANBAI.json");
            //    if (!File.Exists(jsonPath))
            //    {
            //        MessageBox.Show("DLB01HANBAI.json が見つかりません");
            //        return;
            //    }

            //    // ==============================
            //    // JSON読込
            //    // ==============================
            //    string json = File.ReadAllText(jsonPath, Encoding.UTF8);

            //    var data = JsonConvert.DeserializeObject<
            //        Dictionary<string, List<ShiireItem>>
            //    >(json);

            //    //  1:仕入先CD 2:部門CD    3:仕入先正式名称 4:仕入先名 5:仕入先名カナ 6:仕入先略名 7:仕入先略名カナ
            //    //  8:郵便番号 9:電話番号 10:FAX番号       11:住所1   12:住所1カナ 　13:住所2     14:住所2カナ

            //    //----------------------------------------------------
            //    // ★CSV出力処理
            //    //----------------------------------------------------
            //    // CSV保存ダイアログ表示
            //    using (var sw = new StreamWriter(jsonPath, false, Encoding.UTF8))
            //    {
            //        // ヘッダ
            //        sw.WriteLine("仕入先CD,部門CD,仕入先正式名称,仕入先名,仕入先名カナ,仕入先略名,仕入先略名カナ,郵便番号,電話番号,FAX番号,住所1,住所1カナ,住所2,住所2カナ");

            //        foreach (var dept in data)
            //        {
            //            string bumonCd = dept.Key;

            //            foreach (var item in dept.Value)
            //            {
            //                string line = string.Join(",",
            //                    item.Code,
            //                    item.Name?.Trim(),
            //                    item.Kana?.Trim(),
            //                    bumonCd
            //                );

            //                sw.WriteLine(line);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"エラーが発生しました: {ex.Message}");
            //}
        }
    }
}
