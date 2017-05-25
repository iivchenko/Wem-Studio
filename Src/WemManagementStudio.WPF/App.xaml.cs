using System.Windows;
using WemManagementStudio.Wpf.ViewModels;
using WemManagementStudio.Wpf.Views;

namespace WemManagementStudio.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApp
    {
        private readonly IMainView _mainView;
        private readonly IMainViewModel _mainViewModel;

        public App(IMainView mainView, IMainViewModel mainViewModel)
        {
            _mainView = mainView;
            _mainViewModel = mainViewModel;

            StartupUri = new System.Uri("Views/MainView.xaml", System.UriKind.Relative);

            _mainView.ToString();
            _mainViewModel.ToString();
        }
    }
}
