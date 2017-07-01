using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using WemStudio.ViewModels;
using WemStudio.Views;

namespace WemStudio.Tool.Wpf.Initialization
{
    public sealed class Bootstrapper : BootstrapperBase, IBootstrapper
    {
        private readonly IComponentContext _container;

        public Bootstrapper(IComponentContext container)
        {
            _container = container;
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { typeof(ShellView).Assembly };
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            return
                string.IsNullOrWhiteSpace(key)
                    ? _container.Resolve(serviceType)
                    : _container.ResolveNamed(key, serviceType);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return
                _container
                    .ComponentRegistry
                    .Registrations
                    .Where(r => serviceType.IsAssignableFrom(r.Activator.LimitType))
                    .Select(r => r.Activator.LimitType)
                    .Select(t => _container.Resolve(t));
        }

        protected override void BuildUp(object instance)
        {            
            // TODO: Will need this stuff for coroutines 
            //_container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
