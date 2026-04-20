#nullable disable

using Application.Common;
using Application.DTOs.Kiot;
using Application.IRepositories;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class KiotRepository : Repository<Kiot>, IKiotRepository
{
    public KiotRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<Kiot>> GetListAsync(KiotQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.MaKiot.Contains(query.Keyword) || (x.LoaiKinhDoanh != null && x.LoaiKinhDoanh.Contains(query.Keyword)));
        if (query.KhuVucId.HasValue)
            q = q.Where(x => x.KhuVucId == query.KhuVucId);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);
        if (!string.IsNullOrWhiteSpace(query.LoaiKinhDoanh))
            q = q.Where(x => x.LoaiKinhDoanh == query.LoaiKinhDoanh);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<Kiot>(items, total);
    }

    public async Task<List<Kiot>> GetByKhuVucAsync(Guid khuVucId)
        => await _dbSet.AsNoTracking()
            .Where(x => x.KhuVucId == khuVucId)
            .OrderBy(x => x.MaKiot)
            .ToListAsync();

    public async Task<List<Kiot>> GetSapHetHanAsync(int soNgay)
    {
        var deadline = DateTime.UtcNow.AddDays(soNgay);
        var kiotIds = await _context.Set<HopDong>()
            .AsNoTracking()
            .Where(h => h.TrangThai == TrangThaiHopDongConst.HoatDong
                     && h.NgayKetThuc <= deadline
                     && h.KiotId.HasValue)
            .Select(h => h.KiotId.Value)
            .Distinct()
            .ToListAsync();

        return await _dbSet.AsNoTracking()
            .Where(x => kiotIds.Contains(x.Id))
            .ToListAsync();
    }
}
