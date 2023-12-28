using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using Helpdesk.Models;
using Helpdesk.Hangfire;
using Helpdesk;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(AttendanceManagement.Startup))]
namespace AttendanceManagement
{
    public partial class Startup
    {
        private readonly BackgroundJobs backgroundJobs;

        public Startup()
        {
            backgroundJobs = new BackgroundJobs(new ApplicationDbContext(), new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext())));
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ConfigureHangfireEnhanced(app);

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        }

        private void ConfigureHangfireEnhanced(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\aspnet-Helpdesk-20240101013948.mdf;Initial Catalog=aspnet-Helpdesk-20240101013948;Integrated Security=True");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            RecurringJob.AddOrUpdate("auto-escalation-job", () => backgroundJobs.AutoEscalateIncidents(), "0 * * * *");
        }
    }
}
