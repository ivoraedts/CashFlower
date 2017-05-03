using System;
using CashFlower.WebApi.Controllers;
using NUnit.Framework;
using CashFlower.Contracts;
using System.Linq;
using System.IO;
using System.Configuration;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class BankTransferControllerTests
    {
        [SetUp]
        public void Setup()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\EmptyXmlFile.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
        }

        [Test]
        public void GivenEmptyStore_GetWithoutArgumentsResultsInEmptyList()
        {
            var ctr = new BankTransferController();
            var results = ctr.Get().ToList();
            Assert.IsEmpty(results);
        }

        [Test]
        public void GivenStoreWithOneBankTransfer_GetWithoutArgumentsResultsOneBankTransfer()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\XmlFileWithOneBanktransfer.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            var ctr = new BankTransferController();
            var results = ctr.Get().ToList();
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void WhenNewBankTransferIsPosted_NotImplementedExceptionIsThrown()
        {
            new BankTransferController().Post(new BankTransfer());
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void WhenTyingToDeleteBankTransfer_NotImplementedExceptionIsThrown()
        {
            new BankTransferController().Delete("SomeGUID");
        }
    }
}
