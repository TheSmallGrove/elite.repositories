using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface IUnitOfWorkTransaction: IDisposable
    {
        Task CompleteAsync();
        bool IsCompleted { get; }
    }
}
