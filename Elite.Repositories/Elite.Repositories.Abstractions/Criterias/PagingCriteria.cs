using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions.Criterias
{
    public class PagingCriteria : ICriteria
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
