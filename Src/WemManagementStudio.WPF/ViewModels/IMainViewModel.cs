using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NLog;

namespace WemManagementStudio.Wpf.ViewModels
{
    public interface IMainViewModel
    {
        string Path { get; set; }

        ObservableCollection<Machine> Machines { get; }

        bool IsMonitorEnabled { get; }

        ICommand StartMonitorCommand { get; }

        ICommand StopMonitorCommand { get; }

        ObservableCollection<LogEventInfo> Log { get; }

        #region Console

        string ConsoleStatus { get; set; }

        IEnumerable<Machine> Consoles { get; }

        Machine SelectedConsole { get; set; }

        #endregion

        IEnumerable<Machine> Brokers { get; }

        Machine SelectedBroker { get; set; }

        IEnumerable<Machine> Agents { get; }

        Machine SelectedAgent { get; set; }
    }
}
