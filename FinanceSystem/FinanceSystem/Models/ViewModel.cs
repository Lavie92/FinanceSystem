using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceSystem.Models
{
    public class ViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}