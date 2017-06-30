using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace WemManagementStudio.Actions
{
    public class OperationExecutor
    {
        private readonly CancellationToken _cancellationToken;
        private ConcurrentQueue<Operation> _queueOperations;

        private OperationExecutor(ICollection<Operation> operations)
        {
            if (operations == null) throw new ArgumentNullException(nameof(operations));
            if (operations.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(operations));

            _queueOperations = new ConcurrentQueue<Operation>(operations);
        }

        public OperationExecutor(ICollection<Operation> operations, ref CancellationToken token) : this(operations)
        {
            _cancellationToken = token;
        }

        public ReadOnlyCollection<Operation> Operations => new ReadOnlyCollection<Operation>(_queueOperations.ToArray());

        public async Task ExecuteSequence()
        {
            await Task.Factory.StartNew(async () =>
            {
                ConcurrentQueue<Operation> queueOperations = this._queueOperations;

                while (queueOperations != null && queueOperations.Count > 0)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        return;

                    Operation operation;

                    if (_queueOperations.TryDequeue(out operation))
                    {
                        await operation.ExecuteAsync();
                    }
                }

            }, _cancellationToken);
        }
    }
}