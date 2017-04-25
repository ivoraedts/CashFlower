using System.Collections.Generic;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public static class KeyValueExtensions
    {
        public static string GetValueOrNull(this Dictionary<string, string> dict, string key)
        {
            string outstr;
            return !dict.TryGetValue(key, out outstr) ? null : outstr;
        }

        public static void AddOrMerge(this Dictionary<string, string> dict, string key, string value)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, value.Trim());
            else
                dict[key] = dict[key] + "-" + value;
        }
    }
}