using System.Linq;

namespace Elite.Repositories.Abstractions
{
    public interface ICriteriaExecutor
    { }

    public interface ICriteriaExecutor<TCriteria> : ICriteriaExecutor
        where TCriteria : ICriteria
    {
        IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, TCriteria arguments)
            where TEntity : class, IEntity;
    }
}
