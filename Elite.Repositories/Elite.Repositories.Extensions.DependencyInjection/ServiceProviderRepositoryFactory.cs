using Elite.Repositories.Abstractions;
using System;

namespace Elite.Repositories.Extensions.DependencyInjection
{
    public class ServiceProviderRepositoryFactory : IRepositoryFactory
    {
        private IServiceProvider Provider { get; }

        public ServiceProviderRepositoryFactory(IServiceProvider provider)
        {
            this.Provider = provider;
        }

        public T CreateRepository<T>() where T : IRepository
        {
            return (T)this.Provider.GetService(typeof(T));
        }
    }
}
