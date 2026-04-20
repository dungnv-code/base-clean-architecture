#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.HopDong;

namespace Application.IServices;

public interface IHopDongService
{
    Task<TResponse<List<HopDongResponseDTO>>> GetListAsync(HopDongQueryDTO query);
    Task<TResponse<HopDongResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<HopDongResponseDTO>> CreateAsync(HopDongRequestDTO request);
    Task<TResponse<HopDongResponseDTO>> UpdateAsync(Guid id, HopDongRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<HopDongResponseDTO>> ChamDutAsync(Guid id, string lyDo);
    Task<TResponse<List<HopDongResponseDTO>>> GetSapHetHanAsync(int soNgay = 30);
    Task<TResponse<List<HopDongResponseDTO>>> GetByKiotAsync(Guid kiotId);
}
