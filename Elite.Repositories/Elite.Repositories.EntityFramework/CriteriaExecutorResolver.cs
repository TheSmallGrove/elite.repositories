using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework
{
    class CriteriaExecutorResolver : ICriteriaExecutorResolver
    {
        private IEnumerable<ICriteriaExecutor> Criterias { get; }

        public CriteriaExecutorResolver(IEnumerable<ICriteriaExecutor> criterias)
        {
            this.Criterias = criterias;
        }

        public ICriteriaExecutor<T> Resolve<T>(string name)
            where T: ICriteria => 
            this.Criterias.OfType<ICriteriaExecutor<T>>().SingleOrDefault();
    }
}
