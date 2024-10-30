using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoWalletAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public decimal BalanceUSD { get; set; } = 100000;

        [NotMapped]
        public Dictionary<string, decimal> Holdings { get; set; } = new Dictionary<string, decimal>(); // Holds currency symbol and amount

        public ICollection<Wallet> Wallets { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
