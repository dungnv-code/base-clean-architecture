#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.NguoiDung;

public class NguoiDungRequestDTO
{
    public string ExternalId { get; set; }
    public string Ten { get; set; }
    public string Email { get; set; }
    public string SoDienThoai { get; set; }
    public string TrangThai { get; set; }
    public string VaiTroNoiBo { get; set; }
}

public class NguoiDungResponseDTO
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Ten { get; set; }
    public string Email { get; set; }
    public string SoDienThoai { get; set; }
    public string TrangThai { get; set; }
    public string VaiTroNoiBo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class NguoiDungQueryDTO : BaseQueryDTO
{
    public string TrangThai { get; set; }
    public string VaiTroNoiBo { get; set; }
}
