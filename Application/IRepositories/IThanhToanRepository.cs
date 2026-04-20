#nullable disable

using Application.Common;
using Application.DTOs.ThanhToan;
using Domain.Entities;

namespace Application.IRepositories;

public interface IThanhToanRepository : IRepository<ThanhToan>
{
    Task<PagedResult<ThanhToan>> GetListAsync(ThanhToanQueryDTO query);
    Task<List<ThanhToan>> GetByHoaDonAsync(Guid hoaDonId);
}
