using System;
using System.Collections.Generic;
using System.IO;

namespace CashFlower.Contracts.Tests.Stubs.IBankTransferReader
{
    public class CrashingBankTransferReader : Contracts.IBankTransferReader
    {
        public List<BankTransferLine> GetBankTransfers()
        {
            throw new FileNotFoundException("Oh no! I am crashed!");
        }
    }
}
