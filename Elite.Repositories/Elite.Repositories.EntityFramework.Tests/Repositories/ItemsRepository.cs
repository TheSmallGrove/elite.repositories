using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Tests.Repositories
{
    public interface IItemsRepository : IRepository<Item, int>
    {
        Task<IEnumerable<Item>> GetBatch(int min, int max);
    }

    class ItemsRepository : EntityRepository<Item, int>, IItemsRepository
    {
        public ItemsRepository(NamesDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        public override Task<Item> GetByKeyAsync(int key)
        {
            return (from entity in this.Set
                    where entity.Id == key
                    select entity).SingleOrDefaultAsync();
        }

        public override async Task DeleteByKeyAsync(int key)
        {
            var entity = await this.GetByKeyAsync(key);
            await base.DeleteAsync(entity);
        }

        public async Task<IEnumerable<Item>> GetBatch(int min, int max)
        {
            return await (from i in this.Set
                          where i.Id >= min && i.Id <= max
                          select i).ToArrayAsync();
        }
    }
}
