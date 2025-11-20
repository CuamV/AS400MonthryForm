using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace あすよん月次帳票
{


    public partial class 販売仕入先Form : Form
    {
        private string mode;  // "HANBAI" or "SHIIRE"
        private List<Torihiki> initialSelected;  // RplForm2 から受け取る
        private Dictionary<string, string> deptCodeToCompany; // 部門コードから会社名へのマッピング
        private Dictionary<string, List<dynamic>> jsonData;  // JSON読込結果
        private Dictionary<string, HashSet<string>> preselectedMap; // RplForm2 から受け取る事前選択データマップ

        public 販売仕入先Form(string mode, List<Torihiki> selectedItems, Dictionary<string, HashSet<string>> existingMap)
        {
            InitializeComponent();
            this.Load += 販売仕入先Form_Load;

            this.mode = mode;  // モード保持
            this.initialSelected = selectedItems ?? new List<Torihiki>();
            this.preselectedMap = existingMap ?? new Dictionary<string, HashSet<string>>();

            treeView販売仕入.NodeMouseClick += TreeView販売仕入_NodeMouseClick;
        }

        public void 販売仕入先Form_Load(object sender, EventArgs e)
        {
            // 部門CD→会社マッピング作成
            deptCodeToCompany = new Dictionary<string, string>();
            foreach (var comp in new[] { "オーノ", "サンミックダスコン", "サンミックカーペット" })
            {
                foreach (var bumon in JsonLoader.GetBUMONs(comp))
                    deptCodeToCompany[bumon.Code] = comp;
            }
            LoadAllJson();
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

        // 全会社のJSONデータを読み込み
        private void LoadAllJson()
        {
            jsonData = new Dictionary<string, List<dynamic>>();
            string[] companyCodes = { "オーノ", "サンミックダスコン", "サンミックカーペット" };

            foreach (var cc in companyCodes)
            {
                var items = new List<dynamic>();
                var validBumons = JsonLoader.GetBUMONs(cc).Select(b => b.Code).ToHashSet(); // BUMON.json に存在する部門

                IEnumerable<string> keys = mode == "HANBAI"
                    ? JsonLoader.GetHanbaiKeys(cc)
                    : JsonLoader.GetShiireKeys(cc);

                foreach (var bumonCode in keys)
                {
                    if (!validBumons.Contains(bumonCode)) continue;

                    List<MfItem> itemsToAdd = new List<MfItem>();

                    if (mode == "HANBAI")
                    {
                        var list = JsonLoader.GetMf_HANBAIs(cc, new[] { bumonCode });
                        itemsToAdd.AddRange(list.Select(x => new MfItem
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Kana = x.Kana,
                            DeptCode = bumonCode
                        }));
                    }
                    else
                    {
                        var list = JsonLoader.GetMf_SHIIREs(cc, new[] { bumonCode });
                        itemsToAdd.AddRange(list.Select(x => new MfItem
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Kana = x.Kana,
                            DeptCode = bumonCode
                        }));
                    }
                    items.AddRange(itemsToAdd);
                }
                jsonData[cc] = items;
            }
        }

        public void InitTreeView(IEnumerable<dynamic> filteredItems = null)
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
            foreach (var cc in jsonData.Keys)
            {
                List<dynamic> items = filteredItems != null
                    ? filteredItems.Where(x => deptCodeToCompany.ContainsKey(x.DeptCode) && deptCodeToCompany[x.DeptCode] == cc).ToList()
                    : jsonData[cc];

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
                        string deptName = JsonLoader.GetBUMONs(cc)
                            .FirstOrDefault(b => b.Code == deptCode)?.Name ?? deptCode;
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
        public void TreeView販売仕入_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
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

        public void btn検索_Click(object sender, EventArgs e)
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

            var filtered = jsonData.Values.SelectMany(list => list)
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

        public void btn追加_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView販売仕入.Nodes)
                AddCheckedNodesToListBox(node);
        }

        public void AddCheckedNodesToListBox(TreeNode node)
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

        public void btn削除_Click(object sender, EventArgs e)
        {
            var selected = listBx販売仕入.SelectedItems.Cast<Torihiki>().ToList();
            foreach (var s in selected)
                listBx販売仕入.Items.Remove(s);
        }

        public List<Torihiki> GetSelectedItems()
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
