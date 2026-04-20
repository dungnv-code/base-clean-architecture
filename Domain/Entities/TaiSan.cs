using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class TaiSan : BaseEntity
{
    public Guid? KhuVucId { get; set; }
    public Guid? KiotId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Loai { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTri { get; set; }

    [MaxLength(500)]
    public string? ViTri { get; set; }

    [MaxLength(500)]
    public string? QRCode { get; set; }

    [MaxLength(50)]
    public string? TrangThai { get; set; }

    public DateTime? NgayMua { get; set; }

    public DateTime? NgayBaoHanh { get; set; }
}
