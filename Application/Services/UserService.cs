#nullable disable

using Application.Constants;
using Application.DTOs.User;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    public async Task<TResponse<List<UserResponseDTO>>> GetListAsync(UserQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<UserResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<UserResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<UserResponseDTO>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null
            ? TResponse<UserResponseDTO>.FailResponse(UserMessage.NOT_FOUND, 404)
            : TResponse<UserResponseDTO>.SuccessResponse(Map(entity));
    }

    public async Task<TResponse<UserResponseDTO>> CreateAsync(UserRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<UserResponseDTO>.FailResponse(error);
        try
        {
            var entity = new User { Ten = request.Ten.Trim(), Email = request.Email.Trim(), SoDienThoai = request.SoDienThoai.Trim() };
            return TResponse<UserResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<UserResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<UserResponseDTO>> UpdateAsync(Guid id, UserRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<UserResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<UserResponseDTO>.FailResponse(UserMessage.NOT_FOUND, 404);
        try
        {
            entity.Ten = request.Ten.Trim();
            entity.Email = request.Email.Trim();
            entity.SoDienThoai = request.SoDienThoai.Trim();
            return TResponse<UserResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<UserResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(UserMessage.NOT_FOUND, 404);
    }

    private static string Validate(UserRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.Ten)) return UserMessage.TEN_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.Email)) return UserMessage.EMAIL_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.SoDienThoai)) return UserMessage.SO_DIEN_THAI_REQUIRED;
        return null;
    }

    private static UserResponseDTO Map(User e) => new()
        { Id = e.Id, Ten = e.Ten, Email = e.Email, SoDienThoai = e.SoDienThoai, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}