using System.Linq.Expressions;
using LMS.Core.Entity;

namespace LMS.Data.Repository;

public interface IRepository<T> where T : class, IEntityBase, new()
{
    Task AddAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task DeleteAsync(T entity);

    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    Task<T> GetByIdAsync(string id);
    Task<T> UpdateAsync(T entity);
    Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? predicate = null);
}