using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectName.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "رقم الوحدة مطلوب")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        [Required(ErrorMessage = "اسم العميل مطلوب")]
        [StringLength(100, ErrorMessage = "اسم العميل يجب ألا يتجاوز 100 حرف")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Range(0, 1000000, ErrorMessage = "السعر الإجمالي يجب أن يكون أكبر من أو يساوي صفر")]
        [DataType(DataType.Currency)]
        public decimal? TotalPrice { get; set; }

        [Range(0, 1000000, ErrorMessage = "المبلغ المدفوع لا يمكن أن يكون سالباً")]
        public decimal? AmountPaid { get; set; }

        [Range(0, 1000000, ErrorMessage = "المبلغ المتبقي لا يمكن أن يكون سالباً")]
        public decimal? AmountRemaining { get; set; }

        [StringLength(200, ErrorMessage = "الملاحظات لا يمكن أن تتجاوز 200 حرف")]
        public string? Notes { get; set; }

        [StringLength(100, ErrorMessage = "اسم السمسار يجب ألا يتجاوز 100 حرف")]
        public string? BrokerName { get; set; }

        [Phone(ErrorMessage = "رقم هاتف السمسار غير صالح")]
        public string? BrokerPhone { get; set; }
    }
}
