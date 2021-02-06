using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestConsole.Entities;
using System.Linq.Expressions;

namespace TestConsole.Repositories
{
    public interface ITracksRepository : IRepository<Track, int>
    { }

    class TracksRepository : EntityRepository<Track, int>, ITracksRepository
    {
        public TracksRepository(ChinookDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        protected override Expression<Func<Track, bool>> MatchKey(int key) 
            => _ => _.TrackId == key;

        protected override Expression<Func<Track, bool>> MatchKeys(params int[] keys)
            => _ => keys.Contains(_.TrackId);
    }
}
