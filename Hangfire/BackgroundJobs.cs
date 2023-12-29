
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




        public void AutoEscalateIncidents()
        {
            // Begin a database transaction
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var openFirstLineIncidents = db.FirstLineSupports
                        .Where(i => i.TicketStatus == TicketStatus.Open || i.TicketStatus == TicketStatus.Pending)
                        .ToList();

                    var openSecondLineIncidents = db.SecondLineSupports
                        .Where(i => i.TicketStatus == TicketStatus.Open || i.TicketStatus == TicketStatus.Pending)
                        .ToList();

                    var openThirdLineIncidents = db.ThirdLineSupports
                        .Where(i => i.TicketStatus == TicketStatus.Open || i.TicketStatus == TicketStatus.Pending)
                        .ToList();

                    foreach (var firstLineIncident in openFirstLineIncidents)
                    {
                        // Process first-line incidents
                        var subcategory = db.SubCategories.Find(firstLineIncident.SubCategoryId);

                        if (IsSLAExceeded(firstLineIncident.LoggedDate, subcategory.SLAValue.SLAValueName))
                        {
                            SendEscalationNotificationToFirstLineSupportWrapper(firstLineIncident);

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
                            db.SaveChanges(); // Commit changes for the current incident
                        }
                    }

                    foreach (var secondLineIncident in openSecondLineIncidents)
                    {
                        // Process second-line incidents
                        var subcategory = db.SubCategories.Find(secondLineIncident.SubCategoryId);

                        if (IsSLAExceeded(secondLineIncident.LoggedDate, subcategory.SLAValue.SLAValueName))
                        {
                            SendEscalationNotificationToSecondLineSupportWrapper(secondLineIncident);

                            var thirdLineSupport = new ThirdLineSupport
                            {
                                ReferenceNumber = secondLineIncident.ReferenceNumber,
                                OnboardingId = secondLineIncident.OnboardingId,
                                ProductId = secondLineIncident.ProductId,
                                CategoryId = secondLineIncident.CategoryId,
                                SubCategoryId = secondLineIncident.SubCategoryId,
                                Subject = secondLineIncident.Subject,
                                Description = secondLineIncident.Description,
                                ProductVersion = secondLineIncident.ProductVersion,
                                DatabaseTypeId = secondLineIncident.DatabaseTypeId,
                                HardwareDescriptionId = secondLineIncident.HardwareDescriptionId,
                                EnvironmentTypeId = secondLineIncident.EnvironmentTypeId,
                                VirtualizedPlatformId = secondLineIncident.VirtualizedPlatformId,
                                Title = secondLineIncident.Title,
                                CallersName = secondLineIncident.CallersName,
                                CallersSurname = secondLineIncident.CallersSurname,
                                EmailAddress = secondLineIncident.EmailAddress,
                                CellNumber = secondLineIncident.CellNumber,
                                DesignationId = secondLineIncident.DesignationId,
                                LoggedDate = secondLineIncident.LoggedDate,
                            };

                            db.ThirdLineSupports.Add(thirdLineSupport);
                            db.SecondLineSupports.Remove(secondLineIncident);
                            db.SaveChanges(); // Commit changes for the current incident
                        }
                    }

                    foreach (var thirdLineIncident in openThirdLineIncidents)
                    {
                        // Process third-line incidents
                        var subcategory = db.SubCategories.Find(thirdLineIncident.SubCategoryId);

                        if (IsSLAExceeded(thirdLineIncident.LoggedDate, subcategory.SLAValue.SLAValueName))
                        {
                            SendEscalationNotificationToThirdLineSupportWrapper(thirdLineIncident);

                            var activManager = new ActiveManager
                            {
                                ReferenceNumber = thirdLineIncident.ReferenceNumber,
                                OnboardingId = thirdLineIncident.OnboardingId,
                                ProductId = thirdLineIncident.ProductId,
                                CategoryId = thirdLineIncident.CategoryId,
                                SubCategoryId = thirdLineIncident.SubCategoryId,
                                Subject = thirdLineIncident.Subject,
                                Description = thirdLineIncident.Description,
                                ProductVersion = thirdLineIncident.ProductVersion,
                                DatabaseTypeId = thirdLineIncident.DatabaseTypeId,
                                HardwareDescriptionId = thirdLineIncident.HardwareDescriptionId,
                                EnvironmentTypeId = thirdLineIncident.EnvironmentTypeId,
                                VirtualizedPlatformId = thirdLineIncident.VirtualizedPlatformId,
                                Title = thirdLineIncident.Title,
                                CallersName = thirdLineIncident.CallersName,
                                CallersSurname = thirdLineIncident.CallersSurname,
                                EmailAddress = thirdLineIncident.EmailAddress,
                                CellNumber = thirdLineIncident.CellNumber,
                                DesignationId = thirdLineIncident.DesignationId,
                                LoggedDate = thirdLineIncident.LoggedDate,
                            };

                            db.ActiveManagers.Add(activManager);
                            db.ThirdLineSupports.Remove(thirdLineIncident);
                            db.SaveChanges(); // Commit changes for the current incident
                        }
                    }

                    // Commit the transaction after processing all incidents
                    transaction.Commit();
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
                case "144":
                    slaHours = 144;
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
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("khayalethu.ngema@xetgroup.com", "Ngema@12"),
                    EnableSsl = true,
                };

                var fromAddress = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Helpdesk System");

                var firstLineSupportRole = db.Roles.SingleOrDefault(r => r.Name == "First Line Support");
                var secondLineSupportRole = db.Roles.SingleOrDefault(r => r.Name == "Second Line Support");

                if (firstLineSupportRole != null && secondLineSupportRole != null)
                {
                    var firstLineSupportAgents = db.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == firstLineSupportRole.Id))
                        .ToList();

                    var secondLineSupportAgents = db.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == secondLineSupportRole.Id))
                        .ToList();

                    var allSupportAgents = firstLineSupportAgents.Concat(secondLineSupportAgents).Distinct().ToList();

                    foreach (var supportAgent in allSupportAgents)
                    {
                        var toAddress = new MailAddress(supportAgent.Email, $"{supportAgent.FirstName} {supportAgent.LastName}");

                        var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = "Incident Escalation Attention Required",
                            Body = $"Good day,\n\n" +
                                   $"Dear {supportAgent.FirstName} {supportAgent.LastName}\n\n" +
                                   $"Please note that the incident with the following details has been escalated " +
                                   $"from first line support to second line support:\n\n" +
                                   $"Ticket No.      : {firstLineIncident.ReferenceNumber}\n" +
                                   $"Client          : {firstLineIncident.ApprovedRequest.ClientName}\n" +
                                   $"Priority        : {firstLineIncident.SubCategories.Priority.PriorityName}\n" +
                                   $"Logged date     : {firstLineIncident.LoggedDate}\n" +
                                   $"Status          : {firstLineIncident.TicketStatus}\n\n" +
                                   $"Kind Regards\n" +
                                   $"XET Helpdesk",
                            IsBodyHtml = false
                        };

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



        private void SendEscalationNotificationToSecondLineSupport(SecondLineSupport secondLineIncident)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("khayalethu.ngema@xetgroup.com", "Ngema@12"),
                    EnableSsl = true,
                };

                var fromAddress = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Helpdesk System");

                var secondLineSupportRole = db.Roles.SingleOrDefault(r => r.Name == "Second Line Support");
                var thirdLineSupportRole = db.Roles.SingleOrDefault(r => r.Name == "Third Line Support");

                if (secondLineSupportRole != null && thirdLineSupportRole != null)
                {
                    var secondLineSupportAgents = db.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == secondLineSupportRole.Id))
                        .ToList();

                    var thirdLineSupportAgents = db.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == thirdLineSupportRole.Id))
                        .ToList();

                    var allSupportAgents = secondLineSupportAgents.Concat(thirdLineSupportAgents).Distinct().ToList();

                    foreach (var supportAgent in allSupportAgents)
                    {
                        var toAddress = new MailAddress(supportAgent.Email, $"{supportAgent.FirstName} {supportAgent.LastName}");

                        var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = "Incident Escalation Attention Required",
                            Body = $"Good day,\n\n" +
                                   $"Dear {supportAgent.FirstName} {supportAgent.LastName}\n\n" +
                                   $"Please note that the incident with the following details has been escalated " +
                                   $"from second line support to third line support:\n\n" +
                                   $"Ticket No.     : {secondLineIncident.ReferenceNumber}\n" +
                                   $"Client          : {secondLineIncident.ApprovedRequest.ClientName}\n" +
                                   $"Priority        : {secondLineIncident.SubCategories.Priority.PriorityName}\n" +
                                   $"Logged date     : {secondLineIncident.LoggedDate}\n" +
                                   $"Status          : {secondLineIncident.TicketStatus}\n\n" +
                                   $"Kind Regards\n" +
                                   $"XET Helpdesk",
                            IsBodyHtml = false
                        };

                        SendEmail(toAddress.Address, message.Subject, message.Body);
                    }
                }
                else
                {
                    Console.WriteLine("Second Line Support role not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending escalation notification: {ex.Message}");
                throw;
            }
        }


        private void SendEscalationNotificationToThirdLineSupport(ThirdLineSupport thirdLineIncident)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("khayalethu.ngema@xetgroup.com", "Ngema@12"),
                    EnableSsl = true,
                };

                var fromAddress = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Helpdesk System");

                var thirdLineSupportRole = db.Roles.SingleOrDefault(r => r.Name == "Third Line Support");
                var activeManagerRole = db.Roles.SingleOrDefault(r => r.Name == "Active Manager");

                if (thirdLineSupportRole != null && activeManagerRole != null)
                {
                    var thirdLineSupportAgents = db.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == thirdLineSupportRole.Id || r.RoleId == activeManagerRole.Id))
                        .ToList();

                    foreach (var supportAgent in thirdLineSupportAgents)
                    {
                        var toAddress = new MailAddress(supportAgent.Email, $"{supportAgent.FirstName} {supportAgent.LastName}");

                        var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = "Incident Escalation Attention Required",
                            Body = $"Good day,\n\n" +
                                   $"Dear {supportAgent.FirstName} {supportAgent.LastName}\n\n" +
                                   $"Please note that the incident with the following details has been escalated" +
                                   $"from third line support:\n\n " +
                                   $"Ticket No.      : {thirdLineIncident.ReferenceNumber}\n " +
                                   $"Client          : {thirdLineIncident.ApprovedRequest.ClientName}\n" +
                                   $"Priority        : {thirdLineIncident.SubCategories.Priority.PriorityName}\n" +
                                   $"Logged date     : {thirdLineIncident.LoggedDate}\n" +
                                   $"Status          : {thirdLineIncident.TicketStatus}\n\n" +
                                   $"Kind Regards\n" +
                                   $"XET Helpdesk",
                            IsBodyHtml = false
                        };

                        SendEmail(toAddress.Address, message.Subject, message.Body);
                    }
                }
                else
                {
                    Console.WriteLine("Third Line Support or Active Manager role not found.");
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

        public void SendEscalationNotificationToSecondLineSupportWrapper(SecondLineSupport secondLineIncident)
        {
            SendEscalationNotificationToSecondLineSupport(secondLineIncident);
        }

        public void SendEscalationNotificationToThirdLineSupportWrapper(ThirdLineSupport thirdLineIncident)
        {
            SendEscalationNotificationToThirdLineSupport(thirdLineIncident);
        }
    }
}
