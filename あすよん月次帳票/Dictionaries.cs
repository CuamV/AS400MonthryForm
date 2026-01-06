using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace あすよん月次帳票
{
    internal static class Dictionaries
    {
        internal static Dictionary<string, Dictionary<string, string>> fileMap = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["オーノ"] = new Dictionary<string, string>
                    {
                        { "原材料", "OIZAIKOG" },
                        { "製品", "OIZAIKOS" },
                        { "加工在庫", "OIZAIKOK" },
                        { "預り在庫", "OIZAIKOAR" },
                        { "預け在庫", "OIZAIKOAK" }
                    },
            ["サンミックダスコン"] = new Dictionary<string, string>
                    {
                        { "原材料", "SDIZAIKOG" },
                        { "コーティング半製品", "SDIZAIKOCH" },
                        { "製品", "SDIZAIKOS" },
                        { "加工在庫", "SDIZAIKOK" },
                    },
            ["サンミックカーペット"] = new Dictionary<string, string>
                    {
                        { "原材料", "SCIZAIKOG" },
                        { "タフト半製品", "SCIZAIKOTH" },
                        { "コーティング半製品", "SCIZAIKOCH" },
                        { "製品", "SCIZAIKOS" },
                        { "加工在庫", "SCIZAIKOK" },
                        { "預り在庫", "SCIZAIKOAR" },
                        { "預け在庫", "SCIZAIKOAK" }
                    }
        };

        internal static Dictionary<string, string> classMap = new Dictionary<string, string>
                        {
                            { "1", "原材料" },
                            { "2", "タフト半製品" },
                            { "3", "コーティング半製品" },
                            { "4", "製品" },
                        };
    }
}
