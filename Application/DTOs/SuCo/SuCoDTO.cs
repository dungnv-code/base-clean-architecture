#nullable disable

using Application.DTOs.Common;

namespace Application.DTOs.SuCo;

public class SuCoRequestDTO
{
    public Guid? KiotId { get; set; }
    public Guid? TaiSanId { get; set; }
    public Guid? NguoiXuLyId { get; set; }
    public string MucDo { get; set; }
    public string MoTa { get; set; }
    public string TrangThai { get; set; }
}

public class SuCoResponseDTO
{
    public Guid Id { get; set; }
    public Guid? KiotId { get; set; }
    public Guid? TaiSanId { get; set; }
    public Guid? NguoiXuLyId { get; set; }
    public Guid? NguoiBaoCaoId { get; set; }
    public string MucDo { get; set; }
    public string MoTa { get; set; }
    public string TrangThai { get; set; }
    public DateTime? NgayXuLy { get; set; }
    public string KetQuaXuLy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SuCoQueryDTO : BaseQueryDTO
{
    public Guid? KiotId { get; set; }
    public string MucDo { get; set; }
    public string TrangThai { get; set; }
}

public class CapNhatTrangThaiSuCoDTO
{
    public string TrangThai { get; set; }
    public string KetQua { get; set; }
}
