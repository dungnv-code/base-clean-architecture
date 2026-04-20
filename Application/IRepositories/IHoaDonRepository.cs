#nullable disable

using Application.Common;
using Application.DTOs.HoaDon;
using Domain.Entities;

namespace Application.IRepositories;

public interface IHoaDonRepository : IRepository<HoaDon>
{
    Task<PagedResult<HoaDon>> GetListAsync(HoaDonQueryDTO query);
    Task<List<HoaDon>> GetQuaHanAsync();
}
