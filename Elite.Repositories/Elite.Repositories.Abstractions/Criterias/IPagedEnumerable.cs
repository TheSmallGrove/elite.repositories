using System.Collections.Generic;

namespace Elite.Repositories.Abstractions.Criterias
{
    public interface IPagedResult<out T>
    {
        PagingInfo Info { get; }
        IEnumerable<T> Items { get; }
    }
}
