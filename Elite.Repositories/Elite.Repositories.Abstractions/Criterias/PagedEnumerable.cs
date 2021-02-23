using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions.Criterias
{

    public class PagedResult<T> : IPagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public PagingInfo Info { get; }

        public PagedResult(IEnumerable<T> items, PagingInfo info)
        {
            this.Items = items;
            this.Info = info;
        }
    }
}
