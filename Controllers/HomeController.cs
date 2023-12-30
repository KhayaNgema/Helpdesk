using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Helpdesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // User is authenticated, redirect to the dashboard
                return RedirectToAction("Dashboard");
            }

            // User is not authenticated, show the home page
            return View();
        }

        public ActionResult Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, redirect to login or handle accordingly
                return RedirectToAction("Login", "Account");
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(User.Identity.GetUserId());

            ViewBag.ClientCount = GetClientCount();

            ViewBag.AllUsersCount = GetRegisteredUsersCount();

            ViewBag.ClientAdminCount = GetClientAdminCount();

            ViewBag.XETEmployeesCount = GetXETEmployeesCount();

            ViewBag.ResolvedIncidentsCountFirst = GetResolvedIncidentsCountFirst();

            ViewBag.UnresolvedIncidentsCountFirst = GetUnresolvedIncidentsCountFirst();

            ViewBag.EscalatedIncidentsCountFirst = GetEscalatedIncidentsCountFirst();


            ViewBag.ResolvedIncidentsCountSecond = GetResolvedIncidentsCountSecond();

            ViewBag.UnresolvedIncidentsCountSecond = GetUnresolvedIncidentsCountSecond();

            ViewBag.EscalatedIncidentsCountSecond = GetEscalatedIncidentsCountSecond();


            ViewBag.ResolvedIncidentsCountThird = GetResolvedIncidentsCountThird();

            ViewBag.UnresolvedIncidentsCountThird = GetUnresolvedIncidentsCountThird();

            ViewBag.EscalatedIncidentsCountThird = GetEscalatedIncidentsCountThird();


            ViewBag.AdminNotificationCount = GetNotificationAdmin();

            ViewBag.FirstLineNotificationCount = GetNotificationFirstLineSupport();

            ViewBag.SecondLineNotificationCount = GetNotificationSecondLineSupport();

            ViewBag.ThirdLineNotificationCount = GetNotificationThirdLineSupport();

            ViewBag.ActiveManagerNotificationCount = GetNotificationActiveManager();

            ViewBag.InactiveManagerNotificationCount = GetNotificationInactiveManager();


            if (roles.Contains("Administrator"))
            {
                // Admin dashboard logic or view
                return View("AdminDashboard");
            }

            if (roles.Contains("First Line Support"))
            {
                return View("FirstLineDashboard");
            }
            else if (roles.Contains("Second Line Support"))
            {
                // Technician dashboard logic or view
                return View("SecondLineDashboard");
            }
            else if (roles.Contains("Third Line Support"))
            {
                // Intern dashboard logic or view
                return View("ThirdLineDashboard");
            }
            else if (roles.Contains("Active Manager"))
            {
                // Senior Manager dashboard logic or view
                return View("ActiveManagerDashboard");
            }
            else if (roles.Contains("Inactive Manager"))
            {
                // Human Resources Employee dashboard logic or view
                return View("InactiveManagerDashboard");
            }
            else if (roles.Contains("Client Admin"))
            {
                // Human Resources Employee dashboard logic or view
                return View("ClientAdminDashboard");
            }

            // Default dashboard for other roles
            return View();
        }

        private int GetClientCount()
        {
            // Replace this with your actual database query
            using (var db = new ApplicationDbContext()) // Adjust this to your DbContext
            {
                // Assuming OnboardingId is the column used for identifying clients
                int clientCount = db.ApprovedRequests.Count();
                return clientCount;
            }
        }

        private int GetClientAdminCount()
        {
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var clientId = roleManager.Roles.FirstOrDefault(r => r.Name == "Client Admin")?.Id;

                if (clientId != null)
                {
                    var clientAdminCount = userManager.Users.Count(u => u.Roles.Any(r => r.RoleId == clientId));
                    return clientAdminCount;
                }
                return 0;
            }
        }

        private int GetRegisteredUsersCount()
        {
            using (var db = new ApplicationDbContext())
            {
                int registeredUsersCount = db.Users.Count();
                return registeredUsersCount;
            }
        }


        private int GetXETEmployeesCount()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming Email is the column used for storing email addresses
                int xETEmployeesCount = db.Users.Count(u => u.Email.EndsWith("@xetgroup.com"));
                return xETEmployeesCount;
            }
        }

        private int GetResolvedIncidentsCountFirst()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int resolvedIncidentsCount = db.ResolvedIncidents.Count(u => u.TicketStatus == TicketStatus.Closed);
                return resolvedIncidentsCount;
            }
        }

        private int GetUnresolvedIncidentsCountFirst()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int unresolvedIncidentsCount = db.FirstLineSupports.Count(u => u.TicketStatus == TicketStatus.Open || u.TicketStatus == TicketStatus.Pending || u.TicketStatus == TicketStatus.WaitingCustomerFeedback);
                return unresolvedIncidentsCount;
            }
        }

        private int GetEscalatedIncidentsCountFirst()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int escalatedIncidentsCount = db.FirstLineSupports.Count(u => u.TicketStatus == TicketStatus.Escalated);
                return escalatedIncidentsCount;
            }
        }


        private int GetResolvedIncidentsCountSecond()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int resolvedIncidentsCount = db.ResolvedIncidents.Count(u => u.TicketStatus == TicketStatus.Closed);
                return resolvedIncidentsCount;
            }
        }

        private int GetUnresolvedIncidentsCountSecond()
        {
            using (var db = new ApplicationDbContext())
            {

                int unresolvedIncidentsCount = db.SecondLineSupports.Count(u => u.TicketStatus == TicketStatus.Open || u.TicketStatus == TicketStatus.Pending || u.TicketStatus == TicketStatus.WaitingCustomerFeedback);
                return unresolvedIncidentsCount;
            }
        }

        private int GetEscalatedIncidentsCountSecond()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int escalatedIncidentsCount = db.SecondLineSupports.Count(u => u.TicketStatus == TicketStatus.Escalated);
                return escalatedIncidentsCount;
            }
        }


        private int GetResolvedIncidentsCountThird()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int resolvedIncidentsCount = db.ResolvedIncidents.Count(u => u.TicketStatus == TicketStatus.Closed);
                return resolvedIncidentsCount;
            }
        }

        private int GetUnresolvedIncidentsCountThird()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int unresolvedIncidentsCount = db.ThirdLineSupports.Count(u => u.TicketStatus == TicketStatus.Open || u.TicketStatus == TicketStatus.Pending || u.TicketStatus == TicketStatus.WaitingCustomerFeedback);
                return unresolvedIncidentsCount;
            }
        }

        private int GetEscalatedIncidentsCountThird()
        {
            using (var db = new ApplicationDbContext())
            {
                // Assuming TicketStatus is the enum property used for storing the status of incidents
                int escalatedIncidentsCount = db.ThirdLineSupports.Count(u => u.TicketStatus == TicketStatus.Escalated);
                return escalatedIncidentsCount;
            }
        }

        private int GetNotificationAdmin()
        {
            using (var db = new ApplicationDbContext())
            {
                var adminRoleName = "Administrator";

                // Get the RoleManager
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // Get the list of admin role IDs asynchronously
                var adminRoleIds = roleManager.Roles
                    .Where(r => r.Name == adminRoleName)
                    .Select(r => r.Id)
                    .ToListAsync()
                    .Result;

                // Get the list of admin user IDs
                var adminUserIds = db.Users
                    .Where(u => u.Roles.Any(r => adminRoleIds.Contains(r.RoleId)))
                    .Select(u => u.Id)
                    .ToList();

                // Count notifications where the recipient is an admin user
                int newNotificationsCount = db.Notifications.Count(n => adminUserIds.Contains(n.RecipientId));
                return newNotificationsCount;
            }
        }


        private int GetNotificationFirstLineSupport()
        {
            using (var db = new ApplicationDbContext())
            {
                var firstLineSupportRoleName = "First Line Support";

                // Get the RoleManager
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // Get the list of First Line Support role IDs asynchronously
                var firstLineSupportRoleIds = roleManager.Roles
                    .Where(r => r.Name == firstLineSupportRoleName)
                    .Select(r => r.Id)
                    .ToListAsync()
                    .Result;

                // Get the list of First Line Support user IDs
                var firstLineSupportUserIds = db.Users
                    .Where(u => u.Roles.Any(r => firstLineSupportRoleIds.Contains(r.RoleId)))
                    .Select(u => u.Id)
                    .ToList();

                // Count notifications where the recipient is in the First Line Support role
                int newNotificationsCount = db.Notifications.Count(n => firstLineSupportUserIds.Contains(n.RecipientId));
                return newNotificationsCount;
            }
        }


        private int GetNotificationSecondLineSupport()
        {
            using (var db = new ApplicationDbContext())
            {
                var secondLineSupportRoleName = "Second Line Support";


                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


                var secondLineSupportRoleIds = roleManager.Roles
                    .Where(r => r.Name == secondLineSupportRoleName)
                    .Select(r => r.Id)
                    .ToListAsync()
                    .Result;


                var secondLineSupportUserIds = db.Users
                    .Where(u => u.Roles.Any(r => secondLineSupportRoleIds.Contains(r.RoleId)))
                    .Select(u => u.Id)
                    .ToList();


                int newNotificationsCount = db.Notifications.Count(n =>
                    secondLineSupportUserIds.Contains(n.RecipientId) &&
                    !n.IsRead
                );

                return newNotificationsCount;
            }
        }


        private int GetNotificationThirdLineSupport()
        {
            using (var db = new ApplicationDbContext())
            {
                var thirdLineSupportRoleName = "Third Line Support";

                // Get the RoleManager
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // Get the list of Third Line Support role IDs asynchronously
                var thirdLineSupportRoleIds = roleManager.Roles
                    .Where(r => r.Name == thirdLineSupportRoleName)
                    .Select(r => r.Id)
                    .ToListAsync()
                    .Result;

                // Get the list of Third Line Support user IDs
                var thirdLineSupportUserIds = db.Users
                    .Where(u => u.Roles.Any(r => thirdLineSupportRoleIds.Contains(r.RoleId)))
                    .Select(u => u.Id)
                    .ToList();

                // Count notifications where the recipient is in the Third Line Support role
                int newNotificationsCount = db.Notifications.Count(n => thirdLineSupportUserIds.Contains(n.RecipientId));
                return newNotificationsCount;
            }
        }

        private int GetNotificationActiveManager()
        {
            using (var db = new ApplicationDbContext())
            {
                var activeManagerRoleName = "Active Manager";

                // Get the RoleManager
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // Get the list of Active Manager role IDs asynchronously
                var activeManagerRoleIds = roleManager.Roles
                    .Where(r => r.Name == activeManagerRoleName)
                    .Select(r => r.Id)
                    .ToListAsync()
                    .Result;

                // Get the list of Active Manager user IDs
                var activeManagerUserIds = db.Users
                    .Where(u => u.Roles.Any(r => activeManagerRoleIds.Contains(r.RoleId)))
                    .Select(u => u.Id)
                    .ToList();

                // Count notifications where the recipient is in the Active Manager role
                int newNotificationsCount = db.Notifications.Count(n => activeManagerUserIds.Contains(n.RecipientId));
                return newNotificationsCount;
            }
        }

        private int GetNotificationInactiveManager()
        {
            using (var db = new ApplicationDbContext())
            {
                var inactiveManagerRoleName = "Inactive Manager";

                // Get the RoleManager
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                // Get the list of Inactive Manager role IDs asynchronously
                var inactiveManagerRoleIds = roleManager.Roles
                    .Where(r => r.Name == inactiveManagerRoleName)
                    .Select(r => r.Id)
                    .ToListAsync()
                    .Result;

                // Get the list of Inactive Manager user IDs
                var inactiveManagerUserIds = db.Users
                    .Where(u => u.Roles.Any(r => inactiveManagerRoleIds.Contains(r.RoleId)))
                    .Select(u => u.Id)
                    .ToList();

                // Count notifications where the recipient is in the Inactive Manager role
                int newNotificationsCount = db.Notifications.Count(n => inactiveManagerUserIds.Contains(n.RecipientId));
                return newNotificationsCount;
            }
        }

        [HttpPost]
        public ActionResult MarkAsRead(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var notification = db.Notifications.Find(id);

                if (notification != null && !notification.IsRead)
                {
                    notification.IsRead = true;
                    db.SaveChanges();

                    // Decrease the count for the user only if they are in the recipient's role
                    var userId = User.Identity.GetUserId(); // Assuming you're using ASP.NET Identity
                    var user = db.Users.Find(userId);

                    if (user != null && user.Id == notification.RecipientId)
                    {
                        user.NotificationCount = Math.Max(0, user.NotificationCount - 1);
                        db.SaveChanges();
                    }
                }

                // Return the updated notification count as JSON
                var updatedCount = GetNotificationSecondLineSupport(); // Or any method to get the count
                return Json(new { NotificationCount = updatedCount }, JsonRequestBehavior.AllowGet);
            }
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}