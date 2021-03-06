﻿using System;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    public static class PointOfSaleTerminalDetailsExtracter
    {
        public static ContraAccountDetails Execute(string contraAccountDetails)
        {
            if (contraAccountDetails.Length < 33)
                throw new CashFlowerException("CFE_ABN_008",
                    "Fewer characters ({0}) than excpected (33) in Point of Sale Terminal Details: '{1}'"
                    , contraAccountDetails.Length, contraAccountDetails);

            return new ContraAccountDetails
            {
                DateTimeStamp = _parsePointOfSaleTerminalDetailsTimeStamp(contraAccountDetails.Substring(18, 14)),
                ContraAccountName = ContraAccountNameHelper.StripCardnumber(contraAccountDetails.Substring(33))
            };
        }

        private static DateTime _parsePointOfSaleTerminalDetailsTimeStamp(string contraAccountDetailsTimeStamp)
        {
            DateTime myDate;
            if (ContraAcountDetailsTimeStampExtracter.TryExecute(contraAccountDetailsTimeStamp, out myDate))
                return myDate;
            throw new CashFlowerException("CFE_ABN_009", "No Valid contraAccountDetailsTimeStamp ({0}) given.", contraAccountDetailsTimeStamp);
        }
    }
}
