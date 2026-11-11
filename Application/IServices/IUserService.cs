#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.User;

namespace Application.IServices;

public interface IUserService
{
    Task<TResponse<List<UserResponseDTO>>> GetListAsync(UserQueryDTO query);
    Task<TResponse<UserResponseDTO>> GetByIdAsync(Guid id);

    Task<TResponse<UserResponseDTO>> CreateAsync(UserRequestDTO request);
    Task<TResponse<UserResponseDTO>> UpdateAsync(Guid id, UserUpdateRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<UserLoginReponseDTO>> LoginAsync(UserLoginDTO request);
}
