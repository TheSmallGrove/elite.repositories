using System;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface IUnitOfWorkFactory
    {
        public IUnitOfWork BeginUnitOfWork();
    }
}
