#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.HopDong;

public class HopDongRequestDTO
{
    public Guid? KiotId { get; set; }
    public Guid? ThuongNhanId { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public decimal GiaThue { get; set; }
    public decimal TienCoc { get; set; }
    public string TrangThai { get; set; }
    public string GhiChu { get; set; }
}

public class HopDongResponseDTO
{
    public Guid Id { get; set; }
    public Guid? KiotId { get; set; }
    public Guid? ThuongNhanId { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public decimal GiaThue { get; set; }
    public decimal TienCoc { get; set; }
    public string TrangThai { get; set; }
    public string GhiChu { get; set; }
    public int SoNgayConLai { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class HopDongQueryDTO : BaseQueryDTO
{
    public Guid? KiotId { get; set; }
    public Guid? ThuongNhanId { get; set; }
    public string TrangThai { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }
}

public class ChamDutHopDongDTO
{
    public string LyDo { get; set; }
}
