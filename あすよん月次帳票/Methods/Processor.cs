using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    internal class Processor
    {
        /// <summary>
        /// 売上・仕入をマージして列名を統一する
        /// </summary>

        internal DataTable MergeSalesPurchase(DataTable salesDt, DataTable purchaseDt,bool fmFlg)
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
                "伝票No", "枝番", "サブシステム区分", "取引区分", 
                "伝票日付","部門CD", "クラス名", "取引先CD", "取引先名", 
                "品名部門CD", "品名CD", "品名", "品種CD", "品種名","色CD", "色名",  
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

        internal DataTable SortData(DataTable dt)
        {
            var dv = dt.DefaultView;
            List<string> sortColumns = new List<string>();
            foreach (var c in new[] { "伝票No", "枝番", "伝票日付", "部門CD", "取引先CD" })
            {
                if (dt.Columns.Contains(c)) sortColumns.Add(c + " ASC");
            }
            if (sortColumns.Count > 0)
                dv.Sort = string.Join(", ", sortColumns);
            return dv.ToTable();
        }

        internal DataTable SortData(DataTable dt, string[] sortcols)
        {
            var dv = dt.DefaultView;
            List<string> sortColumns = new List<string>();
            foreach (var c in sortcols)
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
        internal DataTable NormalizeColumnNames(DataTable dt, string type)
        {
            if (dt == null) return new DataTable();
            DataTable dtCopy = dt.Copy();

            try
            {
                if (type == "Sales") // 売上
                {
                    if (dtCopy.Columns.Contains("URNHNO")) dtCopy.Columns["URNHNO"].ColumnName = "伝票No";
                    if (dtCopy.Columns.Contains("URNHEB")) dtCopy.Columns["URNHEB"].ColumnName = "枝番";
                    if (dtCopy.Columns.Contains("URSBKB")) dtCopy.Columns["URSBKB"].ColumnName = "SbSys区分";
                    if (dtCopy.Columns.Contains("URTRKB")) dtCopy.Columns["URTRKB"].ColumnName = "取引区分";
                    if (dtCopy.Columns.Contains("URDNDT")) dtCopy.Columns["URDNDT"].ColumnName = "伝票日付";
                    if (dtCopy.Columns.Contains("URBMCD")) dtCopy.Columns["URBMCD"].ColumnName = "部門CD";
                    if (dtCopy.Columns.Contains("SHCLAS")) dtCopy.Columns["SHCLAS"].ColumnName = "クラス";
                    if (dtCopy.Columns.Contains("URHBSC")) dtCopy.Columns["URHBSC"].ColumnName = "取引先CD";
                    if (dtCopy.Columns.Contains("TOTHNM")) dtCopy.Columns["TOTHNM"].ColumnName = "取引先名";
                    if (dtCopy.Columns.Contains("URHBCD")) dtCopy.Columns["URHBCD"].ColumnName = "品部門CD";
                    if (dtCopy.Columns.Contains("URHMCD")) dtCopy.Columns["URHMCD"].ColumnName = "品名CD";
                    if (dtCopy.Columns.Contains("URHSCD")) dtCopy.Columns["URHSCD"].ColumnName = "品種CD";
                    if (dtCopy.Columns.Contains("URCLCD")) dtCopy.Columns["URCLCD"].ColumnName = "色CD";
                    if (dtCopy.Columns.Contains("URHNNM")) dtCopy.Columns["URHNNM"].ColumnName = "品名";
                    if (dtCopy.Columns.Contains("URHSNM")) dtCopy.Columns["URHSNM"].ColumnName = "品種名";
                    if (dtCopy.Columns.Contains("URCLNM")) dtCopy.Columns["URCLNM"].ColumnName = "色名";
                    if (dtCopy.Columns.Contains("URQNTY")) dtCopy.Columns["URQNTY"].ColumnName = "数量";
                    if (dtCopy.Columns.Contains("URUNCD")) dtCopy.Columns["URUNCD"].ColumnName = "単位CD";
                    if (dtCopy.Columns.Contains("URUNPR")) dtCopy.Columns["URUNPR"].ColumnName = "単価";
                    if (dtCopy.Columns.Contains("URAMNT")) dtCopy.Columns["URAMNT"].ColumnName = "金額";
                }
                else if (type == "Purchase") // 仕入
                {
                    if (dtCopy.Columns.Contains("SRSRNO")) dtCopy.Columns["SRSRNO"].ColumnName = "伝票No";
                    if (dtCopy.Columns.Contains("SRSREB")) dtCopy.Columns["SRSREB"].ColumnName = "枝番";
                    if (dtCopy.Columns.Contains("SRSBKB")) dtCopy.Columns["SRSBKB"].ColumnName = "SbSys区分";
                    if (dtCopy.Columns.Contains("SRTRKB")) dtCopy.Columns["SRTRKB"].ColumnName = "取引区分";
                    if (dtCopy.Columns.Contains("SRDNDT")) dtCopy.Columns["SRDNDT"].ColumnName = "伝票日付";
                    if (dtCopy.Columns.Contains("SRBMCD")) dtCopy.Columns["SRBMCD"].ColumnName = "部門CD";
                    if (dtCopy.Columns.Contains("SHCLAS")) dtCopy.Columns["SHCLAS"].ColumnName = "クラス";
                    if (dtCopy.Columns.Contains("SRSRCD")) dtCopy.Columns["SRSRCD"].ColumnName = "取引先CD";
                    if (dtCopy.Columns.Contains("TOTHNM")) dtCopy.Columns["TOTHNM"].ColumnName = "取引先名";
                    if (dtCopy.Columns.Contains("SRHBCD")) dtCopy.Columns["SRHBCD"].ColumnName = "品部門CD";
                    if (dtCopy.Columns.Contains("SRHMCD")) dtCopy.Columns["SRHMCD"].ColumnName = "品名CD";
                    if (dtCopy.Columns.Contains("SRHSCD")) dtCopy.Columns["SRHSCD"].ColumnName = "品種CD";
                    if (dtCopy.Columns.Contains("SRCLCD")) dtCopy.Columns["SRCLCD"].ColumnName = "色CD";
                    if (dtCopy.Columns.Contains("SRHNNM")) dtCopy.Columns["SRHNNM"].ColumnName = "品名";
                    if (dtCopy.Columns.Contains("SRHSNM")) dtCopy.Columns["SRHSNM"].ColumnName = "品種名";
                    if (dtCopy.Columns.Contains("SRCLNM")) dtCopy.Columns["SRCLNM"].ColumnName = "色名";
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
        internal DataTable ProsessStockTable(DataTable dt, string file)
        {
            foreach (DataRow row in dt.Rows)
            {
                // null値は0に変換
                if (dt.Columns.Contains("ZHTZQT") && row["ZHTZQT"] == DBNull.Value)
                    row["ZHTZQT"] = 0m;
                if (dt.Columns.Contains("ZHTGZA") && row["ZHTGZA"] == DBNull.Value)
                    row["ZHTGZA"] = 0m;
            }
            // 色CD列追加
            if (!dt.Columns.Contains("ZGCLCD"))
                dt.Columns.Add("ZGCLCD", typeof(string));

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
        internal DataTable MergeData(params DataTable[] tables)
        {
            //==============================================================
            // 「 Now 」
            // 1:年月           2:ZHCSNM(クラス名) 3:ZHBMCD(部門CD)     4:ZHHNNM(品名)
            // 5:ZHHMCD(品名CD) 6:ZHHSCD(品種CD)   7:ZHTZQT(当月残数量) 8:ZHTGZA(当月残金額)
            //==============================================================
            // 「 Old 」
            //  1:年月                2:ZGZKSB(在庫種別)     3:SHCLAS(クラス)      4:ZGBMCD(部門コード)  5:ZGWHCD(倉庫コード)
            //  6:倉庫名              7:ZGAZCD(預り先コード) 8:預り先名            9:品名               10:ZHHMCD(品名コード)
            // 11:ZHHSCD(品種コード) 12:ZGCLCD(色コード)    13:ZGTZQT(当月残数量) 14:ZGTGZA(当月残金額) 
            //==============================================================
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
        internal DataTable FormatStockTable(DataTable dt)
        {
            DataTable formated = new DataTable();
            //==============================================================
            // 「 Now 」
            // 1:年月           2:ZHBMCD(部門CD)   3:ZHCSNM(クラス名)      4:ZHHNNM(品名)
            // 5:ZHHMCD(品名CD) 6:ZHHSCD(品種CD)   7:ZHTZQT(当月残数量) 8:ZHTGZA(当月残金額)
            //==============================================================
            // 列定義
            formated.Columns.Add("年月", typeof(string));
            formated.Columns.Add("部門CD", typeof(string));
            formated.Columns.Add("クラス名", typeof(string));
            formated.Columns.Add("取引区分", typeof(string));
            formated.Columns.Add("品種", typeof(string));
            formated.Columns.Add("品名", typeof(string));
            formated.Columns.Add("当月残数量", typeof(decimal));
            formated.Columns.Add("当月残金額", typeof(decimal));

            if (dt == null) return formated; // 空のテーブルを返す

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = formated.NewRow();

                // 列が存在するかチェックして代入
                newRow["年月"] = dt.Columns.Contains("年月") ? row["年月"]?.ToString() : string.Empty;
                newRow["部門CD"] = dt.Columns.Contains("ZHBMCD") ? row["ZHBMCD"]?.ToString() : string.Empty;
                newRow["クラス名"] = dt.Columns.Contains("ZHCSNM") ? row["ZHCSNM"]?.ToString() : string.Empty;
                newRow["取引区分"] = "在庫";
                string hmcd = dt.Columns.Contains("ZHHMCD") ? row["ZHHMCD"]?.ToString() : string.Empty;
                string hscd = dt.Columns.Contains("ZHHSCD") ? row["ZHHSCD"]?.ToString() : string.Empty;
                newRow["品種"] = $"{hmcd}-{hscd}";
                newRow["品名"] = dt.Columns.Contains("ZHHNNM") ? row["ZHHNNM"]?.ToString() : string.Empty;
                newRow["当月残数量"] = (dt.Columns.Contains("ZHTZQT") && row["ZHTZQT"] != DBNull.Value)
                    ? Convert.ToDecimal(row["ZHTZQT"]) : 0m;
                newRow["当月残金額"] = (dt.Columns.Contains("ZHTGZA") && row["ZHTGZA"] != DBNull.Value)
                    ? Convert.ToDecimal(row["ZHTGZA"]) : 0m;
                formated.Rows.Add(newRow);
            }

            // ソート
            DataView dv = formated.DefaultView;
            dv.Sort = "年月 ASC, 部門CD ASC, クラス名 ASC";
            
            return dv.ToTable();
        }
        internal DataTable FormatStockTable(DataTable dt, bool newold)
        {
            DataTable formated = new DataTable();
            //==============================================================
            // 「 Old 」
            //  1:年月                2:ZGBMCD(部門コード)   3:ZGZKSB(在庫種別)  4:SHCLAS(クラス)  5:ZGWHCD(倉庫コード)
            //  6:TOTHNM1(倉庫名)     7:ZGAZCD(預り先コード) 8:TOTHNM2(預り先名) 9:ZHHNNM(品名)   10:ZHHMCD(品名コード)
            // 11:ZHHSCD(品種コード) 12:ZGCLCD(色コード)    13:ZGTZQT(残数量)   14:ZGTGZA(残金額) 
            //==============================================================
            // 列定義
            formated.Columns.Add("年月", typeof(string));
            formated.Columns.Add("部門CD", typeof(string));
            formated.Columns.Add("在庫種別", typeof(string));
            formated.Columns.Add("クラス", typeof(string));
            formated.Columns.Add("倉庫CD", typeof (string));
            formated.Columns.Add("倉庫名", typeof(string));
            formated.Columns.Add("預り先CD", typeof(string));
            formated.Columns.Add("預り先名", typeof(string));
            formated.Columns.Add("品名", typeof(string));
            formated.Columns.Add("品目CD",typeof (string));
            formated.Columns.Add("残数量", typeof(decimal));
            formated.Columns.Add("残金額",typeof(decimal));

            if (dt == null) return formated; // 空のテーブルを返す

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = formated.NewRow();

                // 列が存在するかチェックして代入
                newRow["年月"] = dt.Columns.Contains("年月") ? row["年月"]?.ToString() : string.Empty;
                newRow["部門CD"] = dt.Columns.Contains("ZHBMCD") ? row["ZHBMCD"]?.ToString() : string.Empty;
                newRow["在庫種別"] = dt.Columns.Contains("ZGZKSB") ? row["ZGZKSB"]?.ToString() : string.Empty;
                newRow["クラス"] = dt.Columns.Contains("SHCLAS") ? row["SHCLAS"]?.ToString() : string.Empty;
                newRow["倉庫CD"] = dt.Columns.Contains("ZGWHCD") ? row["ZGWHCD"]?.ToString() : string.Empty;
                newRow["倉庫名"] = dt.Columns.Contains("TOTHNM1") ? row["TOTHNM1"]?.ToString() : string.Empty;
                newRow["預り先CD"] = dt.Columns.Contains("ZGAZCD") ? row["ZGAZCD"]?.ToString() : string.Empty;
                newRow["預り先名"] = dt.Columns.Contains("TOTHNM2") ? row["TOTHNM2"]?.ToString() : string.Empty;
                newRow["品名"] = dt.Columns.Contains("ZHHNNM") ? row["ZHHNNM"]?.ToString() : string.Empty;
                string hmcd = dt.Columns.Contains("ZHHMCD") ? row["ZHHMCD"]?.ToString() : string.Empty;
                string hscd = dt.Columns.Contains("ZHHSCD") ? row["ZHHSCD"]?.ToString() : string.Empty;
                string ircd = dt.Columns.Contains("ZGCLCD") ? row["ZGCLCD"]?.ToString() : string.Empty;
                newRow["品目CD"] = $"{hmcd}-{hscd}-{ircd}";
                
                newRow["残数量"] = (dt.Columns.Contains("ZHTZQT") && row["ZHTZQT"] != DBNull.Value)
                    ? Convert.ToDecimal(row["ZHTZQT"]) : 0m;
                newRow["残金額"] = (dt.Columns.Contains("ZHTGZA") && row["ZHTGZA"] != DBNull.Value)
                    ? Convert.ToDecimal(row["ZHTGZA"]) : 0m;
                formated.Rows.Add(newRow);
            }

            // ソート
            DataView dv = formated.DefaultView;
            dv.Sort = "年月 ASC, 部門CD ASC, クラス ASC";

            return dv.ToTable();

        }

        internal DataTable CustFilter(DataTable filtered, DataTable dt, List<string> selected, string cutCD)
        {
            string filter = string.Join(" OR ", selected.Select(code => $"{cutCD} = '{code}'"));
            DataRow[] rows = dt.Select(filter);
            DataTable tmp = filtered.Clone(); // 構造をコピー
            foreach (var row in rows) tmp.ImportRow(row);
            return tmp;
        }

        // 在庫種別フィルター
        internal DataTable IvTypeFilter(DataTable filtered, Dictionary<string, string> selected)
        {
            // ZGZKSB: 在庫種別
            // 0=自社,1=預り,2=預け,3=投入
            //==============================================================
            ///  1:年月                2:ZGZKSB(在庫種別)     3:SHCLAS(クラス)      4:ZGBMCD(部門コード)  5:ZGWHCD(倉庫コード)
            //  6:倉庫名              7:ZGAZCD(預り先コード) 8:預り先名            9:品名               10:ZHHMCD(品名コード)
            // 11:ZHHSCD(品種コード) 12:ZGCLCD(色コード)    13:ZGTZQT(当月残数量) 14:ZGTGZA(当月残金額) 
            //==============================================================
            DataTable tmp = filtered.Clone();
            foreach (DataRow row in filtered.Rows)
            {
                string ivtype = row["ZGZKSB"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(ivtype) && selected.ContainsKey(ivtype))
                {
                    tmp.ImportRow(row);
                }
            }
            return tmp;
        }
        // クラス区分フィルター
        //   売上・仕入
        internal DataTable ProductFileter(DataTable filtered, Dictionary<string, string> classProduct, List<string> selected)
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
        // 在庫
        internal DataTable ProductFileter(DataTable filtered, List<string> selected)
        {
            DataTable tmp = filtered.Clone();
            foreach (DataRow row in filtered.Rows)
            {
                string classname = row["SHCLAS"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(classname) && selected.Contains(classname))
                {
                    tmp.ImportRow(row);
                }
            }
            return tmp;
        }

        internal DataTable BumonFilter(DataTable filtered, List<string> selected, string bCD)
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

