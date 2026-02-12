using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CMD = あすよん月次帳票.CommonData;

namespace あすよん月次帳票
{
    internal partial class 販売仕入先Form : Form
    {
        private string mode;  // "HANBAI" or "SHIIRE"
        private List<Torihiki> initialSelected;  // DataExtractionFm から受け取る
        private Dictionary<string, string> deptCodeToCompany; // 部門コードから会社名へのマッピング
        private Dictionary<string, string> deptCodeToName; // 部門コードから部門名へのマッピング
        private Dictionary<string, List<dynamic>> masterData;  // マスター読込結果 (会社キー -> MfItemリスト)
        private Dictionary<string, HashSet<string>> preselectedMap; // RplForm2 から受け取る事前選択データマップ

        internal 販売仕入先Form(string mode, List<Torihiki> selectedItems, Dictionary<string, HashSet<string>> existingMap)
        {
            InitializeComponent();
            this.Load += 販売仕入先Form_Load;

            this.mode = mode;  // モード保持
            this.initialSelected = selectedItems ?? new List<Torihiki>();
            this.preselectedMap = existingMap ?? new Dictionary<string, HashSet<string>>();

            treeView販売仕入.NodeMouseClick += TreeView販売仕入_NodeMouseClick;
        }

        internal void 販売仕入先Form_Load(object sender, EventArgs e)
        {
            // 部門CD→会社/部門名 マッピング作成 (BUMON.txt を利用)
            deptCodeToCompany = new Dictionary<string, string>();
            deptCodeToName = new Dictionary<string, string>();
            string bumonPath = Path.Combine(CMD.mfPath, "BUMON.txt");
            if (File.Exists(bumonPath))
            {
                foreach (var line in File.ReadLines(bumonPath, CMD.utf8 ?? System.Text.Encoding.Default))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    // 列は空白区切り。先頭が部門CD、末尾が会社名、2番目が部門名の想定
                    var toks = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (toks.Length >= 1)
                    {
                        var code = toks[0].Trim();
                        var company = toks.Length >= 4 ? toks[3].Trim() : (toks.Length >= 2 ? toks[toks.Length - 1].Trim() : "");
                        var name = toks.Length >= 2 ? toks[1].Trim() : string.Empty;
                        if (!string.IsNullOrEmpty(code))
                        {
                            deptCodeToCompany[code] = company;
                            deptCodeToName[code] = name;
                        }
                    }
                }
            }

            // マスター読み込み (TORIHIKI系テキストを利用)
            LoadAllMasters();
            InitTreeView();

            // ListBoxに復元
            foreach (var item in initialSelected)
                listBx販売仕入.Items.Add(item);

            // ← TreeView 構築完了後、UIが整ってから復元をかける
            this.BeginInvoke(new Action(() =>
            {
                // AfterCheck を止めてから
                treeView販売仕入.AfterCheck -= TreeView販売仕入_AfterCheck;
                // TreeViewのチェック状態を復元
                RestoreTreeViewChecked(treeView販売仕入.Nodes);
                treeView販売仕入.AfterCheck += TreeView販売仕入_AfterCheck;

                // 最初のノードに選択を合わせる
                if (treeView販売仕入.Nodes.Count > 0)
                    treeView販売仕入.SelectedNode = treeView販売仕入.Nodes[0];
            }));
        }

        // 全会社のマスタデータを読み込み
        private void LoadAllMasters()
        {
            // Read TORIHIKI master and role files to build company->items map
            masterData = new Dictionary<string, List<dynamic>>();
            string[] companyCodes = { "オーノ", "サンミックダスコン", "サンミックカーペット" };

            // Load TORIHIKI (code -> name)
            var torihikiName = new Dictionary<string, string>();
            string torihikiPath = Path.Combine(CMD.mfPath, "TORIHIKI.txt");
            if (File.Exists(torihikiPath))
            {
                foreach (var line in File.ReadLines(torihikiPath, CMD.utf8 ?? System.Text.Encoding.Default))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var toks = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (toks.Length >= 1)
                    {
                        var code = toks[0].Trim();
                        // try to find a reasonable name field: prefer index 3 or 2 or 1
                        string name = string.Empty;
                        if (toks.Length > 3) name = toks[3].Trim();
                        else if (toks.Length > 2) name = toks[2].Trim();
                        else if (toks.Length > 1) name = toks[1].Trim();
                        if (!string.IsNullOrEmpty(code) && !torihikiName.ContainsKey(code))
                            torihikiName[code] = name;
                    }
                }
            }

