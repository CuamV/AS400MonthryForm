using IBM.Data.DB2.iSeries;
using Ohno.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace あすよん月次帳票
{
    internal class GetDataMethod
    {
        // 売上データ取得
        public static DataTable GetSalesData(string symd, string eymd, string lib)
        {
            //  1:[6]URDNDT(伝票日付)  2:[1]URBMCD(部門CD)      3:[8]URHBSC(販売先CD) 4:PM1.TOTHNM(販売先名)
            //  5:[4]M4.SHCLAS(クラス) 6:[14]URHBCD(品名部門CD) 7:[15]URHMCD(品名CD)  8:[42]URHNNM(品名)
            //  9:[16]URHSCD(品種CD)  10:[43]URHSNM(品種名)    11:URTRKB(取引区分)   12:[5]URSBKB(サブシステム区分)
            // 13:[20]URQNTY(数量)    14:[21]URUNCD(単位CD)    15:[22]URUNPR(単価)   16:[23]URAMNT(金額)
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
            string sql = $@"
                        SELECT URDNDT, URBMCD, SL.URHBSC, 
                               COALESCE(PM1.TOTHNM, '') AS TOTHNM, 
                               M4.SHCLAS, URHBCD, URHMCD, URHNNM, URHSCD, URHSNM, 
                               URTRKB, URSBKB, URQNTY, URUNCD, URUNPR, URAMNT 
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

            var dt = dbManager.GetDataTable(sql,ps.ToArray());
                
            return dt;
        }

        // 仕入データ取得
        public static DataTable GetPurchaseData(string symd, string eymd, string lib, string code = "")
        {
            //  1:[9]SRDNDT(伝票日付)  2:[1]SRBMCD(部門CD)      3:[3]SRSRCD(仕入先CD) 4:PM1.TOTHNM(販売先名)
            //  5:[4]M4.SHCLAS(クラス) 6:[10]SRHBCD(品名部門CD) 7:[11]SRHMCD(品名CD)  8:[26]SRHNNM(品名)
            //  9:[12]SRHSCD(品種CD)  10:[27]SRHSNM(品種名)    11:SRTRKB(取引区分)   12:[7]SRSBKB(サブシステム区分)
            // 13:[16]SRQNTY(数量)    14:[17]SRUNCD(単位CD)    15:[18]SRUNPR(単価)   16:[19]SRAMNT(金額)
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
            string sql = $@"
                        SELECT SRDNDT, SRBMCD, PR.SRSRCD, 
                               COALESCE(PM1.TOTHNM, '') AS TOTHNM, 
                               M4.SHCLAS, SRHBCD, SRHMCD, SRHNNM, SRHSCD, SRHSNM, 
                               SRTRKB, SRSBKB, SRQNTY, SRUNCD, SRUNPR, SRAMNT 
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
        public static DataTable GetStockData(string file)
        {
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
            
            // 移動在庫ファイル
            // OIZAIKOG [オーノ原材料在庫],OIZAIKOS [オーノ製品在庫],OIZAIKOK [オーノ加工在庫]
            // SCIZAIKOG [サンミックカーペット原材料在庫],SCIZAIKOTH [サンミックカーペットタフト半製品在庫]
            // SCIZAIKOCH [サンミックカーペットコーティング半製品在庫],SCIZAIKOS [サンミックカーペット製品在庫]
            // SDIZAIKOG [サンミックダスコン原材料在庫],SDIZAIKOCH [サンミックダスコンコーティング半製品在庫]
            // SDIZAIKOS [サンミックダスコン製品在庫]
            // -----------------------------------------------------------------------------
            // 1:ZHCSNM(クラス名) 2:ZHBMCD(部門CD) 3:ZHHNNM(品名)
            // 4:ZHHMCD(品名CD)   5:ZHHSCD(品種CD) 6:ZHTZQT(当月残数量) 7:ZHTGZA(当月残金額)
            string sql = $@"SELECT * FROM OHNO000.{file}";

            var dt = dbManager.GetDataTable(sql);

            return dt;
        }

        // 過去月在庫データ取得
        public static DataTable GetStockData(string lib, string yy, string mm)
        {
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);

            // 在庫月次マスタ
            //  1:ZGNEND(年度)            2:ZGMOTH(月)              3:ZGBMCD(部門コード)      4:ZGWHCD(倉庫コード)          5:ZGTNNO(棚番)
            //  6:ZGHBCD(品名部門コード)  7:ZGHMCD(品名コード)	    8:ZGHSCD(品種コード)      9:ZGCLCD(色コード)           10:ZGSZCD(サイズコード)
            // 11:ZGZKSB(在庫種別)       12:ZGAZCD(預り先コード)   13:ZGZZQT(前月残数量)     14:ZGNKQS(入庫数量　仕入)     15:ZGNKQH(入庫数量　引受)	
            // 16:ZGNKQF(入庫数量　振替) 17:ZGNKQK(入庫数量　加工) 18:ZGSKQU(出庫数量　売上) 19:ZGSKQH(出庫数量　引渡)     20:ZGSKQF(出庫数量　振替)	
            // 21:ZGSKQK(出庫数量　加工) 22:ZGSKQL(出庫数量　ロス) 23:ZGTZQT(当月残数量)     24:ZGZGZA(前月残金額)         25:ZGNKAG(入庫金額　仕入原価)
            // 26:ZGNKAH(入庫金額　引受) 27:ZGNKAF(入庫金額　振替) 28:ZGNKAK(入庫金額　加工) 29:ZGSKAG(出庫金額　売上原価) 30:ZGSKAH(出庫金額　引渡)
            // 31:ZGSKAF(出庫金額　振替) 32:ZGSKAK(出庫金額　加工) 33:ZGSKAL(出庫金額　ロス) 34:ZGTGZA(当月残金額)         35:ZGSRAM(仕入金額)
            // 36:ZGURAM(売上金額)       37:ZGHWAM(引渡金額)       38:ZGHWAR(引渡粗利)       39:ZGURAR(売上粗利)           40:ZGADAT(作成日)
            // 41:ZGATIM(作成時刻)       42:ZGUDAT(更新日)         43:ZGUTIM(更新時刻)       44:ZGUPGM(更新ＰＧＭ)
            //-----------------------------------------------------------------------------
            // 1:クラス 2:ZGBMCD(部門コード) 3:品名 4:ZGHMCD(品名コード) 5:ZGHSCD(品種コード)
            // 6:ZGTZQT(当月残数量) 7:ZGTGZA(当月残金額) ZHTZQT
            string sql = $@"
                        SELECT M4.SHCLAS AS ZHCSNM, ZGBMCD AS ZHBMCD,
                               M5.HNHNSM AS ZHHNNM, ZGHMCD AS ZHHMCD, ZGHSCD AS ZHHSCD, 
                               ZGTZQT AS ZHTZQT, ZGTGZA AS ZHTGZA
                        FROM {lib}.MOZGETP AS IV
                        LEFT JOIN SM1MLB01.MMSHUP AS M4
                            ON IV.ZGHSCD = M4.SHHSCD
                        LEFT JOIN SM1MLB01.MMHNAMP AS M5
                            ON IV.ZGHBCD = M5.HNHBCD AND IV.ZGHMCD = M5.HNHMCD
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
