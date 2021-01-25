using Elite.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
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
        where TContext: DbContext
    {
        private IServiceScope Scope { get; }
        private TContext Context { get; }

        public EntityUnitOfWork(IServiceScope scope)
        {
            this.Scope = scope;
            this.Context = this.Scope.ServiceProvider.GetRequiredService<TContext>();
        }

        public async Task<IUnitOfWorkTransaction> BeginTransaction()
        {
            return new EntityUnitOfWorkTransaction(this, await this.Context.Database.BeginTransactionAsync());
        }

        public T GetRepository<T>()
            where T: IRepository
        {
            return this.Scope.ServiceProvider.GetRequiredService<T>();
        }

        public void Dispose()
        {
            this.Scope.Dispose();
        }

        private class EntityUnitOfWorkTransaction : IUnitOfWorkTransaction
        {
            private EntityUnitOfWork<TContext> UnitOfWork { get; }
            private IDbContextTransaction Transaction { get; set; }
            public bool IsCompleted { get; private set; }

            public EntityUnitOfWorkTransaction(EntityUnitOfWork<TContext> unitOfWork, IDbContextTransaction transaction)
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