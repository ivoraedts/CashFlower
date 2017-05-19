using System;
using System.Linq;
using System.Web.Http;
using CashFlower.Contracts;
using CashFlower.Contracts.Helpers;

namespace CashFlower.WebApi.Controllers
{
    public class AvailablePeriodsController : CashFlowerControllerBase
    {
        // GET: api/AvailablePeriods
        // http://localhost:50813/api/AvailablePeriods/
        [HttpGet]
        [Route("api/AvailablePeriods/")]
        public AvailablePeriods Get()
        {
            var banktransfers = _filestorage.GetAll();
            if (!banktransfers.Any())
                return new AvailablePeriods {
                First = DateTime.Now.Year, Last = DateTime.Now.Year
                };
            var first = banktransfers.Min(b => b.GetDate().Year);
            var last = banktransfers.Max(b => b.GetDate().Year);
            return new AvailablePeriods { First = first, Last = last };
        }

        // GET: api/AvailablePeriods/2015
        // http://localhost:50813/api/AvailablePeriods/2017
        [HttpGet]
        [Route("api/AvailablePeriods/{year:int}")]
        public AvailablePeriods Get(int year)
        {
            var banktransfers = _filestorage.GetAll().Where(b=>b.GetDate().Year==year).ToList();
            if (!banktransfers.Any())
                return new AvailablePeriods
                {
                    First = DateTime.Now.Month,
                    Last = DateTime.Now.Month
                };
            var first = banktransfers.Min(b => b.GetDate().Month);
            var last = banktransfers.Max(b => b.GetDate().Month);
            return new AvailablePeriods { First = first, Last = last };
        }

        // GET: api/AvailablePeriods/2016/3
        // http://localhost:50813/api/AvailablePeriods/2017/4
        [HttpGet]
        [Route("api/AvailablePeriods/{year:int}/{month:int}")]
        public AvailablePeriods Get(int year, int month)
        {
            var banktransfers = _filestorage.GetAll()
                .Where(b => b.GetDate().Year == year && b.GetDate().Month == month).ToList();
            if (!banktransfers.Any())
                return new AvailablePeriods
                {
                    First = DateTime.Now.Day,
                    Last = DateTime.Now.Day
                };
            var first = banktransfers.Min(b => b.GetDate().Day);
            var last = banktransfers.Max(b => b.GetDate().Day);
            return new AvailablePeriods { First = first, Last = last };
        }
    }
}
