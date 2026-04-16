using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ThanhToan : BaseEntity
{
    [Column(TypeName = "decimal(18,2)")]
    public decimal SoTien { get; set; }
    public string PhuongThuc { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public DateTime NgayThanhToan { get; set; }
}
