using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWorker
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<Category> GetByName(string name);
    }

    class CategoryRepository : EntityRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(TestDbContext context, ICriteriaResolver criteriaResolver) 
            : base(context, criteriaResolver)
        { }

        public override Task<Category> GetByKeyAsync(int key)
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

        public Task<Category> GetByName(string name)
        {
            return (from entity in this.Set
                    where entity.Name == name
                    select entity).SingleOrDefaultAsync();
        }
    }
}