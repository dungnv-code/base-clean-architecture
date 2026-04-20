#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.HoaDon;
using Application.IRepositories;
using Application.IServices;
using Domain.Constants;
using Domain.Entities;

namespace Application.Services;

public class HoaDonService : IHoaDonService
{
    private readonly IHoaDonRepository _repo;
    private readonly IRepository<ChiTietHoaDon> _chiTietRepo;
    private readonly IHopDongRepository _hopDongRepo;

    public HoaDonService(IHoaDonRepository repo, IRepository<ChiTietHoaDon> chiTietRepo, IHopDongRepository hopDongRepo)
    {
        _repo = repo; _chiTietRepo = chiTietRepo; _hopDongRepo = hopDongRepo;
    }

    public async Task<TResponse<List<HoaDonResponseDTO>>> GetListAsync(HoaDonQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<HoaDonResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<HoaDonResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<HoaDonResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null
            ? TResponse<HoaDonResponseDTO>.FailResponse(HoaDonMessage.NOT_FOUND, 404)
            : TResponse<HoaDonResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<HoaDonResponseDTO>> CreateAsync(HoaDonRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<HoaDonResponseDTO>.FailResponse(error);
        try
        {
            var entity = new HoaDon
            {
                HopDongId = request.HopDongId,
                MaHoaDon = string.IsNullOrWhiteSpace(request.MaHoaDon) ? $"HD{DateTime.UtcNow:yyyyMMddHHmmss}" : request.MaHoaDon.Trim(),
                TongTien = request.TongTien, TrangThai = TrangThaiHoaDonConst.ChuaThanhToan,
                NgayPhatHanh = request.NgayPhatHanh, NgayDaoHan = request.NgayDaoHan, GhiChu = request.GhiChu?.Trim()
            };
            var created = await _repo.CreateAsync(entity);
            if (request.ChiTiet?.Count > 0)
            {
                var chiTiets = request.ChiTiet.Select(ct => new ChiTietHoaDon
                {
                    HoaDonId = created.Id, LoaiPhiId = ct.LoaiPhiId, TenKhoanPhi = ct.TenKhoanPhi?.Trim(),
                    SoLuong = ct.SoLuong, DonGia = ct.DonGia, ThanhTien = ct.SoLuong * ct.DonGia
                }).ToList();
                await _chiTietRepo.CreateRangeAsync(chiTiets);
                created.TongTien = chiTiets.Sum(x => x.ThanhTien);
                await _repo.UpdateAsync(created);
            }
            return TResponse<HoaDonResponseDTO>.SuccessResponse(Map(created));
        }
        catch { return TResponse<HoaDonResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<HoaDonResponseDTO>> UpdateAsync(Guid id, HoaDonRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<HoaDonResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<HoaDonResponseDTO>.FailResponse(HoaDonMessage.NOT_FOUND, 404);
        try
        {
            entity.TongTien = request.TongTien; entity.NgayDaoHan = request.NgayDaoHan; entity.GhiChu = request.GhiChu?.Trim();
            return TResponse<HoaDonResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<HoaDonResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<HoaDonResponseDTO>> PhatHanhAsync(Guid hopDongId, DateTime thang)
    {
        var hopDong = await _hopDongRepo.GetByIdAsync(hopDongId);
        if (hopDong == null) return TResponse<HoaDonResponseDTO>.FailResponse(HoaDonMessage.HOP_DONG_NOT_FOUND);
        try
        {
            var entity = new HoaDon
            {
                HopDongId = hopDongId,
                MaHoaDon = $"HD{thang:yyyyMM}-{hopDongId.ToString()[..8].ToUpper()}",
                TongTien = hopDong.GiaThue, TrangThai = TrangThaiHoaDonConst.ChuaThanhToan,
                NgayPhatHanh = new DateTime(thang.Year, thang.Month, 1),
                NgayDaoHan = new DateTime(thang.Year, thang.Month, DateTime.DaysInMonth(thang.Year, thang.Month)),
                GhiChu = $"Hóa đơn tháng {thang:MM/yyyy}"
            };
            return TResponse<HoaDonResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<HoaDonResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<List<HoaDonResponseDTO>>> GetQuaHanAsync()
    {
        var items = await _repo.GetQuaHanAsync();
        return TResponse<List<HoaDonResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(HoaDonMessage.NOT_FOUND, 404);
    }

    private static string Validate(HoaDonRequestDTO r)
    {
        if (r.TongTien < 0) return HoaDonMessage.TONG_TIEN_INVALID;
        if (r.NgayPhatHanh == default) return HoaDonMessage.NGAY_PHAT_HANH_REQUIRED;
        return null;
    }

    private static HoaDonResponseDTO Map(HoaDon e) => new()
    {
        Id = e.Id, HopDongId = e.HopDongId, MaHoaDon = e.MaHoaDon, TongTien = e.TongTien,
        TrangThai = e.TrangThai, NgayPhatHanh = e.NgayPhatHanh, NgayDaoHan = e.NgayDaoHan, GhiChu = e.GhiChu,
        IsQuaHan = e.NgayDaoHan.HasValue && e.NgayDaoHan < DateTime.UtcNow && e.TrangThai != TrangThaiHoaDonConst.DaThanhToan,
        CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
    };
}
