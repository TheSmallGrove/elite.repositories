using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Elite.Repositories.EntityFramework.Tests.Repositories;
using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Tests.Entities;

namespace Elite.Repositories.EntityFramework.Tests
{
    public class EntityRepositoryTests : IClassFixture<TestFixture>
    {
        public TestFixture Fixture { get; set; }

        public EntityRepositoryTests(TestFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task GetByKeyAsync_Should_Return_Exact_Match()
        {
            using (var data = await this.Fixture.Setup(100))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                Item item;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    item = await items.GetByKeyAsync(50);
                }

                // ASSERT
                item.Should().NotBeNull();
                item.Id.Should().Be(50);
                item.Letter.Should().Be("X");
                item.Name.Should().Be("Xylia");
                item.Size.Should().Be(5);
            }
        }

        [Fact]
        public async Task CountAllAsync_Should_Return_Exact_Number_Of_Items()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int item;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    item = await items.CountAllAsync();
                }

                // ASSERT
                item.Should().Be(count);
            }
        }

        private IServiceProvider BuildServiceProvider(TestFixture.IDatabaseSetup data)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddEntityRepository<NamesDbContext>(
                builder => builder.UseSqlite(data.ConnectionString));

            services
                .AddRepository<IItemsRepository, ItemsRepository>();

            return services.BuildServiceProvider();
        }
    }
}
