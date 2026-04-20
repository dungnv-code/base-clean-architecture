#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.ThanhToan;

public class ThanhToanRequestDTO
{
    public Guid? HoaDonId { get; set; }
    public decimal SoTien { get; set; }
    public string PhuongThuc { get; set; }
    public string MaGiaoDich { get; set; }
    public string GhiChu { get; set; }
}

public class ThanhToanResponseDTO
{
    public Guid Id { get; set; }
    public Guid? HoaDonId { get; set; }
    public decimal SoTien { get; set; }
    public string PhuongThuc { get; set; }
    public string TrangThai { get; set; }
    public DateTime NgayThanhToan { get; set; }
    public string MaGiaoDich { get; set; }
    public string GhiChu { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ThanhToanQueryDTO : BaseQueryDTO
{
    public Guid? HoaDonId { get; set; }
    public string PhuongThuc { get; set; }
    public string TrangThai { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }
}
