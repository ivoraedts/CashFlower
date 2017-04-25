using System;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers
{
    [TestFixture]
    public class ContraAcountDetailsExtracterTests
    {
        [Test]
        [ExpectedCashFlowerException("CFE_ABN_008")]
        public void GivenContraAccountDetailsWithPaymentFromPointOfSaleTerminalWithoutEnoughCharacters_ThrowsException()
        {
            ContraAcountDetailsExtracter.Execute("BEA   NR:108T6F   07.03.16/19.57");
        }

        [Test]
        public void
            GivenContraAccountDetailsWithPaymentFromPointOfSaleTerminalWithJustEnoughCharacters_ReturnsContraAccountDetails
            ()
        {
            var result = ContraAcountDetailsExtracter.Execute("BEA   NR:108T6F   07.03.16/19.57 ");
            Assert.AreEqual(new DateTime(2016, 3, 7, 19, 57, 0), result.DateTimeStamp);
            Assert.AreEqual("", result.ContraAccountName);
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_010")]
        public void GivenContraAccountDetailsWithPaymentFromCashWithdrawalWithoutEnoughCharacters_ThrowsException()
        {
            ContraAcountDetailsExtracter.Execute("GEA   NR:S1N413   07.03.16/18.29");
        }

        [Test]
        public void
            GivenContraAccountDetailsWithPaymentFromCashWithdrawalWithJustEnoughCharacters_ReturnsContraAccountDetails()
        {
            var result = ContraAcountDetailsExtracter.Execute("GEA   NR:S1N413   07.03.16/18.29 ");
            Assert.AreEqual(new DateTime(2016, 3, 7, 18, 29, 0), result.DateTimeStamp);
            Assert.AreEqual("", result.ContraAccountName);
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_012")]
        public void GivenContraAccountDetailsWithOfSepaType_WithoutNameAndIban_ThrowsException()
        {
            ContraAcountDetailsExtracter.Execute(
                "SEPA Incasso algemeen doorlopend Incassant: NL18ZZZ15438675834700  Machtiging: 0002222222243        Omschrijving: 222222222222/KLANT  222222222 KNMRK 222222222/FACT 222222222222 DAT. 14102016/Termijn 33,00 BTW 15,10");
        }

        [Test]
        public void GivenCostsFromAbnAmro_LineIsReturned()
        {
            var result = ContraAcountDetailsExtracter.Execute(
                "ABN AMRO Bank N.V.               BetaalGemak S               1,40Korting 50%                -0,70                                 ");
            Assert.AreEqual("ABN AMRO Bank N.V.", result.ContraAccountName);
        }

        [Test]
        public void GivenInsuranceCostsFromAbnAmro_LineIsReturned()
        {
            var result = ContraAcountDetailsExtracter.Execute(
                "PAKKETVERZ. POLISNR.   123455678 VERZEKERINGSBEWIJS DD 06-05-16");
            Assert.AreEqual("ABN AMRO Bank N.V. PAKKETVERZEKERING", result.ContraAccountName);
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_014")]
        public void GivenTrtpLineWithoutNameAndIban_ThrowsException()
        {
            ContraAcountDetailsExtracter.Execute(
                @"/TRTP/iDEAL/BOMB/NL11RABO0101010101/BIC/RABONL2U/FCUK/Ingenico /REMI/3232323255 0021112223334466 CNL1111111 internetstores GmbH/EREF/06-12-2016 20:38 0101010101010101  ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_015")]
        public void GivenContraAccountDetailsfUnknownType_ThrowsException()
        {
            ContraAcountDetailsExtracter.Execute("UnknownType   NR:108T6F   07.03.16/19.57");
        }
    }
}