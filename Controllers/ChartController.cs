using CryptoWalletAPI.Services;
using Microsoft.AspNetCore.Mvc;
using ScottPlot;

[ApiController]
[Route("api/[controller]")]
public class ChartController : ControllerBase
{
    private readonly IWalletService _walletService;

    public ChartController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet("crypto-chart")]
    public async Task<IActionResult> GetCryptoChart()
    {

        var cryptoData = await _walletService.GetCryptoDataAsync();
        var topCryptos = cryptoData.Take(5).ToList(); 


        var prices = topCryptos.Select(c => c.PriceUSD).ToArray();
        var names = topCryptos.Select(c => c.Name).ToArray();
        var percentChange1h = topCryptos.Select( x => x.PercentChange1h ).ToArray();
        var percentChange24h = topCryptos.Select(x => x.PercentChange24h).ToArray();
        var percentChange7d = topCryptos.Select(x => x.PercentChange7d).ToArray();

        return Ok(new
        {
            prices,
            names,
            percentChange1h,
            percentChange24h,
            percentChange7d
        });

    }
}
