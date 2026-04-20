#nullable disable

using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.EForm;
using Application.DTOs.Kiot;
using Application.IRepositories;
using Application.IServices;
using Domain.Constants;
using Domain.Entities;

namespace Application.Services;

public class KiotService : IKiotService
{
    private readonly IKiotRepository _repo;
    private readonly IHopDongRepository _hopDongRepo;

    private static readonly HashSet<string> ValidTrangThai = new()
    {
        TrangThaiKiotConst.Trong, TrangThaiKiotConst.DangChoThue,
        TrangThaiKiotConst.SapHetHan, TrangThaiKiotConst.DangBaoTri,
        TrangThaiKiotConst.DangCoc
    };

    public KiotService(IKiotRepository repo, IHopDongRepository hopDongRepo)
    {
        _repo = repo; _hopDongRepo = hopDongRepo;
    }

    public async Task<TResponse<List<KiotResponseDTO>>> GetListAsync(KiotQueryDTO query)
    {
        try
        {
            var result = await _repo.GetListAsync(query);
            return TResponse<List<KiotResponseDTO>>.SuccessResponse(
                result.Items.Select(Map).ToList(),
                metaData: new MetaDataDTO { Page = query.Page, PageSize = query.PageSize, Total = result.Total });
        }
        catch { return TResponse<List<KiotResponseDTO>>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<KiotResponseDTO>> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity == null
            ? TResponse<KiotResponseDTO>.FailResponse(KiotMessage.NOT_FOUND, 404)
            : TResponse<KiotResponseDTO>.SuccessResponse(Map(entity));
    }

    public async Task<TResponse<KiotResponseDTO>> CreateAsync(KiotRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<KiotResponseDTO>.FailResponse(error);
        try
        {
            if (await _repo.AnyAsync(x => x.MaKiot == request.MaKiot.Trim()))
                return TResponse<KiotResponseDTO>.FailResponse(KiotMessage.MA_KIOT_DUPLICATE);

            var entity = new Kiot
            {
                KhuVucId = request.KhuVucId, MaKiot = request.MaKiot.Trim(),
                DienTich = request.DienTich, ViTri2D = request.ViTri2D?.Trim(),
                ViTri3D = request.ViTri3D?.Trim(),
                TrangThai = request.TrangThai ?? TrangThaiKiotConst.Trong,
                LoaiKinhDoanh = request.LoaiKinhDoanh?.Trim()
            };
            return TResponse<KiotResponseDTO>.SuccessResponse(Map(await _repo.CreateAsync(entity)));
        }
        catch { return TResponse<KiotResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<KiotResponseDTO>> UpdateAsync(Guid id, KiotRequestDTO request)
    {
        var error = Validate(request);
        if (error != null) return TResponse<KiotResponseDTO>.FailResponse(error);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<KiotResponseDTO>.FailResponse(KiotMessage.NOT_FOUND, 404);
        try
        {
            entity.KhuVucId = request.KhuVucId; entity.DienTich = request.DienTich;
            entity.ViTri2D = request.ViTri2D?.Trim(); entity.ViTri3D = request.ViTri3D?.Trim();
            entity.TrangThai = request.TrangThai; entity.LoaiKinhDoanh = request.LoaiKinhDoanh?.Trim();
            return TResponse<KiotResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<KiotResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<KiotResponseDTO>> CapNhatTrangThaiAsync(Guid id, string trangThai)
    {
        if (string.IsNullOrWhiteSpace(trangThai)) return TResponse<KiotResponseDTO>.FailResponse(KiotMessage.TRANG_THAI_REQUIRED);
        if (!ValidTrangThai.Contains(trangThai)) return TResponse<KiotResponseDTO>.FailResponse(KiotMessage.TRANG_THAI_INVALID);
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return TResponse<KiotResponseDTO>.FailResponse(KiotMessage.NOT_FOUND, 404);
        try
        {
            entity.TrangThai = trangThai;
            return TResponse<KiotResponseDTO>.SuccessResponse(Map(await _repo.UpdateAsync(entity)));
        }
        catch { return TResponse<KiotResponseDTO>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500); }
    }

    public async Task<TResponse<List<KiotResponseDTO>>> GetByKhuVucAsync(Guid khuVucId)
    {
        var items = await _repo.GetByKhuVucAsync(khuVucId);
        return TResponse<List<KiotResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<List<KiotResponseDTO>>> GetSapHetHanAsync(int soNgay = 30)
    {
        var items = await _repo.GetSapHetHanAsync(soNgay);
        return TResponse<List<KiotResponseDTO>>.SuccessResponse(items.Select(Map).ToList());
    }

    public async Task<TResponse<bool>> DeleteAsync(Guid id)
    {
        var ok = await _repo.DeleteAsync(id);
        return ok ? TResponse<bool>.SuccessResponse(true) : TResponse<bool>.FailResponse(KiotMessage.NOT_FOUND, 404);
    }

    private static string Validate(KiotRequestDTO r)
    {
        if (string.IsNullOrWhiteSpace(r.MaKiot)) return KiotMessage.MA_KIOT_REQUIRED;
        if (r.DienTich <= 0) return KiotMessage.DIEN_TICH_INVALID;
        return null;
    }

    private static KiotResponseDTO Map(Kiot e) => new()
    {
        Id = e.Id, KhuVucId = e.KhuVucId, MaKiot = e.MaKiot, DienTich = e.DienTich,
        ViTri2D = e.ViTri2D, ViTri3D = e.ViTri3D, TrangThai = e.TrangThai,
        LoaiKinhDoanh = e.LoaiKinhDoanh, CreatedAt = e.CreatedAt, UpdatedAt = e.UpdatedAt
    };
}
