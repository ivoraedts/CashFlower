using System;

namespace CashFlower.Contracts.Helpers
{
    public static class BankTransferHelper
    {
        public static DateTime GetDate(this BankTransfer banktransfer)
        {
            if (banktransfer.Extension == null || banktransfer.Extension.CalculationDate == null)
                return banktransfer.TransactionDate;
            return banktransfer.Extension.CalculationDate.Value;
        }

        public static DistributionType GetDistributionType(this BankTransfer banktransfer)
        {
            if (banktransfer.Extension == null)
                return DistributionType.None;
            return banktransfer.Extension.DistributionType;
        }
    }
}
