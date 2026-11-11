#nullable disable

using Application.Common;
using Application.DTOs.Role;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(AppDbContext context) : base(context) { }
    public async Task<List<RoleDTO>> GetRolesByUserIdAsync(Guid IdUser)
    {
        return await _dbSet.AsNoTracking().Where(x => x.UserId == IdUser).Select(ur => new RoleDTO
        {
            Name = ur.Role.Name,
            Code = ur.Role.Code
        }).ToListAsync();
    }
}
