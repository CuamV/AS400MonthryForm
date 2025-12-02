using Ohno.Db;
using OHNO.PComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace あすよん月次帳票
{

    internal class FormActionMethod
    {
        static public string ctl = "[enter]";  // Ctlr(実行)
        static public string f3 = "[pf3]";  // F3(終了)
        static public string tb = "[tab]"; // Tab(カーソル次送り)
        static public string ent = "[fldext]"; // Enter(カーソル位置以降の入力exit)

        private static string HIZ = DateTime.Now.ToString("yyyyMMdd");

        private static List<string> runtimelog = new List<string>();

        // =======================================================================
        // 【マスターデータ取得メソッド】
        //  <Form1>
        //   ◆販売先マスター取得
        public static Dictionary<string, List<mf_HANBAI>> GetHanbaiAll(string lib)
        {
            var result = new Dictionary<string, List<mf_HANBAI>>();
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);

            var cmdText = $@"
                        SELECT SL.URBMCD, SL.URHBSC, MIN(PM.TOTHNM), MIN(PM.TOKANM)
                        FROM {lib}.SLURIMP AS SL
                        LEFT JOIN SM1MLB01.MMTORIP AS PM ON SL.URHBSC = PM.TOTHCD
                        WHERE SL.URDNDT >= 20030701
                        GROUP BY SL.URBMCD, SL.URHBSC
                        ORDER BY SL.URBMCD, MIN(PM.TOKANM)";

            var dTable = dbManager.GetDataTable(cmdText);

            foreach (DataRow row in dTable.Rows)
            {
                string bumon = row["URBMCD"].ToString();
                string code = row["URHBSC"].ToString();
                string name = row[2].ToString();
                string kana = row[3].ToString();

                if (!result.ContainsKey(bumon))
                    result[bumon] = new List<mf_HANBAI>();

                result[bumon].Add(new mf_HANBAI
                {
                    Code = code,
                    Name = name,
                    Kana = kana
                });
            }
            return result;
        }
        //   ◆仕入先マスター取得
        public static Dictionary<string, List<mf_SHIIRE>> GetShiireAll(string lib)
        {
            var result = new Dictionary<string, List<mf_SHIIRE>>();
            var dbManager = (DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2);
            var cmdText = $@"
                        SELECT PR.SRBMCD, PR.SRSRCD, MIN(PM.TOTHNM), MIN(PM.TOKANM)
                        FROM {lib}.PRSREMP AS PR
                        LEFT JOIN SM1MLB01.MMTORIP AS PM ON PR.SRSRCD = PM.TOTHCD
                        WHERE PR.SRDNDT >= 20030701
                        GROUP BY PR.SRBMCD, PR.SRSRCD
                        ORDER BY PR.SRBMCD, MIN(PM.TOKANM)";

            var dTable = dbManager.GetDataTable(cmdText);

            foreach (DataRow row in dTable.Rows)
            {
                string bumon = row["SRBMCD"].ToString();
                string code = row["SRSRCD"].ToString();
                string name = row[2].ToString();
                string kana = row[3].ToString();

                if (!result.ContainsKey(bumon))
                    result[bumon] = new List<mf_SHIIRE>();

                result[bumon].Add(new mf_SHIIRE
                {
                    Code = code,
                    Name = name,
                    Kana = kana
                });
            }
            return result;
        }
        // 【JSON操作メソッド】
        ///  <Form1>
        ///   ◆JSONファイル保存
        public static void SaveToJson<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        // =======================================================================

        // =======================================================================
        // 【エラーチェックメソッド】
        //  <RplForm2>
        //   ◆年月入力チェック(yyyyMM形式)
        public static bool TryParseYearMonth(TextBox txtBox, out int year, out int month)
        {
            year = 0;
            month = 0;

            string input = txtBox.Text.Trim();

            // yyyyMM 6桁の数字かチェック
            if (!Regex.IsMatch(input, @"^\d{6}$"))
                return false;

            year = int.Parse(input.Substring(0, 4));
            month = int.Parse(input.Substring(4, 2));

            // 月が1～12かチェック
            if (month < 1 || month > 12)
                return false;

            return true;
        }
        // =======================================================================

        // =======================================================================
        // 【条件選択取得メソッド】
        //  <RplForm2>
        //   ◆帳票名選択
        public static string GetBookName(TextBox txtBox)
        {
            return txtBox.Text.Trim();
        }
        //   ◆年月選択
        public static (string startDate, string endDate) GetStartEndDate(int syear, int smonth, int eyear, int emonth)
        {
            string startDate = new DateTime(syear, smonth, 1).ToString("yyyyMMdd");
            int lastDay = DateTime.DaysInMonth(eyear, emonth);
            string endDate = new DateTime(eyear, emonth, lastDay).ToString("yyyyMMdd");
            return (startDate, endDate);
        }
        //   ◆会社選択
        public static List<string> GetCompany(CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            var selectedCompanies = new List<string>();
            if (chkBxOhno.Checked) selectedCompanies.Add("オーノ");
            if (chkBxSundus.Checked) selectedCompanies.Add("サンミックダスコン");
            if (chkBxSuncar.Checked) selectedCompanies.Add("サンミックカーペット");
            return selectedCompanies;
        }
        //   ◆部門選択
        public static List<string> GetSelectedBumons(ListBox listBxB)
        {
            var result = new List<string>();

            foreach (var item in listBxB.Items)
            {
                switch (item) 
                {
                    case Torihiki t:
                        result.Add(t.DeptCode); break;
                    case Department d:
                        result.Add(d.Code); break;
                }
            }
            return result;
        }
        //   ◆販売先/仕入先選択
        public static List<string> GetSallerOrSupplier(ListBox listbx)
        {
            var result = new List<string>();
            foreach (var item in listbx.Items)
            {
                result.Add(((Torihiki)item).Code);
            }
            return result;
        }
        //   ◆データ区分選択
        public static List<string> GetSalseProduct(CheckBox chkBxSl, CheckBox chkBxPr, CheckBox chkBxIv)
        {
            var selectedSlProduct = new List<string>();
            if (chkBxSl.Checked) selectedSlProduct.Add("売上");
            if (chkBxPr.Checked) selectedSlProduct.Add("仕入");
            if (chkBxIv.Checked) selectedSlProduct.Add("在庫");
            return selectedSlProduct;
        }
        //   ◆クラス区分選択
        //    ＊売上・仕入
        public static List<string> GetProduct(CheckBox chkBxRawMaterials, CheckBox chkBxSemiFinProducts, CheckBox chkBxProduct,
                                              CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            var selectedSlPrProduct = new List<string>();

            if (chkBxRawMaterials.Checked) selectedSlPrProduct.Add("原材料");
            if (chkBxSemiFinProducts.Checked) selectedSlPrProduct.Add("半製品");
            if (chkBxProduct.Checked) selectedSlPrProduct.Add("製品");

            return selectedSlPrProduct;
        }
        //    ＊在庫
        public static List<string> GetProduct(CheckBox chkBxRawMaterials, CheckBox chkBxSemiFinProducts, CheckBox chkBxProduct,
                                              CheckBox chkBxProcess, CheckBox chkBxCustody, CheckBox chkEntrust,
                                              CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            var selectedIvProduct = new List<string>();

            if (chkBxRawMaterials.Checked) selectedIvProduct.Add("原材料");
            if (chkBxSuncar.Checked && chkBxSundus.Checked && chkBxSemiFinProducts.Checked)
            {
                selectedIvProduct.Add("タフト半製品");
                selectedIvProduct.Add("コーティング半製品");
            }
            else if (chkBxSuncar.Checked && chkBxSemiFinProducts.Checked)
            {
                selectedIvProduct.Add("タフト半製品");
                selectedIvProduct.Add("コーティング半製品");
            }
            else if (chkBxSundus.Checked && chkBxSemiFinProducts.Checked) selectedIvProduct.Add("コーティング半製品");

            if (chkBxProduct.Checked) selectedIvProduct.Add("製品");
            if (chkBxProcess.Checked) selectedIvProduct.Add("加工在庫");
            if (chkBxCustody.Checked) selectedIvProduct.Add("預り在庫");
            if (chkEntrust.Checked) selectedIvProduct.Add("預け在庫");

            if (selectedIvProduct.Count > 0)
                return selectedIvProduct;

            if (chkBxOhno.Checked)
            {
                selectedIvProduct.Add("原材料");
                selectedIvProduct.Add("製品");
                selectedIvProduct.Add("加工在庫");
                selectedIvProduct.Add("預り在庫");
                selectedIvProduct.Add("預け在庫");
            }
            if (chkBxSundus.Checked)
            {
                selectedIvProduct.Add("原材料");
                selectedIvProduct.Add("コーティング半製品");
                selectedIvProduct.Add("製品");
                selectedIvProduct.Add("加工在庫");
                selectedIvProduct.Add("預り在庫");
                selectedIvProduct.Add("預け在庫");
            }
            if (chkBxSuncar.Checked)
            {
                selectedIvProduct.Add("原材料");
                selectedIvProduct.Add("タフト半製品");
                selectedIvProduct.Add("コーティング半製品");
                selectedIvProduct.Add("製品");
                selectedIvProduct.Add("加工在庫");
                selectedIvProduct.Add("預り在庫");
                selectedIvProduct.Add("預け在庫");
            }
            // selectedIvProductに重複がある場合は削除
            return selectedIvProduct.Distinct().ToList();
        }
        //   ◆在庫種別選択
        public static Dictionary<string, string> GetIvType(CheckBox chkBxOneCom, CheckBox chkBxCustody, CheckBox chkBxEntrust, CheckBox chkBxProcess)
        {
            // 0=自社,1=預り,2=預け,3=投入
            var selIvType = new Dictionary<string, string>();

            if (chkBxOneCom.Checked) selIvType["0"] = "自社";
            if (chkBxCustody.Checked) selIvType["1"] = "預り";
            if (chkBxEntrust.Checked) selIvType["2"] = "預け";
            if (chkBxProcess.Checked) selIvType["3"] = "投入";

            return selIvType;
        }
        //   ◆売上・仕入・在庫集計区分選択
        public static string GetAggregte(GroupBox grpBox)
        {
            return grpBox.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.Checked)?
                .Tag?.ToString();
        }
        // =======================================================================

        // =======================================================================
        // 【データ取得メソッド】
        //  <RplForm2>
        //   ◆売上・仕入データ取得
        public static DataTable MakeReadData_SLPR(string startDate, string endDate, string company, string kubun)

        {
            DataTable dt = new DataTable();
            // 各データ取得(売上,仕入)
            if (company == "オーノ" && kubun == "SL")
                dt = GetDataMethod.GetSalesData(startDate, endDate, "SM1DLB01");  // オーノ(売上)
            else if (company == "サンミックカーペット" && kubun == "SL")
                dt = GetDataMethod.GetSalesData(startDate, endDate, "SM1DLB03");  // サンミック カーペット(売上)
            else if (company == "サンミックダスコン" && kubun == "SL")
                dt = GetDataMethod.GetSalesData(startDate, endDate, "SM1DLB02");  // サンミック ダスコン(売上)
            else if (company == "オーノ" && kubun == "PR")
                dt = GetDataMethod.GetPurchaseData(startDate, endDate, "SM1DLB01");  // オーノ(仕入)
            else if (company == "サンミックカーペット" && kubun == "PR")
                dt = GetDataMethod.GetPurchaseData(startDate, endDate, "SM1DLB03", "0001576");  // サンミック カーペット(仕入)
            else if (company == "サンミックダスコン" && kubun == "PR") 
                dt = GetDataMethod.GetPurchaseData(startDate, endDate, "SM1DLB02", "0000009");  // サンミック ダスコン(仕入)
            return dt;
        }
        //   ◆在庫データ取得
        public static (DataTable, DataTable) MakeReadData_IV(string startDate, string endDate, string company, List<string> selIvProducts)
        {
            string monthlyFile = @"\\ohnosv01\OhnoSys\099_sys\mf\Monthly.txt";
            string firstLine = File.ReadLines(monthlyFile).FirstOrDefault();
            string currentYm = firstLine.Substring(0, 6); // 先頭6文字を取得(当月)

            string startYM = startDate.Substring(0, 6);

            DataTable dtNow = null;
            DataTable dtOld = null;
            if (startYM == currentYm)
            {
                // 今月分の在庫データ取得(ライブラリより取得)
                // 会社ごと＋品目ごとのファイルマッピング
                var fileMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
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

                // 選択された品目のファイル名リストを作る
                var files = selIvProducts
                    .Where(p => fileMap[company].ContainsKey(p))
                    .Select(p => fileMap[company][p])
                    .ToList();

                var processor = new DataProcessor();
                var stockList = new List<DataTable>();

                foreach (var file in files)
                {
                    // 1.SQLで在庫データ取得
                    dtNow = GetDataMethod.GetStockData(file);

                    if (dtNow == null) continue;

                    // 2.カテゴリ列追加+コード列作成+欠損値０埋め
                    dtNow = processor.ProsessStockTable(dtNow, file);
                    stockList.Add(dtNow);
                }
                //==============================================================
                // 1:ZHCSNM(クラス名) 2:ZHBMCD(部門CD) 3:ZHHNNM(品名)
                // 4:ZHHMCD(品名CD)   5:ZHHSCD(品種CD) 6:ZHTZQT(当月残数量) 7:ZHTGZA(当月残金額)
                //==============================================================
                dtNow = processor.MergeData(stockList.ToArray());

                //==============================================================
                // 1:年月           2:ZHCSNM(クラス名) 3:ZHBMCD(部門CD)     4:ZHHNNM(品名)
                // 5:ZHHMCD(品名CD) 6:ZHHSCD(品種CD)   7:ZHTZQT(当月残数量) 8:ZHTGZA(当月残金額)
                //==============================================================
                dtNow = AddYearMonthColum(dtNow, startYM);

            }
            else
            {
                // 過去月分の在庫データ取得(在庫月次マスタより取得)
                string lib = null;

                switch (company)
                {
                    case "オーノ":
                        lib = "SM1DLB01";
                        break;
                    case "サンミックダスコン":
                        lib = "SM1DLB02";
                        break;
                    case "サンミックカーペット":
                        lib = "SM1DLB03";
                        break;
                }

                // まずループ用の DateTime に変換
                DateTime start = DateTime.ParseExact(startDate, "yyyyMMdd", null);
                DateTime end = DateTime.ParseExact(endDate, "yyyyMMdd", null);

                DataTable IVdataAll = null;

                for (DateTime dtMonth = start; dtMonth <= end; dtMonth = dtMonth.AddMonths(1))
                {
                    string yy = dtMonth.Year.ToString();
                    string mm = dtMonth.Month.ToString("D2");
                    var IVdata = GetDataMethod.GetStockData(lib, yy, mm);

                    if (IVdata != null && IVdata.Rows.Count > 0)
                    {
                        // クラスコード → 日本語変換
                        Dictionary<string, string> classMap = new Dictionary<string, string>
                        {
                            { "1", "原材料" },
                            { "2", "タフト半製品" },
                            { "3", "コーティング半製品" },
                            { "4", "製品" },
                        };

                        foreach (DataRow r in IVdata.Rows)
                        {
                            string code = r["SHCLAS"]?.ToString()?.Trim();
                            if (classMap.ContainsKey(code))
                            {
                                r["SHCLAS"] = classMap[code];
                            }
                            else
                            {
                                // マッピングが見つからない場合はコードをそのまま使用
                            }
                        }

                        // 縦連結
                        if (IVdataAll == null)
                            IVdataAll = IVdata.Clone(); // スキーマコピー
                        foreach (DataRow r in IVdata.Rows)
                            IVdataAll.ImportRow(r);
                    }
                }
                //==============================================================
                //  1:ZGZKSB(在庫種別)     2:SHCLAS(クラス)      3:ZGBMCD(部門コード) 4:ZGWHCD(倉庫コード)  5:倉庫名
                //  6:ZGAZCD(預り先コード) 7:預り先名            8:品名               9:ZGHMCD(品名コード) 10:ZGHSCD(品種コード)
                // 11:ZGCLCD(色コード)    12:ZGTZQT(当月残数量) 13:ZGTGZA(当月残金額) 
                //==============================================================
                dtOld = IVdataAll ?? new DataTable(); // データなしは空テーブル

                //==============================================================
                //  1:年月                2:ZGZKSB(在庫種別)     3:SHCLAS(クラス)      4:ZGBMCD(部門コード)  5:ZGWHCD(倉庫コード)
                //  6:倉庫名              7:ZGAZCD(預り先コード) 8:預り先名            9:品名               10:ZHHMCD(品名コード)
                // 11:ZHHSCD(品種コード) 12:ZGCLCD(色コード)    13:ZGTZQT(当月残数量) 14:ZGTGZA(当月残金額) 
                //==============================================================
                dtOld = AddYearMonthColum(dtOld, startYM);
            }
            return (dtNow,dtOld);
        }
        //   ◆年月列追加共通処理
        public static DataTable AddYearMonthColum(DataTable dt, string startYM)
        {
            // 🔸 共通：年月列を追加
            if (dt != null && !dt.Columns.Contains("年月"))
            {
                dt.Columns.Add("年月", typeof(string));
                foreach (DataRow r in dt.Rows)
                    r["年月"] = startYM;
            }
            // 不要列を削除
            if (dt.Columns.Contains("ZGNEND")) dt.Columns.Remove("ZGNEND");
            if (dt.Columns.Contains("ZGMOTH")) dt.Columns.Remove("ZGMOTH");

            // 年月列を先頭へ移動
            dt.Columns["年月"].SetOrdinal(0);

            return dt;
        }
        //   ◆フィルター処理
        public static (DataTable, DataTable, DataTable) FilterData(string startDate, string endDate,
                                                                   List<string> selCompanies, List<string> selBumons,
                                                                   List<string> selSelleres, List<string> selSupplieres,
                                                                   List<string> selSlCategories, List<string> selSlPrProducts, List<string> selIvProducts,
                                                                   Dictionary<string,string> selIvTypes)
        {
            DataProcessor processor = new DataProcessor();

            // 各データ取得
            DataTable ohnoSales = null, ohnoPurchase = null, ohnoStockNow = null, ohnoStockOld = null;
            DataTable suncarSales = null, suncarPurchase = null, suncarStockNow = null, suncarStockOld = null;
            DataTable sundusSales = null, sundusPurchase = null, sundusStockNow = null, sundusStockOld = null;

            var classProduct = new Dictionary<string, string>
                            {
                                { "1", "原材料" },
                                { "2", "半製品" },
                                { "3", "半製品" },
                                { "4", "製品" },
                            };

            DataTable ohnoDt = null;
            DataTable suncarDt = null;
            DataTable sundusDt = null;
            DataTable stockDtNow = null;
            DataTable stockDtOld = null;

            // ★売上データ
            if (selSlCategories.Contains("売上"))
            {
                foreach (var company in selCompanies)
                {
                    if (company == "オーノ") ohnoSales = MakeReadData_SLPR(startDate, endDate, company, "SL");
                    else if (company == "サンミックカーペット") suncarSales = MakeReadData_SLPR(startDate, endDate, company, "SL");
                    else if (company == "サンミックダスコン") sundusSales = MakeReadData_SLPR(startDate, endDate, company, "SL");
                }

                var datasetsS = new[]
                {
                    new { Name = "オーノ", Table = ohnoSales },
                    new { Name = "サンミックカーペット", Table = suncarSales },
                    new { Name = "サンミックダスコン", Table = sundusSales }
                };

                // ▼▼条件フィルター

                foreach (var d in datasetsS)
                {
                    if (d.Table == null) continue;

                    DataTable filtered = d.Table;
                    // 販売先
                    if (selSelleres.Count > 0)
                    {
                        filtered = processor.CustFilter(filtered, d.Table, selSelleres, "URHBSC");
                    }

                    // クラス区分選択
                    if (selSlPrProducts.Count > 0 && selSlPrProducts.Count < 3)
                    {
                        filtered = processor.ProductFileter(filtered, classProduct, selSlPrProducts);
                    }

                    // 部門選択
                    if (d.Name == "オーノ" && selBumons.Count > 0)
                    {
                        filtered = processor.BumonFilter(filtered, selBumons, "URBMCD");
                    }

                    var salesList = new List<DataTable>();
                    if (ohnoSales != null) salesList.Add(ohnoSales);
                    if (suncarSales != null) salesList.Add(suncarSales);
                    if (sundusSales != null) salesList.Add(sundusSales);

                    if (d.Name == "オーノ") ohnoSales = filtered;
                    else if (d.Name == "サンミックカーペット") suncarSales = filtered;
                    else if (d.Name == "サンミックダスコン") sundusSales = filtered;
                }
            }

            // ★仕入データ
            if (selSlCategories.Contains("仕入"))
            {
                foreach (var company in selCompanies)
                {
                    if (company == "オーノ") ohnoPurchase = MakeReadData_SLPR(startDate, endDate, company, "PR");
                    else if (company == "サンミックカーペット") suncarPurchase = MakeReadData_SLPR(startDate, endDate, company, "PR");
                    else if (company == "サンミックダスコン") sundusPurchase = MakeReadData_SLPR(startDate, endDate, company, "PR");
                }

                var datasetsP = new[]
                {
                    new { Name = "オーノ", Table = ohnoPurchase },
                    new { Name = "サンミックカーペット", Table = suncarPurchase },
                    new { Name = "サンミックダスコン", Table = sundusPurchase }
                };

                // ▼▼条件フィルター
                foreach (var d in datasetsP)
                {
                    if (d.Table == null) continue;

                    DataTable filtered = d.Table;
                    // 仕入先選択
                    if (selSupplieres.Count > 0)
                    {
                        filtered = processor.CustFilter(filtered, d.Table, selSupplieres, "SRSRCD");
                    }

                    // クラス区分選択
                    if (selSlPrProducts.Count > 0 && selSlPrProducts.Count < 3)
                    {
                        filtered = processor.ProductFileter(filtered, classProduct, selSlPrProducts);
                    }

                    // 部門選択
                    if (d.Name == "オーノ" && selBumons.Count > 0)
                    {
                        filtered = processor.BumonFilter(filtered, selBumons, "SRBMCD");
                    }

                    var purchaseList = new List<DataTable>();
                    if (ohnoPurchase != null) purchaseList.Add(ohnoPurchase);
                    if (suncarPurchase != null) purchaseList.Add(suncarPurchase);
                    if (sundusPurchase != null) purchaseList.Add(sundusPurchase);

                    if (d.Name == "オーノ") ohnoPurchase = filtered;
                    else if (d.Name == "サンミックカーペット") suncarPurchase = filtered;
                    else if (d.Name == "サンミックダスコン") sundusPurchase = filtered;
                }
            }

            // 売上・仕入データ結合
            var datasetsSP = new[]
            {
                    new { Name = "オーノ", Sales = ohnoSales, Purchase = ohnoPurchase },
                    new { Name = "サンミックカーペット", Sales = suncarSales, Purchase = suncarPurchase },
                    new { Name = "サンミックダスコン", Sales = sundusSales, Purchase = sundusPurchase }
            };

            foreach (var d in datasetsSP)
            {
                DataTable result = null;
                if (d.Sales != null && d.Purchase != null)
                {
                    result = processor.MergeSalesPurchase(d.Sales, d.Purchase, false);
                }
                else if (d.Sales != null)
                {
                    var dt = processor.NormalizeColumnNames(d.Sales, "Sales");
                    result = processor.SortData(dt);
                }
                else if (d.Purchase != null)
                {
                    var dt = processor.NormalizeColumnNames(d.Purchase, "Purchase");
                    result = processor.SortData(dt);
                }
                if (result != null)
                {
                    if (d.Name == "オーノ") ohnoDt = result;
                    else if (d.Name == "サンミックカーペット") suncarDt = result;
                    else if (d.Name == "サンミックダスコン") sundusDt = result;
                }
            }
            var SlPrList = new List<DataTable>();
            if (ohnoDt != null) SlPrList.Add(ohnoDt);
            if (suncarDt != null) SlPrList.Add(suncarDt);
            if (sundusDt != null) SlPrList.Add(sundusDt);

            DataTable slprResult = null;
            if (SlPrList.Count > 0)
            {
                slprResult = processor.MergeData(SlPrList.ToArray());

                DataView dv = slprResult.DefaultView;
                dv.Sort = "伝票No ASC, 枝番 ASC";
                slprResult = dv.ToTable();
            }

            //在庫
            if (selSlCategories.Contains("在庫"))
            {
                foreach (var company in selCompanies)
                {
                    if (company == "オーノ") (ohnoStockNow,ohnoStockOld) = MakeReadData_IV(startDate, endDate, company, selIvProducts);
                    else if (company == "サンミックカーペット") (suncarStockNow,ohnoStockOld) = MakeReadData_IV(startDate, endDate, company, selIvProducts);
                    else if (company == "サンミックダスコン") (sundusStockNow,ohnoStockOld) = MakeReadData_IV(startDate, endDate, company, selIvProducts);
                }

                var datasetsINow = new[]
                {
                    new { Name = "オーノ", Table = ohnoStockNow },
                    new { Name = "サンミックカーペット", Table = suncarStockNow },
                    new { Name = "サンミックダスコン", Table = sundusStockNow },
                };
                var datasetsIOld = new[]
                {
                    new { Name = "オーノ", Table = ohnoStockOld },
                    new { Name = "サンミックカーペット", Table = suncarStockOld },
                    new { Name = "サンミックダスコン", Table = sundusStockOld }
                };

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
                // ▼▼条件フィルター
                if (ohnoStockNow != null)
                {
                    foreach (var d in datasetsINow)
                    {
                        var current = d.Table;
                        if (current == null) continue;
                        DataTable filtered = current;

                        // 部門選択
                        if (d.Name == "オーノ" && selBumons.Count > 0)
                        {
                            filtered = processor.BumonFilter(filtered, selBumons, "ZHBMCD");
                        }

                        if (d.Name == "オーノ") ohnoStockNow = filtered;
                        else if (d.Name == "サンミックカーペット") suncarStockNow = filtered;
                        else if (d.Name == "サンミックダスコン") sundusStockNow = filtered;
                    }
                }

                if (ohnoStockOld != null)
                {
                    foreach (var d in datasetsIOld)
                    {
                        var current = d.Table;
                        if (current == null) continue;
                        DataTable filtered = current;

                        //クラス区分選択
                        if (selIvProducts.Count > 0)
                            filtered = processor.ProductFileter(filtered, selIvProducts);

                        // 在庫種別選択
                        if (selIvTypes.Count > 0)
                            filtered = processor.IvTypeFilter(filtered, selIvTypes);

                        // 部門選択
                        if (d.Name == "オーノ" && selBumons.Count > 0)
                        {
                            filtered = processor.BumonFilter(filtered, selBumons, "ZHBMCD");
                        }

                        if (d.Name == "オーノ") ohnoStockOld = filtered;
                        else if (d.Name == "サンミックカーペット") suncarStockOld = filtered;
                        else if (d.Name == "サンミックダスコン") sundusStockOld = filtered;
                    }
                }
            }
            stockDtNow = MargeAndFormat_StockData(ohnoStockNow, suncarStockNow, sundusStockNow, false);
            stockDtOld = MargeAndFormat_StockData(ohnoStockOld, suncarStockOld, sundusStockOld, true);

            return(slprResult,stockDtNow, stockDtOld);
        }
        public static DataTable MargeAndFormat_StockData(DataTable ohnoStock,DataTable suncarStock, DataTable sundusStock, bool newold)
        {
            DataTable stockDt = null;
            var stockList = new List<DataTable>();
            DataProcessor processor = new DataProcessor();

            if (ohnoStock != null) stockList.Add(ohnoStock);
            if (suncarStock != null) stockList.Add(suncarStock);
            if (sundusStock != null) stockList.Add(sundusStock);

            if (stockList.Count > 0)
            {
                stockDt = processor.MergeData(stockList.ToArray());　
                if (newold)
                    stockDt = processor.FormatStockTable(stockDt, newold);
                else
                    stockDt = processor.FormatStockTable(stockDt);
            }
            return stockDt;
        }
        
        // =======================================================================

        // =======================================================================
        // 【シュミレーションメソッド】
        //  <Form3>
        //   ◆シュミレーションロックチェック
        public static bool CheckAndLockSimulation(string currentUID, string lockFilePath, string LogFilePath, int lockMinutes)
        {
            string logFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_Simulation.txt");
            string AllLogFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_AllSimulation.txt");
            try
            {
                // ロックファイルのディレクトリがなければ作成
                Directory.CreateDirectory(Path.GetDirectoryName(lockFilePath));

                // ファイルが存在しない＝実施者なし→新規作成してロックし、処理実施
                if (!File.Exists(lockFilePath))
                {
                    WriteLockFile(lockFilePath, currentUID);
                    AppendLog(logFilePath, $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] LOCKED by {currentUID}");
                    return true;
                }
                // ファイルが存在する＝他のユーザーが使用中(ロック中)
                var lines = File.ReadAllLines(lockFilePath);
                string lockUser = lines.FirstOrDefault(l => l.StartsWith("UserID="))?.Split('=')[1];
                string timeStr = lines.FirstOrDefault(l => l.StartsWith("StartTime="))?.Split('=')[1];

                if (DateTime.TryParse(timeStr, out DateTime lockTime))
                {
                    var elapsed = DateTime.Now - lockTime;

                    // 同一ユーザーなら上書きしてOK
                    if (lockUser == currentUID)
                    {
                        WriteLockFile(lockFilePath, currentUID);
                        AppendLog(logFilePath, $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] RE-LOCKED by same user {currentUID}");
                        return true;
                    }

                    // 他人のロックが有効な場合（10分以内）
                    if (elapsed.TotalMinutes < lockMinutes)
                    {
                        MessageBox.Show(
                            $"他のユーザー({lockUser})が {lockTime:yyyy/MM/dd HH:mm:ss} からシミュレーションを実行中です。\n" +
                            $"約 {lockMinutes - (int)elapsed.TotalMinutes} 分後に再試行してください。",
                            "シミュレーションロック中",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                // ここまで来たら新規ロックまたは上書き
                WriteLockFile(lockFilePath, currentUID);
                // ログ追記
                AppendLog(logFilePath, $"LOCKED by {currentUID}");
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"ロックチェック中にエラーが発生しました：\n{ex.Message}", "エラー");
                return false;
            }
        }
        //   ◆ロックファイル書き込み
        public static void WriteLockFile(string path, string uid)
        {
            File.WriteAllLines(path, new[]
            {
                $"UserID={uid}",
                $"StartTime={DateTime.Now:yyyy/MM/dd HH:mm:ss}",
                $"PC={Environment.MachineName}",
                $"Status=LOCKED"
            });
        }
        //   ◆シュミレーションロック解除
        public static bool ReleaseSimulationLock(string currentUID, string lockFilePath, string LogFilePath)
        {
            string logFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_Simulation.txt");
            string AllLogFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_AllSimulation.txt");
            string HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            try
            {
                if (!File.Exists(lockFilePath))
                    // ロックファイルが存在しないときは何もしない
                    return false;

                var lines = File.ReadAllLines(lockFilePath);
                string lockUser = lines.FirstOrDefault(l => l.StartsWith("UserID="))?.Split('=')[1];
                string timeLine = lines.FirstOrDefault(l => l.StartsWith("Time="))?.Split('=')[1];

                if (lockUser == currentUID)
                {
                    // 実行者本人のときはロック解除+ログ記録
                    lines[3] = "Status=RELEASED";
                    File.WriteAllLines(lockFilePath, lines);

                    AppendLog(logFilePath, $"RELEASED by {currentUID}");
                    if (Application.OpenForms["Form1"] is Form1 form1) form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} ロック解除");
                    return true;
                }
                else
                {
                    // 実行者本人でないときはロック解除せずに、何もしない
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ロック解除中にエラーが発生しました：\n{ex.Message}", "エラー");
                return false;
            }
        }
        //   ◆オーノ版シュミレーション(全部門)
        public void SimulateIZAIKO_Ohno(string uid, string pass, string ym)
        {
            PCommOperator.StartConnection("AUT000", PCommWindowState.MIN);

            // オーノシュミレーション実行
            Simulate(uid, pass);

            // オーノ原料在庫印刷
            PrintIZAIKO(ym, "1");
            // オーノ原料在庫のライブラリ作成
            MakeLibrary("OIZAIKOG");

            // オーノ製品在庫印刷
            PrintIZAIKO(ym, "4");
            // オーノ製品在庫のライブラリ作成
            MakeLibrary("OIZAIKOS");

            // オーノ加工在庫印刷
            PrintIZAIKO(ym, "5");
            // オーノ加工在庫のライブラリ作成
            MakeLibrary("OIZAIKOK");

            // オーノ預り在庫印刷
            PrintIZAIKO(ym, "6");
            // オーノ預り在庫のライブラリ作成
            MakeLibrary("OIZAIKOAR");

            // オーノ預け在庫印刷
            PrintIZAIKO(ym, "7");
            // オーノ預け在庫のライブラリ作成
            MakeLibrary("OIZAIKOAK");

            PCommOperator.SendKeys(f3, 20, 7);
            PCommOperator.SendKeys("24", 20, 7);
            PCommOperator.SignOff();
            PCommOperator.StopConnection();
        }
        //   ◆オーノ版シュミレーション(部門別)
        public void SimulateIZAIKO_Ohno(string uid, string pass, string ym, string bumon)
        {
            PCommOperator.StartConnection("AUT000", PCommWindowState.MIN);

            // オーノシュミレーション実行
            Simulate(uid, pass, bumon);

            // オーノ原料在庫印刷
            PrintIZAIKO(ym, "1", bumon);
            // オーノ原料在庫のライブラリ作成
            MakeLibrary("OIZAIKOG");

            // オーノ製品在庫印刷
            PrintIZAIKO(ym, "4", bumon);
            // オーノ製品在庫のライブラリ作成
            MakeLibrary("OIZAIKOS");

            // オーノ加工在庫印刷
            PrintIZAIKO(ym, "5", bumon);
            // オーノ加工在庫のライブラリ作成
            MakeLibrary("OIZAIKOK");

            // オーノ預り在庫印刷
            PrintIZAIKO(ym, "6", bumon);
            // オーノ預り在庫のライブラリ作成
            MakeLibrary("OIZAIKOAR");

            // オーノ預け在庫印刷
            PrintIZAIKO(ym, "7", bumon);
            // オーノ預け在庫のライブラリ作成
            MakeLibrary("OIZAIKOAK");

            PCommOperator.SendKeys(f3, 20, 7);
            PCommOperator.SendKeys("24", 20, 7);
            PCommOperator.SignOff();
            PCommOperator.StopConnection();
        }
        //   ◆サンミック版シュミレーション
        public void SimulateIZAIKO_Sun(string uid, string pass, string ym, string file)
        {
            PCommOperator.StartConnection("AUT000", PCommWindowState.MIN);
            // サンミックシュミレーション実行
            Simulate(uid, pass);

            // サンミック原料在庫印刷
            PrintIZAIKO(ym, "1");
            // サンミック原料在庫のライブラリ作成
            MakeLibrary(file + "IZAIKOG");

            if (file == "SC")
            {
                // サンミックタフト半製品在庫印刷
                PrintIZAIKO(ym, "2");
                // サンミックタフト半製品在庫のライブラリ作成
                MakeLibrary(file + "IZAIKOTH");

                // サンミック預り在庫印刷
                PrintIZAIKO(ym, "6");
                // サンミック預り在庫のライブラリ作成
                MakeLibrary(file + "IZAIKOAR");

                // サンミック預け在庫印刷
                PrintIZAIKO(ym, "7");
                // サンミック預け在庫のライブラリ作成
                MakeLibrary(file + "IZAIKOAK");
            }

            // サンミックコーティング半製品在庫印刷
            PrintIZAIKO(ym, "3");
            // サンミックコーティング半製品在庫のライブラリ作成
            MakeLibrary(file + "IZAIKOCH");

            // サンミック製品在庫印刷
            PrintIZAIKO(ym, "4");
            // サンミック製品在庫のライブラリ作成
            MakeLibrary(file + "IZAIKOS");

            // サンミック加工在庫印刷
            PrintIZAIKO(ym, "5");
            // サンミック加工在庫のライブラリ作成
            MakeLibrary(file + "IZAIKOK");

            PCommOperator.SendKeys(f3, 20, 7);
            PCommOperator.SendKeys("24", 20, 7);
            PCommOperator.SignOff();
            PCommOperator.StopConnection();
        }
        //   ◆シュミレーション実行(全部門)
        public void Simulate(string uid, string pass)
        {
            PCommOperator.SignOn(uid, pass);
            PCommOperator.SendKeys("11" + ctl, 20, 7);  // 月次原価シュミレーションメニュー
            PCommOperator.SendKeys("1" + ctl, 20, 7);  // 月次原価シュミレーション
            PCommOperator.SendKeys(ctl + ctl, 7, 35);  // 全部門選択＋実行
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機
            PCommOperator.SendKeys(f3, 7, 35);  // シュミレーション終了
        }
        //   ◆シュミレーション実行(部門別)
        public void Simulate(string uid, string pass, string bumon)
        {
            PCommOperator.SignOn(uid, pass);
            PCommOperator.SendKeys("11" + ctl, 20, 7);  // 月次原価シュミレーションメニュー
            PCommOperator.SendKeys("1" + ctl, 20, 7);  // 月次原価シュミレーション
            PCommOperator.SendKeys(bumon + ctl + ctl, 7, 35);  // 全部門選択＋実行
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機
            PCommOperator.SendKeys(f3, 7, 35);  // シュミレーション終了
        }
        //   ◆在庫表印刷(全部門)
        public void PrintIZAIKO(string ym, string cls)
        {
            PCommOperator.SendKeys("13" + ctl, 20, 7);  // 在庫表(月次原価シュミレーションメニュー)
            PCommOperator.SendKeys(tb, 5, 14);  // 部門選択は変更なしで年月へ移動
            PCommOperator.SendKeys(ym + ent, 8, 14);  // 年月(当月)
            PCommOperator.SendKeys(tb, 10, 14);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 16);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 26);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 28);  // 品名選択は変更なしでクラスへ移動
            PCommOperator.SendKeys(cls + ctl + ctl, 12, 14);  // 原材料決定＋実行
            PCommOperator.Wait(3000);  // 3秒待機
            PCommOperator.SendKeys("OHNOQ" + ctl, 10, 40);  // OHNOQへ印刷実行
            PCommOperator.SendKeys(f3, 5, 14);  // 印刷完了後、1つ戻る
        }
        //   ◆// 在庫表印刷(部門別)
        public void PrintIZAIKO(string ym, string cls, string bumon)
        {
            PCommOperator.SendKeys("13" + ctl, 20, 7);  // 在庫表(月次原価シュミレーションメニュー)
            PCommOperator.SendKeys(bumon, 5, 14);  // 部門選択は変更なしで年月へ移動
            PCommOperator.SendKeys(ym + ent, 8, 14);  // 年月(当月)
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys(tb, 10, 14);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 16);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 26);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 28);  // 品名選択は変更なしでクラスへ移動
            PCommOperator.SendKeys(cls + ctl + ctl, 12, 14);  // 原材料決定＋実行
            PCommOperator.Wait(3000);  // 3秒待機
            var txt = PCommOperator.GetText(24, 1, 30).Trim();
            if(txt.Contains("作表データがありません"))
            {
                PCommOperator.SendKeys(f3, 5, 14);  // 印刷完了後、1つ戻る
                return;
            }
            PCommOperator.SendKeys("OHNOQ" + ctl, 10, 40);  // OHNOQへ印刷実行
            PCommOperator.SendKeys(f3, 5, 14);  // 印刷完了後、1つ戻る
        }
        //   ◆ライブラリー作成
        public void MakeLibrary(string file)
        {
            PCommOperator.SendKeys("WRKQRY" + ctl,20, 7);  // 選択項目またはコマンド(月次原価シュミレーションメニュー)
            PCommOperator.SendKeys("2",5,26);  // QUERY処理,オプション:変更
            PCommOperator.SendKeys(file + tb, 8, 26);  // QUERY定義入力
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys("OHNO000" + ctl, 9, 28); // 実行
            PCommOperator.SendKeys(f3, 10, 3);  // QUERY定義の終了
            PCommOperator.SendKeys(tb, 5, 29);  // オプションへ移動
            PCommOperator.SendKeys("1", 7, 29);  // 対話式で実行"1"を選択
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys(ctl, 5, 29);  // QUERY作成実行
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys(f3, 5, 26);  // QUERY処理終了、1つ戻る
        }
        // =======================================================================

        // =======================================================================
        private static void AppendLog(string logFilePath, string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            File.AppendAllText(logFilePath,
                $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} | {message} ({Environment.MachineName}){Environment.NewLine}");
        }
        /// <summary>
        /// 各FormのlistBxSituationとメモリ上にログ追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddLog(string message, ListBox listBxSituation)
        {
            string logMessage = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} {message}";
            runtimelog.Add(logMessage);

            if (listBxSituation.InvokeRequired)
            {
                listBxSituation.Invoke(new Action(() =>
                {
                    listBxSituation.Items.Add(logMessage);
                }));
            }
            else
            {
                listBxSituation.Items.Add(logMessage);
            }
        }
        /// <summary>
        /// 各Formを表示するたびに既存ログもlistBxSituationに表示
        /// </summary>
        public void LoadRuntimeLog(ListBox listBxSituation)
        {
            listBxSituation.Items.Clear();
            foreach (var log in runtimelog)
            {
                listBxSituation.Items.Add(log);
            }
        }
        // =======================================================================


        // 以下のメソッドは不要なため後ほど削除
        public static void ShowBumon(Form form, ListBox listBxBumon, ListBox listBxSaller, ListBox listBxSupplier,
                                     CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            // Invokeで後回しにしなくてOK（選択状態はすでに反映済み）
            var selBumon = listBxBumon.SelectedItems
                .Cast<string>()
                .Select(item => item.Split(':')[0])
                .ToList();

            Bumon_selectedChanged(selBumon, listBxSaller, listBxSupplier, chkBxOhno, chkBxSundus, chkBxSuncar);
        }
        public static void SelectCompany_Bumon(CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar, ListBox listBxBumon)
        {
            listBxBumon.Items.Clear();

            // 会社選択確認
            var selctedComp = FormActionMethod.GetCompany(chkBxOhno, chkBxSundus, chkBxSuncar);

            // 先頭に空白行を追加
            listBxBumon.Items.Add(string.Empty);

            // 選択された会社の部門をlistBxBumonに追加
            foreach (var bumon in JsonLoader.GetBUMONs(selctedComp.ToArray()))
            {
                listBxBumon.Items.Add($"{bumon.Code}:{bumon.Name}");
            }
            listBxBumon.SelectedIndex = 0; // 空白行を選択状態にする
        }
        private static bool isUpdating = false;
        private static void Bumon_selectedChanged(List<string> selBumon, ListBox listBxSaller, ListBox listBxSupplier,
                                                 CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            if (isUpdating) return; // 再入防止
            isUpdating = true;

            try
            {
                // チェックされている部門がない場合はクリアして終了
                if (selBumon.Count == 0)
                {
                    listBxSaller.DataSource = null;
                    listBxSupplier.DataSource = null;
                    return;
                }

                // 会社選択確認
                var selctedComp = FormActionMethod.GetCompany(chkBxOhno, chkBxSundus, chkBxSuncar);

                // 部門ごとに販売先リストを取得
                var sallerList = new List<mf_HANBAI>();
                var supplierList = new List<mf_SHIIRE>();

                foreach (var comp in selctedComp)
                {
                    sallerList.AddRange(JsonLoader.GetMf_HANBAIs(comp, selBumon));
                    supplierList.AddRange(JsonLoader.GetMf_SHIIREs(comp, selBumon));
                }

                // **既存アイテムをクリアしてから追加**
                listBxSaller.Items.Clear();
                listBxSupplier.Items.Clear();

                // ---販売先を表示---
                // 先頭に空白行を追加
                listBxSaller.Items.Add(string.Empty);
                foreach (var s in sallerList.OrderBy(x => x.Code))
                {
                    listBxSaller.Items.Add($"[{s.Code}] {s.Name}");
                }

                // 仕入先を表示
                // 先頭に空白行を追加
                listBxSupplier.Items.Add(string.Empty);
                foreach (var s in supplierList.OrderBy(x => x.Code))
                {
                    listBxSupplier.Items.Add($"[{s.Code}] {s.Name}");
                }
                // 空白行を選択状態にする
                listBxSaller.SelectedIndex = 0;
                listBxSupplier.SelectedIndex = 0;
            }
            finally
            {
                isUpdating = false; // 再入防止フラグ解除
            }
        }
        public (string sd, string ed) UpdateStartEndDate(TextBox txtBxStrYearMonth, TextBox txtBxEndYearMonth)
        {
            string strInput = txtBxStrYearMonth.Text.Trim();
            string endInput = txtBxEndYearMonth.Text.Trim();

            // yyyyMM形式のチェック
            if (!Regex.IsMatch(strInput, @"^\d{6}$"))
            {
                MessageBox.Show("開始年月の形式が不正です。YYYYMM 形式で入力してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return (null, null);
            }

            if (!Regex.IsMatch(endInput, @"^\d{6}$"))
            {
                MessageBox.Show("終了年月の形式が不正です。YYYYMM 形式で入力してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return (null, null);
            }

            // DateTimeに変換（yyyyMM → yyyy/MM/01）
            DateTime start = DateTime.ParseExact(strInput, "yyyyMM", null);
            DateTime end = DateTime.ParseExact(endInput, "yyyyMM", null);

            string sd = start.ToString("yyyyMMdd"); // 月初
            int lastDay = DateTime.DaysInMonth(end.Year, end.Month);
            string ed = new DateTime(end.Year, end.Month, lastDay).ToString("yyyyMMdd"); // 月末

            return (sd, ed);
        }
    }
}
