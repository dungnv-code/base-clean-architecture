using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ThanhToan : BaseEntity
{
    public Guid? HoaDonId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SoTien { get; set; }

    [Required]
    [MaxLength(100)]
    public string PhuongThuc { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;

    public DateTime NgayThanhToan { get; set; }

    [MaxLength(200)]
    public string? MaGiaoDich { get; set; }

    [MaxLength(1000)]
    public string? GhiChu { get; set; }
}
