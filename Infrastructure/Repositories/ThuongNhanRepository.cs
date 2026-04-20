#nullable disable

using Application.Common;
using Application.DTOs.ThuongNhan;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ThuongNhanRepository : Repository<ThuongNhan>, IThuongNhanRepository
{
    public ThuongNhanRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<ThuongNhan>> GetListAsync(ThuongNhanQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.Ten.Contains(query.Keyword)
                           || x.SoDienThoai.Contains(query.Keyword)
                           || x.CCCD.Contains(query.Keyword));

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<ThuongNhan>(items, total);
    }
}
