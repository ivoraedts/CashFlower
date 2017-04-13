using System.Collections.Generic;
using CashFlower.Contracts;
using CashFlower.Contracts.Tests.Stubs.IBankTransferReader;
using CashFlower.Contracts.Tests.Stubs.IExistingBankTransferDeterminator;
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
            new BankTransferImporter(new CrashingBankTransferReader(), null).Execute();
        }

        [Test]
        public void GivenReaderThatReturnsEmptyList_NoExceptionIsRaised()
        {
            new BankTransferImporter(new ReaderThatReturnsEmptyList(), null).Execute();
        }

        [Test]
        [ExpectedException(ExpectedMessage = "Oh no! I am crashed while trying to determine if banktransfer exists!")]
        public void GivenReaderThatReturnsSomeResults_WhenExistingBankTransferDeterminatorCrashes_ExceptionIsRaised()
        {
            new BankTransferImporter(
                new ReaderThatReturnsPreSetList(new List<BankTransferLine> {new BankTransferLine()}),
                new BankTransferMatcherThatCrashes()).Execute();
        }

        [Test]
        public void GivenReaderThatReturnsSomeResults_WhenMatcherReturnsTrue_NoExceptionsIsRaised()
        {
            new BankTransferImporter(
                new ReaderThatReturnsPreSetList(new List<BankTransferLine> {new BankTransferLine()}),
                new BankTransferMatcherIndicatingAllTransfersAlreadyExist()).Execute();
        }
    }
}
