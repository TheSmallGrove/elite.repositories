using Elite.Repositories.Abstractions;
using Elite.Repositories.Abstractions.Criterias;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class PagingCriteriaExecutor : ICriteriaExecutor
    {
        public Type CriteriaType => typeof(PagingCriteria);

        public IQueryable Apply(IQueryable query, ICriteria criteria)
        {
            PagingCriteria arguments = criteria as PagingCriteria;
            int pageSize = arguments.PageSize;
            int pageIndex = arguments.PageIndex;
            return query.Skip(pageSize * pageIndex).Take(pageSize);
        }
    }
}
