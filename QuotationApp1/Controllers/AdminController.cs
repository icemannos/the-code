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
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<string> hideItems;
        private List<int> hideItemsInt;

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

            if (!String.IsNullOrEmpty(searchString))
            {
                quotations = quotations.Where(s => s.Quote.Contains(searchString)
                                       || s.Author.Contains(searchString) || s.Category.Name.Contains(searchString)
                                       || s. Username.Contains(searchString));

                ViewBag.ShowButton = true;
            }

            return View(quotations.ToList());
        }

        public ActionResult Hide(int id)
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

            return RedirectToAction("Index");
        }

        public ActionResult Unhide()
        {
            var cookie = Request.Cookies.Get("HideCookie");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }

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