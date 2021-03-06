﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Elite.Repositories.Abstractions
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity
    {
        public abstract Task<bool> ExistsByKeyAsync(TKey key);
        public abstract Task<TEntity> GetByKeyAsync(TKey key);
        public abstract Task<IEnumerable<TEntity>> GetByKeyAsync(params TKey[] key);
        public abstract Task InsertAsync(TEntity entity);
        public abstract Task InsertAsync(params TEntity[] entities);
        public abstract Task UpdateAsync(TEntity entity);
        public abstract Task UpdateAsync(params TEntity[] entities);
        public abstract Task DeleteAsync(TEntity entity);
        public abstract Task DeleteAsync(params TEntity[] entities);
        public abstract Task DeleteByKeyAsync(TKey key);
        public abstract Task DeleteByKeyAsync(params TKey[] key);
        public abstract Task<IEnumerable<dynamic>> GetByCriteriaAsync(string projection, params ICriteria[] criterias);
        public abstract Task<int> CountByCriteriaAsync(params ICriteria[] criterias);
        public abstract Task<IEnumerable<TEntity>> GetAllAsync();
        public abstract Task<int> CountAllAsync();
        protected abstract IQueryable<TEntity> Set { get; }
    }
}
