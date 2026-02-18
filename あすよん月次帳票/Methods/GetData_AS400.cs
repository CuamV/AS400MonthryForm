using IBM.Data.DB2.iSeries;
using Ohno.Db;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using DCN = あすよん月次帳票.Dictionaries;
using MessageBox = System.Windows.MessageBox;

namespace あすよん月次帳票
{
    internal class GetData_AS400
    {
        // クラスフィールドとしてdbManagerを宣言
        private readonly DbManager_Db2 dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);

        // 売上データ取得
        internal DataTable GetSalesData(string symd, string eymd, string lib)
        {
            string sql = $@"
                        SELECT URNHNO, URNHEB, URSBKB, URTRKB, 
                               URDNDT, URBMCD, M4.SHCLAS, SL.URHBSC, 
                               COALESCE(PM1.TOTHNM, '') AS TOTHNM, URHBCD, URHMCD, URHSCD,
                               URCLCD, URHNNM, URHSNM, URCLNM, 
                               URQNTY, URUNCD, URUNPR, URAMNT 
                        FROM {lib}.SLURIMP AS SL 
                        LEFT JOIN SM1MLB01.MMSHUP AS M4
                            ON SL.URHSCD = M4.SHHSCD
                        LEFT JOIN SM1MLB01.MMTORIP AS PM1 
                            ON SL.URHBSC = PM1.TOTHCD 
                        WHERE URDNDT >= ? AND URDNDT <= ?
                        AND URSBKB = 'SL'
                        AND URTRKB NOT IN (38, 39, 48, 49)";
            var ps = new List<iDB2Parameter>
            {
                new iDB2Parameter("p1",symd),
                new iDB2Parameter("p2",eymd)
            };

            var dt = dbManager.GetDataTable(sql, ps.ToArray());

            return dt;
        }

        // 仕入データ取得
        internal DataTable GetPurchaseData(string symd, string eymd, string lib, string code = "")
        {
            string sql = $@"
                        SELECT SRSRNO, SRSREB, SRSBKB, SRTRKB,
                               SRDNDT, SRBMCD, M4.SHCLAS, PR.SRSRCD, 
                               COALESCE(PM1.TOTHNM, '') AS TOTHNM, SRHBCD, SRHMCD, SRHSCD, 
                               SRCLCD, SRHNNM, SRHSNM, SRCLNM,
                               SRQNTY, SRUNCD, SRUNPR, SRAMNT 
                        FROM {lib}.PRSREMP AS PR 
                        LEFT JOIN SM1MLB01.MMSHUP AS M4
                            ON PR.SRHSCD = M4.SHHSCD
                        LEFT JOIN SM1MLB01.MMTORIP AS PM1 
                            ON PR.SRSRCD = PM1.TOTHCD 
                        WHERE SRDNDT >= ? AND SRDNDT <= ?
                        AND SRSBKB = 'PR'
                        AND SRTRKB NOT IN (38, 39, 48, 49)
                        AND PR.SRSRCD <> '{code}'";
            var ps = new List<iDB2Parameter>
            {
                new iDB2Parameter("p1",symd),
                new iDB2Parameter("p2",eymd)
            };

            var dt = dbManager.GetDataTable(sql, ps.ToArray());

            return dt;
        }

