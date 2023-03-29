using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceSystem.Models
{
    public class UserViewModel
    {
        public IEnumerable<AspNetUser> AspNetUser { get; set; }
        public IEnumerable<UserInformation> UserInformation { get; set; }
        public IEnumerable<Wallet> Wallet { get; set; }
        public IEnumerable<Transaction> Transaction { get; set; }

    }
}