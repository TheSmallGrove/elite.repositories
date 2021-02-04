using Elite.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Elite.Repositories.EntityFramework.Criterias;
using Microsoft.EntityFrameworkCore.DynamicLinq;

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

        protected abstract Expression<Func<TEntity, bool>> MatchKey(TKey key);

        protected abstract Expression<Func<TEntity, bool>> MatchKeys(params TKey[] keys);

        public override async Task<bool> ExistsByKeyAsync(TKey key)
        {
            return await this.Set
                .Where(this.MatchKey(key))
                .Select(e => e)
                .AnyAsync();
        }

        public override async Task<TEntity> GetByKeyAsync(TKey key)
        {
            return await this.Set
                .Where(this.MatchKey(key))
                .Select(e => e)
                .SingleOrDefaultAsync();                
        }

        public override async Task<TEntity> GetByKeyAsync(params TKey[] keys)
        {
            return await this.Set
                .Where(this.MatchKeys(keys))
                .Select(e => e)
                .SingleOrDefaultAsync();
        }

        public override async Task DeleteByKeyAsync(TKey key)
        {
            var entity = await this.GetByKeyAsync(key);
            await this.DeleteAsync(entity);
        }

        public override async Task DeleteByKeyAsync(params TKey[] keys)
        {
            var entity = await this.GetByKeyAsync(keys);
            await this.DeleteAsync(entity);
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

        public override async Task<IEnumerable<dynamic>> GetByCriteriaAsync(string projection, params ICriteria[] criterias)
        {
            if (projection == null)
                throw new ArgumentNullException(nameof(projection));
            if (criterias == null)
                throw new ArgumentNullException(nameof(criterias));

            return await this.CreateQueryFromCriteria(criterias)
                .Select(projection).ToDynamicArrayAsync();
        }

        public override async Task<int> CountByCriteriaAsync(params ICriteria[] criterias)
        {
            if (criterias == null)
                throw new ArgumentNullException(nameof(criterias));

            return await this.CreateQueryFromCriteria(criterias).CountAsync();
        }

        private IQueryable CreateQueryFromCriteria(ICriteria[] criterias)
        {

            if (this.CriteriaResolver == null)
                throw new InvalidOperationException("No resolver available for criteria translation");

            IQueryable query = this.Set;

            foreach (var criteria in criterias)
            {
                var executor = this.CriteriaResolver.Resolve(criteria.GetType());
                query = executor.Apply(query, criteria);
            }

            return query;
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync() => await this.Set.ToArrayAsync();

        public override async Task<int> CountAllAsync() => await this.Set.CountAsync();

        protected override IQueryable<TEntity> Set => this.Context.Set<TEntity>();
    }
}
