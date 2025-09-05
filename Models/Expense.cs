using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectName.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "رقم الوحدة مطلوب")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        [Required(ErrorMessage = "الوصف مطلوب")]
        [StringLength(300, ErrorMessage = "الوصف طويل جدًا")]
        public string Description { get; set; }

        [Required(ErrorMessage = "المبلغ مطلوب")]
        [Range(1, 1000000, ErrorMessage = "المبلغ يجب أن يكون أكبر من صفر")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "التاريخ مطلوب")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
