//using Elite.Repositories.Abstractions;
//using Microsoft.Extensions.DependencyInjection;
//using System;

//namespace Elite.Repositories.Extensions.DependencyInjection
//{
//    public class ServiceProviderRepositoryFactory : IUnitOfWorkFactory
//    {
//        private Guid id = Guid.NewGuid();

//        private IServiceProvider Provider { get; }

//        public ServiceProviderRepositoryFactory(IServiceProvider provider)
//        {
//            this.Provider = provider;
//            var scope = provider.CreateScope();
//        }

//        public IUnitOfWork CreateUnitOfWork()
//        {
//            return new EntityUnitOfWork(this);
//        }

//        public T GetRepository<T>() where T : IRepository
//        {
//            return this.Provider.GetRequiredService<T>();
//        }
//    }
//}