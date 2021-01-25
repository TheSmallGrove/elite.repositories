using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Elite.Repositories
{
    public interface IRepositorySettings
    {
        string Database { get; }
        string ConnectionString { get; }
        string DefaultSchema { get; }
    }

    class RepositorySettings : IRepositorySettings
    {
        public const string MSSQL = "mssql";
        public const string MYSQL = "mysql";

        public string Database { get; set; }
        public string ConnectionString { get; set; }
        public string DefaultSchema { get; set; }
    }
}
