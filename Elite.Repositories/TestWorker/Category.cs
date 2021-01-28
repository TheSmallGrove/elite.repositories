using Elite.Repositories.Abstractions;

namespace TestWorker
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}