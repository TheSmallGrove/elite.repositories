using System;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        T GetRepository<T>()
            where T : IRepository;
        Task<IUnitOfWorkTransaction> BeginTransaction();
    }
}
