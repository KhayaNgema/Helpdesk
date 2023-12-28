using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpdesk.Models
{
    public class IncidentViewModel
    {
        public string ReferenceNumber { get; set; }

        public int OnboardingId { get; set; }

        public virtual ApprovedRequest ApprovedRequest { get; set; }

        public int ProductId { get; set; }
        public virtual Product Products { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategories { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public HttpPostedFileBase IssueFiles { get; set; }

        public string ProductVersion { get; set; }

        public int DatabaseTypeId { get; set; }
        public virtual DatabaseType DatabaseType { get; set; }

        public int HardwareDescriptionId { get; set; }
        public virtual HardwareDescription HardwareDescription { get; set; }

        public int EnvironmentTypeId { get; set; }

        public virtual EnvironmentType EnvironmentType { get; set; }

        public int VirtualizedPlatformId { get; set; }
        public virtual VirtualizedPlatform VirtualizedPlatforms { get; set; }

        public PersonalTitle Title { get; set; }

        public string CallersName { get; set; }

        public string CallersSurname { get; set; }

        public string EmailAddress { get; set; }

        public string CellNumber { get; set; }

        public int DesignationId { get; set; }

        public DateTime LoggedDate { get; set; }

        public string CreatedById { get; set; }

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
}