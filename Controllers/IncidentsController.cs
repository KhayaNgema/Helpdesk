using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Hangfire;
using Helpdesk.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AttendanceManagement;
using Microsoft.Ajax.Utilities;
using static Hangfire.Storage.JobStorageFeatures;

namespace Helpdesk.Controllers
{
    public class IncidentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ApplicationUserManager _userManager;



        private readonly BackgroundJobs backgroundJobs;

        public IncidentsController()
        {
            // Initialize BackgroundJobs with your DbContext instance and ApplicationUserManager
            var dbContext = new ApplicationDbContext();
            var owinContext = System.Web.HttpContext.Current.GetOwinContext();
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();
            backgroundJobs = new BackgroundJobs(dbContext, userManager);
        }




        // GET: Incidents
        public ActionResult Index()
        {
            var incidents = db.Incidents
                .Include(i => i.ApprovedRequest)
                .Include(i => i.Category)
                .Include(i => i.DatabaseType)
                .Include(i => i.EnvironmentType)
                .Include(i => i.HardwareDescription)
                .Include(i => i.Products)
                .Include(i => i.SubCategories)
                .Include(i => i.VirtualizedPlatforms)
                .DistinctBy(i => i.ReferenceNumber) // Ensure uniqueness based on IncidentId
                .ToList();

            return View(incidents);
        }



        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        [Authorize]
        public ActionResult Create()
        {
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
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName");
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName");
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName");


            // Determine the initial client dynamically (you can replace this logic based on your requirement)
            var initialClient = db.ApprovedRequests.FirstOrDefault();

            if (initialClient != null)
            {
                // Retrieve products for the initial client
                var initialClientProducts = GetProductsForClient(initialClient.OnboardingId);

                ViewBag.ProductId = new SelectList(initialClientProducts, "ProductId", "ProductName");
            }
            else
            {
                // Handle the case when there are no clients available
                ViewBag.ProductId = new SelectList(new List<Product>(), "ProductId", "ProductName");
            }
            var initialCategory = db.Categories.FirstOrDefault();

            if (initialCategory != null)
            {
                // Retrieve products for the initial client
                var initialCategorySubcategories = GetSubCategoriesForCategory(initialCategory.CategoryId);

                ViewBag.SubCategoryId = new SelectList(initialCategorySubcategories, "SubCategoryId", "SubCategoryName");
            }
            else
            {
                // Handle the case when there are no clients available
                ViewBag.SubaCategoryId = new SelectList(new List<SubCategory>(), "SubCategoryId", "SubCategoryName");
            }
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName");

            return View();
        }


        // POST: Incidents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncidentViewModel incidentViewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the client and product instances from your context
                ApprovedRequest client = GetClientFromContext(incidentViewModel.OnboardingId); // Replace with your actual method
                Product product = GetProductFromContext(incidentViewModel.ProductId); // Replace with your actual method

                // Generate the reference number
                incidentViewModel.ReferenceNumber = incidentViewModel.GenerateReferenceNumber(client, product);

                string loggedInUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.Find(loggedInUserId);


                Incident newIncident = new Incident
                {

                    ReferenceNumber = incidentViewModel.ReferenceNumber,
                    OnboardingId = incidentViewModel.OnboardingId,
                    ProductId = incidentViewModel.ProductId,
                    CategoryId = incidentViewModel.CategoryId,
                    SubCategoryId = incidentViewModel.SubCategoryId,
                    Subject = incidentViewModel.Subject,
                    Description = incidentViewModel.Description,
                    ProductVersion = incidentViewModel.ProductVersion,
                    DatabaseTypeId = incidentViewModel.DatabaseTypeId,
                    HardwareDescriptionId = incidentViewModel.HardwareDescriptionId,
                    EnvironmentTypeId = incidentViewModel.EnvironmentTypeId,
                    VirtualizedPlatformId = incidentViewModel.VirtualizedPlatformId,
                    Title = currentUser.Title,
                    CallersName = currentUser.FirstName,
                    CallersSurname = currentUser.LastName,
                    EmailAddress = currentUser.UserName,
                    CellNumber = currentUser.PhoneNumber,
                    DesignationId = currentUser.DesignationId,
                    LoggedDate = DateTime.Now,
                };




