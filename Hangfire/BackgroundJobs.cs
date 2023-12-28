
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using Hangfire;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using Helpdesk.Models;
using Helpdesk;

namespace Helpdesk.Hangfire
{
    public class BackgroundJobs
    {
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager userManager;

        // Parameterized constructor
        public BackgroundJobs(ApplicationDbContext dbContext, ApplicationUserManager userManager)
        {
            db = dbContext;
            this.userManager = userManager;
        }

        public ApprovedRequest GetApprovedRequestById(int requestId)
        {
            // Perform the query to get the approved request by its ID
            var approvedRequest = db.ApprovedRequests
                .AsNoTracking()
                .FirstOrDefault(a => a.OnboardingId == requestId);

            return approvedRequest;
        }

        // Parameterless constructor for Hangfire
        public BackgroundJobs()
        {
            // Initialize your db or other fields if needed
            db = new ApplicationDbContext();
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // Rest of your class

        public void AutoEscalateIncidents()
        {
            // Begin a database transaction
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Get all open incidents in the FirstLineSupport table
                    var openFirstLineIncidents = db.FirstLineSupports
                        .Where(i => i.TicketStatus == TicketStatus.Open)
                        .ToList();

                    foreach (var firstLineIncident in openFirstLineIncidents)
                    {
                        // Get the associated subcategory to check SLA
                        var subcategory = db.SubCategories.Find(firstLineIncident.SubCategoryId);

                        // Check if SLA is reached or exceeded at the time of querying
                        if (IsSLAExceeded(firstLineIncident.LoggedDate, subcategory.SLAValue.SLAValueName))
                        {
                            // Enqueue a Hangfire job to send escalation notification
                            BackgroundJob.Enqueue(() => SendEscalationNotificationToFirstLineSupportWrapper(firstLineIncident));


                            var secondLineSupport = new SecondLineSupport
                            {
                                ReferenceNumber = firstLineIncident.ReferenceNumber,
                                OnboardingId = firstLineIncident.OnboardingId,
                                ProductId = firstLineIncident.ProductId,
                                CategoryId = firstLineIncident.CategoryId,
                                SubCategoryId = firstLineIncident.SubCategoryId,
                                Subject = firstLineIncident.Subject,
                                Description = firstLineIncident.Description,
                                ProductVersion = firstLineIncident.ProductVersion,
                                DatabaseTypeId = firstLineIncident.DatabaseTypeId,
                                HardwareDescriptionId = firstLineIncident.HardwareDescriptionId,
                                EnvironmentTypeId = firstLineIncident.EnvironmentTypeId,
                                VirtualizedPlatformId = firstLineIncident.VirtualizedPlatformId,
                                Title = firstLineIncident.Title,
                                CallersName = firstLineIncident.CallersName,
                                CallersSurname = firstLineIncident.CallersSurname,
                                EmailAddress = firstLineIncident.EmailAddress,
                                CellNumber = firstLineIncident.CellNumber,
                                DesignationId = firstLineIncident.DesignationId,
                                LoggedDate = firstLineIncident.LoggedDate,
                            };

                            db.SecondLineSupports.Add(secondLineSupport);
                            db.FirstLineSupports.Remove(firstLineIncident);
                            try
                            {
                                // Save changes to the database
                                db.SaveChanges();
                                transaction.Commit();
                            }
                            catch (DbUpdateConcurrencyException ex)
                            {
                                // Handle concurrency conflict
                                foreach (var entry in ex.Entries)
                                {
                                    if (entry.State == EntityState.Modified)
                                    {
                                        // Reload the entity from the database
                                        entry.Reload();
                                    }
                                }
                                // Retry the update or handle accordingly
                                // ...
                            }

                            // ... (rest of your code)
                        }
                    }
                }
                catch (Exception)
                {
                    // An error occurred, rollback the transaction
                    transaction.Rollback();
                    throw; // Re-throw the exception to let it propagate
                }
            }
        }

        private bool IsSLAExceeded(DateTime loggedDate, string slaValueName)
        {
            // Retrieve SLA hours based on the SLAValueName
            int slaHours = GetSLAHoursBySLAValueName(slaValueName);

            // Calculate the time difference from the logged date to now
            var elapsedTime = DateTime.Now - loggedDate;

            // Check if the elapsed time exceeds the SLA hours
            return elapsedTime.TotalHours >= slaHours;
        }

        private int GetSLAHoursBySLAValueName(string slaValueName)
        {
            // Implement your logic to retrieve SLA hours based on SLAValueName
            // Replace the following switch statement with your actual implementation

            int slaHours;

            switch (slaValueName)
            {
                case "4":
                    slaHours = 4;
                    break;
                case "8":
                    slaHours = 8;
                    break;
                case "24":
                    slaHours = 24;
                    break;
                case "48":
                    slaHours = 48;
                    break;
                case "72":
                    slaHours = 72;
                    break;
                case "96":
                    slaHours = 96;
                    break;
                case "120":
                    slaHours = 120;
                    break;
                default:
                    slaHours = 0; // Default to 0 if no match found
                    break;
            }

            return slaHours;
        }

        private void SendEscalationNotificationToFirstLineSupport(FirstLineSupport firstLineIncident)
        {
            try
            {
                // Set your SMTP server details
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Replace with your SMTP server address
                    Port = 587, // Replace with your SMTP server port
                    Credentials = new NetworkCredential("khayalethu.ngema@xetgroup.com", "Ngema@12"), // Replace with your email credentials
                    EnableSsl = true,
                };

                // Set the sender's email address
                var fromAddress = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Helpdesk System");

                // Fetch the "First Line Support" role
                var firstLineSupportRole = db.Roles.SingleOrDefault(r => r.Name == "First Line Support");

                if (firstLineSupportRole != null)
                {
                    // Fetch users in the "First Line Support" role
                    var firstLineSupportAgents = db.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == firstLineSupportRole.Id))
                        .ToList();

                    foreach (var firstLineAgent in firstLineSupportAgents)
                    {
                        // Set the recipient's email address
                        var toAddress = new MailAddress(firstLineAgent.Email, $"{firstLineAgent.FirstName} {firstLineAgent.LastName}");

                        // Create the email message
                        var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = "Incident Escalation Attention Required",
                            Body = $"Attention!\n\n" +
                                   $"The incident with Reference Number: {firstLineIncident.ReferenceNumber} " +
                                   $"has been escalated from FirstLine Support to Second line support\n\n" +
                                   $"Kind Regards\n" +
                                   $"XET Helpdesk System",
                            IsBodyHtml = false // Set to true if your email body is in HTML format
                        };

                        // Send the email using the centralized method
                        SendEmail(toAddress.Address, message.Subject, message.Body);
                    }
                }
                else
                {
                    Console.WriteLine("First Line Support role not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending escalation notification: {ex.Message}");
                throw;
            }
        }

        // Centralized method for sending emails
        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Set your SMTP server details
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Replace with your SMTP server address
                    Port = 587, // Replace with your SMTP server port
                    Credentials = new NetworkCredential("khayalethu.ngema@xetgroup.com", "Ngema@12"), // Replace with your email credentials
                    EnableSsl = true,
                };

                // Set the sender's email address
                var fromAddress = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Helpdesk System");

                // Set the recipient's email address
                var toAddress = new MailAddress(toEmail);

                // Create the email message
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false // Set to true if your email body is in HTML format
                };

                // Send the email
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public void SendEscalationNotificationToFirstLineSupportWrapper(FirstLineSupport firstLineIncident)
        {
            SendEscalationNotificationToFirstLineSupport(firstLineIncident);
        }

    }
}
