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
    public class ClientProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClientProducts
        public ActionResult Index()
        {
            var clientProducts = db.ClientProducts.Include(c => c.ApprovedRequest).Include(c => c.Product);
            return View(clientProducts.ToList());
        }

        // GET: ClientProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientProduct clientProduct = db.ClientProducts.Find(id);
            if (clientProduct == null)
            {
                return HttpNotFound();
            }
            return View(clientProduct);
        }

        // GET: ClientProducts/Create
        public ActionResult Create()
        {
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: ClientProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientProductId,OnboardingId,ProductId")] ClientProduct clientProduct)
        {
            if (ModelState.IsValid)
            {
                db.ClientProducts.Add(clientProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", clientProduct.OnboardingId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", clientProduct.ProductId);
            return View(clientProduct);
        }

        // GET: ClientProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientProduct clientProduct = db.ClientProducts.Find(id);
            if (clientProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", clientProduct.OnboardingId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", clientProduct.ProductId);
            return View(clientProduct);
        }

        // POST: ClientProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientProductId,OnboardingId,ProductId")] ClientProduct clientProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", clientProduct.OnboardingId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", clientProduct.ProductId);
            return View(clientProduct);
        }

        // GET: ClientProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientProduct clientProduct = db.ClientProducts.Find(id);
            if (clientProduct == null)
            {
                return HttpNotFound();
            }
            return View(clientProduct);
        }

        // POST: ClientProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientProduct clientProduct = db.ClientProducts.Find(id);
            db.ClientProducts.Remove(clientProduct);
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
