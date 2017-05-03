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
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\EmptyFile.tab");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.IsEmpty(result);
        }

        [Test]
        public void GivenLineWithPaymentFromPointOfSaleTerminal_BanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ExampleLine_BoughtStuffAtSupermarket.TAB");
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
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ExampleLine_CashWithdrawal.TAB");
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

        [Test]
        public void GivenLineWithSepaPayment_MonthlyBillForMobile_ExpectedBanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ExampleLine_MonthlyBillForMobile.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 4, 21), bankTransferLine.TransactionDate);
            Assert.AreEqual((decimal)997.07, bankTransferLine.InitialBalance);
            Assert.AreEqual((decimal)987.07, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 4, 21), bankTransferLine.InterestDate);
            Assert.AreEqual(-10, bankTransferLine.Amount);
            Assert.AreEqual("YOUFONE NEDERLAND BV", bankTransferLine.ContraAccountDescription);
            Assert.AreEqual("NL32ABNA0522222212", bankTransferLine.ContraAccountIban);
        }

        [Test]
        public void GivenLineWithSepaPayment_PaidJohnDoe_ExpectedBanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ExampleLine_PayedMoneyToJohnDoe.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.TransactionDate);
            Assert.AreEqual(90, bankTransferLine.InitialBalance);
            Assert.AreEqual(85, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.InterestDate);
            Assert.AreEqual(-5, bankTransferLine.Amount);
            Assert.AreEqual("J DOE", bankTransferLine.ContraAccountDescription);
            Assert.AreEqual("NL88RABO0173737871", bankTransferLine.ContraAccountIban);
        }

        [Test]
        public void GivenLineWithSepaPayment_ReceivedFromJohnDoe_ExpectedBanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ExampleLine_ReceivedMoneyFromJohnDoe.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.TransactionDate);
            Assert.AreEqual(0, bankTransferLine.InitialBalance);
            Assert.AreEqual(100, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.InterestDate);
            Assert.AreEqual(100, bankTransferLine.Amount);
            Assert.AreEqual("J. DOE", bankTransferLine.ContraAccountDescription);
            Assert.AreEqual("NL88RABO0173737871", bankTransferLine.ContraAccountIban);
        }

        [Test]
        public void GivenLineWithSepaPayment_ReceivedSalary_ExpectedBanktransferLineIsReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ExampleLine_ReceivedSalary.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(1, result.Count);

            var bankTransferLine = result.Single();
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 3, 24), bankTransferLine.TransactionDate);
            Assert.AreEqual((decimal)2049.37, bankTransferLine.InitialBalance);
            Assert.AreEqual((decimal)4131.23, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 3, 24), bankTransferLine.InterestDate);
            Assert.AreEqual((decimal)2081.86, bankTransferLine.Amount);
            Assert.AreEqual("NINJA SOFTWARE", bankTransferLine.ContraAccountDescription);
            Assert.AreEqual("NL22ABNA0222228847", bankTransferLine.ContraAccountIban);
        }

        [Test]
        public void GivenFileWithManyLines_ManyBankTransferLinesAreReturned()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\ModifiedExample.TAB");
            var result = new AbnAmroTabReader(filename).GetBankTransfers();
            Assert.AreEqual(39, result.Count);
            Assert.AreEqual(39, result.Where(line => line.AccountNumber == "480226345").ToList().Count);
            Assert.IsTrue(result.Any(line => line.ContraAccountDescription == "vof de Resting Hunter"));
        }
    }
}
