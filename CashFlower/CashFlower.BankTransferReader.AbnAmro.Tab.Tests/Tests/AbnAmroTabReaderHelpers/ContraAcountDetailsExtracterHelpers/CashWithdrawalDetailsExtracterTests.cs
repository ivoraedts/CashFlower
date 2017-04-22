using System;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    [TestFixture]
    public class CashWithdrawalDetailsExtracterTests
    {
        [Test]
        [ExpectedCashFlowerException("CFE_ABN_010")]
        public void GivenContraAccountDetailsWithPaymentFromPointOfSaleTerminalWithoutEnoughCharacters_ExceptionIsThrown()
        {
            CashWithdrawalDetailsExtracter.Execute("GEA   NR:108T6F   07.03.16/19.57");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_011")]
        public void GivenContraAccountDetailsWithPaymentFromPointOfSaleTerminalWithInvalidDateString_ExceptionIsThrown()
        {
            CashWithdrawalDetailsExtracter.Execute("GEA   NR:108T6F   17.13.16/19.57   ");
        }

        [Test]
        public void GivenValidCashwithdrawalString_ExpectedDetailsAreReturned()
        {
            var result = CashWithdrawalDetailsExtracter.Execute(
                "GEA   NR:S1N413   07.03.16/18.29 MARKET 4 NORTHRIDGE,PAS464      ");
            Assert.AreEqual(new DateTime(2016, 3, 7, 18, 29, 0), result.DateTimeStamp);
            Assert.AreEqual("MARKET 4 NORTHRIDGE", result.ContraAccountName);
        }
    }
}
