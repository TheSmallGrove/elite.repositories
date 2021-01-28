# Elite.Repositories data access library

This library aims to be an easy to use, dependency container friendly implementation of the repository and unit of work patterns. Using this library is possible to build effective data layers. The unit of work is designed also to correctly handle transactions without the need to use a TransactionScope.

The library is build to be highly estensible, with a basic abstraction assembly and a concrete implementation that uses Entity Framework Core for querying data.

## Getting started

To build the data layer, as the pattern suggests, you have to create a repository for each entity. This said, the first step is to create the entity itself:

    public class Category : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

The entity has to implement "IEntity" to enforce the constraints on the IRepository<T, K> interface. After this step you have to create the repository as the following example:

    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<Category> GetByName(string name);
    }

    class CategoryRepository : EntityRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(TestDbContext context) 
            : base(context)
        { }

        public override Task<Category> GetByKeyAsync(int key)
        {
            return (from entity in this.Set
                    where entity.Id == key 
                    select entity).SingleOrDefaultAsync();
        }

        public override async Task DeleteByKeyAsync(int key)
        {
            var entity = await this.GetByKeyAsync(key);
            await base.DeleteAsync(entity);
        }

        public Task<Category> GetByName(string name)
        {
            return (from entity in this.Set
                    where entity.Name == name
                    select entity).SingleOrDefaultAsync();
        }
    }

The base EntityRepository is the EntityFrameworkCore implementation of the library. The base repository class prevides standard methods to common operations. These are GetAll, Insert, Update and Delete. The GetByKey and DeleteByKey must be implemented in the concrete CategoryRepository to leave the developer free to use the preferred key name.

The ICategoryRepository interface should be exposed to support injection of repository into the dependency container. Tne concrete CategoryRepository can implement additional methods like the GetByName in the example code.

To use the repository you have to use a dependency container that might be wrapper into the IServiceCollection/IServiceProvider implementation of dependency injection. It can be done as follows:

    .ConfigureServices((hostContext, services) =>
    {
        services.AddEntityRepository<TestDbContext>(
            builder => builder.UseSqlite("Data Source=.\\database.db;"));

        services
            .AddRepository<ICategoryRepository, CategoryRepository>();
    });

The first call to AddEntityRepository setup the container with the requires services that supports the repository and the DbContext. Into the callback you can setup the DbContet options.

The AddRepository method adds a reference to the concrete repository through its specific interface. The repository is added with a transient lifetime.

Finally the repository can be used as the following code shows:

    public class StoreManager
    {
        private IUnitOfWorkFactory Factory { get; }

        public StoreManager(IUnitOfWorkFactory factory)
        {
            this.Factory = factory;
        }

        public Task<IEnumerable<Category>> GetHomeCategories()
        {
            using(var unitOfWork = this.Factory.BeginUnitOfWork())
            {
                var categoryRepo = unitOfWork.GetRepository<ICategoryRepository>();
                return categoryRepo.GetAllAsync();
            }
        }
    }

## Support for transactions

To open a transaction you can use the BeginTransaction method. The transaction that is returned exposes the CompleteAsync method that commits the transaction and the Dispose method that does the rollback.

    public async Task AddProducts(IEnumerable<Product> products)
    {
        using (var unitOfWork = this.Factory.BeginUnitOfWork())
        {
            using (var transaction = await unitOfWork.BeginTransaction())
            {
                var productRepo = unitOfWork.GetRepository<IProductRepository>();
                await productRepo.InsertAsync(products.ToArray());
                await transaction.CompleteAsync();
            }
        }
    }

## Support for composite keys

To support composite keys using a simple Tuple you can extend one of Entity<...> classes. These classes provide a "Key" property that contains the keys into an "EntityKey<...>". Using this protected property, you can provide specific properties that maps to the composite key columns. Here's and example:

    public class Product : Entity<string, string>
    {
        public string Id
        {
            get => this.Key.Key1;
            set => this.Key.Key1 = value;
        }
        public string IdGroup
        {
            get => this.Key.Key2;
            set => this.Key.Key2 = value;
        }

        public string Name { get; set; }
    }

After this you can implement the reposityr using a simple tuple syntax:

    class ProductRepository : EntityRepository<Product, (string Id, string IdGroup)>, IProductRepository
    {
        // ...
    }