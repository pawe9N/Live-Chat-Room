using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiveChatRoom.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Chat()
        {
            return View();
        }
    }
}