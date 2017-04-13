using System.Collections.Generic;

namespace CashFlower.Contracts.Tests.Stubs.IBankTransferReader
{
    public class ReaderThatReturnsEmptyList : Contracts.IBankTransferReader
    {
        public List<BankTransferLine> GetBankTransfers()
        {
            return new List<BankTransferLine>();
        }
    }
}
