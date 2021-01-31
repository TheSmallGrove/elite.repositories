using Elite.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Elite.Repositories.EntityFramework.Criterias;

namespace Elite.Repositories.EntityFramework
{
    public abstract class EntityRepository<TEntity, TKey> : Repository<TEntity, TKey>
        where TEntity : class, IEntity
    {
        private DbContext Context { get; }
        private ICriteriaExecutorResolver CriteriaResolver { get; }

        public EntityRepository(DbContext context, ICriteriaExecutorResolver criteriaResolver = null)
        {
            this.Context = context;
            this.CriteriaResolver = criteriaResolver;
        }

        public override async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await this.Context.Set<TEntity>().AddAsync(entity);
            await this.Context.SaveChangesAsync(true);
        }

        public override async Task InsertAsync(params TEntity[] entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.Any())
            {
                await this.Context.Set<TEntity>().AddRangeAsync(entities);
                await this.Context.SaveChangesAsync(true);
            }
        }

        public override Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this.Context.Set<TEntity>().Update(entity);
            return this.Context.SaveChangesAsync(true);
        }

        public override Task UpdateAsync(params TEntity[] entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.Any())
            {
                this.Context.Set<TEntity>().UpdateRange(entities);
                return this.Context.SaveChangesAsync(true);
            }

            return Task.CompletedTask;
        }

        public override Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this.Context.Set<TEntity>().Remove(entity);
            return this.Context.SaveChangesAsync(true);
        }

        public override Task DeleteAsync(params TEntity[] entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.Any())
            {
                this.Context.Set<TEntity>().RemoveRange(entities);
                return this.Context.SaveChangesAsync(true);
            }

            return Task.CompletedTask;
        }

        public override async Task<IEnumerable<TEntity>> GetByCriteria(params ICriteria[] criterias)
        {
            var query = this.Set;
            query = ApplyCriteria<SortingCriteria>("sorting", criterias, query);
            query = ApplyCriteria<PagingCriteria>("paging", criterias, query);
            return await query.ToArrayAsync();
        }

        private IQueryable<TEntity> ApplyCriteria<TCriteria>(string name, IEnumerable<ICriteria> criterias, IQueryable<TEntity> query)
            where TCriteria : ICriteria
        {
            if (this.CriteriaResolver != null)
            {
                TCriteria arguments = criterias.OfType<TCriteria>().SingleOrDefault();

                if (arguments != null)
                {
                    ICriteriaExecutor<TCriteria> criteria = this.CriteriaResolver.Resolve<TCriteria>(name);
                    query = criteria.Apply(query, arguments);
                }
            }

            return query;
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync() => await this.Set.ToArrayAsync();

        protected override IQueryable<TEntity> Set => this.Context.Set<TEntity>();
    }
}
