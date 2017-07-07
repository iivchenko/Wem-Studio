using System;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using WemStudio.Tool.Wpf.Initialization;

namespace WemStudio.Tool.Wpf
{
    public static class Entrance
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var configInit = new ConfigurationBuilder();
            var configViewModel = new ConfigurationBuilder();
            var configData = new ConfigurationBuilder();

            configInit.AddJsonFile(@"autofac\init.json");
            configViewModel.AddJsonFile(@"autofac\views.json");
            configData.AddJsonFile(@"autofac\data.json");

            var moduleInit = new ConfigurationModule(configInit.Build());
            var moduleViewModel = new ConfigurationModule(configViewModel.Build());
            var moduleData = new ConfigurationModule(configData.Build());

            var builder = new ContainerBuilder();

            builder.RegisterModule(moduleInit);
            builder.RegisterModule(moduleViewModel);
            builder.RegisterModule(moduleData);

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var app = scope.Resolve<IApp>();
                app.Initialize();
                app.Run();
            }
        }
    }
}
