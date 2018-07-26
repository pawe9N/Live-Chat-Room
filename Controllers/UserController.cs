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
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "Home");
            }
            else
            {
                return View();
            }
        }

        //Verify account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            if(id != null)
            {
                bool Status = false;
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    dc.Configuration.ValidateOnSaveEnabled = false;

                    var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                    if (v != null)
                    {
                        v.IsEmailVerified = true;
                        dc.SaveChanges();
                        Status = true;
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Request";
                    }
                }
                ViewBag.Status = Status;
                return View();
            }
            else
            {
                return HttpNotFound();
            }       
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "Home");
            }
            else
            {
                return View();
            }
        }
    
        //Forgot Password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "Home");
            }
            else
            {
                return View();
            }
        }

        //Verify the reset password link
        //Find account associated with this link
        //Rediret to reset password page
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            if (id != null)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                    if (user != null)
                    {
                        ResetPasswordModel model = new ResetPasswordModel
                        {
                            ResetCode = id
                        };
                        return View(model);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}