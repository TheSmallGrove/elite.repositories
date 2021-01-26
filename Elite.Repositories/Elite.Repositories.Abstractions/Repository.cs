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
        where TKey: ITuple
    {
        public abstract Task<TEntity> GetByKeyAsync(TKey key);
        public abstract Task InsertAsync(TEntity entity);
        public abstract Task InsertAsync(params TEntity[] entities);
        public abstract Task UpdateAsync(TEntity entity);
        public abstract Task UpdateAsync(params TEntity[] entities);
        public abstract Task DeleteAsync(TEntity entity);
        public abstract Task DeleteAsync(params TEntity[] entities);
        protected abstract IQueryable<TEntity> Set { get; }
    }
}
