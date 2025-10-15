using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace あすよん月次帳票
{

    internal class mf_BUMON
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    internal class mf_HANBAI
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kana { get; set; }
    }
    internal class mf_SHIIRE
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kana { get; set; }
    }

    internal static class JsonLoader
    {
        // 部門（会社キー => List<mf_BUMON>）は今まで通り
        private static Dictionary<string, List<mf_BUMON>> _BUMONS = new Dictionary<string, List<mf_BUMON>>();
        // 販売先：会社 => (部門コード => List<mf_HANBAI>)
        private static Dictionary<string, Dictionary<string, List<mf_HANBAI>>> _HANBAIS
            = new Dictionary<string, Dictionary<string, List<mf_HANBAI>>>();
        // 仕入先：会社 => (部門コード => List<mf_SHIIRE>)
        private static Dictionary<string, Dictionary<string, List<mf_SHIIRE>>> _SHIIRES
            = new Dictionary<string, Dictionary<string, List<mf_SHIIRE>>>();

        public static void LoadBumon(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("部門マスタがみつかりません", filePath);
            string json = File.ReadAllText(filePath);

            _BUMONS = JsonSerializer.Deserialize<Dictionary<string, List<mf_BUMON>>>(json) ?? new Dictionary<string, List<mf_BUMON>>();
        }

        public static void LoadHanbai(string company, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("販売先マスタがみつかりません", filePath);

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<mf_HANBAI>>>(json)
                   ?? new Dictionary<string, List<mf_HANBAI>>();

            // キー（部門コード）をトリムして正規化して格納
            var normalized = new Dictionary<string, List<mf_HANBAI>>();
            foreach (var kv in data)
            {
                var key = kv.Key?.Trim() ?? "";
                normalized[key] = kv.Value ?? new List<mf_HANBAI>();
            }
            _HANBAIS[company] = normalized;
        }

        public static void LoadShiire(string company, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("仕入先マスタがみつかりません", filePath);

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<mf_SHIIRE>>>(json)
                   ?? new Dictionary<string, List<mf_SHIIRE>>();
            var normalized = new Dictionary<string, List<mf_SHIIRE>>();

            foreach (var kv in data)
            {
                var key = kv.Key?.Trim() ?? "";
                normalized[key] = kv.Value ?? new List<mf_SHIIRE>();
            }

            _SHIIRES[company] = normalized;
        }

        // 部門の取得
        public static IEnumerable<mf_BUMON> GetBUMONs(params string[] companies)
        {
            foreach (var company in companies)
            {
                if (_BUMONS.ContainsKey(company))
                {
                    foreach (var bumon in _BUMONS[company])
                        yield return bumon;
                }
            }
        }

        // 会社→部門にぶら下がる販売先取得
        public static IEnumerable<mf_HANBAI> GetMf_HANBAIs(string company, IEnumerable<string> bumonCodes)
        {
            if (!_HANBAIS.ContainsKey(company)) yield break;

            var dict = _HANBAIS[company];

            foreach (var bumon in bumonCodes)
            {
                var key = (bumon ?? "").Trim();
                if (string.IsNullOrEmpty(key)) continue;

                if (dict.ContainsKey(key))
                {
                    foreach (var item in dict[key]) yield return item;
                }
                else
                {
                    // デバッグ用にここで何も見つからないことをログする（必要なら MessageBox）
                    System.Diagnostics.Debug.WriteLine($"GetMf_HANBAIs: company={company} に key={key} は存在しません。利用可能キー: {string.Join(",", dict.Keys)}");
                }
            }
        }

        // 会社→部門にぶら下がる仕入先取得
        public static IEnumerable<mf_SHIIRE> GetMf_SHIIREs(string company, IEnumerable<string> bumonCodes)
        {
            if (!_SHIIRES.ContainsKey(company)) yield break;

            var dict = _SHIIRES[company];

            foreach (var bumon in bumonCodes)
            {
                var key = (bumon ?? "").Trim();
                if (string.IsNullOrEmpty(key)) continue;

                if (dict.ContainsKey(key))
                {
                    foreach (var item in dict[key]) yield return item;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"GetMf_SHIIREs: company={company} に key={key} は存在しません。利用可能キー: {string.Join(",", dict.Keys)}");
                }
            }
        }

        // デバッグ用：会社のキー一覧を取得するヘルパー
        public static IEnumerable<string> GetHanbaiKeys(string company)
        {
            if (!_HANBAIS.ContainsKey(company)) yield break;
            foreach (var k in _HANBAIS[company].Keys) yield return k;
        }
        public static IEnumerable<string> GetShiireKeys(string company)
        {
            if (!_SHIIRES.ContainsKey(company)) yield break;
            foreach (var k in _SHIIRES[company].Keys) yield return k;
        }

    }
    
}
