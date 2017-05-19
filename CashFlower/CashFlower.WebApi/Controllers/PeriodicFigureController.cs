using CashFlower.Contracts;
using CashFlower.Contracts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CashFlower.WebApi.Controllers
{
    public class PeriodicFigureController : CashFlowerControllerBase
    {
        // GET: api/PeriodicFigure
        // http://localhost:50813/api/PeriodicFigure
        [HttpGet]
        [Route("api/PeriodicFigure/")]
        public PeriodicFigure Get()
        {
            var banktransfers = _filestorage.GetAll();
            var expenses = _getExpenses(banktransfers);
            var income = _getIncome(banktransfers);
            return new PeriodicFigure
            {
                Expenses = expenses.Sum(e => e.Amount * -1),
                Income = income.Sum(e => e.Amount)
            };
        }

        private List<BankTransfer> _getExpenses(List<BankTransfer> all)
        {
            return all.Where(b => b.Amount < 0 && (b.Extension == null || !b.Extension.HideFromCalculations)).ToList();
        }

        private List<BankTransfer> _getIncome(List<BankTransfer> all)
        {
            return all.Where(b => b.Amount > 0 && (b.Extension == null || !b.Extension.HideFromCalculations)).ToList();
        }

        // GET: api/PeriodicFigure/5
        // http://localhost:50813/api/PeriodicFigure/2016
        [HttpGet]
        [Route("api/PeriodicFigure/{year:int}")]
        public PeriodicFigure Get(int year)
        {
            _validateYear(year);
            var banktransfers = _filestorage.GetAll();
            var expenses = _filterByYear(_getExpenses(banktransfers),year);
            var income = _filterByYear(_getIncome(banktransfers),year);
            return new PeriodicFigure
            {
                Expenses = expenses.Sum(e => e.Amount * -1),
                Income = income.Sum(e => e.Amount)
            };
        }

        // GET: api/PeriodicFigure/2016/11
        // http://localhost:50813/api/PeriodicFigure/2017/4
        [HttpGet]
        [Route("api/PeriodicFigure/{year:int}/{month:int}")]
        public PeriodicFigure Get(int year, int month)
        {
            _validateYear(year);
            _validateMonth(month);
            var banktransfers = _filestorage.GetAll();
            var expenses = _filterByYearAndMonth(_getExpenses(banktransfers), year, month);
            var income = _filterByYearAndMonth(_getIncome(banktransfers), year, month);
            return new PeriodicFigure
            {
                Expenses = expenses.Sum(e => 
                (e.Extension!=null && e.Extension.DistributionType== DistributionType.Year) ?
                (e.Amount / 12) * -1 : e.Amount * -1),
                Income = income.Sum(e =>
                (e.Extension != null && e.Extension.DistributionType == DistributionType.Year) ?
                (e.Amount / 12): e.Amount)
            };
        }

        private static void _validateYear(int year)
        {
            if (year < 1950)
                throw new ArgumentException("Before 1950 no BankTransactions were available");

            if (year > 2350)
                throw new ArgumentException("Earth will be gone after 2350");
        }

        private static void _validateMonth(int month)
        {
            if ((month < 1) || (month > 12))
                throw new ArgumentException("Give an existing month not smaller than one and not bigger than twelve, please.");
        }

        private List<BankTransfer> _filterByYear(List<BankTransfer> all, int year)
        {
            return all.Where(b => b.GetDate().Year==year).ToList();
        }

        private List<BankTransfer> _filterByYearAndMonth(List<BankTransfer> all, int year, int month)
        {
            return all.Where(b =>
                ( b.GetDate().Year==year && b.GetDistributionType() == DistributionType.Year) 
                || ( b.GetDate().Year==year && b.GetDate().Month==month)
                ).ToList();
        }

    }
}
