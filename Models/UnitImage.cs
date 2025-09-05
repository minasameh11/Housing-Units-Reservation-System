using ProjectName.Models;
using System.ComponentModel.DataAnnotations;

public class UnitImage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "مسار الصورة مطلوب")]
    [StringLength(300, ErrorMessage = "مسار الصورة طويل جدًا")]
    public string ImagePath { get; set; }

    [Required(ErrorMessage = "رقم الوحدة مطلوب")]
    public int UnitId { get; set; }
    public Unit Unit { get; set; }
}