                HandleImageUpload(newIncident, incidentViewModel.IssueFiles);


                // Determine the appropriate table based on subcategory priority and SLA
                SubCategory selectedSubcategory = db.SubCategories.Find(incidentViewModel.SubCategoryId);

                string tableName = GetTableNameBasedOnPriorityAndSLA(selectedSubcategory);

                // Create instances of the appropriate support types
                FirstLineSupport firstLineSupport = new FirstLineSupport
                {
                  
                    ReferenceNumber = incidentViewModel.ReferenceNumber,
                    OnboardingId = incidentViewModel.OnboardingId,
                    ProductId = incidentViewModel.ProductId,
                    CategoryId = incidentViewModel.CategoryId,
                    SubCategoryId = incidentViewModel.SubCategoryId,
                    Subject = incidentViewModel.Subject,
                    Description = incidentViewModel.Description,
                    ProductVersion = incidentViewModel.ProductVersion,
                    DatabaseTypeId = incidentViewModel.DatabaseTypeId,
                    HardwareDescriptionId = incidentViewModel.HardwareDescriptionId,
                    EnvironmentTypeId = incidentViewModel.EnvironmentTypeId,
                    VirtualizedPlatformId = incidentViewModel.VirtualizedPlatformId,
                    Title = currentUser.Title,
                    CallersName = currentUser.FirstName,
                    CallersSurname = currentUser.LastName,
                    EmailAddress = currentUser.UserName,
                    CellNumber = currentUser.PhoneNumber,
                    DesignationId = currentUser.DesignationId,
                    LoggedDate = DateTime.Now,
                };
                SecondLineSupport secondLineSupport = new SecondLineSupport
                {
                 
                    ReferenceNumber = incidentViewModel.ReferenceNumber,
                    OnboardingId = incidentViewModel.OnboardingId,
                    ProductId = incidentViewModel.ProductId,
                    CategoryId = incidentViewModel.CategoryId,
                    SubCategoryId = incidentViewModel.SubCategoryId,
                    Subject = incidentViewModel.Subject,
                    Description = incidentViewModel.Description,
                    ProductVersion = incidentViewModel.ProductVersion,
                    DatabaseTypeId = incidentViewModel.DatabaseTypeId,
                    HardwareDescriptionId = incidentViewModel.HardwareDescriptionId,
                    EnvironmentTypeId = incidentViewModel.EnvironmentTypeId,
                    VirtualizedPlatformId = incidentViewModel.VirtualizedPlatformId,
                    Title = currentUser.Title,
                    CallersName = currentUser.FirstName,
                    CallersSurname = currentUser.LastName,
                    EmailAddress = currentUser.UserName,
                    CellNumber = currentUser.PhoneNumber,
                    DesignationId = currentUser.DesignationId,
                    LoggedDate = DateTime.Now,
                };
                ThirdLineSupport thirdLineSupport = new ThirdLineSupport
                {
                   
                    ReferenceNumber = incidentViewModel.ReferenceNumber,
                    OnboardingId = incidentViewModel.OnboardingId,
                    ProductId = incidentViewModel.ProductId,
                    CategoryId = incidentViewModel.CategoryId,
                    SubCategoryId = incidentViewModel.SubCategoryId,
                    Subject = incidentViewModel.Subject,
                    Description = incidentViewModel.Description,
                    ProductVersion = incidentViewModel.ProductVersion,
                    DatabaseTypeId = incidentViewModel.DatabaseTypeId,
                    HardwareDescriptionId = incidentViewModel.HardwareDescriptionId,
                    EnvironmentTypeId = incidentViewModel.EnvironmentTypeId,
                    VirtualizedPlatformId = incidentViewModel.VirtualizedPlatformId,
                    Title = currentUser.Title,
                    CallersName = currentUser.FirstName,
                    CallersSurname = currentUser.LastName,
                    EmailAddress = currentUser.UserName,
                    CellNumber = currentUser.PhoneNumber,
                    DesignationId = currentUser.DesignationId,
                    LoggedDate = DateTime.Now,
                };

