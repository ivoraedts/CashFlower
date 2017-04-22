using System;
using System.Globalization;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    public static class ContraAcountDetailsTimeStampExtracter
    {
        public static bool TryExecute(string contraAccountDetailsTimeStamp, out DateTime myDate)
        {
            return DateTime.TryParseExact(contraAccountDetailsTimeStamp, @"dd.MM.yy/HH.mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out myDate);
        }
    }
}
