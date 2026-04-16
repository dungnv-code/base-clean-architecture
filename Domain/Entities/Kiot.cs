using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Kiot : BaseEntity
{
    public string MaKiot { get; set; } = string.Empty;
    [Column(TypeName = "decimal(10,2)")]
    public decimal DienTich { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public string LoaiKinhDoanh { get; set; } = string.Empty;
}
