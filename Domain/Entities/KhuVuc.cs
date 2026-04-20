using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class KhuVuc : BaseEntity
{
    public Guid? ChoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Loai { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? MoTa { get; set; }
}
