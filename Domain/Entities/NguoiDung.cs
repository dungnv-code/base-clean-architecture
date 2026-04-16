using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class NguoiDung : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string ExternalId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    [Phone]
    public string? SoDienThoai { get; set; }

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string VaiTroNoiBo { get; set; } = string.Empty;
}
