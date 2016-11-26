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
    public class AppViewModel : Conductor<object>, IHandle<AutodictorDiagnosticEvent>
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

            DiagnosticVm1 = new DiagnosticViewModel("Вокзал 1", _eventAggregator);
            DiagnosticVm2 = new DiagnosticViewModel("Вокзал 2", _eventAggregator);

            _serviceHost = new DefaultServiceHostFactory().CreateServiceHost("CisServiceResolver", new Uri[0]);

            ApkDk= new ApkDk(_windsorContainer);
        }






        public async void RailwayStation1()
        {
            const string railwayStationName = "Вокзал 1";

            //DEBUG
            //---------------------------------------------------------------------
           //IUnitOfWork unitOfWork = _windsorContainer.Resolve<IUnitOfWork>();
           // var query = unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName)
           //     .Include(s => s.Stations);

           // var railWaySt= await query.FirstOrDefaultAsync();


           // var newStation = new Station { Name = "Станция 11", Description = "описание 1256", EcpCode = 96, RailwayStations = new List<RailwayStation> { railWaySt } };
           // unitOfWork.StationRepository.Insert(newStation);

            //railWaySt.Stations.Add(newStation);

            //unitOfWork.RailwayStationRepository.Update(railWaySt);
            //await unitOfWork.SaveAsync();
            //------------------------------------------------------------------------------





            var editViewModel = new RailwayStationEditViewModel(railwayStationName, _windsorContainer);
            var result = _windowManager.ShowDialog(editViewModel);
        }


        public async void RailwayStation2()
        {

            ApkDk.RequestRegulatorySchedule();//DEBUG
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



        #region EventHandler

        public async void Handle(AutodictorDiagnosticEvent message)
        {
            using (var unitOfWork = _windsorContainer.Resolve<IUnitOfWork>())
            {
                //var railwayStation = await await Task.Factory.StartNew(async () =>
                //{
                //    var query = unitOfWork.RailwayStationRepository.Search(r => r.Name == message.NameRailwayStation).Include(r => r.Diagnostics);
                //    return await query.FirstOrDefaultAsync();
                //});

                //if (railwayStation != null)
                //{
                //    //удалим тек
                //    unitOfWork.DiagnosticRepository.RemoveRange(railwayStation.Diagnostics.ToList());


                //    var diagnosticDatas =   message.DiagnosticData.Select( d => new Diagnostic { DeviceNumber = d.DeviceNumber, DeviceName = d.DeviceName, Fault = d.Fault, Status = d.Status, Date = DateTime.Now });
                //    diagnosticDatas.ForEach(d => railwayStation.Diagnostics.Add(d));
 
       


                //    await unitOfWork.SaveAsync();
                //}
            }

            #endregion

        }
    }
}