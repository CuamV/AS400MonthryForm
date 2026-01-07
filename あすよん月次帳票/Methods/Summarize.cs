using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DCN = あすよん月次帳票.Dictionaries;

namespace あすよん月次帳票
{
    internal class Summarize
    {
        /// <summary>
        /// 集計処理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="groupKeys"></param>
        /// <param name="sortcols"></param>
        /// <param name="type"></param>
        /// <param name="ptn"></param>
        /// <returns></returns>
        internal DataTable SumData(DataTable data, List<string> groupKeys, string[] sortcols, string type, string ptn)
        {
            if (data == null || data.Rows.Count == 0) return new DataTable();

            // 伝票日付→年月(売上・仕入)
            if (data.Columns.Contains("伝票日付") && !data.Columns.Contains("年月"))
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

            Processor processor = new Processor();
            result = processor.SortData(result, sortcols);

            return result;
        }

        /// <summary>
        /// ◆社長用集計
        /// </summary>
        /// <param name="mergedSummary"></param>
        /// <returns></returns>
        internal DataTable SummarizeByCategoryTypeDept(DataTable sourceData, DataTable stockData)
        {
            // 売上・仕入データの集計
            // 1:分類 2:部門グループ 3:金額
            DataTable slprResultTable = ProcessSlprData(sourceData);

            // 在庫データの集計
            // 1:分類 2:部門グループ 3:金額
            DataTable stockResultTable = ProcessStockData(stockData);

            // ==========================================
            // 売上・仕入と在庫の集計果をマージ（重複を統合）
            // ==========================================
            var mergedResults = new Dictionary<(string Category, string Group), decimal>();

            // 売上・仕入データを辞書に追���
            foreach (DataRow row in slprResultTable.Rows)
            {
                string category = row["分類"].ToString();
                string group = row["部門グループ"].ToString();
                decimal amount = Convert.ToDecimal(row["金額"]);

                var key = (category, group);
                if (mergedResults.ContainsKey(key))
                    mergedResults[key] += amount;
                else
                    mergedResults[key] = amount;
            }

            // 在庫データを辞書に追加（既存があれば合算）
            foreach (DataRow row in stockResultTable.Rows)
            {
                string category = row["分類"].ToString();
                string group = row["部門グループ"].ToString();
                decimal amount = Convert.ToDecimal(row["金額"]);

                var key = (category, group);
                if (mergedResults.ContainsKey(key))
                    mergedResults[key] += amount;
                else
                    mergedResults[key] = amount;
            }

            // ==========================================
            // 辞書からDataTableに変換
            // ==========================================
            DataTable resultTable = CreateEmptyResultTable();

            foreach (var kvp in mergedResults)
            {
                DataRow row = resultTable.NewRow();
                row["分類"] = kvp.Key.Category;
                row["部門グループ"] = kvp.Key.Group;
                row["金額"] = kvp.Value;
                resultTable.Rows.Add(row);
            }
            //// 売上・仕入データを追加
            //foreach (DataRow row in slprResultTable.Rows)
            //{
            //    resultTable.ImportRow(row);
            //}

            //// 在庫データを追加
            //foreach (DataRow row in stockResultTable.Rows)
            //{
            //    resultTable.ImportRow(row);
            //}

            return resultTable;
        }

