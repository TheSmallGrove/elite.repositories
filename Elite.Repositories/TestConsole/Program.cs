using Microsoft.Extensions.DependencyInjection;
using Elite.Repositories.EntityFramework;
using System;
using TestConsole.Repositories;
using Microsoft.EntityFrameworkCore;
using Elite.Repositories.Abstractions;
using System.Threading.Tasks;
using Elite.Repositories.EntityFramework.Criterias;
using Elite.Repositories.Abstractions.Criterias;

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
            var paging = new PagingCriteria { PageIndex = 0, PageSize = 25 };
            var sorting = new SortingCriteria { Properties = new string[] { "Genre.Name asc" } };

            while (true)
            {
                using (var uow = factory.BeginUnitOfWork())
                {
                    var tracks = uow.GetRepository<ITracksRepository>();
                    var set = await tracks.GetByCriteriaAsync("new (TrackId, Name, UnitPrice, Genre.Name as Genre)", sorting, paging);
                    var count = await tracks.CountByCriteriaAsync();
                    PagingInfo info = paging.Calculate(count);

                    Console.Clear();

                    foreach (var item in set)
                        Console.WriteLine($"{item.TrackId.ToString().PadLeft(10)} | {item.Name.PadRight(100)} | {item.Genre.PadRight(20)} | {item.UnitPrice}");

                    Console.WriteLine($"Page {info.CurrentPage}/{info.VirtualNumberOfPages} di records {info.VirtualNumberOfRecords} ({Math.Round(info.Percentage * 100, 1)}%)");

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                        return;
                    else if (key.Key == ConsoleKey.LeftArrow)
                        paging = info.GoToPrevious();
                    else if (key.Key == ConsoleKey.RightArrow)
                        paging = info.GoToNext();
                    else if (key.Key == ConsoleKey.PageUp)
                        paging = info.GoToFirst();
                    else if (key.Key == ConsoleKey.PageDown)
                        paging = info.GoToLast();
                    else if (key.Key == ConsoleKey.UpArrow)
                        sorting.Properties = new string[] { "Genre.Name asc" };
                    else if (key.Key == ConsoleKey.DownArrow)
                        sorting.Properties = new string[] { "Genre.Name desc" };
                }
            }
        }
    }
}
