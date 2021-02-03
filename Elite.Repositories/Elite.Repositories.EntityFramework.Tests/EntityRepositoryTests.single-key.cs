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
    public partial class EntityRepositoryTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task SingleKey_GetByKeyAsync_Should_Return_Exact_Match()
        {
            using (var data = await this.Fixture.SetupSingleKey(100))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                SingleKeyItem item;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
        public async Task SingleKey_DeleteByKeyAsync_Should_Delete_The_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                SingleKeyItem found;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
        public async Task SingleKey_DeleteAsync_Should_Delete_The_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                SingleKeyItem found;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
        public async Task SingleKey_DeleteAsync_Should_Delete_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                IEnumerable<SingleKeyItem> batchBeforeDelete, batchAfterDelete;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
        public async Task SingleKey_InsertAsync_Should_Add_A_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert;
                SingleKeyItem found;
                SingleKeyItem newItem = new SingleKeyItem
                {
                    Id = 1000,
                    Letter = "Z",
                    Name = "Zelda",
                    Size = 5
                };

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
        public async Task SingleKey_InsertAsync_Should_Add_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();
                
                int countAfterInsert;

                IEnumerable<SingleKeyItem> batchToInsert =
                    (from i in Enumerable.Range(1000, 20)
                     select new SingleKeyItem
                     {
                         Id = i,
                         Letter = "X",
                         Name = $"X{i}",
                         Size = 0
                     }).ToArray();

                IEnumerable<SingleKeyItem> batchAfterInsert;

                // ACT

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
        public async Task SingleKey_UpdateAsync_Should_Change_A_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterUpdate;
                SingleKeyItem found, oldItem;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();

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
        public async Task SingleKey_UpdateAsync_Should_Update_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                int countAfterUpdate;

                IEnumerable<SingleKeyItem> batchToUpdate, batchAfterUpdate;

                // ACT

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
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
    }
}
