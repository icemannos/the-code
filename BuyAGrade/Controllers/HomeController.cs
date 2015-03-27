using BuyAGrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyAGrade.Controllers
{
    public class HomeController : Controller
    {
        private static List<Customer> customers = new List<Customer>();

        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Time = DateTime.Now.ToString("h:mm tt");

            if (DateTime.Now.CompareTo(DateTime.Parse("12:00 PM")) < 0)
            {
                ViewBag.Greeting = "Good Morning";
            }
            else
            {
                ViewBag.Greeting = "Good Afternoon";
            }
            
            return View();
        }

        public ActionResult BuyNow()
        {
            ViewBag.Courses = new List<SelectListItem>
            { 
                new SelectListItem() {Text = "CSIS 4135", Value = "CSIS 4135"},
                new SelectListItem() {Text = "CSIS 3230", Value = "CSIS 3230"},
                new SelectListItem() {Text = "CSIS 3103", Value = "CSIS 3103"}
            };
            
            return View();
        }

        [HttpPost]
        public ActionResult BuyNow(Customer customer)
        {
            // processing of the form data goes here
            if (customer.AmountPaid >= 5000)
            {
                ViewBag.Grade = "A";
            }
            else if (customer.AmountPaid >= 3000)
            {
                ViewBag.Grade = "B";
            }
            else
            {
                ViewBag.Grade = "C";
            }

            customers.Add(customer);

            return View("Thanks", customer);
        }

        public ActionResult Customers()
        {
            ViewBag.CustomerList = customers;

            return View();
        }
    }
}