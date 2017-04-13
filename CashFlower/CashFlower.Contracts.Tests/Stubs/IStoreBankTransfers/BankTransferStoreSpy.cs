using System.Collections.Generic;

namespace CashFlower.Contracts.Tests.Stubs.IStoreBankTransfers
{
    public class BankTransferStoreSpy : Contracts.IStoreBankTransfers
    {
        public List<BankTransferLine> ProcessedRequests { get; set; }

        public BankTransferStoreSpy()
        {
            ProcessedRequests = new List<BankTransferLine>();
        }

        public void Store(BankTransferLine line)
        {
            ProcessedRequests.Add(line);
        }
    }
}
