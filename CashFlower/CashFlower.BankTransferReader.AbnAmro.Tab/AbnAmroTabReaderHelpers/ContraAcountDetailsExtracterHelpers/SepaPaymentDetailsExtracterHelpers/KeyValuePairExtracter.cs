using System;
using System.Collections.Generic;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.
    SepaPaymentDetailsExtracterHelpers
{
    public static class KeyValuePairExtracter
    {
        public static Dictionary<string, string> Execute(string sepaPaymentDetails)
        {
            var result = new Dictionary<string, string>();

            _addRemainingValuesToResult(sepaPaymentDetails, result, "SEPA Header");

            return result;
        }

        private static void _addRemainingValuesToResult(
            string sepaPaymentDetails, Dictionary<string, string> result, string key)
        {
            var nextSemiColon = sepaPaymentDetails.IndexOf(":", StringComparison.Ordinal);
            if (nextSemiColon == -1)
            {
                result.AddOrMerge(key, sepaPaymentDetails.Trim());
                return;
            }

            var firstpart = sepaPaymentDetails.Substring(0, nextSemiColon);
            var lastSpaceInFirstPart = firstpart.LastIndexOf(" ", StringComparison.Ordinal);

            if (lastSpaceInFirstPart == -1)
            {
                throw new CashFlowerException(
                    "CFE_ABN_013",
                    "Unexpected SEPA Payment input. Failed to extract key before semicolon at this part : '{0}'",
                    sepaPaymentDetails);
            }

            result.AddOrMerge(key, firstpart.Substring(0, lastSpaceInFirstPart).Trim());

            var secondpart = sepaPaymentDetails.Substring(nextSemiColon + 1);
            var nextKey = firstpart.Substring(lastSpaceInFirstPart + 1);

            // ReSharper disable once TailRecursiveCall
            _addRemainingValuesToResult(secondpart, result, nextKey);
        }
    }
}