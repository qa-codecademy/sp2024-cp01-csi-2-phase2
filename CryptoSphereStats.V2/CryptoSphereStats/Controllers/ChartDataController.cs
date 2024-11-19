using Microsoft.AspNetCore.Mvc;
using CryptoSphereStats.DataAccess.DataContext;
using CryptoSphereStats.Models;
using System.Linq;

namespace CryptoSphereStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChartDataController : ControllerBase
    {
        private readonly ChartDataContext _context;

        public ChartDataController(ChartDataContext context)
        {
            _context = context;
        }

        [HttpGet("chartdata/{cryptocurrencyName}")]
        public IActionResult GetChartData(string cryptocurrencyName)
        {
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
                return NotFound();
            }

            return Ok(data);
        }

        [HttpGet("cryptocurrencies")]
        public IActionResult GetCryptocurrencies()
        {
            var cryptocurrencies = _context.Cryptocurrencies
                .Select(c => new { c.Name })
                .ToList();

            return Ok(cryptocurrencies);
        }
    }
}
