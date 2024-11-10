using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoSphereStats.DataAccess.DataContext;
using CryptoSphereStats.Domain.Models;
using System.Threading.Tasks;

namespace CryptoSphereStats.Controllers
{
    public class DataController : Controller
    {
        private readonly ChartDataContext _context;

        public DataController(ChartDataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> JsonData()
        {
            try
            {
                var chartData = await _context.ChartData.ToListAsync();
                return View(chartData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return View(new List<ChartData>());
            }
        }

        public async Task<IActionResult> GetAllCryptocurrencies()
        {
            var cryptocurrencies = await _context.Cryptocurrencies.ToListAsync();
            return View(cryptocurrencies);
        }


        public async Task<IActionResult> GetAllChartData()
        {
            var chartData = await _context.ChartData
                                          .Include(cd => cd.Cryptocurrency)
                                          .ToListAsync();

            return View(chartData);
        }



        public async Task<IActionResult> GetAllData()
        {
            try
            {
                var cryptocurrencies = await _context.Cryptocurrencies.ToListAsync();
                var chartData = await _context.ChartData
                                              .Include(cd => cd.Cryptocurrency)
                                              .ToListAsync();

                var data = new
                {
                    Cryptocurrencies = cryptocurrencies,
                    ChartData = chartData

                };

                return Json(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
