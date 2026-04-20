#nullable disable

using Application.Common;
using Application.DTOs.NguoiDung;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NguoiDungRepository : Repository<NguoiDung>, INguoiDungRepository
{
    public NguoiDungRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<NguoiDung>> GetListAsync(NguoiDungQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Keyword))
            q = q.Where(x => x.Ten.Contains(query.Keyword)
                           || (x.Email != null && x.Email.Contains(query.Keyword)));
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);
        if (!string.IsNullOrWhiteSpace(query.VaiTroNoiBo))
            q = q.Where(x => x.VaiTroNoiBo == query.VaiTroNoiBo);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.CreatedAt)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<NguoiDung>(items, total);
    }

    public async Task<NguoiDung> GetByExternalIdAsync(string externalId)
        => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ExternalId == externalId);
}
