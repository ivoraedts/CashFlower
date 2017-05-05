using CashFlower.BankTransferStorage.File;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;

namespace CashFlower.WebApi.Controllers
{
    public class CashFlowerControllerBase : ApiController
    {
        protected BankTransferFileStore _filestorage;
        public CashFlowerControllerBase()
        {
            _filestorage = new BankTransferFileStore();
            _filestorage.OpenFrom(_getStorageFile());
        }

        [ExcludeFromCodeCoverage]
        private string _getStorageFile()
        {
            string storagefilename = ConfigurationManager.AppSettings["storagefilename"];
            return File.Exists(storagefilename) ? storagefilename : HostingEnvironment.MapPath(@"~/App_Data/" + storagefilename);
        }
    }
}