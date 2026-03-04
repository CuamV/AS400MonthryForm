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
using あすよん月次帳票.Repositories;

namespace あすよん月次帳票
{
    internal class GetData_AS400
    {
        // クラスフィールドとしてdbManagerを宣言
        private readonly DbManager_Db2 dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);

        // 新しいRepositoryを使用
        private readonly 取引先Repository _取引先Repository;

        // コンストラクタでRepositoryを初期化
        public GetData_AS400()
        {
            _取引先Repository = new 取引先Repository(dbManager); // AS400取引先のマスタ実体を作成
        }

        // ---- 取引先関連Method(新Repositoryに委譲) ----
        internal DataTable GetTorihikiMaster()
        {
            // AS400取引先マスタをRepositoryから取得
            return _取引先Repository.Get取引先マスタ();
        }
        internal DataTable GetTorihikiHistory()
        {
            // AS400取引先履歴をRepositoryから取得
            return _取引先Repository.Get取引先履歴();
        }

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
    }
}
