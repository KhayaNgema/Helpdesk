﻿using System;
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
    public class SubCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SubCategories
        public ActionResult Index()
        {
            return View(db.SubCategories.ToList());
        }

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.PriorityId = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.SLAValueId = new SelectList(db.SLAValues, "SLAValueId", "SLAValueName");

            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryId, faultCode, SubCategoryName, PriorityId, SLAValueId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.PriorityId = new SelectList(db.Priorities, "PriorityId", "PriorityName", subCategory.PriorityId);
            ViewBag.SLAValueId = new SelectList(db.SLAValues, "SLAValueId", "SLAValueName", subCategory.SLAValueId);

            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.PriorityId = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.SLAValueId = new SelectList(db.SLAValues, "SLAValueId", "SLAValueName");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCategoryId,faultCode,SubCategoryName")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PriorityId = new SelectList(db.Priorities, "PriorityId", "PriorityName", subCategory.PriorityId);
            ViewBag.SLAValueId = new SelectList(db.SLAValues, "SLAValueId", "SLAValueName", subCategory.SLAValueId);

            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.SubCategories.Find(id);
            db.SubCategories.Remove(subCategory);
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
