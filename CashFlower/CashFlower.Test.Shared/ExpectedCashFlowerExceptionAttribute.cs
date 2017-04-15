using System;
using NUnit.Framework;

namespace CashFlower.Test.Shared
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExpectedCashFlowerExceptionAttribute : ExpectedExceptionAttribute
    {
        public ExpectedCashFlowerExceptionAttribute(string code)
        {
            MatchType       = MessageMatch.StartsWith;
            ExpectedMessage = code + ":";
        }
    }
}
