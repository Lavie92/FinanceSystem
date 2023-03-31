namespace FinanceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Transaction")]
    public partial class Transaction
    {
        public int TransactionId { get; set; }

        public int? WalletId { get; set; }

        public int? CategoryId { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(100)]
        public string Image { get; set; }
        public bool? Income { get; set; }

        [StringLength(200)]
        public string Note { get; set; }
        public HttpPostedFileBase ImageFile;
        public virtual Category Category { get; set; }

        public virtual Wallet Wallet { get; set; }
    }
}
