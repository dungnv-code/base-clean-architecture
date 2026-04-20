using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class LichKiemTra : BaseEntity
{
    public Guid? KhuVucId { get; set; }
    public Guid? NguoiPhuTrachId { get; set; }

    [Required]
    [MaxLength(200)]
    public string TieuDe { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LoaiKiemTra { get; set; } = string.Empty;

    public DateTime NgayKiemTra { get; set; }

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? GhiChu { get; set; }

    [MaxLength(2000)]
    public string? KetQua { get; set; }
}
