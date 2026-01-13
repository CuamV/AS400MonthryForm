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

        // --------------------------------
        // 在庫区分コード→日本語変換用辞書
        // --------------------------------
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

        // --------------------------------
        // 会社選択用リスト
        // --------------------------------
        internal static List<string> selCompanies = new List<string> 
        {
            "オーノ",
            "サンミックダスコン",
            "サンミックカーペット"
        };

        
        internal static List<string> selBumons = new List<string>();
        internal static List<string> selSelleres = new List<string>();
        internal static List<string> selSupplieres = new List<string>();
        // --------------------------------
        // データ種別選択用リスト
        // --------------------------------
        internal static List<string> selCategories = new List<string> 
        {
            "売上",
            "仕入",
            "在庫"
        };
        internal static List<string> selSlPrProducts = new List<string>();

        // --------------------------------
        // 在庫種別選択用リスト
        // --------------------------------
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

        // --------------------------------
        // サブシステム区分→日本語変換用辞書
        // --------------------------------
        internal static Dictionary<string, string> sysTypeMap = new Dictionary<string, string> 
        {
            { "SL", "売上高" },
            { "PR", "仕入高" }
        };

        // --------------------------------
        // 部門グループ定義と IncludeZero フラグ
        // --------------------------------
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

        internal static Dictionary<string,string> toriRoll_masterMap = new Dictionary<string,string>
        {
            { "商社", "SYOSYA.txt" },
            { "仕入先", "SHIIRE.txt" },
            { "販売先", "HANBAI.txt" },
            { "得意先", "TOKUI.txt" },
            { "出荷先", "SYUKKA.txt" },
            { "預り先", "AZUKARI.txt" },
            { "運送便", "UNSOU.txt" },
            { "倉庫", "SOKO.txt" }
        };
    }


}

