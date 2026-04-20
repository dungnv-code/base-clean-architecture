#nullable disable

using Application.Common;
using Application.DTOs.LichKiemTra;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LichKiemTraRepository : Repository<LichKiemTra>, ILichKiemTraRepository
{
    public LichKiemTraRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<LichKiemTra>> GetListAsync(LichKiemTraQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.TieuDe.Contains(query.Keyword));
        if (query.KhuVucId.HasValue)
            q = q.Where(x => x.KhuVucId == query.KhuVucId);
        if (!string.IsNullOrWhiteSpace(query.LoaiKiemTra))
            q = q.Where(x => x.LoaiKiemTra == query.LoaiKiemTra);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);
        if (query.TuNgay.HasValue)
            q = q.Where(x => x.NgayKiemTra >= query.TuNgay);
        if (query.DenNgay.HasValue)
            q = q.Where(x => x.NgayKiemTra <= query.DenNgay);

        var total = await q.CountAsync();
        var items = await q
            .OrderBy(x => x.NgayKiemTra)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<LichKiemTra>(items, total);
    }

    public async Task<List<LichKiemTra>> GetSapDenAsync(int soNgay)
    {
        var deadline = DateTime.UtcNow.AddDays(soNgay);
        return await _dbSet.AsNoTracking()
            .Where(x => x.NgayKiemTra >= DateTime.UtcNow && x.NgayKiemTra <= deadline)
            .OrderBy(x => x.NgayKiemTra)
            .ToListAsync();
    }
}
