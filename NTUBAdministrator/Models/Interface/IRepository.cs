using System;
using System.Linq;
using System.Linq.Expressions;

namespace NTUBAdministrator.Models.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Insert(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        void SaveChanges();
    }
}
