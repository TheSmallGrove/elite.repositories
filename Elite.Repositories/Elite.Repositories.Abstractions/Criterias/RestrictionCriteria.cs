using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions.Criterias
{
    public class RestrictionCriteria : ICriteria
    {
        public string RestrictionTemplate { get; set; }

        public object[] Arguments { get; set; }

        public static RestrictionCriteria FromQuerySet(IEnumerable<string> set, params object[] arguments)
        {
            var zipped = set
                .Zip(arguments)
                .Where(_ => _.Second != null);

            var items = zipped
                .Select((o, i) => $"({o.First.Replace("??", $"@{i}")}) ");

            return new RestrictionCriteria
            {
                RestrictionTemplate = string.Join("&& ", items).Trim(),
                Arguments = zipped.Select(_ => _.Second).ToArray(),
            };
        }
    }
}
