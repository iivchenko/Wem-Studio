using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WemManagementStudio.Wpf.Common;
using WemManagementStudio.Wpf.Views;

namespace WemManagementStudio.Wpf.ViewModels
{
    public sealed class AddMachineViewModel : IAddMachineViewModel
    {
        private string _name;
        private string _host;
        private string _login;
        private string _pass;
        private MachineType _type;

        public AddMachineViewModel(Settings settings)
        {
            AddCommand = new RelayCommand(
                o =>
                {
                    var machine = new Machine
                    {
                        Name = DisplayName,
                        Host = Host,
                        User = User,
                        Pass = Pass,
                        MachineType = Type
                    };

                    settings.Machines.Add(machine);

                    Serializer.Save(settings);
                },
                o =>
                {
                    var machine = new Machine
                    {
                        Name = DisplayName,
                        Host = Host,
                        User = User,
                        Pass = Pass,
                        MachineType = Type
                    };

                    return
                        !settings.Machines.Contains(machine)
                        || !string.IsNullOrWhiteSpace(DisplayName)
                        || !string.IsNullOrWhiteSpace(Host)
                        || !string.IsNullOrWhiteSpace(User)
                        || !string.IsNullOrWhiteSpace(Pass);
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DisplayName
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;

                OnPropertyChanged();
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }

            set
            {
                _host = value;

                OnPropertyChanged();
            }
        }

        public string User
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;

                OnPropertyChanged();
            }
        }

        public string Pass
        {
            get
            {
                return _pass;
            }

            set
            {
                _pass = value;

                OnPropertyChanged();
            }
        }

        public MachineType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;

                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