        /// <summary>
        /// 売上・仕入データの処理
        /// </summary>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        private DataTable ProcessSlprData(DataTable sourceData)
        {
            if (sourceData == null || sourceData.Rows.Count == 0)
                return CreateEmptyResultTable();

            //  1:伝票No   2:枝番    3:SbSys区分 4:取引区分 5:伝票日付 6:部門CD 7:クラス 8:取引先CD 9:取引先名 10:品部門CD
            // 11:品名CD  12:品種CD 13:色CD    14:品名    15:品種名  16:色名  17:数量 18:単位CD  19:単価    20:金額
            // 21:クラス名 22:数量計 23:金額計
            // ==========================================
            // 1段階目: SbSys区分・部門CD・クラスで集計
            // ==========================================
            var firstStageGroups = sourceData.AsEnumerable()
                .GroupBy(row => new
                {
                    SbSys = row.Field<string>("SbSys区分")?.Trim() ?? "",
                    DeptCd = row.Field<string>("部門CD")?.Trim() ?? "",
                    ClassCd = row.Field<string>("クラス")?.Trim() ?? ""
                })
                .Select(g => new
                {
                    g.Key.SbSys,
                    g.Key.DeptCd,
                    g.Key.ClassCd,
                    Amount = g.Sum(r => r.Field<decimal?>("金額") ?? r.Field<decimal?>("金額計") ?? 0)
                })
                .ToList();

            // ==========================================
            // 分類名マッピング関数
            // ==========================================
            Func<string, string, string> mapCategory = (className, transType) =>
            {
                string cls = (className ?? "").Trim();
                string type = (transType ?? "").Trim();

                //// 在庫系の半製品
                //if ((cls.Contains("半製品") || cls.Contains("タフト半製品") || cls.Contains("コーティング半製品"))
                //    && type.Contains("在庫"))
                //    return "仕掛品在庫";

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
                //if (cls.Contains("原材料") && type.Contains("在庫")) return "原材料在庫";
                //if (cls.Contains("製品") && type.Contains("在庫")) return "製品在庫";

                return $"{cls}{type}";
            };

            // ==========================================
            // 1段階目の集計結果を整形
            // ==========================================
            var firstStageResults = firstStageGroups.Select(g =>
            {
                // クラス名への変換
                string className = DCN.classMap.ContainsKey(g.ClassCd)
                    ? DCN.classMap[g.ClassCd]
                    : g.ClassCd;

                // 取引区分名への変換
                string transTypeName = DCN.sysTypeMap.ContainsKey(g.SbSys)
                    ? DCN.sysTypeMap[g.SbSys]
                    : g.SbSys;

                // 分類名の決定
                string categoryName = mapCategory(className, transTypeName);

                return new
                {
                    CategoryName = categoryName,
                    ClassName = className,
                    DeptCd = g.DeptCd,
                    Amount = g.Amount
                };
            }).ToList();

            // ==========================================
            // 2段階目: DpCateGroupsを利用した集計
            // ==========================================
            var secondStageResults = new List<dynamic>();

            foreach (var kvp in DCN.DpCateGroups)
            {
                string categoryName = kvp.Key;
                var groups = kvp.Value;

                foreach (var group in groups)
                {
                    string groupName = group.Name;
                    var predicate = group.Predicate;
                    bool includeZero = group.IncludeZero;

                    // 該当するデータを抽出して集計
                    var totalAmount = firstStageResults
                        .Where(r => r.CategoryName == categoryName && predicate(r.DeptCd))
                        .Sum(x => x.Amount);

                    // IncludeZero フラグに従って追加
                    if (includeZero || totalAmount != 0)
                    {
                        secondStageResults.Add(new
                        {
                            CategoryName = categoryName,
                            GroupName = groupName,
                            Amount = totalAmount
                        });
                    }
                }
            }

            // ==========================================
            // 3段階目: 分類と部門グループで合計を出す
            // ==========================================
            var finalResults = secondStageResults
                .GroupBy(x => new { x.CategoryName, x.GroupName })
                .Select(g => new
                {
                    CategoryName = g.Key.CategoryName,
                    GroupName = g.Key.GroupName,
                    Amount = g.Sum(x => (decimal)x.Amount)
                })
                .ToList();

            // ==========================================
            // 結果をDataTableに変換 [分類][部門グループ][金額]
            // ==========================================
            DataTable resultTable = CreateEmptyResultTable();

            foreach (var item in finalResults)
            {
                DataRow row = resultTable.NewRow();
                row["分類"] = item.CategoryName;
                row["部門グループ"] = item.GroupName;
                row["金額"] = item.Amount;
                resultTable.Rows.Add(row);
            }

            return resultTable;
        }

