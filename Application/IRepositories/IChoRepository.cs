#nullable disable

using Application.Common;
using Application.DTOs.Cho;
using Domain.Entities;

namespace Application.IRepositories;

public interface IChoRepository : IRepository<Cho>
{
    Task<PagedResult<Cho>> GetListAsync(ChoQueryDTO query);
}
