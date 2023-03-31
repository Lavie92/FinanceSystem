﻿using System;
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
using Newtonsoft.Json;

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
            var viewModel = new TransactionViewModel
            {
                Transactions = transactions,
                Categories = categories
            };
            //var totalAmount = GetTotalAmount();
            //ViewBag.TotalAmount = totalAmount;
            return View(viewModel);
        }

        public PartialViewResult FilterCategory(int? cate)
        {
            List<Transaction> list = new List<Transaction>();
            if (cate == null || cate == 0)
            {
                list = db.Transactions.ToList();
            }
            else
            {
                list = db.Transactions.Where(p => p.CategoryId == cate).ToList();

            }
            var totalAmount = list.Sum(x => x.Amount);
            TempData["TotalAmount"] = totalAmount;
            return PartialView(list);
        }
        

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public decimal GetTotalAmount()
        {
            return db.Transactions.Sum(x => x.Amount.Value);
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
