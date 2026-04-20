#nullable disable

using Application.Common;
using Application.DTOs.HoaDon;
using Application.IRepositories;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class HoaDonRepository : Repository<HoaDon>, IHoaDonRepository
{
    public HoaDonRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<HoaDon>> GetListAsync(HoaDonQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (query.HopDongId.HasValue)
            q = q.Where(x => x.HopDongId == query.HopDongId);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);
        if (query.TuNgay.HasValue)
            q = q.Where(x => x.NgayPhatHanh >= query.TuNgay);
        if (query.DenNgay.HasValue)
            q = q.Where(x => x.NgayPhatHanh <= query.DenNgay);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.NgayPhatHanh)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<HoaDon>(items, total);
    }

    public async Task<List<HoaDon>> GetQuaHanAsync()
        => await _dbSet.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiHoaDonConst.ChuaThanhToan
                     && x.NgayDaoHan.HasValue
                     && x.NgayDaoHan < DateTime.UtcNow)
            .OrderBy(x => x.NgayDaoHan)
            .ToListAsync();
}
