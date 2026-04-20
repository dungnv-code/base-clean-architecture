#nullable disable

using Application.Common;
using Application.DTOs.ThuongNhan;
using Domain.Entities;

namespace Application.IRepositories;

public interface IThuongNhanRepository : IRepository<ThuongNhan>
{
    Task<PagedResult<ThuongNhan>> GetListAsync(ThuongNhanQueryDTO query);
}
