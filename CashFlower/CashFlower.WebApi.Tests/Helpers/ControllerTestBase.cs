using NUnit.Framework;
using System.IO;
using System.Configuration;

namespace CashFlower.WebApi.Tests.Helpers
{
    public abstract class ControllerTestBase
    {
        [SetUp]
        public void Setup()
        {
            var filename = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Resources\EmptyXmlFile.xml");
            ConfigurationManager.AppSettings["storagefilename"] = filename;
        }
    }
}
