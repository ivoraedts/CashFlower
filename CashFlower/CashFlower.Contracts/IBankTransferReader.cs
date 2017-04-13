using System.Collections.Generic;

namespace CashFlower.Contracts
{
    public interface IBankTransferReader
    {
        List<BankTransferLine> GetBankTransfers();
    }
}
