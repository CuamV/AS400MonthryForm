using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace あすよん月次帳票
{
    internal class DataSummarizeMethod
    {

        internal DataTable SumData(DataTable data, List<string> groupKeys, string[] sortcols, string type, string ptn)
        {
            if (data == null || data.Rows.Count == 0) return new DataTable();

            // 伝票日付→年月(売上・仕入)
            if(data.Columns.Contains("伝票日付") && !data.Columns.Contains("年月"))
            {
                data.Columns.Add("年月", typeof(string));
                foreach (DataRow r in data.Rows)
                {
                    string dd = r["伝票日付"].ToString();
                    r["年月"] = dd.Substring(0, 6);
                }
            }

            // 結果用テーブルの作成
            DataTable result = new DataTable();

            // 結果テーブルの列作成(元テーブル列をコピー)
            foreach (DataColumn col in data.Columns)
                result.Columns.Add(col.ColumnName, col.DataType);
            //==============================================================
            // 売上・仕入
            //  1:伝票No  2:枝番    3:SbSys区分 4:取引区分  5:伝票日付
            //  6:部門CD  7:クラス  8:取引先CD  9:取引先名 10:品部門CD
            // 11:品名CD 12:品種CD 13:色CD     14:品名     15:品種名
            // 16:色名   17:数量   18:単位CD   19:単価     20:金額
            //==============================================================
            // 在庫
            //  1:年月    2:部門CD   3:在庫種別 4:クラス 5:倉庫CD
            //  6:倉庫名  7:預り先CD 8:預り先名 9:品名  10:品目CD
            // 11:残数量 12:残金額 
            //==============================================================

            //  SUM 対象となる列を明示的に定義
            var allowedSumColumns = new List<string>
            {
                "数量", "金額", "残数量", "残金額", "当月残数量", "当月残金額"
            };
            var numericColumns = data.Columns
                    .Cast<DataColumn>()
                    .Where(c => allowedSumColumns.Contains(c.ColumnName))
                    .Select(c => c.ColumnName)
                    .ToList();

            // GroupByキーでグループ化
            IEnumerable<IGrouping<string, DataRow>> grouped;
            if (groupKeys.Count > 0)
                grouped = data.AsEnumerable()
                    .GroupBy(row => string.Join("§", groupKeys.Select(k => row[k]?.ToString() ?? "")));
            else
                grouped = new List<IGrouping<string, DataRow>> { new GroupingAllRows(data) };

            // 行を生成
            foreach (var g in grouped)
            {
                DataRow newRow = result.NewRow();

                // キー列セット
                if (groupKeys.Count > 0)
                {
                    var keys = g.Key.Split('§');
                    for (int i = 0; i < groupKeys.Count; i++)
                    {
                        newRow[groupKeys[i]] = keys[i];
                    }
                }

                // 数値列をSUM
                foreach (var numCol in numericColumns)
                    newRow[numCol] = g.Sum(r => Convert.ToDecimal(r[numCol]));

                // その他列は先頭行の値をそのままセット(文字列など)
                foreach (DataColumn col in data.Columns)
                {
                    if (!groupKeys.Contains(col.ColumnName) && !numericColumns.Contains(col.ColumnName))
                        newRow[col.ColumnName] = g.First()[col.ColumnName];
                }
                result.Rows.Add(newRow);
            }
            switch (type)
            {
                case "売仕":
                    if (ptn == "1")
                    {
                        // 不要列を削除
                        //==============================================================
                        //  1:年月     2:SbSys区分 3:部門CD  4:クラス 5:取引先CD 6:取引先名
                        //  7:品部門CD 8:品名CD    9:品種CD 10:色CD  11:品名    12:品種名
                        // 13:色名    14:数量     15:単位CD 16:単価  17:金額
                        //==============================================================
                        if (result.Columns.Contains("伝票No")) result.Columns.Remove("伝票No");
                        if (result.Columns.Contains("枝番")) result.Columns.Remove("枝番");
                        if (result.Columns.Contains("取引区分")) result.Columns.Remove("取引区分");
                        if (result.Columns.Contains("伝票日付")) result.Columns.Remove("伝票日付");
                        // 年月列を先頭へ移動
                        result.Columns["年月"].SetOrdinal(0);
                    }
                    else if (ptn == "2")
                    {
                        // 不要列を削除
                        //==============================================================
                        //  1:年月 2:SbSys区分 3:部門CD 4:クラス 5:取引先CD 6:取引先名
                        //  7:数量 8:金額
                        //==============================================================
                        if (result.Columns.Contains("伝票No")) result.Columns.Remove("伝票No");
                        if (result.Columns.Contains("枝番")) result.Columns.Remove("枝番");
                        if (result.Columns.Contains("取引区分")) result.Columns.Remove("取引区分");
                        if (result.Columns.Contains("伝票日付")) result.Columns.Remove("伝票日付");
                        if (result.Columns.Contains("品部門CD")) result.Columns.Remove("品部門CD");
                        if (result.Columns.Contains("品名CD")) result.Columns.Remove("品名CD");
                        if (result.Columns.Contains("品種CD")) result.Columns.Remove("品種CD");
                        if (result.Columns.Contains("色CD")) result.Columns.Remove("色CD");
                        if (result.Columns.Contains("品名")) result.Columns.Remove("品名");
                        if (result.Columns.Contains("品種名")) result.Columns.Remove("品種名");
                        if (result.Columns.Contains("色名")) result.Columns.Remove("色名");
                        if (result.Columns.Contains("単位CD")) result.Columns.Remove("単位CD");
                        if (result.Columns.Contains("単価")) result.Columns.Remove("単価");
                        // 年月列を先頭へ移動
                        result.Columns["年月"].SetOrdinal(0);
                    }
                    else if (ptn == "3")
                    {
                        // 不要列を削除
                        //==============================================================
                        //  1:年月 2:SbSys区分 3:部門CD 4:クラス 5:数量 6:金額
                        //==============================================================
                        if (result.Columns.Contains("伝票No")) result.Columns.Remove("伝票No");
                        if (result.Columns.Contains("枝番")) result.Columns.Remove("枝番");
                        if (result.Columns.Contains("取引区分")) result.Columns.Remove("取引区分");
                        if (result.Columns.Contains("伝票日付")) result.Columns.Remove("伝票日付");
                        if (result.Columns.Contains("取引先CD")) result.Columns.Remove("取引先CD");
                        if (result.Columns.Contains("伝票日付")) result.Columns.Remove("伝票日付");
                        if (result.Columns.Contains("取引先名")) result.Columns.Remove("取引先名");
                        if (result.Columns.Contains("品名CD")) result.Columns.Remove("品名CD");
                        if (result.Columns.Contains("品種CD")) result.Columns.Remove("品種CD");
                        if (result.Columns.Contains("色CD")) result.Columns.Remove("色CD");
                        if (result.Columns.Contains("品名")) result.Columns.Remove("品名");
                        if (result.Columns.Contains("品種名")) result.Columns.Remove("品種名");
                        if (result.Columns.Contains("色名")) result.Columns.Remove("色名");
                        if (result.Columns.Contains("単位CD")) result.Columns.Remove("単位CD");
                        if (result.Columns.Contains("単価")) result.Columns.Remove("単価");
                        // 年月列を先頭へ移動
                        result.Columns["年月"].SetOrdinal(0);
                    }
                    break;
                case "在庫":
                    if (ptn == "1")
                    {
                        // 不要列を削除
                        //==============================================================
                        // 1:年月 2:部門CD 3:在庫種別 4:クラス 5:品名 6:品目CD 7:残数量 8:残金額 
                        //==============================================================
                        if (result.Columns.Contains("倉庫CD")) result.Columns.Remove("倉庫CD");
                        if (result.Columns.Contains("倉庫名")) result.Columns.Remove("倉庫名");
                        if (result.Columns.Contains("預り先CD")) result.Columns.Remove("預り先CD");
                        if (result.Columns.Contains("預り先名")) result.Columns.Remove("預り先名");
                    }
                    break;
            }
            
                DataProcessor processor = new DataProcessor();
            result = processor.SortData(result, sortcols);

            return result;
        }
        //====================================================================
        // ◆社長用集計
        internal DataTable SummarizeSalesPurchase(DataTable dt)
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
                YearMonth = r["伝票日付"]?.ToString()?.Length >= 6
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

        internal DataTable MergedSummary(List<DataTable> allTables)
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

                    if (dt.Columns.Contains("年月"))
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

        internal DataTable SummarizeByCategoryTypeDept(DataTable mergedSummary)
        {
            DataTable summary = new DataTable();
            summary.Columns.Add("年月", typeof(string));
            summary.Columns.Add("分類名", typeof(string));
            summary.Columns.Add("部門グループ", typeof(string));
            summary.Columns.Add("数量合計", typeof(decimal));
            summary.Columns.Add("金額合計", typeof(decimal));

            if (mergedSummary == null || mergedSummary.Rows.Count == 0) return summary;

            Func<string, string, string> mapCategory = (className, transType) =>
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
        //==============================================================================
    }
    // 全行を1グループとして返すヘルパークラス (NONE 用)
    internal class GroupingAllRows : IGrouping<string, DataRow>
    {
        private IEnumerable<DataRow> _rows;
        public GroupingAllRows(DataTable dt) => _rows = dt.AsEnumerable();
        public string Key => "";
        public IEnumerator<DataRow> GetEnumerator() => _rows.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _rows.GetEnumerator();
    }
}
