using System;
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
            if (mainViewModel == null)
            {
                throw new ArgumentNullException(nameof(mainViewModel));
            }

            if (mainView == null)
            {
                throw new ArgumentNullException(nameof(mainView));
            }

            _mainView = mainView;
            _mainViewModel = mainViewModel;
        }

        public int Run()
        {
            return base.Run((Window) _mainView);
        }

        public void Initialize()
        {
            _mainView.DataContext = _mainViewModel;
        }
    }
}
