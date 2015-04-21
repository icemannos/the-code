using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuotationApp1;
using QuotationApp1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QuotationApp1.Controllers
{
    public class QuotationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> manager;
        private List<string> hideItems;
        private List<int> hideItemsInt;

        public QuotationsController()
        {
             manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Hide(int id, bool mine)
        {
            var cookie = Request.Cookies.Get("HideCookie");
            if (cookie == null)
            {
                cookie = new HttpCookie("HideCookie");
                hideItems = new List<string>() { id.ToString() };
                hideItemsInt = new List<int>() { id };
                cookie.Value = id.ToString();
                cookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(cookie);
            }
            else
            {
                cookie = Request.Cookies.Get("HideCookie");
                hideItems = cookie.Value.Split(',').ToList();
                hideItemsInt = new List<int>() { };
                foreach (string element in hideItems)
                {
                    hideItemsInt.Add(int.Parse(element));
                }
                hideItems.Add(id.ToString());
                hideItemsInt.Add(id);
                cookie.Value = string.Join(",", hideItems);
                Response.Cookies.Add(cookie);
            }

            if (mine == true)
            {
                return RedirectToAction("MyQuotes");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Unhide(bool mine)
        {
            var cookie = Request.Cookies.Get("HideCookie");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            if (mine == true)
            {
                return RedirectToAction("MyQuotes");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Quotations
        public ActionResult Index(string searchString)
        {
            var quotations = from s in db.Quotations.Include(q => q.Category)
                             where !hideItemsInt.Contains(s.QuotationID)
                             select s;

            var cookie = Request.Cookies.Get("HideCookie");
            if (cookie == null)
            {
                hideItems = new List<string>() { };
                hideItemsInt = new List<int>() { };
                ViewBag.Hide = false;
            }
            else
            {
                cookie = Request.Cookies.Get("HideCookie");
                hideItems = cookie.Value.Split(',').ToList();
                hideItemsInt = new List<int>() { };
                foreach (string element in hideItems)
                {
                    hideItemsInt.Add(int.Parse(element));
                }
                ViewBag.Hide = true;
            }

            ViewBag.ShowButton = false;
            ViewBag.Mine = false;

            if (!String.IsNullOrEmpty(searchString))
            {
                quotations = quotations.Where(s => s.Quote.Contains(searchString)
                                       || s.Author.Contains(searchString) || s.Category.Name.Contains(searchString));

                ViewBag.ShowButton = true;
            }

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Mine = true;
            }

            return View(quotations.ToList());
        }

        public ActionResult MyQuotes(string searchString)
        {
            var quotations = from s in db.Quotations.Include(q => q.Category)
                             where !hideItemsInt.Contains(s.QuotationID)
                             select s;

            var cookie = Request.Cookies.Get("HideCookie");
            if (cookie == null)
            {
                hideItems = new List<string>() { };
                hideItemsInt = new List<int>() { };
                ViewBag.Hide = false;
            }
            else
            {
                cookie = Request.Cookies.Get("HideCookie");
                hideItems = cookie.Value.Split(',').ToList();
                hideItemsInt = new List<int>() { };
                foreach (string element in hideItems)
                {
                    hideItemsInt.Add(int.Parse(element));
                }
                ViewBag.Hide = true;
            }

            string username = User.Identity.GetUserId();

            quotations = quotations.Where(s => s.Username.Contains(username));

            ViewBag.ShowButton = false;

            if (!String.IsNullOrEmpty(searchString))
            {
                quotations = quotations.Where(s => s.Quote.Contains(searchString)
                                       || s.Author.Contains(searchString) || s.Category.Name.Contains(searchString));

                ViewBag.ShowButton = true;
            }

            return View(quotations.ToList());
        }

        // GET: Quotations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // GET: Quotations/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Quotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuotationID,Quote,Author,Date,CategoryID,CreateCategory,Username")] Quotation quotation)
        {
            quotation.Username = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                db.Quotations.Add(quotation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (quotation.CreateCategory != null)
            {
                if (!db.Categories.Any(x => x.Name == quotation.CreateCategory))
                {
                    string name = quotation.CreateCategory;
                    int categoryId = 0;
                    return CreateNew(categoryId, name, quotation);
                }
                else
                {
                    ModelState.AddModelError("CategoryError", "That Category already exists.");
                }
            }
            else
            {
                ModelState.AddModelError("EmptyError", "The CategoryID field is required.");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", quotation.CategoryID);
            return View(quotation);
        }

        public ActionResult CreateNew(int categoryId, string name, Quotation quotation)
        {
            Category category = new Category();
            category.CategoryID = categoryId;
            category.Name = name;

            db.Categories.Add(category);
            db.Quotations.Add(quotation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Quotations/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", quotation.CategoryID);
            return View(quotation);
        }

        // POST: Quotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuotationID,Quote,Author,Date,CategoryID")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", quotation.CategoryID);
            return View(quotation);
        }

        // GET: Quotations/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // POST: Quotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quotation quotation = db.Quotations.Find(id);
            db.Quotations.Remove(quotation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}