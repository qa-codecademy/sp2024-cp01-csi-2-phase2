using CryptoWalletAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWalletAPI.Data.FluentConfig
{
    public class CryptoHoldingFluentConfig : IEntityTypeConfiguration<CryptoHoldings>
    {
        public void Configure(EntityTypeBuilder<CryptoHoldings> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(w =>w.Wallet)
                .WithMany(x => x.Cryptos)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.ValueUSD)
                .HasPrecision(18, 2)
                .IsRequired();
        }
    }
}
