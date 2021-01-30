using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Elite.Repositories.Abstractions
{
    public interface IRepository
    { }

    public interface IRepository<TEntity, TKey> : IRepository
    where TEntity : class, IEntity
    {
        Task<TEntity> GetByKeyAsync(TKey key);
        Task InsertAsync(TEntity entity);
        Task InsertAsync(params TEntity[] entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(params TEntity[] entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(params TEntity[] entities);
        Task DeleteByKeyAsync(TKey key);
        Task<IEnumerable<TEntity>> GetByCriteria(IDictionary<string, object> criterias);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}