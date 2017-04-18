using System;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public static class ContraAcountDetailsExtracter
    {
        public static ContraAccountDetails Execute(string contraAccountDetails)
        {
            if (contraAccountDetails.StartsWith("BEA   NR:"))
                return PointOfSaleTerminalDetailsExtracter.Execute(contraAccountDetails);
            throw new NotImplementedException();
        }

  
    }
}
