using CashFlower.Contracts.Helpers;
using NUnit.Framework;
using System;

namespace CashFlower.Contracts.Tests.Tests.Helpers
{
    [TestFixture]
    public class BankTransferHelperTests
    {
        [Test]
        public void GivenBankTransferWithoutExtension_GetDateReturnsTransactionDate()
        {
            var banktransfer = new BankTransfer { TransactionDate = new DateTime(2012,4,4) };
            var result = banktransfer.GetDate();
            Assert.AreEqual(new DateTime(2012, 4, 4), result);
        }

        [Test]
        public void GivenBankTransferWithExtensionWithoutCalculationDate_GetDateReturnsTransactionDate()
        {
            var banktransfer = new BankTransfer {
                TransactionDate = new DateTime(2012, 4, 4),
                Extension = new BankTransferExtension { }
            };
            var result = banktransfer.GetDate();
            Assert.AreEqual(new DateTime(2012, 4, 4), result);
        }

        [Test]
        public void GivenBankTransferWithExtensionAndCalculationdate_GetDateReturnsCalculationDate()
        {
            var banktransfer = new BankTransfer
            {
                TransactionDate = new DateTime(2012, 4, 4),
                Extension = new BankTransferExtension { CalculationDate = new DateTime(2013, 3, 3) }
            };
            var result = banktransfer.GetDate();
            Assert.AreEqual(new DateTime(2013, 3, 3), result);
        }

        [Test]
        public void GivenBankTransferWithoutExtension_GetDistributionTypeReturnsNone()
        {
            var banktransfer = new BankTransfer { };
            var result = banktransfer.GetDistributionType();
            Assert.AreEqual(DistributionType.None, result);
        }

        [Test]
        public void GivenBankTransferWithExtensionWithoutSpecifiedDistributionType_GetDistributionTypeReturnsNone()
        {
            var banktransfer = new BankTransfer { Extension = new BankTransferExtension { } };
            var result = banktransfer.GetDistributionType();
            Assert.AreEqual(DistributionType.None, result);
        }

        [Test]
        public void GivenBankTransferWithExtensionWithSpecifiedDistributionType_GetDistributionTypeReturnsGivenType()
        {
            var allDistributionTypes = (DistributionType[])Enum.GetValues(typeof(DistributionType));
            foreach(var distributionType in allDistributionTypes)
            {
                var banktransfer = new BankTransfer { Extension = new BankTransferExtension { DistributionType = distributionType } };
                var result = banktransfer.GetDistributionType();
                Assert.AreEqual(distributionType, result);
            }
        }

    }
}
