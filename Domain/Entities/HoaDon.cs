using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HoaDon : BaseEntity
{
    public Guid? HopDongId { get; set; }

    [MaxLength(50)]
    public string? MaHoaDon { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTien { get; set; }

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;

    public DateTime NgayPhatHanh { get; set; }

    public DateTime? NgayDaoHan { get; set; }

    [MaxLength(2000)]
    public string? GhiChu { get; set; }
}
