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
    public class SLAValuesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SLAValues
        public ActionResult Index()
        {
            return View(db.SLAValues.ToList());
        }

        // GET: SLAValues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLAValue sLAValue = db.SLAValues.Find(id);
            if (sLAValue == null)
            {
                return HttpNotFound();
            }
            return View(sLAValue);
        }

        // GET: SLAValues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SLAValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SLAValueId,SLAValueName")] SLAValue sLAValue)
        {
            if (ModelState.IsValid)
            {
                db.SLAValues.Add(sLAValue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sLAValue);
        }

        // GET: SLAValues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLAValue sLAValue = db.SLAValues.Find(id);
            if (sLAValue == null)
            {
                return HttpNotFound();
            }
            return View(sLAValue);
        }

        // POST: SLAValues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SLAValueId,SLAValueName")] SLAValue sLAValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sLAValue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sLAValue);
        }

        // GET: SLAValues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SLAValue sLAValue = db.SLAValues.Find(id);
            if (sLAValue == null)
            {
                return HttpNotFound();
            }
            return View(sLAValue);
        }

        // POST: SLAValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SLAValue sLAValue = db.SLAValues.Find(id);
            db.SLAValues.Remove(sLAValue);
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
