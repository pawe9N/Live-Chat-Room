using LiveChatRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiveChatRoom.Controllers
{
    public class HomeController : Controller
    {
        private User UserModel; 

        [Authorize]
        public ActionResult Index()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
                if (v != null)
                {
                    User user = (User)v;
                    UserModel = user;
                }
            }

            return View(UserModel);
        }

        [Authorize]
        public ActionResult Chat()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == HttpContext.User.Identity.Name).FirstOrDefault();
                if (v != null)
                {
                    User user = (User)v;
                    UserModel = user;
                }
            }

            return View(UserModel);
        }
    }
}