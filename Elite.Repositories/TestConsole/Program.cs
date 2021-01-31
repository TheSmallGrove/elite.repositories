using Microsoft.Extensions.DependencyInjection;
using Elite.Repositories.EntityFramework;
using System;
using TestConsole.Repositories;
using Microsoft.EntityFrameworkCore;
using Elite.Repositories.Abstractions;
using System.Threading.Tasks;
using Elite.Repositories.EntityFramework.Criterias;

namespace TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddEntityRepository<ChinookDbContext>(
                builder => builder.UseSqlite("Data Source=.\\chinook.db;"));

            services
                .AddRepository<ITracksRepository, TracksRepository>();

            IServiceProvider provider = services.BuildServiceProvider();

            var factory = provider.GetRequiredService<IUnitOfWorkFactory>();
            var paging = new PagingCriteria { PageIndex = 0, PageSize = 10 };
            var sorting = new SortingCriteria { Properties = new string[] { "Name asc" } };

            while (true)
            {
                using (var uow = factory.BeginUnitOfWork())
                {
                    var tracks = uow.GetRepository<ITracksRepository>();
                    var set = await tracks.GetByCriteriaAsync(sorting, paging);

                    Console.Clear();

                    foreach (var item in set)
                        Console.WriteLine($"{item.TrackId.ToString().PadLeft(10)} | {item.Name.PadRight(100)} | {item.UnitPrice}");
                }

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    return;
                else if (key.Key == ConsoleKey.LeftArrow && paging.PageIndex > 0)
                    paging.PageIndex--;
                else if (key.Key == ConsoleKey.RightArrow)
                    paging.PageIndex++;
                else if (key.Key == ConsoleKey.UpArrow)
                    sorting.Properties = new string[] { "Name asc" };
                else if (key.Key == ConsoleKey.DownArrow)
                    sorting.Properties = new string[] { "Name desc" };
            }
        }
    }
}
