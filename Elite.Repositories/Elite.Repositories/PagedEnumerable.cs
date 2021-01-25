using System.Collections.Generic;

namespace Elite.Repositories
{
    public interface IPagedEnumerable<out T>
    {
        int VirtualItemCount { get; }
        IEnumerable<T> Items { get; }
    }

    public class PagedEnumerable<T> : IPagedEnumerable<T>
    {
        public IEnumerable<T> Items { get; }
        public int VirtualItemCount { get; }

        public PagedEnumerable(int virtualItemCount, IEnumerable<T> items)
        {
            this.VirtualItemCount = virtualItemCount;
            this.Items = items;
        }
    }
}
