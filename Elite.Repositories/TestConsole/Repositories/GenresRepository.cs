using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestConsole.Entities;

namespace TestConsole.Repositories
{
    public interface IGenresRepository : IRepository<Genre, int>
    { }

    class GenresRepository : EntityRepository<Genre, int>, IGenresRepository
    {
        public GenresRepository(ChinookDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        public override Task<Genre> GetByKeyAsync(int key)
        {
            return (from entity in this.Set
                    where entity.GenreId == key
                    select entity).SingleOrDefaultAsync();
        }

        public override async Task DeleteByKeyAsync(int key)
        {
            var entity = await this.GetByKeyAsync(key);
            await base.DeleteAsync(entity);
        }
    }
}
