using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface IRepository
    { }

    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        public abstract Task Insert(TEntity entity);
        public abstract Task Update(TEntity entity);
        public abstract Task Delete(TEntity entity);
    }

    //public abstract class Repository<TEntity> : IReadableRepository<TEntity>, IWriteableRepository<TEntity>
    //    where TEntity : class
    //{
    //    private DbContext Context { get; }

    //    public Repository(RepositoryDbContext context)
    //    {
    //        this.Context = context;
    //    }

    //    IQueryable<TEntity> IReadableRepository<TEntity>.Query()
    //    {
    //        return this.Context
    //            .Set<TEntity>();
    //    }

    //    IQueryable<TEntity> IReadableRepository<TEntity>.Query(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return this.Context
    //            .Set<TEntity>()
    //            .Where(restriction);
    //    }

    //    async Task<bool> IReadableRepository<TEntity>.Exists(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return await this.Context
    //            .Set<TEntity>()
    //            .Where(restriction)
    //            .AnyAsync();
    //    }

    //    async Task<int> IReadableRepository<TEntity>.Count(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return await this.Context
    //            .Set<TEntity>()
    //            .Where(restriction)
    //            .CountAsync();
    //    }

    //    async Task<TEntity> IReadableRepository<TEntity>.Single(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return await this.Context
    //            .Set<TEntity>()
    //            .Where(restriction)
    //            .SingleAsync();
    //    }

    //    async Task<TEntity> IReadableRepository<TEntity>.SingleOrDefault(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return await this.Context
    //            .Set<TEntity>()
    //            .Where(restriction)
    //            .SingleOrDefaultAsync();
    //    }

    //    async Task<TEntity> IReadableRepository<TEntity>.First(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return await this.Context
    //            .Set<TEntity>()
    //            .Where(restriction)
    //            .FirstAsync();
    //    }

    //    async Task<TEntity> IReadableRepository<TEntity>.FirstOrDefault(Expression<Func<TEntity, bool>> restriction)
    //    {
    //        return await this.Context
    //            .Set<TEntity>()
    //            .Where(restriction)
    //            .FirstOrDefaultAsync();
    //    }

    //    Task IWriteableRepository<TEntity>.Insert(TEntity entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    Task IWriteableRepository<TEntity>.Update(TEntity entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    Task IWriteableRepository<TEntity>.Delete(TEntity entity)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
