#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.Kiot;

public class KiotRequestDTO
{
    public Guid? KhuVucId { get; set; }
    public string MaKiot { get; set; }
    public decimal DienTich { get; set; }
    public string ViTri2D { get; set; }
    public string ViTri3D { get; set; }
    public string TrangThai { get; set; }
    public string LoaiKinhDoanh { get; set; }
}

public class KiotResponseDTO
{
    public Guid Id { get; set; }
    public Guid? KhuVucId { get; set; }
    public string MaKiot { get; set; }
    public decimal DienTich { get; set; }
    public string ViTri2D { get; set; }
    public string ViTri3D { get; set; }
    public string TrangThai { get; set; }
    public string LoaiKinhDoanh { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class KiotQueryDTO : BaseQueryDTO
{
    public Guid? KhuVucId { get; set; }
    public string TrangThai { get; set; }
    public string LoaiKinhDoanh { get; set; }
}
