using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuotationApp1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A database of quotations.";

            ViewBag.Admin = false;

            if (User.IsInRole("admin"))
            {
                ViewBag.Admin = true;
            }

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}