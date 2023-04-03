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
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;

namespace FinanceSystem.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private FinanceSystemDBContext db = new FinanceSystemDBContext();

        // GET: Transactions


        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            var transactions = db.Transactions.ToList().Where(x => x.Wallet.UserId == id).ToList();
            var categories = db.Categories.ToList();
            var viewModel = new TransactionViewModel
            {
                Transactions = transactions,
                Categories = categories
            };
            return View(viewModel);
        }

        public PartialViewResult FilterCategory(int? cate)
        {
            string id = User.Identity.GetUserId();
            List<Transaction> list = new List<Transaction>();
            if (cate == null || cate == 0)
            {
                list = db.Transactions.ToList().Where(x => x.Wallet.UserId == id).ToList();
            }
            else
            {
                list = db.Transactions.ToList().Where(x => x.Wallet.UserId == id).ToList().Where(p => p.CategoryId == cate).ToList();

            }

            var totalAmount = list.Sum(x => x.Amount);
            TempData["TotalAmount"] = totalAmount;
            return PartialView(list);
        }


        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    Transaction transaction = db.Transactions.Find(id);
        //    string userId = User.Identity.GetUserId();
        //    if (transaction.Wallet.UserId != userId)
        //    {
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    if (transaction == null)
        //    {
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return View(transaction);
        //}

        // GET: Transactions/Create
        public ActionResult Create()
        {
            string id = User.Identity.GetUserId();
            ViewBag.WalletId = new SelectList(db.Wallets.ToList().Where(x => x.UserId == id).ToList(), "WalletId", "WalletName");
            ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionId,WalletId,CategoryId,Amount,CreateDate,Image,Income,Note")] Transaction transaction)
        {
            //string id = User.Identity.GetUserId();
            transaction.ImageFile = Request.Files["ImageFile"];
            if (transaction.ImageFile != null && transaction.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(transaction.ImageFile.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                transaction.ImageFile.SaveAs(filePath);
                transaction.Image = "/Content/Images/" + fileName;

            }
            bool? selectedIncome = transaction.Income;
            if (!selectedIncome.Value)
            {
                transaction.Amount *= -1;
            }
            transaction.Amount *= 1;
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                Wallet wallet = db.Wallets.FirstOrDefault(x => x.WalletId == transaction.WalletId);
                wallet.AccountBalance += transaction.Amount;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", transaction.CategoryId);
            ViewBag.WalletId = new SelectList(db.Wallets, "WalletId", "UserId", transaction.WalletId);
            return View(transaction);
        }
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
            return PartialView("Edit", transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,WalletId,CategoryId,Amount,CreateDate,Income,Image,Note")] Transaction transaction)
        {
            transaction.ImageFile = Request.Files["ImageFile"];
            if (transaction.ImageFile != null && transaction.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(transaction.ImageFile.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                transaction.ImageFile.SaveAs(filePath);
                transaction.Image = "/Content/Images/" + fileName;

            }
            bool? selectedIncome = transaction.Income;
            if (!selectedIncome.Value)
            {
                transaction.Amount *= -1;
            }
            transaction.Amount *= 1;
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", transaction.CategoryId);
            ViewBag.WalletId = new SelectList(db.Wallets, "WalletId", "WalletName", transaction.WalletId);
            return PartialView("Edit", transaction);
        }
      
        public ActionResult Delete(int id)
        {
            try
            {
                Transaction transaction = db.Transactions.Find(id);
                if (transaction == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                db.Transactions.Remove(transaction);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xảy ra khi xóa sản phẩm: " + ex.Message });
            }
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
