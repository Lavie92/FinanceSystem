using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FinanceSystem.Models
{
    public partial class FinanceSystemDBContext : DbContext
    {
        public FinanceSystemDBContext()
            : base("name=FinanceSystemDBContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<UserInformation> UserInformations { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Plan>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Plan>()
                .HasMany(e => e.Wallets)
                .WithOptional(e => e.Plan)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Wallet>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Wallet>()
                .Property(e => e.AccountBalance)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Wallet>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Wallet)
                .HasForeignKey(e => e.WalletId)
                .WillCascadeOnDelete();
        }
    }
}
