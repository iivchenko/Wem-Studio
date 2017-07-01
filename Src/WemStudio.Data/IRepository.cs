using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WemManagementStudio.Data
{
    public interface IRepository<TEntity, in TKey>
    {
        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        TEntity Get(TKey key);

        IEnumerable<TEntity> FindAll();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> query);
    }
}
