using CryptoWalletAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "testuser", BalanceUSD = 100000 }
            );

            modelBuilder.Entity<Wallet>().HasData(
                new Wallet { WalletId = 1, UserId = 1, Symbol = "BTC", Holdings = 0 }
            );
        }
    }
}
