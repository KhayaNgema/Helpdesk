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
    public class SecondLineSupportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SecondLineSupports
        public ActionResult Index()
        {
            var incidents = db.SecondLineSupports.Include(s => s.ApprovedRequest).Include(s => s.VirtualizedPlatforms);
            return View(incidents.ToList());
        }

        // GET: SecondLineSupports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecondLineSupport secondLineSupport = db.SecondLineSupports.Find(id);
            if (secondLineSupport == null)
            {
                return HttpNotFound();
            }
            return View(secondLineSupport);
        }

        /*// GET: SecondLineSupports/Create
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

        // POST: SecondLineSupports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] SecondLineSupport secondLineSupport)
        {
            if (ModelState.IsValid)
            {
                db.SecondLineSupports.Add(secondLineSupport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", secondLineSupport.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", secondLineSupport.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", secondLineSupport.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", secondLineSupport.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", secondLineSupport.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", secondLineSupport.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", secondLineSupport.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", secondLineSupport.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", secondLineSupport.VirtualizedPlatformId);
            return View(secondLineSupport);
        }

        // GET: SecondLineSupports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecondLineSupport secondLineSupport = db.SecondLineSupports.Find(id);
            if (secondLineSupport == null)
            {
                return HttpNotFound();
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", secondLineSupport.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", secondLineSupport.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", secondLineSupport.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", secondLineSupport.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", secondLineSupport.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", secondLineSupport.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", secondLineSupport.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", secondLineSupport.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", secondLineSupport.VirtualizedPlatformId);
            return View(secondLineSupport);
        }

        // POST: SecondLineSupports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] SecondLineSupport secondLineSupport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(secondLineSupport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", secondLineSupport.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", secondLineSupport.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", secondLineSupport.DatabaseTypeId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", secondLineSupport.DesignationId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", secondLineSupport.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", secondLineSupport.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", secondLineSupport.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", secondLineSupport.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", secondLineSupport.VirtualizedPlatformId);
            return View(secondLineSupport);
        }

        // GET: SecondLineSupports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecondLineSupport secondLineSupport = db.SecondLineSupports.Find(id);
            if (secondLineSupport == null)
            {
                return HttpNotFound();
            }
            return View(secondLineSupport);
        }

        // POST: SecondLineSupports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecondLineSupport secondLineSupport = db.SecondLineSupports.Find(id);
            db.Incidents.Remove(secondLineSupport);
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
    }*/
    }
}
