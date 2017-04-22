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
        public void GivenContraAccountDetailsWithPaymentFromPointOfSaleTerminalWithJustEnoughCharacters_ReturnsContraAccountDetails()
        {
            var result = ContraAcountDetailsExtracter.Execute("BEA   NR:108T6F   07.03.16/19.57 ");
            Assert.AreEqual(new DateTime(2016, 3, 7, 19, 57, 0),result.DateTimeStamp);
            Assert.AreEqual("", result.ContraAccountName);
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_010")]
        public void GivenContraAccountDetailsWithPaymentFromCashWithdrawalWithoutEnoughCharacters_ThrowsException()
        {
            ContraAcountDetailsExtracter.Execute("GEA   NR:S1N413   07.03.16/18.29");
        }

        [Test]
        public void GivenContraAccountDetailsWithPaymentFromCashWithdrawalWithJustEnoughCharacters_ReturnsContraAccountDetails()
        {
            var result = ContraAcountDetailsExtracter.Execute("GEA   NR:S1N413   07.03.16/18.29 ");
            Assert.AreEqual(new DateTime(2016, 3, 7, 18, 29, 0), result.DateTimeStamp);
            Assert.AreEqual("", result.ContraAccountName);
        }
    }
}
