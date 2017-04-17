using System;
using System.IO;
using System.Linq;
using CashFlower.Test.Shared;
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
        [ExpectedCashFlowerException("CFE_ABN_001")]
        public void GivenFileWithWrongNumberOfTabs_ThenExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_WrongNumberOfTabs.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_002")]
        public void GivenLineWithOtherCurrencyThanEuro_AnExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_WrongCurrency.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_003")]
        public void GivenLineWithInvalidTransactionDate_AnExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_InvalidTransactionDate.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();            
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_004")]
        public void GivenLineWithInvalidInitialBalance_AnExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_InvalidInitialBalance.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();              
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_005")]
        public void GivenLineWithInvalidFinalBalance_AnExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_InvalidFinalBalance.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();              
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_006")]
        public void GivenLineWithInvalidInterestDate_AnExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_InvalidInterestDate.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();             
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_007")]
        public void GivenLineWithInvalidAmount_ExceptionIsThrown()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Recources\ExampleLine_InvalidAmount.TAB");
            new AbnAmroTabReader(filename).GetBankTransfers();
        }

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
            Assert.AreEqual(new DateTime(2016,3,7), bankTransferLine.InterestDate);
            Assert.AreEqual((decimal)-53.51, bankTransferLine.Amount);
            Assert.AreEqual("ALBERT HEIJN 1394 NORTHR", bankTransferLine.ContraAccountDescription);
        }
    }
}
