using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using Helpdesk.Services;

namespace Helpdesk.Controllers
{
    public class ClientOnboardingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly CountryApiHelper _countryApiHelper;

        public ClientOnboardingsController()
        {
            db = new ApplicationDbContext();
            _countryApiHelper = new CountryApiHelper(db);
        }

        // GET: ClientOnboardings
        public ActionResult Index()
        {
            var clientOnboardings = db.ClientOnboardings.Include(c => c.Designations);
            return View(clientOnboardings.ToList());
        }

        // GET: ClientOnboardings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientOnboarding clientOnboarding = db.ClientOnboardings.Find(id);
            if (clientOnboarding == null)
            {
                return HttpNotFound();
            }
            return View(clientOnboarding);
        }

        public ActionResult Create()
        {
            var countryApiHelper = new CountryApiHelper(db);
            var countries = countryApiHelper.GetCountries();

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Name");

            // In your controller action
            var titleList = Enum.GetValues(typeof(PersonalTitle))
                                .Cast<PersonalTitle>()
                                .Select(t => new SelectListItem
                                {
                                    Value = t.ToString(),
                                    Text = t.ToString()
                                });

            ViewBag.TitleList = new SelectList(titleList, "Value", "Text");

            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OnboardingId,ClientName,OfficeAddress,EmailAddress,CountryId,PostalCode,PeriodFrom,PeriodTo,OpretaionalHoursOpen,OpretationalHourseClose,Status,TitleId,FirstName,LastName,Email,PhoneNumber,DesignationId,EmployeeOfficeAddress")] ClientOnboarding clientOnboarding)
        {
            if (ModelState.IsValid)
            {
                var countryApiHelper = new CountryApiHelper(db);

        
                countryApiHelper.SaveCountriesToDatabase();

                db.ClientOnboardings.Add(clientOnboarding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var countries = new CountryApiHelper(db).GetCountries();

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Name");

            // In your controller action
            var titleList = Enum.GetValues(typeof(PersonalTitle))
                                .Cast<PersonalTitle>()
                                .Select(t => new SelectListItem
                                {
                                    Value = t.ToString(),
                                    Text = t.ToString()
                                });

            ViewBag.TitleList = new SelectList(titleList, "Value", "Text");

            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName");

            return View(clientOnboarding);
        }



        // GET: ClientOnboardings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientOnboarding clientOnboarding = db.ClientOnboardings.Find(id);
            if (clientOnboarding == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", clientOnboarding.CountryId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", clientOnboarding.DesignationId);
            return View(clientOnboarding);
        }

        // POST: ClientOnboardings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OnboardingId,ClientName,OfficeAddress,CountryId,PostalCode,PeriodFrom,PeriodTo,OpretaionalHoursOpen,OpretationalHourseClose,Status,TitleId,FirstName,LastName,Email,PhoneNumber,DesignationId,EmployeeOfficeAddress")] ClientOnboarding clientOnboarding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientOnboarding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", clientOnboarding.CountryId);
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", clientOnboarding.DesignationId);
            return View(clientOnboarding);
        }

        // GET: ClientOnboardings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientOnboarding clientOnboarding = db.ClientOnboardings.Find(id);
            if (clientOnboarding == null)
            {
                return HttpNotFound();
            }
            return View(clientOnboarding);
        }

        // POST: ClientOnboardings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientOnboarding clientOnboarding = db.ClientOnboardings.Find(id);
            db.ClientOnboardings.Remove(clientOnboarding);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: ClientOnboardings/GeneratePdfReport


        private void SendNotificationEmail(ClientOnboarding request, string subject, string message)
        {
            string recipientEmail = request.Email;
            string senderEmail = "khayalethu.ngema@xetgroup.com";
            string senderPassword = "Ngema@12";

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "XET Helpdesk System"),
                    Subject = subject,
                    Body = $"Dear {request.ClientName}({request.ClientAbbr}) <br><br>" +
                           $"{message}<br><br>",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ClientOnboarding clientOnboarding = db.ClientOnboardings.Find(id);
            PrimaryContact primaryContact = db.ClientOnboardings.Find(id);


            if (clientOnboarding == null)
            {
                return HttpNotFound();
            }

            // Perform approval logic (update status, send notification, etc.)
            clientOnboarding.Status = RequestStatus.Approved; // Assume you have a Status property in your model

            // Move the approved request to the ApprovedRequests table
            ApprovedRequest approvedRequest = new ApprovedRequest
            {
                CountryId = clientOnboarding.CountryId,
                OnboardingId = clientOnboarding.OnboardingId,
                ClientName = clientOnboarding.ClientName,
                OfficeAddress = clientOnboarding.OfficeAddress,
                EmailAddress = clientOnboarding.EmailAddress,
                PostalCode = clientOnboarding.PostalCode,
                PeriodFrom = clientOnboarding.PeriodFrom,
                PeriodTo = clientOnboarding.PeriodTo,
                OpretaionalHoursOpen = clientOnboarding.OpretaionalHoursOpen,
                OpretationalHourseClose = clientOnboarding.OpretationalHourseClose,
                Status = clientOnboarding.Status,
                Title = clientOnboarding.Title,
                FirstName = clientOnboarding.FirstName,
                LastName = clientOnboarding.LastName,
                Email = clientOnboarding.Email,
                PhoneNumber = clientOnboarding.PhoneNumber,
                DesignationId = clientOnboarding.DesignationId,
                EmployeeOfficeAddress = clientOnboarding.EmployeeOfficeAddress,
            };

            // Send approval notification email with custom subject
            SendNotificationEmail(clientOnboarding, "Onboarding Request Feedback", "Your onboarding Request to XET Helpdesk system has been approved. See details below.");


            db.ApprovedRequests.Add(approvedRequest);

            // Remove the request from the TimeAwayRequests table
            db.ClientOnboardings.Remove(clientOnboarding);

            db.SaveChanges();

            TempData["SuccessMessage"] = "Time Away request approved successfully.";

            return RedirectToAction("Index");
        }

        public ActionResult Decline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ClientOnboarding clientOnboarding = db.ClientOnboardings.Find(id);
            PrimaryContact primaryContact = db.ClientOnboardings.Find(id);


            if (clientOnboarding == null)
            {
                return HttpNotFound();
            }

            // Perform approval logic (update status, send notification, etc.)
            clientOnboarding.Status = RequestStatus.Declined; // Assume you have a Status property in your model

            // Move the approved request to the ApprovedRequests table
            DeclinedRequest declinedRequest = new DeclinedRequest
            {
                CountryId=clientOnboarding.CountryId,   
                OnboardingId = clientOnboarding.OnboardingId,
                ClientName = clientOnboarding.ClientName,
                OfficeAddress = clientOnboarding.OfficeAddress,
                EmailAddress = clientOnboarding.EmailAddress,
                PostalCode = clientOnboarding.PostalCode,
                PeriodFrom = clientOnboarding.PeriodFrom,
                PeriodTo = clientOnboarding.PeriodTo,
                OpretaionalHoursOpen = clientOnboarding.OpretaionalHoursOpen,
                OpretationalHourseClose = clientOnboarding.OpretationalHourseClose,
                Status = clientOnboarding.Status,
                Title = clientOnboarding.Title,
                FirstName = clientOnboarding.FirstName,
                LastName = clientOnboarding.LastName,
                Email = clientOnboarding.Email,
                PhoneNumber = clientOnboarding.PhoneNumber,
                DesignationId = clientOnboarding.DesignationId,
                EmployeeOfficeAddress = clientOnboarding.EmployeeOfficeAddress,
            };

            SendNotificationEmail(clientOnboarding, "Onboarding Request Feedback", "Your onboarding Request to XET Helpdesk system has been approved. See details below.");



            db.DeclinedRequests.Add(declinedRequest);

            // Remove the request from the TimeAwayRequests table
            db.ClientOnboardings.Remove(clientOnboarding);

            db.SaveChanges();

            TempData["SuccessMessage"] = "Time Away request approved successfully.";

            return RedirectToAction("Index");
        }








    }
}
