using System;

namespace CashFlower.Contracts
{
    public class BankTransferLine
    {
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime InterestDate { get; set; }
        public decimal Amount { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public string ContraAccountNumber { get; set; }
        public string ContraAccountIban { get; set; }
        public string ContraAccountDescription { get; set; }
    }
}
