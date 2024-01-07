using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Rename;
using MimeKit;
using Org.BouncyCastle.Cms;
using Symphony_LTD.Data;
using Symphony_LTD.Models;

namespace Symphony_LTD.Controllers
{
    public class AutoEmailSenderController : Controller
    {
        private readonly DbSymphonyContext _db;
        private readonly IConfiguration _configuration;

        public AutoEmailSenderController(DbSymphonyContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public IActionResult Send(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                var email = _db._Contact.FirstOrDefault(email => email.ContactId == id);

                if(email == null) {
                    return View();
                }   

                ViewBag.SendEmail = email.Email ?? null;
                
                return View();
            }

            return RedirectToAction("LogIn", "Admin");

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(string recipient_email, string recipient_name, string subject, string message)
        {
            string secretValue = _configuration["Gmail_Smtp_Pass"];
            //Email Send Code Down Below Through MAILKIT package using Gmail SMTP service.
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Symphony Ltd", "wot.official.786@gmail.com"));
            msg.To.Add(new MailboxAddress(recipient_name, recipient_email));
            msg.Subject = subject;
            msg.Body = new TextPart { Text = message };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
            client.Authenticate("wot.official.786@gmail.com", secretValue);
            client.Send(msg);

            return RedirectToAction("Contact", "Admin");


        }

        
    }
}
