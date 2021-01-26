using Elite.Repositories.Abstractions;
using System;

namespace TestWorker
{
    public class Product : Entity<string, string>
    {
        public string Id
        {
            get => this.Key.Item1;
            set => this.Key = new Tuple<string, string>(value, this.IdGroup);
        }

        public string IdGroup
        {
            get => this.Key.Item2;
            set => this.Key = new Tuple<string, string>(this.Id, value);
        }

        public string Name { get; set; }
    }
}