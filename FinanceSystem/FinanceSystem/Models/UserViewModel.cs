using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceSystem.Models
{
    public class WalletViewModel
    {
        public UserInformation UserInformation { get; set; }
        public IEnumerable<Wallet> Wallet { get; set; }

    }
}