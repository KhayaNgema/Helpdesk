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
    public class HardwareDescriptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HardwareDescriptions
        public ActionResult Index()
        {
            return View(db.HardwareDescriptions.ToList());
        }

        // GET: HardwareDescriptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HardwareDescription hardwareDescription = db.HardwareDescriptions.Find(id);
            if (hardwareDescription == null)
            {
                return HttpNotFound();
            }
            return View(hardwareDescription);
        }

        // GET: HardwareDescriptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HardwareDescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HardwareDescriptionId,HardwareDescriptionName")] HardwareDescription hardwareDescription)
        {
            if (ModelState.IsValid)
            {
                db.HardwareDescriptions.Add(hardwareDescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hardwareDescription);
        }

        // GET: HardwareDescriptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HardwareDescription hardwareDescription = db.HardwareDescriptions.Find(id);
            if (hardwareDescription == null)
            {
                return HttpNotFound();
            }
            return View(hardwareDescription);
        }

        // POST: HardwareDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HardwareDescriptionId,HardwareDescriptionName")] HardwareDescription hardwareDescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hardwareDescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hardwareDescription);
        }

        // GET: HardwareDescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HardwareDescription hardwareDescription = db.HardwareDescriptions.Find(id);
            if (hardwareDescription == null)
            {
                return HttpNotFound();
            }
            return View(hardwareDescription);
        }

        // POST: HardwareDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HardwareDescription hardwareDescription = db.HardwareDescriptions.Find(id);
            db.HardwareDescriptions.Remove(hardwareDescription);
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
