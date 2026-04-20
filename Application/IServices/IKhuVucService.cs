#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.KhuVuc;

namespace Application.IServices;

public interface IKhuVucService
{
    Task<TResponse<List<KhuVucResponseDTO>>> GetListAsync(KhuVucQueryDTO query);
    Task<TResponse<KhuVucResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<KhuVucResponseDTO>> CreateAsync(KhuVucRequestDTO request);
    Task<TResponse<KhuVucResponseDTO>> UpdateAsync(Guid id, KhuVucRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
}
