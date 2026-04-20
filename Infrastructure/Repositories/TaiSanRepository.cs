#nullable disable

using Application.Common;
using Application.DTOs.TaiSan;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaiSanRepository : Repository<TaiSan>, ITaiSanRepository
{
    public TaiSanRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<TaiSan>> GetListAsync(TaiSanQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.Ten.Contains(query.Keyword));
        if (query.KhuVucId.HasValue)
            q = q.Where(x => x.KhuVucId == query.KhuVucId);
        if (query.KiotId.HasValue)
            q = q.Where(x => x.KiotId == query.KiotId);
        if (!string.IsNullOrWhiteSpace(query.Loai))
            q = q.Where(x => x.Loai == query.Loai);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<TaiSan>(items, total);
    }

    public async Task<TaiSan> GetByQRCodeAsync(string qrCode)
        => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.QRCode == qrCode);
}
