using Elite.Repositories.Abstractions;
using Elite.Repositories.Abstractions.Criterias;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Elite.Repositories.EntityFramework.Criterias
{
    class RestrictionCriteriaExecutor : ICriteriaExecutor
    {
        public Type CriteriaType => typeof(RestrictionCriteria);

        public IQueryable Apply(IQueryable query, ICriteria criteria)
        {
            RestrictionCriteria arguments = criteria as RestrictionCriteria;
            return query.Where(arguments.RestrictionTemplate, arguments.Arguments);
        }
    }
}
