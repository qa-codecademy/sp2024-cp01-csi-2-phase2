using System.Text.Json.Serialization;

namespace CryptoWalletAPI.Models
{
    public class CryptoHoldings
    {
        public int Id { get; set; }
        public int WalletId {  get; set; }
        public string? Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal ValueUSD { get; set; }

        [JsonIgnore]
        public Wallet Wallet {  get; set; }
    }
}
