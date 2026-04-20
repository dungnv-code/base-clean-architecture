#nullable disable

using Application.Common;
using Application.DTOs.HopDong;
using Application.IRepositories;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class HopDongRepository : Repository<HopDong>, IHopDongRepository
{
    public HopDongRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<HopDong>> GetListAsync(HopDongQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (query.KiotId.HasValue)
            q = q.Where(x => x.KiotId == query.KiotId);
        if (query.ThuongNhanId.HasValue)
            q = q.Where(x => x.ThuongNhanId == query.ThuongNhanId);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);
        if (query.TuNgay.HasValue)
            q = q.Where(x => x.NgayBatDau >= query.TuNgay);
        if (query.DenNgay.HasValue)
            q = q.Where(x => x.NgayKetThuc <= query.DenNgay);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<HopDong>(items, total);
    }

    public async Task<List<HopDong>> GetSapHetHanAsync(int soNgay)
    {
        var deadline = DateTime.UtcNow.AddDays(soNgay);
        return await _dbSet.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiHopDongConst.HoatDong && x.NgayKetThuc <= deadline)
            .OrderBy(x => x.NgayKetThuc)
            .ToListAsync();
    }

    public async Task<List<HopDong>> GetByKiotAsync(Guid kiotId)
        => await _dbSet.AsNoTracking()
            .Where(x => x.KiotId == kiotId)
            .OrderByDescending(x => x.NgayBatDau)
            .ToListAsync();
}
