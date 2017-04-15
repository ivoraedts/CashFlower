using NUnit.Framework;

namespace CashFlower.Framework.Tests
{
    [TestFixture]
    public class CashFlowerExceptionTests
    {
        private const string SomeCode = "MyCode";
        private const string SomeMessage = "MyMessage";
        private const string SomeMessageWithArguments = "Argument 1 is'{0}' and Argument 2 is '{1}'";
        private const string Argument1 = "This Is Argument Number One";
        private const int Argument2 = 223;

        [Test]
        public void GivenCreatedCashFlowerException_ErrorCodeAndErrorMessageAndInheritedMessageAreSet()
        {
            var cashFlowerException = new CashFlowerException(SomeCode, SomeMessage);
            Assert.AreEqual(SomeCode, cashFlowerException.ErrorCode);
            Assert.AreEqual(SomeMessage, cashFlowerException.ErrorMessage);
            Assert.AreEqual(SomeCode + ": " + SomeMessage, cashFlowerException.Message);
        }

        [Test]
        public void GivenCreatedFlowerExceptionWithMessageArguments_ArgumentsAreStoredInMessage()
        {
            var cashFlowerException = new CashFlowerException(SomeCode, SomeMessageWithArguments, Argument1, Argument2);
            Assert.AreEqual(SomeCode, cashFlowerException.ErrorCode);
            Assert.IsTrue(cashFlowerException.ErrorMessage.Contains(Argument1));
            Assert.IsTrue(cashFlowerException.ErrorMessage.Contains(Argument2.ToString()));
            Assert.IsTrue(cashFlowerException.Message.Contains(Argument1));
            Assert.IsTrue(cashFlowerException.Message.Contains(Argument2.ToString()));
        }
    }
}
