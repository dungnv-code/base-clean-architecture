using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class LoaiPhi : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string TenPhi { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string MaPhi { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? DonViTinh { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaMacDinh { get; set; }

    [MaxLength(1000)]
    public string? MoTa { get; set; }

    public bool IsActive { get; set; } = true;
}
