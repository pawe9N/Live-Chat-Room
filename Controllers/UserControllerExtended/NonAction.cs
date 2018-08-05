using LiveChatRoom.App_Classes;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Hosting;
using System.Web.Mvc;

namespace LiveChatRoom.Controllers
{
    public partial class UserController : Controller
    {    
        [NonAction]
        //Method to sending users verification emails
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            SenderEmail senderData = new SenderEmail();
            var verifyUrl = "User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "/" + verifyUrl);

            var fromEmail = new MailAddress(senderData.email, "Live Chat Room"); 
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = senderData.password; 

            //creating messages
            string subject = "", body = "";

            using (StreamReader reader = new StreamReader(HostingEnvironment.MapPath("~/Content/ResetPasswordMessage.html")))
            {
                body = reader.ReadToEnd();
            }
            var userRecord = userRepo.Users.Where(a => a.EmailID == emailID).FirstOrDefault();

            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created";        
                body = body.Replace("#link", "<a href='" + link + "'>Verify Account</a>");
                body = body.Replace("#message", "Welcome #user, <br/><br/>We are excited to tell you that your account is successfully created!" +
                        "<br/><br/>Click on the link below to verify your account!");
                body = body.Replace("#user", userRecord.UserName);
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = body.Replace("#link", "<a href=" + link + ">Reset Password link</a>");
                body = body.Replace("#message", "Welcome #user, <br /><br />We got request for reset your account password. Please click on the link below to reset your password!");
                body = body.Replace("#user", userRecord.UserName);
            }

            //configuring smtp
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

            smtp.Send(message);
        }
    }
}