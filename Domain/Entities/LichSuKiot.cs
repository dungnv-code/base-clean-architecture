using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class LichSuKiot : BaseEntity
{
    public Guid? KiotId { get; set; }
    public Guid? ThuongNhanCuId { get; set; }
    public Guid? ThuongNhanMoiId { get; set; }
    public Guid? HopDongCuId { get; set; }
    public Guid? HopDongMoiId { get; set; }

    [Required]
    [MaxLength(50)]
    public string LoaiBienDong { get; set; } = string.Empty;

    public DateTime NgayBanGiao { get; set; }

    [MaxLength(2000)]
    public string? LyDo { get; set; }

    [MaxLength(2000)]
    public string? GhiChu { get; set; }
}
