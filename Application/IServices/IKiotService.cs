#nullable disable

using Application.DTOs.EForm;
using Application.DTOs.Kiot;

namespace Application.IServices;

public interface IKiotService
{
    Task<TResponse<List<KiotResponseDTO>>> GetListAsync(KiotQueryDTO query);
    Task<TResponse<KiotResponseDTO>> GetByIdAsync(Guid id);
    Task<TResponse<KiotResponseDTO>> CreateAsync(KiotRequestDTO request);
    Task<TResponse<KiotResponseDTO>> UpdateAsync(Guid id, KiotRequestDTO request);
    Task<TResponse<bool>> DeleteAsync(Guid id);
    Task<TResponse<KiotResponseDTO>> CapNhatTrangThaiAsync(Guid id, string trangThai);
    Task<TResponse<List<KiotResponseDTO>>> GetByKhuVucAsync(Guid khuVucId);
    Task<TResponse<List<KiotResponseDTO>>> GetSapHetHanAsync(int soNgay = 30);
}
