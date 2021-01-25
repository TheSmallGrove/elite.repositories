//using Microsoft.EntityFrameworkCore;
//using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
//using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Elite.Ashley.Services.Common.Data
//{
//    public abstract class RepositoryDbContext : DbContext
//    {
//        protected IRepositorySettings Settings { get; }

//        public RepositoryDbContext(IRepositorySettings settings)
//        {
//            this.Settings = settings;
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        { 
//            switch (this.Settings.Database.ToLowerInvariant())
//            {
//                case RepositorySettings.MSSQL:
//                    optionsBuilder.UseSqlServer(this.Settings.ConnectionString);
//                    return;
//                case RepositorySettings.MYSQL:
//                    optionsBuilder.UseMySql(this.Settings.ConnectionString, opts =>
//                    {
//                        opts.SchemaBehavior(MySqlSchemaBehavior.Translate, (a,b) =>
//                        {
//                            return $"{a}_{b}".ToUpperInvariant();
//                        });
//                    });
//                    return;
//            }

//            throw new NotSupportedException($"Database '{this.Settings.Database}' is not supported yet");
//        }

//        public abstract void CreateModel(ModelBuilder builder);

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            if (!string.IsNullOrEmpty(this.Settings.DefaultSchema))
//                modelBuilder.HasDefaultSchema(this.Settings.DefaultSchema);

//            this.CreateModel(modelBuilder);
//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}
