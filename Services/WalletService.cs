using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoWalletAPI.Data;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;


namespace CryptoWalletAPI.Services
{
    public class WalletService : IWalletService
    {
        private readonly CryptoWalletContext _context;
        private readonly HttpClient _httpClient;

        public WalletService(CryptoWalletContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public Wallet GetWallet(int userId)
        {
            var user = _context.Users
                               .Include(u => u.Wallet)
                               .ThenInclude(w => w.Cryptos)
                               .FirstOrDefault(u => u.Id == userId);

            return user?.Wallet;
        }

        public Result AddCrypto(CryptoTransactionDTO dto, int userId)
        {
            var wallet = GetWallet(userId);
            if (wallet == null) return Result.Failure("Wallet not found.");

            var holding = wallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.Symbol);
            var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;

            if (holding != null)
            {
                holding.Amount += dto.Amount;
                holding.ValueUSD += dto.Amount * cryptoPrice;
            }
            else
            {
                wallet.Cryptos.Add(new CryptoHoldings
                {
                    Symbol = dto.Symbol,
                    Amount = dto.Amount,
                    ValueUSD = dto.Amount * cryptoPrice
                });
            }

            wallet.BalanceUSD += dto.Amount * cryptoPrice;
            _context.SaveChanges();
            return Result.Success();
        }

        public Result BuyCrypto(CryptoTransactionDTO dto, int userId)
        {
            var wallet = GetWallet(userId);
            if (wallet == null) return Result.Failure("Wallet not found.");

            var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;
            var totalCost = cryptoPrice * dto.Amount;
            if (wallet.BalanceUSD < totalCost)
                return Result.Failure("Insufficient funds.");

            wallet.BalanceUSD -= totalCost;
            var holding = wallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.Symbol);
            if (holding != null)
            {
                holding.Amount += dto.Amount;
                holding.ValueUSD += totalCost;
            }
            else
            {
                wallet.Cryptos.Add(new CryptoHoldings
                {
                    Symbol = dto.Symbol,
                    Amount = dto.Amount,
                    ValueUSD = totalCost
                });
            }

            _context.SaveChanges();
            return Result.Success();
        }

        public Result SellCrypto(CryptoTransactionDTO dto, int userId)
        {
            var wallet = GetWallet(userId);
            if (wallet == null) return Result.Failure("Wallet not found.");

            var holding = wallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.Symbol);
            if (holding == null || holding.Amount < dto.Amount)
                return Result.Failure("Not enough crypto to sell.");

            var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;
            var totalSale = cryptoPrice * dto.Amount;

            wallet.BalanceUSD += totalSale;
            holding.Amount -= dto.Amount;
            holding.ValueUSD -= totalSale;

            if (holding.Amount == 0) wallet.Cryptos.Remove(holding);

            _context.SaveChanges();
            return Result.Success();
        }

        public Result SendCrypto(SendCryptoDTO dto, int senderId)
        {
            var senderWallet = GetWallet(senderId);
            if (senderWallet == null) return Result.Failure("Sender's wallet not found.");

            var recipient = _context.Users.Include(u => u.Wallet).ThenInclude(w => w.Cryptos)
                                          .FirstOrDefault(u => u.Email == dto.RecipientEmail);
            if (recipient == null || recipient.Wallet == null)
                return Result.Failure("Recipient wallet not found.");

            var senderHolding = senderWallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.Symbol);
            if (senderHolding == null || senderHolding.Amount < dto.Amount)
                return Result.Failure("Insufficient crypto to send.");

            var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;
            var transferValue = dto.Amount * cryptoPrice;

            // Deduct from sender
            senderHolding.Amount -= dto.Amount;
            senderHolding.ValueUSD -= transferValue;
            if (senderHolding.Amount == 0) senderWallet.Cryptos.Remove(senderHolding);

            // Add to recipient
            var recipientHolding = recipient.Wallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.Symbol);
            if (recipientHolding != null)
            {
                recipientHolding.Amount += dto.Amount;
                recipientHolding.ValueUSD += transferValue;
            }
            else
            {
                recipient.Wallet.Cryptos.Add(new CryptoHoldings
                {
                    Symbol = dto.Symbol,
                    Amount = dto.Amount,
                    ValueUSD = transferValue
                });
            }

            _context.SaveChanges();
            return Result.Success();
        }

        public async Task<decimal> GetCryptoPrice(string symbol)
        {
            var response = await _httpClient.GetAsync("https://api.coinlore.net/api/tickers/");
            if (!response.IsSuccessStatusCode) return 0m;

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var crypto = json["data"]?.FirstOrDefault(c => c["symbol"]?.ToString() == symbol);
            return crypto != null ? decimal.Parse(crypto["price_usd"]?.ToString() ?? "0") : 0m;
        }
    }
}
