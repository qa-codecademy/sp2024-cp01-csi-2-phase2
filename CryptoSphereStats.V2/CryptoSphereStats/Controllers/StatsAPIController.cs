using Microsoft.AspNetCore.Mvc;
using CryptoSphereStats.DataAccess.DataContext;
using CryptoSphereStats.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryptoSphereStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsAPIController : ControllerBase
    {
        private readonly ChartDataContext _context;
        private readonly ILogger<StatsAPIController> _logger;

        public StatsAPIController(ChartDataContext context, ILogger<StatsAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("chartdata/{cryptocurrencyName}")]
        public IActionResult GetChartData(string cryptocurrencyName)
        {

            if (string.IsNullOrWhiteSpace(cryptocurrencyName))
            {
                _logger.LogWarning("Cryptocurrency name is missing or invalid.");
                return BadRequest("Cryptocurrency name is required.");
            }

            var data = _context.ChartData
                .Where(d => d.Name == cryptocurrencyName)
                .Select(d => new
                {
                    d.Name,
                    d.Month,
                    d.Value
                })
                .ToList();

            if (data.Count == 0)
            {
                _logger.LogInformation("No data found for cryptocurrency: {CryptocurrencyName}", cryptocurrencyName);
                return NotFound(new { message = $"No data found for {cryptocurrencyName}" });
            }

            return Ok(data);
        }

        [HttpGet("cryptocurrencies")]
        public IActionResult GetCryptocurrencies()
        {
            var cryptocurrencies = _context.Cryptocurrencies
                .Select(c => new { c.Name })
                .ToList();

            if (cryptocurrencies.Count == 0)
            {
                _logger.LogInformation("No cryptocurrencies found in the database.");
                return NotFound(new { message = "No cryptocurrencies available." });
            }

            return Ok(cryptocurrencies);
        }

    }
}
