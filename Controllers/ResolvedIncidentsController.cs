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
    public class ResolvedIncidentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResolvedIncidents
        public ActionResult Index()
        {
            var incidents = db.ResolvedIncidents.Include(r => r.ApprovedRequest).Include(r => r.Category).Include(r => r.DatabaseType).Include(r => r.Designation).Include(r => r.EnvironmentType).Include(r => r.HardwareDescription).Include(r => r.Products).Include(r => r.SubCategories).Include(r => r.VirtualizedPlatforms);
            return View(incidents.ToList());
        }

        // GET: ResolvedIncidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResolvedIncident resolvedIncident = db.ResolvedIncidents.Find(id);
            if (resolvedIncident == null)
            {
                return HttpNotFound();
            }
            return View(resolvedIncident);
        }

        // GET: ResolvedIncidents/Create
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

        // POST: ResolvedIncidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] ResolvedIncident resolvedIncident)
        {
            if (ModelState.IsValid)
            {
                db.Incidents.Add(resolvedIncident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", resolvedIncident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", resolvedIncident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", resolvedIncident.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", resolvedIncident.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", resolvedIncident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", resolvedIncident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", resolvedIncident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", resolvedIncident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", resolvedIncident.VirtualizedPlatformId);
            return View(resolvedIncident);
        }

        // GET: ResolvedIncidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResolvedIncident resolvedIncident = db.ResolvedIncidents.Find(id);
            if (resolvedIncident == null)
            {
                return HttpNotFound();
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", resolvedIncident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", resolvedIncident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", resolvedIncident.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", resolvedIncident.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", resolvedIncident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", resolvedIncident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", resolvedIncident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", resolvedIncident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", resolvedIncident.VirtualizedPlatformId);
            return View(resolvedIncident);
        }

        // POST: ResolvedIncidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] ResolvedIncident resolvedIncident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resolvedIncident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", resolvedIncident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", resolvedIncident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", resolvedIncident.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", resolvedIncident.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", resolvedIncident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", resolvedIncident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", resolvedIncident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", resolvedIncident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", resolvedIncident.VirtualizedPlatformId);
            return View(resolvedIncident);
        }

        // GET: ResolvedIncidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResolvedIncident resolvedIncident = db.ResolvedIncidents.Find(id);
            if (resolvedIncident == null)
            {
                return HttpNotFound();
            }
            return View(resolvedIncident);
        }

        // POST: ResolvedIncidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResolvedIncident resolvedIncident = db.ResolvedIncidents.Find(id);
            db.Incidents.Remove(resolvedIncident);
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
