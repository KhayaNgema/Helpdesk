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
    public class ActiveManagersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ActiveManagers
        public ActionResult Index()
        {
            var activeManagers = db.ActiveManagers.Include(a => a.ApprovedRequest).Include(a => a.Category).Include(a => a.DatabaseType).Include(a => a.Designation).Include(a => a.EnvironmentType).Include(a => a.HardwareDescription).Include(a => a.Products).Include(a => a.SubCategories).Include(a => a.VirtualizedPlatforms);
            return View(activeManagers.ToList());
        }

        // GET: ActiveManagers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActiveManager activeManager = db.ActiveManagers.Find(id);
            if (activeManager == null)
            {
                return HttpNotFound();
            }
            return View(activeManager);
        }

        // GET: ActiveManagers/Create
        public ActionResult Create()
        {
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName");
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName");
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName");
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode");
             ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName");
            return View();
        }

        // POST: ActiveManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] ActiveManager activeManager)
        {
            if (ModelState.IsValid)
            {
                db.ActiveManagers.Add(activeManager);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", activeManager.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", activeManager.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", activeManager.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", activeManager.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", activeManager.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", activeManager.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", activeManager.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", activeManager.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", activeManager.VirtualizedPlatformId);
            return View(activeManager);
        }

        // GET: ActiveManagers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActiveManager activeManager = db.ActiveManagers.Find(id);
            if (activeManager == null)
            {
                return HttpNotFound();
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", activeManager.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", activeManager.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", activeManager.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", activeManager.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", activeManager.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", activeManager.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", activeManager.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", activeManager.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", activeManager.VirtualizedPlatformId);
            return View(activeManager);
        }

        // POST: ActiveManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] ActiveManager activeManager)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activeManager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", activeManager.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", activeManager.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", activeManager.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", activeManager.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", activeManager.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", activeManager.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", activeManager.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", activeManager.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", activeManager.VirtualizedPlatformId);
            return View(activeManager);
        }

        // GET: ActiveManagers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActiveManager activeManager = db.ActiveManagers.Find(id);
            if (activeManager == null)
            {
                return HttpNotFound();
            }
            return View(activeManager);
        }

        // POST: ActiveManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActiveManager activeManager = db.ActiveManagers.Find(id);
            db.ActiveManagers.Remove(activeManager);
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
