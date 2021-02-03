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
        public TestFixture Fixture { get; set; }

        public EntityRepositoryTests(TestFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task CountAllAsync_Should_Return_Exact_Number_Of_Items()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countResult;

                using (var uow = factory.BeginUnitOfWork())
                {
                    var items = uow.GetRepository<ISingleKeyItemsRepository>();
                    countResult = await items.CountAllAsync();
                }

                // ASSERT
                countResult.Should().Be(count);
            }
        }

        [Fact]
        public async Task CompleteAsync_Should_Commit_Changes()
        {
            int count = 100;

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert, countAfterCommit;
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

            using (var data = await this.Fixture.SetupSingleKey(count))
            {
                // ARRANGE
                var container = TestUtilities.BuildServiceProvider(data);
                var factory = container.GetRequiredService<IUnitOfWorkFactory>();

                // ACT
                int countAfterInsert, countAfterCommit;
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
    }
}
