#nullable disable

using Application.Common;
using Application.DTOs.ThanhToan;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ThanhToanRepository : Repository<ThanhToan>, IThanhToanRepository
{
    public ThanhToanRepository(AppDbContext context) : base(context) { }

    public async Task<PagedResult<ThanhToan>> GetListAsync(ThanhToanQueryDTO query)
    {
        var q = _dbSet.AsNoTracking();

        if (query.HoaDonId.HasValue)
            q = q.Where(x => x.HoaDonId == query.HoaDonId);
        if (!string.IsNullOrWhiteSpace(query.PhuongThuc))
            q = q.Where(x => x.PhuongThuc == query.PhuongThuc);
        if (!string.IsNullOrWhiteSpace(query.TrangThai))
            q = q.Where(x => x.TrangThai == query.TrangThai);
        if (query.TuNgay.HasValue)
            q = q.Where(x => x.NgayThanhToan >= query.TuNgay);
        if (query.DenNgay.HasValue)
            q = q.Where(x => x.NgayThanhToan <= query.DenNgay);

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(x => x.NgayThanhToan)
            .Skip(query.Skip)
            .Take(query.IsGetAll ? int.MaxValue : query.PageSize)
            .ToListAsync();

        return new PagedResult<ThanhToan>(items, total);
    }

    public async Task<List<ThanhToan>> GetByHoaDonAsync(Guid hoaDonId)
        => await _dbSet.AsNoTracking()
            .Where(x => x.HoaDonId == hoaDonId)
            .OrderByDescending(x => x.NgayThanhToan)
            .ToListAsync();
}
