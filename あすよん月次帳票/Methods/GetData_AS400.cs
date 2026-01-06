using IBM.Data.DB2.iSeries;
using Ohno.Db;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using DCN = あすよん月次帳票.Dictionaries;

namespace あすよん月次帳票
{
    internal class GetData_AS400
    {
        // 売上データ取得
        internal DataTable GetSalesData(string symd, string eymd, string lib)
        {
            //  1:[2]URNHNO(納品書No)   2:[3]URNHEB(納品書枝番    3:[5]URSBKB(サブシステム区分) 4:URTRKB(取引区分)
            //  5:[6]URDNDT(伝票日付)   6:[1]URBMCD(部門CD)       7:[4]M4.SHCLAS(クラス)        8:[8]URHBSC(販売先CD)
            //  9:PM1.TOTHNM(販売先名) 10:[14]URHBCD(品名部門CD) 11:[15]URHMCD(品名CD)         13:[16]URHSCD(品種CD)
            //  15:[17]URCLCD(色CD)　　12:[42]URHNNM(品名)       14:[43]URHSNM(品種名)         16:[44]URCLNM(色名)         
            // 17:[20]URQNTY(数量)   18:[21]URUNCD(単位CD)       19:[22]URUNPR(単価)           20:[23]URAMNT(金額)
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
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
            //  1:[5]SRSRNO(仕入No)     2:[6]SRSREB(仕入枝番)     3:[7]SRSBKB(サブシステム区分) 4:SRTRKB(取引区分)
            //  5:[9]SRDNDT(伝票日付)   6:[1]SRBMCD(部門CD)       7:[4]M4.SHCLAS(クラス)        8:[3]SRSRCD(仕入先CD)
            //  9:PM1.TOTHNM(仕入先名) 10:[10]SRHBCD(品名部門CD) 11:[11]SRHMCD(品名CD)         12:[12]SRHSCD(品種CD)   
            // 13:[13]SRCLCD(色CD)     14:[26]SRHNNM(品名)       15:[27]SRHSNM(品種名)         16:[28]SRCLNM(色名)        
            // 17:[16]SRQNTY(数量)     18:[17]SRUNCD(単位CD)     19:[18]SRUNPR(単価)           20:[19]SRAMNT(金額)
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
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
            //  1:ZGNEND(年度)       2:ZGMOTH(月)           3:ZGZKSB(在庫種別)     4:SHCLAS(クラス)      5:ZGBMCD(部門コード)
            //  6:ZGWHCD(倉庫コード) 7:TOTHNM1(倉庫名)      8:ZGAZCD(預り先コード) 9:TOTHNM2(預り先名)  10:ZHHNNM(品名)
            // 11:ZHHMCD(品名コード) 12:ZHHSCD(品種コード) 13:ZGCLCD(色コード)    14:ZGTZQT(当月残数量) 15:ZGTGZA(当月残金額) 
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
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
            // 取引先マスタ
            //  1:TOTHCD(取引先コード)      2:TOTHSM(取引先正式名)    3:TOTHNM(取引先名)            4:TOABNM(略名)                5:TOKANM(カナ名)          6:TOPSCD(郵便番号)
            //  7:TOPHNO(電話番号)          8:TOFAX(FAX番号)          9:TOADR1(住所１)             10:TOADR2(住所２)             11:TOKPSN(代表者)         12:TOCPTL(資本金)
            // 13:TOTHBM(取引先部門)       14:TOTHTT(取引先担当者)   15:TOTSDT(取引開始日)         16:TOHBKB(販売先区分)         17:TOSRKB(仕入先区分)     18:TOSKKB(出荷先区分)
            // 19:TOUSKB(運送便区分)       20:TOWHKB(倉庫区分)       21:TOSJKB(指示書発行倉庫区分) 22:TOSUKB(消費税有無区分)     23:TOSCKB(消費税計算区分) 24:TOQCKB(数量計算区分)
            // 25:TOAMCK(金額計算区分)     26:TONHSK(納品書出力区分) 27:TONHSG(納品書出力行数)     28:TOATCD(相手先取引先コード) 29:TOKMDT(買掛用締日)     30:TOKSDT(買掛用支払日)
            // 31:TOKTGS(買掛用手形サイト) 32:TOBNCD(銀行コード)     33:TOBRCD(銀行支店コード)     34:TOKZNO(口座番号)           35:TOKZKB(口座区分)       36:TOKZNM(口座名義)
            // 37:TOTTC1(担当者コード１)   38:TOTTC2(担当者コード２) 39:TOTTC3(担当者コード３)     40:TOUSC1(運送便コード１)     41:TOUSC2(運送便コード２) 42:TOUSC3(運送便コード３)
            // 43:TOYSG1(与信限度額１)     44:TOYSG2(与信限度額２)   45:TOYSG3(与信限度額３)       46:TONHKB(納品書発行区分)     47:TOBLKB(請求書発行区分) 48:TOAHKB(出荷案内書発行区分)
            // 49:TOOHKB(送り状発行区分)   50:TOTHKT(反番表示桁数)   51:TOSNKB(社内加工区分)       52:TOSJMK(削除ＭＫ)           53:TOADAT(作成日)         54:TOATIM(作成時刻)
            // 55:TOUDAT(更新日)           56:TOUTIM(更新時刻)       57:TOUPGM(更新ＰＧＭ)
            string sql = $@"
                        SELECT *
                        FROM SM1MLB01.MMTORIP";
            var dt = dbManager.GetDataTable(sql);

            return dt;
        }

        /// <summary>
        /// ファイル名から在庫種別名を取得
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>在庫種別名（会社名 + 在庫種別）</returns>
        private string GetStockTypeNameFromFile(string fileName)
        {
            // fileMapから該当するエントリを検索
            foreach (var companyEntry in DCN.fileMap)
            {
                string companyName = companyEntry.Key;
                var stockTypes = companyEntry.Value;

                foreach (var stockType in stockTypes)
                {
                    if (stockType.Value == fileName)
                    {
                        return $"{companyName} {stockType.Key}";
                    }
                }
            }

            return string.Empty;
        }
    }
}
