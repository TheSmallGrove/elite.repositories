using System;
using System.Linq;

namespace Elite.Repositories.Abstractions
{
    public interface ICriteriaExecutor
    {
        Type CriteriaType { get; }

        IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, ICriteria arguments)
            where TEntity : class, IEntity;
    }
}
