#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.HopDong;
using Application.IRepositories;
using Application.IServices;
using Domain.Constants;
using Domain.Entities;

namespace Application.Services;

public class HopDongService : IHopDongService
{
    private readonly IHopDongRepository _repo;
    private readonly IKiotRepository _kiotRepo;

    public HopDongService(IHopDongRepository repo, IKiotRepository kiotRepo)
    {
        _repo = repo; _kiotRepo = kiotRepo;
    }

    public async Task<TResponse<List<HopDongResponseDTO>>> GetListAsync(HopDongQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<HopDongResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<HopDongResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<HopDongResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null
            ? TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.NOT_FOUND, 404)
            : TResponse<HopDongResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<HopDongResponseDTO>> CreateAsync(HopDongRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<HopDongResponseDTO>.FailResponse(error);
        try
        {
            if (request.KiotId.HasValue)
            {
                var kiot = await _kiotRepo.GetByIdAsync(request.KiotId.Value);
                if (kiot == null) return TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.KIOT_NOT_EXISTS);
                if (kiot.TrangThai != TrangThaiKiotConst.Trong && kiot.TrangThai != TrangThaiKiotConst.DangCoc)
                    return TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.KIOT_NOT_AVAILABLE);
                kiot.TrangThai = TrangThaiKiotConst.DangChoThue;
                await _kiotRepo.UpdateAsync(kiot);
            }
            var entity = new HopDong
            {
                KiotId = request.KiotId, ThuongNhanId = request.ThuongNhanId,
                NgayBatDau = request.NgayBatDau, NgayKetThuc = request.NgayKetThuc,
                GiaThue = request.GiaThue, TienCoc = request.TienCoc,
                TrangThai = TrangThaiHopDongConst.HoatDong, GhiChu = request.GhiChu?.Trim()
            };
            return TResponse<HopDongResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<HopDongResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<HopDongResponseDTO>> UpdateAsync(Guid id, HopDongRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<HopDongResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.NOT_FOUND, 404);
        try
        {
            entity.NgayBatDau = request.NgayBatDau; entity.NgayKetThuc = request.NgayKetThuc;
            entity.GiaThue = request.GiaThue; entity.TienCoc = request.TienCoc; entity.GhiChu = request.GhiChu?.Trim();
            return TResponse<HopDongResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<HopDongResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<HopDongResponseDTO>> ChamDutAsync(Guid id, string lyDo)
    {
        if (string.IsNullOrWhiteSpace(lyDo)) return TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.LY_DO_REQUIRED);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.NOT_FOUND, 404);
        if (entity.TrangThai == TrangThaiHopDongConst.ChamDut) return TResponse<HopDongResponseDTO>.FailResponse(HopDongMessage.ALREADY_TERMINATED);
        try
        {
            entity.TrangThai = TrangThaiHopDongConst.ChamDut;
            entity.GhiChu = $"Chấm dứt: {lyDo.Trim()}";
            if (entity.KiotId.HasValue)
            {
                var kiot = await _kiotRepo.GetByIdAsync(entity.KiotId.Value);
                if (kiot != null) { kiot.TrangThai = TrangThaiKiotConst.Trong; await _kiotRepo.UpdateAsync(kiot); }
            }
            return TResponse<HopDongResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<HopDongResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<List<HopDongResponseDTO>>> GetSapHetHanAsync(int soNgay = 30)
    {
        var items = await _repo.GetSapHetHanAsync(soNgay);
        return TResponse<List<HopDongResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<List<HopDongResponseDTO>>> GetByKiotAsync(Guid kiotId)
    {
        var items = await _repo.GetByKiotAsync(kiotId);
        return TResponse<List<HopDongResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(HopDongMessage.NOT_FOUND, 404);
    }

    private static string Validate(HopDongRequestDTO r)
    {
        if (!r.KiotId.HasValue) return HopDongMessage.KIOT_REQUIRED;
        if (!r.ThuongNhanId.HasValue) return HopDongMessage.THUONG_NHAN_REQUIRED;
        if (r.NgayKetThuc <= r.NgayBatDau) return HopDongMessage.NGAY_INVALID;
        if (r.GiaThue <= 0) return HopDongMessage.GIA_THUE_INVALID;
        return null;
    }

    private static HopDongResponseDTO Map(HopDong e) => new()
    {
        Id = e.Id, KiotId = e.KiotId, ThuongNhanId = e.ThuongNhanId,
        NgayBatDau = e.NgayBatDau, NgayKetThuc = e.NgayKetThuc, GiaThue = e.GiaThue,
        TienCoc = e.TienCoc, TrangThai = e.TrangThai, GhiChu = e.GhiChu,
        SoNgayConLai = Math.Max(0, (int)(e.NgayKetThuc - DateTime.UtcNow).TotalDays),
        CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
    };
}
