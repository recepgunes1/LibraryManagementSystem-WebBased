using LMS.Core.Entity;
using LMS.Data.Repository;

namespace LMS.Data.UnitOfWorks;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<T> GetRepository<T>() where T : class, IEntityBase, new();
    Task<int> SaveAsync();
}