using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class SortingCriteriaExecutor : ICriteriaExecutor<SortingCriteria>
    {
        public string Name => "sorting";

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, SortingCriteria arguments)
            where TEntity : class, IEntity
        {
            if (arguments.Properties.Any())
                return query.OrderBy(string.Join(',', arguments.Properties));

            return query;
        }
    }
}
