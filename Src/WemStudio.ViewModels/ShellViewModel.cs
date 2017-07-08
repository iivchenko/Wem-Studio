using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

            _machines.Modified += (sender, args) =>
            {
                switch (args.EntityStatus)
                {
                    case RepositoryEntityStatus.New:
                        Machines.Add(new MachineViewModel(args.Entity, _machines));
                        break;
                }
            };

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
            var settings = new Dictionary<string, object>
            {
                { "ResizeMode", ResizeMode.NoResize },
                { "WindowStyle", WindowStyle.ToolWindow }
            };

            _windows.ShowDialog(IoC.Get<AddMachineViewModel>(), null, settings);
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
