#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.ThuongNhan;

public class ThuongNhanRequestDTO
{
    public string Ten { get; set; }
    public string SoDienThoai { get; set; }
    public string CCCD { get; set; }
    public string GiayPhepKinhDoanh { get; set; }
    public string DiaChi { get; set; }
}

public class ThuongNhanResponseDTO
{
    public Guid Id { get; set; }
    public string Ten { get; set; }
    public string SoDienThoai { get; set; }
    public string CCCD { get; set; }
    public string GiayPhepKinhDoanh { get; set; }
    public string DiaChi { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ThuongNhanQueryDTO : BaseQueryDTO
{
}
