using Elite.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework
{
    public class EntityUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IServiceProvider Provider { get; }

        public EntityUnitOfWorkFactory(IServiceProvider provider)
        {
            this.Provider = provider;
        }

        public IUnitOfWork BeginUnitOfWork()
        {
            return this.Provider.GetRequiredService<IUnitOfWork>();
        }
    }
}
