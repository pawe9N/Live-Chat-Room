using LiveChatRoom.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LiveChatRoom.Controllers
{
    public class HomeController : Controller
    {
        //For sending user data to chat
        private User UserModel = null;

        //For testing without database
        private IUserRepository userRepo;

        public HomeController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public HomeController()
        {
            this.userRepo = new EFUserRepository();
        }

        //To see user Account page
        [Authorize]
        [HttpGet]
        public ActionResult Account(string EmailID)
        {
            User userRecord;
            if (String.IsNullOrEmpty(EmailID))
            {
                userRecord = userRepo.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
            }
            else
            {
                userRecord = userRepo.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
            }

            if (userRecord != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                userRecord.ResetPasswordCode = resetCode;
                userRepo.ValidateOnSaveEnabledFalse();
                userRepo.SaveChanges();

                UserModel = userRecord;
                return View(UserModel);
            }
            else
                return HttpNotFound();
        }
  
        [Authorize]
        [HttpPost]
        public ActionResult Account(ChangeAvatarModel avatarModel)
        {
            User userRecord = userRepo.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
            if (userRecord != null)
            {
                if (ModelState.IsValid)
                {
                    userRecord.Avatar = avatarModel.Avatar;
                    userRepo.ValidateOnSaveEnabledFalse();
                    userRepo.SaveChanges();
                }                 

                UserModel = userRecord;

                return View(UserModel);
            }
            else
                return HttpNotFound();
        }

        //To see active users
        [Authorize]
        public ActionResult ActiveUsers()
        {
            return View();
        }

        //To chat with random people
        [Authorize]
        public ActionResult Chat(string EmailID = null)
        {
            User userRecord;
            if (String.IsNullOrEmpty(EmailID))
            {
                userRecord = userRepo.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
            }
            else
            {
                userRecord = userRepo.Users.Where(a => a.EmailID == EmailID).FirstOrDefault();
            }        
            
            if (userRecord != null)
            {
                User user = (User)userRecord;
                UserModel = user;
            }

            return View(UserModel);
        }
    }
}