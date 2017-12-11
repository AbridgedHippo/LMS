using LMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LMS.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        #region properties
        //private LMSContext _context;
        //public LMSContext Context
        //{
        //    get { return _context ?? (_context = new LMSContext()); }
        //}
        #endregion

        public GenericRepository() { }

        #region implementation

        public virtual IEnumerable<T> GetAll()
        {
            // To make sure that the context will be disposed of as soon as possible
            using (var context = new LMSContext())
            {
                return context.Set<T>().ToList();
            }
        }
        public virtual T Get(int id)
        {
            using (var context = new LMSContext())
            {
                return context.Set<T>().Find(id);
            }
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            using (var context = new LMSContext())
            {
                return context.Set<T>().FirstOrDefault(where);
            }
        }
        public virtual void Add(T entity)
        {
            using (var context = new LMSContext())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }
        public virtual void Update(T entity)
        {
            using (var context = new LMSContext())
            {
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public virtual void Delete(T entity)
        {
            using (var context = new LMSContext())
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }
        public virtual void Delete(int id)
        {
            using (var context = new LMSContext())
            {
                var entities = context.Set<T>();
                var entity = entities.Find(id);
                entities.Remove(entity);
                context.SaveChanges();
            }
        }

        // async
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            using (var context = new LMSContext())
            {
                return await context.Set<T>().ToListAsync();
            }
        }
        public virtual async Task<T> GetAsync(int id)
        {
            using (var context = new LMSContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            using (var context = new LMSContext())
            {
                return await context.Set<T>().FirstOrDefaultAsync(where);
            }
        }
        public virtual async Task AddAsync(T entity)
        {
            using (var context = new LMSContext())
            {
                context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
            }
        }
        public virtual async Task UpdateAsync(T entity)
        {
            using (var context = new LMSContext())
            {
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
        public virtual async Task DeleteAsync(T entity)
        {
            using (var context = new LMSContext())
            {
                var entities = context.Set<T>();
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    entities.Attach(entity);
                }
                entities.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
        public virtual async Task DeleteAsync(int id)
        {
            using (var context = new LMSContext())
            {
                var entities = context.Set<T>();
                var entity = await entities.FindAsync(id);
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    entities.Attach(entity);
                }
                entities.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
        #endregion
    }
}