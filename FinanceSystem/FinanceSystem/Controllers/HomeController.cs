using FinanceSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinanceSystem.Controllers
{
    //public class HomeController : Controller
    //{
    //    FinanceSystemDBContext db = new FinanceSystemDBContext();
    //    public ActionResult Index()
    //    {
    //        return View(db.Transactions.ToList());
    //    }
    //    public ActionResult Create()
    //    {
    //        ViewBag.WalletId = new SelectList(db.Wallets.ToList(), "WalletId", "WalletName");
    //        ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
    //        return View();
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]

    //    public ActionResult Create()
    //    {
    //        ViewBag.WalletId = new SelectList(db.Wallets.ToList(), "WalletId", "WalletName");
    //        ViewBag.CategoryId = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
    //        return View();
    //    }

    //    public ActionResult About()
    //    {
    //        ViewBag.Message = "Your application description page.";

    //        return View();
    //    }

    //    public ActionResult Contact()
    //    {
    //        ViewBag.Message = "Your contact page.";

    //        return View();
    //    }
    //}
}