using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Credimujer.Op.Common;
using Credimujer.Op.Repository.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace Credimujer.Op.Repository.Implementations.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting settings;

        private Dictionary<Type, object> repositories;
        private bool _disposed;
        private readonly ILifetimeScope _lifetimeScope;
        private IDbContextTransaction _transaction;
        private Func<DateTime> CurrentDateTime { get; set; } = () => DateTime.Now;

        public UnitOfWork(IOptions<AppSetting> settings, IHttpContextAccessor httpContext, ILifetimeScope lifetimeScope)
        {
            this.settings = settings.Value;
            _lifetimeScope = lifetimeScope;
            _context = new DataContext(new
                DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(this.settings.ConnectionStrings.DefaultConnection).Options);
            _context.Database.SetCommandTimeout(60);
            _httpContext = httpContext;
        }

        public DbContext Get()
        {
            return _context;
        }

        public T Repository<T>() where T : class
        {
            //if (repositories == null)
            //    repositories = new Dictionary<Type, object>();

            //var type = typeof(T);
            //if (!repositories.ContainsKey(type))
            //{
            //    repositories.Add(type, lifetimeScope.Resolve<T>("context", _context));
            //}

            //return (T)repositories[type];
            return _lifetimeScope.Resolve<T>(new NamedParameter("context", _context));
        }

        public void SaveChanges()
        {
            TrackChanges();
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            TrackChanges();
            await _context.SaveChangesAsync();
        }

        private void TrackChanges()
        {
            if (_context.ChangeTracker.Entries().Any(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var identity = _httpContext?.HttpContext?.User.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario)?.Value ?? Constants.Core.Audit.System;

                foreach (var entry in _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    foreach (PropertyEntry property in entry.Properties.ToList().Where(o => !o.Metadata.IsKey()))
                        TrimFieldValue(property);

                    if (entry.State == EntityState.Added)
                    {
                        if (entry.Metadata.FindProperty(Constants.Core.Audit.CreationUser) != null)
                            entry.CurrentValues[Constants.Core.Audit.CreationUser] = identity;

                        if (entry.Metadata.FindProperty(Constants.Core.Audit.CreationDate) != null)
                            entry.CurrentValues[Constants.Core.Audit.CreationDate] = CurrentDateTime();

                        if (entry.Metadata.FindProperty(Constants.Core.Audit.RowStatu) != null)
                            entry.CurrentValues[Constants.Core.Audit.RowStatu] = true;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        if (entry.Metadata.FindProperty(Constants.Core.Audit.ModificationUser) != null)
                            entry.CurrentValues[Constants.Core.Audit.ModificationUser] = identity;

                        if (entry.Metadata.FindProperty(Constants.Core.Audit.ModificationDate) != null)
                            entry.CurrentValues[Constants.Core.Audit.ModificationDate] = CurrentDateTime();
                    }
                }
            }
        }

        private void TrimFieldValue(PropertyEntry property)
        {
            var metaData = property.Metadata;
            var currentValue = property.CurrentValue == null ? null : property.CurrentValue.ToString();

            if (currentValue == null) return;

            System.Reflection.FieldInfo fi = metaData.FieldInfo;

            if (fi.FieldType == typeof(string))
                property.CurrentValue = currentValue.Trim();
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                {
                    if (repositories != null)
                        repositories.Clear();

                    _context.Dispose();
                }

            _disposed = true;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangeTransaction()
        {
            try
            {
                TrackChanges();
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task Rollback()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public async Task SaveChangesTransactionalAsync()
        {
            await using var dbContextTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
            }
        }

        public async Task BulkInsert<TEntity>(List<TEntity> listaEntidades, bool migracion = false) where TEntity : class
        {
            TrackChanges();
            await _context.BulkInsertAsync(listaEntidades);
            await _context.SaveChangesAsync();
        }
    }
}