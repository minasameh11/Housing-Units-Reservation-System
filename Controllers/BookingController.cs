using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectName.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectName.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // عرض جميع الحجوزات
        public IActionResult Index(string search)
        {
            var bookings = _context.Bookings
                            .Include(b => b.Unit)
                            .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                bookings = bookings.Where(b => b.CustomerName.ToLower().Contains(search)
                                           || b.Unit.Name.ToLower().Contains(search));
            }

            var result = bookings.OrderByDescending(b => b.StartDate).ToList();
            return View(result);
        }


        // تفاصيل حجز معين
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Unit)
                .FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        // GET: إنشاء حجز جديد
        public async Task<IActionResult> Create(int unitId)
        {
            var unit = await _context.Units.FindAsync(unitId);
            if (unit == null) return NotFound();

            var booking = new Booking
            {
                UnitId = unitId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };
            return View(booking);
        }

        // POST: إنشاء حجز جديد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var unit = await _context.Units.FindAsync(booking.UnitId);
            if (unit == null) return NotFound();

            // التحقق من التواريخ
            if (booking.StartDate >= booking.EndDate)
            {
                ModelState.AddModelError("", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية.");
            }

            // التحقق من الحجز المتداخل
            bool isBooked = await _context.Bookings
                .AnyAsync(b => b.UnitId == booking.UnitId &&
                               b.EndDate > booking.StartDate &&
                               b.StartDate < booking.EndDate);

            if (isBooked)
            {
                ModelState.AddModelError("", "هذه الوحدة محجوزة بالفعل في الفترة المحددة.");
            }

            if (!ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Unit", new { id = booking.UnitId });
            }



            return RedirectToAction("Details", "Unit", new { id = booking.UnitId });
        }

        // GET: تعديل حجز
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        // POST: تعديل حجز
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            // التحقق من التواريخ
            if (booking.StartDate >= booking.EndDate)
            {
                ModelState.AddModelError("", "تاريخ النهاية يجب أن يكون بعد تاريخ البداية.");
            }

            // التحقق من الحجز المتداخل (باستثناء الحجز الحالي)
            bool isBooked = await _context.Bookings
                .AnyAsync(b => b.UnitId == booking.UnitId &&
                               b.BookingId != booking.BookingId &&
                               b.EndDate > booking.StartDate &&
                               b.StartDate < booking.EndDate);

            if (isBooked)
            {
                ModelState.AddModelError("", "هذه الوحدة محجوزة بالفعل في هذا التاريخ.");
            }

            if (!ModelState.IsValid)
            {

                try
                {
                    _context.Attach(booking).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.BookingId == booking.BookingId))
                        return NotFound();
                    else
                        throw;
                }
            return RedirectToAction("Details", "Unit", new { id = booking.UnitId });


            }
                return View(booking);

            
        }

        // حذف حجز
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
