using System;

namespace CashFlower.Contracts
{
    public class BankTransferExtension
    {
        public DateTime? CalculationDate { get; set; }
        public bool HideFromCalculations { get; set; }
        public DistributionType DistributionType { get; set; }
    }
}
