using CashFlower.Contracts;
using CashFlower.WebApi.Controllers;
using CashFlower.WebApi.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class PeriodicFigureController_GetWithTwoParametersTests : ControllerTestBase
    {
        [TestCase(1916,2)]
        [TestCase(2916,2)]
        [TestCase(2016, -2)]
        [TestCase(2016, 13)]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenInputOutsideBoundaries_ThrowsException(int year, int month)
        {
            var ctr = new PeriodicFigureController().Get(year, month);
        }

        [Test]
        public void GivenEmptyStore_GetPeriodicReturnsZeros()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer>());
            var result = ctr.Get(2016, 3);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(0, result.Income);
        }

        [Test]
        public void GivenIncomeFromAnotherMonth_GetPeriodicReturnsZeros()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer{Amount = 2055, TransactionDate = new DateTime(2016,4,1)}
            });
            var result = ctr.Get(2016, 3);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(0, result.Income);
        }

        [Test]
        public void GivenIncomeFromGivenYearAndMonth_GetPeriodicReturnsNotZero()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer{Amount = 55, TransactionDate = new DateTime(2016,3,1)}
            });
            var result = ctr.Get(2016, 3);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(55, result.Income);
        }

        [Test]
        public void GivenIncomeFromAnotherYearMonthButShouldBeConsideredAsGivenYearMonthAccordingToExtension_GetPeriodicReturnsNotZero()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer
            {
                Amount = 55,
                TransactionDate = new DateTime(2015, 12, 1),
                Extension = new BankTransferExtension { CalculationDate = new DateTime(2016, 1, 1) }
            }
            });
            var result = ctr.Get(2016, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(55, result.Income);
        }

        [Test]
        public void GivenIncomeSameMonthButDistributedOverYear_GetPeriodicReturns_1_dividedBy_12thPart()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer
            {
                Amount = 1200,
                TransactionDate = new DateTime(2015, 12, 1),
                Extension = new BankTransferExtension { CalculationDate = new DateTime(2016, 1, 1),
                DistributionType = DistributionType.Year}
            }
            });
            var result = ctr.Get(2016, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(100, result.Income);
        }

        [Test]
        public void GivenIncomeSameYearButOtherMonthButDistributedOverYear_GetPeriodicReturns_1_dividedBy_12thPart()
        {
            var ctr = new PeriodicFigureControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer
            {
                Amount = 1200,
                TransactionDate = new DateTime(2015, 12, 1),
                Extension = new BankTransferExtension { CalculationDate = new DateTime(2016, 1, 1),
                DistributionType = DistributionType.Year}
            }
            });
            var result = ctr.Get(2016, 5);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Expenses);
            Assert.AreEqual(100, result.Income);
        }
    }
}
