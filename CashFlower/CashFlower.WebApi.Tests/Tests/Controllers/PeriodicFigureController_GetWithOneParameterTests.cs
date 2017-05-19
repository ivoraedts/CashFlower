using CashFlower.Contracts;
using CashFlower.WebApi.Controllers;
using CashFlower.WebApi.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class PeriodicFigureController_GetWithOneParameterTests : ControllerTestBase
    {
        [TestCase(1916)]
        [TestCase(2916)]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenInputOutsideBoundaries_ThrowsException(int year)
        {
            var ctr = new PeriodicFigureController().Get(year);
        }

        [Test]
        public void GivenEmptyStore_GetPeriodicReturnsZeros()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer>());
            var result = ctr.Get(2016);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(0, result.Income);
        }

        [Test]
        public void GivenIncomeFromAnotherYear_GetPeriodicReturnsZeros()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer{Amount = 55, TransactionDate = new DateTime(2017,1,1)}
            });
            var result = ctr.Get(2016);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(0, result.Income);
        }

        [Test]
        public void GivenIncomeFromGivenYear_GetPeriodicReturnsNotZero()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer{Amount = 55, TransactionDate = new DateTime(2016,1,1)}
            });
            var result = ctr.Get(2016);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(55, result.Income);
        }

        [Test]
        public void GivenIncomeFromAnotherYearButShouldBeConsideredAsGivenYearAccordingToExtension_GetPeriodicReturnsNotZero()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer
            {
                Amount = 55,
                TransactionDate = new DateTime(2015, 1, 1),
                Extension = new BankTransferExtension { CalculationDate = new DateTime(2016, 1, 1) }
            }
            });
            var result = ctr.Get(2016);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(55, result.Income);
        }

        [Test]
        public void GivenIncomeFromGivenYearButShouldBeConsideredAsAnotherYearAccordingToExtension_GetPeriodicReturnsZero()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer
            {
                Amount = 55,
                TransactionDate = new DateTime(2016, 1, 1),
                Extension = new BankTransferExtension { CalculationDate = new DateTime(2017, 1, 1) }
            }
            });
            var result = ctr.Get(2016);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(0, result.Income);
        }
    }
}
