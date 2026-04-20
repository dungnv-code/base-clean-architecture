#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.HoaDon;

public class HoaDonRequestDTO
{
    public Guid? HopDongId { get; set; }
    public string MaHoaDon { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; }
    public DateTime NgayPhatHanh { get; set; }
    public DateTime? NgayDaoHan { get; set; }
    public string GhiChu { get; set; }
    public List<ChiTietHoaDonRequestDTO> ChiTiet { get; set; } = new();
}

public class ChiTietHoaDonRequestDTO
{
    public Guid? LoaiPhiId { get; set; }
    public string TenKhoanPhi { get; set; }
    public decimal SoLuong { get; set; }
    public decimal DonGia { get; set; }
}

public class HoaDonResponseDTO
{
    public Guid Id { get; set; }
    public Guid? HopDongId { get; set; }
    public string MaHoaDon { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; }
    public DateTime NgayPhatHanh { get; set; }
    public DateTime? NgayDaoHan { get; set; }
    public string GhiChu { get; set; }
    public bool IsQuaHan { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class HoaDonQueryDTO : BaseQueryDTO
{
    public Guid? HopDongId { get; set; }
    public string TrangThai { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }
}

public class PhatHanhHoaDonDTO
{
    public Guid HopDongId { get; set; }
    public DateTime Thang { get; set; }
}
