#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.SuCo;
using Application.IRepositories;
using Application.IServices;
using Domain.Constants;
using Domain.Entities;

namespace Application.Services;

public class SuCoService : ISuCoService
{
    private readonly ISuCoRepository _repo;
    public SuCoService(ISuCoRepository repo) => _repo = repo;

    public async Task<TResponse<List<SuCoResponseDTO>>> GetListAsync(SuCoQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<SuCoResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<SuCoResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<SuCoResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null ? TResponse<SuCoResponseDTO>.FailResponse(SuCoMessage.NOT_FOUND, 404) : TResponse<SuCoResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<SuCoResponseDTO>> TiepNhanAsync(SuCoRequestDTO request)
    {
        var error = ValidateTiepNhan(request);
        if (error != null) return TResponse<SuCoResponseDTO>.FailResponse(error);
        try
        {
            var entity = new SuCo { KiotId = request.KiotId, TaiSanId = request.TaiSanId, NguoiXuLyId = request.NguoiXuLyId, MucDo = request.MucDo.Trim(), MoTa = request.MoTa.Trim(), TrangThai = TrangThaiSuCoConst.MoiTiepNhan };
            return TResponse<SuCoResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<SuCoResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<SuCoResponseDTO>> UpdateAsync(Guid id, SuCoRequestDTO request)
    {
        var error = ValidateTiepNhan(request);
        if (error != null) return TResponse<SuCoResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<SuCoResponseDTO>.FailResponse(SuCoMessage.NOT_FOUND, 404);
        try
        {
            entity.MucDo = request.MucDo.Trim(); entity.MoTa = request.MoTa.Trim(); entity.NguoiXuLyId = request.NguoiXuLyId;
            return TResponse<SuCoResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<SuCoResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<SuCoResponseDTO>> CapNhatTrangThaiAsync(Guid id, string trangThai, string ketQua)
    {
        if (string.IsNullOrWhiteSpace(trangThai)) return TResponse<SuCoResponseDTO>.FailResponse(SuCoMessage.TRANG_THAI_REQUIRED);
        if (trangThai == TrangThaiSuCoConst.HoanThanh && string.IsNullOrWhiteSpace(ketQua))
            return TResponse<SuCoResponseDTO>.FailResponse(SuCoMessage.KET_QUA_REQUIRED);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<SuCoResponseDTO>.FailResponse(SuCoMessage.NOT_FOUND, 404);
        try
        {
            entity.TrangThai = trangThai; entity.KetQuaXuLy = ketQua?.Trim();
            if (trangThai == TrangThaiSuCoConst.HoanThanh) entity.NgayXuLy = DateTime.UtcNow;
            return TResponse<SuCoResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<SuCoResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(SuCoMessage.NOT_FOUND, 404);
    }

    private static string ValidateTiepNhan(SuCoRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.MucDo)) return SuCoMessage.MUC_DO_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.MoTa)) return SuCoMessage.MO_TA_REQUIRED;
        return null;
    }

    private static SuCoResponseDTO Map(SuCo e) => new()
        { Id = e.Id, KiotId = e.KiotId, TaiSanId = e.TaiSanId, NguoiXuLyId = e.NguoiXuLyId, NguoiBaoCaoId = e.NguoiBaoCaoId, MucDo = e.MucDo, MoTa = e.MoTa, TrangThai = e.TrangThai, NgayXuLy = e.NgayXuLy, KetQuaXuLy = e.KetQuaXuLy, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
