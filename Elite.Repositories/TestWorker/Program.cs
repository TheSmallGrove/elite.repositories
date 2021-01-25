using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework;
using Elite.Repositories.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<TestDbContext>(builder =>
                    {
                        builder.UseInMemoryDatabase("data");
                    });

                    services
                        .AddSingleton<IRepositoryFactory, ServiceProviderRepositoryFactory>()
                        .AddTransient<IUnitOfWork, EntityUnitOfWork<TestDbContext>>()
                        .AddTransient<IProductRepository, ProductRepository>()
                        .AddHostedService<Worker>();
                });
    }

    public interface IProductRepository : IRepository
    { }

    public class ProductRepository : EntityRepository<Product>, IProductRepository
    {
        public ProductRepository(TestDbContext context) : base(context)
        {
        }
    }
}
