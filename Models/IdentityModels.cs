using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Helpdesk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string OfficeAddress { get; set; }
        public PersonalTitle Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellNumber { get; set; }
        public int DesignationId { get; set; }
        public virtual Designation Designations { get; set; }
        public string EmployeeOfficeAddress { get; set; }
        public bool Active { get; set; }
        public bool Inactive { get; set; }

        public int NotificationCount { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", $"{FirstName} {LastName}"));

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Helpdesk.Models.Country> Countries { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.Designation> Designations { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.ClientOnboarding> ClientOnboardings { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.ApprovedRequest> ApprovedRequests { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.DeclinedRequest> DeclinedRequests { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.ClientProduct> ClientProducts { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.Priority> Priorities { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.SubCategory> SubCategories { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.SLAValue> SLAValues { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.DatabaseType> DatabaseTypes { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.VirtualizedPlatform> VirtualizedPlatforms { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.HardwareDescription> HardwareDescriptions { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.EnvironmentType> EnvironmentTypes { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.CategorySubcategory> CategorySubcategories { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.Incident> Incidents { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.DailySequentialNumber> DailySequentialNumbers { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.FirstLineSupport> FirstLineSupports { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.SecondLineSupport> SecondLineSupports { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.ThirdLineSupport> ThirdLineSupports { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.ResolvedIncident> ResolvedIncidents { get; set; }
        public System.Data.Entity.DbSet<Helpdesk.Models.UnresolvedIncident> UnresolvedIncidents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Example: modelBuilder.Entity<YourEntity>().Property(x => x.YourProperty).HasColumnName("YourColumnName");

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }

        public System.Data.Entity.DbSet<Helpdesk.Models.ActiveManager> ActiveManagers { get; set; }

        public System.Data.Entity.DbSet<Helpdesk.Models.Notification> Notifications { get; set; }

    }
}