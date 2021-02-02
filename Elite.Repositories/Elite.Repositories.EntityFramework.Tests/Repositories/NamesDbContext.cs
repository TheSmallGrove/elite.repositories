using Elite.Repositories.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Tests.Repositories
{
    class NamesDbContext : DbContext
    {
        public NamesDbContext(DbContextOptions<NamesDbContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .ToTable("names")
                .HasKey(_ => _.Id);

            base.OnModelCreating(modelBuilder);
        }

    }
}
