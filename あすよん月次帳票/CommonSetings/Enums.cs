namespace あすよん月次帳票
{
    //-----　<各種マスタレイアウト>　-----
    internal enum BUMON_MASTTER // 部門マスタ
    {
        部門CD,
        部門名,
        部門名カナ,
        会社
    }

    internal enum TORIHIKI_MASTER // 取引先マスタ
    {
        取引先CD,      // 0
        取引先正式名, // 1
        取引先名,      // 2
        取引先名カナ,   // 3 
        取引先略名,    // 4
        取引先略名カナ, // 5
        郵便番号,      // 6
        電話番号1,     // 7
        電話番号2,     // 8
        FAX番号1,     // 9
        FAX番号2,     // 10
        住所1,        // 11
        住所1カナ,     // 12
        住所2,        // 13
        住所2カナ,     // 14
        商社区分,      // 15
        仕入先区分,    // 16
        販売先区分,    // 17
        得意先区分,    // 18
        出荷先区分,    // 19
        預り先区分,    // 20
        運送便区分,    // 21
        倉庫区分,      // 22
        備考,         // 23
        登録者,       // 24
        登録日付,     // 25
        登録時刻      // 26
    }

    internal enum TORIHIKI_MASTER_INOUT // 取引先マスタ(ダウンロード)
    {
        取引先CD,
        部門CD,
        取引先正式名,
        取引先名,
        取引先名カナ,
        取引先略名,
        取引先略名カナ,
        郵便番号,
        電話番号1,
        電話番号2,
        FAX番号1,
        FAX番号2,
        住所1,
        住所1カナ,
        住所2,
        住所2カナ,
        商社区分,
        仕入先区分,
        販売先区分,
        得意先区分,
        出荷先区分,
        預り先区分,
        運送便区分,
        倉庫区分,
        備考,
        登録者,
        登録日付,
        登録時刻
    }

    internal enum TORIHIKIBUMON_MASTER  // 取引先-部門マスタ
    {
        取引先CD,
        部門CD
    }

    internal enum TORIHIKIROLL_MASTER  // 取引先ロール別マスタ
    {
        取引先CD,
        部門CD,
        取引先名,
        取引先名カナ,
    }

    internal enum TORIHIKIROLL_MASTER_TOKUI
    {
        取引先CD,
        部門CD,
        取引先名,
        取引先名カナ,
        適用開始日,
        適用終了日,
    }
    //PostalCodes
    internal enum POSTALCODES // 郵便番号辞書
    {
        郵便番号,
        都道府県名ｶﾅ,
        市区町村名ｶﾅ,
        町域名ｶﾅ,
        都道府県名,
        市区町村名,
        町域名
    }

    internal enum USERMASTER // 社員マスタ
    {
        社員番号,
        ユーザーID,
        ドメインユーザー,
        氏名,
        氏名ｶﾅ,
        部門コード,
        メールアドレス1,
        メールアドレス２,
        社用電話番号,
        内線番号,
        機器管理番号,
        バックアップパス1,
        バックアップパス2,
    }

    //-----　<各種分岐パターン>　-----

    internal enum VaridationPattern // バリデーションパターン
    {
        必須項目登録前初期チェック,
        入力値チェック,
        取引先マスタインポートエラーチェック,
        取引先マスタ関連登録先チェック,
        ユーザーマスタ登録前チェック1,
    }

    internal enum AddMasterPattern  // マスタ追加パターン(キー位置)
    {
        Keyが1項目,
        Keyが1項目と2項目,
        Keyなし,
    }

    internal enum  JudgeMfPattern
    {
        パスのみ,
        パスとファイル名,
    }

    internal enum  AnimationPattern
    {
        開く,
        閉じる,
    }

    internal enum  MstMntPattern
    {
        登録,
        削除,
    }

    internal enum SelectUserCDPattern
    {
        社員番号,
        ユーザーID,
        ドメインユーザー,
    }

}
