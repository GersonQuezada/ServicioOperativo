using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Repository.Interfaces.Data
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<T> GetById(int? id);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        T Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);
    }
}