            // Choose role file based on mode
            string roleFile = mode == "HANBAI" ? "TROLE-HANBAI.txt" : "TROLE-SIIRE.txt";
            string rolePath = Path.Combine(CMD.mfPath, roleFile);

            // Build valid bumon sets per company
            var validByCompany = new Dictionary<string, HashSet<string>>();
            foreach (var cc in companyCodes)
                validByCompany[cc] = new HashSet<string>(deptCodeToCompany.Where(kv => kv.Value == cc).Select(kv => kv.Key));

            // Initialize company lists
            foreach (var cc in companyCodes)
                masterData[cc] = new List<dynamic>();

            if (File.Exists(rolePath))
            {
                foreach (var line in File.ReadLines(rolePath, CMD.utf8 ?? System.Text.Encoding.Default))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var toks = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (toks.Length < 2) continue;
                    var code = toks[0].Trim();
                    var bumon = toks[1].Trim();
                    string name = toks.Length > 2 ? toks[2].Trim() : (torihikiName.ContainsKey(code) ? torihikiName[code] : string.Empty);

                    if (!deptCodeToCompany.TryGetValue(bumon, out var company)) continue; // skip unknown bumon
                    if (!validByCompany.ContainsKey(company) || !validByCompany[company].Contains(bumon)) continue;

                    masterData[company].Add(new MfItem
                    {
                        Code = code,
                        Name = name,
                        Kana = string.Empty,
                        DeptCode = bumon
                    });
                }
            }
        }

        private void InitTreeView(IEnumerable<dynamic> filteredItems = null)
        {
            treeView販売仕入.Nodes.Clear();
            var companyNodes = new Dictionary<string, TreeNode>();

            // 全会社のルートノードを作成
            foreach (var comp in new[] { "オーノ", "サンミックダスコン", "サンミックカーペット" })
            {
                var cNode = new TreeNode(comp) { Tag = comp, Checked = false };
                companyNodes[comp] = cNode;
                treeView販売仕入.Nodes.Add(cNode);
            }

            // 部門ごとにアイテムを追加
            foreach (var cc in masterData.Keys)
            {
                List<dynamic> items = filteredItems != null
                    ? filteredItems.Where(x => deptCodeToCompany.ContainsKey(x.DeptCode) && deptCodeToCompany[x.DeptCode] == cc).ToList()
                    : masterData[cc];

                foreach (var item in items)
                {
                    string deptCode = item.DeptCode;
                    //if (!deptCodeToCompany.TryGetValue(deptCode, out string company)) continue;

                    var compNode = companyNodes[cc];

                    // 部門ノード取得または作成
                    TreeNode deptNode = compNode.Nodes.Cast<TreeNode>()
                        .FirstOrDefault(n => (string)n.Tag == deptCode);
                    if (deptNode == null)
                    {
                        string deptName = deptCodeToName.ContainsKey(deptCode) ? deptCodeToName[deptCode] : deptCode;
                        deptNode = new TreeNode($"{deptCode} {deptName}") { Tag = deptCode, Checked = false };
                        compNode.Nodes.Add(deptNode);
                    }

                    // 取引先ノード追加
                    TreeNode itemNode = new TreeNode($"{item.Code} {item.Name}")
                    {
                        Tag = item.Code,
                        Checked = false
                    };
                    deptNode.Nodes.Add(itemNode);
                }
            }
            treeView販売仕入.ExpandAll();

            if (treeView販売仕入.Nodes.Count > 0)
            {
                treeView販売仕入.SelectedNode = treeView販売仕入.Nodes[0];
                treeView販売仕入.Nodes[0].EnsureVisible();
            }
        }

        private void RestoreTreeViewChecked(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Nodes.Count == 0 && node.Parent != null) // 子ノードのみ
                {
                    string deptCode = node.Parent.Tag.ToString();
                    string company = node.Parent.Parent.Tag.ToString();
                    string code = node.Tag.ToString();

                    bool shouldCheck = false;

                    // preselectedMap を参照して、部門ごとにチェック
                    if (preselectedMap.TryGetValue(code, out var deptSet))
                    {
                        shouldCheck = deptSet.Contains(deptCode);
                    }
                    node.Checked = shouldCheck;
                    // 子ノードのチェック状態に応じて親ノードの見た目チェック更新
                    if (shouldCheck)
                        UpdateParentRecursive(node.Parent, node);
                }
                // 子ノードも再帰
                if (node.Nodes.Count > 0)
                    RestoreTreeViewChecked(node.Nodes);
            }
        }

        // クリックでチェック切替
        private void TreeView販売仕入_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Parent != null && e.Node.Nodes.Count == 0)
                // 子ノード(取引先)のみチェック可能
                e.Node.Checked = !e.Node.Checked;
        }

        private void TreeView販売仕入_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView販売仕入.AfterCheck -= TreeView販売仕入_AfterCheck; // 無限ループ防止


            //// 親ノード(会社・部門)は操作禁止 → チェックを強制的に元に戻す
            if (e.Node.Nodes.Count > 0)
            {
                // 子が1つでもチェックされていればChecked,それ以外はUnchecked   
                e.Node.Checked = e.Node.Nodes.Cast<TreeNode>().Any(n => n.Checked);
                //treeView販売仕入.AfterCheck += TreeView販売仕入_AfterCheck;
                //return;
            }

            // 子ノードのチェック状態に応じて親ノードの状態を更新
            if (e.Node.Nodes.Count == 0 && e.Node.Parent != null)
                UpdateParentRecursive(e.Node.Parent, e.Node);

            treeView販売仕入.AfterCheck += TreeView販売仕入_AfterCheck;
        }

        private void UpdateParentRecursive(TreeNode parent, TreeNode node)
        {
            if (parent != null)
            {
                if (node.Checked)
                {
                    // 子に1つでもチェックがあれば親をチェック
                    parent.Checked = true;
                    // 上位の親も再帰的に更新
                    UpdateParentRecursive(parent.Parent, parent);
                }
                else
                {
                    // 子にチェックが1つもなければ親のチェックを外す
                    bool anyChecked = parent.Nodes.Cast<TreeNode>().Any(n => n.Checked);
                    parent.Checked = anyChecked;

                    // 上位の親も再帰的に更新
                    UpdateParentRecursive(parent.Parent, parent);

                }
                // 子ノードのどれか一つでもChecked → 親もChecked
                parent.Checked = parent.Nodes.Cast<TreeNode>().Any(n => n.Checked);

                // 再帰で上の親も更新
                UpdateParentRecursive(parent.Parent, parent);
            }
        }

        private void btn検索_Click(object sender, EventArgs e)
        {
            // 現在のチェック状態を保持
            var selectedCodes = GetSelectedItems().Select(s => s.Code).ToList();

            string codeSearch = txtBxコード.Text.Trim();
            string nameSearch = txtBx名称.Text.Trim();

            // 何も検索条件がなければ全件表示
            if (string.IsNullOrEmpty(codeSearch) && string.IsNullOrEmpty(nameSearch))
            {
                InitTreeView();
                RestoreTreeViewChecked(treeView販売仕入.Nodes);
                return;
            }

            var filtered = masterData.Values.SelectMany(list => list)
                .Where(item =>
                    (string.IsNullOrEmpty(codeSearch) || item.Code == codeSearch) &&
                    (string.IsNullOrEmpty(nameSearch) || item.Name.IndexOf(nameSearch, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();
            // TreeView再構築
            InitTreeView(filtered);

            // チェック状態復元
            initialSelected = GetSelectedItems().Where(t => selectedCodes.Contains(t.Code)).ToList();
            RestoreTreeViewChecked(treeView販売仕入.Nodes);
        }

        private void btn追加_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView販売仕入.Nodes)
                AddCheckedNodesToListBox(node);
        }

        private void AddCheckedNodesToListBox(TreeNode node)
        {
            if (node.Checked && node.Parent != null && node.Nodes.Count == 0) // 子ノードのみ対象
            {
                var deptNode = node.Parent;
                var compNode = deptNode.Parent;

                string deptCode = deptNode.Tag.ToString();
                string deptName = deptNode.Text.Contains(' ') ? deptNode.Text.Split(' ')[1] : "";
                string company = compNode.Tag.ToString();
                string code = node.Tag.ToString();
                string name = node.Text.Contains(' ') ? node.Text.Split(' ')[1] : node.Text;

                // ListBox に入れる Torihiki オブジェクトを作成
                var torihiki = new Torihiki
                {
                    Code = code,
                    Name = name,
                    Company = company,
                    DeptCode = deptCode,
                    DeptName = deptName
                };
                // --- 重複チェック（同じ Code & Company のものは1行だけ表示） ---
                bool exists = listBx販売仕入.Items.Cast<Torihiki>()
                    .Any(t => t.Code == torihiki.Code && t.Company == torihiki.Company);

                if (!exists)
                    listBx販売仕入.Items.Add(torihiki);

            }
            foreach (TreeNode child in node.Nodes)
                AddCheckedNodesToListBox(child);
        }

        private void btn削除_Click(object sender, EventArgs e)
        {
            var selected = listBx販売仕入.SelectedItems.Cast<Torihiki>().ToList();
            foreach (var s in selected)
            {
                listBx販売仕入.Items.Remove(s);

                // TreeView 上のチェックも外す
                foreach (TreeNode compNode in treeView販売仕入.Nodes)
                {
                    if (compNode.Tag.ToString() != s.Company) continue;

                    foreach (TreeNode deptNode in compNode.Nodes)
                    {
                        if (deptNode.Tag.ToString() != s.DeptCode) continue;

                        foreach (TreeNode itemNode in deptNode.Nodes)
                        {
                            if (itemNode.Tag.ToString() == s.Code)
                            {
                                itemNode.Checked = false;

                                // preselectedMap の更新（削除）
                                if (preselectedMap.TryGetValue(s.Code, out var set))
                                {
                                    set.Remove(s.DeptCode);
                                    if (set.Count == 0)
                                        preselectedMap.Remove(s.Code); // Dept がなくなったら Key 自体も削除
                                }
                            }

                        }
                    }
                }
            }
        }

        internal List<Torihiki> GetSelectedItems()
        {
            var result = new List<Torihiki>();

            foreach (TreeNode compNode in treeView販売仕入.Nodes)
            {
                string company = compNode.Tag.ToString();
                foreach (TreeNode deptNode in compNode.Nodes)
                {
                    string deptCode = deptNode.Tag.ToString();
                    string deptName = deptNode.Text.Contains(' ') ? deptNode.Text.Split(' ')[1] : "";
                    foreach (TreeNode itemNode in deptNode.Nodes)
                    {
                        if (itemNode.Checked)
                        {
                            string code = itemNode.Tag.ToString();
                            string name = itemNode.Text.Contains(' ') ? itemNode.Text.Split(' ')[1] : itemNode.Text;
                            result.Add(new Torihiki
                            {
                                Code = code,
                                Name = name,
                                Company = company,
                                DeptCode = deptCode,
                                DeptName = deptName
                            });
                        }
                    }
                }
            }
            return result;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
    // MF取引先共通クラス(ラッパークラス) 
    public class MfItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kana { get; set; }
        public string DeptCode { get; set; }  // 部門コード
    }

    public class Torihiki
    {
        public string Code { get; set; }  // 取引先コード
        public string Name { get; set; }  // 取引先名称
        public string Company { get; set; }  // 紐づく会社
        public string DeptCode { get; set; }  // 紐づく部門コード
        public string DeptName { get; set; }  // 紐づく部門名称

        public override string ToString() => $"{Code} {Name}";
    }
}
