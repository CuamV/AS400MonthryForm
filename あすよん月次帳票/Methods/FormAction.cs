using Ohno.Db;
using OHNO.PComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using DCN = あすよん月次帳票.Dictionaries;

namespace あすよん月次帳票
{
    //=======================================================================
    // --------FormActionMethod(各種Form使用する処理メソッド)クラス--------
    //=======================================================================
    internal class FormAction
    {
        //========================================================================
        // 【インスタンス】
        //========================================================================
        internal GetData_AS400 gdm = new GetData_AS400();

        // フィールド変数
        static internal string ctl = "[enter]";  // Ctlr(実行)
        static internal string f3 = "[pf3]";  // F3(終了)
        static internal string tb = "[tab]"; // Tab(カーソル次送り)
        static internal string ent = "[fldext]"; // Enter(カーソル位置以降の入力exit)
        private string HIZTIM;
        private string TIM;

        private List<string> runtimelog = new List<string>();
        // =======================================================================

        // 【マスターデータ取得メソッド】
        // =======================================================================
        //  <FormMainTop>
        internal void GetMasterFromAS400(bool flg)
        {
            if (flg)
            {
                var lines = CheckAndLoadMater(Path.Combine(CMD.mfPath, "ReMasterCal.txt"), "マスタ一括更新", CMD.utf8);
                // 1:更新日付(yyyymmdd)
                // CMD.HIZ と ReMasterCalの1行目の日付を比較→日付が違う場合はマスタ更新実行
                DateTime lastUpdate;
                if (lines.Count > 0)
                {
                    if (DateTime.TryParse(lines[0], out lastUpdate))
                        if (CMD.HIZ == lastUpdate.ToString("yyyyMMdd")) return;
                }
            }
            // マスタ更新実行
            TakeInMaster();
            // 更新後、ReMasterCal.txtの1行目を現在日付に更新
            File.WriteAllText(Path.Combine(CMD.mfPath, "ReMasterCal.txt"), DateTime.Now.ToString("yyyyMMdd"), CMD.utf8);
        }

