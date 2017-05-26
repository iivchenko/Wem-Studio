using System.Windows;
using WemManagementStudio.Wpf.ViewModels;

namespace WemManagementStudio.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var add = new AddMachineView
            {
                DataContext = new AddMachineViewModel(((IMainViewModel)DataContext).Machines)
            };

            add.ShowDialog();
        }
    }
}
