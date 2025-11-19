using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace あすよん月次帳票
{
    public partial class 部門Form : Form
    {
        public string Code { get; set; }
        public string Name { get; set; }

        private List<string> initialSelected;  // ★ RplForm2 から受け取る
        public 部門Form(List<string> selectedItems)
        {
            InitializeComponent();
            this.Load += 部門Form_Load;

            initialSelected = selectedItems ?? new List<string>();
            // フォーム初期化時にイベント登録
            treeView部門.NodeMouseClick += TreeView部門_NodeMouseClick;
        }

        public void 部門Form_Load(object sender, EventArgs e)
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

        public void InitTreeView()
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
                if (initialSelected.Any(s => s.StartsWith(node.Tag.ToString())))
                {
                    node.Checked = true;
                }

                if (node.Nodes.Count > 0)
                    RestoreTreeViewChecked(node.Nodes);

                if (node.Parent != null)
                    UpdateParentRecursive(node.Parent,node);
            }
        }

        // クリックでチェック切替
        public void TreeView部門_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Parent != null && e.Node.Nodes.Count == 0)
                // 子ノード(取引先)のみチェック可能
                e.Node.Checked = !e.Node.Checked;
        }

        private void TreeView部門_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView部門.AfterCheck -= TreeView部門_AfterCheck; // 無限ループ防止

            // 子ノードのみ親を更新
            if (e.Node.Parent != null)
            {
                UpdateParentRecursive(e.Node.Parent, e.Node);
            }

            treeView部門.AfterCheck += TreeView部門_AfterCheck;
        }

        private void UpdateParentRecursive(TreeNode parent, TreeNode node)
        {
            if (parent != null) return;
            {
                if (node.Checked)
                {
                    // 子に1つでもチェックがあれば親をチェック
                    parent.Checked = true;
                }
                else
                {
                    // 子にチェックが1つもなければ親のチェックを外す
                    bool anyChecked = parent.Nodes.Cast<TreeNode>().Any(n => n.Checked);
                    parent.Checked = anyChecked;
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
                string text = node.Text;
                if (!listBx部門.Items.Contains(text))
                    listBx部門.Items.Add(text);
            }

            foreach (TreeNode child in node.Nodes)
                AddCheckedNodesToListBox(child);
        }

        private void btn削除_Click(object sender, EventArgs e)
        {
            var selectedItems = listBx部門.SelectedItems.Cast<string>().ToList();
            foreach (var item in selectedItems)
                listBx部門.Items.Remove(item);
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
        public List<string> GetSelectedBumons()
        {
            return listBx部門.Items.Cast<string>().ToList();
        }
    }
}
