using System.IO;
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
    }
}
