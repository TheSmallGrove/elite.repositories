using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class PagingCriteria : ICriteria
    {
        public string Name => "paging";

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, dynamic arguments)
            where TEntity : class, IEntity
        {
            int pageSize = arguments.PageSize;
            int pageIndex = arguments.PageIndex;
            return query.Skip(pageSize * pageIndex).Take(pageSize);
        }
    }
}
