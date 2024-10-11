﻿using CryptoSphere.Wallet.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CryptoSphere.Wallet.Entities
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public string? UserId { get; set; }
        public decimal BalanceUSD { get; set; }
        public List<CryptoCoin>? Cryptos { get; set; } 
        public string? WalletAddress { get; set; }
        public WalletStatus WalletStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<Transaction> Transaction { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        public CryptoCoin? CryptoCoins { get; set; }
    }
}