using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace あすよん月次帳票
{
    internal class DataSummarizeMethod
    {
        public static DataTable SummarizeSalesPurchase(DataTable dt)
        {
            DataTable summary = new DataTable();
            summary.Columns.Add("年月", typeof(string));
            summary.Columns.Add("クラス名", typeof(string));
            summary.Columns.Add("取引区分", typeof(string));
            summary.Columns.Add("部門CD", typeof(string));
            summary.Columns.Add("取引先/品種CD", typeof(string));
            summary.Columns.Add("取引先/品名", typeof(string));
            summary.Columns.Add("数量計", typeof(decimal));
            summary.Columns.Add("金額計", typeof(decimal));

            // コード→日本語変換用辞書
            var systemTypeMap = new Dictionary<string, string>
            {
                { "SL", "売上高" },
                { "PR", "仕入高" }
            };

            if (dt == null || dt.Rows.Count == 0) return summary;

            var grouped = dt.AsEnumerable().GroupBy(r => new
            {
                YearMonth = r["伝票日付"]?.ToString()?.Length >=6
                            ? r["伝票日付"].ToString().Substring(0, 6)
                            : "",
                ClassName = r["クラス名"]?.ToString()?.Trim(),
                SystemType = r["サブシステム区分"]?.ToString()?.Trim(),
                DeptCD = r["部門CD"]?.ToString()?.Trim(),
                CustCD = r["取引先CD"]?.ToString()?.Trim(),
                CustName = r["取引先名"]?.ToString()?.Trim()
            });

            foreach (var g in grouped)
            {
                DataRow newRow = summary.NewRow();
                newRow["年月"] = g.Key.YearMonth;
                newRow["クラス名"] = g.Key.ClassName;
                string systemTypeCode = g.Key.SystemType;
                newRow["取引区分"] = systemTypeMap.ContainsKey(systemTypeCode) ? systemTypeMap[systemTypeCode] : systemTypeCode;
                newRow["部門CD"] = g.Key.DeptCD;
                newRow["取引先/品種CD"] = g.Key.CustCD;
                newRow["取引先/品名"] = g.Key.CustName;
                newRow["数量計"] = g.Sum(r => r.Field<decimal?>("数量計") ?? 0m);
                newRow["金額計"] = g.Sum(r => r.Field<decimal?>("金額計") ?? 0m);
                summary.Rows.Add(newRow);
            }
            return summary;
        }

        public static DataTable MergedSummary(List<DataTable> allTables)
        {
            DataTable mergedSummary = new DataTable();

            // 列名を統一
            mergedSummary.Columns.Add("年月", typeof(string));
            mergedSummary.Columns.Add("クラス名", typeof(string));
            mergedSummary.Columns.Add("取引区分", typeof(string));
            mergedSummary.Columns.Add("部門CD", typeof(string));
            mergedSummary.Columns.Add("取引先/品種CD", typeof(string));
            mergedSummary.Columns.Add("取引先/品名", typeof(string));
            mergedSummary.Columns.Add("数量計", typeof(decimal));
            mergedSummary.Columns.Add("金額計", typeof(decimal));

            // 各テーブルの行をマージ
            foreach (var dt in allTables)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = mergedSummary.NewRow();

                    if(dt.Columns.Contains("年月"))
                        newRow["年月"] = row["年月"]?.ToString()?.Trim();
                    else
                        newRow["年月"] = "";
                    newRow["クラス名"] = row["クラス名"];
                    newRow["取引区分"] = row["取引区分"];
                    newRow["部門CD"] = row["部門CD"];
                    newRow["取引先/品種CD"] = row["取引先/品種CD"];
                    newRow["取引先/品名"] = row["取引先/品名"];
                    newRow["数量計"] = row["数量計"];
                    newRow["金額計"] = row["金額計"];

                    mergedSummary.Rows.Add(newRow);
                }
            }

            // ソート
            DataView dv = mergedSummary.DefaultView;
            dv.Sort = "年月 ASC, クラス名 ASC, 取引区分 ASC, 部門CD ASC, 取引先/品種CD ASC";
            return dv.ToTable();
        }

        public static DataTable SummarizeByCategoryTypeDept(DataTable mergedSummary)
        {
            DataTable summary = new DataTable();
            summary.Columns.Add("年月", typeof(string));
            summary.Columns.Add("分類名", typeof(string));
            summary.Columns.Add("部門グループ", typeof(string));
            summary.Columns.Add("数量合計", typeof(decimal));
            summary.Columns.Add("金額合計", typeof(decimal));

            if (mergedSummary == null || mergedSummary.Rows.Count == 0) return summary;

            Func<string, string,string> mapCategory = (className, transType) =>
            {
                string cls = (className ?? "").Trim();
                string type = (transType ?? "").Trim();

                // 在庫系の半製品
                if ((cls.Contains("半製品") || cls.Contains("タフト半製品") || cls.Contains("コーティング半製品"))
                    && type.Contains("在庫"))
                    return "仕掛品在庫";

                // 売上高の半製品
                if (cls.Contains("半製品") && type.Contains("売上"))
                    return "製品売上高";

                // 仕入高の半製品は原材料仕入高
                if (cls.Contains("半製品") && type.Contains("仕入"))
                    return "原材料仕入高";

                // 通常パターン
                if (cls.Contains("原材料") && type.Contains("仕入")) return "原材料仕入高";
                if (cls.Contains("製品") && type.Contains("仕入")) return "製品仕入高";
                if (cls.Contains("原材料") && type.Contains("売上")) return "原材料売上高";
                if (cls.Contains("製品") && type.Contains("売上")) return "製品売上高";
                if (cls.Contains("原材料") && type.Contains("在庫")) return "原材料在庫";
                if (cls.Contains("製品") && type.Contains("在庫")) return "製品在庫";

                return $"{cls}{type}";
            };

            // 部門グループ定義と IncludeZero フラグ
            var categoryDeptGroups = new Dictionary<string, List<(string Name, Func<string, bool> Predicate, bool IncludeZero)>>()
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

            // 分類名リスト(クラス名＋取引区分)
            var classTypes = mergedSummary.AsEnumerable()
                        .Select(r => mapCategory(r["クラス名"]?.ToString(), r["取引区分"]?.ToString()))
                        .Distinct();

            // --- ★ 年月単位でのループ ---
            var months = mergedSummary.AsEnumerable()
                .Select(r => r["年月"]?.ToString()?.Trim())
                .Where(y => !string.IsNullOrEmpty(y))
                .Distinct()
                .OrderBy(y => y);

            foreach (var ym in months)
            {
                // 3. 分類ごとにグループ集計
                foreach (var cls in classTypes)
                {
                    if (!categoryDeptGroups.ContainsKey(cls)) continue;

                    foreach (var group in categoryDeptGroups[cls])
                    {
                        var rows = mergedSummary.AsEnumerable()
                                    .Where(r =>
                                    (r["年月"]?.ToString()?.Trim() == ym) &&
                                    mapCategory(r["クラス名"]?.ToString(), r["取引区分"]?.ToString()) == cls
                                                && group.Predicate(r["部門CD"]?.ToString()));

                        decimal sumQty = rows.Sum(r => r.Field<decimal?>("数量計") ?? 0m);
                        decimal sumAmt = rows.Sum(r => r.Field<decimal?>("金額計") ?? 0m);

                        // IncludeZeroフラグで表示制御
                        if (!rows.Any() && !group.IncludeZero) continue;

                        DataRow newRow = summary.NewRow();
                        newRow["年月"] = ym;
                        newRow["分類名"] = cls;
                        newRow["部門グループ"] = group.Name;
                        newRow["数量合計"] = sumQty;
                        newRow["金額合計"] = sumAmt;
                        summary.Rows.Add(newRow);
                    }
                }
            }

            // ソート用列を追加して分類名の順序を制御
            summary.Columns.Add("ソート順", typeof(int));
            Func<string, int> getSortOrder = cls =>
            {
                switch (cls)
                {
                    case "原材料仕入高": return 10;
                    case "製品仕入高": return 20;
                    case "原材料売上高": return 30;
                    case "製品売上高": return 40;
                    case "原材料在庫": return 50;
                    case "仕掛品在庫": return 60;
                    case "製品在庫": return 70;
                    default: return 999;
                }
            };
            foreach (DataRow r in summary.Rows)
            {
                r["ソート順"] = getSortOrder(r["分類名"].ToString());
            }

            // ソート
            DataView dv = summary.DefaultView;
            dv.Sort = "年月 ASC, ソート順 ASC, 部門グループ ASC";

            DataTable result = dv.ToTable();
            result.Columns.Remove("ソート順");

            return result;
        }

        public static DataTable SumOldStockData(DataTable IVdata)
        {
            var grouped = IVdata.AsEnumerable()
                    .GroupBy(r => new
                    {
                        ClassName = r.Field<string>("ZHCSNM"),
                        Dept = r.Field<string>("ZHBMCD"),
                        ProductName = r.Field<string>("ZHHNNM"), 
                        ProductCode = r.Field<string>("ZHHMCD"), // 品名CD
                        VarietyCode = r.Field<string>("ZHHSCD")  // 品種CD
                    })
                    .Select(g => {
                        var row = IVdata.NewRow();
                        row["ZHCSNM"] = g.Key.ClassName;
                        row["ZHBMCD"] = g.Key.Dept;
                        row["ZHHNNM"] = g.Key.ProductName;
                        row["ZHHMCD"] = g.Key.ProductCode;
                        row["ZHHSCD"] = g.Key.VarietyCode;
                        row["ZHTZQT"] = g.Sum(x => Convert.ToDouble(x["ZHTZQT"]));
                        row["ZHTGZA"] = g.Sum(x => Convert.ToInt32(x["ZHTGZA"]));
                        return row;
                    }).CopyToDataTable();

            return grouped;
        }
    }
}
