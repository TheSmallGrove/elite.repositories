using Elite.Repositories.Abstractions;
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

                var test = products1.GetAll();

                using (var transaction = await unitOfWork.BeginTransaction())
                {
                    var products = unitOfWork.GetRepository<IProductRepository>();
                    var test1 = products.GetAll();

                    await transaction.CompleteAsync();
                }
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
