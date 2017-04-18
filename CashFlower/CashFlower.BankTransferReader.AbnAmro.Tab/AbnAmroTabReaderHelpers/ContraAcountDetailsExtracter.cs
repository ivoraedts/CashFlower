using System;
using System.Globalization;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public static class ContraAcountDetailsExtracter
    {
        public static ContraAccountDetails Execute(string contraAccountDetails)
        {
            if (contraAccountDetails.StartsWith("BEA   NR:"))
                return _getDetailsFromPointOfSaleTerminalDetails(contraAccountDetails);
            throw new NotImplementedException();
        }

        private static ContraAccountDetails _getDetailsFromPointOfSaleTerminalDetails(string contraAccountDetails)
        {
            if (contraAccountDetails.Length < 33)
                throw new CashFlowerException("CFE_ABN_008",
                    "Fewer characters ({0}) than excpected (33) in Point of Sale Terminal Details: '{1}'"
                    , contraAccountDetails.Length, contraAccountDetails);

            return new ContraAccountDetails
            {
                DateTimeStamp = _parsePointOfSaleTerminalDetailsTimeStamp(contraAccountDetails.Substring(18, 14)),
                ContraAccountName = _stripCardnumberIfSpecified(contraAccountDetails.Substring(33))
            };
        }

        private static string _stripCardnumberIfSpecified(string contraAccountName)
        {
            return contraAccountName.IndexOf(",PAS", StringComparison.InvariantCulture) == -1 ?
            contraAccountName :
            contraAccountName.Substring(0, contraAccountName.IndexOf(",PAS", StringComparison.InvariantCulture));
        }

        private static DateTime _parsePointOfSaleTerminalDetailsTimeStamp(string contraAccountDetailsTimeStamp)
        {
            DateTime myDate;
            if (_tryParsePointOfSaleTerminalDetailsTimeStamp(contraAccountDetailsTimeStamp, out myDate))
                return myDate;
            throw new CashFlowerException("CFE_ABN_009", "No Valid contraAccountDetailsTimeStamp ({0}) given.", contraAccountDetailsTimeStamp);
        }

        private static bool _tryParsePointOfSaleTerminalDetailsTimeStamp(string contraAccountDetailsTimeStamp, out DateTime myDate)
        {
            return DateTime.TryParseExact(contraAccountDetailsTimeStamp, @"dd.MM.yy/HH.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out myDate);
        }
    }
}
