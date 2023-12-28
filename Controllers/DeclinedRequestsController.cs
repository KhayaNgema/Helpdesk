using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;

namespace Helpdesk.Controllers
{
    public class DeclinedRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult DeclinedRequests()
        {
            var userRequests = db.DeclinedRequests
                .ToList();

            return View(userRequests);
        }

        // GET: DeclinedRequests
        public ActionResult Index()
        {
            var declinedRequests = db.DeclinedRequests;
            return View(declinedRequests.ToList());
        }

        // GET: DeclinedRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeclinedRequest declinedRequest = db.DeclinedRequests.Find(id);
            if (declinedRequest == null)
            {
                return HttpNotFound();
            }
            return View(declinedRequest);
        }

        // GET: DeclinedRequests/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View();
        }

        // POST: DeclinedRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OnboardingId,ClientName,OfficeAddress,CountryId,EmailAddress,PostalCode,PeriodFrom,PeriodTo,OpretaionalHoursOpen,OpretationalHourseClose,Status,ClientAbbr")] DeclinedRequest declinedRequest)
        {
            if (ModelState.IsValid)
            {
                db.DeclinedRequests.Add(declinedRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", declinedRequest.CountryId);
            return View(declinedRequest);
        }

        // GET: DeclinedRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeclinedRequest declinedRequest = db.DeclinedRequests.Find(id);
            if (declinedRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", declinedRequest.CountryId);
            return View(declinedRequest);
        }

        // POST: DeclinedRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OnboardingId,ClientName,OfficeAddress,CountryId,EmailAddress,PostalCode,PeriodFrom,PeriodTo,OpretaionalHoursOpen,OpretationalHourseClose,Status,ClientAbbr")] DeclinedRequest declinedRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(declinedRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", declinedRequest.CountryId);
            return View(declinedRequest);
        }

        // GET: DeclinedRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeclinedRequest declinedRequest = db.DeclinedRequests.Find(id);
            if (declinedRequest == null)
            {
                return HttpNotFound();
            }
            return View(declinedRequest);
        }

        // POST: DeclinedRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeclinedRequest declinedRequest = db.DeclinedRequests.Find(id);
            db.DeclinedRequests.Remove(declinedRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
