using LMS.DataAccess;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace LMS.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        #region properties
        private LMSContext _context;
        public LMSContext Context
        {
            get { return _context ?? (_context = HttpContext.Current.GetOwinContext().Get<LMSContext>()); }
            private set { _context = value; }
        }
        #endregion

        public GenericRepository() { }
        public GenericRepository(LMSContext context)
        {
            Context = context;
        }

        #region Implementation

        #region Not Async
        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }
        public virtual T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }
        public virtual T Get(string id)
        {
            return Context.Set<T>().Find(id);
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return Context.Set<T>().FirstOrDefault(where);
        }
        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(int id)
        {
            var entities = Context.Set<T>();
            var entity = entities.Find(id);
            entities.Remove(entity);
            Context.SaveChanges();
        }

        #endregion

        #region Async

        // async
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }
        public virtual async Task<T> GetAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }
        public virtual async Task<T> GetAsync(string id)
        {
            return await Context.Set<T>().FindAsync(id);
        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(where);
        }
        public virtual async Task AddAsync(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
        }
        public virtual async Task UpdateAsync(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(T entity)
        {
            var entities = Context.Set<T>();
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                entities.Attach(entity);
            }
            entities.Remove(entity);
            await Context.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(int id)
        {
            var entities = Context.Set<T>();
            var entity = await entities.FindAsync(id);
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                entities.Attach(entity);
            }
            entities.Remove(entity);
            await Context.SaveChangesAsync();
        }

        #endregion

        public void Save()
        {
            Context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        #endregion
    }
}