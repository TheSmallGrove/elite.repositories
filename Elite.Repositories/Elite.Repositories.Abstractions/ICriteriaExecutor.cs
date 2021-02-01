using System;
using System.Linq;

namespace Elite.Repositories.Abstractions
{
    public interface ICriteriaExecutor
    {
        Type CriteriaType { get; }

        IQueryable Apply(IQueryable query, ICriteria criteria);
    }
}
