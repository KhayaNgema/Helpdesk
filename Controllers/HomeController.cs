using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Controllers
{
    public class HomeController : Controller
    {
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