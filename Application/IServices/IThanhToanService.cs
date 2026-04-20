#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.ThanhToan;

namespace Application.IServices;

public interface IThanhToanService
{
    Task<TResponse<List<ThanhToanResponseDTO>>> GetListAsync(ThanhToanQueryDTO query);
    Task<TResponse<ThanhToanResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<ThanhToanResponseDTO>> XuLyThanhToanAsync(ThanhToanRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<List<ThanhToanResponseDTO>>> GetByHoaDonAsync(Guid hoaDonId);
}
