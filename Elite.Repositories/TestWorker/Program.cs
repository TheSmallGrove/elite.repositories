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
                    services.AddEntityRepository<TestDbContext>(
                        builder => builder.UseSqlite("Data Source=.\\database.db;"));

                    services
                        .AddRepository<IProductRepository, ProductRepository>();

                    services
                        .AddHostedService<Worker>();
                });
    }

    public interface IProductRepository : IRepository<Product, (string Id, string IdGroup)>
    {
        Task<IEnumerable<Product>> GetAllAsync();
    }

    public class ProductRepository : EntityRepository<Product, (string Id, string IdGroup)>, IProductRepository
    {
        public ProductRepository(TestDbContext context) 
            : base(context)
        { }

        public override Task<Product> GetByKeyAsync((string Id, string IdGroup) key)
        {
            return (from entity in this.Set
                    where entity.Id == key.Id && entity.IdGroup == key.IdGroup
                    select entity).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await this.Set.ToArrayAsync();
    }
}
