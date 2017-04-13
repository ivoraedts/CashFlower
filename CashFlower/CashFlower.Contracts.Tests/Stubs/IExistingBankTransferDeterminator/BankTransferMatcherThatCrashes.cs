using System;

namespace CashFlower.Contracts.Tests.Stubs.IExistingBankTransferDeterminator
{
    public class BankTransferMatcherThatCrashes : Contracts.IExistingBankTransferDeterminator
    {
        public bool Exists(BankTransferLine line)
        {
            throw new Exception("Oh no! I am crashed while trying to determine if banktransfer exists!");
        }
    }
}
