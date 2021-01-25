using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions
{
    public interface IUnitOfWorkSession : IDisposable
    {
        Task SaveAsync();
        Task CompleteAsync();
        bool IsCompleted { get; }
    }

    //public class UnitOfWork<TContext> : IUnitOfWork
    //    where TContext : RepositoryDbContext
    //{
    //    private IContainer Container { get; }
    //    private TContext Context { get; }

    //    public UnitOfWork(IContainer container, TContext context)
    //    {
    //        this.Container = container;
    //        this.Context = context;
    //    }

    //    public T CreateRepository<T>()
    //        where T : IRepository
    //    {
    //        var nested = this.Container.GetNestedContainer();
    //        nested.Inject<TContext>(this.Context);
    //        return nested.GetService<T>();
    //    }

    //    public async Task<IUnitOfWorkSession> BeginAsync()
    //    {
    //        return new UnitOkWorkSession(this, await this.Context.Database.BeginTransactionAsync());
    //    }

    //    private class UnitOkWorkSession : IUnitOfWorkSession
    //    {
    //        private UnitOfWork<TContext> UnitOfWork { get; }
    //        private IDbContextTransaction Transaction { get; set; }

    //        public bool IsCompleted { get; private set; }

    //        public UnitOkWorkSession(UnitOfWork<TContext> unitOfWork, IDbContextTransaction transaction)
    //        {
    //            this.UnitOfWork = unitOfWork;
    //            this.Transaction = transaction;
    //            this.IsCompleted = false;
    //        }

    //        public async Task CompleteAsync()
    //        {
    //            if (this.IsCompleted)
    //                throw new InvalidOperationException("Cannot complete a transaction that has been already completed");

    //            await this.SaveAsync();
    //            await this.Transaction.CommitAsync();
    //            this.IsCompleted = true;
    //        }

    //        public async Task SaveAsync()
    //        {
    //            if (this.IsCompleted)
    //                throw new InvalidOperationException("Cannot save in a transaction that has been already completed");

    //            if (this.UnitOfWork.Context.ChangeTracker.HasChanges())
    //                await this.UnitOfWork.Context.SaveChangesAsync();
    //        }

    //        public void Dispose()
    //        {
    //            if (!this.IsCompleted)
    //            {
    //                this.Transaction.Rollback();
    //                this.IsCompleted = true;
    //            }

    //            this.Transaction.Dispose();
    //        }
    //    }
    //}
}
