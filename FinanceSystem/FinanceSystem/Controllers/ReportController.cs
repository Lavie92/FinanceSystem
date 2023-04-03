using FinanceSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinanceSystem.Controllers
{
    public class ReportController : Controller
    {
        private FinanceSystemDBContext db = new FinanceSystemDBContext();

        // GET: Report
        public ActionResult Report()
        {
            return View();
        }

        public ActionResult GetData()
        {
           string id = User.Identity.GetUserId();

            var data = db.Transactions
             .Where(t => t.CreateDate.HasValue && t.Amount.HasValue && t.Income.HasValue && t.Wallet.UserId == id)
             .ToList() 
             .Select(t => new
             {
                 Date = t.CreateDate.Value.ToString("yyyy-MM-dd"),
                 Income = t.Income.Value,
                 Amount = t.Amount.Value
             })
             .ToList(); // 


            var result = new List<object>();

            result.Add(new { name = "Income", data = data.Where(d => d.Income).Select(d => new string[] { d.Date, d.Amount.ToString() }).ToArray() });
            result.Add(new { name = "Expense", data = data.Where(d => !d.Income).Select(d => new string[] { d.Date, d.Amount.ToString() }).ToArray() });

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ReportCategory()
        {
            return View();
        }
        
 
        public ActionResult GetDataCategory()
        {
            string id = User.Identity.GetUserId();

            using (var db = new FinanceSystemDBContext()) // khởi tạo biến db với đối tượng DbContext tương ứng của bạn

            {
                var data = db.Transactions.Where(x => x.Wallet.UserId == id)
                    .GroupBy(t => t.Category.CategoryName)
                    .Select(g => new { name = g.Key, count = g.Count() })
                    .ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }



    }
}
