#nullable disable

using Application.Common;
using Application.DTOs.SuCo;
using Domain.Entities;

namespace Application.IRepositories;

public interface ISuCoRepository : IRepository<SuCo>
{
    Task<PagedResult<SuCo>> GetListAsync(SuCoQueryDTO query);
}
