using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Tests.Entities
{
    public class MultiKeyItem : Entity<int, int>
    {
        public int Id { get => this.Key.Key1; set => this.Key.Key1 = value; }
        public int GroupId { get => this.Key.Key2; set => this.Key.Key2 = value; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Letter { get; set; }
    }
}
