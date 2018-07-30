using LiveChatRoom.App_Classes;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace LiveChatRoom.Controllers
{
    public partial class UserController : Controller
    {    
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
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created";
                body = "<br/><br/>We are excited to tell you that your account is successfully created!" +
                    "<br/><br/>Click on the below link to verify your account" + "<a href='" + link + "'>" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi, <br/><br/>We got request for reset your account password. Please click on the below link to reset your password"
                    + "<br/></br><a href=" + link + ">Reset Password link</a>";
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