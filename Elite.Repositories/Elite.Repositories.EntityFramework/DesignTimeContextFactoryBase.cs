//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;

//namespace Elite.Ashley.Services.Common.Data
//{
//    public abstract class DesignTimeContextFactoryBase<T> : IDesignTimeDbContextFactory<T>
//        where T : DbContext
//    {
//        protected abstract T Create(IRepositorySettings settings);

//        protected abstract FileInfo SettingsFile { get; }

//        public T CreateDbContext(string[] args)
//        {
//            if (!this.SettingsFile.Exists)
//                throw new ApplicationException($"File not found: {this.SettingsFile.FullName}");

//            IRepositorySettings settings = this.FromFile(this.SettingsFile.Directory.FullName, this.SettingsFile.Name);
//            Console.WriteLine(settings.ConnectionString);
//            return this.Create(settings);
//        }

//        private IRepositorySettings FromFile(string directory, string filename)
//        {
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//                .SetBasePath(directory)
//                .AddJsonFile(filename)
//                .Build();

//            return configuration.GetSection("Repository").Get<RepositorySettings>();
//        }
//    }
//}
