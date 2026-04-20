#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.KhuVuc;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class KhuVucService : IKhuVucService
{
    private readonly IKhuVucRepository _repo;
    public KhuVucService(IKhuVucRepository repo) => _repo = repo;

    public async Task<TResponse<List<KhuVucResponseDTO>>> GetListAsync(KhuVucQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<KhuVucResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<KhuVucResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<KhuVucResponseDTO>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null
            ? TResponse<KhuVucResponseDTO>.FailResponse(KhuVucMessage.NOT_FOUND, 404)
            : TResponse<KhuVucResponseDTO>.SuccessResponse(Map(entity));
    }

    public async Task<TResponse<KhuVucResponseDTO>> CreateAsync(KhuVucRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<KhuVucResponseDTO>.FailResponse(error);
        try
        {
            var entity = new KhuVuc { ChoId = request.ChoId, Ten = request.Ten.Trim(), Loai = request.Loai.Trim(), MoTa = request.MoTa?.Trim() };
            return TResponse<KhuVucResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<KhuVucResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<KhuVucResponseDTO>> UpdateAsync(Guid id, KhuVucRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<KhuVucResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<KhuVucResponseDTO>.FailResponse(KhuVucMessage.NOT_FOUND, 404);
        try
        {
            entity.ChoId = request.ChoId; entity.Ten = request.Ten.Trim(); entity.Loai = request.Loai.Trim(); entity.MoTa = request.MoTa?.Trim();
            return TResponse<KhuVucResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<KhuVucResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(KhuVucMessage.NOT_FOUND, 404);
    }

    private static string Validate(KhuVucRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.Ten)) return KhuVucMessage.TEN_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.Loai)) return KhuVucMessage.LOAI_REQUIRED;
        return null;
    }

    private static KhuVucResponseDTO Map(KhuVuc e) => new()
        { Id = e.Id, ChoId = e.ChoId, Ten = e.Ten, Loai = e.Loai, MoTa = e.MoTa, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
