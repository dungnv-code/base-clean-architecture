using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class TaiSan : BaseEntity
{
    public string Ten { get; set; } = string.Empty;
    public string Loai { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaTri { get; set; }
    public string? ViTri { get; set; }
}
