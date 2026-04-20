using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Kiot : BaseEntity
{
    public Guid? KhuVucId { get; set; }

    [Required]
    [MaxLength(50)]
    public string MaKiot { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal DienTich { get; set; }

    [MaxLength(200)]
    public string? ViTri2D { get; set; }

    [MaxLength(200)]
    public string? ViTri3D { get; set; }

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? LoaiKinhDoanh { get; set; }
}
