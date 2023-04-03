using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinanceSystem.Models;

namespace FinanceSystem.Controllers
{
    public class UserInformationsController : Controller
    {
    
        private FinanceSystemDBContext db = new FinanceSystemDBContext();

        // GET: UserInformations
        public ActionResult Index()
        {
            var userInformations = db.UserInformations.Include(u => u.AspNetUser);
            return View(userInformations.ToList());
        }

        // GET: UserInformations/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformations.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // GET: UserInformations/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: UserInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,BirthDate,Gender,Image")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                db.UserInformations.Add(userInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userInformation.UserId);
            return View(userInformation);
        }

        // GET: UserInformations/Edit/5
        public ActionResult AccountSetting(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformations.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userInformation.UserId);
            
            return View(userInformation);
        }

        // POST: UserInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountSetting([Bind(Include = "UserId,FirstName,LastName,BirthDate,Gender,Image")] string PhoneNumber, UserInformation userInformation)
        {
            userInformation.ImageFile = Request.Files["ImageFile"];

            if (userInformation.ImageFile != null && userInformation.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(userInformation.ImageFile.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                userInformation.ImageFile.SaveAs(filePath);
                userInformation.Image = "/Content/Images/" + fileName;

            }
            AspNetUser aspNetUser = db.AspNetUsers.FirstOrDefault(x => x.Id == userInformation.UserId);
            //aspNetUser.PhoneNumber = PhoneNumber;
            if (ModelState.IsValid)
            {
                db.Entry(userInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Transactions");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", userInformation.UserId);
            return View(userInformation);
        }

        // GET: UserInformations/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformations.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // POST: UserInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserInformation userInformation = db.UserInformations.Find(id);
            db.UserInformations.Remove(userInformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult AccountSetting()
        //{

        //    return View();
        //}
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
