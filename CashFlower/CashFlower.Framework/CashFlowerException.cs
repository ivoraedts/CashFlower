using System;

namespace CashFlower.Framework
{
    public class CashFlowerException : Exception
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public CashFlowerException(string code, string message, params object[] parameters)
            : this(code, string.Format(message, parameters))
        {
        }

        public CashFlowerException(string code, string message)
            :base(code + ": "+message)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }
    }
}
