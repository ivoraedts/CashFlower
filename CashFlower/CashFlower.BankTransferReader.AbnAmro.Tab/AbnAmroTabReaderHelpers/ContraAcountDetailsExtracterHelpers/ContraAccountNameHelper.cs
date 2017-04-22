using System;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    public static class ContraAccountNameHelper
    {
        public static string StripCardnumber(string contraAccountName)
        {
            return contraAccountName.IndexOf(",PAS", StringComparison.InvariantCulture) == -1
                ? contraAccountName
                : contraAccountName.Substring(0, contraAccountName.IndexOf(",PAS", StringComparison.InvariantCulture));
        }
    }
}
