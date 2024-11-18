using CryptoWalletAPI.Data;
using CryptoWalletAPI.DTOs;
using CryptoWalletAPI.Helpers;
using CryptoWalletAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;


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

        public async Task<Result> BuyCrypto(CryptoTransactionDTO dto, int userId)
        {
            try
            {
                var wallet = await _context.Wallets
                     .Include(w => w.Cryptos)
                      .FirstOrDefaultAsync(x => x.UserId == userId);
                if (wallet == null) { return new Result<Wallet>("No wallet found!") { IsSuccess = false }; }

                var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;
                decimal cryptoValue = cryptoPrice.Outcome;
                var holding = wallet.Cryptos.FirstOrDefault(c => !string.IsNullOrEmpty(c.Symbol) &&
                                  c.Symbol.Equals(dto.Symbol, StringComparison.OrdinalIgnoreCase));
                if (wallet.BalanceUSD < dto.Amount * cryptoValue) { return new Result("Insufficient funds!"); }
                wallet.BalanceUSD -= dto.Amount * cryptoValue;
                if (holding == null)
                {
                    wallet.Cryptos.Add(new CryptoHoldings
                    {
                        Symbol = dto.Symbol,
                        Amount = dto.Amount,
                        ValueUSD = cryptoValue
                    });
                }
                else
                {
                    holding.Amount += dto.Amount;
                    holding.ValueUSD += cryptoValue;
                }

                await _context.SaveChangesAsync();
                return new Result("Coin bought!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Result> CryptoToCrypto(CryptoToCryptoDTO dto, int userId)
        {
            if (string.IsNullOrEmpty(dto.CryptoHolding))
            {
                return new Result("You must enter a crypto coin that you own");
            }

            if (string.IsNullOrEmpty(dto.ExchangeCrypto))
            {
                return new Result("You must enter a crypto symbol you want to exchange with!");
            }

            var wallet = await _context.Wallets
                .Include(w => w.Cryptos)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (wallet == null)
            {
                return new Result<Wallet>("No wallet found!") { IsSuccess = false };
            }

            var userHolding = wallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.CryptoHolding);

            if (userHolding is null)
            {
                return new Result("You don't own that crypto coin!");
            }

            var holdingValueInUSD = userHolding.ValueUSD * dto.HoldingAmount;

            var exchangeCryptoPriceResult = await GetCryptoPrice(dto.ExchangeCrypto);
            if (!exchangeCryptoPriceResult.IsSuccess)
            {
                return new Result("Failed to fetch the price for the exchange cryptocurrency.");
            }

            var exchangeCryptoValueInUSD = exchangeCryptoPriceResult.Outcome;

            if (holdingValueInUSD < exchangeCryptoValueInUSD * dto.HoldingAmount)
            {
                return new Result($"Insufficient {userHolding.Symbol} to exchange with {dto.ExchangeCrypto}");
            }

            var exchangedAmount = holdingValueInUSD / exchangeCryptoValueInUSD;

            wallet.Cryptos.Remove(userHolding);

            wallet.Cryptos.Add(new CryptoHoldings
            {
                Symbol = dto.ExchangeCrypto,
                Amount = exchangedAmount,
                ValueUSD = exchangeCryptoValueInUSD
            });

            await _context.SaveChangesAsync();

            return new Result("Crypto exchange successful!") { IsSuccess = true };
        }


        public async Task<Result<decimal>> GetCryptoPrice(string symbol)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.coinlore.net/api/tickers/");
                if (!response.IsSuccessStatusCode)
                {
                    return new Result<decimal>(0m)
                    {
                        IsSuccess = false,
                        ErrorMessage = new List<string> { "Failed to fetch data from the API." }
                    };
                }

                var content = await response.Content.ReadAsStringAsync();

                // Parse the response assuming "data" is an array
                var json = JObject.Parse(content);
                var crypto = json["data"]?.FirstOrDefault(c => c["symbol"]?.ToString() == symbol);

                if (crypto == null)
                {
                    return new Result<decimal>(0m)
                    {
                        IsSuccess = false,
                        ErrorMessage = new List<string> { "Symbol not found in API data." }
                    };
                }

                // Parse price_usd as decimal
                if (decimal.TryParse(crypto["price_usd"]?.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var priceUsd))
                {
                    return new Result<decimal>(priceUsd)
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    return new Result<decimal>(0m)
                    {
                        IsSuccess = false,
                        ErrorMessage = new List<string> { "Invalid price format in API data." }
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result<decimal>(0m)
                {
                    IsSuccess = false,
                    ErrorMessage = new List<string> { $"Error: {ex.Message}" }
                };
            }

        }


        public async Task<Result<Wallet>> GetWallet(int userId)
        {
            try
            {
                var user = _context.Users
                                  .Include(u => u.Wallet)
                                  .ThenInclude(w => w.Cryptos)
                                  .FirstOrDefault(u => u.Id == userId);

                return new Result<Wallet>(user?.Wallet);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Result> SellCrypto(CryptoTransactionDTO dto, int userId)
        {
            try
            {
                var wallet = await _context.Wallets
                         .Include(w => w.Cryptos)
                          .FirstOrDefaultAsync(x => x.UserId == userId);
                if (wallet == null) return new Result<Wallet>("Wallet not found.");

                var holding = wallet.Cryptos.FirstOrDefault(c => c.Symbol == dto.Symbol);

                if (holding == null)
                {
                    return new Result("You don't have that Crypto coin");
                }

                if (holding.Amount < dto.Amount)
                    return new Result<Wallet>("Not enough crypto to sell.");

                var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;
                var totalSale = cryptoPrice.Outcome * dto.Amount;

                wallet.BalanceUSD += totalSale;
                holding.Amount -= dto.Amount;
                holding.ValueUSD = cryptoPrice.Outcome;

                if (holding.Amount == 0) wallet.Cryptos.Remove(holding);

                await _context.SaveChangesAsync();
                return new Result($"{dto.Symbol}, {dto.Amount} sold successfully!");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Result> SendCrypto(SendCryptoDTO dto, int receiverId, int senderId)
        {
            try
            {
                //Fetch and check for the receivers wallet
                var receiverWallet = await _context.Wallets.Include(c => c.Cryptos).FirstOrDefaultAsync(x => x.Id == receiverId);
                if (receiverWallet is null) return new Result("The wallet you are trying to send crypto to doesn't exist!");

                //Fetch and check for the senders wallet
                var senderWallet = await _context.Wallets.Include(c => c.Cryptos).FirstOrDefaultAsync(x => x.Id == senderId);
                if (senderWallet is null) return new Result("Sender wallet can't be found!");

                //Fetch and check senders holdings
                var senderHolding = senderWallet.Cryptos.FirstOrDefault(x => x.Symbol == dto.Symbol);
                if (senderHolding is null) return new Result($"You don't own {senderHolding.Symbol}");
                if (senderHolding.Amount < dto.Amount) return new Result($"You don't own {dto.Amount} of {senderHolding.Symbol}");

                var cryptoPrice = GetCryptoPrice(dto.Symbol).Result;
                var transferValue = dto.Amount * cryptoPrice.Outcome;

                //Deduct from sender
                senderHolding.Amount -= dto.Amount;
                senderHolding.ValueUSD -= transferValue;
                if (senderHolding.Amount == 0) senderWallet.Cryptos.Remove(senderHolding);

                //Add crypto to the receiver
                var receiverHoldings = receiverWallet.Cryptos.FirstOrDefault(x => x.Symbol == dto.Symbol);
                if (receiverHoldings != null)
                {
                    receiverHoldings.Amount += dto.Amount;
                    receiverHoldings.ValueUSD = cryptoPrice.Outcome;
                }
                else
                {
                    receiverWallet.Cryptos.Add(new CryptoHoldings
                    {
                        Symbol = dto.Symbol,
                        Amount = dto.Amount,
                        ValueUSD = transferValue
                    });


                }
                await _context.SaveChangesAsync();
                return new Result("Coin sent!");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}