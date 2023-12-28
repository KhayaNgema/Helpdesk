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
    public class ThirdLineSupportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ThirdLineSupports
        public ActionResult Index()
        {
            var incidents = db.ThirdLineSupports.Include(t => t.ApprovedRequest).Include(t => t.Category).Include(t => t.DatabaseType).Include(t => t.Designation).Include(t => t.EnvironmentType).Include(t => t.HardwareDescription).Include(t => t.Products).Include(t => t.SubCategories).Include(t => t.VirtualizedPlatforms);
            return View(incidents.ToList());
        }

        // GET: ThirdLineSupports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThirdLineSupport thirdLineSupport = db.ThirdLineSupports.Find(id);
            if (thirdLineSupport == null)
            {
                return HttpNotFound();
            }
            return View(thirdLineSupport);
        }

        /* // GET: ThirdLineSupports/Create
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

         // POST: ThirdLineSupports/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] ThirdLineSupport thirdLineSupport)
         {
             if (ModelState.IsValid)
             {
                 db.ThirdLineSupports.Add(thirdLineSupport);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", thirdLineSupport.OnboardingId);
             ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", thirdLineSupport.CategoryId);
             ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", thirdLineSupport.DatabaseTypeId);
             ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", thirdLineSupport.DesignationId);
             ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", thirdLineSupport.EnvironmentTypeId);
             ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", thirdLineSupport.HardwareDescriptionId);
             ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", thirdLineSupport.ProductId);
             ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", thirdLineSupport.SubCategoryId);
             ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", thirdLineSupport.VirtualizedPlatformId);
             return View(thirdLineSupport);
         }

         // GET: ThirdLineSupports/Edit/5
         public ActionResult Edit(int? id)
         {
             if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             ThirdLineSupport thirdLineSupport = db.ThirdLineSupports.Find(id);
             if (thirdLineSupport == null)
             {
                 return HttpNotFound();
             }
             ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", thirdLineSupport.OnboardingId);
             ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", thirdLineSupport.CategoryId);
             ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", thirdLineSupport.DatabaseTypeId);
             ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", thirdLineSupport.DesignationId);
             ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", thirdLineSupport.EnvironmentTypeId);
             ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", thirdLineSupport.HardwareDescriptionId);
             ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", thirdLineSupport.ProductId);
             ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", thirdLineSupport.SubCategoryId);
             ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", thirdLineSupport.VirtualizedPlatformId);
             return View(thirdLineSupport);
         }

         // POST: ThirdLineSupports/Edit/5
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Edit([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,TitleId,CallersName,CallersSurname,EmailAddress,CellNumber,DesignationId,LoggedDate,TicketStatus")] ThirdLineSupport thirdLineSupport)
         {
             if (ModelState.IsValid)
             {
                 db.Entry(thirdLineSupport).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }
             ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", thirdLineSupport.OnboardingId);
             ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", thirdLineSupport.CategoryId);
             ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", thirdLineSupport.DatabaseTypeId);
             ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", thirdLineSupport.DesignationId);
             ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", thirdLineSupport.EnvironmentTypeId);
             ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", thirdLineSupport.HardwareDescriptionId);
             ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", thirdLineSupport.ProductId);
             ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", thirdLineSupport.SubCategoryId);
             ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", thirdLineSupport.VirtualizedPlatformId);
             return View(thirdLineSupport);
         }

         // GET: ThirdLineSupports/Delete/5
         public ActionResult Delete(int? id)
         {
             if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             ThirdLineSupport thirdLineSupport = db.ThirdLineSupports.Find(id);
             if (thirdLineSupport == null)
             {
                 return HttpNotFound();
             }
             return View(thirdLineSupport);
         }

         // POST: ThirdLineSupports/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public ActionResult DeleteConfirmed(int id)
         {
             ThirdLineSupport thirdLineSupport = db.ThirdLineSupports.Find(id);
             db.Incidents.Remove(thirdLineSupport);
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
