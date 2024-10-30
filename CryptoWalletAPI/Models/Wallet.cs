namespace CryptoWalletAPI.Models
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public string Symbol { get; set; }
        public decimal Holdings { get; set; }
        public User User { get; set; }
    }
}
