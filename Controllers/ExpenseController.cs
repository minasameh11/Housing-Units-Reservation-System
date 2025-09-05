using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectName.Models;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        // عرض كل المصروفات
        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses
                .Include(e => e.Unit)
                .ToListAsync();
            return View(expenses);
        }

        // GET - إضافة مصروف
        public IActionResult Create(int unitId)
        {
            var unit = _context.Units.Find(unitId);
            if (unit == null) return NotFound();

            var expense = new Expense
            {
                UnitId = unitId,
                Date = DateTime.Now
            };

            return View(expense);
        }

        // POST - إضافة مصروف
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid==false)
            {
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Unit", new { id = expense.UnitId });
            }
            return View(expense);
        }
    }
}
