using CryptoWalletAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWalletAPI.Data.FluentConfig
{
    public class WalletFluentConfig : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(u => u.User)
                .WithOne(w => w.Wallet)
                .HasForeignKey<Wallet>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Cryptos)
               .WithOne(w => w.Wallet)
               .HasForeignKey(c => c.WalletId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.BalanceUSD)
                .HasPrecision(18, 2)
                .HasDefaultValue(100000.00m);
        }
    }
}
