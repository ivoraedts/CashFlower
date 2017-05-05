using CashFlower.WebApi.Controllers;
using NUnit.Framework;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using CashFlower.Contracts;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class PeriodicFigureController_GetWithoutParametersTests
    {
        [SetUp]
        public void SetUp()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\EmptyXmlFile.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
        }

        [Test]
        public void GivenNoBanktransfers_GetPeriodicFigureReturnsZeros()
        {
            var ctr = new PeriodicFigureController();
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(0, result.Income);
        }

        [Test]
        public void GivenStoreWithOneBankTransfer_GetPeriodicFigureReturnsSomeValue()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\XmlFileWithOneBanktransfer.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            var ctr = new PeriodicFigureController();
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(323, result.Income);
        }

        [Test]
        public void GivenStoreWithOnePositiveAndOneNegativeBankTransfer_GetPeriodicReturnsFigureWithoutZeros()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { Amount = 5 },
                new BankTransfer { Amount = -13 }
            });
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(13, result.Expenses);
            Assert.AreEqual(5, result.Income);
        }

        [Test]
        public void GivenStoreWithTwoPositiveAndTwoNegativeBankTransfers_GetPeriodicReturnsFigureWithoutAmountsAdded()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { Amount = 5 },
                new BankTransfer { Amount = 55 },
                new BankTransfer { Amount = -13 },
                new BankTransfer { Amount = -11 }
            });
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(24, result.Expenses);
            Assert.AreEqual(60, result.Income);
        }

        [Test]
        public void GivenStoreWithBankTransferThatMustBeIgnoredAccordingToExtension_ReturnsZero()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { Amount = 5 , Extension = new BankTransferExtension { HideFromCalculations = true } },
            });
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Income);
        }
    }
}
