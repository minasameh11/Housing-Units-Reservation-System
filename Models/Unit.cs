using System.ComponentModel.DataAnnotations;

namespace ProjectName.Models
{
    public class Unit
    {
        public int UnitId { get; set; }

        [Required(ErrorMessage = "اسم الوحدة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم الوحدة يجب ألا يتجاوز 100 حرف")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(1, 100000, ErrorMessage = "السعر يجب أن يكون أكبر من صفر")]
        public decimal PricePerNight { get; set; }

        [Required(ErrorMessage = "الطاقة الاستيعابية مطلوبة")]
        [Range(1, 50, ErrorMessage = "السعة يجب أن تكون بين 1 و 50 شخص")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Url(ErrorMessage = "الرابط يجب أن يكون صحيح")]
        public string? ImagePath { get; set; }

        public ICollection<UnitImage> Images { get; set; } = new List<UnitImage>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
