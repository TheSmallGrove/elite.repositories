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
    public interface IGenresRepository : IRepository<Genre, int>
    { }

    class GenresRepository : EntityRepository<Genre, int>, IGenresRepository
    {
        public GenresRepository(ChinookDbContext context, ICriteriaExecutorResolver criteriaResolver)
            : base(context, criteriaResolver)
        { }

        protected override Expression<Func<Genre, bool>> MatchKey(int key) 
            => _ => _.GenreId == key;

        protected override Expression<Func<Genre, bool>> MatchKeys(params int[] keys)
            => _ => keys.Contains(_.GenreId);
    }
}
