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
        public void GivenLineWithPaymentFromPointOfSaleTerminalWithoutEnoughCharacters_ExceptionIsThrown()
        {
            ContraAcountDetailsExtracter.Execute("BEA   NR:108T6F   07.03.16/19.57");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_009")]
        public void GivenLineWithPaymentFromPointOfSaleTerminalWithInvalidDate_ExceptionIsThrown()
        {
            ContraAcountDetailsExtracter.Execute("BEA   NR:108T6F   07.03.2016/19.57 ALB");
        }

        [Test]
        public void GivenLineWithPaymentFromPointOfSaleTerminalWitJustEnoughCharacters_BanktransferLineIsReturned()
        {
            var result = ContraAcountDetailsExtracter.Execute("BEA   NR:108T6F   07.03.16/19.57 ");

            Assert.AreEqual(new DateTime(2016, 3, 7, 19, 57, 0),result.DateTimeStamp);
            Assert.AreEqual("", result.ContraAccountName);
        }
    }
}
