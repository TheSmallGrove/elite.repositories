using Elite.Repositories.Abstractions;
using System.Linq;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class PagingCriteriaExecutor : ICriteriaExecutor<PagingCriteria>
    {
        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, PagingCriteria arguments)
            where TEntity : class, IEntity
        {
            int pageSize = arguments.PageSize;
            int pageIndex = arguments.PageIndex;
            return query.Skip(pageSize * pageIndex).Take(pageSize);
        }
    }
}
