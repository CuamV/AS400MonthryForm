using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using あすよん月次帳票.Services;

namespace あすよん月次帳票
{
    public partial class 管理者用設定 : Form
    {
        private readonly 取引先取得Service _取引先Service = new 取引先取得Service();

        public 管理者用設定()
        {
            InitializeComponent();
        }

        private void btnAS400取引先マスタ取得_Click(object sender, EventArgs e)
        {
            // 確認メッセージ
            var confirmResult = MessageBox.Show(
                "AS400から取引先マスタと取引履歴を取得し、SQL Serverに保存します。\n\n" +
                "※処理には時間がかかる場合があります。\n\n" +
                "よろしいですか？",
                "AS400取引先マスタ取得確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No) return;

            try
            {
                // AS400取引先マスタをSQL Serverに保存
                var (success, message) = _取引先Service.SaveAS400取引先マスタToSqlServer();

                if (success)
                {
                    MessageBox.Show(
                        message,
                        "完了",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        message,
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"予期しないエラーが発生しました。\n\n{ex.Message}",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAS400取引先マスタエクスポート_Click(object sender, EventArgs e)
        {
            取引先出力Service 取引先履歴取得 = new 取引先出力Service();
            取引先履歴取得.Create取引先部門展開Table(ToriBumonbtnPattern.出力);
        }

        private void btnAS400取引先マスタ展開_Click(object sender, EventArgs e)
        {
            取引先出力Service 取引先履歴取得 = new 取引先出力Service();
            取引先履歴取得.Create取引先部門展開Table(ToriBumonbtnPattern.反映);
        }
    }
}
