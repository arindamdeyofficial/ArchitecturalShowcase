using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Context
{
    public interface ICommandContext<T> where T : DbContext
    {
        Task CreateTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesWithTransactionAsync();
        Task<int> SaveChangesAsync();
        T GetDbContext();
        void Dispose();
    }
}
