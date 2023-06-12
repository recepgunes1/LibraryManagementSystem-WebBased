//Add Sum method following repository

using System.Linq.Expressions;
using LMS.Core.Entity;
using LMS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repository;

public class Repository<T> : IRepository<T> where T : class, IEntityBase, new()
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    private DbSet<T> Table => _context.Set<T>();

    public async Task AddAsync(T entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await Table.AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return await Table.CountAsync(predicate!);
    }

    public async Task DeleteAsync(T entity)
    {
        await Task.Run(() => { Table.Remove(entity); });
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Table;
        if (predicate != null) query = query.Where(predicate);
        if (includeProperties.Any())
            foreach (var property in includeProperties)
                query = query.Include(property);
        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = Table;
        query = query.Where(predicate);
        if (includeProperties.Any())
            foreach (var property in includeProperties)
                query = query.Include(property);
        return (await query.FirstOrDefaultAsync())!;
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return (await Table.FindAsync(id))!;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await Task.Run(() => { Table.Update(entity); });
        return entity;
    }

    public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector,
        Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = Table;
        if (predicate != null) query = query.Where(predicate);
        return await query.SumAsync(selector);
    }
}