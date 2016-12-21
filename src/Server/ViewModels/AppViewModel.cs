using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using Domain.Abstract;
using Domain.DbContext;
using Domain.Entities;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Castle.Core.Internal;
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;
using DataExchange.Event;
using DataExchange.InitDb;
using DataExchange.XmlGetter;
using Domain.Concrete;
using Library.Xml;
using Server.ClientSOAP;
using Server.Event;
using Server.HostWCF;
using WCFCis2AvtodictorContract.Contract;
using MessageBox = System.Windows.MessageBox;
using Screen = Caliburn.Micro.Screen;



namespace Server.ViewModels
{
    public class AppViewModel : Conductor<object>, IHandle<InitDbFromXmlStatus>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IWindsorContainer _windsorContainer;
        private readonly ServiceHostBase _serviceHost;




        public ApkDk ApkDk { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private string _messageBusy;
        public string MessageBusy
        {
            get { return _messageBusy; }
            set
            {
                _messageBusy = value;
                NotifyOfPropertyChange(() => MessageBusy);
            }
        }




        public DiagnosticViewModel DiagnosticVm1 { get; set; }
        public DiagnosticViewModel DiagnosticVm2 { get; set; }




        public AppViewModel(IEventAggregator events, IWindowManager windowManager, IWindsorContainer windsorContainer)
        {
            _windowManager = windowManager;
            _windsorContainer = windsorContainer;
            _eventAggregator = events;
            events.Subscribe(this);

            DiagnosticVm1 = new DiagnosticViewModel("Курский", _eventAggregator);
            DiagnosticVm2 = new DiagnosticViewModel("Вокзал 2", _eventAggregator);

            _serviceHost = new DefaultServiceHostFactory().CreateServiceHost("CisServiceResolver", new Uri[0]);

            ApkDk= new ApkDk(_windsorContainer);
        }





        #region RibbonButtonHandler

        public async void ShowRailwayStation(string railwayStationName, int ecpCode)
        {
            var stationOwner = new Station {Name = railwayStationName, EcpCode = ecpCode};

            var editViewModel = new RailwayStationEditViewModel(_windsorContainer, stationOwner);
            _windowManager.ShowDialog(editViewModel);
        }

        #endregion






        #region BackstageMenuHandler

        public async void LoadXmlDataInDb(string tableName, string railwayStationName, int ecpCode)
        {
            string pathShedule = null;
            string pathStations = null;


            var fbd = new OpenFileDialog
            {
                Filter = @"XML Files (.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Файл регулярного расписания"
            };
            var result = fbd.ShowDialog();
            if ((result == DialogResult.OK) && (!string.IsNullOrWhiteSpace(fbd.FileName)))
            {
                pathShedule = fbd.FileName;

                fbd = new OpenFileDialog
                {
                    Filter = @"XML Files (.xml)|*.xml|All Files (*.*)|*.*",
                    Title = "Файл со станциями"
                };
                result = fbd.ShowDialog();
                if ((result == DialogResult.OK) && (!string.IsNullOrWhiteSpace(fbd.FileName)))
                {
                    pathStations = fbd.FileName;
                }
            }

            if (string.IsNullOrEmpty(pathShedule) || string.IsNullOrEmpty(pathStations))
                return;


            var stationOwner = new Station {Name = railwayStationName, EcpCode = ecpCode};
            var initDb = new InitDbFromXml(_windsorContainer, _eventAggregator, stationOwner);

            try
            {
                switch (tableName)
                {
                    case "regular":
                        var sheduleGetter = new GetterXmlFromDisk(pathShedule, stationOwner);
                        var stationsGetter = new GetterXmlFromDisk(pathStations, stationOwner);
                        await initDb.InitRegulatorySh(sheduleGetter, stationsGetter);
                        break;



                    case "operative":
                        break;
                }
            }
            catch (Exception)    //TODO: более точно определять тип исключения и выводить его в окно.
            {
                MessageBox.Show($"Ошибка работы с БД.");
            }


        }

        #endregion



        private void ShowBusyIndicator(bool? isBusy = null, string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageBusy = message;
            }
            if (isBusy.HasValue)
            {
                IsBusy = isBusy.Value;
            }
        }


        protected override void OnInitialize()
        {
            try
            {
                _serviceHost?.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                base.OnInitialize();
            }
        }


        protected override void OnDeactivate(bool close)
        {
            _eventAggregator?.Unsubscribe(this);

            if (_serviceHost?.State == CommunicationState.Opened)
                _serviceHost.Close();

            base.OnDeactivate(close);
        }



        #region EventHandler

        public void Handle(InitDbFromXmlStatus message)
        {
            MessageBox.Show(message.Status);//DEBUG
        }

        #endregion


    }
}