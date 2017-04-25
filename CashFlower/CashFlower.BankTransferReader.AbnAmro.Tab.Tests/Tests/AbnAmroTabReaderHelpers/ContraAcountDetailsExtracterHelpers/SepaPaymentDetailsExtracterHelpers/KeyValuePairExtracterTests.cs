using System;
using System.Collections.Generic;
using System.Linq;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.SepaPaymentDetailsExtracterHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.SepaPaymentDetailsExtracterHelpers
{
    [TestFixture]
    public class KeyValuePairExtracterTests
    {
        [Test]
        public void GivenEmptyString_ReturnsEmptyHeader()
        {
            var res = KeyValuePairExtracter.Execute("");
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("", res.Values.Single());
            Assert.AreEqual("SEPA Header", res.Keys.Single());
        }

        [Test]
        public void GivenHeaderOnly_ReturnsGivenStringAsHeader()
        {
            var res = KeyValuePairExtracter.Execute("SEPA Overboeking                 ");
            Assert.AreEqual(1, res.Count);
            Assert.AreEqual("SEPA Overboeking", res.Values.Single());
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_013")]
        public void GivenNospacebeforeColon_ThrowsException()
        {
            KeyValuePairExtracter.Execute(":  gfdjgdfkv fdksjngkjnfvv  vsjfdslkfsdvs ");
        }

        [Test]
        public void GivenSomeDate_ReturnsExpectedData()
        {
            var res = KeyValuePairExtracter.Execute("Header key1:value1 key2:value 2 key3: value3");
            Assert.AreEqual(4, res.Count);

            Assert.IsTrue(res.ContainsKey("key1"));
            Assert.AreEqual("value1", res.GetValueOrNull("key1"));

            Assert.IsTrue(res.ContainsKey("key2"));
            Assert.AreEqual("value 2", res.GetValueOrNull("key2"));
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_013")]
        public void GivenNospacebetweenSemiColons_ThrowsException()
        {
            KeyValuePairExtracter.Execute("kfdgkjf sfdlvsdv sdffds: dfsjknfdskn kdsjfdsk :theError: fsdodhkgjf : joknk");
        }

        [Test]
        public void GivenGetValueThatIsNotInDictionary_ReturnsNull()
        {
            Assert.IsNull(new Dictionary<string, string>().GetValueOrNull("I Don't exist"));
        }

        [Test]
        public void GivenSepaRecordWithDoubleKey_DoubleKeyIsMerged()
        {
            var result = KeyValuePairExtracter.Execute(
                    "//SEPA Incasso algemeen doorlopend Incassant: NL55ZZZ111111123456  Naam: De Nederlanden van Nu      Machtiging: 654321607122015123456789                             Omschrijving: Kenmerk: 3125.9993.3199.3338 Omschrijving: POLIS D D 13-07-16 NOTA 012345674 POLISNR NVN54321 Wa/Beperkt Casco ");
            Assert.IsTrue(result.ContainsKey("Omschrijving"));
            Assert.IsTrue(result["Omschrijving"].Contains("POLIS D D 13-07-16"));
        }
    }
}
