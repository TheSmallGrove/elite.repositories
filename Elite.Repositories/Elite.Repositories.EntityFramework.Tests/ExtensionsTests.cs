using Microsoft.Extensions.DependencyInjection;
using System;
using Elite.Repositories.EntityFramework;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Elite.Repositories.Abstractions;
using FluentAssertions;

namespace Elite.Repositories.EntityFramework.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void AddEntityRepository_DbContextOptionsBuilder_ShouldNot_Be_Called_When_Null()
        {
            // ARRANGE
            IServiceCollection services = new ServiceCollection();

            // ACT
            services.AddEntityRepository<TestDbContext>();

            // ASSERT
            IServiceProvider provider = services.BuildServiceProvider();
            var dbContext = provider.GetRequiredService<TestDbContext>();
            dbContext.Should().NotBeNull();
            dbContext.Should().BeOfType<TestDbContext>();
        }

        [Fact]
        public void AddEntityRepository_DbContextOptionsBuilder_Should_Be_Called_When_NotNull()
        {
            // ARRANGE
            IServiceCollection services = new ServiceCollection();
            bool optionsCalled = false;

            // ACT
            services.AddEntityRepository<TestDbContext>(o => optionsCalled = true);

            // ASSERT
            IServiceProvider provider = services.BuildServiceProvider();
            var dbContext = provider.GetRequiredService<TestDbContext>();
            dbContext.Should().NotBeNull();
            dbContext.Should().BeOfType<TestDbContext>();
            optionsCalled.Should().BeTrue();
        }

        [Fact]
        public void AddEntityRepository_Should_Add_IUnitOfWorkFactory_As_Transient()
        {
            // ARRANGE
            IServiceCollection services = new ServiceCollection();

            // ACT
            services.AddEntityRepository<TestDbContext>();

            // ASSERT
            IServiceProvider provider = services.BuildServiceProvider();

            var euw1 = provider.GetRequiredService<IUnitOfWorkFactory>();
            euw1.Should().NotBeNull();
            euw1.Should().BeOfType<EntityUnitOfWorkFactory>();

            var euw2 = provider.GetRequiredService<IUnitOfWorkFactory>();
            euw2.Should().NotBeNull();
            euw2.Should().BeOfType<EntityUnitOfWorkFactory>();

            // test transiency
            euw1.Should().NotBeSameAs(euw2);
        }

        [Fact]
        public void AddEntityRepository_Should_Add_IUnitOfWork_As_Transient()
        {
            // ARRANGE
            IServiceCollection services = new ServiceCollection();

            // ACT
            services.AddEntityRepository<TestDbContext>();

            // ASSERT
            IServiceProvider provider = services.BuildServiceProvider();

            var uw1 = provider.GetRequiredService<IUnitOfWork>();
            uw1.Should().NotBeNull();
            uw1.Should().BeOfType<EntityUnitOfWork<TestDbContext>>();

            var uw2 = provider.GetRequiredService<IUnitOfWork>();
            uw2.Should().NotBeNull();
            uw2.Should().BeOfType<EntityUnitOfWork<TestDbContext>>();

            // test transiency
            uw1.Should().NotBeSameAs(uw2);
        }

        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions opts)
                : base(opts)
            { }
        }
    }
}
