using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Entities
{
    public class Genre : IEntity
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }
}
