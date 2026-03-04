using IBM.Data.DB2.iSeries;
using Ohno.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using あすよん月次帳票.Models;

namespace あすよん月次帳票.Repositories
{
    /// <summary>
    /// 取引先データアクセスインターフェース
    /// </summary>
    public interface I取引先Repository
    {
        // ---- AS400関連 ----
        DataTable Get取引先マスタ();
        DataTable Get取引先履歴();
        void SaveAS400取引先マスタToSqlServer(List<取引先Master> マスタリスト);
        void SaveAS400取引先部門マスタToSqlServer(List<取引部門履歴> マスタリスト);

        // ---- SQL Server関連 ----
        DataTable GetSQL取引先マスタ();
        DataTable GetSQL取引先履歴();
    }

    /// <summary>
    /// 取引先データアクセス実装
    /// </summary>
    public class 取引先Repository : I取引先Repository
    {
        private readonly DbManager_Db2 _dbManager;
        private const int MaxRetries = 1; // リトライ1回 = 計2回実行
        private const int RetryDelayMilliseconds = 3000; // 3秒待機

        public 取引先Repository(DbManager_Db2 dbManager)
        {
            _dbManager = dbManager;
        }

        // デフォルトコンストラクタ(既存コードとの互換の為)
        public 取引先Repository()
            : this((DbManager_Db2)DbManager.CreateDbManager(OhnoSysDBName.Db2))
        {
        }

        /// <summary>
        /// リトライロジック付きでデータ取得を実行
        /// </summary>
        private T ExecuteWithRetry<T>(Func<T> operation, string operationName)
        {
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount <= MaxRetries)
            {
                try
                {
                    return operation();
                }
                catch (Ohno.Db.ConnectionFailedException ex)
                {
                    lastException = ex;
                    retryCount++;

                    if (retryCount > MaxRetries)
                    {
                        // リトライ回数を超えた場合は例外を再スロー
                        throw new Exception(
                            $"【{operationName}】データベース接続に失敗しました。\n" +
                            $"試行回数: {retryCount}回\n" +
                            $"エラー: {ex.Message}\n\n" +
                            $"ネットワーク接続を確認して、もう一度実行してください。",
                            ex);
                    }

                    // リトライ前に3秒待機
                    Thread.Sleep(RetryDelayMilliseconds);
                }
                catch (Exception ex)
                {
                    // ConnectionFailedException以外の例外はリトライせずに即座にスロー
                    throw new Exception($"【{operationName}】予期しないエラーが発生しました。\n{ex.Message}", ex);
                }
            }

            // ここには到達しないはずだが、念のため
            throw lastException ?? new Exception($"【{operationName}】不明なエラーが発生しました。");
        }

        /// <summary>
        /// AS400から取引先マスタを取得する
        /// </summary>
        /// <returns></returns>
        public DataTable Get取引先マスタ()
        {
            return ExecuteWithRetry(() =>
            {
                const string sql = @"SELECT * FROM SM1MLB01.MMTORIP";
                return _dbManager.GetDataTable(sql);
            }, "取引先マスタ取得");
        }

        /// <summary>
        /// SQL Serverから取引先マスタを取得する
        /// </summary>
        /// <returns></returns>
        public DataTable GetSQL取引先マスタ()
        {
            return ExecuteWithRetry(() =>
            {
                var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");
                const string sql = @"SELECT * FROM Torihiki_AS400";

                var dt = dbManager.GetDataTable(sql, CommandType.Text);
                return dt;
            }, "SQL Sever取引先マスタ取得");
        }

        /// <summary>
        /// AS400から取引先履歴を取得する
        /// </summary>
        /// <returns></returns>
        public DataTable Get取引先履歴()
        {
            return ExecuteWithRetry(() =>
            {
                var libraryNames = new[]
                {
                    new[]{"SM1DLB01"}, //オーノ
                    new[]{"SM1DLB02", "SM1DLB03"} //サンミック
                };

                DataTable slDt = null;
                DataTable prDt = null;

                foreach (var libraries in libraryNames)
                {
                    foreach (var libraryName in libraries)
                    {
                        var slTable = Get売上履歴(libraryName);
                        slDt = slDt == null ? slTable : MergeTables(slDt, slTable);

                        var prTable = Get仕入履歴(libraryName);
                        prDt = prDt == null ? prTable : MergeTables(prDt, prTable);
                    }
                }
                return UnionAndSort(slDt, prDt);
            }, "取引先履歴取得");
        }

