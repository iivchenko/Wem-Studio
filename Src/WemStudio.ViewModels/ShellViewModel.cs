using System.Linq;
using Caliburn.Micro;
using WemStudio.Data;
using WemStudio.Domain;

namespace WemStudio.ViewModels
{
    public sealed class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IWindowManager _windows;
        private readonly INotifiableRepository<Machine, long> _machines;

        private MachineViewModel _selectedMachine;


        public ShellViewModel(IWindowManager windows, INotifiableRepository<Machine, long> machines)
        {
            _windows = windows;
            _machines = machines;

            // TODO: refactor
            Machines = new BindableCollection<MachineViewModel>(machines.FindAll().Select(x => new MachineViewModel(x, machines)).ToList());

            DisplayName = "Wem Studio";
        }

        public BindableCollection<MachineViewModel> Machines { get; }

        public MachineViewModel SelectedMachine
        {
            get { return _selectedMachine; }

            set
            {
                if (_selectedMachine != null)
                {
                    DeactivateItem(ActiveItem, close: true);
                }

                _selectedMachine = value;
                
                NotifyOfPropertyChange(() => SelectedMachine);

                ActivateItem(_selectedMachine);
            }
        }

        public void AddMachine()
        {
            if (_windows.ShowDialog(IoC.Get<AddMachineViewModel>()) == true)
            {
                // TODO: Donesn't work!
                //foreach (var newMachine in _machines.Find(x => Machines.All(y => x.Id != y.Id)).ToList())
                foreach (var newMachine in _machines.FindAll().Where(x => Machines.All(y => x.Id != y.Id)).ToList())
                {
                    Machines.Add(new MachineViewModel(newMachine, _machines));
                }

                NotifyOfPropertyChange(() => Machines);
            }
        }

        // TODO: Something with remove! Investigate it.
        //public void RemoveMachine(MachineViewModel machine)
        //{
        //    machine.Remove();
        //    Machines.Remove(machine);
        //}

        public void RemoveMachine()
        {
            DeactivateItem(ActiveItem, close: true);
            SelectedMachine.Remove();
            Machines.Remove(SelectedMachine);
        }
    }
}
