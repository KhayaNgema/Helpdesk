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
    public class UnresolvedIncidentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UnresolvedIncidents
        public ActionResult Index()
        {
            var incidents = db.UnresolvedIncidents.Include(u => u.ApprovedRequest).Include(u => u.Category).Include(u => u.DatabaseType).Include(u => u.Designation).Include(u => u.EnvironmentType).Include(u => u.HardwareDescription).Include(u => u.Products).Include(u => u.SubCategories).Include(u => u.VirtualizedPlatforms);
            return View(incidents.ToList());
        }

        // GET: UnresolvedIncidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnresolvedIncident unresolvedIncident = db.UnresolvedIncidents.Find(id);
            if (unresolvedIncident == null)
            {
                return HttpNotFound();
            }
            return View(unresolvedIncident);
        }

        // GET: UnresolvedIncidents/Create
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

        // POST: UnresolvedIncidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] UnresolvedIncident unresolvedIncident)
        {
            if (ModelState.IsValid)
            {
                db.Incidents.Add(unresolvedIncident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", unresolvedIncident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", unresolvedIncident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", unresolvedIncident.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", unresolvedIncident.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", unresolvedIncident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", unresolvedIncident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", unresolvedIncident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", unresolvedIncident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", unresolvedIncident.VirtualizedPlatformId);
            return View(unresolvedIncident);
        }

        // GET: UnresolvedIncidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnresolvedIncident unresolvedIncident = db.UnresolvedIncidents.Find(id);
            if (unresolvedIncident == null)
            {
                return HttpNotFound();
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", unresolvedIncident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", unresolvedIncident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", unresolvedIncident.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", unresolvedIncident.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", unresolvedIncident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", unresolvedIncident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", unresolvedIncident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", unresolvedIncident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", unresolvedIncident.VirtualizedPlatformId);
            return View(unresolvedIncident);
        }

        // POST: UnresolvedIncidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] UnresolvedIncident unresolvedIncident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unresolvedIncident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", unresolvedIncident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", unresolvedIncident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", unresolvedIncident.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", unresolvedIncident.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", unresolvedIncident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", unresolvedIncident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", unresolvedIncident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", unresolvedIncident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", unresolvedIncident.VirtualizedPlatformId);
            return View(unresolvedIncident);
        }

        // GET: UnresolvedIncidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnresolvedIncident unresolvedIncident = db.UnresolvedIncidents.Find(id);
            if (unresolvedIncident == null)
            {
                return HttpNotFound();
            }
            return View(unresolvedIncident);
        }

        // POST: UnresolvedIncidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnresolvedIncident unresolvedIncident = db.UnresolvedIncidents.Find(id);
            db.Incidents.Remove(unresolvedIncident);
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
