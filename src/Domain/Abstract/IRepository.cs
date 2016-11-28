using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IRepository<TEntity> where TEntity : IEntitie
    {
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(int id);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> Search(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        void Insert(TEntity entity);
        void InsertRange(List<TEntity> entitys);

        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entitys);
    }
}
