namespace CashFlower.Contracts
{
    public interface IExistingBankTransferDeterminator
    {
        bool Exists(BankTransferLine line);
    }
}
