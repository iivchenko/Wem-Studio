using System.ComponentModel;
using System.Windows.Input;

namespace WemManagementStudio.Wpf.Views
{
    public interface IAddMachineViewModel : INotifyPropertyChanged
    {
        string DisplayName { get; set; }

        string Host { get; set; }

        string User { get; set; }

        string Pass { get; set; }

        MachineType Type { get; set; }

        ICommand AddCommand { get; set; }
    }
}
