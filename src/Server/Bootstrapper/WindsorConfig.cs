using System.Data.Entity.Infrastructure;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DbContext;
using Server.ViewModels;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorService;

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
                .Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifeStyle.Singleton)
                .Register(Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifeStyle.Singleton)                 //TODO LifeStyle.Singleton ??
                .Register(Component.For<CisDbContext>().ImplementedBy<CisDbContext>().LifeStyle.Singleton)              //TODO LifeStyle.Singleton ??
                .Register(Component.For<IServerContract>().ImplementedBy<CisServise>().LifeStyle.Singleton);
        }
    }
}