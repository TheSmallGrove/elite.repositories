using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework;
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
                        builder.UseSqlite("Data Source=.\\database.db;");
                    });

                    services
                        .AddTransient<IUnitOfWorkFactory, EntityUnitOfWorkFactory>()
                        .AddTransient<IUnitOfWork, EntityUnitOfWork<TestDbContext>>(o => new EntityUnitOfWork<TestDbContext>(o.CreateScope()))
                        .AddTransient<IProductRepository, ProductRepository>()
                        .AddHostedService<Worker>();
                });
    }

    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetAll();
    }

    public class ProductRepository : EntityRepository<Product>, IProductRepository
    {
        public ProductRepository(TestDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetAll()
        {
            return this.All().ToArray();
        }
    }
}
