using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface ICriteriaExecutorResolver
    {
        ICriteriaExecutor<T> Resolve<T>(string name)
            where T : ICriteria;
    }
}
