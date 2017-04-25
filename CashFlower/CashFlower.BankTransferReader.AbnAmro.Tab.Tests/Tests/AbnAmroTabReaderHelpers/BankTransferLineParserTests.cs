using System;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers
{
    [TestFixture]
    public class BankTransferLineParserTests
    {
        [Test]
        [ExpectedCashFlowerException("CFE_ABN_001")]
        public void GivenFileWithWrongNumberOfTabs_ThenExceptionIsThrown()
        {
            BankTransferLineParser.Execute(
                "480226345	480226345	EUR	20160307	85,00	31,49	20160307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_002")]
        public void GivenLineWithOtherCurrencyThanEuro_AnExceptionIsThrown()
        {
            BankTransferLineParser.Execute(
                "480226345	DOL	20160307	85,00	31,49	20160307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_003")]
        public void GivenLineWithInvalidTransactionDate_AnExceptionIsThrown()
        {
            BankTransferLineParser.Execute("480226345	EUR	20161307	85,00	31,49	20160307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_004")]
        public void GivenLineWithInvalidInitialBalance_AnExceptionIsThrown()
        {
            BankTransferLineParser.Execute("480226345	EUR	20160307	85.00	31,49	20160307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_005")]
        public void GivenLineWithInvalidFinalBalance_AnExceptionIsThrown()
        {
            BankTransferLineParser.Execute("480226345	EUR	20160307	85,00	3149	20160307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_006")]
        public void GivenLineWithInvalidInterestDate_AnExceptionIsThrown()
        {
            BankTransferLineParser.Execute("480226345	EUR	20160307	85,00	31,49	20161307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_007")]
        public void GivenLineWithInvalidAmount_ExceptionIsThrown()
        {
            BankTransferLineParser.Execute("480226345	EUR	20160307	85,00	31,49	20160307	-53a5,1	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
        }

        [Test]
        public void GivenValidLine_ReturnsResult()
        {
            var bankTransferLine = BankTransferLineParser.Execute("480226345	EUR	20160307	85,00	31,49	20160307	-53,51	BEA   NR:108T6F   07.03.16/19.57 ALBERT HEIJN 1394 NORTHR,PAS464 ");
            Assert.AreEqual("480226345", bankTransferLine.AccountNumber);
            Assert.AreEqual(new DateTime(2016, 3, 7, 19, 57, 0), bankTransferLine.TransactionDate);
            Assert.AreEqual(85, bankTransferLine.InitialBalance);
            Assert.AreEqual(31.49, bankTransferLine.FinalBalance);
            Assert.AreEqual(new DateTime(2016, 3, 7), bankTransferLine.InterestDate);
            Assert.AreEqual((decimal)-53.51, bankTransferLine.Amount);
            Assert.AreEqual("ALBERT HEIJN 1394 NORTHR", bankTransferLine.ContraAccountDescription);
        }
    }
}
