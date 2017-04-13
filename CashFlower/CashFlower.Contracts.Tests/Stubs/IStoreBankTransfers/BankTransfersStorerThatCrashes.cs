using System;

namespace CashFlower.Contracts.Tests.Stubs.IStoreBankTransfers
{
    public class BankTransfersStorerThatCrashes : Contracts.IStoreBankTransfers
    {
        public void Store(BankTransferLine line)
        {
            throw new Exception("I just crashed while storing a bank transfer");
        }
    }
}
