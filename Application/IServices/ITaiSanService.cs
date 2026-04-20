#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.TaiSan;

namespace Application.IServices;

public interface ITaiSanService
{
    Task<TResponse<List<TaiSanResponseDTO>>> GetListAsync(TaiSanQueryDTO query);
    Task<TResponse<TaiSanResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<TaiSanResponseDTO>> CreateAsync(TaiSanRequestDTO request);
    Task<TResponse<TaiSanResponseDTO>> UpdateAsync(Guid id, TaiSanRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<TaiSanResponseDTO>> GetByQRCodeAsync(string qrCode);
}
