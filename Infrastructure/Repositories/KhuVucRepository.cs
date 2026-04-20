#nullable disable

using Application.Common;
using Application.DTOs.KhuVuc;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class KhuVucRepository : Repository<KhuVuc>, IKhuVucRepository
{
    public KhuVucRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<KhuVuc>> GetListAsync(KhuVucQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.Ten.Contains(query.Keyword));
        if (query.ChoId.HasValue)
            q = q.Where(x => x.ChoId == query.ChoId);
        if (!string.IsNullOrWhiteSpace(query.Loai))
            q = q.Where(x => x.Loai == query.Loai);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<KhuVuc>(items, total);
    }

    public async Task<List<KhuVuc>> GetByChoAsync(Guid choId)
        => await _dbSet.AsNoTracking()
            .Where(x => x.ChoId == choId)
            .OrderBy(x => x.Ten)
            .ToListAsync();
}
