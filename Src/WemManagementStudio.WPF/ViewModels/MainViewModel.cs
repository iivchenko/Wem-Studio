using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NLog;
using WemManagementStudio.Wpf.Common;

namespace WemManagementStudio.Wpf.ViewModels
{
    public sealed class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        private string _consoleStatus;
        
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IList<string> _consoleFiles;
        private readonly FileSystemWatcher _watcher;

        private string _path;
        
        public MainViewModel()
        {
            Log = new ObservableCollection<LogEventInfo>();

            _watcher = new FileSystemWatcher
            {
                Path = Path,
                EnableRaisingEvents = false,
                IncludeSubdirectories = true
            };
            
            var settings = Serializer.Load();

            Machines = new ObservableCollection<Machine>(settings.Machines);

            Path = settings.Path;

            Machines.CollectionChanged += (sender, args) =>
            {
                Serializer.Save(new Settings
                {
                    Path = Path,
                    Machines = new Collection<Machine>(Machines)
                });

                OnPropertyChanged("Consoles");
                OnPropertyChanged("Brokers");
                OnPropertyChanged("Agents");
            };
           
            _watcher.Changed += (sender, args) =>
            {
                _consoleFiles.Add(args.FullPath);
                ConsoleStatus = $"Files: {_consoleFiles.Count}";
            };

            _watcher.Created += (sender, args) =>
            {
                _consoleFiles.Add(args.FullPath);
                ConsoleStatus = $"Files: {_consoleFiles.Count}";
            };

            _watcher.Deleted += (sender, args) =>
            {
                _consoleFiles.Add(args.FullPath);
                ConsoleStatus = $"Files: {_consoleFiles.Count}";
            };

            _consoleFiles = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Path
        {
            get { return _path; }

            set
            {
                _path = value;

                Serializer.Save(new Settings
                {
                    Path = Path,
                    Machines = new Collection<Machine>(Machines)
                });

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Machine> Machines { get; }

        public bool IsMonitorEnabled
        {
            get { return _watcher.EnableRaisingEvents; }
        }

        public ICommand StartMonitorCommand
        {
            get
            {
                return new RelayCommand(o => StartMonitor(), o => Directory.Exists(Path) && !_watcher.EnableRaisingEvents);
            }
        }

        public ICommand StopMonitorCommand
        {
            get
            {
                return new RelayCommand(o => StopMonitor(), o => _watcher.EnableRaisingEvents);
            }
        }

        public ObservableCollection<LogEventInfo> Log { get; }

        #region Console
        public string ConsoleStatus
        {
            get
            {
                return _consoleStatus;
            }

            set
            {
                _consoleStatus = value;

                OnPropertyChanged();
            }
        }

        public IEnumerable<Machine> Consoles => Machines.Where(x => x.MachineType == MachineType.Console);

        public Machine SelectedConsole { get; set; }

        #endregion

        public IEnumerable<Machine> Brokers => Machines.Where(x => x.MachineType == MachineType.Broker);

        public Machine SelectedBroker { get; set; }

        public IEnumerable<Machine> Agents => Machines.Where(x => x.MachineType == MachineType.Agent);

        public Machine SelectedAgent { get; set; }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartMonitor()
        {
            _watcher.Path = Path;
            _watcher.EnableRaisingEvents = true;

            OnPropertyChanged("IsMonitorEnabled");

            LogInfo("File Monitor Started");
        }

        private void StopMonitor()
        {
            _watcher.EnableRaisingEvents = false;

            OnPropertyChanged("IsMonitorEnabled");
            
            LogInfo("File Monitor Stopped");
        }

        private void LogInfo(string message)
        {
            var entry = new LogEventInfo(LogLevel.Info, Logger.Name, message);

            Logger.Info(entry);
            Log.Add(entry);
        }
    }
}
