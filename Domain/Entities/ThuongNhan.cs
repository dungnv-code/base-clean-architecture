using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ThuongNhan : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Phone]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CCCD { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? GiayPhepKinhDoanh { get; set; }

    [Required]
    [MaxLength(500)]
    public string DiaChi { get; set; } = string.Empty;
}
