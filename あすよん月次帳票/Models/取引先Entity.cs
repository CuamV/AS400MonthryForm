namespace あすよん月次帳票.Models
{
    /// <summary>
    /// 取引先マスタエンティティ
    /// </summary>
    public class  取引先Master
    {
        public string 取引先コード { get; set; }
        public string 取引先正式名 { get; set; }
        public string 取引先名 { get; set; }
        public string 略名 { get; set; }
        public string カナ名 { get; set; }
        public string 郵便番号 { get; set; }
        public string 電話番号 { get; set; }
        public string FAX番号 { get; set; }
        public string 住所1 { get; set; }
        public string 住所2 { get; set; }
        public string 代表者 { get; set; }
        public decimal 資本金 { get; set; }
        public string 取引先部門 { get; set; }
        public string 取引先担当者 { get; set; }
        public string 取引開始日 { get; set; }
        public string 販売先区分 { get; set; }
        public string 仕入先区分 { get; set; }
        public string 出荷先区分 { get; set; }
        public string 運送便区分 { get; set; }
        public string 倉庫区分 { get; set; }
        public string 指示書発行倉庫区分 { get; set; }
        public string 消費税有無区分 { get; set; }
        public string 消費税計算区分 { get; set; }
        public string 数量計算区分 { get; set; }
        public string 金額計算区分 { get; set; }
        public string 納品書出力区分 { get; set; }
        public int 納品書出力行数 { get; set; }
        public string 相手先取引先コード { get; set; }
        public string 買掛用締日 { get; set; }
        public string 買掛用支払日 { get; set; }
        public string 買掛用手形サイト { get; set; }
        public string 銀行コード { get; set; }
        public string 銀行支店コード { get; set; }
        public string 口座番号 { get; set; }
        public string 口座区分 { get; set; }
        public string 口座名義 { get; set; }
        public string 担当者コード1 { get; set; }
        public string 担当者コード2 { get; set; }
        public string 担当者コード3 { get; set; }
        public string 運送便コード1 { get; set; }
        public string 運送便コード2 { get; set; }
        public string 運送便コード3 { get; set; }
        public decimal 与信限度額1 { get; set; }
        public decimal 与信限度額2 { get; set; }
        public decimal 与信限度額3 { get; set; }
        public string 納品書発行区分 { get; set; }
        public string 請求書発行区分 { get; set; }
        public string 出荷案内書発行区分 { get; set; }
        public string 送り状発行区分 { get; set; }
        public int 反番表示桁数 { get; set; }
        public string 社内加工区分 { get; set; }
        public string 削除MK { get; set; }
        public string 作成日 { get; set; }
        public string 作成時刻 { get; set; }
        public string 更新日 { get; set; }
        public string 更新時刻 { get; set; }
        public string 更新PGM { get; set; }
    }

    /// <summary>
    /// 取引先履歴エンティティ
    /// </summary>
    public class 取引部門履歴
    {
        public string 取引先コード {  get; set; }
        public string 部門コード { get; set; }
    }


    public class  取引先部門展開
    {
        public string 取引先CD { get; set; }
        public string 部門CD { get; set; }
        public string 取引先正式名 { get; set; }
        public string 取引先名 { get; set; }
        public string 取引先名カナ { get; set; }
        public string 取引先略名 { get; set; }
        public string 取引先略名カナ { get; set; }
        public string 郵便番号 { get; set; }
        public string 電話番号1 { get; set; }
        public string 電話番号2 { get; set; }
        public string FAX番号1 { get; set; }
        public string FAX番号2 { get; set; }
        public string 住所1 { get; set; }
        public string 住所1カナ { get; set; }
        public string 住所2 { get; set; }
        public string 住所2カナ { get; set; }
        public string 商社区分 { get; set; }
        public string 仕入先区分 { get; set; }
        public string 販売先区分 { get; set; }
        public string 得意先区分 { get; set; }
        public string 出荷先区分 { get; set; }
        public string 預り先区分 { get; set; }
        public string 運送便区分 { get; set; }
        public string 倉庫区分 { get; set; }
        public string 備考 { get; set; }
        public string 登録者 { get; set; }
        public string 登録日付 { get; set; }
        public string 登録時刻 { get; set; }
    }
}
