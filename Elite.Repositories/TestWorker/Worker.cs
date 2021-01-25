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
        private readonly ILogger<Worker> _logger;

        private IServiceScope Provider { get; }

        public Worker(ILogger<Worker> logger, IServiceProvider provider)
        {
            _logger = logger;
            this.Provider = provider.CreateScope();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var unitOfWork = this.Provider.ServiceProvider.GetService<IUnitOfWork>();

//            using(var session = await unitOfWork.BeginAsync())
//            {
                var produts = unitOfWork.CreateRepository<IProductRepository>();

//            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            this.Provider.Dispose();
            base.Dispose();
        }
    }
}