                ActiveManager activeManager = new ActiveManager
                {
                   
                    ReferenceNumber = incidentViewModel.ReferenceNumber,
                    OnboardingId = incidentViewModel.OnboardingId,
                    ProductId = incidentViewModel.ProductId,
                    CategoryId = incidentViewModel.CategoryId,
                    SubCategoryId = incidentViewModel.SubCategoryId,
                    Subject = incidentViewModel.Subject,
                    Description = incidentViewModel.Description,
                    ProductVersion = incidentViewModel.ProductVersion,
                    DatabaseTypeId = incidentViewModel.DatabaseTypeId,
                    HardwareDescriptionId = incidentViewModel.HardwareDescriptionId,
                    EnvironmentTypeId = incidentViewModel.EnvironmentTypeId,
                    VirtualizedPlatformId = incidentViewModel.VirtualizedPlatformId,
                    Title = currentUser.Title,
                    CallersName = currentUser.FirstName,
                    CallersSurname = currentUser.LastName,
                    EmailAddress = currentUser.UserName,
                    CellNumber = currentUser.PhoneNumber,
                    DesignationId = currentUser.DesignationId,
                    LoggedDate = DateTime.Now,
                };
             

                // Set the IncidentId for support entities
                firstLineSupport.IncidentId = newIncident.IncidentId;
                secondLineSupport.IncidentId = newIncident.IncidentId;
                thirdLineSupport.IncidentId = newIncident.IncidentId;

                switch (tableName)
                {
                    case "FirstLineSupport":
                        db.FirstLineSupports.Add(firstLineSupport);
                        break;
                    case "SecondLineSupport":
                        db.SecondLineSupports.Add(secondLineSupport);
                        break;
                    case "ThirdLineSupport":
                        db.ThirdLineSupports.Add(thirdLineSupport);

                        db.ActiveManagers.Add(activeManager);
                        break;
                    default:
                        break;
                }

                db.SaveChanges();  


