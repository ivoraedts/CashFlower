using CashFlower.WebApi.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using CashFlower.Contracts;
using System;
using CashFlower.WebApi.Tests.Helpers;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    [TestFixture]
    public class AvailablePeriodsController_GetWithoutParametersTests : ControllerTestBase
    {
        [Test]
        public void GivenNoBanktransfers_GetReturnsCurrentYear()
        {
            var ctr = new AvailablePeriodsController();
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Now.Year, result.First);
            Assert.AreEqual(DateTime.Now.Year, result.Last);
        }

        [Test]
        public void GivenOneBanktransfer_GetReturnsRelatedYearAsFirstAndLast()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2014, 11, 11) },
            });
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(2014, result.First);
            Assert.AreEqual(2014, result.Last);
        }

        [Test]
        public void GivenStoreWithTwoBankTransfersWithoutExtension_GetReturnsLowestAndHighestTransactionDate()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer { TransactionDate = new DateTime (2012, 5, 5) },
                new BankTransfer { TransactionDate = new DateTime (2017, 5, 5) },
            });
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(2012, result.First);
            Assert.AreEqual(2017, result.Last);
        }

        [Test]
        public void GivenStoreWithTwoBankTransfersExtensionWithDate_GetReturnsLowestAndHighestTransactionDate()
        {
            var ctr = new AvailablePeriodsControllerWithAccessToStore();
            ctr.SetStorage(new List<BankTransfer> {
                new BankTransfer {
                    TransactionDate = new DateTime (2012, 5, 5),
                    Extension = new BankTransferExtension
                    {
                        CalculationDate = new DateTime(2011, 4, 4)
                    }
                },
                new BankTransfer {
                    TransactionDate = new DateTime (2017, 5, 5),
                    Extension = new BankTransferExtension
                    {
                        CalculationDate = new DateTime(2016, 4, 4)
                    }
                }
            });
            var result = ctr.Get();
            Assert.IsNotNull(result);
            Assert.AreEqual(2011, result.First);
            Assert.AreEqual(2016, result.Last);
        }
    }
}
