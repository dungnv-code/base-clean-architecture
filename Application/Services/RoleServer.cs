using Application.DTOs.Role;
using Application.IRepositories;
using Application.IServices;
using Domain.Entities;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<Role> _repository;
    public RoleService(IRepository<Role> repository)
    {
        _repository = repository;
    }

    public async Task<Role> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Role> CreateAsync(RoleDTO request)
    {
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Code))
        {
            throw new ArgumentException("Name and Code cannot be null or empty.");
        }

        var role = new Role
        {
            Name = request.Name.Trim(),
            Code = request.Code.Trim()
        };

        return await _repository.CreateAsync(role);
    }

    public async Task<Role> UpdateAsync(Guid id, RoleDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Code))
        {
            throw new ArgumentException("Name and Code cannot be null or empty.");
        }

        var existingRole = await _repository.GetByIdAsync(id);
        if (existingRole == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        }

        existingRole.Name = request.Name.Trim();
        existingRole.Code = request.Code.Trim();
        return await _repository.UpdateAsync(existingRole);
    }

    public async Task<List<Role>> GetListAsync(RoleQueryDTO query)
    {
        var roles = await _repository.GetListAsync(
            predicate: r => (string.IsNullOrWhiteSpace(query.Name) || r.Name.Contains(query.Name)) &&
                            (string.IsNullOrWhiteSpace(query.Code) || r.Code.Contains(query.Code)),
            skip: (query.Page - 1) * query.PageSize,
            take: query.PageSize,
            orderBy: "CreatedAt",
            descending: true
        );

        return roles;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var role = await _repository.GetByIdAsync(id);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {id} not found.");
        }

        return await _repository.DeleteAsync(id);
    }
}