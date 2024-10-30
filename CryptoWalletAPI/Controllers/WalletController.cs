using CryptoWalletAPI.Data;
using CryptoWalletAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string CoinloreApiUrl = "https://api.coinlore.net/api/tickers/";

    public WalletController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyCrypto([FromBody] BuySellRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }

        var price = await GetCryptoPrice(request.Symbol);
        if (price == null)
        {
            return BadRequest(new { message = "Cryptocurrency not found" });
        }

        var cost = request.Amount * price.Value;
        if (user.BalanceUSD < cost)
        {
            return BadRequest(new { message = "Insufficient balance" });
        }

        user.BalanceUSD -= cost;

        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.UserId == user.UserId && w.Symbol == request.Symbol);

        if (wallet == null)
        {
            wallet = new Wallet
            {
                UserId = user.UserId,
                Symbol = request.Symbol,
                Holdings = request.Amount
            };
            await _context.Wallets.AddAsync(wallet);
        }
        else
        {
            wallet.Holdings += request.Amount;
            _context.Wallets.Update(wallet);
        }

        var transaction = new Transaction
        {
            UserId = user.UserId,
            Symbol = request.Symbol,
            Amount = request.Amount,
            TransactionType = "buy",
            PricePerUnit = price.Value,
            TransactionDate = DateTime.UtcNow
        };

        await _context.Transactions.AddAsync(transaction);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Successfully bought {request.Amount} {request.Symbol} for ${cost:F2}.", newBalanceUSD = user.BalanceUSD });
    }

    [HttpPost("sell")]
    public async Task<IActionResult> SellCrypto([FromBody] BuySellRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }

        var price = await GetCryptoPrice(request.Symbol);
        if (price == null)
        {
            return BadRequest(new { message = "Cryptocurrency not found" });
        }

        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.UserId == user.UserId && w.Symbol == request.Symbol);

        if (wallet == null || wallet.Holdings < request.Amount)
        {
            return BadRequest(new { message = "Insufficient holdings to sell" });
        }

        var gain = request.Amount * price.Value;

        user.BalanceUSD += gain;

        wallet.Holdings -= request.Amount;

        if (wallet.Holdings == 0)
        {
            _context.Wallets.Remove(wallet);
        }
        else
        {
            _context.Wallets.Update(wallet);
        }

        var transaction = new Transaction
        {
            UserId = user.UserId,
            Symbol = request.Symbol,
            Amount = request.Amount,
            TransactionType = "sell",
            PricePerUnit = price.Value,
            TransactionDate = DateTime.UtcNow
        };

        await _context.Transactions.AddAsync(transaction);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Successfully sold {request.Amount} {request.Symbol} for ${gain:F2}.", newBalanceUSD = user.BalanceUSD });
    }

    private async Task<decimal?> GetCryptoPrice(string symbol)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync(CoinloreApiUrl);
        var cryptoData = JObject.Parse(response)["data"];

        var currency = cryptoData.FirstOrDefault(c => c["symbol"].Value<string>() == symbol);

        if (currency != null)
        {
            return currency["price_usd"].Value<decimal>();
        }
        return null;
    }
}

public class BuySellRequest
{
    public string Symbol { get; set; }
    public decimal Amount { get; set; }
}
