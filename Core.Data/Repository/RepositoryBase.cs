using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        private BikeStoresContext _dataContext;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(BikeStoresContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<T>();
        }

        #region Implementation
        public virtual IQueryable<T> GetAllQueryable()
        {
            return _dbSet.AsQueryable<T>();
        }

        public virtual IQueryable<T> GetAllNoTracking()
        {
            return _dbSet.AsNoTracking<T>();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            return query;
        }

        public virtual IQueryable<T> FindByAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.Where(predicate).AsNoTracking();
            return query;
        }

        public virtual IQueryable<T> FindByAsQueryable(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.Where(predicate).AsQueryable();
            return query;
        }

        public virtual T FindById(int id)
        {
            T result = _dataContext.Find<T>(id);
            return result;
        }


        public virtual async Task<T> FindByIdAsync(int id)
        {
            T result = await _dataContext.FindAsync<T>(id);
            return result;
        }
        public virtual T FindById(Guid id)
        {
            T result = _dataContext.Find<T>(id);
            return result;
        }

        public virtual T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }
        public virtual void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual T Delete(T entity)
        {
            return _dbSet.Remove(entity).Entity;
        }
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }
        public virtual void Save()
        {
            _dataContext.SaveChanges();
        }


        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public virtual int Count()
        {
            return GetAllNoTracking().Count();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolation = IsolationLevel.Serializable)
        {
            return _dataContext.Database.BeginTransaction();
        }

        #endregion
    }
}
