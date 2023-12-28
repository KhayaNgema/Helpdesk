using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Helpdesk.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;



namespace Helpdesk.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly RoleManager<IdentityRole> _roleManager;

        public ActionResult ClientsPrimaryContacts()
        {
            var clientsPrimaryContacts = db.Users.Where(user => !user.Email.EndsWith("@xetgroup.com"));
            return View(clientsPrimaryContacts.ToList());
        }

        public ActionResult XETEmployees()
        {
            var xetEmployees = db.Users.Where(user => user.Email.EndsWith("@xetgroup.com"));
            return View(xetEmployees.ToList());
        }





        public AccountController()
        {
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
       
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string clientName, string officeAddress, string emailAddress)
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

            // Create a new instance of RegisterViewModel and pre-fill the data
            var model = new RegisterViewModel
            {
                ClientName = clientName,
                OfficeAddress = officeAddress,
                EmailAddress = emailAddress
            };


            return View(model);
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string generatedPassword = GenerateRandomPassword();

                var user = new ApplicationUser
                {
                    ClientName = model.ClientName,
                    OfficeAddress = model.ClientAddress,
                    ClientEmail = model.EmailAddress,
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.CellNumber,
                    Title = model.Title,
                    DesignationId = model.DesignationId,
                    EmployeeOfficeAddress = model.EmployeeOfficeAddress
                };
                var result = await UserManager.CreateAsync(user, generatedPassword);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // Send email with user's password
                    SendPasswordEmail(user.Email, generatedPassword);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
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
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", model.DesignationId);


            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [AllowAnonymous]
        public ActionResult RegisterXET(string firstName, string lastName, string idNumber, string cellNumber, string emailAddress, string employeePosition)
        {
            // If we got this far, something failed, redisplay form
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            ViewBag.Roles = new SelectList(roleManager.Roles, "Name", "Name");

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

          
            var model = new RegisterXETViewModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = emailAddress,
                Role = employeePosition,

            };

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterXET(RegisterXETViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Begin a database transaction
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Generate a random password
                        string generatedPassword = GenerateRandomPassword();

                        var user = new ApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            CellNumber = model.CellNumber,
                            Title = model.Title,
                            DesignationId = model.DesignationId,
                        };

                        var result = await UserManager.CreateAsync(user, generatedPassword);

                        if (result.Succeeded)
                        {
                            // Add user to the selected role
                            if (!string.IsNullOrEmpty(model.Role))
                            {
                                await UserManager.AddToRoleAsync(user.Id, model.Role);
                            }

                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            // Send email with user's password
                            SendPasswordEmail(user.Email, generatedPassword);

                            // Commit the transaction if everything succeeds
                            dbContextTransaction.Commit();

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // Rollback the transaction if any step fails
                            dbContextTransaction.Rollback();
                            AddErrors(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions and log if necessary
                        dbContextTransaction.Rollback();
                        // Log the exception
                        // ...

                        ModelState.AddModelError("", "An error occurred while processing your request.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            var titleList = Enum.GetValues(typeof(PersonalTitle))
                                .Cast<PersonalTitle>()
                                .Select(t => new SelectListItem
                                {
                                    Value = t.ToString(),
                                    Text = t.ToString()
                                });

            ViewBag.TitleList = new SelectList(titleList, "Value", "Text");
            ViewBag.DesignationId = new SelectList(db.Designations, "DesignationId", "DesignationName", model.DesignationId);

            // If we got this far, something failed, redisplay form
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            ViewBag.Roles = new SelectList(roleManager.Roles, "Name", "Name");
            return View(model);
        }







        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user with the provided email exists
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // Check if the user's email is confirmed
                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        // Don't reveal that the user's email is not confirmed
                        return View("ForgotPasswordConfirmation");
                    }

                    // Generate a password reset token and create a callback URL
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    // Send the password reset link via email
                    SendResetPasswordEmail(model.Email, callbackUrl);

                    // Redirect to a confirmation page
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                {
                    // Don't reveal that the user does not exist
                    return View("ForgotPasswordConfirmation");
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }




        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        private void SendResetPasswordEmail(string userEmail, string callbackUrl)
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

                // Create the email message
                var message = new MailMessage
                {
                    From = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Attendance Management System"),
                    Subject = "XET Helpdesk System - Reset Password",
                    Body = $"Dear User,\n\n\nYou have requested to reset your password for the " +
                           $"XET Helpdesk System. To reset your password, please click on the following link:\n\n" +
                           $"{callbackUrl}\n\n" +
                           $"If you did not request this change or have any concerns, please contact your system administrator.\n\n" +
                           $"Kind Regards\n" +
                           $"XET Management",
                    IsBodyHtml = false // Set to true if your email body is in HTML format
                };

                // Set the recipient's email address
                message.To.Add(new MailAddress(userEmail));

                // Send the email
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                // Throw an exception if the email sending fails
                throw new ApplicationException($"Failed to send email: {ex.Message}", ex);
            }
        }





        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToAction("Login");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private string GenerateRandomPassword()
        {
            // You can customize this method to generate a password based on your requirements
            int length = 10; // Set the desired length of the password
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string specialChars = "!@#$%^&*()-=_+[]{}|;:'\",.<>/?";

            Random random = new Random();
            char[] chars = new char[length];

            // Include at least one uppercase letter
            chars[0] = validChars[random.Next(26)]; // Assuming the first character is uppercase

            // Include at least one special character
            chars[1] = specialChars[random.Next(specialChars.Length)];

            // Include at least one digit
            chars[2] = validChars[random.Next(10) + 52]; // Assuming the third character is a digit

            // Fill the rest of the password
            for (int i = 3; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            // Shuffle the array to randomize the position of the special character, uppercase letter, and digit
            for (int i = length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }

            return new string(chars);
        }

        private void SendPasswordEmail(string userEmail, string generatedPassword)
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
            var fromAddress = new MailAddress("khayalethu.ngema@xetgroup.com", "XET Attendance Management System");

            // Set the recipient's email address
            var toAddress = new MailAddress(userEmail, "User Name");

            // Create the email message
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "XET Helpdesk System Login Credentials",
                Body = $"Good Day,\n\n\nYou have been granted access to use the " +
                       $"XET Helpdesk System . You are advised to change your password at first time login.\n\n" +
                       $"Below, please find your login credentials.\n\n" +
                       $"Email Address:< {userEmail}\n" +
                       $"Password: {generatedPassword}\n\n" +
                       $"If you have any enquires regarding the above login credentials, please contact your system administrator.\n\n" +
                       $"Kind Regards\n" +
                       $"XET Management",
                IsBodyHtml = false // Set to true if your email body is in HTML format
            };

            // Send the email
            smtpClient.Send(message);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}