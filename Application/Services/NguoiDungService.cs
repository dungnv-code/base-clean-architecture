#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.NguoiDung;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class NguoiDungService : INguoiDungService
{
    private readonly INguoiDungRepository _repo;
    public NguoiDungService(INguoiDungRepository repo) => _repo = repo;

    public async Task<TResponse<List<NguoiDungResponseDTO>>> GetListAsync(NguoiDungQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<NguoiDungResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<NguoiDungResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<NguoiDungResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null ? TResponse<NguoiDungResponseDTO>.FailResponse(NguoiDungMessage.NOT_FOUND, 404) : TResponse<NguoiDungResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<NguoiDungResponseDTO>> GetByExternalIdAsync(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId)) return TResponse<NguoiDungResponseDTO>.FailResponse(CommonMessage.MISSING_PARAM);
        var e = await _repo.GetByExternalIdAsync(externalId.Trim());
        return e == null ? TResponse<NguoiDungResponseDTO>.FailResponse(NguoiDungMessage.NOT_FOUND, 404) : TResponse<NguoiDungResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<NguoiDungResponseDTO>> CreateAsync(NguoiDungRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<NguoiDungResponseDTO>.FailResponse(error);
        try
        {
            if (await _repo.AnyAsync(x => x.ExternalId == request.ExternalId.Trim()))
                return TResponse<NguoiDungResponseDTO>.FailResponse(NguoiDungMessage.EXTERNAL_ID_DUPLICATE);
            var entity = new NguoiDung { ExternalId = request.ExternalId.Trim(), Ten = request.Ten.Trim(), Email = request.Email?.Trim(), SoDienThoai = request.SoDienThoai?.Trim(), TrangThai = request.TrangThai ?? "HoatDong", VaiTroNoiBo = request.VaiTroNoiBo.Trim() };
            return TResponse<NguoiDungResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<NguoiDungResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<NguoiDungResponseDTO>> UpdateAsync(Guid id, NguoiDungRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<NguoiDungResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<NguoiDungResponseDTO>.FailResponse(NguoiDungMessage.NOT_FOUND, 404);
        try
        {
            entity.Ten = request.Ten.Trim(); entity.Email = request.Email?.Trim(); entity.SoDienThoai = request.SoDienThoai?.Trim(); entity.TrangThai = request.TrangThai; entity.VaiTroNoiBo = request.VaiTroNoiBo.Trim();
            return TResponse<NguoiDungResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<NguoiDungResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(NguoiDungMessage.NOT_FOUND, 404);
    }

    private static string Validate(NguoiDungRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.ExternalId)) return NguoiDungMessage.EXTERNAL_ID_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.Ten)) return NguoiDungMessage.TEN_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.VaiTroNoiBo)) return NguoiDungMessage.VAI_TRO_REQUIRED;
        return null;
    }

    private static NguoiDungResponseDTO Map(NguoiDung e) => new()
        { Id = e.Id, ExternalId = e.ExternalId, Ten = e.Ten, Email = e.Email, SoDienThoai = e.SoDienThoai, TrangThai = e.TrangThai, VaiTroNoiBo = e.VaiTroNoiBo, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
