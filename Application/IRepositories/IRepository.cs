#nullable disable

using System.Linq.Expressions;
using Domain.Entities;

namespace Application.IRepositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T entity);
    Task<List<T>> CreateRangeAsync(List<T> entities);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
