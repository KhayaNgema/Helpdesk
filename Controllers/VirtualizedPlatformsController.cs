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
    public class VirtualizedPlatformsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VirtualizedPlatforms
        public ActionResult Index()
        {
            return View(db.VirtualizedPlatforms.ToList());
        }

        // GET: VirtualizedPlatforms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirtualizedPlatform virtualizedPlatform = db.VirtualizedPlatforms.Find(id);
            if (virtualizedPlatform == null)
            {
                return HttpNotFound();
            }
            return View(virtualizedPlatform);
        }

        // GET: VirtualizedPlatforms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VirtualizedPlatforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VirtualizedPlatformId,VirtualizedPlatformName")] VirtualizedPlatform virtualizedPlatform)
        {
            if (ModelState.IsValid)
            {
                db.VirtualizedPlatforms.Add(virtualizedPlatform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(virtualizedPlatform);
        }

        // GET: VirtualizedPlatforms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirtualizedPlatform virtualizedPlatform = db.VirtualizedPlatforms.Find(id);
            if (virtualizedPlatform == null)
            {
                return HttpNotFound();
            }
            return View(virtualizedPlatform);
        }

        // POST: VirtualizedPlatforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VirtualizedPlatformId,VirtualizedPlatformName")] VirtualizedPlatform virtualizedPlatform)
        {
            if (ModelState.IsValid)
            {
                db.Entry(virtualizedPlatform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(virtualizedPlatform);
        }

        // GET: VirtualizedPlatforms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirtualizedPlatform virtualizedPlatform = db.VirtualizedPlatforms.Find(id);
            if (virtualizedPlatform == null)
            {
                return HttpNotFound();
            }
            return View(virtualizedPlatform);
        }

        // POST: VirtualizedPlatforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VirtualizedPlatform virtualizedPlatform = db.VirtualizedPlatforms.Find(id);
            db.VirtualizedPlatforms.Remove(virtualizedPlatform);
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
