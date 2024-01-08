using Helpdesk.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Services.Description;

namespace Helpdesk.Models
{
    public class Incident
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Submission Id")]
        public int IncidentId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Product")]
        public int OnboardingId { get; set; }

        public virtual ApprovedRequest ApprovedRequest { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Sub-Category")]
        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategories { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Issue Description")]
        public string Description { get; set; }

        [Display(Name = "Issue evidence")]
        public string IssueFile { get; set; }

        [Display(Name = "Product Version")]
        public string ProductVersion { get; set; }

        [Display(Name = "Database Type")]
        public int DatabaseTypeId { get; set; }
        public virtual DatabaseType DatabaseType { get; set; }

        [Display(Name = "Hardware Description")]
        public int HardwareDescriptionId { get; set; }
        public virtual HardwareDescription HardwareDescription { get; set; }

        [Display(Name = "Environment Type")]
        public int EnvironmentTypeId { get; set; }

        public virtual EnvironmentType EnvironmentType { get; set; }

        [Display(Name = "Virtualized Platform")]
        public int VirtualizedPlatformId { get; set; }
        public virtual VirtualizedPlatform VirtualizedPlatforms { get; set; }

        [Display(Name = "Title")]
        public PersonalTitle Title { get; set; }

        [Display(Name = "Caller's Name")]
        public string CallersName { get; set; }

        [Display(Name = "Caller's Surname")]
        public string CallersSurname { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Cell Phone number")]
        public string CellNumber { get; set; }

        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public virtual Designation Designation { get; set; }

        [Display(Name = "Date Logged")]
        public DateTime LoggedDate { get; set; }

        [Display(Name = "Status")]
        public TicketStatus TicketStatus { get; set; }


        [NotMapped]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public string GenerateReferenceNumber(ApprovedRequest client, Product product)
        {
            // Get the client abbreviation, product key, current year, month, and day
            string clientAbbr = client.ClientAbbr; // Assuming there is a property for client abbreviation in the Client class
            string productKey = product.ProductKey; // Assuming there is a property for product key in the Product class
            int year = DateTime.Now.Year % 100; // 2 digits of the current year
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;

            // Get the sequential number for the day
            int sequentialNumber = GetSequentialNumberForDay();

            // Format the reference number
            string referenceNumber = $"{clientAbbr}-{productKey}-{year:D2}{month:D2}{day:D2}-{sequentialNumber:D4}";

            return referenceNumber;
        }

        private int GetSequentialNumberForDay()
        {
            DateTime currentDate = DateTime.Now.Date;

            using (ApplicationDbContext context = new ApplicationDbContext()) // Replace YourDbContext with your actual DbContext
            {
                // Retrieve the record for the current date, or create a new one if it doesn't exist
                DailySequentialNumber dailyNumber = context.DailySequentialNumbers
                    .FirstOrDefault(d => d.Date == currentDate);

                if (dailyNumber == null)
                {
                    // If the record doesn't exist, create a new one
                    dailyNumber = new DailySequentialNumber
                    {
                        Date = currentDate,
                        SequentialNumber = 1 // Start from 1 for a new day
                    };

                    context.DailySequentialNumbers.Add(dailyNumber);
                }
                else
                {
                    // If the record exists, increment the sequential number
                    dailyNumber.SequentialNumber++;
                }

                // Save changes to the database
                context.SaveChanges();

                // Return the updated sequential number
                return dailyNumber.SequentialNumber;
            }
        }
    }



    public class FirstLineSupport : Incident
    {
        
    }

    public class ActiveManager : Incident
    {

    }

    public class SecondLineSupport: Incident
    {
       
    }

    public class ThirdLineSupport : Incident    
    {
       
    }

    public class ResolvedIncident : Incident    
    {
       
    }

    public class UnresolvedIncident : Incident  
    {
       
    }



    public enum TicketStatus
    {
        [Display(Name = "Open")]
        Open,

        [Display(Name = "Reopened")]
        Reopened,

        [Display(Name = "Waiting Customer Feedback")]
        WaitingCustomerFeedback,

        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "Closed")]
        Closed,

        [Display(Name = "Escalated")]
        Escalated


    }

    public class DatabaseType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseTypeId { get; set; }

        public string DatabaseName { get; set; }
    }

    public class HardwareDescription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HardwareDescriptionId { get; set; }

        public string HardwareDescriptionName { get; set; }
    }

    public class EnvironmentType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnvironmentTypeId { get; set; }

        public string EnvironmentName { get; set; }
    }

    public class VirtualizedPlatform
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VirtualizedPlatformId { get; set; }

        public string VirtualizedPlatformName { get; set; }
    }
}