        /// <summary>
        /// SQL Serverから取引先履歴を取得する
        /// </summary>
        /// <returns></returns>
        public DataTable GetSQL取引先履歴()
        {
            return ExecuteWithRetry(() =>
            {
                var dbManager = (DbManager_Sql)DbManager_Sql.CreateDbManager("AS400MonthlyFormDb");
                const string sql = @"SELECT * FROM TxnHistory";

                var dt = dbManager.GetDataTable(sql, CommandType.Text);
                return dt;
            }, "SQL Server取引先履歴取得");
        }

        /// <summary>
        /// 正規済み取引先マスタをSQL Serverに保存する
        /// </summary>
        /// <param name="マスタリスト"></param>
        public void SaveAS400取引先マスタToSqlServer(List<取引先Master> マスタリスト)
        {
            // 1. SQL Server接続
            var sqlDbManager = (DbManager_Sql)DbManager.CreateDbManager("AS400MonthlyFormDB");

            // 2. BulkCopy用のDataTable作成
            var bulkData = ConvertToDataTable_ToriMST(マスタリスト);

            // 3. トランザクション + DELETE + BulkCopy
            using (SqlConnection conn = (SqlConnection)sqlDbManager.Connection)
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 3-1. 既存データを削除
                        var delCommand = new SqlCommand("DELETE FROM Torihiki_AS400", conn, trans);
                        delCommand.ExecuteNonQuery();

                        // 3-2. BulkCopyで新データを挿入
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                        {
                            bulkCopy.DestinationTableName = "Torihiki_AS400";
                            bulkCopy.BatchSize = 1000;
                            bulkCopy.BulkCopyTimeout = 600;

                            // ★ SQL Serverのテーブル構造を取得
                            var schemaTable = GetSqlServerTableSchema(conn, trans, "Torihiki_AS400");

                            // ★ DataTableのカラムのうち、SQL Serverに存在するものだけをマッピング
                            foreach (DataColumn column in bulkData.Columns)
                            {
                                if (schemaTable.Contains(column.ColumnName))
                                {
                                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                                }
                            }

                            bulkCopy.WriteToServer(bulkData);
                        }
                        // 3-3. トランザクションコミット
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        // エラー発生時はロールバック
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SaveAS400取引先部門マスタToSqlServer(List<取引部門履歴> マスタリスト)
        {
            // 1. SQL Server接続
            var sqlDbManager = (DbManager_Sql)DbManager.CreateDbManager("AS400MonthlyFormDB");

            // 2. BulkCopy用のDataTable作成
            var bulkData = ConvertToDataTable_TxnHis(マスタリスト);

            // 3. トランザクション + DELETE + BulkCopy
            using (SqlConnection conn = (SqlConnection)sqlDbManager.Connection)
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 3-1. 既存データを削除
                        var delCommand = new SqlCommand("DELETE FROM TxnHistory", conn, trans);
                        delCommand.ExecuteNonQuery();

                        // 3-2. BulkCopyで新データを挿入
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
                        {
                            bulkCopy.DestinationTableName = "TxnHistory";
                            bulkCopy.BatchSize = 1000;
                            bulkCopy.BulkCopyTimeout = 600;

                            // ★ SQL Serverのテーブル構造を取得
                            var schemaTable = GetSqlServerTableSchema(conn, trans, "TxnHistory");

                            // ★ DataTableのカラムのうち、SQL Serverに存在するものだけをマッピング
                            foreach (DataColumn column in bulkData.Columns)
                            {
                                if (schemaTable.Contains(column.ColumnName))
                                {
                                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                                }
                            }

                            bulkCopy.WriteToServer(bulkData);
                        }
                        // 3-3. トランザクションコミット
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        // エラー発生時はロールバック
                        trans.Rollback();
                        throw;
                    }
                }
            }

        }
        /// <summary>
        /// SQL Serverのテーブル構造（カラム名リスト）を取得
        /// </summary>
        private HashSet<string> GetSqlServerTableSchema(SqlConnection conn, SqlTransaction trans, string tableName)
        {
            var columns = new HashSet<string>();

            var sql = @"
                SELECT COLUMN_NAME 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = @TableName";

            using (var cmd = new SqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@TableName", tableName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }
            }

            return columns;
        }

        /// <summary>
        /// List<取引先Master>をDataTableに変換(BulkCopy用)
        /// </summary>
        /// <param name="リスト"></param>
        /// <returns></returns>
        private DataTable ConvertToDataTable_ToriMST(List<取引先Master> リスト)
        {
            var dt = new DataTable();

            // 列定義(SQL Serverのテーブル構造に合わせる)
            dt.Columns.Add("取引先コード", typeof(string));
            dt.Columns.Add("取引先正式名", typeof(string));
            dt.Columns.Add("取引先名", typeof(string));
            dt.Columns.Add("略名", typeof(string));
            dt.Columns.Add("カナ名", typeof(string));
            dt.Columns.Add("郵便番号", typeof(string));
            dt.Columns.Add("電話番号", typeof(string));
            dt.Columns.Add("FAX番号", typeof(string));
            dt.Columns.Add("住所1", typeof(string));
            dt.Columns.Add("住所2", typeof(string));
            dt.Columns.Add("代表者", typeof(string));
            dt.Columns.Add("資本金", typeof(decimal)); // ★ NULL許可のため、後でDBNull.Valueに変換
            dt.Columns.Add("取引先部門", typeof(string));
            dt.Columns.Add("取引先担当者", typeof(string));
            dt.Columns.Add("取引開始日", typeof(string));
            dt.Columns.Add("販売先区分", typeof(string));
            dt.Columns.Add("仕入先区分", typeof(string));
            dt.Columns.Add("出荷先区分", typeof(string));
            dt.Columns.Add("運送便区分", typeof(string));
            dt.Columns.Add("倉庫区分", typeof(string));
            dt.Columns.Add("指示書発行倉庫区分", typeof(string));
            dt.Columns.Add("消費税有無区分", typeof(string));
            dt.Columns.Add("消費税計算区分", typeof(string));
            dt.Columns.Add("数量計算区分", typeof(string));
            dt.Columns.Add("金額計算区分", typeof(string));
            dt.Columns.Add("納品書出力区分", typeof(string));
            dt.Columns.Add("納品書出力行数", typeof(int)); // ★ int型のまま
            dt.Columns.Add("相手先取引先コード", typeof(string));
            dt.Columns.Add("買掛用締日", typeof(string));
            dt.Columns.Add("買掛用支払日", typeof(string));
            dt.Columns.Add("買掛用手形サイト", typeof(string));
            dt.Columns.Add("銀行コード", typeof(string));
            dt.Columns.Add("銀行支店コード", typeof(string));
            dt.Columns.Add("口座番号", typeof(string));
            dt.Columns.Add("口座区分", typeof(string));
            dt.Columns.Add("口座名義", typeof(string));
            dt.Columns.Add("担当者コード1", typeof(string));
            dt.Columns.Add("担当者コード2", typeof(string));
            dt.Columns.Add("担当者コード3", typeof(string));
            dt.Columns.Add("運送便コード1", typeof(string));
            dt.Columns.Add("運送便コード2", typeof(string));
            dt.Columns.Add("運送便コード3", typeof(string));
            dt.Columns.Add("与信限度額1", typeof(decimal));
            dt.Columns.Add("与信限度額2", typeof(decimal));
            dt.Columns.Add("与信限度額3", typeof(decimal));
            dt.Columns.Add("納品書発行区分", typeof(string));
            dt.Columns.Add("請求書発行区分", typeof(string));
            dt.Columns.Add("出荷案内書発行区分", typeof(string));
            dt.Columns.Add("送り状発行区分", typeof(string));
            dt.Columns.Add("反番表示桁数", typeof(int)); // ★ int型のまま
            dt.Columns.Add("社内加工区分", typeof(string));
            dt.Columns.Add("削除MK", typeof(string));
            dt.Columns.Add("作成日", typeof(string));
            dt.Columns.Add("作成時刻", typeof(string));
            dt.Columns.Add("更新日", typeof(string));
            dt.Columns.Add("更新時刻", typeof(string));
            dt.Columns.Add("更新PGM", typeof(string));

            // int型とdecimal型の列にNULL許可を設定
            dt.Columns["納品書出力行数"].AllowDBNull = true;
            dt.Columns["反番表示桁数"].AllowDBNull = true;
            dt.Columns["資本金"].AllowDBNull = true;
            dt.Columns["与信限度額1"].AllowDBNull = true;
            dt.Columns["与信限度額2"].AllowDBNull = true;
            dt.Columns["与信限度額3"].AllowDBNull = true;

            // データ行の追加
            foreach (var item in リスト)
            {
                dt.Rows.Add(
                    item.取引先コード ?? "",
                    item.取引先正式名 ?? "",
                    item.取引先名 ?? "",
                    item.略名 ?? "",
                    item.カナ名 ?? "",
                    item.郵便番号 ?? "",
                    item.電話番号 ?? "",
                    item.FAX番号 ?? "",
                    item.住所1 ?? "",
                    item.住所2 ?? "",
                    item.代表者 ?? "",
                    item.資本金 == 0 ? (object)DBNull.Value : item.資本金, // 0ならNULLとして扱う
                    item.取引先部門 ?? "",
                    item.取引先担当者 ?? "",
                    item.取引開始日 ?? "",
                    item.販売先区分 ?? "",
                    item.仕入先区分 ?? "",
                    item.出荷先区分 ?? "",
                    item.運送便区分 ?? "",
                    item.倉庫区分 ?? "",
                    item.指示書発行倉庫区分 ?? "",
                    item.消費税有無区分 ?? "",
                    item.消費税計算区分 ?? "",
                    item.数量計算区分 ?? "",
                    item.金額計算区分 ?? "",
                    item.納品書出力区分 ?? "",
                    item.納品書出力行数 == 0 ? (object)DBNull.Value : item.納品書出力行数, // 0ならNULLとして扱う
                    item.相手先取引先コード ?? "",
                    item.買掛用締日 ?? "",
                    item.買掛用支払日 ?? "",
                    item.買掛用手形サイト ?? "",
                    item.銀行コード ?? "",
                    item.銀行支店コード ?? "",
                    item.口座番号 ?? "",
                    item.口座区分 ?? "",
                    item.口座名義 ?? "",
                    item.担当者コード1 ?? "",
                    item.担当者コード2 ?? "",
                    item.担当者コード3 ?? "",
                    item.運送便コード1 ?? "",
                    item.運送便コード2 ?? "",
                    item.運送便コード3 ?? "",
                    item.与信限度額1 == 0 ? (object)DBNull.Value : item.与信限度額1,
                    item.与信限度額2 == 0 ? (object)DBNull.Value : item.与信限度額2,
                    item.与信限度額3 == 0 ? (object)DBNull.Value : item.与信限度額3,
                    item.納品書発行区分 ?? "",
                    item.請求書発行区分 ?? "",
                    item.出荷案内書発行区分 ?? "",
                    item.送り状発行区分 ?? "",
                    item.反番表示桁数 == 0 ? (object)DBNull.Value : item.反番表示桁数,  // ★ 0ならNULLに変換
                    item.社内加工区分 ?? "",
                    item.削除MK ?? "",
                    item.作成日 ?? "",
                    item.作成時刻 ?? "",
                    item.更新日 ?? "",
                    item.更新時刻 ?? "",
                    item.更新PGM ?? ""
                    );
            }
            return dt;
        }

        private DataTable ConvertToDataTable_TxnHis(List<取引部門履歴> リスト)
        {
            var dt = new DataTable();

            // 列定義(SQL Serverのテーブル構造に合わせる)
            dt.Columns.Add("取引先コード", typeof(string));
            dt.Columns.Add("部門コード", typeof(string));

            // データ行の追加
            foreach (var item in リスト)
            {
                dt.Rows.Add(
                    item.取引先コード ?? "",
                    item.部門コード ?? ""
                    );
            }
            return dt;
        }

        private DataTable Get売上履歴(string libraryName)
        {
            string sql = $@"
                        SELECT SL.URHBSC, SL.URBMCD
                        FROM {libraryName}.SLURIMP AS SL
                        LEFT JOIN SM1MLB01.MMTORIP AS PM
                                        ON SL.URHBSC = PM.TOTHCD
                         WHERE SL.URDNDT >= 20030101 
                         GROUP BY SL.URBMCD, SL.URHBSC
                         ORDER BY SL.URBMCD, MIN(PM.TOKANM)";
            var table = _dbManager.GetDataTable(sql);
            RenameColumns(table);
            return table;
        }

        private DataTable Get仕入履歴(string libraryName)
        {
            string sql = $@"
                        SELECT PR.SRSRCD, PR.SRBMCD
                        FROM {libraryName}.PRSREMP AS PR
                        LEFT JOIN SM1MLB01.MMTORIP AS PM
                            ON PR.SRSRCD = PM.TOTHCD
                        WHERE PR.SRDNDT >= 20030101 
                        GROUP BY PR.SRBMCD, PR.SRSRCD
                        ORDER BY PR.SRBMCD, MIN(PM.TOKANM)";
            var table = _dbManager.GetDataTable(sql);
            RenameColumns(table);
            return table;
        }

        private void RenameColumns(DataTable table)
        {
            table.Columns[0].ColumnName = "取引先コード";
            table.Columns[1].ColumnName = "部門コード";
        }

        private DataTable MergeTables(DataTable dt1, DataTable dt2)
        {
            dt1.Merge(dt2);
            return dt1;
        }

        private DataTable UnionAndSort(DataTable slDt, DataTable prDt)
        {
            if (slDt == null) slDt = prDt?.Clone() ?? new DataTable();
            if (prDt == null) prDt = slDt.Clone();

            return slDt.AsEnumerable()
                .Union(prDt.AsEnumerable())
                .GroupBy(row => new
                {
                    TorihikiCode = row.Field<string>("取引先コード"),
                    BumonCode = row.Field<string>("部門コード")
                })
                .Select(g => g.First())
                .OrderBy(row => row.Field<string>("取引先コード"))
                .CopyToDataTable();
        }
    }
}
