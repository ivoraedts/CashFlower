using CashFlower.WebApi.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using CashFlower.Contracts;
using System;
using CashFlower.WebApi.Tests.Helpers;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class AvailablePeriodsController_GetWithTwoParametersTests : ControllerTestBase
    {
        [Test]
        public void GivenNoBanktransfers_GetReturnsCurrentDay()
        {
            var ctr = new AvailablePeriodsController();
            var result = ctr.Get(DateTime.Now.Year, DateTime.Now.Month);
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Now.Day, result.First);
            Assert.AreEqual(DateTime.Now.Day, result.Last);
        }

        [Test]
        public void GivenNoBanktransfersInRelatedPeriod_GetReturnsCurrentDay()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 11) },
            });
            var result = ctr.Get(2012, 10);
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Now.Day, result.First);
            Assert.AreEqual(DateTime.Now.Day, result.Last);
        }

        [Test]
        public void GivenOneBanktransfersInRelatedPeriod_GetReturnsRelatedDay()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 11) },
            });
            var result = ctr.Get(2014, 11);
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.First);
            Assert.AreEqual(11, result.Last);
        }

        [Test]
        public void GivenFiveBanktransfersInRelatedPeriod_GetReturnsFirstAndLastDay()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 7) },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 6) },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 17) },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 20) },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 11) },
            });
            var result = ctr.Get(2014, 11);
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.First);
            Assert.AreEqual(20, result.Last);
        }

        [Test]
        public void GivenFiveBanktransfersIncludingSomeWithCalculationDates_GetReturnsFirstAndLastDay()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    TransactionDate = new DateTime (2014, 11, 7),
                    Extension = new BankTransferExtension{CalculationDate = new DateTime(2014, 11, 4)}
                },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 6) },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 17) },
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 20) },
                new BankTransfer {
                    TransactionDate = new DateTime (2014, 11, 11),
                    Extension = new BankTransferExtension{CalculationDate = new DateTime(2014, 11, 24)}
                },
            });
            var result = ctr.Get(2014, 11);
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.First);
            Assert.AreEqual(24, result.Last);
        }
    }
}
