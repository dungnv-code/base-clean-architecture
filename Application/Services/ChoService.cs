#nullable disable

using Application.Constants;
using Application.DTOs.Cho;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class ChoService : IChoService
{
    private readonly IChoRepository _repo;
    public ChoService(IChoRepository repo) => _repo = repo;

    public async Task<TResponse<List<ChoResponseDTO>>> GetListAsync(ChoQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<ChoResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<ChoResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<ChoResponseDTO>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null
            ? TResponse<ChoResponseDTO>.FailResponse(ChoMessage.NOT_FOUND, 404)
            : TResponse<ChoResponseDTO>.SuccessResponse(Map(entity));
    }

    public async Task<TResponse<ChoResponseDTO>> CreateAsync(ChoRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<ChoResponseDTO>.FailResponse(error);
        try
        {
            var entity = new Cho { Ten = request.Ten.Trim(), DiaChi = request.DiaChi.Trim() };
            return TResponse<ChoResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<ChoResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<ChoResponseDTO>> UpdateAsync(Guid id, ChoRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<ChoResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<ChoResponseDTO>.FailResponse(ChoMessage.NOT_FOUND, 404);
        try
        {
            entity.Ten = request.Ten.Trim();
            entity.DiaChi = request.DiaChi.Trim();
            return TResponse<ChoResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<ChoResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(ChoMessage.NOT_FOUND, 404);
    }

    private static string Validate(ChoRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.Ten)) return ChoMessage.TEN_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.DiaChi)) return ChoMessage.DIA_CHI_REQUIRED;
        return null;
    }

    private static ChoResponseDTO Map(Cho e) => new()
        { Id = e.Id, Ten = e.Ten, DiaChi = e.DiaChi, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
