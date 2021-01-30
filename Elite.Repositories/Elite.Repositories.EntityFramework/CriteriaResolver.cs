using Elite.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework
{
    class CriteriaResolver : ICriteriaResolver
    {
        private IEnumerable<ICriteria> Criterias { get; }

        public CriteriaResolver(IEnumerable<ICriteria> criterias)
        {
            this.Criterias = criterias;
        }

        public ICriteria Resolve(string name)
        {
            return this.Criterias.Single(_ => _.Name == name);
        }
    }
}
