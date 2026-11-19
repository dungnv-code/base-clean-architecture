#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.User;
using Application.Helpers;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRepository<RefreshToken> _refreshTokenRepo;
    private readonly IRepository<UserRole> _userRoleRepository;
    public UserService(IUserRepository repo, IUserRoleRepository userRoleRepo, IJwtTokenService jwtTokenService, IRepository<RefreshToken> refreshTokenRepo, IRepository<UserRole> userRoleRepository){
        _repo = repo;
        _userRoleRepo = userRoleRepo;
        _jwtTokenService=jwtTokenService;
        _refreshTokenRepo=refreshTokenRepo;
        _userRoleRepository=userRoleRepository;
    }

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
        var pwHash = HashPass.HashPassword(request.Password.Trim());
        if (error != null) return TResponse<UserResponseDTO>.FailResponse(error);
        try
        {
            var entity = new User { Name = request.Name.Trim(), Email = request.Email.Trim(), PhoneNumber = request.PhoneNumber.Trim(), Address = request.Address.Trim(), Avatar = request.Avatar.Trim(), Password = pwHash.Trim() };
            var userRole= new UserRole { UserId=entity.Id, RoleId=Guid.Parse(request.IdRole) };
            var newUser = await _repo.CreateAsync(entity);
            await _userRoleRepository.CreateAsync(userRole);
            return TResponse<UserResponseDTO>.SuccessResponse(Map(newUser));
        }
        catch { return TResponse<UserResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<UserResponseDTO>> UpdateAsync(Guid id, UserUpdateRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<UserResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<UserResponseDTO>.FailResponse(UserMessage.NOT_FOUND, 404);
        try
        {
            entity.Name = request.Name.Trim();
            entity.Email = request.Email.Trim();
            entity.PhoneNumber = request.PhoneNumber.Trim();
            entity.Address = request.Address.Trim();
            entity.Avatar = request.Avatar.Trim();
            return TResponse<UserResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<UserResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(UserMessage.NOT_FOUND, 404);
    }

    public async Task<TResponse<UserLoginReponseDTO>> LoginAsync(UserLoginDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return TResponse<UserLoginReponseDTO>.FailResponse(UserMessage.NOT_FOUND, 400);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return TResponse<UserLoginReponseDTO>.FailResponse("Thiếu mật khẩu!", 400);
        }

        var User = await _repo.GetByEmailAsync(request.Email);
        if (User == null || User.IsDeleted) return TResponse<UserLoginReponseDTO>.FailResponse(UserMessage.NOT_FOUND, 401);

        if (!HashPass.VerifyPassword(User.Password, request.Password))
        {
            return TResponse<UserLoginReponseDTO>.FailResponse(UserMessage.NOT_PASS, 401);
        }
        var roles = await _userRoleRepo.GetRolesByUserIdAsync(User.Id);
        var accessToken=  _jwtTokenService.GenerateAccessToken(User, roles.Select(r=>r.Code));
        var refreshToken= _jwtTokenService.GenerateRefreshToken(User.Id, request.IpAddress);
        await _refreshTokenRepo.CreateAsync(refreshToken);
        var testrole=roles.Select(r=>r.Code);
        Console.WriteLine($"Role: {string.Join(", ", testrole)}");
        return TResponse<UserLoginReponseDTO>.SuccessResponse(new UserLoginReponseDTO
        {
            AccessToken = accessToken,
            User = Map(User)
        });
    }

    private static string Validate(UserBaseDTO r)
    {
        var fields = new (string Value, string Message)[]
        {
            (r.Name, UserMessage.NAME_REQUIRED),
            (r.Email, UserMessage.EMAIL_REQUIRED),
            (r.PhoneNumber, UserMessage.PHONE_NUMBER_REQUIRED),
            (r.Address, UserMessage.ADDRESS_REQUIRED),
            (r.Avatar, UserMessage.AVATAR_REQUIRED),
        };
        foreach (var (value, message) in fields)
            if (string.IsNullOrWhiteSpace(value)) return message;
        return null;
    }

    private static UserResponseDTO Map(User e) => new()
    { Id = e.Id, Name = e.Name, Email = e.Email, PhoneNumber = e.PhoneNumber, Address = e.Address, Avatar = e.Avatar, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };

}
