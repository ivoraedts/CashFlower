using System;
using System.IO;
using System.Linq;
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
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_PointOfSaleTerminalDetailsWithoutEnoughCharacters.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_009")]
        public void GivenLineWithPaymentFromPointOfSaleTerminalWithInvalidDate_ExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_PointOfSaleTerminalDetailsWithInvalidDate.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();
        }

        [Test]
        public void GivenLineWithPaymentFromPointOfSaleTerminalWitJustEnoughCharacters_BanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_PointOfSaleTerminalDetailsWithJustEnoughCharacters.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual(new DateTime(2016, 3, 7, 19, 57, 0), bankTransferLine.TransactionDate);
            Assert.AreEqual("", bankTransferLine.ContraAccountDescription);
        }
    }
}
