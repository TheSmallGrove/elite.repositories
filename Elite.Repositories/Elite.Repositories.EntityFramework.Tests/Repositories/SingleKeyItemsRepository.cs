using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Tests.Repositories
{
    public interface ISingleKeyItemsRepository : IRepository<SingleKeyItem, int>
    {
        Task<IEnumerable<SingleKeyItem>> GetBatch(int min, int max);
    }

    class SingleKeyItemsRepository : EntityRepository<SingleKeyItem, int>, ISingleKeyItemsRepository
    {
        public SingleKeyItemsRepository(NamesDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        protected override Expression<Func<SingleKeyItem, bool>> MatchKey(int key) 
            => _ => _.Id == key;

        protected override Expression<Func<SingleKeyItem, bool>> MatchKeys(params int[] keys)
            => _ => keys.Contains(_.Id);

        public async Task<IEnumerable<SingleKeyItem>> GetBatch(int min, int max)
        {
            return await (from i in this.Set
                          where i.Id >= min && i.Id <= max
                          select i).ToArrayAsync();
        }
    }
}
