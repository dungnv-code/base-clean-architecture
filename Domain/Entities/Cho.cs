using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Cho : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Ten { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string DiaChi { get; set; } = string.Empty;
}
