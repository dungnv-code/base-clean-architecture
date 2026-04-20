#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.NguoiDung;

namespace Application.IServices;

public interface INguoiDungService
{
    Task<TResponse<List<NguoiDungResponseDTO>>> GetListAsync(NguoiDungQueryDTO query);
    Task<TResponse<NguoiDungResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<NguoiDungResponseDTO>> GetByExternalIdAsync(string externalId);
    Task<TResponse<NguoiDungResponseDTO>> CreateAsync(NguoiDungRequestDTO request);
    Task<TResponse<NguoiDungResponseDTO>> UpdateAsync(Guid id, NguoiDungRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
}
