using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    [TestFixture]
    public class TrtpPaymentDetailsExtracterTests
    {
        [Test]
        [ExpectedCashFlowerException("CFE_ABN_014")]
        public void GivenTrtpLineWithoutNameAndIban_ThrowsException()
        {
            TrtpPaymentDetailsExtracter.Execute(
                @"/TRTP/iDEAL/BOMB/NL11RABO0101010101/BIC/RABONL2U/FCUK/Ingenico /REMI/3232323255 0021112223334466 CNL1111111 internetstores GmbH/EREF/06-12-2016 20:38 0101010101010101  ");
        }

        [Test]
        public void GivenTrtpLineWithName_ReturnsResult()
        {
            var result =
            TrtpPaymentDetailsExtracter.Execute(
                @"/TRTP/iDEAL/BOMB/NL11RABO0101010101/BIC/RABONL2U/NAME/Ingenico /REMI/3232323255 0021112223334466 CNL1111111 internetstores GmbH/EREF/06-12-2016 20:38 0101010101010101  ");
            Assert.AreEqual("Ingenico", result.ContraAccountName);
            Assert.IsNull(result.ContraAccountIban);
        }

        [Test]
        public void GivenTrtpLineWithNameAndIban_ReturnsResult()
        {
            var result =
            TrtpPaymentDetailsExtracter.Execute(
                @"/TRTP/iDEAL/IBAN/NL11RABO0101010101/BIC/RABONL2U/NAME/Ingenico /REMI/3232323255 0021112223334466 CNL1111111 internetstores GmbH/EREF/06-12-2016 20:38 0101010101010101  ");
            Assert.AreEqual("Ingenico", result.ContraAccountName);
            Assert.AreEqual("NL11RABO0101010101", result.ContraAccountIban);
        }

        [Test]
        public void GivenTrtpLineWithAnotherNameAndIban_ReturnsResult()
        {
            var result =
            TrtpPaymentDetailsExtracter.Execute(
                @"/TRTP/iDEAL/IBAN/NL55ABNA1234454236/BIC/ABNANL2A/NAME/STG AAPJE/REMI/1414141414141489 0012314234424426 2 x Alligator restaurant & club 000123Dot.com/EREF/11-12-2016 16:02 0012352353245325  ");
            Assert.AreEqual("STG AAPJE", result.ContraAccountName);
            Assert.AreEqual("NL55ABNA1234454236", result.ContraAccountIban);
        }
     
    }
}
