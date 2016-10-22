﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IRepository<TEntity> where TEntity: class
    {
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(int id);

        IQueryable<TEntity> Get();

        IQueryable<TEntity> Search(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
