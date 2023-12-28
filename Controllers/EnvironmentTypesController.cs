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
    public class EnvironmentTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EnvironmentTypes
        public ActionResult Index()
        {
            return View(db.EnvironmentTypes.ToList());
        }

        // GET: EnvironmentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvironmentType environmentType = db.EnvironmentTypes.Find(id);
            if (environmentType == null)
            {
                return HttpNotFound();
            }
            return View(environmentType);
        }

        // GET: EnvironmentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnvironmentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnvironmentTypeId,EnvironmentName")] EnvironmentType environmentType)
        {
            if (ModelState.IsValid)
            {
                db.EnvironmentTypes.Add(environmentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(environmentType);
        }

        // GET: EnvironmentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvironmentType environmentType = db.EnvironmentTypes.Find(id);
            if (environmentType == null)
            {
                return HttpNotFound();
            }
            return View(environmentType);
        }

        // POST: EnvironmentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnvironmentTypeId,EnvironmentName")] EnvironmentType environmentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(environmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(environmentType);
        }

        // GET: EnvironmentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvironmentType environmentType = db.EnvironmentTypes.Find(id);
            if (environmentType == null)
            {
                return HttpNotFound();
            }
            return View(environmentType);
        }

        // POST: EnvironmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnvironmentType environmentType = db.EnvironmentTypes.Find(id);
            db.EnvironmentTypes.Remove(environmentType);
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
