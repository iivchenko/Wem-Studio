using System;
using System.Threading.Tasks;
using WemManagementStudio.Actions.Actions;

namespace WemManagementStudio.Actions
{
    public class Operation
    {
        protected Machine Machine { get; }
        protected IOperatopImplementation Implementation { get; }

        protected Operation(Machine machine, IOperatopImplementation implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            Machine = machine ?? throw new ArgumentNullException(nameof(machine));
        }


        public async Task ExecuteAsync() => await Implementation.Execute();
    }
}