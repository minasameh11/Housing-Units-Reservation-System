using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectName.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalUnits = await _context.Units.CountAsync();
            var availableUnits = await _context.Units.CountAsync(u => u.IsAvailable);
            var activeBookings = await _context.Bookings.CountAsync();
            var totalExpenses = await _context.Expenses.SumAsync(e => (decimal?)e.Amount) ?? 0;
            var totalIncome = await _context.Bookings.SumAsync(b => (decimal?)b.TotalPrice) ?? 0;
            var netProfit = totalIncome - totalExpenses;

            ViewBag.TotalUnits = totalUnits;
            ViewBag.AvailableUnits = availableUnits;
            ViewBag.ActiveBookings = activeBookings;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.TotalIncome = totalIncome;
            ViewBag.NetProfit = netProfit;

            return View();
        }
    }
}
