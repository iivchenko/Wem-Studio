using System;
using System.Collections.Generic;
using Caliburn.Micro;
using WemManagementStudio.Data;

namespace WemManagementStudio.ViewModels
{
    public sealed class AddMachineViewModel : Screen
    {
        private readonly IRepository<Machine, long> _machines;
        private readonly Machine _machine;

        public AddMachineViewModel(IRepository<Machine, long> machines)
        {
            if (machines == null)
            {
                throw new ArgumentNullException(nameof(machines));
            }

            _machines = machines;

            MachineType = new[] { WemManagementStudio.MachineType.Broker, WemManagementStudio.MachineType.Agent, WemManagementStudio.MachineType.Console };

            _machine = new Machine();

            DisplayName = "Add new Vm";
        }

        public string MachineName
        {
            get
            {
                return _machine.Name;
            }

            set
            {
                _machine.Name = value;

                NotifyOfPropertyChange(() => MachineName);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string Host
        {
            get
            {
                return _machine.Host;
            }

            set
            {
                _machine.Host = value;

                NotifyOfPropertyChange(() => Host);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string User
        {
            get
            {
                return _machine.User;
            }

            set
            {
                _machine.User = value;

                NotifyOfPropertyChange(() => User);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string Pass
        {
            get
            {
                return _machine.Pass;
            }

            set
            {
                _machine.Pass = value;

                NotifyOfPropertyChange(() => Pass);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public string Path
        {
            get { return _machine.Path; }

            set
            {
                _machine.Path = value;
                
                NotifyOfPropertyChange(() => Path);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public IEnumerable<MachineType> MachineType { get; }

        public MachineType ActiveType
        {
            get
            {
                return _machine.MachineType;
            }

            set
            {
                _machine.MachineType = value;

                NotifyOfPropertyChange(() => ActiveType);
                NotifyOfPropertyChange(() => CanAdd);
            }
        }

        public void Add()
        {
            _machines.Add(_machine);

            TryClose(true);
        }

        public bool CanAdd => !string.IsNullOrWhiteSpace(MachineName) &&
                              !string.IsNullOrWhiteSpace(Host) &&
                              !string.IsNullOrWhiteSpace(User) &&
                              !string.IsNullOrWhiteSpace(Pass) &&
                              !string.IsNullOrWhiteSpace(Path);
    }
}
