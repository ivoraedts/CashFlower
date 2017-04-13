namespace CashFlower.Contracts.Tests.Stubs.IExistingBankTransferDeterminator
{
    public class BankTransferMatcherIndicatingNoTransfersAlreadyExists : Contracts.IExistingBankTransferDeterminator
    {
        public bool Exists(BankTransferLine line)
        {
            return false;
        }
    }
}
