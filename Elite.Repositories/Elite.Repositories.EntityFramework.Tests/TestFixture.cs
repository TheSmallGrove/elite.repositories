using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Tests
{
    public class TestFixture : IDisposable
    {
        public TestFixture()
        { }

        public async Task<IDatabaseSetup> SetupSingleKey(int count = 0)
        {
            var loader = new DatabaseSetup();
            await loader.LoadSingleKey(count);
            return loader;
        }

        public async Task<IDatabaseSetup> SetupMultiKey(int count = 0)
        {
            var loader = new DatabaseSetup();
            await loader.LoadMultiKey(count);
            return loader;
        }

        public void Dispose()
        {
        }

        public interface IDatabaseSetup : IDisposable
        {
            string ConnectionString { get; }
        }

        private class DatabaseSetup : IDatabaseSetup
        {
            private readonly string TempFile = Path.GetTempFileName();
            public string ConnectionString { get; private set; }

            internal DatabaseSetup()
            { }

            public void Dispose()
            {
                File.Delete(TempFile);
            }

            internal async Task LoadSingleKey(int count = 0)
            {
                var file = new FileInfo(Path.Combine($"{Environment.CurrentDirectory}", "names.json"));
                var json = File.ReadAllText(file.FullName);
                var names = (IEnumerable<string>)JsonSerializer.Deserialize<string[]>(json);
                var data = new List<Stack<string>>();

                foreach (var grouping in names.GroupBy(o => o[0].ToString().ToUpper()))
                {
                    data.Add(new Stack<string>(grouping));
                }

                var cursor = new Cursor(data);
                var sequence = 1;
                this.ConnectionString = $"Data Source ={TempFile};";

                using (var connection = new SQLiteConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SQLiteCommand command = new SQLiteCommand(
                        @"CREATE TABLE IF NOT EXISTS single_names (
                               Id INTEGER PRIMARY KEY,
                               Name TEXT NOT NULL,
                               Size INTEGER NOT NULL,
                               Letter TEXT NOT NULL
                          );", connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("insert into single_names (Id, Name, Size, Letter) values (@id, @name, @size, @letter)", connection))
                        {
                            command.Parameters.Clear();
                            var id = command.CreateParameter();
                            id.ParameterName = "id";
                            var name = command.CreateParameter();
                            name.ParameterName = "name";
                            var size = command.CreateParameter();
                            size.ParameterName = "size";
                            var letter = command.CreateParameter();
                            letter.ParameterName = "letter";

                            for (var i = 0; i < count; i++)
                            {
                                var item = cursor.Next();
                                Debug.WriteLine(item);
                                id.Value = sequence++;
                                command.Parameters.Add(id);
                                name.Value = item;
                                command.Parameters.Add(name);
                                size.Value = item.Length;
                                command.Parameters.Add(size);
                                letter.Value = item[0].ToString().ToUpper();
                                command.Parameters.Add(letter);
                                await command.ExecuteNonQueryAsync();
                            }
                        }

                        transaction.Commit();
                    }
                }
            }

            internal async Task LoadMultiKey(int count = 0)
            {
                var file = new FileInfo(Path.Combine($"{Environment.CurrentDirectory}", "names.json"));
                var json = File.ReadAllText(file.FullName);
                var names = (IEnumerable<string>)JsonSerializer.Deserialize<string[]>(json);
                var data = new List<Stack<string>>();

                foreach (var grouping in names.GroupBy(o => o[0].ToString().ToUpper()))
                {
                    data.Add(new Stack<string>(grouping));
                }

                var cursor = new Cursor(data);
                var sequence = 1;
                this.ConnectionString = $"Data Source ={TempFile};";

                using (var connection = new SQLiteConnection(this.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SQLiteCommand command = new SQLiteCommand(
                        @"CREATE TABLE IF NOT EXISTS multi_names (
                               Id INTEGER PRIMARY KEY,
                               GroupId INTEGER,
                               Name TEXT NOT NULL,
                               Size INTEGER NOT NULL,
                               Letter TEXT NOT NULL
                          );", connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("insert into multi_names (Id, GroupId, Name, Size, Letter) values (@id, @groupid, @name, @size, @letter)", connection))
                        {
                            command.Parameters.Clear();
                            var id = command.CreateParameter();
                            id.ParameterName = "id";
                            var groupid = command.CreateParameter();
                            groupid.ParameterName = "groupid";
                            var name = command.CreateParameter();
                            name.ParameterName = "name";
                            var size = command.CreateParameter();
                            size.ParameterName = "size";
                            var letter = command.CreateParameter();
                            letter.ParameterName = "letter";

                            for (var i = 0; i < count; i++)
                            {
                                var item = cursor.Next();
                                Debug.WriteLine(item);
                                id.Value = sequence++;
                                command.Parameters.Add(id);
                                groupid.Value = 1;
                                command.Parameters.Add(groupid);
                                name.Value = item;
                                command.Parameters.Add(name);
                                size.Value = item.Length;
                                command.Parameters.Add(size);
                                letter.Value = item[0].ToString().ToUpper();
                                command.Parameters.Add(letter);
                                await command.ExecuteNonQueryAsync();
                            }
                        }

                        transaction.Commit();
                    }
                }
            }
        }

        private class Cursor
        {
            private IEnumerable<Stack<string>> Data { get; }
            private int pos = 0;

            public Cursor(IEnumerable<Stack<string>> data)
            {
                this.Data = data;
            }

            public string Next()
            {
                var row = this.Data.ElementAt(pos);

                if (row.Count() > 0)
                {
                    var item = this.Data.ElementAt(pos).Pop();
                    if (++pos == this.Data.Count()) pos = 0;
                    return item;
                }

                if (++pos == this.Data.Count()) pos = 0;
                return this.Next();
            }
        }
    }
}
