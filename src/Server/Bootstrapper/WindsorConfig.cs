using System.Data.Entity.Infrastructure;
using System.ServiceModel.Security;
using Caliburn.Micro;
using Castle.Core.Logging;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Domain.Abstract;
using Domain.Concrete;
using Domain.DbContext;
using Server.ViewModels;
using WCFCis2AvtodictorContract.Contract;
using Server.HostWCF;

namespace Server.Bootstrapper
{
    public class WindsorConfig : IWindsorInstaller
    {
        #region ctor

        public WindsorConfig()
        {

        }

        #endregion





        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Component.For<IWindsorContainer>().Instance(container).LifeStyle.Singleton)
                .Register(Component.For<AppViewModel>().LifeStyle.Singleton)
                .Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifeStyle.Singleton)
                .Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifeStyle.Singleton)
                .Register(Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifestyleTransient())  //LifestyleTransient()
                .Register(Component.For<CisDbContext>().ImplementedBy<CisDbContext>().LifestyleTransient());      //LifestyleTransient()

            container.AddFacility<WcfFacility>()
                .Register(Component.For<IServerContract>().ImplementedBy<CisServise>().Named("CisServiceResolver"));
        }
    }
}