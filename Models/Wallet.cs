using System.Text.Json.Serialization;

namespace CryptoWalletAPI.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int CryptoId {  get; set; }
        public decimal BalanceUSD { get; set; } = 100000;
        public List<CryptoHoldings> Cryptos { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}


