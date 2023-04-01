using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FinanceSystem.Models;
using Microsoft.AspNet.Identity;

namespace FinanceSystem.Controllers
{
    [Authorize]
    public class WalletsController : Controller
    {
        private FinanceSystemDBContext db = new FinanceSystemDBContext();
         
        // GET: Wallets
        public ActionResult Index()
        {
            
            string id = User.Identity.GetUserId();
            var wallets = db.Wallets.Include(w => w.AspNetUser).Include(w => w.Plan).Where(x => x.UserId == id);
            return View(wallets.ToList());
        }

        // GET: Wallets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = db.Wallets.Find(id);
            string userId = User.Identity.GetUserId();
            if (wallet.UserId != userId)
            {
            
                return View("~/Views/Shared/Error.cshtml");
            }
            if (wallet == null)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            return View(wallet);
        }

        // GET: Wallets/Create
        public ActionResult Create()
        {
            string id = User.Identity.GetUserId();
            ViewBag.UserId = new SelectList(db.AspNetUsers.Where(x => x.Id == id), "Id", "Email");           
            ViewBag.PlanId = new SelectList(db.Plans, "PlanId", "Description");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WalletId,UserId,WalletName,AccountBalance,PlanId")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                db.Wallets.Add(wallet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", wallet.UserId);
            ViewBag.PlanId = new SelectList(db.Plans, "PlanId", "Description", wallet.PlanId);
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = db.Wallets.Find(id);
            if (wallet == null)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            string userId = User.Identity.GetUserId();
            if (wallet.UserId != userId)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers.Where(x => x.Id == userId), "Id", "Email", wallet.UserId);
            ViewBag.PlanId = new SelectList(db.Plans, "PlanId", "Description", wallet.PlanId);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WalletId,UserId,WalletName,AccountBalance,PlanId")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wallet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", wallet.UserId);
            ViewBag.PlanId = new SelectList(db.Plans, "PlanId", "Description", wallet.PlanId);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = db.Wallets.Find(id);            
            if (wallet == null)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            string userId = User.Identity.GetUserId();
            if (wallet.UserId != userId)
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wallet wallet = db.Wallets.Find(id);
            db.Wallets.Remove(wallet);
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
