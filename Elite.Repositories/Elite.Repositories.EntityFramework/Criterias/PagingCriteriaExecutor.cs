using Elite.Repositories.Abstractions;
using System;
using System.Linq;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class PagingCriteriaExecutor : ICriteriaExecutor
    {
        public Type CriteriaType => typeof(PagingCriteria);

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, ICriteria criteria)
            where TEntity : class, IEntity
        {
            PagingCriteria arguments = criteria as PagingCriteria;
            int pageSize = arguments.PageSize;
            int pageIndex = arguments.PageIndex;
            return query.Skip(pageSize * pageIndex).Take(pageSize);
        }
    }
}
