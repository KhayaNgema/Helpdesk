using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Helpdesk.Models;
using Helpdesk.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Helpdesk.Controllers
{
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notifications
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            // Filter notifications where the recipient is the current user
            var notifications = db.Notifications
                .Include(n => n.Recipient)
                .Include(n => n.Sender)
                .Where(n => n.RecipientId == userId)
                .OrderBy(n => n.NotificationId)
                .ToList();

            return View(notifications);
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Notification notification = db.Notifications.Find(id);

            if (notification == null)
            {
                return HttpNotFound();
            }

            // Update IsRead status
            notification.IsRead = true;
            notification.IsNew = false;
            db.SaveChanges();

            // Decrement notification count
            var userId = User.Identity.GetUserId(); // Assuming you're using ASP.NET Identity
            var user = db.Users.Find(userId);

            if (user != null)
            {
                user.NotificationCount = Math.Max(0, user.NotificationCount - 1);
                db.SaveChanges();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "ClientName");
            ViewBag.SenderId = new SelectList(db.Users, "Id", "ClientName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NotificationSubject,NotificationBody, RecipientId")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                // Set other properties of the notification
                notification.SenderId = User.Identity.GetUserId();
                notification.NotificationDate = DateTime.Now;
                notification.IsRead = false;


                db.Notifications.Add(notification);
                db.SaveChanges();

                // Send notification to the recipient using SignalR
                SendNotificationToRecipient(notification);

                return RedirectToAction("Index");
            }

            // ... other logic ...

            return View(notification);
        }

        // Helper method to send a notification to the recipient using SignalR
        private void SendNotificationToRecipient(Notification notification)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var recipientConnectionId = GetRecipientConnectionId(notification.RecipientId);

            if (!string.IsNullOrEmpty(recipientConnectionId))
            {
                // Notify the recipient about the new notification
                hubContext.Clients.Client(recipientConnectionId).receiveNotification(notification);
            }
        }

        // Helper method to get the connection ID of a user
        private string GetRecipientConnectionId(string userId)
        {
            var user = db.Users.Find(userId);
            return user?.Id;
        }

        // GET: Notifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "ClientName", notification.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "ClientName", notification.SenderId);
            return View(notification);
        }

        // POST: Notifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NotificationId,NotificationSubject,SenderId,RecipientId,NotificationDate,IsRead,IsUnread")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "ClientName", notification.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "ClientName", notification.SenderId);
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notification = db.Notifications.Find(id);
            db.Notifications.Remove(notification);
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
