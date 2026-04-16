using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HoaDon : BaseEntity
{
    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public DateTime NgayPhatHanh { get; set; }
}
