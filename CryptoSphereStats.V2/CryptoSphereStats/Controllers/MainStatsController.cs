using Microsoft.AspNetCore.Mvc;
using CryptoSphereStats.DataAccess.DataContext;
using CryptoSphereStats.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CryptoSphereStats.Models;
using System.Diagnostics;

namespace CryptoSphereStats.Controllers
{
    public class MainStatsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public MainStatsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult MainStats()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}