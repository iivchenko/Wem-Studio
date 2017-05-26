using System;
using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;

namespace WemManagementStudio.Wpf
{
    public static class Entrance
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");

            
            var module = new ConfigurationModule(config.Build());
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var app = scope.Resolve<IApp>();
                app.Initialize();
                app.Run();
            }
        }
    }
}
