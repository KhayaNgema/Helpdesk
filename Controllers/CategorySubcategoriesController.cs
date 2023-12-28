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
    public class CategorySubcategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategorySubcategories
        public ActionResult Index()
        {
            var categorySubcategories = db.CategorySubcategories.Include(c => c.Categories).Include(c => c.SubCategories);
            return View(categorySubcategories.ToList());
        }

        // GET: CategorySubcategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategorySubcategory categorySubcategory = db.CategorySubcategories.Find(id);
            if (categorySubcategory == null)
            {
                return HttpNotFound();
            }
            return View(categorySubcategory);
        }

        // GET: CategorySubcategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode");
            return View();
        }

        // POST: CategorySubcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategorySubCategoryId,CategoryId,SubCategoryId")] CategorySubcategory categorySubcategory)
        {
            if (ModelState.IsValid)
            {
                db.CategorySubcategories.Add(categorySubcategory);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", categorySubcategory.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", categorySubcategory.SubCategoryId);
            return View(categorySubcategory);
        }

        // GET: CategorySubcategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategorySubcategory categorySubcategory = db.CategorySubcategories.Find(id);
            if (categorySubcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", categorySubcategory.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", categorySubcategory.SubCategoryId);
            return View(categorySubcategory);
        }

        // POST: CategorySubcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategorySubCategoryId,CategoryId,SubCategoryId")] CategorySubcategory categorySubcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categorySubcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", categorySubcategory.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", categorySubcategory.SubCategoryId);
            return View(categorySubcategory);
        }

        // GET: CategorySubcategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategorySubcategory categorySubcategory = db.CategorySubcategories.Find(id);
            if (categorySubcategory == null)
            {
                return HttpNotFound();
            }
            return View(categorySubcategory);
        }

        // POST: CategorySubcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategorySubcategory categorySubcategory = db.CategorySubcategories.Find(id);
            db.CategorySubcategories.Remove(categorySubcategory);
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
