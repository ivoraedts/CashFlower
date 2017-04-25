using System.Linq;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.TrtpPaymentDetailsExtracterHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers.TrtpPaymentDetailsExtracterHelpers
{
    [TestFixture]
    public class TrtpKeyValuePairExtracterTests
    {
        [Test]
        public void GivenSlashdelimitedStringWithoutIbanAndNameTag_ReturnsKeyValues()
        {
            var result = TrtpKeyValuePairExtracter.Execute(@"/keyOne/valueOne/keyTwo/valueTwo");
            Assert.IsEmpty(result);
        }

        [Test]
        public void GivenSlashdelimitedStringWithJustNameTag_ReturnsName()
        {
            var result = TrtpKeyValuePairExtracter.Execute(@"/NAME/valueOne/keyTwo/valueTwo");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("valueOne", result.Single().Value);
            Assert.AreEqual("NAME", result.Single().Key);
        }

        [Test]
        public void GivenSlashdelimitedStringWithJustIbanTag_ReturnsName()
        {
            var result = TrtpKeyValuePairExtracter.Execute(@"/notthename/valueOne/IBAN/valueTwo");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("valueTwo", result.Single().Value);
            Assert.AreEqual("IBAN", result.Single().Key);
        }

        [Test]
        public void GivenSlashDelimitedStringWithIbanAndName_ReturnsBoth()
        {
            var result = TrtpKeyValuePairExtracter.Execute(@"/TRTP/iDEAL/IBAN/NL11RABO0101010101/BIC/RABONL2U/"
                + @"NAME/Ingenico /REMI/3232323255 0021112223334466 CNL1111111 internetstores GmbH/"
                + @"EREF/06-12-2016 20:38 0101010101010101  ");
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("IBAN"));
            Assert.AreEqual("NL11RABO0101010101", result["IBAN"]);
            Assert.IsTrue(result.ContainsKey("NAME"));
            Assert.AreEqual("Ingenico", result["NAME"]);
        }
    }
}
