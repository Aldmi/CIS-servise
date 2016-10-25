using System;
using System.ComponentModel;
using System.Windows;
using Caliburn.Micro;
using Castle.Windsor;
using Castle.Facilities.WcfIntegration;
using Server.HostWCF;
using Server.ViewModels;
using WCFCis2AvtodictorContract.Contract;
using Component = Castle.MicroKernel.Registration.Component;

namespace Server.Bootstrapper
{
    public class AppBootstrapper : BootstrapperBase
    {
        private readonly IWindsorContainer _container = new WindsorContainer();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<AppViewModel>();                            //Вызов конструктора начального окна
        }


        protected override void Configure()
        {
            _container.Install(new WindsorConfig());
        }


        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key)

                ? _container.Kernel.HasComponent(service)
                    ? _container.Resolve(service)
                    : base.GetInstance(service, key)

                : _container.Kernel.HasComponent(key)
                    ? _container.Resolve(key, service)
                    : base.GetInstance(service, key);
        }



    }

}