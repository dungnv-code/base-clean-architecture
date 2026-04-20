#nullable disable

using Application.DTOs.Cho;
using Application.DTOs.EForm;

namespace Application.IServices;

public interface IChoService
{
    Task<TResponse<List<ChoResponseDTO>>> GetListAsync(ChoQueryDTO query);
    Task<TResponse<ChoResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<ChoResponseDTO>> CreateAsync(ChoRequestDTO request);
    Task<TResponse<ChoResponseDTO>> UpdateAsync(Guid id, ChoRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
}
