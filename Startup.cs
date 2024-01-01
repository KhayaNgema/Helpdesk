using Hangfire;
using Hangfire.SqlServer;
using Helpdesk;
using Helpdesk.Hangfire;
using Helpdesk.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

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

            app.MapSignalR();

            ConfigureHangfireEnhanced(app);

            ConfigureAdditionalAppSettings(app);
        }


        private void ConfigureHangfireEnhanced(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\aspnet-Helpdesk-20240101013948.mdf;Initial Catalog=aspnet-Helpdesk-20240101013948;Integrated Security=True");
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\aspnet-Helpdesk-20240101013948.mdf;Initial Catalog=aspnet-Helpdesk-20240101013948;Integrated Security=True"); // Change this line
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            RecurringJob.AddOrUpdate("auto-escalation-job", () => backgroundJobs.AutoEscalateIncidents(), "0 * * * *");
        }

        private void ConfigureAdditionalAppSettings(IAppBuilder app)
        {
            // Additional configurations for your application
            // app.UseErrorPage(); // Commented this line to address the compilation error
        }
    }
}
