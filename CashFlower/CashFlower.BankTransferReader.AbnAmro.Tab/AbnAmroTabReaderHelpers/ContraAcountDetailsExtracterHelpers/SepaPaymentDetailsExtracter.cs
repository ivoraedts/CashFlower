using
    CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.
        SepaPaymentDetailsExtracterHelpers;
using CashFlower.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    public static class SepaPaymentDetailsExtracter
    {
        public static ContraAccountDetails Execute(string sepaPaymentDetailsString)
        {
            var sepaPaymentDetails = KeyValuePairExtracter.Execute(sepaPaymentDetailsString);

            if (!sepaPaymentDetails.ContainsKey("Naam") && (!sepaPaymentDetails.ContainsKey("IBAN")))
            {
                throw new CashFlowerException(
                    "CFE_ABN_012", "SEPA payment details does not contain name and IBAN. " +
                                   "Given sepa details : '{0}'", sepaPaymentDetails);
            }

            return new ContraAccountDetails {
                ContraAccountName = sepaPaymentDetails.GetValueOrNull("Naam"),
                ContraAccountIban = sepaPaymentDetails.GetValueOrNull("IBAN")
            };
        }
    }
}