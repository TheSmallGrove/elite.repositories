using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework.Tests.Entities
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Letter { get; set; }
    }
}
