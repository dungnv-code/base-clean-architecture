#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.TaiSan;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class TaiSanService : ITaiSanService
{
    private readonly ITaiSanRepository _repo;
    public TaiSanService(ITaiSanRepository repo) => _repo = repo;

    public async Task<TResponse<List<TaiSanResponseDTO>>> GetListAsync(TaiSanQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<TaiSanResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<TaiSanResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<TaiSanResponseDTO>> GetByIdAsync(Guid id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e == null ? TResponse<TaiSanResponseDTO>.FailResponse(TaiSanMessage.NOT_FOUND, 404) : TResponse<TaiSanResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<TaiSanResponseDTO>> GetByQRCodeAsync(string qrCode)
    {
        if (string.IsNullOrWhiteSpace(qrCode)) return TResponse<TaiSanResponseDTO>.FailResponse(CommonMessage.MISSING_PARAM);
        var e = await _repo.GetByQRCodeAsync(qrCode.Trim());
        return e == null ? TResponse<TaiSanResponseDTO>.FailResponse(TaiSanMessage.QR_NOT_FOUND, 404) : TResponse<TaiSanResponseDTO>.SuccessResponse(Map(e));
    }

    public async Task<TResponse<TaiSanResponseDTO>> CreateAsync(TaiSanRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<TaiSanResponseDTO>.FailResponse(error);
        try
        {
            var entity = new TaiSan { KhuVucId = request.KhuVucId, KiotId = request.KiotId, Ten = request.Ten.Trim(), Loai = request.Loai.Trim(), GiaTri = request.GiaTri, ViTri = request.ViTri?.Trim(), QRCode = request.QRCode?.Trim(), TrangThai = request.TrangThai ?? "HoatDong", NgayMua = request.NgayMua, NgayBaoHanh = request.NgayBaoHanh };
            return TResponse<TaiSanResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<TaiSanResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<TaiSanResponseDTO>> UpdateAsync(Guid id, TaiSanRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<TaiSanResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<TaiSanResponseDTO>.FailResponse(TaiSanMessage.NOT_FOUND, 404);
        try
        {
            entity.KhuVucId = request.KhuVucId; entity.KiotId = request.KiotId; entity.Ten = request.Ten.Trim(); entity.Loai = request.Loai.Trim(); entity.GiaTri = request.GiaTri; entity.ViTri = request.ViTri?.Trim(); entity.QRCode = request.QRCode?.Trim(); entity.TrangThai = request.TrangThai; entity.NgayMua = request.NgayMua; entity.NgayBaoHanh = request.NgayBaoHanh;
            return TResponse<TaiSanResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<TaiSanResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(TaiSanMessage.NOT_FOUND, 404);
    }

    private static string Validate(TaiSanRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.Ten)) return TaiSanMessage.TEN_REQUIRED;
        if (string.IsNullOrWhiteSpace(r.Loai)) return TaiSanMessage.LOAI_REQUIRED;
        if (r.GiaTri < 0) return TaiSanMessage.GIA_TRI_INVALID;
        return null;
    }

    private static TaiSanResponseDTO Map(TaiSan e) => new()
        { Id = e.Id, KhuVucId = e.KhuVucId, KiotId = e.KiotId, Ten = e.Ten, Loai = e.Loai, GiaTri = e.GiaTri, ViTri = e.ViTri, QRCode = e.QRCode, TrangThai = e.TrangThai, NgayMua = e.NgayMua, NgayBaoHanh = e.NgayBaoHanh, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt };
}
