using CashFlower.BankTransferStorage.File;
using CashFlower.Contracts;
using CashFlower.WebApi.Controllers;
using System.Collections.Generic;

namespace CashFlower.WebApi.Tests.Tests.Controllers
{
    public class AvailablePeriodsControllerWithAccessToStore : AvailablePeriodsController
    {
        private BankTransferFileStoreWithTestExtension _testStore;

        public AvailablePeriodsControllerWithAccessToStore()
        {
            _testStore = new BankTransferFileStoreWithTestExtension();
            _filestorage = _testStore;
        }

        public void SetStorage(List<BankTransfer> bankTransfers)
        {
            _testStore.SetStorage(bankTransfers);
        }

        private class BankTransferFileStoreWithTestExtension : BankTransferFileStore
        {
            public void SetStorage(List<BankTransfer> bankTransfers)
            {
                _storage = bankTransfers;
            }
        }
    }
}
