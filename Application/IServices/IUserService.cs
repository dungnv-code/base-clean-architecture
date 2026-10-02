#nullable disable

using Application.DTOs.User;
using Application.DTOs.EForm;

namespace Application.IServices;

public interface IUserService
{
    Task<TResponse<List<UserResponseDTO>>> GetListAsync(UserQueryDTO query);
    Task<TResponse<UserResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<UserResponseDTO>> CreateAsync(UserRequestDTO request);
    Task<TResponse<UserResponseDTO>> UpdateAsync(Guid id, UserRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
}