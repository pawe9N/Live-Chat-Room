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
        //For testing without dataabse
        private IUserRepository userRepo;

        public UserController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserController()
        {
            this.userRepo = new EFUserRepository();
        }

        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            if (HttpContext != null && HttpContext.User.Identity.IsAuthenticated == true)
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
                userRepo.ValidateOnSaveEnabledFalse();

                var userData = userRepo.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (userData != null)
                {
                    userData.IsEmailVerified = true;
                    userRepo.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
                ViewBag.Status = Status;
                return View();
            }
            else
            {
                return HttpNotFound();
            }       
        }

        //To login
        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "Home");
            }
            else
            {
                return View();
            }
        }
    
        //To forgot password page
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //Find account associated with this reset password code, and then go to reset password page
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            if (id != null)
            {
                var user = userRepo.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
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
            else
            {
                return HttpNotFound();
            }
        }
    }
}