using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WemManagementStudio.Wpf.ViewModels
{
    public sealed class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        private string _path;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Path
        {
            get { return _path; }

            set
            {
                _path = value;
                
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
