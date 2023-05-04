using Library_Management_System.Core.Entity;
using Library_Management_System.Data.Context;
using Library_Management_System.Data.Repository;

namespace Library_Management_System.Data.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public IRepository<T> GetRepository<T>() where T : class, IEntityBase, new()
    {
        return new Repository<T>(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}