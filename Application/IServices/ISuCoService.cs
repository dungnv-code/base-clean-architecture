#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.SuCo;

namespace Application.IServices;

public interface ISuCoService
{
    Task<TResponse<List<SuCoResponseDTO>>> GetListAsync(SuCoQueryDTO query);
    Task<TResponse<SuCoResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<SuCoResponseDTO>> TiepNhanAsync(SuCoRequestDTO request);
    Task<TResponse<SuCoResponseDTO>> UpdateAsync(Guid id, SuCoRequestDTO request);
    Task<TResponse<SuCoResponseDTO>> CapNhatTrangThaiAsync(Guid id, string trangThai, string ketQua);
    Task<TResponse<bool>> DeleteAsync(Guid id);
}
