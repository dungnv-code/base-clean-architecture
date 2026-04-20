#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.TaiSan;

public class TaiSanRequestDTO
{
    public Guid? KhuVucId { get; set; }
    public Guid? KiotId { get; set; }
    public string Ten { get; set; }
    public string Loai { get; set; }
    public decimal GiaTri { get; set; }
    public string ViTri { get; set; }
    public string QRCode { get; set; }
    public string TrangThai { get; set; }
    public DateTime? NgayMua { get; set; }
    public DateTime? NgayBaoHanh { get; set; }
}

public class TaiSanResponseDTO
{
    public Guid Id { get; set; }
    public Guid? KhuVucId { get; set; }
    public Guid? KiotId { get; set; }
    public string Ten { get; set; }
    public string Loai { get; set; }
    public decimal GiaTri { get; set; }
    public string ViTri { get; set; }
    public string QRCode { get; set; }
    public string TrangThai { get; set; }
    public DateTime? NgayMua { get; set; }
    public DateTime? NgayBaoHanh { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TaiSanQueryDTO : BaseQueryDTO
{
    public Guid? KhuVucId { get; set; }
    public Guid? KiotId { get; set; }
    public string Loai { get; set; }
    public string TrangThai { get; set; }
}
