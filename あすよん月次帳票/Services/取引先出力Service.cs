using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;
using Path = System.IO.Path;
using あすよん月次帳票.Services;
using あすよん月次帳票.Models;

namespace あすよん月次帳票
{
    internal class 取引先出力Service
    {
        FormAction fam = new FormAction();

        // Service層 = ビジネスロジックの担当者
        // •「データをどう加工するか」を知っている
        // •複数のRepositoryからデータを集めて組み合わせる
        private readonly 取引先取得Service _取引先Service = new 取引先取得Service();

        string mst = "AS400取引先マスタ";
        string BUMONmf = Path.Combine(CMD.mfPath, "BUMON.txt");

        string mf = Path.Combine(CMD.mfPath, "TORIHIKI.txt");
        string mf_bumon = Path.Combine(CMD.mfPath, "TORIHIKI-BUMON.txt");
        string mf_bumon_omit = Path.Combine(CMD.mfPath, "TORIHIKI-BUMON_omit.txt");
        string[] mf_torirolePaths = new[]
        {
            Path.Combine(CMD.mfPath, "TROLE-SYOSYA.txt"),
            Path.Combine(CMD.mfPath, "TROLE-SIIRE.txt"),
            Path.Combine(CMD.mfPath, "TROLE-HANBAI.txt"),
            Path.Combine(CMD.mfPath, "TROLE-TOKUISAKI.txt"),
            Path.Combine(CMD.mfPath, "TROLE-SYUKKA.txt"),
            Path.Combine(CMD.mfPath, "TROLE-AZUKARI.txt"),
            Path.Combine(CMD.mfPath, "TROLE-UNSOU.txt"),
            Path.Combine(CMD.mfPath, "TROLE-SOUKO.txt"),
        };

        /// <summary>
        /// 取引先データを部門コードで展開してDataTableを作成
        /// </summary>
        /// <returns></returns>
        internal void Create取引先部門展開Table(ToriBumonbtnPattern pattern)
        {
            try
            {
                // 2. Serviseから部門展開データを取得(正規化済)
                var 部門展開リスト = _取引先Service.Get部門展開データ();

                // 2. 部門マスタでフィルタリング
                var allowedBumons = Get許可部門リスト();
                var フィルタとソート済リスト = 部門展開リスト
                    .Where(x => string.IsNullOrWhiteSpace(x.部門CD) || allowedBumons.Contains(x.部門CD))
                    .OrderBy(x=> x.取引先CD)
                    .ThenBy(x => x.部門CD)
                    .ToList();

                if(pattern == ToriBumonbtnPattern.出力)
                {
                    // Excel出力
                    Excel出力処理(フィルタとソート済リスト);
                }
                else if(pattern == ToriBumonbtnPattern.反映)
                {
                    // マスタファイル出力
                    マスタファイル出力処理(フィルタとソート済リスト);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラーが発生しました。\n\n{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///  Excel出力
        /// </summary>
        private void Excel出力処理(List<取引先部門展開> データリスト)
        {
            // 出力件数を入力
            int? 出力件数 = Get出力件数(データリスト.Count);
            if (出力件数 == null) return;

            // Excel出力Serviceに委譲
            var excelService = new Excel出力Service();
            excelService.Export部門展開データToExcel(データリスト, mst + ".xlsx", 出力件数);

            MessageBox.Show("取引先部門展開データの出力が完了しました。",
                "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 出力件数を入力取得
        /// </summary>
        private int? Get出力件数(int 最大件数)
        {
            string inputValue = Interaction.InputBox(
                $"出力件数を入力してください。\n（全{最大件数}件）\n\n※空欄の場合は全件出力されます。",
                "出力件数指定",
                "",
                -1,
                -1);

            // 空欄の場合は全件
            if (string.IsNullOrEmpty(inputValue))
            {
                return 最大件数;
            }

            // 数値として解析
            if (int.TryParse(inputValue, out int parsedCount))
            {
                if (parsedCount > 0 && parsedCount <= 最大件数)
                {
                    return parsedCount;
                }
                else if (parsedCount > 最大件数)
                {
                    MessageBox.Show($"指定件数が全件数（{最大件数}件）を超えています。\n全件で出力します。",
                        "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 最大件数;
                }
                else
                {
                    MessageBox.Show("出力件数は1以上を指定してください。",
                        "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("数値を入力してください。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        /// <summary>
        /// ★③の処理: マスタファイル出力（全件差替え）
        /// </summary>
        private void マスタファイル出力処理(List<取引先部門展開> データリスト)
        {
            // 確認メッセージ
            var result = MessageBox.Show(
                $"SQL Serverのデータ（{データリスト.Count}件）でマスタファイルを全件差替えします。\n\n" +
                "対象ファイル:\n" +
                "・TORIHIKI(取引先マスタ)\n" +
                "・TORIHIKI-BUMON(取引先部門マスタ)\n" +
                "・TROLE-*(取引先ロール別マスタ/8ファイル）\n\n" +
                "※旧ファイルは圧縮してバックアップされます\n\n" +
                "よろしいですか？",
                "マスタファイル出力確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            try
            {
                // Service層のExportを呼び出し
                var (success, message) = _取引先Service.Export部門展開ToMasterFile(
                    データリスト,
                    BUMONmf,
                    mf,
                    mf_bumon,
                    mf_torirolePaths);

                if (success)
                {
                    MessageBox.Show(
                        $"{message}\n\n" +
                        $"出力件数: {データリスト.Count}件\n" +
                        $"出力日時: {DateTime.Now:yyyy/MM/dd HH:mm:ss}",
                        "完了",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // ログ記録
                    string HIZTIM = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    fam.AddLog($"{HIZTIM} マスタファイル出力 1 {CMD.UserName} Create取引先部門展開Table {mst}");
                    fam.AddLog2($"{HIZTIM} マスタファイル出力 0 {CMD.UserName} {mst}が更新されました（{データリスト.Count}件）");
                }
                else
                {
                    MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"マスタファイル出力中にエラーが発生しました。\n\n{ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 部門マスタから許可された部門リストを取得
        /// </summary>
        /// <returns></returns>
        private HashSet<string> Get許可部門リスト()
        {
            var allowedBumons = new HashSet<string>();
            var bumonMasterLines = fam.CheckAndLoadMater(BUMONmf, "部門マスタ", CMD.utf8, 1);

            foreach (var bmLine in bumonMasterLines)
            {
                if(string.IsNullOrWhiteSpace(bmLine)) continue;
                var bmParts = bmLine.Split(' ');
                if (bmParts.Length >= 4)
                {
                    allowedBumons.Add(bmParts[(int)BUMON_MASTER.部門CD]);
                }
            }
            return allowedBumons;
        }
    }
}

