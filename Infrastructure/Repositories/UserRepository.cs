#nullable disable

using Application.Common;
using Application.DTOs.User;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<User>> GetListAsync(UserQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.Ten.Contains(query.Keyword) || x.Email.Contains(query.Keyword) || x.SoDienThoai.Contains(query.Keyword));

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<User>(items, total);
    }
}