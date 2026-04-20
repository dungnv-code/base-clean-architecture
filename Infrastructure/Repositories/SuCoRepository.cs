#nullable disable

using Application.Common;
using Application.DTOs.SuCo;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SuCoRepository : Repository<SuCo>, ISuCoRepository
{
    public SuCoRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<SuCo>> GetListAsync(SuCoQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.MoTa.Contains(query.Keyword));
        if (query.KiotId.HasValue)
            q = q.Where(x => x.KiotId == query.KiotId);
        if (!string.IsNullOrWhiteSpace(query.MucDo))
            q = q.Where(x => x.MucDo == query.MucDo);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<SuCo>(items, total);
    }
}
