#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.LichKiemTra;

public class LichKiemTraRequestDTO
{
    public Guid? KhuVucId { get; set; }
    public Guid? NguoiPhuTrachId { get; set; }
    public string TieuDe { get; set; }
    public string LoaiKiemTra { get; set; }
    public DateTime NgayKiemTra { get; set; }
    public string TrangThai { get; set; }
    public string GhiChu { get; set; }
}

public class LichKiemTraResponseDTO
{
    public Guid Id { get; set; }
    public Guid? KhuVucId { get; set; }
    public Guid? NguoiPhuTrachId { get; set; }
    public string TieuDe { get; set; }
    public string LoaiKiemTra { get; set; }
    public DateTime NgayKiemTra { get; set; }
    public string TrangThai { get; set; }
    public string GhiChu { get; set; }
    public string KetQua { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class LichKiemTraQueryDTO : BaseQueryDTO
{
    public Guid? KhuVucId { get; set; }
    public string LoaiKiemTra { get; set; }
    public string TrangThai { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }
}
