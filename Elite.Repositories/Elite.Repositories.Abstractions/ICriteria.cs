using System.Linq;

namespace Elite.Repositories.Abstractions
{
    public interface ICriteria
    {
        string Name { get; }
        IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, dynamic arguments)
            where TEntity : class, IEntity;
    }
}
