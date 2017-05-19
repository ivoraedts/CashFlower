using CashFlower.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CashFlower.WebApplication.Controllers
{
    public class PeriodicFigureController : Controller
    {
        // GET: PeriodicFigure
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();

            var response = await client.GetAsync("http://localhost:50813/api/PeriodicFigure");
            if (!response.IsSuccessStatusCode) return HttpNotFound();
            var figure = JsonConvert.DeserializeObject<PeriodicFigure>(await response.Content.ReadAsStringAsync());

            var response2 = await client.GetAsync("http://localhost:50813/api/AvailablePeriods/");
            if (!response2.IsSuccessStatusCode)
            {
                ViewData["FirstPeriod"] = 0;
                ViewData["LastPeriod"] = 0;
            }
            else
            {
                var availablePeriods = JsonConvert.DeserializeObject<AvailablePeriods>(await response2.Content.ReadAsStringAsync());
                {
                    ViewData["FirstPeriod"] = availablePeriods.First;
                    ViewData["LastPeriod"] = availablePeriods.Last;
                }
            }
            return View(figure);
        }

        // GET: PeriodicFigure/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PeriodicFigure/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PeriodicFigure/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PeriodicFigure/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PeriodicFigure/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PeriodicFigure/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PeriodicFigure/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
