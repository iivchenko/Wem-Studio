using System;

namespace WemStudio.Data
{
    public interface INotifiableRepository<TEntity, in TKey> : IRepository<TEntity, TKey>
    {
        event EventHandler<RepositoryModifiedEventArgs<TEntity>> Modified;
    }
}
