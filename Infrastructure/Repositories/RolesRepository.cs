#nullable disable

using Application.Common;
using Application.DTOs.Role;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RolesRepository : Repository<Role>, IRoleRepository
{
    public RolesRepository(AppDbContext context) : base(context) { }
    // public async Task<RoleDTO> GetRolesByUserIdAsync(Guid IdUser)
    // {
    //     return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==IdUser)
    // }
}
