using System;

namespace CashFlower.Contracts
{
    public class BankTransfer
    {
        public Account Account { get; set; }
        public Account ContraAccount { get; set; }
        public BankTransferExtension Extension { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime InterestDate { get; set; }
        public decimal Amount { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }

    }
}
