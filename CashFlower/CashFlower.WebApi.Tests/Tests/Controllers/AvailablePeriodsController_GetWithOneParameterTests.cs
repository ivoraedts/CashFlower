using CashFlower.WebApi.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using CashFlower.Contracts;
using System;
using CashFlower.WebApi.Tests.Helpers;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class AvailablePeriodsController_GetWithOneParameterTests : ControllerTestBase
    {
        [Test]
        public void GivenNoBanktransfers_GetReturnsCurrentMonth()
        {
            var ctr = new AvailablePeriodsController();
            var result = ctr.Get(DateTime.Now.Year);
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Now.Month, result.First);
            Assert.AreEqual(DateTime.Now.Month, result.Last);
        }

        [Test]
        public void GivenNoBanktransfersInRelatedYear_GetReturnsCurrentMonth()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 11) },
            });
            var result = ctr.Get(2015);
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Now.Month, result.First);
            Assert.AreEqual(DateTime.Now.Month, result.Last);
        }

        [Test]
        public void GivenOneBanktransfersInRelatedYear_GetReturnsRelatedMonth()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 11) },
            });
            var result = ctr.Get(2014);
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.First);
            Assert.AreEqual(11, result.Last);
        }

        [Test]
        public void GivenThreeBanktransfersInRelatedYear_GetReturnsFirstAndLastMonth()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 9, 2) },
                new BankTransfer { TransactionDate = new DateTime (2014, 3, 25) },
                new BankTransfer { TransactionDate = new DateTime (2014, 6, 27) },
            });
            var result = ctr.Get(2014);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.First);
            Assert.AreEqual(9, result.Last);
        }

        [Test]
        public void GivenThreeBanktransfersWithCalculationDateInRelatedYear_GetReturnsFirstAndLastMonthAccordingToCalculationDate()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 9, 2),
                    Extension = new BankTransferExtension { CalculationDate = new DateTime (2015, 3, 3) }
                },
                new BankTransfer { TransactionDate = new DateTime (2014, 9, 2),
                    Extension = new BankTransferExtension { CalculationDate = new DateTime (2015, 1, 13) }
                },
                new BankTransfer { TransactionDate = new DateTime (2014, 9, 2),
                    Extension = new BankTransferExtension { CalculationDate = new DateTime (2015, 12, 31) }
                },
            });
            var result = ctr.Get(2015);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.First);
            Assert.AreEqual(12, result.Last);
        }
    }
}
