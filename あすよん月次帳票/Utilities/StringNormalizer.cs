using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace あすよん月次帳票.Utilities
{
    /// <summary>
    /// 文字列正規化ユーティリティ
    /// </summary>
    public static class StringNormalizer
    {
        /// <summary>
        /// 環境依存文字を置換する辞書
        /// </summary>
        private static readonly Dictionary<string, string> EnviromentCharMap = new Dictionary<string, string>
        {
            { "㈱", "(株)" },
            { "㈲", "(有)" },
            { "㈹", "(代)" },
            { "㈶", "(財)" },
            { "㈳", "(社)" },
            { "㈼", "(学)" },
            { "㈾", "(協)" },
            { "㈿", "(祭)" },
            { "㊀", "(企)" },
            { "㊁", "(資)" },
            { "㊂", "(団)" },
            { "㊃", "(労)" },
        };

        /// <summary>
        /// 文字列を正規化(環境依存文字の置換,タブとスペースの除去,全て大文字の英語(取引先の名称は除く)をタイトルケースに変換)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replaceEnvChars"></param>
        /// <param name="convertToTitleCase"></param>
        /// <returns>正規化された文字列</returns>
        public static string Normalize(string input, bool replaceEnvChars = false, bool convertToTitleCase = false)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string result = input;

            // 環境依存文字の置換(取引先名関連のみ)
            if (replaceEnvChars)
            {
                result = ReplaceEnvironmentChars(result);
            }

            // タブ文字を削除
            result = result.Replace("\t", "");

            // 全て大文字の英語(取引先の名称は除く)をタイトルケースに変換（住所のみ、スペース削除前に実行）
            if (convertToTitleCase)
            {
                result = ConvertToTitleCase(result);
            }

            // 全てのスペース(半角・全角)を削除
            result = Regex.Replace(result, @"[ 　]+", "");

            return result;
        }

        /// <summary>
        /// 取引先名を正規化(環境依存文字 + タブ・スペース削除)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NormalizeTorihikiName(string input)
        {
            return Normalize(input, replaceEnvChars: true, convertToTitleCase: false);
        }

        /// <summary>
        /// 住所を正規化(タイトルケース変換 + タブ・スペース削除)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NormalizeAddress(string input)
        {
            return Normalize(input, replaceEnvChars: false, convertToTitleCase: true);
        }

        /// <summary>
        /// 環境依存文字を置換
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ReplaceEnvironmentChars(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string result = input;
            foreach (var kvp in EnviromentCharMap)
            {
                result = result.Replace(kvp.Key, kvp.Value);
            }
            return result;
        }

        private static string ConvertToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var words = input.Split(new[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();
            var isFirstWord = true;

            foreach (var word in words)
            {
                // 英語のアルファベットのみで構成され、全て大文字の場合のみ変換
                if (Regex.IsMatch(word, @"^[A-Z]+$"))
                {
                    var titleCaseWord = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
                    if (!isFirstWord)
                    {
                        result.Append(" ");
                    }
                    result.Append(titleCaseWord);
                }
                else
                {
                    // そのまま追加
                    if (!isFirstWord)
                    {
                        result.Append(" ");
                    }
                    result.Append(word);
                }
                isFirstWord = false;
            }
            return result.ToString();
        }
    }
}