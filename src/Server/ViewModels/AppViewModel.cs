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
using Castle.Facilities.WcfIntegration;
using Castle.Windsor;
using Domain.Concrete;
using Library.Xml;
using Server.HostWCF;
using WCFCis2AvtodictorContract.Contract;
using MessageBox = System.Windows.MessageBox;
using Screen = Caliburn.Micro.Screen;



namespace Server.ViewModels
{
    public class AppViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IWindsorContainer _windsorContainer;
        private readonly ServiceHostBase _serviceHost;



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

            DiagnosticVm1 = new DiagnosticViewModel("Вокзал 1", _eventAggregator);
            DiagnosticVm2 = new DiagnosticViewModel("Вокзал 2", _eventAggregator);

            _serviceHost = new DefaultServiceHostFactory().CreateServiceHost("CisServiceResolver", new Uri[0]);
        }






        public async void RailwayStation1()
        {
            using (var unitOfWork = _windsorContainer.Resolve<IUnitOfWork>())
            {
                //DEBUG-------------------------------------------------------
                //var station = _unitOfWork.StationRepository.GetById(1);
                //var copyStation = new Station
                //{
                //    Id = station.Id,
                //    Name = "qqqq",
                //    EcpCode = station.EcpCode,
                //    Description = station.Description
                //};
                ////_unitOfWork.StationRepository.Update(copyStation, copyStation.Id);
                //_unitOfWork.StationRepository.Update(copyStation);
                //await _unitOfWork.SaveAsync();
                //DEBUG-------------------------------------------------------

                ShowBusyIndicator(true, "Идет загрузка данных из БД");
                const string railwayStationName = "Вокзал 3";
                var railwayStation = await await Task.Factory.StartNew(async () =>
                {
                    var query = unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName);
                    return await query.FirstOrDefaultAsync();
                });
                ShowBusyIndicator(false);

                if (railwayStation != null)
                {
                    //создадим копию--------------------------------------------------------------
                    //var stations = railwayStation.Stations.Select(st => new Station
                    //{
                    //    Id = st.Id,
                    //    Name = st.Name,
                    //    EcpCode = st.EcpCode,
                    //    Description = st.Description,
                    //    RailwayStations = st.RailwayStations,
                    //    OperativeSchedulesListWithoutStops = st.OperativeSchedulesListWithoutStops,
                    //    OperativeSchedulesListOfStops = st.OperativeSchedulesListOfStops
                    //}).ToList();

                    //var railwayStationCpy = new RailwayStation
                    //{
                    //    Id = railwayStation.Id,
                    //    Name = railwayStation.Name,
                    //    Stations = stations,
                    //    OperativeSchedules = railwayStation.OperativeSchedules.Select(op => new OperativeSchedule
                    //    {
                    //        Id = op.Id,
                    //        ArrivalTime = op.ArrivalTime,
                    //        DepartureTime = op.DepartureTime,
                    //        ListOfStops = op.ListOfStops,
                    //        ListWithoutStops = op.ListWithoutStops,
                    //        NumberOfTrain = op.NumberOfTrain,
                    //        RouteName = op.RouteName,
                    //        DispatchStation = stations.FirstOrDefault(st => st.Id == op.DispatchStation.Id),
                    //        StationOfDestination = stations.FirstOrDefault(st => st.Id == op.StationOfDestination.Id)
                    //    }).ToList(),
                    //    Diagnostics = railwayStation.Diagnostics
                    //};
                    //создадим копию--------------------------------------------------------------

                    var editViewModel = new RailwayStationEditViewModel(unitOfWork, railwayStation);
                    var result = _windowManager.ShowDialog(editViewModel);
                    if (result != null && result.Value)
                    {
                        // MessageBox.Show("Ok");
                    }
                    else
                    {
                        // MessageBox.Show("Cancel");
                    }
                }
                else
                {
                    MessageBox.Show("Вокзала с именеем {0} не найденно", railwayStationName);
                }
            }
            IsBusy = false;
        }


        public void LoadXmlDataInDb(string nameRailwayStations)
        {
            var fbd = new OpenFileDialog { Filter = @"XML Files (.xml)|*.xml|All Files (*.*)|*.*" };
            var result = fbd.ShowDialog();
            if ((result == DialogResult.OK) && (!string.IsNullOrWhiteSpace(fbd.FileName)))
            {
                var x = XElement.Load(fbd.FileName);
            }

            //var newStationDisp= new Station {Description = "новая станция назначения", EcpCode = 586, Name = "Станция ууу"};
            //var newStationArriv = new Station { Description = "новая станция прибытия", EcpCode = 423, Name = "Станция ттт" };
            //var operSh= new OperativeSchedule {ArrivalTime = DateTime.Now, DepartureTime = DateTime.Today, NumberOfTrain = 10, RouteName = "Новый маршрут", DispatchStation = newStationDisp, StationOfDestination = newStationArriv};
            //_unitOfWork.OperativeScheduleRepository.Insert(operSh);
            //_unitOfWork.SaveAsync();
        }


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
    }
}