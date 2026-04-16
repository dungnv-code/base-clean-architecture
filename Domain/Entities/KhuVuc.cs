using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class KhuVuc : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Loai { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? MoTa { get; set; }
}
