using CashFlower.BankTransferStorage.File;
using CashFlower.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;

namespace CashFlower.WebApi.Controllers
{
    public class BankTransferController : ApiController
    {
        private readonly BankTransferFileStore _filestorage;
        public BankTransferController()
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

        // GET: api/BankTransfer
        // Access with: http://localhost:50813/api/BankTransfer
        public IEnumerable<BankTransfer> Get()
        {
            return _filestorage.GetAll().ToArray();
        }

        /*
        // GET: api/BankTransfer/5
        public BankTransfer Get(int id)
        {
            return new BankTransfer
            {
                Account = new Account
                {
                    AccountNumber = "Just testing around"
                }
            };
        }
        I first need an ID for this
        */

        // POST: api/BankTransfer
        public void Post([FromBody]BankTransfer value)
        {
            throw new NotImplementedException("Posting new BankTransfer objects is not supported by this API.");
        }

        /*
        // PUT: api/BankTransfer/5
        public void Put(int id, [FromBody]string value)
        {
        }
        I first need an ID for this
            */

        // DELETE: api/BankTransfer/5
        public void Delete(string guid)
        {
            throw new NotImplementedException("Deleting BankTransfer objects is not supported by this API.");
        }
    }
}