        /// <summary>
        /// 在庫データの処理
        /// </summary>
        /// <param name="stockData"></param>
        /// <returns></returns>
        public DataTable ProcessStockData(DataTable stockData)
        {
            if (stockData == null || stockData.Rows.Count == 0)
                return CreateEmptyResultTable();

            // 1:年月 2:部門CD 3:クラス名 4:取引区分 5:品種 6:品名 7:当月残数量 8:当月残金額
            // ==========================================
            // 1段階目: 部門CD・クラス名で集計
            // ==========================================
            var firstStageGroups = stockData.AsEnumerable()
                .GroupBy(row => new
                {
                    DeptCd = row.Field<string>("部門CD")?.Trim() ?? "",
                    ClassName = row.Field<string>("クラス名")?.Trim() ?? ""
                })
                .Select(g => new
                {
                    g.Key.DeptCd,
                    g.Key.ClassName,
                    Amount = g.Sum(r =>
                        r.Field<decimal?>("当月残金額") ??
                        r.Field<decimal?>("残金額") ??
                        r.Field<decimal?>("金額計") ?? 0)
                })
                .ToList();
            // ==========================================
            // 分類名マッピング関数（在庫用）
            // ==========================================
            Func<string, string> mapStockCategory = (className) =>
            {
                string cls = (className ?? "").Trim();

                // 半製品系は「仕掛品在庫」
                if (cls.Contains("半製品") || cls.Contains("タフト半製品") || cls.Contains("コーティング半製品"))
                    return "仕掛品在庫";

                // 原材料在庫
                if (cls.Contains("原材料"))
                    return "原材料在庫";

                // 製品在庫
                if (cls.Contains("製品"))
                    return "製品在庫";

                // その他は加工在庫等そのまま（必要に応じて追加）
                return cls + "在庫";
            };
            // ==========================================
            // 1段階目の集計結果を整形
            // ==========================================
            var firstStageResults = firstStageGroups.Select(g => new
            {
                CategoryName = mapStockCategory(g.ClassName),
                ClassName = g.ClassName,
                DeptCd = g.DeptCd,
                Amount = g.Amount
            }).ToList();
            // ==========================================
            // 2段階目: DpCateGroupsを利用した集計
            // ==========================================
            var secondStageResults = new List<dynamic>();

            foreach (var kvp in DCN.DpCateGroups)
            {
                string categoryName = kvp.Key;
                var groups = kvp.Value;

                // 在庫の分類のみ処理（原材料在庫、製品在庫、仕掛品在庫）
                if (!categoryName.Contains("在庫"))
                    continue;

                foreach (var group in groups)
                {
                    string groupName = group.Name;
                    var predicate = group.Predicate;
                    bool includeZero = group.IncludeZero;

                    // 該当するデータを抽出して集計
                    var totalAmount = firstStageResults
                        .Where(r => r.CategoryName == categoryName && predicate(r.DeptCd))
                        .Sum(x => x.Amount);

                    // IncludeZero フラグに従って追加
                    if (includeZero || totalAmount != 0)
                    {
                        secondStageResults.Add(new
                        {
                            CategoryName = categoryName,
                            GroupName = groupName,
                            Amount = totalAmount
                        });
                    }
                }
            }

            // ==========================================
            // 3段階目: 分類と部門グループで合計を出す
            // ==========================================
            var finalResults = secondStageResults
                .GroupBy(x => new { x.CategoryName, x.GroupName })
                .Select(g => new
                {
                    CategoryName = g.Key.CategoryName,
                    GroupName = g.Key.GroupName,
                    Amount = g.Sum(x => (decimal)x.Amount)
                })
                .ToList();

            // ==========================================
            // 結果をDataTableに変換 [分類][部門グループ][金額]
            // ==========================================
            DataTable resultTable = CreateEmptyResultTable();

            foreach (var item in finalResults)
            {
                DataRow row = resultTable.NewRow();
                row["分類"] = item.CategoryName;
                row["部門グループ"] = item.GroupName;
                row["金額"] = item.Amount;
                resultTable.Rows.Add(row);
            }
            return resultTable;
        }
        /// <summary>
        /// 空の結果テーブルを作成
        /// </summary>
        private DataTable CreateEmptyResultTable()
        {
            DataTable dt = new DataTable("SummarizedData");
            dt.Columns.Add("分類", typeof(string));
            dt.Columns.Add("部門グループ", typeof(string));
            dt.Columns.Add("金額", typeof(decimal));
            return dt;
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
}
