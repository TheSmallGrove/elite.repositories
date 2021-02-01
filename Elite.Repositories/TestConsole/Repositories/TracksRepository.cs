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
    public interface ITracksRepository : IRepository<Track, int>
    { }

    class TracksRepository : EntityRepository<Track, int>, ITracksRepository
    {
        public TracksRepository(ChinookDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        public override Task<Track> GetByKeyAsync(int key)
        {
            return (from entity in this.Set
                    where entity.TrackId == key
                    select entity).SingleOrDefaultAsync();
        }

        public override async Task DeleteByKeyAsync(int key)
        {
            var entity = await this.GetByKeyAsync(key);
            await base.DeleteAsync(entity);
        }
    }
}
