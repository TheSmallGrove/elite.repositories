using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface ICriteriaResolver
    {
        ICriteria Resolve(string name);
    }
}
