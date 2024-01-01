using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Helpdesk.Models;
using Helpdesk;
using Microsoft.AspNet.SignalR;
using System.Data.Entity;
using AttendanceManagement;
using Microsoft.Owin.Builder;

namespace Helpdesk
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            CreateRolesAndUsers();
        }



        private void CreateRolesAndUsers()
        {
            var context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Check if the "Admin" role exists, if not, create it
            if (!roleManager.RoleExists("Administrator"))
            {
                var role = new IdentityRole { Name = "Administrator" };
                roleManager.Create(role);

                // Create a default admin user
                var user = new ApplicationUser { UserName = "khayalethu@xetgroup.com", DesignationId = 1 };
                var adminCreated = userManager.Create(user, "Ngema@12");

                // Add the admin user to the "Admin" role
                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }
            }

            // Check if other roles exist, if not, create them
            CreateRoleIfNotExist("First Line Support", roleManager);
            CreateRoleIfNotExist("Second Line Support", roleManager);
            CreateRoleIfNotExist("Third Line Support", roleManager);
            CreateRoleIfNotExist("Active Manager", roleManager);
            CreateRoleIfNotExist("Inactive Manager", roleManager);
            CreateRoleIfNotExist("Client Admin", roleManager);
        }

        private void CreateRoleIfNotExist(string roleName, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole { Name = roleName };
                roleManager.Create(role);
            }
        }
    }
}
