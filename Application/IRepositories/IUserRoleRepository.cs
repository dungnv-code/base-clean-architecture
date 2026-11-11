#nullable disable

using Application.Common;
using Application.DTOs.Role;
using Domain.Entities;

namespace Application.IRepositories;

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<List<RoleDTO>> GetRolesByUserIdAsync(Guid IdUser);
}
