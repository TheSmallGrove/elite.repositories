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
        private IEnumerable<ICriteriaExecutor> Executors { get; }
        public CriteriaExecutorResolver(IEnumerable<ICriteriaExecutor> criterias) => this.Executors = criterias;
        public ICriteriaExecutor Resolve(Type criteriaType) => this.Executors.SingleOrDefault(_ => _.CriteriaType == criteriaType);
    }
}
