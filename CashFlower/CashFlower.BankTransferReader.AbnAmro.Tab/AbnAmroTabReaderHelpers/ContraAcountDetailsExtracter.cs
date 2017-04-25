using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public static class ContraAcountDetailsExtracter
    {
        public static ContraAccountDetails Execute(string contraAccountDetails)
        {
            if (contraAccountDetails.StartsWith("BEA   NR:"))
                return PointOfSaleTerminalDetailsExtracter.Execute(contraAccountDetails);
            if (contraAccountDetails.StartsWith("GEA   NR:"))
                return CashWithdrawalDetailsExtracter.Execute(contraAccountDetails);
            if (contraAccountDetails.StartsWith("SEPA"))
                return SepaPaymentDetailsExtracter.Execute(contraAccountDetails);
            if (contraAccountDetails.StartsWith("ABN AMRO Bank N.V."))
                return new ContraAccountDetails { ContraAccountName = "ABN AMRO Bank N.V." };
            if (contraAccountDetails.StartsWith("PAKKETVERZ."))
                return new ContraAccountDetails { ContraAccountName = "ABN AMRO Bank N.V. PAKKETVERZEKERING" };
            if (contraAccountDetails.StartsWith(@"/TRTP"))
                return TrtpPaymentDetailsExtracter.Execute(contraAccountDetails);

            throw new CashFlowerException(
                "CFE_ABN_015", 
                "Encountered Unknown type of Contra Account Details : {0}",
                contraAccountDetails);
        }
    }
}
