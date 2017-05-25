namespace WemManagementStudio.Wpf
{
    public static class Entrance
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
