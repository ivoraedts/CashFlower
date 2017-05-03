using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashFlower.Contracts
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public string Iban { get; set; }
        public string Description { get; set; }
    }
}
