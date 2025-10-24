using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    internal class DataProcessor
    {
        /// <summary>
        /// 売上・仕入をマージして列名を統一する
        /// </summary>

        public DataTable MergeSalesPurchase(DataTable salesDt, DataTable purchaseDt,bool fmFlg)
        {
            if (salesDt == null) salesDt = new DataTable();
            if (purchaseDt == null) purchaseDt = new DataTable();

            // 売上・仕入の列名を統一
            var normalizedSales = NormalizeColumnNames(salesDt, "Sales");
            var normalizedPurchase = NormalizeColumnNames(purchaseDt, "Purchase");

            DataTable merged = normalizedSales.Copy();
            merged.Merge(normalizedPurchase); // 仕入データを追加

            // 必要な列を作る(空でも存在させる) Excelエクスポート時のみ
            if (fmFlg) 
            {
                string[] requiredColumns = {
                "伝票日付", "部門CD", "取引先CD", "取引先名", "クラス名", "品名部門CD",
                "品名CD", "品名", "品種CD", "品種名", "取引区分", "サブシステム区分",
                "数量", "単位CD", "単価", "金額"
                };

                foreach (var col in requiredColumns)
                {
                    if (!merged.Columns.Contains(col))
                        merged.Columns.Add(col, typeof(string));
                }
            }

            // 品種名の null 対策
            if (!merged.Columns.Contains("品種名"))
                merged.Columns.Add("品種名", typeof(string));
            if (!merged.Columns.Contains("品種CD"))
                merged.Columns.Add("品種CD", typeof(string));

            foreach (DataRow row in merged.Rows)
            {
                if (row["品種名"] == DBNull.Value || row["品種名"] == null)
                    row["品種名"] = string.Empty;
                if (row["品種CD"] == DBNull.Value || row["品種CD"] == null)
                    row["品種CD"] = string.Empty;
            }

            DataTable dv = SortData(merged);

            return dv;
        }

        public DataTable SortData(DataTable dt)
        {
            var dv = dt.DefaultView;
            List<string> sortColumns = new List<string>();
            foreach (var c in new[] { "伝票日付", "サブシステム区分", "取引区分", "部門CD", "取引先CD" })
            {
                if (dt.Columns.Contains(c)) sortColumns.Add(c + " ASC");
            }
            if (sortColumns.Count > 0)
                dv.Sort = string.Join(", ", sortColumns);
            return dv.ToTable();
        }

        /// <summary>
        /// 売上・仕入の列名を統一する
        /// </summary>
        public DataTable NormalizeColumnNames(DataTable dt, string type)
        {
            if (dt == null) return new DataTable();
            DataTable dtCopy = dt.Copy();

            try
            {
                if (type == "Sales") // 売上
                {
                    if (dtCopy.Columns.Contains("URDNDT")) dtCopy.Columns["URDNDT"].ColumnName = "伝票日付";
                    if (dtCopy.Columns.Contains("URBMCD")) dtCopy.Columns["URBMCD"].ColumnName = "部門CD";
                    if (dtCopy.Columns.Contains("URHBSC")) dtCopy.Columns["URHBSC"].ColumnName = "取引先CD";
                    if (dtCopy.Columns.Contains("TOTHNM")) dtCopy.Columns["TOTHNM"].ColumnName = "取引先名";
                    if (dtCopy.Columns.Contains("SHCLAS")) dtCopy.Columns["SHCLAS"].ColumnName = "クラス";
                    if (dtCopy.Columns.Contains("URHBCD")) dtCopy.Columns["URHBCD"].ColumnName = "品名部門CD";
                    if (dtCopy.Columns.Contains("URHMCD")) dtCopy.Columns["URHMCD"].ColumnName = "品名CD";
                    if (dtCopy.Columns.Contains("URHNNM")) dtCopy.Columns["URHNNM"].ColumnName = "品名";
                    if (dtCopy.Columns.Contains("URHSCD")) dtCopy.Columns["URHSCD"].ColumnName = "品種CD";
                    if (dtCopy.Columns.Contains("URHSNM")) dtCopy.Columns["URHSNM"].ColumnName = "品種名";
                    if (dtCopy.Columns.Contains("URTRKB")) dtCopy.Columns["URTRKB"].ColumnName = "取引区分";
                    if (dtCopy.Columns.Contains("URSBKB")) dtCopy.Columns["URSBKB"].ColumnName = "サブシステム区分";
                    if (dtCopy.Columns.Contains("URQNTY")) dtCopy.Columns["URQNTY"].ColumnName = "数量";
                    if (dtCopy.Columns.Contains("URUNCD")) dtCopy.Columns["URUNCD"].ColumnName = "単位CD";
                    if (dtCopy.Columns.Contains("URUNPR")) dtCopy.Columns["URUNPR"].ColumnName = "単価";
                    if (dtCopy.Columns.Contains("URAMNT")) dtCopy.Columns["URAMNT"].ColumnName = "金額";
                }
                else if (type == "Purchase") // 仕入
                {
                    if (dtCopy.Columns.Contains("SRDNDT")) dtCopy.Columns["SRDNDT"].ColumnName = "伝票日付";
                    if (dtCopy.Columns.Contains("SRBMCD")) dtCopy.Columns["SRBMCD"].ColumnName = "部門CD";
                    if (dtCopy.Columns.Contains("SRSRCD")) dtCopy.Columns["SRSRCD"].ColumnName = "取引先CD";
                    if (dtCopy.Columns.Contains("TOTHNM")) dtCopy.Columns["TOTHNM"].ColumnName = "取引先名";
                    if (dtCopy.Columns.Contains("SHCLAS")) dtCopy.Columns["SHCLAS"].ColumnName = "クラス";
                    if (dtCopy.Columns.Contains("SRHBCD")) dtCopy.Columns["SRHBCD"].ColumnName = "品名部門CD";
                    if (dtCopy.Columns.Contains("SRHMCD")) dtCopy.Columns["SRHMCD"].ColumnName = "品名CD";
                    if (dtCopy.Columns.Contains("SRHNNM")) dtCopy.Columns["SRHNNM"].ColumnName = "品名";
                    if (dtCopy.Columns.Contains("SRHSCD")) dtCopy.Columns["SRHSCD"].ColumnName = "品種CD";
                    if (dtCopy.Columns.Contains("SRHSNM")) dtCopy.Columns["SRHSNM"].ColumnName = "品種名";
                    if (dtCopy.Columns.Contains("SRTRKB")) dtCopy.Columns["SRTRKB"].ColumnName = "取引区分";
                    if (dtCopy.Columns.Contains("SRSBKB")) dtCopy.Columns["SRSBKB"].ColumnName = "サブシステム区分";
                    if (dtCopy.Columns.Contains("SRQNTY")) dtCopy.Columns["SRQNTY"].ColumnName = "数量";
                    if (dtCopy.Columns.Contains("SRUNCD")) dtCopy.Columns["SRUNCD"].ColumnName = "単位CD";
                    if (dtCopy.Columns.Contains("SRUNPR")) dtCopy.Columns["SRUNPR"].ColumnName = "単価";
                    if (dtCopy.Columns.Contains("SRAMNT")) dtCopy.Columns["SRAMNT"].ColumnName = "金額";
                }
            }
            catch
            {
                // 列が存在しない場合は無視
            }
            // 品種名・品種CDを必ず空白埋め
            foreach (DataRow row in dtCopy.Rows)
            {
                if (!dtCopy.Columns.Contains("品種名")) dtCopy.Columns.Add("品種名", typeof(string));
                if (!dtCopy.Columns.Contains("品種CD")) dtCopy.Columns.Add("品種CD", typeof(string));

                if (row["品種名"] == DBNull.Value || row["品種名"] == null)
                    row["品種名"] = string.Empty;
                if (row["品種CD"] == DBNull.Value || row["品種CD"] == null)
                    row["品種CD"] = string.Empty;
            }
            return dtCopy;
        }

        // カテゴリ列追加＆欠損値０埋め
        public DataTable ProsessStockTable(DataTable dt, string file)
        {
            foreach (DataRow row in dt.Rows)
            {
                // null値は0に変換
                if (dt.Columns.Contains("ZHTZQT") && row["ZHTZQT"] == DBNull.Value)
                    row["ZHTZQT"] = 0m;
                if (dt.Columns.Contains("ZHTGZA") && row["ZHTGZA"] == DBNull.Value)
                    row["ZHTGZA"] = 0m;
            }
            // 品名CD・品種CDの結合
            if (!dt.Columns.Contains("CodeCombined"))
                dt.Columns.Add("CodeCombined", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                row["CodeCombined"] = $"{row["ZHHMCD"]}-{row["ZHHSCD"]}";
            }
            return dt;
        }

        // 複数DataTableを縦にマージ
        public DataTable MergeStockData(params DataTable[] tables)
        {
            if (tables == null || tables.Length == 0)
                return new DataTable();

            DataTable merged = null;
            foreach (var dt in tables)
            {
                if (dt == null) continue;
                if (merged == null) merged = dt.Clone(); // 列構造をコピー
                foreach (DataRow row in dt.Rows)
                    merged.ImportRow(row);
            }

            // 全部 null or 行数ゼロだった場合も列構造は保証
            if (merged == null)
                merged = tables[0].Clone();

            return merged;
        }

        // レイアウト調整
        public DataTable FormatStockTable(DataTable dt, bool flg = false)
        {
            DataTable formated = new DataTable();

            // 列定義
            formated.Columns.Add("年月", typeof(string));
            formated.Columns.Add("クラス名", typeof(string));
            formated.Columns.Add("取引区分", typeof(string));
            formated.Columns.Add("部門CD", typeof(string));
            if (flg)
            {
                // true Excel用
                formated.Columns.Add("取引先/品種CD", typeof(string));
                formated.Columns.Add("取引先/品名", typeof(string));
            }
            else 
            {
                // false 表示用
                formated.Columns.Add("品種", typeof(string));
                formated.Columns.Add("品名", typeof(string));
            }
            formated.Columns.Add("数量計", typeof(decimal));
            formated.Columns.Add("金額計", typeof(decimal));

            if (dt == null) return formated; // 空のテーブルを返す

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = formated.NewRow();

                // 列が存在するかチェックして代入
                newRow["年月"] = dt.Columns.Contains("年月") ? row["年月"]?.ToString() : string.Empty;
                newRow["クラス名"] = dt.Columns.Contains("ZHCSNM") ? row["ZHCSNM"]?.ToString() : string.Empty;
                newRow["取引区分"] = "在庫";
                newRow["部門CD"] = dt.Columns.Contains("ZHBMCD") ? row["ZHBMCD"]?.ToString() : string.Empty;
                string hmcd = dt.Columns.Contains("ZHHMCD") ? row["ZHHMCD"]?.ToString() : string.Empty;
                string hscd = dt.Columns.Contains("ZHHSCD") ? row["ZHHSCD"]?.ToString() : string.Empty;
                if (flg)
                {
                    newRow["取引先/品種CD"] = $"{hmcd}-{hscd}";
                    newRow["取引先/品名"] = dt.Columns.Contains("ZHHNNM") ? row["ZHHNNM"]?.ToString() : string.Empty;
                }
                else
                {
                    newRow["品種"] = $"{hmcd}-{hscd}";
                    newRow["品名"] = dt.Columns.Contains("ZHHNNM") ? row["ZHHNNM"]?.ToString() : string.Empty;
                }
                    
                newRow["数量計"] = (dt.Columns.Contains("ZHTZQT") && row["ZHTZQT"] != DBNull.Value)
                    ? Convert.ToDecimal(row["ZHTZQT"]) : 0m;
                newRow["金額計"] = (dt.Columns.Contains("ZHTGZA") && row["ZHTGZA"] != DBNull.Value)
                    ? Convert.ToDecimal(row["ZHTGZA"]) : 0m;

                formated.Rows.Add(newRow);
            }

            // ソート
            DataView dv = formated.DefaultView;
            if (flg)
            {
                dv.Sort = "年月 ASC, クラス名 ASC, 取引区分 ASC, 部門CD ASC, 取引先/品種CD ASC";
            } 
            else
            {
                dv.Sort = "年月 ASC, クラス名 ASC, 取引区分 ASC, 部門CD ASC, 品種 ASC";
            }
            return dv.ToTable();
        }

        public DataTable CustFilter(DataTable filtered, DataTable dt, List<string> selected, string cutCD)
        {
            string filter = string.Join(" OR ", selected.Select(code => $"{cutCD} = '{code}'"));
            DataRow[] rows = dt.Select(filter);
            DataTable tmp = filtered.Clone(); // 構造をコピー
            foreach (var row in rows) tmp.ImportRow(row);
            return tmp;
        }

        public DataTable ProductFileter(DataTable filtered, Dictionary<string, string> classProduct, List<string> selected)
        {
            DataTable tmp = filtered.Clone();
            foreach (DataRow row in filtered.Rows)
            {
                string classcode = row["SHCLAS"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(classcode) && classProduct.ContainsKey(classcode))
                {
                    string product = classProduct[classcode];

                    if (selected.Contains(product)) tmp.ImportRow(row);
                }
            }
            return tmp;
        }

        public DataTable ProductFileter(DataTable filtered, List<string> selIvProducts, List<string> selected)
        {
            DataTable tmp = filtered.Clone();
            foreach (DataRow row in filtered.Rows)
            {
                string classname = row["ZHCSNM"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(classname) && selIvProducts.Contains(classname))
                {
                    tmp.ImportRow(row);
                }
            }
            return tmp;
        }

        public DataTable BumonFilter(DataTable filtered, List<string> selected, string bCD)
        {
            DataTable tmp = filtered.Clone();
            foreach (DataRow row in filtered.Rows)
            {
                string bumoncode = row[bCD]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(bumoncode) && selected.Contains(bumoncode))
                    tmp.ImportRow(row);
            }
            return tmp;
        }
    }

}

