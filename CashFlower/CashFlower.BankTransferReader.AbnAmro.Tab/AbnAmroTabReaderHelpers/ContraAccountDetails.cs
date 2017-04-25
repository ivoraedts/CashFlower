using System;

namespace CashFlower.BankTransferReader.AbnAmro.Tab.AbnAmroTabReaderHelpers
{
    public class ContraAccountDetails
    {
        public DateTime? DateTimeStamp { get; set; }
        public string ContraAccountName { get; set; }
        public string ContraAccountIban { get; set; }
    }
}
