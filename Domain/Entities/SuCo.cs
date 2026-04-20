using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class SuCo : BaseEntity
{
    public Guid? KiotId { get; set; }
    public Guid? TaiSanId { get; set; }
    public Guid? NguoiXuLyId { get; set; }
    public Guid? NguoiBaoCaoId { get; set; }

    [Required]
    [MaxLength(50)]
    public string MucDo { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string MoTa { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;

    public DateTime? NgayXuLy { get; set; }

    [MaxLength(2000)]
    public string? KetQuaXuLy { get; set; }
}
