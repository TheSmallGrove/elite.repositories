using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Elite.Repositories.EntityFramework.Tests.Repositories
{
    public interface IMultiKeyItemsRepository : IRepository<MultiKeyItem, (int Id, int GroupId)>
    {
        Task<IEnumerable<MultiKeyItem>> GetBatch(int min, int max, int groupId);
    }

    class MultiKeyItemsRepository : EntityRepository<MultiKeyItem, (int Id, int GroupId)>, IMultiKeyItemsRepository
    {
        public MultiKeyItemsRepository(NamesDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        protected override Expression<Func<MultiKeyItem, bool>> MatchKey((int Id, int GroupId) key)
            => _ => _.Id == key.Id && _.GroupId == key.GroupId;

        protected override Expression<Func<MultiKeyItem, bool>> MatchKeys(params (int Id, int GroupId)[] keys)
            => _ => keys.Select(o => o.Id).Contains(_.Id) && keys.Select(o => o.GroupId).Contains(_.GroupId); // todo: improve here

        public async Task<IEnumerable<MultiKeyItem>> GetBatch(int min, int max, int groupId)
        {
            return await (from i in this.Set
                          where i.Id >= min && i.Id <= max && i.GroupId == groupId
                          select i).ToArrayAsync();
        }
    }
}
