using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashFlower.Contracts
{
    public interface IExistingBankTransferDeterminator
    {
        bool Exists(BankTransferLine line);
    }
}
