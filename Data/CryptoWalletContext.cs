using Microsoft.EntityFrameworkCore;
using CryptoWalletAPI.Models;

namespace CryptoWalletAPI.Data
{
    public class CryptoWalletContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<CryptoHoldings> CryptoHoldings { get; set; }

        public CryptoWalletContext(DbContextOptions<CryptoWalletContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-52E2U68\\SQLEXPRESS;Database=CryptoSphereWallet;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(CryptoWalletContext).Assembly);
            base.OnModelCreating(builder);
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
