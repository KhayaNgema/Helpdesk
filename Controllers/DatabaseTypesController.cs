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
    public class DatabaseTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DatabaseTypes
        public ActionResult Index()
        {
            return View(db.DatabaseTypes.ToList());
        }

        // GET: DatabaseTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatabaseType databaseType = db.DatabaseTypes.Find(id);
            if (databaseType == null)
            {
                return HttpNotFound();
            }
            return View(databaseType);
        }

        // GET: DatabaseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DatabaseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DatabaseTypeId,DatabaseName")] DatabaseType databaseType)
        {
            if (ModelState.IsValid)
            {
                db.DatabaseTypes.Add(databaseType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(databaseType);
        }

        // GET: DatabaseTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatabaseType databaseType = db.DatabaseTypes.Find(id);
            if (databaseType == null)
            {
                return HttpNotFound();
            }
            return View(databaseType);
        }

        // POST: DatabaseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DatabaseTypeId,DatabaseName")] DatabaseType databaseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(databaseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(databaseType);
        }

        // GET: DatabaseTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DatabaseType databaseType = db.DatabaseTypes.Find(id);
            if (databaseType == null)
            {
                return HttpNotFound();
            }
            return View(databaseType);
        }

        // POST: DatabaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DatabaseType databaseType = db.DatabaseTypes.Find(id);
            db.DatabaseTypes.Remove(databaseType);
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
