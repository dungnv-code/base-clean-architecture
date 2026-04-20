using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ChiTietHoaDon : BaseEntity
{
    public Guid? HoaDonId { get; set; }
    public Guid? LoaiPhiId { get; set; }

    [Required]
    [MaxLength(200)]
    public string TenKhoanPhi { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal SoLuong { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; }

    [MaxLength(500)]
    public string? GhiChu { get; set; }
}
