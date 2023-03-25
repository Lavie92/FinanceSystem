using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Util;
using FinanceSystem.Models;

namespace FinanceSystem.Controllers
{
    public class TransactionsController : Controller
    {
        private FinanceSystemDBContext db = new FinanceSystemDBContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.ToList();
            var categories = db.Categories.ToList();
            var viewModel = new ViewModel
            {
                Transactions = transactions,
                Categories = categories
            };
            return View(viewModel);
        }
        public ActionResult FilterByCategory(string category)
        {
            var transactions = db.Transactions.Include(t => t.Category);
            var categories = db.Categories.ToList();
            if (!string.IsNullOrEmpty(category))
            {
                transactions = transactions.Where(t => t.Category.CategoryName == category);
            }

            var viewModel = new ViewModel
            {
                Transactions = transactions.ToList(),
                Categories = db.Categories.ToList()
            };

            // Trả về một partial view chứa danh sách transaction đã được lọc

            return View("Index", viewModel);
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.WalletId = new SelectList(db.Wallets.ToList(), "WalletId", "WalletName");
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionId,WalletId,CategoryId,Amount,CreateDate,Image,Note")] Transaction transaction)
        {
                transaction.ImageFile = Request.Files["ImageFile"];
            if (transaction.ImageFile != null && transaction.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(transaction.ImageFile.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                transaction.ImageFile.SaveAs(filePath);
                transaction.Image = "/Content/Images/" + fileName;

            }
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", transaction.CategoryId);
            ViewBag.WalletId = new SelectList(db.Wallets, "WalletId", "WalletName", transaction.WalletId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", transaction.CategoryId);
            ViewBag.WalletId = new SelectList(db.Wallets, "WalletId", "WalletName", transaction.WalletId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,WalletId,CategoryId,Amount,CreateDate,Image,Note")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", transaction.CategoryId);
            ViewBag.WalletId = new SelectList(db.Wallets, "WalletId", "WalletName", transaction.WalletId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FilterCategory ()
        {
            return View("Index");
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
