using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GestorInventarioEmpresas.BackEnd.DAL
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual TEntity GetByID(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public virtual void Insert(TEntity entity)
        {

            dbSet.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(params object[] keyValues)
        {
            TEntity entityToDelete = dbSet.Find(keyValues);
            if (entityToDelete != null)
                Delete(entityToDelete);
            context.SaveChanges();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            context.SaveChanges();
        }

        public virtual TEntity Update(TEntity entityToUpdate, params object[] keyValues)
        {
            if (entityToUpdate != null)
            {
                TEntity element = dbSet.Find(keyValues);
                if (element != null)
                {
                    ((DbContext)context).Entry(element).CurrentValues.SetValues(entityToUpdate);
                }
            }
            context.SaveChanges();
            return entityToUpdate;
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }


        public bool Exists(TEntity entityToUpdate)
        {
            return dbSet.ToList().Contains(entityToUpdate);
        }



        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }

}
