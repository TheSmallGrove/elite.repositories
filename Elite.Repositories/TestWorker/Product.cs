using Elite.Repositories.Abstractions;
using System;

namespace TestWorker
{
    public class Product : Entity<string, string>
    {
        public string Id
        {
            get => this.Key.Key1;
            set => this.Key.Key1 = value;
        }
        public string IdGroup
        {
            get => this.Key.Key2;
            set => this.Key.Key2 = value;
        }

        public string Name { get; set; }
    }
}