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
            throw new NotImplementedException();
        }

        public override Task Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<TEntity> All()
        {
            return this.Context.Set<TEntity>();
        }
    }
}
