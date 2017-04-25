using System;
using System.Collections.Generic;
using System.Linq;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.TrtpPaymentDetailsExtracterHelpers
{
    public static class TrtpKeyValuePairExtracter
    {
        private static readonly char[] SlashDelimiter = new char[] { '/' };

        public static Dictionary<string, string> Execute(string trtpPaymentDetails)
        {
            var result = new Dictionary<string, string>();

            _addRemainingPairsToResult(trtpPaymentDetails.Substring(1), result);

            return result;
        }

        private static void _addRemainingPairsToResult(string remainingDetails, Dictionary<string, string> result)
        {
            var parts = remainingDetails.Split(SlashDelimiter);

            var i = 0;
            while (i<parts.Length)
            {
                if ((parts[i] == "NAME" || parts[i] == "IBAN") && (i + 1 < parts.Length))
                result.AddOrMerge(parts[i], parts[i+1]);
                i = i + 1;
            }
        }
    }
}
