using Elite.Repositories.Abstractions;
using System.Linq;

namespace Elite.Repositories.EntityFramework
{
    internal class NullCriteriaResolver : ICriteriaResolver
    {
        public ICriteria Resolve(string name)
        {
            return new NullCriteria();
        }

        class NullCriteria : ICriteria
        {
            public string Name => "null";

            IQueryable<TEntity> ICriteria.Apply<TEntity>(IQueryable<TEntity> query, object arguments)
            {
                return query;
            }
        }
    }
}