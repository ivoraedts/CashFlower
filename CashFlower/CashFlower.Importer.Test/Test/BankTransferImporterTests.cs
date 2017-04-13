using System.Collections.Generic;
using System.Linq;
using CashFlower.Contracts;
using CashFlower.Contracts.Tests.Stubs.IBankTransferReader;
using CashFlower.Contracts.Tests.Stubs.IExistingBankTransferDeterminator;
using CashFlower.Contracts.Tests.Stubs.IStoreBankTransfers;
using NUnit.Framework;

namespace CashFlower.Importer.Test.Test
{
    [TestFixture]
    public class BankTransferImporterTests
    {
        [Test]
        [ExpectedException(ExpectedMessage = "Oh no! I am crashed!")]
        public void GivenErrorFromReader_ThrowsException()
        {
            new BankTransferImporter(new CrashingBankTransferReader(), null, null).Execute();
        }

        [Test]
        public void GivenReaderThatReturnsEmptyList_NoExceptionIsRaisedAndNoBankTransfersAreStored()
        {
            var storeSpy = new BankTransferStoreSpy();

            new BankTransferImporter(new ReaderThatReturnsEmptyList(), null, storeSpy).Execute();

            Assert.IsEmpty(storeSpy.ProcessedRequests);
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Oh no! I am crashed while trying to determine if banktransfer exists!")]
        public void GivenReaderThatReturnsSomeResults_WhenExistingBankTransferDeterminatorCrashes_ExceptionIsRaised()
        {
            new BankTransferImporter(
                new ReaderThatReturnsPreSetList(new List<BankTransferLine> { new BankTransferLine() }),
                new BankTransferMatcherThatCrashes(), null).Execute();
        }

        [Test]
        public void GivenReaderThatReturnsSomeResults_WhenMatcherReturnsTrue_NoExceptionsIsRaisedAndNoBanktransfersAreStored()
        {
            var storeSpy = new BankTransferStoreSpy();

            new BankTransferImporter(
                new ReaderThatReturnsPreSetList(new List<BankTransferLine> { new BankTransferLine() }),
                new BankTransferMatcherIndicatingAllTransfersAlreadyExist(), storeSpy).Execute();

            Assert.IsEmpty(storeSpy.ProcessedRequests);
        }

        [Test]
        [ExpectedException(ExpectedMessage = "I just crashed while storing a bank transfer")]
        public void GivenReaderThatReturnsSomeResults_WhenMatcherReturnsFalse_WhenStorerCrashes_ExceptionIsRaised()
        {
            new BankTransferImporter(
                new ReaderThatReturnsPreSetList(new List<BankTransferLine> { new BankTransferLine() }),
                new BankTransferMatcherIndicatingNoTransfersAlreadyExists(),
                new BankTransfersStorerThatCrashes()).Execute();
        }

        [Test]
        public void GivenReaderThatReturnsSomeResults_WhenMatcherReturnsFalse_BankTransfersAreStored()
        {
            const string accountNumber1 = "123ABC";
            const string accountNumber2 = "123ABCDE";
            var storeSpy = new BankTransferStoreSpy();

            new BankTransferImporter(
                new ReaderThatReturnsPreSetList(
                    new List<BankTransferLine> {
                        new BankTransferLine {
                            AccountNumber = accountNumber1
                        }
                        ,
                        new BankTransferLine {
                            AccountNumber = accountNumber2
                        }
                    }),
                new BankTransferMatcherIndicatingNoTransfersAlreadyExists(),
                storeSpy).Execute();

            Assert.IsNotEmpty(storeSpy.ProcessedRequests);
            Assert.AreEqual(2, storeSpy.ProcessedRequests.Count);
            Assert.IsTrue(storeSpy.ProcessedRequests.Any(p => p.AccountNumber == accountNumber1));
            Assert.IsTrue(storeSpy.ProcessedRequests.Any(p => p.AccountNumber == accountNumber2));
        }
    }
}