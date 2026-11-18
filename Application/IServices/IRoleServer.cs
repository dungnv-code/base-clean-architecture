using Application.DTOs.Role;
using Domain.Entities;

namespace Application.IServices;

public interface IRoleService
{
    Task<Role> GetByIdAsync(Guid id);
    Task<Role> CreateAsync(RoleDTO request);
    Task<Role> UpdateAsync(Guid id, RoleDTO request);
    Task<List<Role>> GetListAsync(RoleQueryDTO query);
    Task<bool> DeleteAsync(Guid id);
}