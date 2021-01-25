using Elite.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework
{
    public class EntityRepository<TEntity> : Repository<TEntity>
        where TEntity : class
    {
        private DbContext Context { get; }

        public EntityRepository(DbContext context)
        {
            this.Context = context;
        }

        public override Task Delete(TEntity entity)
        {
            this.Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public override Task Insert(TEntity entity)
        {
            this.Context.Set<TEntity>().Add(entity);
            return Task.CompletedTask;
        }

        public override Task Update(TEntity entity)
        {
            this.Context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        protected override IQueryable<TEntity> All()
        {
            return this.Context.Set<TEntity>();
        }
    }
}
