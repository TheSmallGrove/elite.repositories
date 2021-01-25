using Elite.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.EntityFramework
{
    public class EntityUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        private IRepositoryFactory Factory { get; }
        private TContext Context { get; }

        public EntityUnitOfWork(IRepositoryFactory factory, TContext context)
        {
            this.Factory = factory;
            this.Context = context;
        }

        public async Task<IUnitOfWorkSession> BeginAsync()
        {
            return new EntityUnitOfWorkSession(this, await this.Context.Database.BeginTransactionAsync());
        }

        public T CreateRepository<T>() where T : IRepository
        {
            return this.Factory.CreateRepository<T>();
        }

        private class EntityUnitOfWorkSession : IUnitOfWorkSession
        {
            private EntityUnitOfWork<TContext> UnitOfWork { get; }
            private IDbContextTransaction Transaction { get; set; }

            public bool IsCompleted { get; private set; }

            public EntityUnitOfWorkSession(EntityUnitOfWork<TContext> unitOfWork, IDbContextTransaction transaction)
            {
                this.UnitOfWork = unitOfWork;
                this.Transaction = transaction;
                this.IsCompleted = false;
            }

            public async Task CompleteAsync()
            {
                if (this.IsCompleted)
                    throw new InvalidOperationException("Cannot complete a transaction that has been already completed");

                await this.SaveAsync();
                await this.Transaction.CommitAsync();
                this.IsCompleted = true;
            }

            public async Task SaveAsync()
            {
                if (this.IsCompleted)
                    throw new InvalidOperationException("Cannot save in a transaction that has been already completed");

                if (this.UnitOfWork.Context.ChangeTracker.HasChanges())
                    await this.UnitOfWork.Context.SaveChangesAsync();
            }

            public void Dispose()
            {
                if (!this.IsCompleted)
                {
                    this.Transaction.Rollback();
                    this.IsCompleted = true;
                }

                this.Transaction.Dispose();
            }
        }
    }
}
