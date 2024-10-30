using System;

namespace CryptoWalletAPI.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public decimal PricePerUnit { get; set; }
        public DateTime TransactionDate { get; set; }

        public User User { get; set; }
    }
}
