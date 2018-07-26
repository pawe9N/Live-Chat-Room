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
                #region //Email is already Exist
                bool isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region //Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                user.IsEmailVerified = false;

                #region Save to Database
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    Message = "Registration successfully done. Account activation link " +
                        "has been sent to your email id: " + user.EmailID;
                    Status = true;
                }
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
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string Message = "";

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 1; //525600 min = 1
                        var ticket = new FormsAuthenticationTicket(v.EmailID, login.RememberMe, timeout);
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
            }

            ViewBag.Message = Message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        //Verify Email ID
        //Generate Reset password link
        //Send Email
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string Message = "";

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var account = dc.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    //to avoid confirm password not match issue,
                    //confirm password property is on mymodel class
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    Message = "Account not found";
                }
            }

            ViewBag.Message = Message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            string Message = "";
            bool Status = false;
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";

                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        Message = "New password updated successfully!";
                        Status = true;
                    }
                }
            }
            else
            {
                Message = "Error in ResetPassword";
            }
            ViewBag.Status = Status;
            ViewBag.Message = Message;
            return View(model);
        }
    }
}