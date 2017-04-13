using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CashFlower.Contracts;

namespace CashFlower.Importer
{
    public class BankTransferImporter
    {
        private readonly IBankTransferReader _reader;
        private readonly IExistingBankTransferDeterminator _matcher;

        public BankTransferImporter(IBankTransferReader reader, IExistingBankTransferDeterminator matcher)
        {
            _reader = reader;
            _matcher = matcher;
        }

        public void Execute()
        {
            var bankTransferLines = _reader.GetBankTransfers();
            foreach (var bankTransferLine in bankTransferLines)
            {
                if (!_matcher.Exists(bankTransferLine))
                {
                    
                };
            }
        }
    }
}
