using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    public partial class 取引先マスタ照会Fm : Form
    {
        private DataGridView dgv;
        private Button btnClose;
        private readonly string _placeholderText = "複数コード検索時は、カンマ/スペース/読点/改行のいずれかで区切ってください";
        //private readonly string _bumonPlaceholderText = "コード検索した取引先が紐づく部門を表示します";
        private Color _normalForeColor;
        private readonly Color _placeholderColor = SystemColors.GrayText;
        
        // 取引先部門マスタのパス
        private string mf_bumon = Path.Combine(CMD.mfPath, "TORIHIKI-BUMON.txt");
        private string BUMONmf = Path.Combine(CMD.mfPath, "BUMON.txt");
        
        // FormAction インスタンス
        private FormAction fam = new FormAction();
        
        public 取引先マスタ照会Fm()
        {
            InitializeComponent();
            // placeholder 初期設定（コード検索）
            _normalForeColor = txtBxコード検索.ForeColor;
            if (string.IsNullOrEmpty(txtBxコード検索.Text))
            {
                txtBxコード検索.Text = _placeholderText;
                txtBxコード検索.ForeColor = _placeholderColor;
            }
            else if (txtBxコード検索.Text == _placeholderText)
            {
                txtBxコード検索.ForeColor = _placeholderColor;
            }

            txtBxコード検索.Enter += TxtBxコード検索_Enter;
            txtBxコード検索.Leave += TxtBxコード検索_Leave;
            
            // listBx部門の初期表示設定
            SetBumonPlaceholder();
        }

        /// <summary>
        /// listBx部門にプレースホルダーを設定
        /// </summary>
        private void SetBumonPlaceholder()
        {
            listBx部門.Items.Clear();
            // 長いメッセージを2行に分けて表示
            listBx部門.Items.Add("コード検索した取引先が");
            listBx部門.Items.Add("紐づく部門を表示します");
            listBx部門.ForeColor = _placeholderColor;
        }

        /// <summary>
        /// データと会社名を設定
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="company"></param>
        public void SetData(List<string> lines)
        {
            if (lines == null) return;
            
            dgv.Columns.Clear();

            //  1:取引先CD   2:取引先正式名称 3:取引先名   4:取引先名カナ 5:取引先略名 6:取引先略名カナ 7:郵便番号 8:電話番号1   9:電話番号2
            // 10:FAX番号1  11:FAX番号2    12:住所1     13:住所1カナ  14:住所2    15:住所2カナ    16:商社区分 17:仕入先区分 18:販売先区分
            // 19:得意先区分 20:出荷先区分   21:預り先区分 22:運送便区分 23:倉庫区分  24:備考        25:登録者  26:登録日付   27:登録時刻
            var cols = Enum.GetNames(typeof(TORIHIKI_MASTER));
            foreach (var c in cols) dgv.Columns.Add(c, c);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(' ');
                var row = new string[cols.Length];
                for (int i = 0; i < cols.Length; i++) row[i] = i < parts.Length ? parts[i] : string.Empty;
                dgv.Rows.Add(row);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 入力された複数コードでフィルタリングを行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBxコード検索_TextChanged(object sender, EventArgs e)
        {
            //// プレースホルダ表示中はフィルタしない（全行表示）
            //var current = txtBxコード検索.Text;
            //if (string.IsNullOrWhiteSpace(current) || current == _placeholderText)
            //{
            //    foreach (DataGridViewRow row in dgv.Rows)
            //    {
            //        row.Visible = true;
            //    }
            //    // 部門リストボックスにプレースホルダーを再設定
            //    SetBumonPlaceholder();
            //    return;
            //}

            //// カンマ、スペース、読点、改行で区切られた複数コードを取得してトリム
            //var codes = current
            //    .Split(new char[] { ',', '、', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            //    .Select(s => s.Trim())
            //    .Where(s => s.Length > 0)
            //    .ToArray();

            //// 部分一致でマッチした取引先コードを収集
            //var matchedCodes = new HashSet<string>();

            //// 部分一致でマッチした行のみ表示、その他は非表示にする
            //foreach (DataGridViewRow row in dgv.Rows)
            //{
            //    var cellValue = row.Cells[(int)TORIHIKI_MASTER_INOUT.取引先CD].Value?.ToString();
            //    if (string.IsNullOrEmpty(cellValue))
            //    {
            //        row.Visible = false;
            //        continue;
            //    }

            //    // いずれかのコードがセル値に含まれていれば表示
            //    bool isMatch = codes.Any(code => cellValue.Contains(code));
            //    row.Visible = isMatch;
                
            //    if (isMatch)
            //    {
            //        matchedCodes.Add(cellValue);
            //    }
            //}

            //// 該当取引先コードの所属部門をlistBx部門に表示
            //UpdateBumonList(matchedCodes);
        }

        /// <summary>
        /// 該当取引先コードの所属部門をlistBx部門に更新表示（対象会社の部門のみ）
        /// </summary>
        /// <param name="torihikiCodes"></param>
        private void UpdateBumonList(HashSet<string> torihikiCodes)
        {
            listBx部門.Items.Clear();

            if (torihikiCodes == null || torihikiCodes.Count == 0)
            {
                // 該当なしの場合はプレースホルダーに戻す
                SetBumonPlaceholder();
                return;
            }

            try
            {
                // 取引先部門マスタを読込（1:取引先CD 2:部門CD）
                var lines_bumon = fam.CheckAndLoadMater(mf_bumon, "取引先部門マスタ", CMD.utf8, 1);
                
                // 部門マスタを読込（1:部門CD 2:部門名 3:部門名カナ 4:会社名）
                var lines_bumon_master = fam.CheckAndLoadMater(BUMONmf, "部門マスタ", CMD.utf8, 1);
                
                // 部門CD -> (部門名, 会社名) のマップ作成
                var bumonMap = new Dictionary<string, (string Name, string Company)>();
                foreach (var line in lines_bumon_master)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(' ');
                    if (parts.Length >= 4)
                    {
                        bumonMap[parts[0]] = (parts[1], parts[3]);
                    }
                }

                // 該当取引先の部門を収集（重複排除）
                var bumonSet = new HashSet<string>();
                
                foreach (var line in lines_bumon)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(' ');
                    if (parts.Length >= 2)
                    {
                        var toriCD = parts[0];
                        var bumonCD = parts[1];
                        
                        // 該当取引先コードの場合のみ部門を追加
                        if (torihikiCodes.Contains(toriCD))
                        {
                            bumonSet.Add(bumonCD);
                        }
                    }
                }

                // listBx部門に部門情報を表示（部門CD 部門名 形式）
                // ※対象会社が指定されている場合は、その会社の部門のみ表示
                int addedCount = 0;
                foreach (var bumonCD in bumonSet.OrderBy(x => x))
                {
                    if (bumonMap.TryGetValue(bumonCD, out var info))
                    {
                        listBx部門.Items.Add($"{bumonCD} {info.Name}");
                        listBx部門.ForeColor = _normalForeColor;
                        addedCount++;
                    }
                    else
                    {
                        listBx部門.Items.Add(bumonCD);
                        listBx部門.ForeColor = _normalForeColor;
                        addedCount++;
                    }
                }
                
                // 何も追加されなかった場合はプレースホルダーを表示
                if (addedCount == 0)
                {
                    SetBumonPlaceholder();
                }
            }
            catch (Exception ex)
            {
                // エラーが発生してもクラッシュしないように
                System.Diagnostics.Debug.WriteLine($"部門リスト更新エラー: {ex.Message}");
                SetBumonPlaceholder();
            }
        }

        private void TxtBxコード検索_Enter(object sender, EventArgs e)
        {
            if (txtBxコード検索.Text == _placeholderText)
            {
                txtBxコード検索.Text = string.Empty;
                txtBxコード検索.ForeColor = _normalForeColor;
            }
        }

        private void TxtBxコード検索_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBxコード検索.Text))
            {
                txtBxコード検索.Text = _placeholderText;
                txtBxコード検索.ForeColor = _placeholderColor;
            }
        }

        private void btn検索_Click(object sender, EventArgs e)
        {
            // プレースホルダ表示中はフィルタしない（全行表示）
            var current = txtBxコード検索.Text;
            if (string.IsNullOrWhiteSpace(current) || current == _placeholderText)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.Visible = true;
                }
                // 部門リストボックスにプレースホルダーを再設定
                SetBumonPlaceholder();
                return;
            }

            // カンマ、スペース、読点、改行で区切られた複数コードを取得してトリム
            var codes = current
                .Split(new char[] { ',', '、', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToArray();

            // 部分一致でマッチした取引先コードを収集
            var matchedCodes = new HashSet<string>();

            // 部分一致でマッチした行のみ表示、その他は非表示にする
            foreach (DataGridViewRow row in dgv.Rows)
            {
                var cellValue = row.Cells[(int)TORIHIKI_MASTER_INOUT.取引先CD].Value?.ToString();
                if (string.IsNullOrEmpty(cellValue))
                {
                    row.Visible = false;
                    continue;
                }

                // いずれかのコードがセル値に含まれていれば表示
                bool isMatch = codes.Any(code => cellValue.Contains(code));
                row.Visible = isMatch;

                if (isMatch)
                {
                    matchedCodes.Add(cellValue);
                }
            }

            // 該当取引先コードの所属部門をlistBx部門に表示
            UpdateBumonList(matchedCodes);
        }
    }
}

