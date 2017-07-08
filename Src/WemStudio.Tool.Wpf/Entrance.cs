using System;
using Autofac;
using Caliburn.Micro;
using WemStudio.Data;
using WemStudio.Domain;
using WemStudio.Tool.Wpf.Initialization;
using WemStudio.ViewModels;

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
            using (var scope = InitializeContainer())
            {
                var app = scope.Resolve<IApp>();
                app.Initialize();
                app.Run();
            }
        }

        private static ILifetimeScope InitializeContainer()
        {
            var builder = new ContainerBuilder();
            
            // init
            builder
                .RegisterType<WindowManager>()
                .As<IWindowManager>()
                .SingleInstance();

            builder
                .RegisterType<Bootstrapper>()
                .As<IBootstrapper>()
                .SingleInstance();

            builder
                .RegisterType<App>()
                .As<IApp>()
                .SingleInstance();

            // Data
            builder
                .RegisterType<WemStudioContext>()
                .SingleInstance();

            builder
                .RegisterType<MachineRepository>()
                .As<IRepository<Machine, long>>()
                .SingleInstance();

            builder
                .RegisterType<NotifiableRepository<Machine, long>>()
                .As<INotifiableRepository<Machine, long>>()
                .SingleInstance();

            // views
            builder
                .RegisterType<ShellViewModel>()
                .SingleInstance();

            builder
                .RegisterType<AddMachineViewModel>();

            return builder.Build().BeginLifetimeScope();
        }
    }
}
