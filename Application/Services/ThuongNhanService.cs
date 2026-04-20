#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.ThuongNhan;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class ThuongNhanService : IThuongNhanService
{
    private readonly IThuongNhanRepository _repo;
    public ThuongNhanService(IThuongNhanRepository repo) => _repo = repo;

    public async Task<TResponse<List<ThuongNhanResponseDTO>>> GetListAsync(ThuongNhanQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<ThuongNhanResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<ThuongNhanResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<ThuongNhanResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null
            ? TResponse<ThuongNhanResponseDTO>.FailResponse(ThuongNhanMessage.NOT_FOUND, 404)
            : TResponse<ThuongNhanResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<ThuongNhanResponseDTO>> CreateAsync(ThuongNhanRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<ThuongNhanResponseDTO>.FailResponse(error);
        try
        {
            if (await _repo.AnyAsync(x => x.CCCD == request.CCCD.Trim()))
                return TResponse<ThuongNhanResponseDTO>.FailResponse(ThuongNhanMessage.CCCD_DUPLICATE);

            var entity = new ThuongNhan { Ten = request.Ten.Trim(), SoDienThoai = request.SoDienThoai.Trim(), CCCD = request.CCCD.Trim(), GiayPhepKinhDoanh = request.GiayPhepKinhDoanh?.Trim(), DiaChi = request.DiaChi?.Trim() };
            return TResponse<ThuongNhanResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<ThuongNhanResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<ThuongNhanResponseDTO>> UpdateAsync(Guid id, ThuongNhanRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<ThuongNhanResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<ThuongNhanResponseDTO>.FailResponse(ThuongNhanMessage.NOT_FOUND, 404);
        try
        {
            entity.Ten = request.Ten.Trim(); entity.SoDienThoai = request.SoDienThoai.Trim(); entity.DiaChi = request.DiaChi?.Trim(); entity.GiayPhepKinhDoanh = request.GiayPhepKinhDoanh?.Trim();
            return TResponse<ThuongNhanResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<ThuongNhanResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(ThuongNhanMessage.NOT_FOUND, 404);
    }

    private static string Validate(ThuongNhanRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.Ten)) return ThuongNhanMessage.TEN_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.SoDienThoai)) return ThuongNhanMessage.SDT_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.CCCD)) return ThuongNhanMessage.CCCD_REQUIRED;
        return null;
    }

    private static ThuongNhanResponseDTO Map(ThuongNhan e) => new()
        { Id = e.Id, Ten = e.Ten, SoDienThoai = e.SoDienThoai, CCCD = e.CCCD, GiayPhepKinhDoanh = e.GiayPhepKinhDoanh, DiaChi = e.DiaChi, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
