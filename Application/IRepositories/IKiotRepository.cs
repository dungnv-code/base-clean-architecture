#nullable disable

using Application.Common;
using Application.DTOs.Kiot;
using Domain.Entities;

namespace Application.IRepositories;

public interface IKiotRepository : IRepository<Kiot>
{
    Task<PagedResult<Kiot>> GetListAsync(KiotQueryDTO query);
    Task<List<Kiot>> GetByKhuVucAsync(Guid khuVucId);
    Task<List<Kiot>> GetSapHetHanAsync(int soNgay);
}
