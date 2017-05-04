using System;
using CashFlower.WebApi.Controllers;
using NUnit.Framework;
using CashFlower.Contracts;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using CashFlower.Test.Shared;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class BankTransferControllerTests
    {
        private BankTransferController _ctr;

        [SetUp]
        public void Setup()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\EmptyXmlFile.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            _ctr = new BankTransferController();
        }

        [Test]
        public void GivenEmptyStore_GetWithoutArgumentsResultsInEmptyList()
        {
            var results = _ctr.Get().ToList();
            Assert.IsEmpty(results);
        }

        [Test]
        public void GivenStoreWithOneBankTransfer_GetWithoutArgumentsResultsOneBankTransfer()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\XmlFileWithOneBanktransfer.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            _ctr = new BankTransferController();
            var results = _ctr.Get().ToList();
            Assert.AreEqual(1, results.Count);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetWithUnexistingId_ThrowsException()
        {
            _ctr.Get("UnknownId");
        }

        [Test]
        public void GetWithExistingId_ReturnsResult()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\XmlFileWithOneBanktransfer.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            _ctr = new BankTransferController();
            var result = _ctr.Get("FakeGUID");
            Assert.IsNotNull(result);
            Assert.AreEqual(323, result.Amount);
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void PutWithUnexistingId_ThrowsException()
        {
            _ctr.Put(new BankTransfer { Id = "UnknownId" });
        }

        [Test]
        public void GetPutExistingId_UpdatesOnlyExtensionOfExistingBankTransfer()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\XmlFileWithOneBanktransfer.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            _ctr = new BankTransferController();
            _ctr.Put(new BankTransfer
            {
                Id = "FakeGUID",
                Account = new Account { AccountNumber = "This should be ignored", Id = ".. ignore" },
                Extension = new BankTransferExtension
                {
                    CalculationDate = new DateTime(2022, 5, 22),
                    DistributionType = DistributionType.Year,
                    HideFromCalculations = true
                }
            });
            var transfer = _ctr.Get("FakeGUID");
            Assert.IsNotNull(transfer);
            Assert.IsNotNull(transfer.Account);
            Assert.AreNotEqual("This should be ignored", transfer.Account.AccountNumber);
            Assert.AreNotEqual(".. ignore", transfer.Account.Id);
            Assert.IsNotNull(transfer.Extension);
            Assert.AreEqual(new DateTime(2022, 5, 22), transfer.Extension.CalculationDate);
            Assert.AreEqual(DistributionType.Year, transfer.Extension.DistributionType);
            Assert.IsTrue(transfer.Extension.HideFromCalculations);
        }

        [Test]
        public void GetPutExistingId_UpdatesIsStored()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\XmlFileWithOneBanktransfer.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
            _ctr = new BankTransferController();
            _ctr.Put(new BankTransfer
            {
                Id = "FakeGUID",
                Account = new Account { AccountNumber = "This should be ignored", Id = ".. ignore" },
                Extension = new BankTransferExtension
                {
                    CalculationDate = new DateTime(2022, 5, 22),
                    DistributionType = DistributionType.Year,
                    HideFromCalculations = true
                }
            });
            var storedFileAsString = FileHelper.ReadFileString(filename);
            System.Diagnostics.Debug.WriteLine(storedFileAsString);
            Assert.IsTrue(storedFileAsString.Contains("<CalculationDate>2022-05-22"));
            Assert.IsTrue(storedFileAsString.Contains("<HideFromCalculations>true</HideFromCalculations>"));
            Assert.IsTrue(storedFileAsString.Contains("<DistributionType>Year</DistributionType>"));
            Assert.IsFalse(storedFileAsString.Contains("This should be ignored"));
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void WhenNewBankTransferIsPosted_NotImplementedExceptionIsThrown()
        {
            _ctr.Post(new BankTransfer());
        }

        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void WhenTyingToDeleteBankTransfer_NotImplementedExceptionIsThrown()
        {
            _ctr.Delete("SomeGUID");
        }
    }
}
