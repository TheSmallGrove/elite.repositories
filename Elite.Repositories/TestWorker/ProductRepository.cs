using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWorker
{
    public interface IProductRepository : IRepository<Product, (string Id, string IdGroup)>
    {
    }

    class ProductRepository : EntityRepository<Product, (string Id, string IdGroup)>, IProductRepository
    {
        public ProductRepository(TestDbContext context, ICriteriaExecutorResolver criteriaResolver) 
            : base(context, criteriaResolver)
        { }

        public override Task<Product> GetByKeyAsync((string Id, string IdGroup) key)
        {
            return (from entity in this.Set
                    where entity.Id == key.Id && entity.IdGroup == key.IdGroup
                    select entity).SingleOrDefaultAsync();
        }

        public override async Task DeleteByKeyAsync((string Id, string IdGroup) key)
        {
            var entity = await this.GetByKeyAsync(key);
            await base.DeleteAsync(entity);
        }
    }
}