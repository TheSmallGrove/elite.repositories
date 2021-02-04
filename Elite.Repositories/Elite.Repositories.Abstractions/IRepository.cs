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
        Task<bool> ExistsByKeyAsync(TKey key);
        Task<TEntity> GetByKeyAsync(TKey key);
        Task<IEnumerable<TEntity>> GetByKeyAsync(params TKey[] key);
        Task InsertAsync(TEntity entity);
        Task InsertAsync(params TEntity[] entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(params TEntity[] entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(params TEntity[] entities);
        Task DeleteByKeyAsync(TKey key);
        Task DeleteByKeyAsync(params TKey[] key);
        Task<IEnumerable<dynamic>> GetByCriteriaAsync(string projection, params ICriteria [] criterias);
        Task<int> CountByCriteriaAsync(params ICriteria [] criterias);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> CountAllAsync();
    }
}