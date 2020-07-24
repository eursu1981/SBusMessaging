using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Data.Repository
{
    public interface IRepositoryBase<T>  where T : class
    {

        IQueryable<T> GetAllQueryable();
        IQueryable<T> GetAllNoTracking();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByAsNoTracking(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByAsQueryable(Expression<Func<T, bool>> predicate);
        T FindById(int id);
        Task<T> FindByIdAsync(int id);
        T Add(T entity);
        void AddRange(IEnumerable<T> entities);
        T Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Save();
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task SaveChangesAsync();
        int Count();
        IDbContextTransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.Serializable);
    }
}
