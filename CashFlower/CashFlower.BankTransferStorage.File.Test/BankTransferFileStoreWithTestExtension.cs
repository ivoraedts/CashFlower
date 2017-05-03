using System.Collections.Generic;
using CashFlower.Contracts;

namespace CashFlower.BankTransferStorage.File.Test
{
    public class BankTransferFileStoreWithTestExtension : BankTransferFileStore
    {
        public void SetStorage(List<BankTransfer> bankTransfers)
        {
            _storage = bankTransfers;
        }
    }
}
