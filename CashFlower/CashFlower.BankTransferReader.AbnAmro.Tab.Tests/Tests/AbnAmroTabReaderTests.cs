using System.IO;
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
    }
}
