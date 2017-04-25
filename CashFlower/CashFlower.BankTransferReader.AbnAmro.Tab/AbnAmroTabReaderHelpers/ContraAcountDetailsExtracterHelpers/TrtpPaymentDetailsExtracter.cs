using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.TrtpPaymentDetailsExtracterHelpers;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    public static class TrtpPaymentDetailsExtracter
    {
        public static ContraAccountDetails Execute(string trtpPaymentDetailsString)
        {
            var trtpPaymentDetails = TrtpKeyValuePairExtracter.Execute(trtpPaymentDetailsString);

            if (!trtpPaymentDetails.ContainsKey("NAME") && (!trtpPaymentDetails.ContainsKey("IBAN")))
            {
                throw new CashFlowerException(
                    "CFE_ABN_014", "TRTP payment details does not contain name and IBAN. " +
                                   "Given details : '{0}'", trtpPaymentDetails);
            }

            return new ContraAccountDetails
            {
                ContraAccountName = trtpPaymentDetails.GetValueOrNull("NAME"),
                ContraAccountIban = trtpPaymentDetails.GetValueOrNull("IBAN")
            };
        }
    }
}
