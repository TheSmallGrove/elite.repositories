using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Criterias;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestWorker
{
    public class Worker : BackgroundService
    {
        private IUnitOfWorkFactory Factory { get; }

        public Worker(IUnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var unitOfWork = this.Factory.BeginUnitOfWork())
            {
                var products1 = unitOfWork.GetRepository<IProductRepository>();

                try
                {
                    var test = await products1.GetByCriteria(
                        new PagingCriteria{ PageIndex = 0, PageSize = 2 },
                        new SortingCriteria { Properties = new string[] { "Name desc" } });

                    foreach(var i in test)
                        Console.WriteLine(i);
                }
                catch(Exception)
                {
                    throw;
                }
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public class StoreManager
    {
        private IUnitOfWorkFactory Factory { get; }

        public StoreManager(IUnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }

        public Task<IEnumerable<Category>> GetHomeCategories()
        {
            using (var unitOfWork = this.Factory.BeginUnitOfWork())
            {
                var categoryRepo = unitOfWork.GetRepository<ICategoryRepository>();
                return categoryRepo.GetAllAsync();
            }
        }

        public async Task AddProducts(IEnumerable<Product> products)
        {
            using (var unitOfWork = this.Factory.BeginUnitOfWork())
            {
                using (var transaction = await unitOfWork.BeginTransaction())
                {
                    var productRepo = unitOfWork.GetRepository<IProductRepository>();
                    await productRepo.InsertAsync(products.ToArray());
                    await transaction.CompleteAsync();
                }
            }
        }
    }
}
