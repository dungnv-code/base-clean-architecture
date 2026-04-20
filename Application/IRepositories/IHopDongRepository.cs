#nullable disable

using Application.Common;
using Application.DTOs.HopDong;
using Domain.Entities;

namespace Application.IRepositories;

public interface IHopDongRepository : IRepository<HopDong>
{
    Task<PagedResult<HopDong>> GetListAsync(HopDongQueryDTO query);
    Task<List<HopDong>> GetSapHetHanAsync(int soNgay);
    Task<List<HopDong>> GetByKiotAsync(Guid kiotId);
}
