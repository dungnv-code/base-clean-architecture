#nullable disable

using System.Linq.Expressions;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
        => await _dbSet.FindAsync(id);

    public async Task<List<T>> GetListAsync(
        Expression<Func<T, bool>> predicate = null,
        int skip = 0,
        int take = 0,
        string orderBy = null,
        bool descending = true)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (predicate != null) query = query.Where(predicate);
        query = descending
            ? query.OrderByDescending(x => x.CreatedAt)
            : query.OrderBy(x => x.CreatedAt);
        if (skip > 0) query = query.Skip(skip);
        if (take > 0) query = query.Take(take);
        return await query.ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = _dbSet;
        if (predicate != null) query = query.Where(predicate);
        return await query.CountAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T>> CreateRangeAsync(List<T> entities)
    {
        var now = DateTime.UtcNow;
        foreach (var e in entities) { e.CreatedAt = now; e.UpdatedAt = now; }
        _dbSet.AddRange(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);
}
