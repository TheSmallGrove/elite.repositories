using Elite.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework
{
    public static class Extensions
    {
        public static IServiceCollection AddEntityRepository<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> builder)
            where TContext: DbContext
        {
            services.AddDbContext<TContext>(_ => builder(_));

            return services
                .AddTransient<IUnitOfWorkFactory, EntityUnitOfWorkFactory>()
                .AddTransient<IUnitOfWork, EntityUnitOfWork<TContext>>(o => new EntityUnitOfWork<TContext>(o.CreateScope()));
        }

        public static IServiceCollection AddRepository<TInterface, TClass>(this IServiceCollection services)
            where TInterface: class
            where TClass: class, TInterface
        {
            return services.AddTransient<TInterface, TClass>();
        }
    }
}
