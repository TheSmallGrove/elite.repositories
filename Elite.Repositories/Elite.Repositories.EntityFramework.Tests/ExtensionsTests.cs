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
        public void AddEntityRepository_Should_Add_Expected_Services()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddEntityRepository<TestDbContext>(o => { });
            IServiceProvider provider = services.BuildServiceProvider();
            var euw = provider.GetRequiredService<IUnitOfWorkFactory>();
            euw.Should().NotBeNull();
        }

        public class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions opts)
                : base(opts)
            { }
        }
    }
}
