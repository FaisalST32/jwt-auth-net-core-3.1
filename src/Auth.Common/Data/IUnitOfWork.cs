using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Common.Data
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }
        void BeginTransaction();
        void SaveChanges();
        Task<int> SaveChangesAsync();
        void Commit();
    }
}
