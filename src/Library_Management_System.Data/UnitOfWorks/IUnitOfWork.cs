using Library_Management_System.Core.Entity;
using Library_Management_System.Data.Repository;

namespace Library_Management_System.Data.UnitOfWorks;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
    Task<int> SaveAsync();
}