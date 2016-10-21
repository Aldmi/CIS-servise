using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.DbContext;

namespace Domain.Concrete
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        protected CisDbContext Context { get; }
        protected DbSet<TEntity> DbSet { get; }



        public GenericRepository(CisDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();

        }



        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }


        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }


        public virtual IQueryable<TEntity> Get()
        {
            return DbSet;
        }


        public virtual IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = Get();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split                        //управление ленивой загрузкой
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy?.Invoke(query).ToList() ?? query.ToList();
        }


        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }


        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Remove(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }
    }
}