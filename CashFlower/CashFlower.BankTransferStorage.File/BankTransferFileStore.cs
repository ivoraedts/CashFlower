using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CashFlower.Contracts;

namespace CashFlower.BankTransferStorage.File
{
    public class BankTransferFileStore : IExistingBankTransferDeterminator, IStoreBankTransfers
    {
        protected List<BankTransfer> _storage = new List<BankTransfer>(); 

        public void OpenFrom(string filename)
        {
            using (var stream = System.IO.File.OpenRead(filename))
            {
                var serializer = new XmlSerializer(_storage.GetType());
                _storage = serializer.Deserialize(stream) as List<BankTransfer>;
            }
        }

        public void SaveAs(string filename)
        {
            using (var writer = new StreamWriter(filename, false))
            {
                var serializer = new XmlSerializer(_storage.GetType());
                serializer.Serialize(writer, _storage);
                writer.Flush();
            }
        }

        public bool Exists(BankTransferLine line)
        {
            return _storage.Any(b=>_accountMatches(b.Account, line) &&
                _contraAccountMatches(b.ContraAccount, line) &&
                b.TransactionDate == line.TransactionDate &&
                b.InterestDate == line.InterestDate &&
                b.InitialBalance == line.InitialBalance &&
                b.FinalBalance== line.FinalBalance);
        }

        private static bool _accountMatches(Account account, BankTransferLine line)
        {
            if (account == null)
                return string.IsNullOrEmpty(line.AccountNumber);

            return account.AccountNumber==line.AccountNumber;
        }

        private static bool _contraAccountMatches(Account account, BankTransferLine line)
        {
            if (account == null)
                return string.IsNullOrEmpty(line.ContraAccountNumber) &&
                    string.IsNullOrEmpty(line.ContraAccountIban) &&
                    string.IsNullOrEmpty(line.ContraAccountDescription);

            return account.AccountNumber == line.ContraAccountNumber &&
                account.Iban == line.ContraAccountIban &&
                account.Description == line.ContraAccountDescription;
        }

        public void Store(BankTransferLine line)
        {
            var account = _detemerineAccountForBankTransfer(line);
            var contraAccount = _detemerineContraAccountForBankTransfer(line);

            _storage.Add(new BankTransfer
            {
                Account = account,
                ContraAccount = contraAccount,
                InterestDate = line.InterestDate,
                TransactionDate = line.TransactionDate,
                InitialBalance = line.InitialBalance,
                FinalBalance = line.FinalBalance,
                Amount = line.Amount
            });
        }

        private Account _detemerineAccountForBankTransfer(BankTransferLine line)
        {
            var bankTransferWithSameAccount =
                _storage.FirstOrDefault(s => s.Account != null && s.Account.AccountNumber == line.AccountNumber);

            var account = (bankTransferWithSameAccount != null)
                ? bankTransferWithSameAccount.Account
                : new Account { AccountNumber = line.AccountNumber };
            return account;
        }

        private Account _detemerineContraAccountForBankTransfer(BankTransferLine line)
        {
            var bankTransferWithSameContraAccount =
                _storage.FirstOrDefault(s => s.ContraAccount != null
                    && s.ContraAccount.AccountNumber == line.ContraAccountNumber
                    && s.ContraAccount.Iban == line.ContraAccountIban
                    && s.ContraAccount.Description == line.ContraAccountDescription);

            var contraAccount = (bankTransferWithSameContraAccount != null)
                ? bankTransferWithSameContraAccount.ContraAccount
                : new Account {
                    AccountNumber = line.ContraAccountNumber,
                    Description = line.ContraAccountDescription,
                    Iban = line.ContraAccountIban
                };
            return contraAccount;
        }

        public List<BankTransfer> GetAll()
        {
            return _storage;
        }
    }
}
