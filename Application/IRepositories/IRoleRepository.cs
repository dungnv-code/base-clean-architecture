#nullable disable

using Application.Common;
using Application.DTOs.Role;
using Domain.Entities;


namespace Application.IRepositories;

public interface IRoleRepository : IRepository<Role>
{
    // Task<RoleDTO> GetRolesByUserIdAsync(Guid IdUser);
}
