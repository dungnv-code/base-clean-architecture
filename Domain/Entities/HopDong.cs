using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HopDong : BaseEntity
{
    [Required]
    public DateTime NgayBatDau { get; set; }

    [Required]
    public DateTime NgayKetThuc { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaThue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TienCoc { get; set; }

    [Required]
    [MaxLength(50)]
    public string TrangThai { get; set; } = string.Empty;
}
