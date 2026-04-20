#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.ThanhToan;
using Application.IRepositories;
using Application.IServices;
using Domain.Constants;
using Domain.Entities;

namespace Application.Services;

public class ThanhToanService : IThanhToanService
{
    private readonly IThanhToanRepository _repo;
    private readonly IHoaDonRepository _hoaDonRepo;

    public ThanhToanService(IThanhToanRepository repo, IHoaDonRepository hoaDonRepo)
    {
        _repo = repo; _hoaDonRepo = hoaDonRepo;
    }

    public async Task<TResponse<List<ThanhToanResponseDTO>>> GetListAsync(ThanhToanQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<ThanhToanResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<ThanhToanResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<ThanhToanResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null
            ? TResponse<ThanhToanResponseDTO>.FailResponse(ThanhToanMessage.NOT_FOUND, 404)
            : TResponse<ThanhToanResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<ThanhToanResponseDTO>> XuLyThanhToanAsync(ThanhToanRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<ThanhToanResponseDTO>.FailResponse(error);
        try
        {
            if (request.HoaDonId.HasValue)
            {
                var hoaDon = await _hoaDonRepo.GetByIdAsync(request.HoaDonId.Value);
                if (hoaDon == null) return TResponse<ThanhToanResponseDTO>.FailResponse(HoaDonMessage.NOT_FOUND, 404);
                if (hoaDon.TrangThai == TrangThaiHoaDonConst.DaThanhToan)
                    return TResponse<ThanhToanResponseDTO>.FailResponse(ThanhToanMessage.HOA_DON_DA_THANH_TOAN);

                var entity = new ThanhToan
                {
                    HoaDonId = request.HoaDonId, SoTien = request.SoTien,
                    PhuongThuc = request.PhuongThuc.Trim(), TrangThai = "ThanhCong",
                    NgayThanhToan = DateTime.UtcNow, MaGiaoDich = request.MaGiaoDich?.Trim(), GhiChu = request.GhiChu?.Trim()
                };
                var created = await _repo.CreateAsync(entity);
                hoaDon.TrangThai = TrangThaiHoaDonConst.DaThanhToan;
                await _hoaDonRepo.UpdateAsync(hoaDon);
                return TResponse<ThanhToanResponseDTO>.SuccessResponse(Map(created));
            }
            var entityNoHD = new ThanhToan
            {
                SoTien = request.SoTien, PhuongThuc = request.PhuongThuc.Trim(),
                TrangThai = "ThanhCong", NgayThanhToan = DateTime.UtcNow,
                MaGiaoDich = request.MaGiaoDich?.Trim(), GhiChu = request.GhiChu?.Trim()
            };
            return TResponse<ThanhToanResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entityNoHD)));
        }
        catch { return TResponse<ThanhToanResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<List<ThanhToanResponseDTO>>> GetByHoaDonAsync(Guid hoaDonId)
    {
        var items = await _repo.GetByHoaDonAsync(hoaDonId);
        return TResponse<List<ThanhToanResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(ThanhToanMessage.NOT_FOUND, 404);
    }

    private static string Validate(ThanhToanRequestDTO r)
    {
        if (r.SoTien <= 0) return ThanhToanMessage.SO_TIEN_INVALID;
        if (string.IsNullOrWhiteSpace(r.PhuongThuc)) return ThanhToanMessage.PHUONG_THUC_REQUIRED;
        return null;
    }

    private static ThanhToanResponseDTO Map(ThanhToan e) => new()
        { Id = e.Id, HoaDonId = e.HoaDonId, SoTien = e.SoTien, PhuongThuc = e.PhuongThuc, TrangThai = e.TrangThai, NgayThanhToan = e.NgayThanhToan, MaGiaoDich = e.MaGiaoDich, GhiChu = e.GhiChu, CreatedAt = e.CreatedAt };
}
