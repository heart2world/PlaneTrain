using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string UserName, string Ticket)
        {
            ViewBag.Title = "Home Page";
            ViewBag.UserName = UserName;
            ViewBag.Ticket = Ticket;
            return View();
        }
    }
}
