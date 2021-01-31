using Elite.Repositories.Abstractions;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class RestrictionCriteriaExecutor : ICriteriaExecutor
    {
        public Type CriteriaType => typeof(RestrictionCriteria);

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, ICriteria criteria)
            where TEntity : class, IEntity
        {
            RestrictionCriteria arguments = criteria as RestrictionCriteria;
            return query.Where(arguments.RestrictionTemplate, arguments.Arguments);
        }
    }
}
