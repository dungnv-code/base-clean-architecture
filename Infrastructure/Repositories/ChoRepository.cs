#nullable disable

using Application.Common;
using Application.DTOs.Cho;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChoRepository : Repository<Cho>, IChoRepository
{
    public ChoRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<Cho>> GetListAsync(ChoQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.Ten.Contains(query.Keyword) || x.DiaChi.Contains(query.Keyword));

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<Cho>(items, total);
    }
}