        internal void TakeInMaster()
        {
            var dt = gdm.GetTorihikiMaster();
            // 入力値チェック
            // [" "→""], ["　"→""], [null→"_"]
            // [→(有)], [","'→(、)], [改行→""]
            if (dt != null) {
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (row[i] == DBNull.Value)
                        {
                            row[i] = "_";
                        }
                        else
                        {
                            string value = row[i].ToString();
                            value = value.Replace(" ", "").Replace("　", "").Replace("\r\n", "").Replace("\n", "").Replace(",", "、").Replace("'", "、").Replace("", "有");
                            if (string.IsNullOrWhiteSpace(value))
                                value = "";
                            row[i] = value;
                        }
                    }
                }
            }
            // テキスト半角スペース区切りにしてTORIHIKI.txtにて保存
            var lines = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                var parts = new List<string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    parts.Add(row[i].ToString().PadRight(50).Substring(0, 50));
                }
                lines.Add(string.Join(" ", parts));
            }

            File.WriteAllLines(Path.Combine(CMD.mfPath, "TORIHIKI.txt"), lines, CMD.utf8);
        }
        /// <summary>
        /// ◆ユーザー名取得
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mf"></param>
        /// <returns></returns>
        internal string GetUserName(string id,string mf)
        {
            try
            {
                if (!File.Exists(mf))
                    return string.Empty;

                foreach (var line in File.ReadLines(mf, Encoding.UTF8))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // CSVカンマ区切り
                    var parts = line.Split(',');
                    if (parts.Length < 2) continue;

                    // CSV特有のダブルクオーテーション除去
                    parts[0] = parts[0].Trim().Trim('"');
                    parts[1] = parts[1].Trim().Trim('"');

                    // ID一致で2つ目の要素(名前)を返す
                    if (parts[0] == id)
                        return parts[1];
                }
            }
            catch
            { 
            }
            return string.Empty;
        }
        //  <Form1>
        //   ◆販売先マスター取得
        internal Dictionary<string, List<mf_HANBAI>> GetHanbaiAll(string lib)
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
        internal Dictionary<string, List<mf_SHIIRE>> GetShiireAll(string lib)
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
        internal void SaveToJson<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        // =======================================================================

        // =======================================================================
        // 【エラーチェックメソッド】
        //  <RplForm2>
        //   ◆年月入力チェック(yyyyMM形式)
        internal bool TryParseYearMonth(TextBox txtBox, out int year, out int month)
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
        internal string GetBookName(TextBox txtBox)
        {
            return txtBox.Text.Trim();
        }
        //   ◆年月選択
        internal (string startDate, string endDate) GetStartEndDate(int syear, int smonth, int eyear, int emonth)
        {
            string startDate = new DateTime(syear, smonth, 1).ToString("yyyyMMdd");
            int lastDay = DateTime.DaysInMonth(eyear, emonth);
            string endDate = new DateTime(eyear, emonth, lastDay).ToString("yyyyMMdd");
            return (startDate, endDate);
        }
        //   ◆会社選択
        internal List<string> GetCompany(CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            var selectedCompanies = new List<string>();
            if (chkBxOhno.Checked) selectedCompanies.Add("オーノ");
            if (chkBxSundus.Checked) selectedCompanies.Add("サンミックダスコン");
            if (chkBxSuncar.Checked) selectedCompanies.Add("サンミックカーペット");
            return selectedCompanies;
        }
        //   ◆部門選択
        internal List<string> GetSelectedBumons(ListBox listBxB)
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
        internal List<string> GetSallerOrSupplier(ListBox listbx)
        {
            var result = new List<string>();
            foreach (var item in listbx.Items)
            {
                result.Add(((Torihiki)item).Code);
            }
            return result;
        }
        //   ◆データ区分選択
        internal List<string> GetSalseProduct(CheckBox chkBxSl, CheckBox chkBxPr, CheckBox chkBxIv)
        {
            var selectedSlProduct = new List<string>();
            if (chkBxSl.Checked) selectedSlProduct.Add("売上");
            if (chkBxPr.Checked) selectedSlProduct.Add("仕入");
            if (chkBxIv.Checked) selectedSlProduct.Add("在庫");
            return selectedSlProduct;
        }
        //   ◆クラス区分選択
        //    ＊売上・仕入
        internal List<string> GetProduct(CheckBox chkBxRawMaterials, CheckBox chkBxSemiFinProducts, CheckBox chkBxProduct,
                                              CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            var selectedSlPrProduct = new List<string>();

            if (chkBxRawMaterials.Checked) selectedSlPrProduct.Add("原材料");
            if (chkBxSemiFinProducts.Checked) selectedSlPrProduct.Add("半製品");
            if (chkBxProduct.Checked) selectedSlPrProduct.Add("製品");

            return selectedSlPrProduct;
        }
        //    ＊在庫
        internal List<string> GetProduct(CheckBox chkBxRawMaterials, CheckBox chkBxSemiFinProducts, CheckBox chkBxProduct,
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
        internal Dictionary<string, string> GetIvType(CheckBox chkBxOneCom, CheckBox chkBxCustody, CheckBox chkBxEntrust, CheckBox chkBxProcess)
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
        internal string GetAggregte(GroupBox grpBox)
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
        internal DataTable MakeReadData_SLPR(string startDate, string endDate, string company, string kubun)
        {
            DataTable dt = new DataTable();
            // 各データ取得(売上,仕入)
            if (company == "オーノ" && kubun == "SL")
                dt = gdm.GetSalesData(startDate, endDate, "SM1DLB01");  // オーノ(売上)
            else if (company == "サンミックカーペット" && kubun == "SL")
                dt = gdm.GetSalesData(startDate, endDate, "SM1DLB03");  // サンミック カーペット(売上)
            else if (company == "サンミックダスコン" && kubun == "SL")
                dt = gdm.GetSalesData(startDate, endDate, "SM1DLB02");  // サンミック ダスコン(売上)
            else if (company == "オーノ" && kubun == "PR")
                dt = gdm.GetPurchaseData(startDate, endDate, "SM1DLB01");  // オーノ(仕入)
            else if (company == "サンミックカーペット" && kubun == "PR")
                dt = gdm.GetPurchaseData(startDate, endDate, "SM1DLB03", "0001576");  // サンミック カーペット(仕入)
            else if (company == "サンミックダスコン" && kubun == "PR") 
                dt = gdm.GetPurchaseData(startDate, endDate, "SM1DLB02", "0000009");  // サンミック ダスコン(仕入)
            return dt;
        }
        //   ◆在庫データ取得
        internal (DataTable, DataTable) MakeReadData_IV(string startDate, string endDate, string company, List<string> selIvProducts)
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
                // 選択された品目のファイル名リストを作る
                var files = selIvProducts
                    .Where(p => DCN.fileMap[company].ContainsKey(p))
                    .Select(p => DCN.fileMap[company][p])
                    .ToList();

                var processor = new Processor();
                var stockList = new List<DataTable>();

                foreach (var file in files)
                {
                    // 1.SQLで在庫データ取得
                    dtNow = gdm.GetStockData(file);

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
                    //==============================================================
                    //  1:ZGZKSB(在庫種別)     2:SHCLAS(クラス)      3:ZGBMCD(部門コード) 4:ZGWHCD(倉庫コード)  5:倉庫名
                    //  6:ZGAZCD(預り先コード) 7:預り先名            8:品名               9:ZGHMCD(品名コード) 10:ZGHSCD(品種コード)
                    // 11:ZGCLCD(色コード)    12:ZGTZQT(当月残数量) 13:ZGTGZA(当月残金額) 
                    //==============================================================
                    var IVdata = gdm.GetStockData(lib, yy, mm);

                    if (IVdata != null && IVdata.Rows.Count > 0)
                    {
                        // クラスコード → 日本語変換
                        foreach (DataRow r in IVdata.Rows)
                        {
                            string code = r["SHCLAS"]?.ToString()?.Trim();
                            if (DCN.classMap.ContainsKey(code))
                            {
                                r["SHCLAS"] = DCN.classMap[code];
                            }
                        }

                        // 年月列追加共通処理
                        string ym = yy + mm;
                        IVdata = AddYearMonthColum(IVdata, ym);

                        //============================================================================
                        //  1:年月    2:ZGZKSB(在庫種別) 3:SHCLAS(クラス)     4:ZGBMCD(部門コード)  5:ZGWHCD(倉庫コード) 6:倉庫名             7:ZGAZCD(預り先コード)
                        //  8:預り先名 9:品名           10:ZHHMCD(品名コード) 11:ZHHSCD(品種コード) 12:ZGCLCD(色コード) 13:ZGTZQT(当月残数量) 14:ZGTGZA(当月残金額) 
                        //============================================================================
                        // 縦連結
                        if (IVdataAll == null)
                            IVdataAll = IVdata.Clone(); // スキーマコピー
                        foreach (DataRow r in IVdata.Rows)
                            IVdataAll.ImportRow(r);
                    }
                }
                dtOld = IVdataAll ?? new DataTable(); // データなしは空テーブル
            }
            return (dtNow,dtOld);
        }
        //   ◆年月列追加共通処理
        internal DataTable AddYearMonthColum(DataTable dt, string startYM)
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
        internal (DataTable, DataTable, DataTable) FilterData(string startDate, string endDate,
                                                                   List<string> selCompanies, List<string> selBumons,
                                                                   List<string> selSelleres, List<string> selSupplieres,
                                                                   List<string> selSlCategories, List<string> selSlPrProducts, List<string> selIvProducts,
                                                                   Dictionary<string,string> selIvTypes)
        {
            Processor processor = new Processor();

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
        internal DataTable MargeAndFormat_StockData(DataTable ohnoStock,DataTable suncarStock, DataTable sundusStock, bool newold)
        {
            DataTable stockDt = null;
            var stockList = new List<DataTable>();
            Processor processor = new Processor();

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
        internal bool CheckAndLockSimulation(int lockMinutes)
        {
            string lockFile = Path.Combine(CMD.LockPath, "LOCK_sim.txt");
            try
            {
                // ロックファイルのディレクトリがなければ作成
                Directory.CreateDirectory(Path.GetDirectoryName(lockFile));

                // ファイルが存在しない＝実施者なし→新規作成してロックし、処理実施
                if (!File.Exists(lockFile))
                {
                    WriteLockFile(lockFile, CMD.UserID);
                    AppendLog(CMD.sLog, $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] LOCKED by {CMD.UserID}");
                    return true;
                }
                // ファイルが存在する＝他のユーザーが使用中(ロック中)
                var lines = File.ReadAllLines(lockFile);
                string lockUser = lines.FirstOrDefault(l => l.StartsWith("UserID="))?.Split('=')[1];
                string timeStr = lines.FirstOrDefault(l => l.StartsWith("StartTime="))?.Split('=')[1];

                if (DateTime.TryParse(timeStr, out DateTime lockTime))
                {
                    var elapsed = DateTime.Now - lockTime;

                    // 同一ユーザーなら上書きしてOK
                    if (lockUser == CMD.UserID)
                    {
                        WriteLockFile(lockFile, CMD.UserID);
                        AppendLog(CMD.sLog, $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss}] RE-LOCKED by same user {CMD.UserID}");
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
                WriteLockFile(lockFile, CMD.UserID);
                // ログ追記
                AppendLog(CMD.sLog, $"LOCKED by {CMD.UserID}");
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"ロックチェック中にエラーが発生しました：\n{ex.Message}", "エラー");
                return false;
            }
        }
        //   ◆ロックファイル書き込み
        internal void WriteLockFile(string path, string uid)
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
        internal bool ReleaseSimulationLock()
        {
            string lockFile = Path.Combine(CMD.LockPath, "LOCK_sim.txt");
            HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            AddLog($"{HIZTIM} シュミレーションメソッド 1 {CMD.UserName} ReleaseSimulationLock");

            try
            {
                if (!File.Exists(CMD.LockPath))
                    // ロックファイルが存在しないときは何もしない
                    return false;

                var lines = File.ReadAllLines(lockFile);
                string lockUser = lines.FirstOrDefault(l => l.StartsWith("UserID="))?.Split('=')[1];
                string timeLine = lines.FirstOrDefault(l => l.StartsWith("Time="))?.Split('=')[1];

                if (lockUser == CMD.UserID)
                {
                    // 実行者本人のときはロック解除+ログ記録
                    lines[3] = "Status=RELEASED";
                    File.WriteAllLines(lockFile, lines);

                    AppendLog(CMD.sLog, $"RELEASED by {CMD.UserID}");
                    AddLog2($"{HIZTIM} 実行者ID:{CMD.UserID} ロック解除");
                    return true;
                }
                else
                {
                    // 実行者本人でないときはロック解除せずに、何もしない
                    AddLog2($"{HIZTIM} 実行者ID:{lockUser} ロック未解除");
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
        internal void SimulateIZAIKO_Ohno(string uid, string pass, string ym)
        {
            PCommOperator.StartConnection("AUT000", PCommWindowState.MIN);
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機

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
        internal void SimulateIZAIKO_Ohno(string uid, string pass, string ym, string bumon)
        {
            PCommOperator.StartConnection("AUT000", PCommWindowState.MIN);
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機

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
        internal void SimulateIZAIKO_Sun(string uid, string pass, string ym, string file)
        {
            PCommOperator.StartConnection("AUT000", PCommWindowState.MIN);
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機

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
        internal void Simulate(string uid, string pass)
        {
            PCommOperator.Wait(5000);  // 5秒待機
            PCommOperator.SignOn(uid, pass);
            PCommOperator.SendKeys("11" + ctl, 20, 7);  // 月次原価シュミレーションメニュー
            PCommOperator.SendKeys("1" + ctl, 20, 7);  // 月次原価シュミレーション
            PCommOperator.SendKeys(ctl + ctl, 7, 35);  // 全部門選択＋実行
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機
            PCommOperator.SendKeys(f3, 7, 35);  // シュミレーション終了
        }
        //   ◆シュミレーション実行(部門別)
        internal void Simulate(string uid, string pass, string bumon)
        {
            PCommOperator.Wait(5000);  // 5秒待機
            PCommOperator.SignOn(uid, pass);
            PCommOperator.SendKeys("11" + ctl, 20, 7);  // 月次原価シュミレーションメニュー
            PCommOperator.SendKeys("1" + ctl, 20, 7);  // 月次原価シュミレーション
            PCommOperator.SendKeys(bumon + ctl + ctl, 7, 35);  // 全部門選択＋実行
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機
            PCommOperator.SendKeys(f3, 7, 35);  // シュミレーション終了
        }
        //   ◆在庫表印刷(全部門)
        internal void PrintIZAIKO(string ym, string cls)
        {
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys("13" + ctl, 20, 7);  // 在庫表(月次原価シュミレーションメニュー)
            PCommOperator.SendKeys(tb, 5, 14);  // 部門選択は変更なしで年月へ移動
            PCommOperator.SendKeys(ym + ent, 8, 14);  // 年月(当月)
            PCommOperator.SendKeys(tb, 10, 14);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 16);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 26);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 28);  // 品名選択は変更なしでクラスへ移動
            PCommOperator.SendKeys(cls + ctl + ctl, 12, 14);  // 原材料決定＋実行
            PCommOperator.Wait(1000);  // 1秒待機
            var txt = PCommOperator.GetText(24, 1, 30).Trim();
            if (txt.Contains("作表データがありません"))
            {
                PCommOperator.SendKeys(f3, 5, 14);  // 印刷完了後、1つ戻る
                return;
            }
            PCommOperator.SendKeys("OHNOQ" + ctl, 10, 40);  // OHNOQへ印刷実行
            PCommOperator.SendKeys(f3, 5, 14);  // 印刷完了後、1つ戻る
        }
        //   ◆// 在庫表印刷(部門別)
        internal void PrintIZAIKO(string ym, string cls, string bumon)
        {
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys("13" + ctl, 20, 7);  // 在庫表(月次原価シュミレーションメニュー)
            PCommOperator.SendKeys(bumon, 5, 14);  // 部門選択は変更なしで年月へ移動
            PCommOperator.SendKeys(ym + ent, 8, 14);  // 年月(当月)
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys(tb, 10, 14);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 16);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 26);  // 品名選択は変更なしで次へ移動
            PCommOperator.SendKeys(tb, 10, 28);  // 品名選択は変更なしでクラスへ移動
            PCommOperator.SendKeys(cls + ctl + ctl, 12, 14);  // 原材料決定＋実行
            PCommOperator.Wait(1000);  // 1秒待機
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
        internal void MakeLibrary(string file)
        {
            PCommOperator.Wait(2000);  // 2秒待機
            PCommOperator.SendKeys("WRKQRY" + ctl,20, 7);  // 選択項目またはコマンド(月次原価シュミレーションメニュー)
            PCommOperator.SendKeys("2",5,26);  // QUERY処理,オプション:変更
            PCommOperator.SendKeys(file + tb, 8, 26);  // QUERY定義入力
            PCommOperator.Wait(1000);  // 1秒待機
            PCommOperator.SendKeys("OHNO000" + ctl, 9, 28); // 実行
            PCommOperator.SendKeys(f3, 10, 3);  // QUERY定義の終了
            PCommOperator.SendKeys(tb, 5, 29);  // オプションへ移動
            PCommOperator.SendKeys("1", 7, 29);  // 対話式で実行"1"を選択
            PCommOperator.Wait(1000);  // 1秒待機
            PCommOperator.SendKeys(ctl, 5, 29);  // QUERY作成実行
            PCommOperator.Wait(1000);  // 1秒待機
            PCommOperator.SendKeys(f3, 5, 26);  // QUERY処理終了、1つ戻る
        }
        // =======================================================================

        // =======================================================================
        // 【ログ関連メソッド】
        ///<summary>
        /// 個人用ログを追加&ログファイル保存
        /// </summary>
        /// <param name="message"></param>
        internal void AddLog(string message)
        {
            if (!Directory.Exists(CMD.LogPath))
                Directory.CreateDirectory(CMD.LogPath);
            // 個人用ログファイルパス
            if (!File.Exists(CMD.uLog))
                File.Create(CMD.uLog).Close();
            //  ログファイルに保存
            File.AppendAllText(CMD.uLog, message + Environment.NewLine);
        }
        ///<summary>
        /// 全体用ログを追加&ログファイル保存
        /// </summary>
        /// <param name="message"></param>
        internal void AddLog2(string message)
        {
            if (!Directory.Exists(CMD.LogPath))
                Directory.CreateDirectory(CMD.LogPath);
            // 全体用ログファイルパス
            if (!File.Exists(CMD.conLog))
                File.Create(CMD.conLog).Close();
            // ログファイルに保存
            File.AppendAllText(CMD.conLog, message + Environment.NewLine);
        }
        private void AppendLog(string logFilePath, string message)
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
        internal void AddLog(string message, ListBox listBxSituation)
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
        internal void LoadRuntimeLog(ListBox listBxSituation)
        {
            listBxSituation.Items.Clear();
            foreach (var log in runtimelog)
            {
                listBxSituation.Items.Add(log);
            }
        }
        internal string GetShowLLog(string id, string mf)
        {
            try
            {
                if (!File.Exists(mf))
                    return string.Empty;

                foreach (var line in File.ReadLines(mf, Encoding.UTF8))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // CSVカンマ区切り
                    var parts = line.Split(',');
                    if (parts.Length < 2) continue;

                    // CSV特有のダブルクオーテーション除去
                    parts[0] = parts[0].Trim().Trim('"');
                    parts[1] = parts[1].Trim().Trim('"');

                    // ID一致で2つ目の要素(名前)を返す
                    if (parts[0] == id)
                        return parts[1];
                }
            }
            catch
            {
            }
            return string.Empty;
        }
        //=======================================================================

        //=======================================================================
        // マスタメニュー関連メソッド
        //=======================================================================
        /// <summary>
        /// ★マスタファイルに新規追加または上書き保存[部門]
        /// (※1項目目キーのみ)
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="bumonCD"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        internal (List<string>, bool) AddMasterFile(List<string> lines, string CD1, string newLine)
        {
            bool replaced = false;
            // 既存データチェック(重複)
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(' ');
                if (parts.Length > 0 && parts[0] == CD1)
                {
                    // 重複あり→上書き
                    lines[i] = newLine;
                    replaced = true;
                    break;
                }
            }
            // 新規追加
            if (!replaced) lines.Add(newLine);

            return (lines, replaced);
        }
        /// <summary>
        /// ★マスタファイルに新規追加または上書き保存[仕入先・販売先]
        /// (※1,2項目目キー)
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="bumonCD"></param>
        /// <param name="newLine"></param>
        /// <returns></returns>
        internal (List<string>, List<string>, bool) AddMasterFile(List<string> lines, List<string> lines_bumon, string CD1, string CD2, string newLine, string newLine_bumon)
        {
            bool replaced = false;
            // 既存データチェック(重複)
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(' ');
                // 仕入先・販売先
                //----------------------------------------------------
                if (parts.Length > 0 && parts[0] == CD1)
                {
                    // 重複あり→上書き
                    lines[i] = newLine;
                    replaced = true;
                    break;
                }
            }
            for (int i = 0; i < lines_bumon.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var parts = lines_bumon[i].Split(' ');
                // 1:取引先CD 2:部門CD
                //----------------------------------------------------
                if (parts.Length > 1 && parts[0] == CD1 && parts[1] == CD2)
                {
                    // 重複あり→上書き
                    lines_bumon[i] = newLine_bumon;
                    replaced = true;
                    break;
                }
            }
            // 新規追加
            if (!replaced) lines.Add(newLine);
            if (!replaced) lines_bumon.Add(newLine_bumon);

            return (lines, lines_bumon, replaced);
        }

        /// <summary>
        /// ★マスタファイルバックアップ処理
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="flg"></param>
        /// <param name="mst"></param>
        internal void BackupMaster(string mf, string targetMfName, string flg, string mst)
        {
            if (File.Exists(mf))
            {
                try
                {
                    // ① バックアップファイル名生成
                    string timestamp = DateTime.Now.ToString("yyyyMMdd.HHmmss");
                    string bkFile = $"{targetMfName}_{flg}.{timestamp}.txt";

                    // ② 移動先パス
                    string bkPath = Path.Combine(CMD.mfBkPath, bkFile);

                    // ③ バックアップフォルダが無ければ作成
                    Directory.CreateDirectory(CMD.mfBkPath);

                    // ④ ファイルを移動（リネーム）
                    File.Move(mf, bkPath);

                    // ⑤ バックアップ 3 個以上の場合、古いものから削除
                    var bkOldFiles = Directory.GetFiles(CMD.mfBkPath, $"{targetMfName}*.txt")
                                               .OrderByDescending(f => f)  // 新しい順
                                               .ToList();

                    if (bkOldFiles.Count > 3)
                    {
                        foreach (var old in bkOldFiles.Skip(3))
                        {
                            try
                            {
                                File.Delete(old);
                            }
                            catch { /* 削除失敗は無視 */ }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("バックアップ処理でエラーが発生しました。\n" + ex.Message,
                        "バックアップエラー",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                    AddLog($"{HIZTIM} エラー 1 {CMD.UserID} BackupMaster {mst}");
                    AddLog2($"{HIZTIM} {mst}エラー 1 {CMD.UserID} BackupMaster {ex.Message}");
                    return;
                }
            }
        }
        /// <summary>
        /// マスターファイルチェック＆読込
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="mst"></param>
        internal List<string> CheckAndLoadMater(string mf, string mst, Encoding encod)
        {
            List<string> lines = new List<string>();
            // ファイルチェック
            if (!File.Exists(mf))
            {
                MessageBox.Show($"{mst}ファイルが存在しません。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
                AddLog($"{HIZTIM} エラー 1 {CMD.UserID} BackupMaster {mst}");
                AddLog2($"{HIZTIM} エラー 1 {CMD.UserID} BackupMaster {mst}ファイルなし");
                return lines;
            }

            // ファイル読込
                    lines = File.ReadAllLines(mf, encod)
                        .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            return lines;
        }

        /// <summary>
        /// CSV取り込み処理（指定会社へインポート）
        /// rawLines: CSVの生行（ヘッダを含む）
        /// レイアウト: 17項目
        /// 1:仕入先CD 2:部門CD 3:仕入先正式名称 4:仕入先名 5:仕入先名カナ 6:仕入先略名 7:仕入先略名カナ
        /// 8:郵便番号 9:電話番号1 10:電話番号2 11:FAX番号1 12:FAX番号2 13:住所1 14:住所1カナ 15:住所2 16:住所2カナ 17:備考
        /// 説明:
        /// - 指定された会社(targetCompany)の DLB0XSHIIRE.txt を全件差し替えします（部門CDは除外）。
        /// - SHIIRE-BUMON.txt は仕入先CD+部門CDをキーに差分置換（既存は保持、該当キーは置換/追加）します。
        /// </summary>
        internal (bool success, string message) ImportMaster(string[] rawLines, string company)
        {
            try
            {
                // ヘッダ解析
                var headers = SplitCsvLine(rawLines[0]);
                string[] requiredColumns =
                { "仕入先CD", "部門CD", "仕入先正式名称", "仕入先名", "仕入先名カナ", "仕入先略名", "仕入先略名カナ",
                  "郵便番号", "電話番号1", "電話番号2", "FAX番号1", "FAX番号2", "住所1", "住所1カナ", "住所2", "住所2カナ", "備考" };

                // 会社別マスター格納先
                var masterByCompany = new Dictionary<string, List<string>>
                {
                    ["オーノ"] = new List<string>(),
                    ["サンミックダスコン"] = new List<string>(),
                    ["サンミックカーペット"] = new List<string>()
                };
                var MfNew = new List<string>();
                var bumonPairs = new List<string>(); // for SHIIRE-BUMON

                // ヘッダ名→インデックス
                var headerIndex = new Dictionary<string, int>();
                for (int i = 0; i < headers.Length; i++) headerIndex[headers[i]] = i;

                // データ行処理
                for (int i = 1; i < rawLines.Length; i++)
                {
                    var fields = SplitCsvLine(rawLines[i]);
                    var f = new string[17];
                    for (int j = 0; j < 17; j++) f[j] = j < fields.Length ? fields[j].Trim() : string.Empty;

                    // '_'を'NULL値'に置換
                    for (int j = 0; j < f.Length; j++) if (f[j] == "_") f[j] = null;

                    var siireCD = f[0];
                    var bumonCD = f[1];

                    // DLB0XSHIIRE
                    // 部門CD列を除外したマスタ行を構築 (16項目+USER,登録日日付,登録時刻)
                    var masterFields = new[] { f[0], f[2], f[3], f[4], f[5], f[6], f[7], f[8], f[9], f[10],
                        f[11], f[12], f[13], f[14], f[15], f[16], CMD.UserID, CMD.HIZ, DateTime.Now.ToString("HHmmss") };
                    var masterLine = string.Join(" ", masterFields.Select(x => string.IsNullOrEmpty(x) ? "" : x));

                    // 仕入マスタ
                    MfNew.Add(masterLine);
                    // SHIIRE-BUMON 用のペアを収集
                    bumonPairs.Add($"{siireCD} {bumonCD}");
                }

                // Prepare file paths
                var mf1 = Path.Combine(CMD.mfPath, "DLB01SHIIRE.txt");
                var mf2 = Path.Combine(CMD.mfPath, "DLB02SHIIRE.txt");
                var mf3 = Path.Combine(CMD.mfPath, "DLB03SHIIRE.txt");
                var shiiresBumon = Path.Combine(CMD.mfPath, "SHIIRE-BUMON.txt");

                // 指定会社の DLB0XSHIIRE を全件差し替えで作成
                string targetMf = mf1;
                string targetMfName = "DLB01SHIIRE";
                if (company == "サンミックダスコン")
                {
                    targetMf = mf2;
                    targetMfName = "DLB02SHIIRE";
                }
                else if (company == "サンミックカーペット")
                {
                    targetMf = mf3;
                    targetMfName = "DLB03SHIIRE";
                }

                // backup old file and write new
                BackupMaster(targetMf, targetMfName, "Import", "仕入先マスタ");
                // 全件差し替え
                File.WriteAllLines(targetMf, MfNew, CMD.utf8);

                // SHIIRE-BUMON: 既存を読み込み、仕入先CD+部門CD をキーに差分置換/追加
                var existingPairs = new List<string>();
                if (File.Exists(shiiresBumon))
                    existingPairs = CheckAndLoadMater(shiiresBumon, "仕入先部門マスタ", CMD.utf8).ToList();

                // build map for existing
                var existMap = new Dictionary<string, string>(); // key: "siireCD|bumonCD" -> line
                foreach (var ex in existingPairs)
                {
                    if (string.IsNullOrWhiteSpace(ex)) continue;
                    var p = ex.Split(' ');
                    if (p.Length >= 2)
                    {
                        var key = p[0] + "|" + p[1];
                        existMap[key] = ex;
                    }
                }

                // apply incoming pairs (overwrite or add)
                foreach (var np in bumonPairs)
                {
                    if (string.IsNullOrWhiteSpace(np)) continue;
                    var p = np.Trim().Split(' ');
                    if (p.Length < 2) continue;
                    var key = p[0] + "|" + p[1];
                    existMap[key] = np.Trim();
                }

                var outPairs = existMap.Values.Where(x => !string.IsNullOrWhiteSpace(x)).OrderBy(x => x.Split(' ')[0]).ThenBy(x => x.Split(' ')[1]).ToList();
                BackupMaster(shiiresBumon, "SHIIRE-BUMON", "Import", "仕入先部門マスタ");
                File.WriteAllLines(shiiresBumon, outPairs, CMD.utf8);

                return (true, "インポートが完了しました。");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private string[] SplitCsvLine(string line)
        {
            var list = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            list.Add(sb.ToString());
            return list.ToArray();
        }
        // 取引先コードで
        internal void GetMadterTorihiki_Bumon()
        {

        }


        // 以下のメソッドは不要なため後ほど削除
        internal List<string> SelectCompany_Bumon(string company, string mst)
        {
            List<string> bumon = new List<string>();

            // 1:部門コード 2:部門名 3:部門名カナ 4:会社
            CMD.utf8 = Encoding.GetEncoding("UTF-8");
            var lines = CheckAndLoadMater(Path.Combine(CMD.mfPath,"BUMON.txt"), mst , CMD.utf8);

            // 既存データチェック(重複)
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = lines[i].Split(' ');
                // 1:部門コード 2:部門名 3:部門名カナ 4:会社
                if (parts.Length > 3 && parts[3] == company)
                {
                    // 会社で一致するレコードをbumonへ追加
                    bumon.Add(parts[0]);
                }
            }
            return bumon;
        }
        private bool isUpdating = false;
        //private void Bumon_selectedChanged(List<string> selBumon, ListBox listBxSaller, ListBox listBxSupplier,
        //                                         CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        //{
        //    if (isUpdating) return; // 再入防止
        //    isUpdating = true;

        //    try
        //    {
        //        // チェックされている部門がない場合はクリアして終了
        //        if (selBumon.Count == 0)
        //        {
        //            listBxSaller.DataSource = null;
        //            listBxSupplier.DataSource = null;
        //            return;
        //        }

        //        // 会社選択確認
        //        var selctedComp = SelectCompany_Bumon(chkBxSundus, chkBxSuncar);

        //        // 部門ごとに販売先リストを取得
        //        var sallerList = new List<mf_HANBAI>();
        //        var supplierList = new List<mf_SHIIRE>();

        //        foreach (var comp in selctedComp)
        //        {
        //            sallerList.AddRange(JsonLoader.GetMf_HANBAIs(comp, selBumon));
        //            supplierList.AddRange(JsonLoader.GetMf_SHIIREs(comp, selBumon));
        //        }

        //        // **既存アイテムをクリアしてから追加**
        //        listBxSaller.Items.Clear();
        //        listBxSupplier.Items.Clear();

        //        // ---販売先を表示---
        //        // 先頭に空白行を追加
        //        listBxSaller.Items.Add(string.Empty);
        //        foreach (var s in sallerList.OrderBy(x => x.Code))
        //        {
        //            listBxSaller.Items.Add($"[{s.Code}] {s.Name}");
        //        }

        //        // 仕入先を表示
        //        // 先頭に空白行を追加
        //        listBxSupplier.Items.Add(string.Empty);
        //        foreach (var s in supplierList.OrderBy(x => x.Code))
        //        {
        //            listBxSupplier.Items.Add($"[{s.Code}] {s.Name}");
        //        }
        //        // 空白行を選択状態にする
        //        listBxSaller.SelectedIndex = 0;
        //        listBxSupplier.SelectedIndex = 0;
        //    }
        //    finally
        //    {
        //        isUpdating = false; // 再入防止フラグ解除
        //    }
        //}
        internal (string sd, string ed) UpdateStartEndDate(TextBox txtBxStrYearMonth, TextBox txtBxEndYearMonth)
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
