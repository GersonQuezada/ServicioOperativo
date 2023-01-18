using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {

        void Dispose();
        void SaveChanges();
        Task SaveChangesAsync();
        void Dispose(bool disposing);
        T Repository<T>() where T : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbContext Get();

        Task BeginTransaction();
        Task SaveChangeTransaction();
        Task Rollback();

        Task BulkInsert<TEntity>(List<TEntity> listaEntidades, bool migracion = false) where TEntity : class;
    }
}
