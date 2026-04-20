#nullable disable

using Application.Common;
using Application.DTOs.KhuVuc;
using Domain.Entities;

namespace Application.IRepositories;

public interface IKhuVucRepository : IRepository<KhuVuc>
{
    Task<PagedResult<KhuVuc>> GetListAsync(KhuVucQueryDTO query);
    Task<List<KhuVuc>> GetByChoAsync(Guid choId);
}
