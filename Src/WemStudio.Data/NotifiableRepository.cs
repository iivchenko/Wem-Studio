using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WemStudio.Data
{
    public sealed class NotifiableRepository<TEntity, TKey> : INotifiableRepository<TEntity, TKey>
    {
        private readonly IRepository<TEntity, TKey> _repository;

        public NotifiableRepository(IRepository<TEntity, TKey> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        public void Add(TEntity entity)
        {
            _repository.Add(entity);

            OnModified(new RepositoryModifiedEventArgs<TEntity>(entity, RepositoryEntityStatus.New));
        }

        public void Remove(TEntity entity)
        {
            _repository.Remove(entity);

            OnModified(new RepositoryModifiedEventArgs<TEntity>(entity, RepositoryEntityStatus.Remove));
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);

            OnModified(new RepositoryModifiedEventArgs<TEntity>(entity, RepositoryEntityStatus.Update));
        }

        public TEntity Get(TKey key)
        {
            return _repository.Get(key);
        }

        public IEnumerable<TEntity> FindAll()
        {
            return _repository.FindAll();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> query)
        {
            return _repository.Find(query);
        }

        public event EventHandler<RepositoryModifiedEventArgs<TEntity>> Modified;

        private void OnModified(RepositoryModifiedEventArgs<TEntity> args)
        {
            var temp = Modified;

            temp?.Invoke(this, args);
        }
    }
}
