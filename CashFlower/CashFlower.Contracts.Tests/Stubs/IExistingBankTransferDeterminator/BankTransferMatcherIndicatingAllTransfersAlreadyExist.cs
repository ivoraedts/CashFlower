namespace CashFlower.Contracts.Tests.Stubs.IExistingBankTransferDeterminator
{
    public class BankTransferMatcherIndicatingAllTransfersAlreadyExist : Contracts.IExistingBankTransferDeterminator
    {
        public bool Exists(BankTransferLine line)
        {
            return true;
        }
    }
}
