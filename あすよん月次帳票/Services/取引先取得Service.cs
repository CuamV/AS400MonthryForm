using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using あすよん月次帳票.Models;
using あすよん月次帳票.Repositories;
using あすよん月次帳票.Utilities;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票.Services
{
    /// <summary>
    /// 取引先に関するビジネスロジックを担当
    /// </summary>
    public class 取引先取得Service
    {
        private readonly I取引先Repository _repository;

        // コンストラクタでRepositoryを注入
        public 取引先取得Service(I取引先Repository repository)
        {
            _repository = repository;　// AS400取引先のマスタ実体の住所(newして実体を作成した時のメモリ上の場所)をコピー
        }

        // デフォルトコンストラクタ(既存コードとの互換の為)
        public 取引先取得Service()
            : this(new 取引先Repository())
        {
        }

        /// <summary>
        /// 正規化済みA00S取引先マスタをSQL Serverに保存
        /// 取引履歴をSQL Serverに保存
        /// </summary>
        public (bool success, string message) SaveAS400取引先マスタToSqlServer()
        {
            try
            {
                var マスタリスト = Get正規化済み取引先マスタ();
                _repository.SaveAS400取引先マスタToSqlServer(マスタリスト);

                var 履歴リスト = Get取引部門履歴();
                _repository.SaveAS400取引先部門マスタToSqlServer(履歴リスト);

                return (true, $"AS400取引先マスタの取得が完了しました。\n\n" +
                     $"取引先マスタ: {マスタリスト.Count}件\n" +
                     $"取引履歴: {履歴リスト.Count}件\n" +
                     $"保存日時: {DateTime.Now:yyyy/MM/dd HH:mm:ss}");
            }
            catch (Exception ex)
            {
                return (false, $"AS400取引先マスタの取得中にエラーが発生しました。\n\n{ex.Message}");
            }
            
        }


        /// <summary>
        /// 正規化済み取引先マスタリストを取得
        /// </summary>
        /// <returns></returns>
        public List<取引先Master> Get正規化済み取引先マスタ()
        {
            var マスタDt = _repository.Get取引先マスタ();
            var 結果 = new List<取引先Master>();

            foreach (DataRow row in マスタDt.Rows)
            {
                結果.Add(CreateNormalizedMaster(row));
            }

            return 結果;
        }

        /// <summary>
        /// AS400から取引部門履歴を取得してエンティティに変換
        /// </summary>
        private List<取引部門履歴> Get取引部門履歴()
        {
            var 履歴Dt = _repository.Get取引先履歴();
            var 結果 = new List<取引部門履歴>();

            foreach(DataRow row in 履歴Dt.Rows)
            {
                結果.Add(new 取引部門履歴
                {
                    取引先コード = row["取引先コード"]?.ToString() ?? "",
                    部門コード = row["部門コード"]?.ToString() ?? ""
                });
            }
            return 結果;
        }

        
        /// <summary>
        /// DataRowから正規化済み取引先Masterエンティティを作成
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private 取引先Master CreateNormalizedMaster(DataRow row)
        {
            return new 取引先Master
            {
                取引先コード = GetValue(row, (int)TORIHIKI_AS400_MASTER.取引先コード),

                // 取引先名系は正規化処理を適用
                取引先正式名 = StringNormalizer.NormalizeTorihikiName(GetValue(row, (int)TORIHIKI_AS400_MASTER.取引先正式名)),
                取引先名 = StringNormalizer.NormalizeTorihikiName(GetValue(row, (int)TORIHIKI_AS400_MASTER.取引先名)),
                略名 = StringNormalizer.NormalizeTorihikiName(GetValue(row, (int)TORIHIKI_AS400_MASTER.略名)),
                カナ名 = StringNormalizer.NormalizeTorihikiName(GetValue(row, (int)TORIHIKI_AS400_MASTER.カナ名)),

                郵便番号 = GetValue(row, (int)TORIHIKI_AS400_MASTER.郵便番号),
                電話番号 = GetValue(row, (int)TORIHIKI_AS400_MASTER.電話番号),
                FAX番号 = GetValue(row, (int)TORIHIKI_AS400_MASTER.FAX番号),

                // 住所は正規化処理を適用
                住所1 = StringNormalizer.NormalizeAddress(GetValue(row, (int)TORIHIKI_AS400_MASTER.住所1)),
                住所2 = StringNormalizer.NormalizeAddress(GetValue(row, (int)TORIHIKI_AS400_MASTER.住所2)),

                代表者 = GetValue(row, (int)TORIHIKI_AS400_MASTER.代表者),
                資本金 = GetDecimalValue(row, (int)TORIHIKI_AS400_MASTER.資本金),
                取引先部門 = GetValue(row, (int)TORIHIKI_AS400_MASTER.取引先部門),
                取引先担当者 = GetValue(row, (int)TORIHIKI_AS400_MASTER.取引先担当者),
                取引開始日 = GetValue(row, (int)TORIHIKI_AS400_MASTER.取引開始日),
                販売先区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.販売先区分),
                仕入先区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.仕入先区分),
                出荷先区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.出荷先区分),
                運送便区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.運送便区分),
                倉庫区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.倉庫区分),
                指示書発行倉庫区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.指示書発行倉庫区分),
                消費税有無区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.消費税有無区分),
                消費税計算区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.消費税計算区分),
                数量計算区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.数量計算区分),
                金額計算区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.金額計算区分),
                納品書出力区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.納品書出力区分),
                納品書出力行数 = GetIntValue(row, (int)TORIHIKI_AS400_MASTER.納品書出力行数),
                相手先取引先コード = GetValue(row, (int)TORIHIKI_AS400_MASTER.相手先取引先コード),
                買掛用締日 = GetValue(row, (int)TORIHIKI_AS400_MASTER.買掛用締日),
                買掛用支払日 = GetValue(row, (int)TORIHIKI_AS400_MASTER.買掛用支払日),
                買掛用手形サイト = GetValue(row, (int)TORIHIKI_AS400_MASTER.買掛用手形サイト),
                銀行コード = GetValue(row, (int)TORIHIKI_AS400_MASTER.銀行コード),
                銀行支店コード = GetValue(row, (int)TORIHIKI_AS400_MASTER.銀行支店コード),
                口座番号 = GetValue(row, (int)TORIHIKI_AS400_MASTER.口座番号),
                口座区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.口座区分),
                口座名義 = GetValue(row, (int)TORIHIKI_AS400_MASTER.口座名義),
                担当者コード1 = GetValue(row, (int)TORIHIKI_AS400_MASTER.担当者コード1),
                担当者コード2 = GetValue(row, (int)TORIHIKI_AS400_MASTER.担当者コード2),
                担当者コード3 = GetValue(row, (int)TORIHIKI_AS400_MASTER.担当者コード3),
                運送便コード1 = GetValue(row, (int)TORIHIKI_AS400_MASTER.運送便コード1),
                運送便コード2 = GetValue(row, (int)TORIHIKI_AS400_MASTER.運送便コード2),
                運送便コード3 = GetValue(row, (int)TORIHIKI_AS400_MASTER.運送便コード3),
                与信限度額1 = GetDecimalValue(row, (int)TORIHIKI_AS400_MASTER.与信限度額1),
                与信限度額2 = GetDecimalValue(row, (int)TORIHIKI_AS400_MASTER.与信限度額2),
                与信限度額3 = GetDecimalValue(row, (int)TORIHIKI_AS400_MASTER.与信限度額3),
                納品書発行区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.納品書発行区分),
                請求書発行区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.請求書発行区分),
                出荷案内書発行区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.出荷案内書発行区分),
                送り状発行区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.送り状発行区分),
                反番表示桁数 = GetIntValue(row, (int)TORIHIKI_AS400_MASTER.反番表示桁数),
                社内加工区分 = GetValue(row, (int)TORIHIKI_AS400_MASTER.社内加工区分),
                削除MK = GetValue(row, (int)TORIHIKI_AS400_MASTER.削除MK),
                作成日 = GetValue(row, (int)TORIHIKI_AS400_MASTER.作成日),
                作成時刻 = GetValue(row, (int)TORIHIKI_AS400_MASTER.作成時刻),
                更新日 = GetValue(row, (int)TORIHIKI_AS400_MASTER.更新日),
                更新時刻 = GetValue(row, (int)TORIHIKI_AS400_MASTER.更新時刻)
            };
        }

        
        /// <summary>
        /// 取引先マスタを部門ごとに展開したデータを取得
        /// ★正規化処理を含む
        /// </summary>
        public List<取引先部門展開> Get部門展開データ()
        {
            // 1. Repositoryからデータを取得
            var マスタDt = _repository.GetSQL取引先マスタ();
            var 履歴Dt = _repository.GetSQL取引先履歴();

            // 2. 履歴から部門マップを作成
            var 部門マップ = Create部門マップ(履歴Dt);

            // 3. マスタを部門ごとに展開
            var 結果 = new List<取引先部門展開>(); // プロパティ取引先部門展開のリスト実体作成

            foreach (DataRow row in マスタDt.Rows)
            {
                var 取引先コード = row["取引先コード"]?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(取引先コード)) continue; // 取引先コードが空の場合はスキップ

                // この取引先に紐づく部門コードを取得
                if (部門マップ.TryGetValue(取引先コード, out var 部門リスト))
                {
                    foreach (var 部門コード in 部門リスト)
                    {
                        // 取引履歴にある場合：各部門コードで展開
                        結果.Add(CreateEntityFromSQL(row, 部門コード));
                    }
                }
                else
                {
                    // 取引履歴にない場合：部門コードは空白で展開
                    結果.Add(CreateEntityFromSQL(row, ""));
                }
            }
            return 結果;
        }
        /// <summary>
        /// 履歴DataTanleから部門マップを作成
        /// </summary>
        public Dictionary<string, List<string>> Create部門マップ(DataTable 履歴Dt)
        {
            var マップ = new Dictionary<string, List<string>>(); // 取引先コードをキー、部門コードのリストを値とするマップディクショナリー実体作成
            foreach (DataRow row in 履歴Dt.Rows)
            {
                var 取引先コード = row["取引先コード"]?.ToString() ?? "";
                var 部門コード = row["部門コード"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(取引先コード) ||
                    string.IsNullOrWhiteSpace(部門コード)) continue; // 取引先コードまたは部門コードが空の場合はスキップ

                if (!マップ.ContainsKey(取引先コード))
                    マップ[取引先コード] = new List<string>(); // 取引先コードがマップに存在しない場合は新しいリストを作成して追加

                if (!マップ[取引先コード].Contains(部門コード))
                    マップ[取引先コード].Add(部門コード); // 取引先コードに対応する部門コードのリストに部門コードを追加（重複は追加しない）
            }
            return マップ;
        }
        /// <summary>
        /// DataRowから取引先部門展開エンティティを作成(正規化処理含む)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="部門コード"></param>
        /// <returns></returns>
        private 取引先部門展開 CreateEntityFromSQL(DataRow row, string 部門コード)
        {
            // 正規化処理を適用
            return new 取引先部門展開
            {
                取引先CD = row["取引先コード"]?.ToString() ?? "",
                部門CD = 部門コード,

                // ★SQL Serverから取得したデータは既に正規化済み
                取引先正式名 = row["取引先正式名"]?.ToString() ?? "",
                取引先名 = row["取引先名"]?.ToString() ?? "",
                取引先名カナ = row["カナ名"]?.ToString() ?? "",
                取引先略名 = row["略名"]?.ToString() ?? "",
                取引先略名カナ = row["カナ名"]?.ToString() ?? "",

                郵便番号 = row["郵便番号"]?.ToString() ?? "",
                電話番号1 = row["電話番号"]?.ToString() ?? "",
                電話番号2 = "",
                FAX番号1 = row["FAX番号"]?.ToString() ?? "",
                FAX番号2 = "",

                住所1 = row["住所1"]?.ToString() ?? "",
                住所1カナ = "",
                住所2 = row["住所2"]?.ToString() ?? "",
                住所2カナ = "",

                商社区分 = row["仕入先区分"]?.ToString() ?? "",
                仕入先区分 = row["仕入先区分"]?.ToString() ?? "",
                販売先区分 = row["販売先区分"]?.ToString() ?? "",
                得意先区分 = row["販売先区分"]?.ToString() ?? "",
                出荷先区分 = row["出荷先区分"]?.ToString() ?? "",
                預り先区分 = row["販売先区分"]?.ToString() ?? "",
                運送便区分 = row["運送便区分"]?.ToString() ?? "",
                倉庫区分 = row["倉庫区分"]?.ToString() ?? "",

                備考 = "",
                登録者 = CMD.UserID,
                登録日付 = CMD.HIZ,
                登録時刻 = DateTime.Now.ToString("HHmmss"),
            };
        }

        /// <summary>
        /// DataRowから安全に値を取得するヘルパーMethod
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetValue(DataRow row, int index)
        {
            try
            {
                if (index < 0 || index >= row.ItemArray.Length) return "";

                return row[index]?.ToString()?.Trim() ?? "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// DataRowから安全にdecimal値を取得
        /// </summary>
        private decimal GetDecimalValue(DataRow row, int index)
        {
            try
            {
                var value = GetValue(row, index);
                if (decimal.TryParse(value, out decimal result))
                    return result;
                return 0m;
            }
            catch
            {
                return 0m;
            }
        }

        /// <summary>
        /// DataRowから安全にint値を取得
        /// </summary>
        private int GetIntValue(DataRow row, int index)
        {
            try
            {
                var value = GetValue(row, index);
                if (int.TryParse(value, out int result))
                    return result;
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// SQL Serverから取得した部門展開データをマスタファイルに出力
        /// ★FormAction.ImportMasterと同じ処理結果を実現
        /// </summary>
        public(bool success, string message) Export部門展開ToMasterFile(
            List<取引先部門展開> データリスト,
            string BUMONmf, string mf,
            string mf_bumon, string[] mf_torirollTxtPaths)
        {
            try
            {
                // 1. TORIHIKI.txt 作成（全件差替え）
                Create取引先マスタファイル(データリスト, mf);

                // 2. TORIHIKI-BUMON.txt 作成（全件差替え）
                Create取引先部門マスタファイル(データリスト, mf_bumon);

                // 3. TROLE-XXXX.txt 作成（得意先のみ追加、他は全件差替え）
                Create取引先ロール別マスタファイル(データリスト, mf_torirollTxtPaths);

                return (true, "マスタファイルの出力が完了しました。");
            }
            catch (Exception ex)
            {
                return (false, $"マスタファイルの出力中にエラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// TORIHIKI.txt 作成（全件差替え）
        /// </summary>
        private void Create取引先マスタファイル(List<取引先部門展開> データリスト, string filePath)
        {
            // 取引先コードでユニーク化
            var uniqData = データリスト
                .GroupBy(x => x.取引先CD)
                .Select(g => g.First())
                .OrderBy(x => x.取引先CD)
                .ToList();

            var lines = new List<string>();
            foreach (var item in uniqData)
            {
                // FormAction.ImportMasterと同じフォーマット
                // 1:取引先CD 2:取引先正式名称 3:取引先名 4:取引先名カナ 5:取引先略名 6:取引先略名カナ
                // 7:郵便番号 8:電話番号1 9:電話番号2 10:FAX番号1 11:FAX番号2 12:住所1 13:住所1カナ
                // 14:住所2 15:住所2カナ 16:商社区分 17:仕入先区分 18:販売先区分 19:得意先区分
                // 20:出荷先区分 21:預り先区分 22:運送便区分 23:倉庫区分 24:備考 25:登録者 26:登録日付 27:登録時刻
                var fields = new[]
                {
                    item.取引先CD ?? "",
                    item.取引先正式名 ?? "",
                    item.取引先名 ?? "",
                    item.取引先名カナ ?? "",
                    item.取引先略名 ?? "",
                    item.取引先略名カナ ?? "",
                    item.郵便番号 ?? "",
                    item.電話番号1 ?? "",
                    item.電話番号2 ?? "",
                    item.FAX番号1 ?? "",
                    item.FAX番号2 ?? "",
                    item.住所1 ?? "",
                    item.住所1カナ ?? "",
                    item.住所2 ?? "",
                    item.住所2カナ ?? "",
                    item.商社区分 ?? "",
                    item.仕入先区分 ?? "",
                    item.販売先区分 ?? "",
                    item.得意先区分 ?? "",
                    item.出荷先区分 ?? "",
                    item.預り先区分 ?? "",
                    item.運送便区分 ?? "",
                    item.倉庫区分 ?? "",
                    item.備考 ?? "",
                    item.登録者 ?? CMD.UserID,
                    item.登録日付 ?? CMD.HIZ,
                    item.登録時刻 ?? DateTime.Now.ToString("HHmmss")
                };
                lines.Add(string.Join(" ", fields));
            }
            // バックアップ & 全件差替え
            BackupAndWrite(filePath, "TORIHIKI", lines);
        }

        /// <summary>
        /// TORIHIKI-BUMON 作成（全件差替え）
        /// </summary>
        private void Create取引先部門マスタファイル(List<取引先部門展開> データリスト, string filePath)
        {
            // 取引先CD + 部門CDでユニーク化
            var lines = データリスト
                .Where(x => !string.IsNullOrWhiteSpace(x.部門CD)) // 部門CDが空でないものに絞る
                .Select(x => $"{x.取引先CD} {x.部門CD}")
                .Distinct()
                .OrderBy(line =>
                {
                    var parts = line.Split(' ');
                    return (parts[0], parts[1]);
                })
                .ToList();
            // バックアップ & 全件差替え
            BackupAndWrite(filePath, "TORIHIKI-BUMON", lines);
        }

        /// <summary>
        /// TROLE-XXXX.txt 作成（得意先のみ追加、他は全件差替え）
        /// </summary>
        private void Create取引先ロール別マスタファイル(List<取引先部門展開> データリスト, string[] filePaths)
        {
            // 0:商社 1:仕入先 2:販売先 3:得意先 4:出荷先 5:預り先 6:運送便 7:倉庫
            var roleNames = new[] {"商社区分", "仕入先区分", "販売先区分", "得意先区分",
                                "出荷先区分", "預り先区分", "運送便区分", "倉庫区分"};

            for(int i = 0; i < roleNames.Length; i++)
            {
                var roleName = roleNames[i];
                var filePath = filePaths[i];

                // このロールに該当するデータを抽出
                var roleData = データリスト.Where(x => GetRoleValue(x, roleName) == "1").ToList();

                if(i == 3) //得意先のみ特別処理
                {
                    Create得意先マスタファイル(roleData, filePath);
                }
                else
                {
                    Createロールマスタファイル(roleData, filePath, Path.GetFileNameWithoutExtension(filePath));
                }
            }
        }

        private void Create得意先マスタファイル(List<取引先部門展開> データリスト, string filePath)
        {
            // 既存ファイル読込
            var existingLines = new List<string>();
            if (File.Exists(filePath))
            {
                existingLines = File.ReadAllLines(filePath, CMD.utf8)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();
            }
            // 新規得意先データ作成
            var newLinesList = データリスト
                .Where(x => !string.IsNullOrWhiteSpace(x.部門CD))
                .Select(x =>
                    $"{x.取引先CD} {x.部門CD} {x.取引先名} {x.取引先名カナ} 00000101 99991231 {CMD.HIZ} {DateTime.Now.ToString("HHmmss")}")
                .ToList();

            // 1~6項目で一致判定
            bool replaced = false;
            foreach (var newLine in newLinesList)
            {
                var newParts = newLine.Split(' ');
                // 1~6項目を取得
                var newKey = string.Join(" ", newParts.Take(6));

                bool found = false;
                for(int i = 0; i < existingLines.Count; i++)
                {
                    var existingParts = existingLines[i].Split(' ');
                    // 既存データの1~6項目を取得
                    var existingKey = string.Join(" ", existingParts.Take(6));

                    // 1~6項目が一致する場合は上書き
                    if (existingKey == newKey)
                    {
                        existingLines[i] = newLine; // 上書き
                        found = true;
                        replaced = true;
                        break;
                    }
                }
                // 一致するデータがなかった場合は新規追加
                if (!found)
                {
                    existingLines.Add(newLine);
                }
            }

            // ソート
            var sortedLines = existingLines
                .OrderBy(line =>
                {
                    var parts = line.Split(' ');
                    return parts.Length > 1 ? (parts[0], parts[1]) : (string.Empty, string.Empty);
                })
                .ToList();

            // バックアップ & 書き込み
            BackupAndWrite(filePath, Path.GetFileNameWithoutExtension(filePath), sortedLines);
        }


        /// <summary>
        /// 得意先以外のロールマスタファイル作成（全件差替え）
        /// </summary>
        private void Createロールマスタファイル(List<取引先部門展開> データリスト, string filePath, string fileName)
        {
            var lines = データリスト
                .Where(x => !string.IsNullOrWhiteSpace(x.部門CD)) // 部門CDが空でないものに絞る
                .Select(x => $"{x.取引先CD} {x.部門CD} {x.取引先名} {x.取引先名カナ}")
                .OrderBy(line =>
                {
                    var parts = line.Split(' ');
                    return parts.Length > 1 ? (parts[0], parts[1]) : (string.Empty, string.Empty);
                })
                .ToList();

            // バックアップ & 全件差替え
            BackupAndWrite(filePath, fileName, lines);
        }

        /// <summary>
        /// ロール区分の値を取得
        /// </summary>
        private string GetRoleValue(取引先部門展開 item, string roleName)
        {
            switch (roleName)
            {
                case "商社区分": return item.商社区分;
                case "仕入先区分": return item.仕入先区分;
                case "販売先区分": return item.販売先区分;
                case "得意先区分": return item.得意先区分;
                case "出荷先区分": return item.出荷先区分;
                case "預り先区分": return item.預り先区分;
                case "運送便区分": return item.運送便区分;
                case "倉庫区分": return item.倉庫区分;
                default: return "";
            }
        }

        /// <summary>
        /// バックアップ & ファイル書き込み
        /// </summary>
        private void BackupAndWrite(string filePath, string fileName, List<string> lines)
        {
            // バックアップ
            if (File.Exists(filePath))
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd.HHmmss");
                string bkFile = $"{fileName}_SQLExport.{timestamp}.txt.gz";
                string bkPath = Path.Combine(CMD.mfBkPath, bkFile);
                Directory.CreateDirectory(CMD.mfBkPath);
                // GZipで圧縮してバックアップ
                using (var originalFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var compressedFileStream = File.Create(bkPath))
                using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }

                // 元ファイルを削除
                File.Delete(filePath);

                // 古いバックアップ削除（3個まで保持）
                var oldBackups = Directory.GetFiles(CMD.mfBkPath, $"{fileName}*.txt.gz")
                    .OrderByDescending(f => f)
                    .Skip(3)
                    .ToList();
                foreach (var old in oldBackups)
                {
                    try { File.Delete(old); } catch { }
                }
            }

            // ファイル書き込み（全件差替え）
            File.WriteAllLines(filePath, lines, CMD.utf8);
        }

        /// <summary>
        /// 取引先コードで取引先情報を検索
        /// </summary>
        /// <param name="取引先コード"></param>
        /// <returns></returns>
        public 取引先Master Get取引先ByCode(string 取引先コード)
        {
            var dt = _repository.Get取引先マスタ();
            foreach (DataRow row in dt.Rows)
            {
                if (row["TOTHCD"]?.ToString()?.Trim() == 取引先コード)
                {
                    return MapToMaster(row);
                }
            }
            return null;
        }

        private 取引先Master MapToMaster(DataRow row)
        {
            return new 取引先Master
            {
                取引先コード = GetValue(row,(int)TORIHIKI_AS400_MASTER.取引先コード),
                取引先正式名 = GetValue(row,(int)TORIHIKI_AS400_MASTER.取引先正式名),
                取引先名 = GetValue(row,(int)TORIHIKI_AS400_MASTER.取引先名),
                略名 = GetValue(row,(int)TORIHIKI_AS400_MASTER.略名),
                カナ名 = GetValue(row,(int)TORIHIKI_AS400_MASTER.カナ名),
                郵便番号 = GetValue(row,(int)TORIHIKI_AS400_MASTER.郵便番号),
                電話番号 = GetValue(row,(int)TORIHIKI_AS400_MASTER.電話番号),
                FAX番号 = GetValue(row,(int)TORIHIKI_AS400_MASTER.FAX番号),
                住所1 = GetValue(row,(int)TORIHIKI_AS400_MASTER.住所1),
                住所2 = GetValue(row,(int)TORIHIKI_AS400_MASTER.住所2),
                取引先部門 = GetValue(row,(int)TORIHIKI_AS400_MASTER.取引先部門),
                取引先担当者 = GetValue(row,(int)TORIHIKI_AS400_MASTER.取引先担当者),
                取引開始日 = GetValue(row,(int)TORIHIKI_AS400_MASTER.取引開始日),
                販売先区分 = GetValue(row,(int)TORIHIKI_AS400_MASTER.販売先区分),
                仕入先区分 = GetValue(row,(int)TORIHIKI_AS400_MASTER.仕入先区分),
                出荷先区分 = GetValue(row,(int)TORIHIKI_AS400_MASTER.出荷先区分),
                運送便区分 = GetValue(row,(int)TORIHIKI_AS400_MASTER.運送便区分),
                倉庫区分 = GetValue(row,(int)TORIHIKI_AS400_MASTER.倉庫区分),
            };
        }
    }
}