#nullable disable

using Application.Common;
using Application.DTOs.User;
using Domain.Entities;

namespace Application.IRepositories;

public interface IUserRepository : IRepository<User>
{
    Task<PagedResult<User>> GetListAsync(UserQueryDTO query);
}