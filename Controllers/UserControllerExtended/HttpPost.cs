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
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified, ActivationCode")] User user)
        {
            bool Status = false;
            string Message = "";

            // Model Validation
            if (ModelState.IsValid)
            {
                #region //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region //Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                user.IsEmailVerified = false;

                #region Save to Database

                userRepo.Save(user);
                userRepo.SaveChanges();

                //Send Email to User
                SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                Message = "Registration successfully done. Account activation link " +
                    "has been sent to your email id: " + user.EmailID;
                Status = true;
                #endregion
            }
            else
            {
                Message = "Invalid Request";
            }

            ViewBag.Message = Message;
            ViewBag.Status = Status;

            return View(user);
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel login, string ReturnUrl = "")
        {
            string Message = "";

            var userRecord = userRepo.Users.Where(a => a.EmailID == login.EmailId).FirstOrDefault();
            if (userRecord != null)
            {
                if (string.Compare(Crypto.Hash(login.Password), userRecord.Password) == 0)
                {
                    int timeout = login.RememberMe ? 66600 : 1;
                    var ticket = new FormsAuthenticationTicket(userRecord.EmailID, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
                    {
                        Expires = DateTime.Now.AddMinutes(timeout),
                        HttpOnly = true
                    };
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Chat", "Home");
                    }
                }
                else
                {
                    Message = "Invalid credential provided!";
                }
            }
            else
            {
                Message = "Invalid credential provided!";
            }

            ViewBag.Message = Message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            if(HttpContext != null)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Login", "User");
        }

        //After verifying email in database, send email to user with link to reset password page
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string Message = "";

            var account = userRepo.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
            if (account != null)
            {
                //Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.EmailID, resetCode, "ResetPassword");
                account.ResetPasswordCode = resetCode;

                //To avoid confirm password not match issue
                userRepo.ValidateOnSaveEnabledFalse();
                userRepo.SaveChanges();
                Message = "Reset password link has been sent to your email.";
            }
            else
            {
                Message = "Account not found";
            }       

            ViewBag.Message = Message;
            return View();
        }

        //After user click in link on email message, he will be directed to reset password page
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public ActionResult GetResetPassword(ResetPasswordModel model)
        {
            string Message = "";
            bool Status = false;
            if (ModelState.IsValid)
            {
                var user = userRepo.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Crypto.Hash(model.NewPassword);
                    user.ResetPasswordCode = "";

                    //To avoid confirm password not match issue
                    userRepo.ValidateOnSaveEnabledFalse();
                    userRepo.SaveChanges();
                    Message = "New password updated successfully!";
                    Status = true;
                }    
            }
            else
            {
                Message = "Error in reset password!";
            }
            ViewBag.Status = Status;
            ViewBag.Message = Message;
            return View(model);
        }
    }
}