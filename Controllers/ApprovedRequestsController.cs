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
    public class ApprovedRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ApprovedRequests()
        {
            var userRequests = db.ApprovedRequests
                .ToList();

            return View(userRequests);
        }

        // GET: ApprovedRequests
        public ActionResult Index()
        {
            var approvedRequests = db.ApprovedRequests;
            return View(approvedRequests.ToList());
        }

        // GET: ApprovedRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedRequest approvedRequest = db.ApprovedRequests.Find(id);
            if (approvedRequest == null)
            {
                return HttpNotFound();
            }
            return View(approvedRequest);
        }

        // GET: ApprovedRequests/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName");
            return View();
        }

        // POST: ApprovedRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OnboardingId,ClientName,OfficeAddress,CountryId,EmailAddress,PostalCode,PeriodFrom,PeriodTo,OpretaionalHoursOpen,OpretationalHourseClose,Status,ClientAbbr")] ApprovedRequest approvedRequest)
        {
            if (ModelState.IsValid)
            {
                db.ApprovedRequests.Add(approvedRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", approvedRequest.CountryId);
            return View(approvedRequest);
        }

        // GET: ApprovedRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedRequest approvedRequest = db.ApprovedRequests.Find(id);
            if (approvedRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", approvedRequest.CountryId);
            return View(approvedRequest);
        }

        // POST: ApprovedRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OnboardingId,ClientName,OfficeAddress,CountryId,EmailAddress,PostalCode,PeriodFrom,PeriodTo,OpretaionalHoursOpen,OpretationalHourseClose,Status,ClientAbbr")] ApprovedRequest approvedRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(approvedRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", approvedRequest.CountryId);
            return View(approvedRequest);
        }

        // GET: ApprovedRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovedRequest approvedRequest = db.ApprovedRequests.Find(id);
            if (approvedRequest == null)
            {
                return HttpNotFound();
            }
            return View(approvedRequest);
        }

        // POST: ApprovedRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApprovedRequest approvedRequest = db.ApprovedRequests.Find(id);
            db.ApprovedRequests.Remove(approvedRequest);
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
