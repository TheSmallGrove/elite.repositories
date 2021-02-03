using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Elite.Repositories.EntityFramework.Tests.Repositories;

namespace Elite.Repositories.EntityFramework.Tests
{
    public static class TestUtilities
    {
        public static IServiceProvider BuildServiceProvider(TestFixture.IDatabaseSetup data)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddEntityRepository<NamesDbContext>(
                builder => builder.UseSqlite(data.ConnectionString));

            services
                .AddRepository<IMultiKeyItemsRepository, MultiKeyItemsRepository>()
                .AddRepository<ISingleKeyItemsRepository, SingleKeyItemsRepository>();

            return services.BuildServiceProvider();
        }
    }
}
