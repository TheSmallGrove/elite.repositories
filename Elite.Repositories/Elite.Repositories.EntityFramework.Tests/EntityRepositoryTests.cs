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
                int countResult;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    countResult = await items.CountAllAsync();
                }

                // ASSERT
                countResult.Should().Be(count);
            }
        }

        [Fact]
        public async Task DeleteByKeyAsync_Should_Delete_The_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                Item found;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    await items.DeleteByKeyAsync(50);
                    countAfterDelete = await items.CountAllAsync();
                    found = await items.GetByKeyAsync(50);
                }

                // ASSERT
                countAfterDelete.Should().Be(count - 1);
                found.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_The_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                Item found;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    var temp = await items.GetByKeyAsync(50);
                    await items.DeleteAsync(temp);
                    countAfterDelete = await items.CountAllAsync();
                    found = await items.GetByKeyAsync(50);
                }

                // ASSERT
                countAfterDelete.Should().Be(count - 1);
                found.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                IEnumerable<Item> batchBeforeDelete, batchAfterDelete;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    batchBeforeDelete = await items.GetBatch(50, 69);
                    await items.DeleteAsync(batchBeforeDelete.ToArray());
                    countAfterDelete = await items.CountAllAsync();
                    batchAfterDelete = await items.GetBatch(50, 69);
                }

                // ASSERT
                countAfterDelete.Should().Be(count - batchBeforeDelete.Count());
                batchAfterDelete.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task InsertAsync_Should_Add_A_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert;
                Item found;
                Item newItem = new Item
                {
                    Id = 1000,
                    Letter = "Z",
                    Name = "Zelda",
                    Size = 5
                };

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    await items.InsertAsync(newItem);
                    countAfterInsert = await items.CountAllAsync();
                    found = await items.GetByKeyAsync(newItem.Id);
                }

                // ASSERT
                countAfterInsert.Should().Be(count + 1);
                found.Should().NotBeNull();
                found.Id.Should().Equals(newItem.Id);
                found.Name.Should().Equals(newItem.Name);
                found.Letter.Should().Equals(newItem.Letter);
                found.Size.Should().Equals(newItem.Size);
            }
        }

        [Fact]
        public async Task InsertAsync_Should_Add_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();
                
                int countAfterInsert;

                IEnumerable<Item> batchToInsert =
                    (from i in Enumerable.Range(1000, 20)
                     select new Item
                     {
                         Id = i,
                         Letter = "X",
                         Name = $"X{i}",
                         Size = 0
                     }).ToArray();

                IEnumerable<Item> batchAfterInsert;

                // ACT

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    await items.InsertAsync(batchToInsert.ToArray());
                    countAfterInsert = await items.CountAllAsync();
                    batchAfterInsert = await items.GetBatch(1000, 1019);
                }

                // ASSERT
                countAfterInsert.Should().Be(count + batchToInsert.Count());
                batchAfterInsert.Should().BeEquivalentTo(batchToInsert);
            }
        }

        [Fact]
        public async Task UpdateAsync_Should_Change_A_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterUpdate;
                Item found, oldItem;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();

                    oldItem = await items.GetByKeyAsync(50);
                    oldItem.Name = "DUMMY";
                    oldItem.Letter = "D";
                    oldItem.Size = 5;

                    await items.UpdateAsync(oldItem);
                    countAfterUpdate = await items.CountAllAsync();
                    found = await items.GetByKeyAsync(oldItem.Id);
                }

                // ASSERT
                countAfterUpdate.Should().Be(count);
                found.Should().NotBeNull();
                found.Id.Should().Equals(oldItem.Id);
                found.Name.Should().Equals(oldItem.Name);
                found.Letter.Should().Equals(oldItem.Letter);
                found.Size.Should().Equals(oldItem.Size);
            }
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                int countAfterUpdate;

                IEnumerable<Item> batchToUpdate, batchAfterUpdate;

                // ACT

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();
                    batchToUpdate = await items.GetBatch(50, 69);

                    foreach (var item in batchToUpdate)
                        item.Name += "TEST";

                    await items.UpdateAsync(batchToUpdate.ToArray());

                    countAfterUpdate = await items.CountAllAsync();

                    batchAfterUpdate = await items.GetBatch(50, 69);
                }

                // ASSERT
                countAfterUpdate.Should().Be(count);
                batchAfterUpdate.Should().BeEquivalentTo(batchToUpdate);
            }
        }

        [Fact]
        public async Task CompleteAsync_Should_Commit_Changes()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert, countAfterCommit;
                Item found;
                Item newItem = new Item
                {
                    Id = 1000,
                    Letter = "Z",
                    Name = "Zelda",
                    Size = 5
                };

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();

                    using (var transaction = await uow.BeginTransaction())
                    {
                        await items.InsertAsync(newItem);
                        countAfterInsert = await items.CountAllAsync();
                        await transaction.CompleteAsync();
                    }

                    countAfterCommit = await items.CountAllAsync();
                    found = await items.GetByKeyAsync(newItem.Id);
                }

                // ASSERT
                countAfterInsert.Should().Be(count + 1);
                countAfterCommit.Should().Be(count + 1);
                found.Should().NotBeNull();
                found.Id.Should().Equals(newItem.Id);
                found.Name.Should().Equals(newItem.Name);
                found.Letter.Should().Equals(newItem.Letter);
                found.Size.Should().Equals(newItem.Size);
            }
        }

        [Fact]
        public async Task Dispose_Should_Rollback_Changes()
        {
            int count = 100;

            using (var data = await this.Fixture.Setup(count))
            {
                // ARRANGE
                var container = this.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert, countAfterCommit;
                Item found;
                Item newItem = new Item
                {
                    Id = 1000,
                    Letter = "Z",
                    Name = "Zelda",
                    Size = 5
                };

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IItemsRepository>();

                    using (var transaction = await uow.BeginTransaction())
                    {
                        await items.InsertAsync(newItem);
                        countAfterInsert = await items.CountAllAsync();
                    }

                    countAfterCommit = await items.CountAllAsync();
                    found = await items.GetByKeyAsync(newItem.Id);
                }

                // ASSERT
                countAfterInsert.Should().Be(count + 1);
                countAfterCommit.Should().Be(count);
                found.Should().BeNull();
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
