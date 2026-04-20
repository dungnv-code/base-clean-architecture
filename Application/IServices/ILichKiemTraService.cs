#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.LichKiemTra;

namespace Application.IServices;

public interface ILichKiemTraService
{
    Task<TResponse<List<LichKiemTraResponseDTO>>> GetListAsync(LichKiemTraQueryDTO query);
    Task<TResponse<LichKiemTraResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<LichKiemTraResponseDTO>> CreateAsync(LichKiemTraRequestDTO request);
    Task<TResponse<LichKiemTraResponseDTO>> UpdateAsync(Guid id, LichKiemTraRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<List<LichKiemTraResponseDTO>>> GetSapDenAsync(int soNgay = 7);
}
