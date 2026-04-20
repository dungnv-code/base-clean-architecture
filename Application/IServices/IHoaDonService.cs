#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.HoaDon;

namespace Application.IServices;

public interface IHoaDonService
{
    Task<TResponse<List<HoaDonResponseDTO>>> GetListAsync(HoaDonQueryDTO query);
    Task<TResponse<HoaDonResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<HoaDonResponseDTO>> CreateAsync(HoaDonRequestDTO request);
    Task<TResponse<HoaDonResponseDTO>> UpdateAsync(Guid id, HoaDonRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<HoaDonResponseDTO>> PhatHanhAsync(Guid hopDongId, DateTime thang);
    Task<TResponse<List<HoaDonResponseDTO>>> GetQuaHanAsync();
}
