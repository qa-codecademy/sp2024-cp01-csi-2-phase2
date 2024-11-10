using Microsoft.AspNetCore.Mvc;
using CryptoSphereStats.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoSphereStats.Controllers
{
    public class CryptoStatsController : Controller
    {
        private readonly CryptoService _cryptoService;

        // Constructor with dependency injection of CryptoService
        public CryptoStatsController(CryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        // Action that fetches cryptocurrency data and passes it to the view
        public async Task<IActionResult> CryptoStats()
        {
            // Fetch cryptocurrency data from the service
            List<CryptoData> cryptoData = await _cryptoService.GetCryptoDataAsync();

            // Return the view with the fetched data
            return View(cryptoData);
        }
    }
}
