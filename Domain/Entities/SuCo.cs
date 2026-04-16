using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class SuCo : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string MucDo { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string MoTa { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;
}
