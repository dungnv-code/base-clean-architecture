#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.LichKiemTra;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class LichKiemTraService : ILichKiemTraService
{
    private readonly ILichKiemTraRepository _repo;
    public LichKiemTraService(ILichKiemTraRepository repo) => _repo = repo;

    public async Task<TResponse<List<LichKiemTraResponseDTO>>> GetListAsync(LichKiemTraQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<LichKiemTraResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<LichKiemTraResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<LichKiemTraResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null ? TResponse<LichKiemTraResponseDTO>.FailResponse(LichKiemTraMessage.NOT_FOUND, 404) : TResponse<LichKiemTraResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<LichKiemTraResponseDTO>> CreateAsync(LichKiemTraRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<LichKiemTraResponseDTO>.FailResponse(error);
        try
        {
            var entity = new LichKiemTra { KhuVucId = request.KhuVucId, NguoiPhuTrachId = request.NguoiPhuTrachId, TieuDe = request.TieuDe.Trim(), LoaiKiemTra = request.LoaiKiemTra.Trim(), NgayKiemTra = request.NgayKiemTra, TrangThai = request.TrangThai ?? "ChuaThucHien", GhiChu = request.GhiChu?.Trim() };
            return TResponse<LichKiemTraResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<LichKiemTraResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<LichKiemTraResponseDTO>> UpdateAsync(Guid id, LichKiemTraRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<LichKiemTraResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<LichKiemTraResponseDTO>.FailResponse(LichKiemTraMessage.NOT_FOUND, 404);
        try
        {
            entity.TieuDe = request.TieuDe.Trim(); entity.LoaiKiemTra = request.LoaiKiemTra.Trim(); entity.NgayKiemTra = request.NgayKiemTra; entity.TrangThai = request.TrangThai; entity.GhiChu = request.GhiChu?.Trim();
            return TResponse<LichKiemTraResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<LichKiemTraResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<List<LichKiemTraResponseDTO>>> GetSapDenAsync(int soNgay = 7)
    {
        var items = await _repo.GetSapDenAsync(soNgay);
        return TResponse<List<LichKiemTraResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(LichKiemTraMessage.NOT_FOUND, 404);
    }

    private static string Validate(LichKiemTraRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.TieuDe)) return LichKiemTraMessage.TIEU_DE_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.LoaiKiemTra)) return LichKiemTraMessage.LOAI_REQUIRED;
        if (r.NgayKiemTra == default) return LichKiemTraMessage.NGAY_REQUIRED;
        return null;
    }

    private static LichKiemTraResponseDTO Map(LichKiemTra e) => new()
        { Id = e.Id, KhuVucId = e.KhuVucId, NguoiPhuTrachId = e.NguoiPhuTrachId, TieuDe = e.TieuDe, LoaiKiemTra = e.LoaiKiemTra, NgayKiemTra = e.NgayKiemTra, TrangThai = e.TrangThai, GhiChu = e.GhiChu, KetQua = e.KetQua, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
