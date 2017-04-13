using System;
using System.Collections.Generic;
using System.IO;
using CashFlower.Contracts;

namespace CashFlower.BankTransferReader.AbnAmro.Tab
{
    public class AbnAmroTabReader: IBankTransferReader
    {
        private readonly string _fullFilename ;
        public AbnAmroTabReader(string fullFilename)
        {
            _fullFilename = fullFilename;
        }

        public List<BankTransferLine> GetBankTransfers()
        {
            if (!File.Exists(_fullFilename))
                throw new FileNotFoundException(_fullFilename);

            var result = new List<BankTransferLine>();
            using (var file = new StreamReader(_fullFilename))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    throw new NotImplementedException();
                }
            }
            return result;
        }
    }
}
