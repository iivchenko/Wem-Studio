using System;

namespace WemStudio.Data
{
    public sealed class RepositoryModifiedEventArgs<TEntity> : EventArgs
    {
        public RepositoryModifiedEventArgs(TEntity entity, RepositoryEntityStatus entityStatus)
        {
            Entity = entity;
            EntityStatus = entityStatus;
        }

        public TEntity Entity { get; }

        public RepositoryEntityStatus EntityStatus { get; }
    }

    public enum RepositoryEntityStatus
    {
        New,
        Update,
        Remove
    }
}
