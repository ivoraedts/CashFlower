using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers;
using CashFlower.Test.Shared;
using NUnit.Framework;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.Tests.Tests.AbnAmroTabReaderHelpers.ContraAcountDetailsExtracterHelpers
{
    [TestFixture]
    public class SepaPaymentDetailsExtracterTests
    {
        [Test]
        [ExpectedCashFlowerException("CFE_ABN_013")]
        public void GivenInvalidSepaPaymentDetails_ThrowsException()
        {
            SepaPaymentDetailsExtracter.Execute(":");
        }

        [Test]
        [ExpectedCashFlowerException("CFE_ABN_012")]
        public void GivenEmptyString_ThrowsException()
        {
            SepaPaymentDetailsExtracter.Execute("");
        }

        [Test]
        public void GivenSepaPaymentStringContainingName_ReturnsName()
        {
            var result = SepaPaymentDetailsExtracter.Execute("header Naam:Ivo");
            Assert.AreEqual("Ivo", result.ContraAccountName);
            Assert.IsNull(result.ContraAccountIban);
            Assert.IsNull(result.DateTimeStamp);
        }

        [Test]
        public void GivenSepaPaymentStringContainingIban_ReturnsIban()
        {
            var result = SepaPaymentDetailsExtracter.Execute("header IBAN: NL02RABO1234567890 anders: kjell");
            Assert.AreEqual("NL02RABO1234567890", result.ContraAccountIban);
            Assert.IsNull(result.ContraAccountName);
            Assert.IsNull(result.DateTimeStamp);
        }
    }
}
