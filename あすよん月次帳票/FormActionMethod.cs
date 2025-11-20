using Ohno.Db;
using OHNO.PComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

        public static DataTable MakeReadData_IV(string startDate, string endDate, string company)
        {
            string startYM = startDate.Substring(0, 6);
            string currentYM = DateTime.Now.ToString("yyyyMM");

            DataTable dt = new DataTable();
            if (startYM == currentYM)
            {
                // 今月分の在庫データ取得(ライブラリより取得)
                string[] files = null;

                switch (company)
                {
                    case "オーノ":
                        files = new string[] { "OIZAIKOG", "OIZAIKOS", "OIZAIKOK","OIZAIKOA" };
                        break;
                    case "サンミックダスコン":
                        files = new string[] { "SDIZAIKOG", "SDIZAIKOCH", "SDIZAIKOS" };
                        break;
                    case "サンミックカーペット":
                        files = new string[] { "SCIZAIKOG", "SCIZAIKOTH", "SCIZAIKOCH", "SCIZAIKOS" };
                        break;
                }
                var processor = new DataProcessor();


                var stockList = new List<DataTable>();
                // オーノ
                foreach (var file in files)
                {
                    // 1.SQLで在庫データ取得
                    dt = GetDataMethod.GetStockData(file);

                    if (dt == null) continue;

                    // 2.カテゴリ列追加+コード列作成+欠損値０埋め
                    dt = processor.ProsessStockTable(dt, file);
                    stockList.Add(dt);
                }

                dt = processor.MergeData(stockList.ToArray());
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
                            { "5", "加工在庫" },
                            { "6", "預り在庫" }
                        };

                        foreach (DataRow r in IVdata.Rows)
                        {
                            string code = r["ZHCSNM"]?.ToString()?.Trim();
                            if (classMap.ContainsKey(code))
                            {
                                r["ZHCSNM"] = classMap[code];
                            }
                            else
                            {
                                // マッピングが見つからない場合はコードをそのまま使用
                            }
                        }

                        // 年月列連結
                        if (!IVdata.Columns.Contains("年月"))
                            IVdata.Columns.Add("年月", typeof(string));

                        foreach (DataRow r in IVdata.Rows)
                        {
                            r["年月"] = $"{yy}{mm}";
                        }

                        // 縦連結
                        if (IVdataAll == null)
                            IVdataAll = IVdata.Clone(); // スキーマコピー
                        foreach (DataRow r in IVdata.Rows)
                            IVdataAll.ImportRow(r);
                    }
                }
                dt = IVdataAll ?? new DataTable(); // データなしは空テーブル
            }

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

        public (string sd, string ed) UpdateStartEndDate(TextBox txtBxStrYearMonth,TextBox txtBxEndYearMonth)
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

        // 年月から yyyyMMdd の開始日・終了日を取得
        public static (string startDate, string endDate) GetStartEndDate(int syear, int smonth, int eyear, int emonth)
        {
            string startDate = new DateTime(syear, smonth, 1).ToString("yyyyMMdd");
            int lastDay = DateTime.DaysInMonth(eyear, emonth);
            string endDate = new DateTime(eyear, emonth, lastDay).ToString("yyyyMMdd");
            return (startDate, endDate);
        }

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

        public static bool ReleaseSimulationLock(string currentUID, string lockFilePath, string LogFilePath, bool timflg)
        {
            string logFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_Simulation.txt");
            string AllLogFilePath = Path.Combine(LogFilePath, $@"{HIZ}\LOG_AllSimulation.txt");
            string HIZTIM = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            try
            {
                if (!File.Exists(lockFilePath))
                    return false;

                var lines = File.ReadAllLines(lockFilePath);
                string lockUser = lines.FirstOrDefault(l => l.StartsWith("UserID="))?.Split('=')[1];
                string timeLine = lines.FirstOrDefault(l => l.StartsWith("Time="))?.Split('=')[1];

                // "Time=" の行に書かれている時刻を取得
                if (!DateTime.TryParse(timeLine, out DateTime lockTime))
                    lockTime = DateTime.Now;

                // ロックファイル作成（または更新）から10分経過しているか判定
                bool isExpired = (DateTime.Now - lockTime) >= TimeSpan.FromMinutes(10);

                // 実行者本人のときだけ「解除状態」に変更
                if (lockUser == currentUID) 
                {
                    if (timflg)
                    {
                        lines[3] = "Status=RELEASED";
                        File.WriteAllLines(lockFilePath, lines);
                    }

                    AppendLog(logFilePath, $"RELEASED by {currentUID}");
                    if (Application.OpenForms["Form1"] is Form1 form1) form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} ロック解除");
                    return true;
                }
                else if (isExpired)
                {
                    if (timflg)
                    {
                        lines[3] = "Status=RELEASED";
                        File.WriteAllLines(lockFilePath, lines);
                    }

                    AppendLog(logFilePath, $"RELEASED by {currentUID}");
                    if (Application.OpenForms["Form1"] is Form1 form1) form1.AddLog2($"{HIZTIM} 実行者ID:{currentUID} 最終実行から10分経過のためロック解除");
                }
                return false; // 他人のロックは解除しない
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ロック解除中にエラーが発生しました：\n{ex.Message}", "エラー");
                return false;
            }
        }

        private static void AppendLog(string logFilePath, string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            File.AppendAllText(logFilePath,
                $"{DateTime.Now:yyyy/MM/dd HH:mm:ss} | {message} ({Environment.MachineName}){Environment.NewLine}");
        }

        // 全部門版シュミレーション
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
            MakeLibrary("OIZAIKOA");

            PCommOperator.SendKeys(f3, 20, 7);
            PCommOperator.SendKeys("24", 20, 7);
            PCommOperator.SignOff();
            PCommOperator.StopConnection();
        }
        // 部門指定版シュミレーション
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
            MakeLibrary("OIZAIKOA");

            PCommOperator.SendKeys(f3, 20, 7);
            PCommOperator.SendKeys("24", 20, 7);
            PCommOperator.SignOff();
            PCommOperator.StopConnection();
        }


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
            }

            // サンミックコーティング半製品在庫印刷
            PrintIZAIKO(ym, "3");
            // サンミックコーティング半製品在庫のライブラリ作成
            MakeLibrary(file + "IZAIKOCH");

            // サンミック製品在庫印刷
            PrintIZAIKO(ym, "4");
            // サンミック製品在庫のライブラリ作成
            MakeLibrary(file + "IZAIKOS");

            PCommOperator.SendKeys(f3, 20, 7);
            PCommOperator.SendKeys("24", 20, 7);
            PCommOperator.SignOff();
            PCommOperator.StopConnection();
        }

        public void Simulate(string uid, string pass)
        {
            // シュミレーション実行(全部門)
            PCommOperator.SignOn(uid, pass);
            PCommOperator.SendKeys("11" + ctl, 20, 7);  // 月次原価シュミレーションメニュー
            PCommOperator.SendKeys("1" + ctl, 20, 7);  // 月次原価シュミレーション
            PCommOperator.SendKeys(ctl + ctl, 7, 35);  // 全部門選択＋実行
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機
            PCommOperator.SendKeys(f3, 7, 35);  // シュミレーション終了
        }

        public void Simulate(string uid, string pass, string bumon)
        {
            // シュミレーション実行(部門指定)
            PCommOperator.SignOn(uid, pass);
            PCommOperator.SendKeys("11" + ctl, 20, 7);  // 月次原価シュミレーションメニュー
            PCommOperator.SendKeys("1" + ctl, 20, 7);  // 月次原価シュミレーション
            PCommOperator.SendKeys(bumon + ctl + ctl, 7, 35);  // 全部門選択＋実行
            PCommOperator.WaitForAppAvailable(20000);  // 最大20秒待機
            PCommOperator.WaitForInputReady(20000);  // 最大20秒待機
            PCommOperator.SendKeys(f3, 7, 35);  // シュミレーション終了
        }

        public void PrintIZAIKO(string ym, string cls)
        {
            // 在庫表印刷(全部門)
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

        public void PrintIZAIKO(string ym, string cls, string bumon)
        {
            // 在庫表印刷(部門指定)
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
            PCommOperator.SendKeys("OHNOQ" + ctl, 10, 40);  // OHNOQへ印刷実行
            PCommOperator.SendKeys(f3, 5, 14);  // 印刷完了後、1つ戻る
        }

        

        public void MakeLibrary(string file)
        {
            // ライブラリー作成
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

        public static Dictionary<string, List<mf_HANBAI>> GetHanbai10Year(string lib)
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

        public static List<string> GetCompany(CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            var selectedCompanies = new List<string>();
            if (chkBxOhno.Checked) selectedCompanies.Add("オーノ");
            if (chkBxSundus.Checked) selectedCompanies.Add("サンミックダスコン");
            if (chkBxSuncar.Checked) selectedCompanies.Add("サンミックカーペット");
            return selectedCompanies;
        }
        public static (List<string> selectedSlPrProduct, List<string> selectedIvProduct) GetProduct(CheckBox chkBxRawMaterials, CheckBox chkBxSemiFinProducts,
                                              CheckBox chkBxProduct, CheckBox chkBxProcess, CheckBox chkBxCustody,
                                              CheckBox chkBxOhno, CheckBox chkBxSundus, CheckBox chkBxSuncar)
        {
            // 売上・仕入の製品区分選択
            var selectedSlPrProduct = new List<string>();
            if (chkBxRawMaterials.Checked) selectedSlPrProduct.Add("原材料");
            if (chkBxSemiFinProducts.Checked) selectedSlPrProduct.Add("半製品");
            if (chkBxProduct.Checked) selectedSlPrProduct.Add("製品");
            if (chkBxProcess.Checked) selectedSlPrProduct.Add("加工");
            if (chkBxCustody.Checked) selectedSlPrProduct.Add("預り");

            // 在庫の製品区分選択
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
            return (selectedSlPrProduct, selectedIvProduct);
        }

        public static List<string> GetSalseProduct(CheckBox chkBxSl, CheckBox chkBxPr, CheckBox chkBxIv)
        {
            var selectedSlProduct = new List<string>();
            if (chkBxSl.Checked) selectedSlProduct.Add("売上");
            if (chkBxPr.Checked) selectedSlProduct.Add("仕入");
            if (chkBxIv.Checked) selectedSlProduct.Add("在庫");
            return selectedSlProduct;
        }

        // 部門リスト用（空白行ガード付き）
        public static List<string> GetSelectedBumons(ListBox listBxB)
        {
            return listBxB.Items
                           .Cast<string>()
                           .Where(x => !string.IsNullOrWhiteSpace(x)) // 空白行を除外
                           .Select(x => x.Split(' ')[0])  // "コード 名前" → スペースで分割して最初の要素をコードとして取得
                           .ToList();
        }

        public static List<string> GetSallerOrSupplier(ListBox listbx)
        {
            return listbx.Items
                        .Cast<string>()
                        .Where(x => !string.IsNullOrWhiteSpace(x)) // 空白行を除外
                        .Select(x => x.Split(' ')[0])  // "コード 名前" → スペースで分割して最初の要素をコードとして取得
                        .ToList();
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

        public static Dictionary<string, List<mf_SHIIRE>> GetShiire10Year(string lib)
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

        public static void SaveToJson<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
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
    }
}
