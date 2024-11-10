using Microsoft.AspNetCore.Mvc;
using CryptoSphereStats.DataAccess.DataContext;
using CryptoSphereStats.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class StatsController : Controller
{
    private readonly ChartDataContext _context;

    public StatsController(ChartDataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
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

}