        // 当月在庫データ取得
        internal DataTable GetStockData(string file)
        {
            // 移動在庫ファイル
            string sql = $@"SELECT * FROM OHNO000.{file}";
            DataTable dt;

            try
            {
                dt = dbManager.GetDataTable(sql);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        // 過去月在庫データ取得
        internal DataTable GetStockData(string lib, string yy, string mm)
        {
            string sql = $@"
                        SELECT ZGNEND, ZGMOTH, ZGZKSB, M4.SHCLAS, ZGBMCD AS ZHBMCD,
                               ZGWHCD, COALESCE(PM1.TOTHNM, '') AS TOTHNM1, ZGAZCD, COALESCE(PM2.TOTHNM, '') AS TOTHNM2, M5.HNHNSM AS ZHHNNM,
                               ZGHMCD AS ZHHMCD, ZGHSCD AS ZHHSCD, ZGCLCD, ZGTZQT AS ZHTZQT, ZGTGZA AS ZHTGZA
                        FROM {lib}.MOZGETP AS IV
                        LEFT JOIN SM1MLB01.MMSHUP AS M4
                            ON IV.ZGHSCD = M4.SHHSCD
                        LEFT JOIN SM1MLB01.MMHNAMP AS M5
                            ON IV.ZGHBCD = M5.HNHBCD AND IV.ZGHMCD = M5.HNHMCD
                        LEFT JOIN SM1MLB01.MMTORIP AS PM1
                            ON IV.ZGWHCD = PM1.TOTHCD
                        LEFT JOIN SM1MLB01.MMTORIP AS PM2
                            ON IV.ZGAZCD = PM2.TOTHCD
                        WHERE ZGNEND = ? AND ZGMOTH = ?
                        AND (ZGZZQT<> 0 OR ZGNKQS<> 0 
                         OR ZGNKQH<> 0 OR ZGNKQF<> 0 
                         OR ZGNKQK<> 0 OR ZGSKQU<> 0 
                         OR ZGSKQH<> 0 OR ZGSKQF<> 0 
                         OR ZGSKQK<> 0 OR ZGSKQL<> 0)";
            var ps = new List<iDB2Parameter>
            {
                new iDB2Parameter("p1",yy),
                new iDB2Parameter("p2",mm)
            };

            var dt = dbManager.GetDataTable(sql, ps.ToArray());

            return dt;
        }

        internal DataTable GetTorihikiMaster()
        {
            string sql = $@"
                        SELECT *
                        FROM SM1MLB01.MMTORIP";
            var dt = dbManager.GetDataTable(sql);

            return dt;
        }

        internal DataTable GetTorihikiHistory()
        {
            var libraryNames = new[]{
               new []{ "SM1DLB01"},  // オーノ
               new []{ "SM1DLB02", "SM1DLB03"}  // サンミック
            };

            DataTable slDt = null;
            DataTable prDt = null;

            // すべてのライブラリから売上履歴と仕入履歴を取得
            foreach (var libraries in libraryNames)
            {
                foreach (var libraryName in libraries)
                {
                    // 売上履歴があった販売先を部門ごとに取得
                    var slTable = MakeSql(libraryName,"売上");

                    if (slDt == null)
                        slDt = slTable;
                    else
                        slDt.Merge(slTable);

                    // 仕入履歴があった仕入先を部門ごとに取得
                    var prTable = MakeSql(libraryName, "仕入");

                    if (prDt == null)
                        prDt = prTable;
                    else
                        prDt.Merge(prTable);
                }
            }

            // slDtとprDtをマージ、取引先コードでソート、取引先コードと部門コードでユニーク
            if (slDt == null) {
                slDt = prDt.Clone();
            }
                if (prDt == null) {
                    prDt = slDt.Clone();
                }
    
                var dt = slDt.AsEnumerable()
                    .Union(prDt.AsEnumerable())
                    .GroupBy(row => new { TorihikiCode = row.Field<string>("取引先コード"), BumonCode = row.Field<string>("部門コード") })
                    .Select(g => g.First())
                    .OrderBy(row => row.Field<string>("取引先コード"))
                    .CopyToDataTable();

            return dt;
        }

        internal DataTable MakeSql(string libraryName, string kubun)
        {
            string sql;
            if (kubun == "売上")
            {
                sql = $@"
                        SELECT SL.URHBSC, SL.URBMCD, MIN(PM.TOTHNM), MIN(PM.TOKANM)
                        FROM {libraryName}.SLURIMP AS SL
                        LEFT JOIN SM1MLB01.MMTORIP AS PM
                                        ON SL.URHBSC = PM.TOTHCD
                         WHERE SL.URDNDT >= 20030101 
                         GROUP BY SL.URBMCD, SL.URHBSC
                         ORDER BY SL.URBMCD, MIN(PM.TOKANM)
                     ";
            }
            else
            {
                sql = $@"
                        SELECT PR.SRSRCD, PR.SRBMCD, MIN(PM.TOTHNM), MIN(PM.TOKANM)
                        FROM {libraryName}.PRSREMP AS PR
                        LEFT JOIN SM1MLB01.MMTORIP AS PM
                            ON PR.SRSRCD = PM.TOTHCD
                        WHERE PR.SRDNDT >= 20030101 
                        GROUP BY PR.SRBMCD, PR.SRSRCD
                        ORDER BY PR.SRBMCD, MIN(PM.TOKANM)
                     ";
            }
            var table = dbManager.GetDataTable(sql);
            table.Columns[0].ColumnName = "取引先コード";
            table.Columns[1].ColumnName = "部門コード";
            table.Columns[2].ColumnName = "取引先名";
            table.Columns[3].ColumnName = "カナ";

            return table;
        }
    }
}
