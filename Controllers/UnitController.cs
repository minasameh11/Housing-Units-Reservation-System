using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectName.Models;

namespace ProjectName.Controllers
{
    public class UnitController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UnitController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Unit/Index
        public async Task<IActionResult> Index(string search, DateTime? startDate, DateTime? endDate)
        {
            var units = _context.Units.AsQueryable();

            // فلتر البحث بالنص
            if (!string.IsNullOrEmpty(search))
            {
                units = units.Where(u => u.Name.Contains(search));
            }

            // فلتر حسب التواريخ
            if (startDate.HasValue && endDate.HasValue && startDate <= endDate)
            {
                units = units.Where(u => !_context.Bookings
                    .Any(b => b.UnitId == u.UnitId &&
                              b.EndDate > startDate &&
                              b.StartDate < endDate));
            }

            var unitList = await units
                .Include(u => u.Bookings)
                .ToListAsync();

            return View(unitList);
        }


        // عرض تفاصيل وحدة
        // عرض تفاصيل وحدة
        public async Task<IActionResult> Details(int id)
        {
            var unit = await _context.Units
                .Include(u => u.Images)       // جلب الصور
                .Include(u => u.Bookings)     // جلب الحجوزات
                .Include(u => u.Expenses)     // جلب المصروفات
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null)
                return NotFound();

            return View(unit);
        }

        // صفحة الإضافة
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Unit unit, List<IFormFile> Images, int? MainImageIndex)
        {
            if (ModelState.IsValid)
            {
                // اجعل الوحدة متاحة تلقائيًا
                unit.IsAvailable = true;

                _context.Units.Add(unit);
                await _context.SaveChangesAsync();

                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        var image = Images[i];
                        if (image.Length > 0)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var unitImage = new UnitImage
                            {
                                UnitId = unit.UnitId,
                                ImagePath = "/images/" + fileName
                            };

                            if (MainImageIndex.HasValue && MainImageIndex.Value == i)
                            {
                                unit.ImagePath = unitImage.ImagePath;
                            }

                            _context.UnitImages.Add(unitImage);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                _context.Update(unit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Unit unit, List<IFormFile> NewImages, int? MainImageIndex)
        {
            if (id != unit.UnitId) return NotFound();

            if (ModelState.IsValid)
            {
                var existingUnit = await _context.Units
                    .Include(u => u.Images)
                    .FirstOrDefaultAsync(u => u.UnitId == id);

                if (existingUnit == null) return NotFound();

                existingUnit.Name = unit.Name;
                existingUnit.Description = unit.Description;
                existingUnit.PricePerNight = unit.PricePerNight;
                existingUnit.Capacity = unit.Capacity;

                // رفع الصور الجديدة
                if (NewImages != null && NewImages.Count > 0)
                {
                    for (int i = 0; i < NewImages.Count; i++)
                    {
                        var image = NewImages[i];
                        if (image.Length > 0)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var unitImage = new UnitImage
                            {
                                UnitId = existingUnit.UnitId,
                                ImagePath = "/images/" + fileName
                            };

                            if (MainImageIndex.HasValue && MainImageIndex.Value == i)
                            {
                                existingUnit.ImagePath = unitImage.ImagePath;
                            }

                            _context.UnitImages.Add(unitImage);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unit);
        }


        // تعديل وحدة
        public async Task<IActionResult> Edit(int id)
        {
            var unit = await _context.Units
                .Include(u => u.Images)
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null) return NotFound();
            return View(unit);
        }

        


        // حذف صورة من وحدة
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var image = await _context.UnitImages.FindAsync(imageId);
            if (image != null)
            {
                var physicalPath = Path.Combine(_env.WebRootPath, image.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

                _context.UnitImages.Remove(image);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }

        // صفحة تأكيد الحذف
        public async Task<IActionResult> Delete(int id)
        {
            var unit = await _context.Units
                .Include(u => u.Images)
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit == null) return NotFound();
            return View(unit);
        }

        // تنفيذ الحذف
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units
                .Include(u => u.Images)
                .FirstOrDefaultAsync(u => u.UnitId == id);

            if (unit != null)
            {
                // حذف الصور من السيرفر
                foreach (var img in unit.Images)
                {
                    var physicalPath = Path.Combine(_env.WebRootPath, img.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }

                _context.UnitImages.RemoveRange(unit.Images);
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
