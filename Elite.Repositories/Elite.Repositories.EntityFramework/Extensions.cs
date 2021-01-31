using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Criterias;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly:InternalsVisibleTo("Elite.Repositories.EntityFramework.Tests")]

namespace Elite.Repositories.EntityFramework
{
    public static class Extensions
    {
        public static IServiceCollection AddEntityRepository<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> builder = null)
            where TContext: DbContext
        {
            services.AddDbContext<TContext>(options => builder?.Invoke(options));

            services
                .AddTransient<IUnitOfWorkFactory, EntityUnitOfWorkFactory>()
                .AddTransient<IUnitOfWork, EntityUnitOfWork<TContext>>(o => new EntityUnitOfWork<TContext>(o.CreateScope()));

            services
                .AddTransient<ICriteriaExecutor, PagingCriteriaExecutor>()
                .AddTransient<ICriteriaExecutor, SortingCriteriaExecutor>()
                .AddSingleton<ICriteriaExecutorResolver, CriteriaExecutorResolver>();

            return services;
        }

        public static IServiceCollection AddRepository<TInterface, TClass>(this IServiceCollection services)
            where TInterface: class
            where TClass: class, TInterface
        {
            return services.AddTransient<TInterface, TClass>();
        }
    }
}
