using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Server.ViewModels;

namespace Server.Bootstrapper
{
    public class WindsorConfig : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
              .Register(Component.For<IWindsorContainer>().Instance(container).LifeStyle.Singleton)
              .Register(Component.For<AppViewModel>().LifeStyle.Singleton)
              .Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifeStyle.Singleton)
              .Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifeStyle.Singleton);
        }
    }
}