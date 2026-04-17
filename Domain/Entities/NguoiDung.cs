using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class NguoiDung : BaseEntity
{
    public string ExternalId { get; set; } = string.Empty;
    public string Ten { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string SoDienThoai { get; set; }

    public string TrangThai { get; set; } = string.Empty;
    public string VaiTroNoiBo { get; set; } = string.Empty;
}
