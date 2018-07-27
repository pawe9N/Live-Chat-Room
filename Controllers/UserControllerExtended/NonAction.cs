using LiveChatRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace LiveChatRoom.Controllers
{
    public partial class UserController : Controller
    {
        private string email = "";
        private string password = "";

        [NonAction]
        private bool IsEmailExist(string emailID)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public bool IsUserAdult(DateTime? date)
        {
            DateTime maxDate = DateTime.Now.AddYears(-18);
            DateTime minDate = DateTime.Now.AddYears(-100);

            return (date >= minDate && date <= maxDate);
        }

        [NonAction]
        private void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "User/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "/" + verifyUrl);

            var fromEmail = new MailAddress(email, "Live Chat Room"); //Replace with actual email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; //Replace with actual password

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