using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AntiTheftClub.Models;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using System.Net.Mail;
using System.IO;

namespace AntiTheftClub
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            MailMessage myMessage = new MailMessage();
            if (string.IsNullOrEmpty(message.Destination) == false)
            {
                myMessage.To.Add(message.Destination);
            }
            else
            {
                myMessage.To.Add("balamurali2020@yahoo.com");
            }

            myMessage.From = new MailAddress("admin@antitheftclub.com");
            MailAddress balaBcc = new MailAddress("balamurali2020@yahoo.com");
            MailAddress hukmBcc = new MailAddress("hukmchand87@gmail.com");
            myMessage.Bcc.Add(hukmBcc);
            myMessage.Bcc.Add(balaBcc);
            myMessage.Subject = message.Subject;
            myMessage.Body = message.Body;
            myMessage.IsBodyHtml = true;


            var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["mailAccount"],
                       ConfigurationManager.AppSettings["mailPassword"]
                       );


            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.antitheftclub.com";
            smtp.Credentials = credentials;

            try
            {
                // Send the email.
                if (smtp != null)
                {
                   smtp.Send(myMessage);
                }
                else
                {
                    Trace.TraceError("Failed to Smtp Client.");
                    await Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message + " Smtp Client probably not configured correctly.");
            }
            finally
            {
                myMessage.Dispose();
            }
        }

    }



    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var Twilio = new
            {
                id = ConfigurationManager.AppSettings["sms_userName"],
                pwd = ConfigurationManager.AppSettings["sms_Password"],
                sender = ConfigurationManager.AppSettings["sms_sender_name"]
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=" + Twilio.id + "&password=" + Twilio.pwd + "&sendername=" + Twilio.sender + "&mobileno=91" + message.Destination + "&message=" + message.Body);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(responseStream, System.Text.Encoding.UTF8);
            string strSMSResponseString = readStream.ReadToEnd();


            // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(strSMSResponseString);

            // Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            // Configure validation logic for passwords
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
