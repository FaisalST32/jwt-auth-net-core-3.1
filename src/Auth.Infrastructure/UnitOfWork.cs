using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Auth.Common.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }

        private IDbContextTransaction Transaction { get; set; }

        public void BeginTransaction()
        {
            if (this.Transaction == null)
                this.Transaction = this.Context.Database.BeginTransaction();
        }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public void Commit()
        {
            Transaction.Commit();
        }
    }
}
