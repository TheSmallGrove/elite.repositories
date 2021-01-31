using Elite.Repositories.Abstractions;

namespace Elite.Repositories.EntityFramework.Criterias
{
    public class SortingCriteria : ICriteria
    {
        public string[] Properties { get; set; }
    }
}
