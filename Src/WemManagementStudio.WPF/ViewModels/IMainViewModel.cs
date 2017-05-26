using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WemManagementStudio.Wpf.ViewModels
{
    public interface IMainViewModel
    {
        string Path { get; set; }

        ObservableCollection<Machine> Machines { get; }

        IEnumerable<Machine> Consoles { get; }

        Machine SelectedConsole { get; set; }

        IEnumerable<Machine> Brokers { get; }

        Machine SelectedBroker { get; set; }

        IEnumerable<Machine> Agents { get; }

        Machine SelectedAgent { get; set; }
    }
}
