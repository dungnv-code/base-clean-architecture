using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ThuongNhan : BaseEntity
{
    public string Ten { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string CCCD { get; set; } = string.Empty;
    public string? GiayPhepKinhDoanh { get; set; }
    public string DiaChi { get; set; } = string.Empty;
}
