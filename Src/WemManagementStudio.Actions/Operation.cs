using System;
using System.Threading.Tasks;
using WemManagementStudio.Actions.Actions;
using WemStudio.Domain;

namespace WemManagementStudio.Actions
{
    /// <summary>
    /// Exposes the model for a generic operation.
    /// </summary>
    public sealed class Operation
    {
        public Machine Machine { get; }
        public IOperatopImplementation Implementation { get; }

        public Operation(Machine machine, IOperatopImplementation implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
        }

        public async Task<bool> ExecuteAsync() => await Implementation.Execute();
    }
}