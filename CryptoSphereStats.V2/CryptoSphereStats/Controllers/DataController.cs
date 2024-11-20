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

<<<<<<< HEAD
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

=======
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
        public async Task<IActionResult> GetAllCryptocurrencies()
        {
            var cryptocurrencies = await _context.Cryptocurrencies.ToListAsync();
            return View(cryptocurrencies);
        }

<<<<<<< HEAD

=======
    
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
        public async Task<IActionResult> GetAllChartData()
        {
            var chartData = await _context.ChartData
                                          .Include(cd => cd.Cryptocurrency)
                                          .ToListAsync();

<<<<<<< HEAD
            return View(chartData);
        }



=======
            return View(chartData);  
        }


 
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
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
<<<<<<< HEAD

                };

                return Json(data);
=======
   
                };

                return Json(data); 
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
<<<<<<< HEAD


=======
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
    }
}
