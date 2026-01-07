//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using CMD = あすよん月次帳票.CommonData;
//using ENM = あすよん月次帳票.Enums;
//using DataTable = System.Data.DataTable;
//using Path = System.IO.Path;

//namespace あすよん月次帳票
//{
//    public partial class 取引先マスタ_2Fm : Form
//    {
//        //=========================================================
//        // インスタンス
//        //=========================================================
//        FormAction fam = new FormAction();
//        ColorManager clrmg = new ColorManager();

//        // フィールド変数
//        string HIZTIM;
//        string mf;
//        string mfName;
//        string mf1 = Path.Combine(CMD.mfPath, "DLB01SHIIRE.txt"); //オーノ
//        string mf2 = Path.Combine(CMD.mfPath, "DLB02SHIIRE.txt"); // サンミックダスコン
//        string mf3 = Path.Combine(CMD.mfPath, "DLB03SHIIRE.txt"); //サンミックカーペット
//        string mf_bumon = Path.Combine(CMD.mfPath, "SHIIRE-BUMON.txt");
//        string mst = "取引先マスタ";
//        string mst_bumon = "取引先部門マスタ";

//        public 取引先マスタ_2Fm()
//        {
//            InitializeComponent();
//        }

//        private void SetData(List<string> newLines, List<string> newLines_bumon, string mf, string mfName)
//        {
//            //マスターファイル有無チェック＆読込
//            var lines = fam.CheckAndLoadMater(mf, mst, CMD.utf8);
//            var lines_bumon = fam.CheckAndLoadMater(mf_bumon, mst, CMD.utf8);

//            bool replaced;
//            (lines, lines_bumon, replaced) = fam.AddMasterFile(lines, lines_bumon, siireCD, bumonCD, newLine, newLine_bumon);

//            // 取引先コードでソート
//            lines = lines
//                .Where(x => !string.IsNullOrWhiteSpace(x))
//                .OrderBy(x => x.Split(' ')[0])
//                .ToList();

//            // 取引先コード＋部門コードでソート
//            lines_bumon = lines_bumon
//                .Where(x => !string.IsNullOrWhiteSpace(x))
//                .OrderBy(x => x.Split(' ')[0])
//                .ThenBy(x => x.Split(' ')[1])
//                .ToList();

//            //------------------------------------------------
//            // ★バックアップ＆ファイル書き込み
//            //------------------------------------------------
//            // バックアップ
//            fam.BackupMaster(mf, mfName, "Add", mst);
//            fam.BackupMaster(mf_bumon, mfName, "Add", mst_bumon);

//            // ファイル書き込み
//            File.WriteAllLines(mf, lines, CMD.utf8);
//            File.WriteAllLines(mf_bumon, lines_bumon, CMD.utf8);

//            // 入力内容クリア
//            cmbBx会社.SelectedItem = null;
//            cmbBx部門.SelectedItem = null;
//            txtBx取引先CD.Clear();
//            txtBx取引先正式名.Clear();
//            txtBx取引先名.Clear();
//            txtBx取引先名カナ.Clear();
//            txtBx取引先略名.Clear();
//            txtBx取引先略名カナ.Clear();
//            txtBx郵便番号.Clear();
//            txtBx電話番号1.Clear();
//            txtBxFAX番号1.Clear();
//            txtBx住所1.Clear();
//            txtBx住所1カナ.Clear();
//            txtBx住所2.Clear();
//            txtBx住所2カナ.Clear();

//            MessageBox.Show(replaced ? "変更登録が完了しました。" : "新規登録が完了しました。",
//                $"{mst}登録", MessageBoxButtons.OK, MessageBoxIcon.None);


//            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
//            fam.AddLog($"{HIZTIM} マスタ登録 1 {CMD.UserName} btn登録_Click {mst}");
//            fam.AddLog($"{HIZTIM} マスタ登録 1 {CMD.UserName} btn登録_Click {mst_bumon}");
//            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst}が更新されました");
//            fam.AddLog2($"{HIZTIM} マスタ登録 0 {CMD.UserName} btn登録_Click {mst_bumon}が更新されました");
//        }
//    }
//}
