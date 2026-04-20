#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.ThuongNhan;

namespace Application.IServices;

public interface IThuongNhanService
{
    Task<TResponse<List<ThuongNhanResponseDTO>>> GetListAsync(ThuongNhanQueryDTO query);
    Task<TResponse<ThuongNhanResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<ThuongNhanResponseDTO>> CreateAsync(ThuongNhanRequestDTO request);
    Task<TResponse<ThuongNhanResponseDTO>> UpdateAsync(Guid id, ThuongNhanRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
}
