using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class SortingCriteria : ICriteria
    {
        public string Name => "sorting";

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, dynamic arguments)
            where TEntity : class, IEntity
        {
            if (arguments.Direction == "ASC")
                return query.OrderBy((string)arguments.Direction, (string[])arguments.Columns);

            throw new NotSupportedException($"Direction {arguments.Direction} is not supported");
        }
    }
}
