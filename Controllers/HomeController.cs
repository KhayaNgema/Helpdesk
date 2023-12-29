using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using Microsoft.AspNet.Identity.EntityFramework;

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

            ViewBag.ClientAdminCount = GetClientAdminCount();

            ViewBag.XETEmployeesCount = GetXETEmployeesCount();

            ViewBag.ResolvedIncidentsCount = GetResolvedIncidentsCountFirst();

            ViewBag.UnresolvedIncidentsCount = GetUnresolvedIncidentsCountFirst();

            ViewBag.EscalatedIncidentsCount = GetEscalatedIncidentsCountFirst();


            ViewBag.ResolvedIncidentsCount = GetResolvedIncidentsCountSecond();

            ViewBag.UnresolvedIncidentsCount = GetUnresolvedIncidentsCountSecond();

            ViewBag.EscalatedIncidentsCount = GetEscalatedIncidentsCountSecond();


            ViewBag.ResolvedIncidentsCount = GetResolvedIncidentsCountThird();

            ViewBag.UnresolvedIncidentsCount = GetUnresolvedIncidentsCountThird();

            ViewBag.EscalatedIncidentsCount = GetEscalatedIncidentsCountThird();


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