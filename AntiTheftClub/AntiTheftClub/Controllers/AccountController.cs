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
using AntiTheftClub.Models;
using System.IO;
using AntiTheftClub.DbModel;
using AntiTheftClub.Services;

namespace AntiTheftClub.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private AccountServices accountServices;
        private DbEntities db;

        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }

        public AccountController()
        {
            
            this.db = new DbEntities();
            this.accountServices = new AccountServices(db);
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
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

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);
            var user = UserManager.Find(model.Email, model.Password);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Member Registration Activated in antitheftclub.com - Resend", "User");

                    // Uncomment to debug locally  
                    ViewBag.Link = callbackUrl;
                    ViewBag.errorMessage = "You must have a confirmed email to log on. "
                                         + "The confirmation token has been resent to your email account.";
                    return View("Error");
                }
            }
            else
            {
                ModelState.AddModelError("", "Your email address is not registered. Please click Sign-up to register");
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe = true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                    Session["userName"] = accountServices.GetFullName(user.Id);
                   // ViewBag.UserFullName  = accountServices.GetFullName(user.Id);;
                   
                    return RedirectToLocal(returnUrl);
                    
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "The email or password you entered is incorrect");
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
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
                // Remove for Debug
                ViewBag.Code = code;
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
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
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

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
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
                if (model.CountryCode == "in" && model.MobileNumber.Length > 10)
                {
                    model.MobileNumber = model.MobileNumber.GetLast(10);
                }
                model.IpAddress = ControllerContext.HttpContext.Request.UserHostAddress;
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    //  Comment the following line to prevent log in until the user is confirmed.
                    //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    model.NewUserId = user.Id;

                    accountServices.Register(model);
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account",model.FullName);
                    Session["callbackUrl"] = callbackUrl;
                    if (model.CountryCode == "in")
                    {
                        //var Signresult = await SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, shouldLockout: false);
                        AddPhoneNumberViewModel pnModel = new AddPhoneNumberViewModel();
                        pnModel.Number = model.MobileNumber;
                        // Generate the token and send it
                        Session["user.Id"] = user.Id;
                        var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, pnModel.Number);
                        if (UserManager.SmsService != null)
                        {
                            var message = new IdentityMessage
                            {
                                Destination = pnModel.Number,
                                Body = string.Format("Dear {0}, Please enter this {1} code to Activate your Account in AntiTheftClub.com", model.FullName, code)
                            };
                            await UserManager.SmsService.SendAsync(message);

                            // Uncomment to debug locally 
                            TempData["ViewBagCode"] = message.Body.ToString();
                        }
                        return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = pnModel.Number });
                    }
                    else
                    {

                        ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                                        + "before you can log in.";

                        // For local debug only
                        ViewBag.Link = callbackUrl;
                        //AuthenticationManager.SignOut();
                        return View("Info");
                    }
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        ////
        //// GET: /Manage/AddPhoneNumber
        //  [AllowAnonymous]
        //public ActionResult AddPhoneNumber()
        //{
        //    return View();
        //}

        
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            string userId =  Session["user.Id"] as string;
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userId, model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = string.Format("Dear {0}, Please enter this {1} code to Activate your Account in AntiTheftClub.com", "User", code)
                };
                await UserManager.SmsService.SendAsync(message);

                // Uncomment to debug locally 
                TempData["ViewBagCode"] = message.Body.ToString();
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // GET: /Manage/VerifyPhoneNumber
          [AllowAnonymous]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            string userId = Session["user.Id"] as string;
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);

            ViewBag.Code = code;
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") :
               View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string userId = Session["user.Id"] as string;
            var result = await UserManager.ChangePhoneNumberAsync(userId, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                //if (user != null)
                //{
                //    await SignInAsync(user, isPersistent: false);
                //}
                //return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });

                 ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                                        + "before you can log in.";

                        // For local debug only
                  ViewBag.Link = Session["callbackUrl"] as string;
                  //AuthenticationManager.SignOut();  
                  return View("Info");

            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
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

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password",
                 composeBodyResetPassowrdMail("User",callbackUrl));
                TempData["ViewBagLink"] = callbackUrl;
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Link = TempData["ViewBagLink"];
            return View();
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
            return RedirectToAction("VerifyCode", new
            {
                Provider = model.SelectedProvider,
                ReturnUrl = model.ReturnUrl,
                RememberMe = model.RememberMe
            });
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
                    return View("ExternalLoginFailure");
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
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject,string userName)
        {

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               composeBodyAccountConfirmationMail(userName, callbackUrl));

            return callbackUrl;
        }


        private string composeBodyAccountConfirmationMail(string userName,string link)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/AccountActivationTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{userName}", userName);
            body = body.Replace("{link}", link);
            return body;
        }


        private string composeBodyResetPassowrdMail(string userName, string link)
        {
            var callbackUrl = Url.Action("ForgotPassword", "Account", null, protocol: Request.Url.Scheme);

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/ResetPasswordTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{userName}", userName);
            body = body.Replace("{link}", link);
            body = body.Replace("{fglink}", callbackUrl);
            return body;
        }
        
        #endregion
    }

    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
    
}