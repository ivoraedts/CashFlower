using System.Collections.Generic;

namespace CashFlower.Contracts.Tests.Stubs.IBankTransferReader
{
    public class ReaderThatReturnsPreSetList : Contracts.IBankTransferReader
    {
        private readonly List<BankTransferLine> _bankTransferLines;
        public ReaderThatReturnsPreSetList(List<BankTransferLine> bankTransferLines)
        {
            _bankTransferLines = bankTransferLines;
        }

        public List<BankTransferLine> GetBankTransfers()
        {
            return _bankTransferLines;
        }
    }
}
