using System.Windows;

namespace WemStudio.Tool.Wpf.Initialization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApp
    {
        private readonly IBootstrapper _bootstrapper;

        public App(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        public void Initialize()
        {
            _bootstrapper.Initialize();

            InitializeComponent();
        }
    }
}
