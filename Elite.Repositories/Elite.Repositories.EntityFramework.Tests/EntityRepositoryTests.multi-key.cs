using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Elite.Repositories.EntityFramework.Tests.Repositories;
using Elite.Repositories.Abstractions;
using Elite.Repositories.EntityFramework.Tests.Entities;

namespace Elite.Repositories.EntityFramework.Tests
{
    public partial class EntityRepositoryTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task MultiKey_ExistsByKeyAsync_Should_Return_True_If_Match()
        {
            using (var data = await this.Fixture.SetupMultiKey(100))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                bool exists;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    exists = await items.ExistsByKeyAsync((50, 1));
                }

                // ASSERT
                exists.Should().BeTrue();
            }
        }

        [Fact]
        public async Task MultiKey_ExistsByKeyAsync_Should_Return_False_If_Match()
        {
            using (var data = await this.Fixture.SetupMultiKey(100))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                bool exists;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    exists = await items.ExistsByKeyAsync((50, 2));
                }

                // ASSERT
                exists.Should().BeFalse();
            }
        }

        [Fact]
        public async Task MultiKey_GetByKeyAsync_Should_Return_Exact_Match()
        {
            using (var data = await this.Fixture.SetupMultiKey(100))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                MultiKeyItem item;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    item = await items.GetByKeyAsync((50, 1));
                }

                // ASSERT
                item.Should().NotBeNull();
                item.Id.Should().Be(50);
                item.GroupId.Should().Be(1);
                item.Letter.Should().Be("X");
                item.Name.Should().Be("Xylia");
                item.Size.Should().Be(5);
            }
        }

        [Fact]
        public async Task MultiKey_GetByKeyAsync_Should_Return_Exact_Match_With_Many_Items()
        {
            using (var data = await this.Fixture.SetupMultiKey(100))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                IEnumerable<MultiKeyItem> item;
                (int, int)[] keys = { (50, 1), (60, 1), (70, 1) };

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    item = await items.GetByKeyAsync(keys);
                }

                // ASSERT
                item.Should().HaveCount(keys.Length);
                item.ElementAt(0).Id.Should().Be(keys[0].Item1);
                item.ElementAt(0).GroupId.Should().Be(keys[0].Item2);
                item.ElementAt(1).Id.Should().Be(keys[1].Item1);
                item.ElementAt(1).GroupId.Should().Be(keys[1].Item2);
                item.ElementAt(2).Id.Should().Be(keys[2].Item1);
                item.ElementAt(2).GroupId.Should().Be(keys[2].Item2);
            }
        }

        [Fact]
        public async Task MultiKey_DeleteByKeyAsync_Should_Delete_The_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                MultiKeyItem found;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    await items.DeleteByKeyAsync((50, 1));
                    countAfterDelete = await items.CountAllAsync();
                    found = await items.GetByKeyAsync((50, 1));
                }

                // ASSERT
                countAfterDelete.Should().Be(count - 1);
                found.Should().BeNull();
            }
        }

        [Fact]
        public async Task MultiKey_DeleteAsync_Should_Delete_The_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                MultiKeyItem found;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    var temp = await items.GetByKeyAsync((50, 1));
                    await items.DeleteAsync(temp);
                    countAfterDelete = await items.CountAllAsync();
                    found = await items.GetByKeyAsync((50, 1));
                }

                // ASSERT
                countAfterDelete.Should().Be(count - 1);
                found.Should().BeNull();
            }
        }

        [Fact]
        public async Task MultiKey_DeleteAsync_Should_Delete_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterDelete;
                IEnumerable<MultiKeyItem> batchBeforeDelete, batchAfterDelete;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    batchBeforeDelete = await items.GetBatch(50, 69, 1);
                    await items.DeleteAsync(batchBeforeDelete.ToArray());
                    countAfterDelete = await items.CountAllAsync();
                    batchAfterDelete = await items.GetBatch(50, 69, 1);
                }

                // ASSERT
                countAfterDelete.Should().Be(count - batchBeforeDelete.Count());
                batchAfterDelete.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task MultiKey_InsertAsync_Should_Add_A_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert;
                MultiKeyItem found;
                MultiKeyItem newItem = new MultiKeyItem
                {
                    Id = 1000,
                    GroupId = 2,
                    Letter = "Z",
                    Name = "Zelda",
                    Size = 5
                };

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    await items.InsertAsync(newItem);
                    countAfterInsert = await items.CountAllAsync();
                    found = await items.GetByKeyAsync((newItem.Id, newItem.GroupId));
                }

                // ASSERT
                countAfterInsert.Should().Be(count + 1);
                found.Should().NotBeNull();
                found.Id.Should().Equals(newItem.Id);
                found.GroupId.Should().Equals(newItem.GroupId);
                found.Name.Should().Equals(newItem.Name);
                found.Letter.Should().Equals(newItem.Letter);
                found.Size.Should().Equals(newItem.Size);
            }
        }

        [Fact]
        public async Task MultiKey_InsertAsync_Should_Add_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                int countAfterInsert;

                IEnumerable<MultiKeyItem> batchToInsert =
                    (from i in Enumerable.Range(1000, 20)
                     select new MultiKeyItem
                     {
                         Id = i,
                         GroupId = 2,
                         Letter = "X",
                         Name = $"X{i}",
                         Size = 0
                     }).ToArray();

                IEnumerable<MultiKeyItem> batchAfterInsert;

                // ACT

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    await items.InsertAsync(batchToInsert.ToArray());
                    countAfterInsert = await items.CountAllAsync();
                    batchAfterInsert = await items.GetBatch(1000, 1019, 2);
                }

                // ASSERT
                countAfterInsert.Should().Be(count + batchToInsert.Count());
                batchAfterInsert.Should().BeEquivalentTo(batchToInsert);
            }
        }

        [Fact]
        public async Task MultiKey_UpdateAsync_Should_Change_A_Record()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterUpdate;
                MultiKeyItem found, oldItem;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();

                    oldItem = await items.GetByKeyAsync((50, 1));
                    oldItem.Name = "DUMMY";
                    oldItem.Letter = "D";
                    oldItem.Size = 5;

                    await items.UpdateAsync(oldItem);
                    countAfterUpdate = await items.CountAllAsync();
                    found = await items.GetByKeyAsync((oldItem.Id, oldItem.GroupId));
                }

                // ASSERT
                countAfterUpdate.Should().Be(count);
                found.Should().NotBeNull();
                found.Id.Should().Equals(oldItem.Id);
                found.GroupId.Should().Equals(oldItem.GroupId);
                found.Name.Should().Equals(oldItem.Name);
                found.Letter.Should().Equals(oldItem.Letter);
                found.Size.Should().Equals(oldItem.Size);
            }
        }

        [Fact]
        public async Task MultiKey_UpdateAsync_Should_Update_A_Batch()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupMultiKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                int countAfterUpdate;

                IEnumerable<MultiKeyItem> batchToUpdate, batchAfterUpdate;

                // ACT

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<IMultiKeyItemsRepository>();
                    batchToUpdate = await items.GetBatch(50, 69, 1);

                    foreach (var item in batchToUpdate)
                        item.Name += "TEST";

                    await items.UpdateAsync(batchToUpdate.ToArray());

                    countAfterUpdate = await items.CountAllAsync();

                    batchAfterUpdate = await items.GetBatch(50, 69, 1);
                }

                // ASSERT
                countAfterUpdate.Should().Be(count);
                batchAfterUpdate.Should().BeEquivalentTo(batchToUpdate);
            }
        }
    }
}
