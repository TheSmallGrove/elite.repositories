using Elite.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Elite.Repositories.EntityFramework
{
    public abstract class EntityRepository<TEntity, TKey> : Repository<TEntity, TKey>
        where TEntity : class, IEntity
        where TKey: ITuple
    {
        private DbContext Context { get; }

        public EntityRepository(DbContext context)
        {
            this.Context = context;
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

        public override Task UpdateAsync(params TEntity [] entities)
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

        protected override IQueryable<TEntity> Set => this.Context.Set<TEntity>();
    }
}
