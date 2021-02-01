using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions.Criterias
{
    public class RestrictionCriteria : ICriteria
    {
        public string RestrictionTemplate { get; set; }

        public object[] Arguments { get; set; }
    }
}