                return RedirectToAction("Index");
            }
            // In your controller action
            var titleList = Enum.GetValues(typeof(PersonalTitle))
                                .Cast<PersonalTitle>()
                                .Select(t => new SelectListItem
                                {
                                    Value = t.ToString(),
                                    Text = t.ToString()
                                });

            ViewBag.TitleList = new SelectList(titleList, "Value", "Text");


            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", incidentViewModel.DesignationId);
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", incidentViewModel.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", incidentViewModel.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", incidentViewModel.DatabaseTypeId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", incidentViewModel.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", incidentViewModel.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", incidentViewModel.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", incidentViewModel.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", incidentViewModel.VirtualizedPlatformId);

            return View(incidentViewModel);
        }







        // Replace this with your actual method to get the client from the context
        private ApprovedRequest GetClientFromContext(int onboardingId)
        {
            return db.ApprovedRequests.Find(onboardingId);
        }

        // Replace this with your actual method to get the product from the context
        private Product GetProductFromContext(int productId)
        {
            return db.Products.Find(productId);
        }



        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", incident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", incident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", incident.DatabaseTypeId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", incident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", incident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", incident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", incident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", incident.VirtualizedPlatformId);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentId,ReferenceNumber,OnboardingId,ProductId,CategoryId,SubCategoryId,Subject,Description,IssueFile,ProductVersion,DatabaseTypeId,HardwareDescriptionId,EnvironmentTypeId,VirtualizedPlatformId,Title,CallersName,EmailAddress,CellNumber,Designation,LoggedDate")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OnboardingId = new SelectList(db.ApprovedRequests, "OnboardingId", "ClientName", incident.OnboardingId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", incident.CategoryId);
            ViewBag.DatabaseTypeId = new SelectList(db.DatabaseTypes, "DatabaseTypeId", "DatabaseName", incident.DatabaseTypeId);
            ViewBag.EnvironmentTypeId = new SelectList(db.EnvironmentTypes, "EnvironmentTypeId", "EnvironmentName", incident.EnvironmentTypeId);
            ViewBag.HardwareDescriptionId = new SelectList(db.HardwareDescriptions, "HardwareDescriptionId", "HardwareDescriptionName", incident.HardwareDescriptionId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", incident.ProductId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "faultCode", incident.SubCategoryId);
            ViewBag.VirtualizedPlatformId = new SelectList(db.VirtualizedPlatforms, "VirtualizedPlatformId", "VirtualizedPlatformName", incident.VirtualizedPlatformId);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
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

        private void HandleImageUpload(Incident newIncident, HttpPostedFileBase issueFiles)
        {
            if (issueFiles != null && issueFiles.ContentLength > 0)
            {
                try
                {
                    // Ensure the necessary using directive for 'Path'
                    string fileName = Path.GetFileName(issueFiles.FileName);
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);
                    issueFiles.SaveAs(path);

                    // Assuming you want to map 'IssueFiles' from view model to 'IssueFile' in the model
                    newIncident.IssueFile = "~/UploadedFiles/" + fileName;
                }
                catch (Exception ex)
                {
                    // Log the exc eption or handle it appropriately
                    Console.WriteLine($"Error uploading file: {ex.Message}");
                }
            }
        }

        private IEnumerable<SubCategory> GetSubCategoriesForCategory(int categoryId)
        {
            // Assuming CategorySubCategories has columns CategoryId and SubCategoryId
            var subcategoryIds = db.CategorySubcategories
                .Where(cs => cs.CategoryId == categoryId)
                .Select(cs => cs.SubCategoryId)
                .ToList();

            // Retrieve the corresponding subcategories based on the IDs
            var subcategories = db.SubCategories
                .Where(s => subcategoryIds.Contains(s.SubCategoryId))
                .ToList();

            return subcategories;
        }

        public JsonResult GetSubCategories(int categoryId)
        {
            var subcategories = GetSubCategoriesForCategory(categoryId);
            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProducts(int onboardingId)
        {
            try
            {
                var products = GetProductsForClient(onboardingId);

                // Return only the necessary data (ProductID and ProductName)
                var result = products.Select(p => new { ProductId = p.ProductId, ProductName = p.ProductName });

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error in GetProducts action: {ex.Message}");

                // Return an error response with the exception message
                return Json(new { error = $"An error occurred while fetching products. {ex.Message}" });
            }
        }

        private IEnumerable<Product> GetProductsForClient(int onboardingId)
        {
            try
            {
                var productIds = db.ClientProducts
                    .Where(cp => cp.OnboardingId == onboardingId)
                    .Select(cp => cp.ProductId)
                    .ToList();

                var products = db.Products
                    .Where(p => productIds.Contains(p.ProductId))
                    .ToList();

                return products;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error in GetProductsForClient method: {ex.Message}");
                throw; // Re-throw the exception for further diagnosis
            }
        }

        private string GetTableNameBasedOnPriorityAndSLA(SubCategory subcategory)
        {
            // Logic to determine the table based on priority and SLA
            if (subcategory.PriorityId == 1 && subcategory.SLAValueId == 3 ||
                subcategory.PriorityId == 1 && subcategory.SLAValueId == 2 ||
                subcategory.PriorityId == 1 && subcategory.SLAValueId == 1 ||
                subcategory.PriorityId == 2 && subcategory.SLAValueId == 4 ||
                subcategory.PriorityId == 2 && subcategory.SLAValueId == 5 ||
                subcategory.PriorityId == 2 && subcategory.SLAValueId == 6) // Assuming 1 corresponds to "High" priority and 1 corresponds to "24 Hours" SLA
            {
                return "ThirdLineSupport";


            }
            else if 
               (subcategory.PriorityId == 3 && subcategory.SLAValueId == 6 ||
                subcategory.PriorityId == 3 && subcategory.SLAValueId == 4 ||
                subcategory.PriorityId == 3 && subcategory.SLAValueId == 8 ||
                subcategory.PriorityId == 3 && subcategory.SLAValueId == 7   )// Assuming 2 corresponds to "Medium" priority and 2 corresponds to "48 Hours" SLA
            {
                return "SecondLineSupport"; // Replace with your actual table name
            }
            else

            {
                return "FirstLineSupport"; // Replace with your actual table name
            }
        }

    }
}
