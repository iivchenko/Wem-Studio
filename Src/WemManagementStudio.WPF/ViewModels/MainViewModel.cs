using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WemManagementStudio.Wpf.ViewModels
{
    public sealed class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        private readonly Settings _settings;
        
        public MainViewModel()
        {
            _settings = Serializer.Load();
            Machines = new ObservableCollection<Machine>(_settings.Machines);
            Machines.CollectionChanged += (sender, args) =>
            {
                _settings.Machines = Machines.ToList();

                Serializer.Save(_settings);

                OnPropertyChanged("Consoles");
                OnPropertyChanged("Brokers");
                OnPropertyChanged("Agents");
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public string Path
        {
            get { return _settings.Path; }

            set
            {
                _settings.Path = value;

                Serializer.Save(_settings);

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Machine> Machines { get; }


        public IEnumerable<Machine> Consoles => Machines.Where(x => x.MachineType == MachineType.Console);

        public Machine SelectedConsole { get; set; }

        public IEnumerable<Machine> Brokers => Machines.Where(x => x.MachineType == MachineType.Broker);

        public Machine SelectedBroker { get; set; }

        public IEnumerable<Machine> Agents => Machines.Where(x => x.MachineType == MachineType.Agent);

        public Machine SelectedAgent { get; set; }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
