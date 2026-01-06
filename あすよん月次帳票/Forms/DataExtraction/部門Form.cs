using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    internal partial class 部門Form : Form
    {
        public string Code { get; set; }
        public string Name { get; set; }

        private List<Department> initialSelected;  // ★ RplForm2 から受け取る

        internal 部門Form(List<Department> selectedItems)
        {
            InitializeComponent();
            this.Load += 部門Form_Load;

            initialSelected = selectedItems ?? new List<Department>();
            // フォーム初期化時にイベント登録
            treeView部門.NodeMouseClick += TreeView部門_NodeMouseClick;
        }

        internal void 部門Form_Load(object sender, EventArgs e)
        {
            InitTreeView();

            // ListBoxに復元
            foreach (var item in initialSelected)
                listBx部門.Items.Add(item);

            // ← TreeView 構築完了後、UIが整ってから復元をかける
            this.BeginInvoke(new Action(() =>
            {
                // AfterCheck を止めてから
                treeView部門.AfterCheck -= TreeView部門_AfterCheck;
                // TreeViewのチェック状態を復元
                RestoreTreeViewChecked(treeView部門.Nodes);
                treeView部門.AfterCheck += TreeView部門_AfterCheck;

                // 最初のノードに選択を合わせる
                if (treeView部門.Nodes.Count > 0)
                    treeView部門.SelectedNode = treeView部門.Nodes[0];
            }));
        }

        private void InitTreeView()
        {
            // TreeViewを初期化
            treeView部門.Nodes.Clear();

            // 全会社のルートノードを作成
            string[] allCompanies = new string[] { "オーノ", "サンミックダスコン", "サンミックカーペット" };

            foreach (var comp in allCompanies)
            {
                TreeNode companyNode = new TreeNode(comp)
                {
                    Tag = comp,  // 会社名をTagに設定
                    Checked = false  // 初期状態ではチェックなし
                };

                // 部門ノードを追加
                foreach (var bumon in JsonLoader.GetBUMONs(comp))
                {
                    TreeNode bumonNode = new TreeNode($"{bumon.Code} {bumon.Name}")
                    {
                        Tag = bumon.Code,
                        Checked = false  // 初期状態ではチェックなし
                    };
                    companyNode.Nodes.Add(bumonNode);
                }
                treeView部門.Nodes.Add(companyNode);
            }
            // ツリーを展開（必要なら）
            treeView部門.ExpandAll();
        }

        
        /// <summary>
        /// ★ listBox の内容から TreeView のチェックを復元
        /// </summary>
        private void RestoreTreeViewChecked(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if(node.Parent != null)
                {
                    string company = node.Parent.Tag.ToString();
                    string code = node.Tag.ToString();

                    //if (initialSelected.Any(s => s.StartsWith(node.Tag.ToString())))
                    if (initialSelected.Any(d => d.Code == code && d.Company == company))
                    {
                        node.Checked = true;
                        // 子ノードのチェック状態に応じて親ノードの見た目チェック更新
                        //if (node.Parent != null)
                        UpdateParentRecursive(node.Parent, node);
                    }
                }
                // 子ノードも再帰
                if (node.Nodes.Count > 0)
                    RestoreTreeViewChecked(node.Nodes);

            }
        }

        // クリックでチェック切替
        private void TreeView部門_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Parent != null && e.Node.Nodes.Count == 0)
                // 子ノード(取引先)のみチェック可能
                e.Node.Checked = !e.Node.Checked;
        }

        private void TreeView部門_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView部門.AfterCheck -= TreeView部門_AfterCheck; // 無限ループ防止

            // 親ノード(会社)は操作禁止 → チェックを強制的に元に戻す
            if (e.Node.Nodes.Count > 0)
            {
                bool original = e.Node.Nodes.Cast<TreeNode>().Any(n => n.Checked);
                e.Node.Checked = original;

                treeView部門.AfterCheck += TreeView部門_AfterCheck;
                return;
            }
            // 子ノードのみ親を更新
            if (e.Node.Nodes.Count == 0 && e.Node.Parent != null)
                UpdateParentRecursive(e.Node.Parent, e.Node);

            treeView部門.AfterCheck += TreeView部門_AfterCheck;
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
            }
        }
        private void btn追加_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView部門.Nodes)
            {
                AddCheckedNodesToListBox(node);
            }
        }

        private void AddCheckedNodesToListBox(TreeNode node)
        {
            if (node.Checked && node.Parent != null) // 子ノードのみ対象
            {
                string[] parts = node.Text.Split(' ');
                var dept = new Department
                {
                    Code = parts[0],
                    Name = parts.Length > 1 ? parts[1] : "",
                    Company = node.Parent.Tag.ToString()
                };

                if (!listBx部門.Items.Cast<Department>().Any(d => d.Code == dept.Code && d.Company == dept.Company))
                    listBx部門.Items.Add(dept);
            }

            foreach (TreeNode child in node.Nodes)
                AddCheckedNodesToListBox(child);
        }

        private void btn削除_Click(object sender, EventArgs e)
        {
            var selectedItems = listBx部門.SelectedItems.Cast<Department>().ToList();
            foreach (var item in selectedItems)
            {
                listBx部門.Items.Remove(item);

                // TreeView 上のチェックも外す
                foreach (TreeNode compNode in treeView部門.Nodes)
                {
                    if (compNode.Tag.ToString() != item.Company) continue;
                    { 
                        foreach (TreeNode itemNode in compNode.Nodes)
                        {
                            if (itemNode.Tag.ToString() == item.Code)
                                itemNode.Checked = false;
                        }
                    }
                }
            }
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

        // Form2に返すための選択部門取得
        public List<Department> GetSelectedBumons()
        {
            return listBx部門.Items.Cast<Department>().ToList();
        }
    }

    // 部門オブジェクト
    public class Department
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }

        public override string ToString() => $"{Code} {Name}";
    }
}
