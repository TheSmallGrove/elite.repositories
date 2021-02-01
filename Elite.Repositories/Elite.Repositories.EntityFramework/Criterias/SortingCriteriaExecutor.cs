using Elite.Repositories.Abstractions;
using Elite.Repositories.Abstractions.Criterias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class SortingCriteriaExecutor : ICriteriaExecutor
    {
        public Type CriteriaType => typeof(SortingCriteria);

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, ICriteria criteria)
            where TEntity : class, IEntity
        {
            SortingCriteria arguments = criteria as SortingCriteria;

            if (arguments.Properties.Any())
                return query.OrderBy(string.Join(',', arguments.Properties));

            return query;
        }
    }
}
