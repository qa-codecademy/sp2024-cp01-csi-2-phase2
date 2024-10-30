using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWalletAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "WalletId", "Holdings", "Symbol", "UserId" },
                values: new object[] { 1, 0m, "BTC", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Wallets",
                keyColumn: "WalletId",
                keyValue: 1);
        }
    }
}
