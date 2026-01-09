using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace あすよん月次帳票
{
    internal static class Dictionaries
    {
        
        

        // --------------------------------
        // 在庫データ抽出ファイルマップ辞書
        // --------------------------------
        internal static Dictionary<string, Dictionary<string, string>> fileMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["オーノ"] = new Dictionary<string, string>
                    {
                        { "原材料", "OIZAIKOG" },
                        { "製品", "OIZAIKOS" },
                        { "加工在庫", "OIZAIKOK" },
                        { "預り在庫", "OIZAIKOAR" },
                        { "預け在庫", "OIZAIKOAK" }
                    },
            ["サンミックダスコン"] = new Dictionary<string, string>
                    {
                        { "原材料", "SDIZAIKOG" },
                        { "コーティング半製品", "SDIZAIKOCH" },
                        { "製品", "SDIZAIKOS" },
                        { "加工在庫", "SDIZAIKOK" },
                    },
            ["サンミックカーペット"] = new Dictionary<string, string>
                    {
                        { "原材料", "SCIZAIKOG" },
                        { "タフト半製品", "SCIZAIKOTH" },
                        { "コーティング半製品", "SCIZAIKOCH" },
                        { "製品", "SCIZAIKOS" },
                        { "加工在庫", "SCIZAIKOK" },
                        { "預り在庫", "SCIZAIKOAR" },
                        { "預け在庫", "SCIZAIKOAK" }
                    }
        };

        internal static Dictionary<string, string> classMap = new Dictionary<string, string> // 在庫区分コード→日本語変換用辞書
        {
            { "1", "原材料" },
            { "2", "タフト半製品" },
            { "3", "コーティング半製品" },
            { "4", "製品" },
            { "5", "加工在庫" },
            { "6", "預り在庫" },
            { "7", "預け在庫" }
        };

        internal static List<string> selCompanies = new List<string> // 会社選択用リスト
        {
            "オーノ",
            "サンミックダスコン",
            "サンミックカーペット"
        };

        internal static List<string> selBumons = new List<string>();
        internal static List<string> selSelleres = new List<string>();
        internal static List<string> selSupplieres = new List<string>();
        internal static List<string> selCategories = new List<string>  // データ種別選択用リスト
        {
            "売上",
            "仕入",
            "在庫"
        };
        internal static List<string> selSlPrProducts = new List<string>();
        internal static List<string> selIvProducts = new List<string> 
        {
            "原材料",
            "製品",
            "加工在庫",
            "預り在庫",
            "預け在庫",
            "タフト半製品",
            "コーティング半製品"
        };
        internal static Dictionary<string, string> selIvTypes = new Dictionary<string, string>();

        internal static Dictionary<string,string> sysTypeMap = new Dictionary<string, string> // コード→日本語変換用辞書
        {
            { "SL", "売上高" },
            { "PR", "仕入高" }
        };

        // 部門グループ定義と IncludeZero フラグ
        internal static Dictionary<string, List<(string Name, Func<string, bool> Predicate, bool IncludeZero)>> DpCateGroups = new Dictionary<string, List<(string Name, Func<string, bool> Predicate, bool IncludeZero)>>()
        {
            ["原材料仕入高"] = new List<(string, Func<string, bool>, bool)> {
                    ("#110～800", c => int.TryParse(c, out int v) && v >= 110 && v <= 800, true),
                    ("#900,950", c => c=="900" || c=="950", true)
                },
            ["製品仕入高"] = new List<(string, Func<string, bool>, bool)> {
                    ("#110～800", c => int.TryParse(c, out int v) && v >= 110 && v <= 800, false),
                    ("#900,950", c => c=="900" || c=="950", true)
                },
            ["原材料売上高"] = new List<(string, Func<string, bool>, bool)> {
                    ("#110～500", c => int.TryParse(c, out int v) && v >= 110 && v <= 500, false),
                    ("#800", c => c=="800", true),
                    ("#900,950", c => c=="900" || c=="950", true)
                },
            ["製品売上高"] = new List<(string, Func<string, bool>, bool)> {
                    ("#110～500", c => int.TryParse(c, out int v) && v >= 110 && v <= 500, true),
                    ("#800", c => c=="800", true),
                    ("#900,950", c => c=="900" || c=="950", true)
                },
            ["原材料在庫"] = new List<(string, Func<string, bool>, bool)> {
                    ("#110～800", c => int.TryParse(c, out int v) && v >= 110 && v <= 800, true),
                    ("#900,950", c => c=="900" || c=="950", true)
                },
            ["仕掛品在庫"] = new List<(string, Func<string, bool>, bool)> {
                    ("#900,950", c => c=="900" || c=="950", true)
                },
            ["製品在庫"] = new List<(string, Func<string, bool>, bool)> {
                    ("#110～800", c => int.TryParse(c, out int v) && v >= 110 && v <= 800, true),
                    ("#900,950", c => c=="900" || c=="950", true)
                }
        };
    }

    internal class Dictionaries2
    {
        //internal Dictionary<string, string> ToriInTxtList;

        //public Dictionaries2()
        //{
        //    ToriInTxtList = new Dictionary<string, string>();
        //}

        //public Dictionaries2(List<string> list)
        //{
        //    ToriInTxtList = new Dictionary<string, string>
        //    {
        //        { "取引先コード", list.Count > 0 ? list[0] : "" },
        //        { "取引先正式名", list.Count > 1 ? list[1] : "" },
        //        { "取引先名", list.Count > 2 ? list[2] : "" },
        //        { "取引先名カナ", list.Count > 3 ? list[3] : "" },
        //        { "取引先略名", list.Count > 4 ? list[4] : "" },
        //        { "取引先略名カナ", list.Count > 5 ? list[5] : "" },
        //        { "郵便番号", list.Count > 6 ? list[6] : "" },
        //        { "電話番号1", list.Count > 7 ? list[7] : "" },
        //        { "電話番号2", list.Count > 8 ? list[8] : "" },
        //        { "FAX番号1", list.Count > 9 ? list[9] : "" },
        //        { "FAX番号2", list.Count > 10 ? list[10] : "" },
        //        { "住所1", list.Count > 11 ? list[11] : "" },
        //        { "住所1カナ", list.Count > 12 ? list[12] : "" },
        //        { "住所2", list.Count > 13 ? list[13] : "" },
        //        { "住所2カナ", list.Count > 14 ? list[14] : "" },
        //        { "商社区分", list.Count > 15 ? list[15] : "" },
        //        { "仕入先区分", list.Count > 16 ? list[16] : "" },
        //        { "販売先区分", list.Count > 17 ? list[17] : "" },
        //        { "得意先区分", list.Count > 18 ? list[18] : "" },
        //        { "出荷先区分", list.Count > 19 ? list[19] : "" },
        //        { "預け先区分", list.Count > 20 ? list[20] : "" },
        //        { "運送便区分", list.Count > 21 ? list[21] : "" },
        //        { "倉庫区分", list.Count > 22 ? list[22] : "" },
        //        { "備考", list.Count > 23 ? list[23] : "" }
        //    };
        //}
    }
}
