using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Domain.Entities;
using System.ServiceModel;
using System.Windows.Forms;
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;
using DataExchange.Event;
using DataExchange.Quartz.Shedules;
using DataExchange.WebClient;
using MessageBox = System.Windows.MessageBox;


namespace Server.ViewModels
{
    public sealed class AppViewModel : Conductor<object>, IHandle<InitDbFromXmlStatus>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IWindsorContainer _windsorContainer;
        private readonly ServiceHostBase _serviceHost;



        public ApkDkWebClient ApkDk { get; set; }

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


        ReadOnlyCollection<Station> OwnerRailwayStations { get; set; } = new ReadOnlyCollection<Station>(new List<Station>
        {
            new Station {Name = "Курский", EcpCode = 19155},
            new Station {Name = "Павелецкий", EcpCode = 19351},
            new Station {Name = "Казанский", EcpCode = 19390},
            new Station {Name = "Ярославский", EcpCode = 19550},
            new Station {Name = "Савеловский", EcpCode = 19600},
            new Station {Name = "Рижский", EcpCode = 19612},
            new Station {Name = "Киевский", EcpCode = 19810},
            new Station {Name = "Смоленский", EcpCode = 19823}
        });

        public DiagnosticViewModel DiagnosticVmKurskiy { get; set; }
        public DiagnosticViewModel DiagnosticVmPavel { get; set; }
        public DiagnosticViewModel DiagnosticVmKazan { get; set; }
        public DiagnosticViewModel DiagnosticVmYaroslav { get; set; }
        public DiagnosticViewModel DiagnosticVmSavelov { get; set; }
        public DiagnosticViewModel DiagnosticVmRigskii { get; set; }
        public DiagnosticViewModel DiagnosticVmKievskii { get; set; }
        public DiagnosticViewModel DiagnosticVmSmolensk { get; set; }





        public AppViewModel(IEventAggregator events, IWindowManager windowManager, IWindsorContainer windsorContainer)
        {
            DisplayName = "Главное окно ЦИС";

            _windowManager = windowManager;
            _windsorContainer = windsorContainer;
            _eventAggregator = events;
            events.Subscribe(this);

            DiagnosticVmKurskiy = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Курский"), _eventAggregator);
            DiagnosticVmPavel = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Павелецкий"), _eventAggregator);
            DiagnosticVmKazan = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Казанский"), _eventAggregator);
            DiagnosticVmYaroslav = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Ярославский"), _eventAggregator);
            DiagnosticVmSavelov = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Савеловский"), _eventAggregator);
            DiagnosticVmRigskii = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Рижский"), _eventAggregator);
            DiagnosticVmKievskii = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Киевский"), _eventAggregator);
            DiagnosticVmSmolensk = new DiagnosticViewModel(OwnerRailwayStations.FirstOrDefault(st => st.Name == "Смоленский"), _eventAggregator);

            _serviceHost = new DefaultServiceHostFactory().CreateServiceHost("CisServiceResolver", new Uri[0]);

            ApkDk = new ApkDkWebClient(_windsorContainer, _eventAggregator);
        }





        #region RibbonButtonHandler

        public async void ShowRailwayStation(string railwayStationName, int ecpCode)
        {
            var stationOwner = new Station { Name = railwayStationName, EcpCode = ecpCode };

            var editViewModel = new RailwayStationEditViewModel(_windsorContainer, stationOwner);
            _windowManager.ShowDialog(editViewModel);
        }

        #endregion





        #region BackstageMenuHandler

        /// <summary>
        /// Загрузка расписания и станций из XML файла с диска.
        /// </summary>
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

            var stationOwner = new Station { Name = railwayStationName, EcpCode = ecpCode };

            var processViewModel = new ProcessViewModel(_eventAggregator, stationOwner);
            _windowManager.ShowWindow(processViewModel);

            await ApkDk.LoadXmlDataInDb(pathShedule, pathStations, tableName, stationOwner);
        }


        /// <summary>
        /// Загрузка расписания и станций по http.
        /// </summary>
        public async void LoadHttpDataInDb(string tableName, string railwayStationName, int ecpCode)
        {
            var stationOwner = new Station { Name = railwayStationName, EcpCode = ecpCode };

            var processViewModel = new ProcessViewModel(_eventAggregator, stationOwner);
            _windowManager.ShowWindow(processViewModel);

            await ApkDk.LoadHttpDataInDb(tableName, stationOwner);
        }


        /// <summary>
        /// Загрузка расписания по http.
        /// Загрузка станций из XML файла.
        /// </summary>
        public async void LoadHttpSheduleAndLoadXmlStationsInDb(string tableName, string railwayStationName, int ecpCode)
        {
            string pathStations = null;

            var fbd = new OpenFileDialog
            {
                Filter = @"XML Files (.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Файл со станциями"
            };
            var result = fbd.ShowDialog();
            if ((result == DialogResult.OK) && (!string.IsNullOrWhiteSpace(fbd.FileName)))
            {
                pathStations = fbd.FileName;
            }

            if (string.IsNullOrEmpty(pathStations))
                return;


            var stationOwner = new Station { Name = railwayStationName, EcpCode = ecpCode };

            var processViewModel = new ProcessViewModel(_eventAggregator, stationOwner);
            _windowManager.ShowWindow(processViewModel);

             await ApkDk.LoadHttpSheduleAndLoadXmlStationsInDb(pathStations, tableName, stationOwner);
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

                QuartzApkDkReglamentRegSh.Start(ApkDk, OwnerRailwayStations);
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

            QuartzApkDkReglamentRegSh.Shutdown();

            base.OnDeactivate(close);
        }



        #region EventHandler

        public void Handle(InitDbFromXmlStatus message)
        {
            // MessageBox.Show(message.Status);//DEBUG
        }

        #endregion


    }
}