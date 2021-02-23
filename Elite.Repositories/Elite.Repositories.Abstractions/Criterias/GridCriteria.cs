using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions.Criterias
{
    public class GridCriteria
    {
        public string[] Columns { get; set; }
        public PagingCriteria Paging { get; set; }
        public SortingCriteria Sorting { get; set; }

        public string Projection()
        {
            return $"new({string.Join(',', this.Columns)})";
        }
    }
}
