namespace FinanceSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wallet")]
    public partial class Wallet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int WalletId { get; set; }
        public string WalletName { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public decimal? Amount { get; set; }

        public decimal? AccountBalance { get; set; }

        public int? PlanId { get; set; }

        public virtual Plan Plan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
