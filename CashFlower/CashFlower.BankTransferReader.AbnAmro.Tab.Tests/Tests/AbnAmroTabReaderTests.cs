using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests
{
    [TestFixture]
    public class AbnAmroTabReaderTests
    {
        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GivenUnexistingFile_ThrowsException()
        {
            new AbnAmroTabReader(@"Some//Unexisting//Location").GetBankTransfers();
        }

        [Test]
        public void GivenEmptyFile_EmptyListIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\EmptyFile.tab");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.IsEmpty(result);
        }

        [Test]
        public void GivenLineWithPaymentFromPointOfSaleTerminal_BanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_BoughtStuffAtSupermarket.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 3, 7, 19, 57, 0), bankTransferLine.TransactionDate);
            Assert.AreEqual(85, bankTransferLine.InitialBalance);
            Assert.AreEqual(31.49, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.InterestDate);
            Assert.AreEqual((decimal)-53.51, bankTransferLine.Amount);
            Assert.AreEqual("ALBERT HEIJN 1394 NORTHR", bankTransferLine.ContraAccountDescription);
        }

        [Test]
        public void GivenLineWithCashWithdrawalDetails_BanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_CashWithdrawal.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 3, 7, 18, 29, 0), bankTransferLine.TransactionDate);
            Assert.AreEqual(100, bankTransferLine.InitialBalance);
            Assert.AreEqual(90, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.InterestDate);
            Assert.AreEqual(-10, bankTransferLine.Amount);
            Assert.AreEqual("MARKET 4 NORTHRIDGE", bankTransferLine.ContraAccountDescription);
        }
    }
}
