using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using NTUBAdministrator.Models.Interface;

namespace NTUBAdministrator.Models.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private DbContext db { get; set; }

        public GenericRepository() : this(new Entities())
        {
        }

        public GenericRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.db = context;
        }

        public GenericRepository(ObjectContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.db = new DbContext(context, true);
        }

        public void Insert(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this.db.Set<TEntity>().Add(instance);
                this.SaveChanges();
            }

        }

        public void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this.db.Entry(instance).State = EntityState.Modified;
                this.SaveChanges();
            }
        }
        
        public void Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                this.db.Entry(instance).State = EntityState.Deleted;
                this.SaveChanges();
            }
        }
        
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.db.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.db.Set<TEntity>().AsQueryable();
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }


    }
}