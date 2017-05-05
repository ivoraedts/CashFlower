using CashFlower.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CashFlower.WebApi.Controllers
{
    public class BankTransferController : CashFlowerControllerBase
    {
        // GET: api/BankTransfer
        // Access with: http://localhost:50813/api/BankTransfer
        [HttpGet]
        [Route("api/BankTransfer")]
        public IEnumerable<BankTransfer> Get()
        {
            return _filestorage.GetAll().ToArray();
        }

        // GET: api/BankTransfer/5
        // Access with: http://localhost:50813/api/BankTransfer/FakeGUID
        [HttpGet]
        [Route("api/BankTransfer/{id}")]
        public BankTransfer Get(string id)
        {
            var transfer = _filestorage.GetAll().FirstOrDefault(t => t.Id == id.ToString());
            if (transfer == null)
                throw new KeyNotFoundException("Cannot find BankTransfer with Id:" + id.ToString());
            return transfer;
        }

        // POST: api/BankTransfer
        [HttpPost]
        [Route("api/BankTransfer/{value}")]
        public void Post([FromBody]BankTransfer value)
        {
            throw new NotImplementedException("Posting new BankTransfer objects is not supported by this API.");
        }

        // PUT: api/BankTransfer/5
        [HttpPut]
        [Route("api/BankTransfer/{value}")]
        public void Put([FromBody]BankTransfer value)
        {
            var transfer = _filestorage.GetAll().FirstOrDefault(t => t.Id == value.Id);
            if (transfer == null)
                throw new KeyNotFoundException("Cannot find BankTransfer with Id:" + value.Id);
            transfer.Extension = value.Extension;
            _filestorage.Save();
        }

        // DELETE: api/BankTransfer/5
        [HttpDelete]
        [Route("api/BankTransfer/{value}")]
        public void Delete(string guid)
        {
            throw new NotImplementedException("Deleting BankTransfer objects is not supported by this API.");
        }
    }
}
