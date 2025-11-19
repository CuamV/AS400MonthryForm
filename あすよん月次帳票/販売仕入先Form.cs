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
        private List<string> initialSelected;  // RplForm2 から受け取る
        private Dictionary<string, string> deptCodeToCompany; // 部門コードから会社名へのマッピング
        private Dictionary<string, List<dynamic>> jsonData;  // JSON読込結果

        public 販売仕入先Form(string mode, List<string> selectedItems)
        {
            InitializeComponent();
            this.Load += 販売仕入先Form_Load;

            this.mode = mode;  // モード保持
            this.initialSelected = selectedItems ?? new List<string>();

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

                if (mode == "HANBAI")
                {
                    foreach (var bumonCode in JsonLoader.GetHanbaiKeys(cc))
                    {
                        var list = JsonLoader.GetMf_HANBAIs(cc, new[] { bumonCode });
                        items.AddRange(list.Select(x => new MfItem
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Kana = x.Kana,
                            DeptCode = bumonCode  // 部門コードを追加
                        }));
                    }
                }
                else
                {
                    foreach(var bumonCode in JsonLoader.GetShiireKeys(cc))
                    {
                        var list = JsonLoader.GetMf_SHIIREs(cc, new[] { bumonCode });
                        foreach (var item in list)
                        items.AddRange(list.Select(x => new MfItem
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Kana = x.Kana,
                            DeptCode = bumonCode  // 部門コードを追加
                        }));
                    }
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
                List<dynamic> items;

                if (filteredItems != null)
                    // フィルタリングされたアイテムのみ使用
                    items = filteredItems
                        .Where(x => deptCodeToCompany.ContainsKey(x.DeptCode) &&
                                       deptCodeToCompany[x.DeptCode] == cc)
                        .ToList();
                else
                    items = jsonData[cc];

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
                if (initialSelected.Any(s => s.StartsWith(node.Tag.ToString())))
                {
                    node.Checked = true;
                    // 子ノードのチェック状態に応じて親ノードの見た目チェック更新
                    if (node.Parent != null)
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
                    UpdateParentRecursive(parent.Parent,parent);
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
            var selectedCodes = GetSelectedItems().Select(s => s.Split(' ')[0]).ToList();

            string codeSearch = txtBxコード.Text.Trim();
            string nameSearch = txtBx名称.Text.Trim();

            // 何も検索条件がなければ全件表示
            if (string.IsNullOrEmpty(codeSearch) && string.IsNullOrEmpty(nameSearch))
            {
                InitTreeView();
                RestoreTreeViewChecked(treeView販売仕入.Nodes);
                return;
            }

            var filtered = new List<dynamic>();
            foreach (var list in jsonData.Values)
            {
                foreach (var item in list)
                {
                    bool match = true;
                    if(!string.IsNullOrEmpty(codeSearch))
                        match &= item.Code == codeSearch;
                    if (!string.IsNullOrEmpty(nameSearch))
                        match &= item.Name.IndexOf(nameSearch, StringComparison.OrdinalIgnoreCase) >= 0;

                    if (match)filtered.Add(item);
                }
            }
            // TreeView再構築
            InitTreeView(filtered);

            // チェック状態復元
            initialSelected = selectedCodes;
            RestoreTreeViewChecked(treeView販売仕入.Nodes);
        }

        public void btn追加_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView販売仕入.Nodes)
                AddCheckedNodesToListBox(node);
        }

        public void AddCheckedNodesToListBox(TreeNode node)
        {
            if(node.Checked && node.Parent != null && node.Nodes.Count == 0) // 子ノードのみ対象
            {
                string text = node.Text;
                if(!listBx販売仕入.Items.Contains(text))
                    listBx販売仕入.Items.Add(text);
            }
            foreach (TreeNode child in node.Nodes)
                AddCheckedNodesToListBox(child);
        }

        public void btn削除_Click(object sender, EventArgs e)
        {
            var selected = listBx販売仕入.SelectedItems.Cast<string>().ToList();
            foreach (var s in selected) 
                listBx販売仕入.Items.Remove(s);
        }

        public List<string> GetSelectedItems()
        {
            return listBx販売仕入.Items.Cast<string>().ToList();
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
}
