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
using System.Windows.Forms;
using System.Xml.Linq;
using Castle.Facilities.WcfIntegration;
using Library.Xml;
using Server.HostWCF;
using WCFCis2AvtodictorContract.Contract;
using MessageBox = System.Windows.MessageBox;


namespace Server.ViewModels
{
    public class AppViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceHostBase _serviceHost;



        public IDisposable DispouseOrderStatusProcess { get; set; }
        public IDisposable DispouseOrderStatusAcsess { get; set; }



        public AppViewModel(IEventAggregator events, IWindowManager windowManager, IUnitOfWork unitOfWork, IServerContract autoDictorServise)
        {
            _windowManager = windowManager;
            _unitOfWork = unitOfWork;
            _eventAggregator = events;
            //events.Subscribe(this);

            _serviceHost = new DefaultServiceHostFactory().CreateServiceHost("CisServiceResolver", new Uri[0]);

        }






        public async void RailwayStation1()
        {
            //----DEBUG-------------------------------------------------------
            //station.Name был равен Name1
            //var station = _unitOfWork.StationRepository.Get().First();
            ////поменяли в локальной коллекции без Update в БД
            //station.Name = "NEWname";
            ////сделали запрос чтобы к БД (хочу предыдущее состояние)
            //_unitOfWork.StationRepository.UndoChanges();
            //station = _unitOfWork.StationRepository.Get().First();
            //// но station.Name уже "NEWname";
            //var c = 5 + 5;
            //----DEBUG--------------------------------------------------------

            const string railwayStationName = "Вокзал 3";
            var query = _unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName, null, "Stations, OperativeSchedules");
            var railwayStation = await query.FirstOrDefaultAsync();

            if (railwayStation != null)
            {
                //DEBUG-------------------------------------------------------------------------------------------
                //Moq вокзал
                //var stations = new List<Station> {new Station {Name = "111", Id = 1}, new Station { Name = "222", Id = 2 }, new Station { Name = "333", Id = 3 } };//DEBUG
                //var operSh = new List<OperativeSchedule>
                //{
                //    new OperativeSchedule {Id = 1, ListOfStops = new ObservableCollection<Station>(stations.Take(2).ToList())}
                //};
                //railwayStation= new RailwayStation {Id = 1, Name = "c1", Stations = stations, OperativeSchedules = operSh};
                //--------------------------------------------------------------------------------------------------

                var editViewModel = new RailwayStationEditViewModel(_unitOfWork, railwayStation);
                var result = _windowManager.ShowDialog(editViewModel);

                if(result != null && result.Value)
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





            //var dialogViewModel= new DialogViewModel();
            //var result= _windowManager.ShowDialog(dialogViewModel);

            //if (result != null && result.Value)
            //{
            //    MessageBox.Show("Ok");
            //}
            //else
            //{
            //    MessageBox.Show("Cancel");
            //}







            //var stations = _unitOfWork.StationRepository.Get().OrderBy(x => x.Id).ToList();


            //заполнение таблицы оперативного расписания
            //_unitOfWork.OperativeScheduleRepository.Insert(new OperativeSchedule {ArrivalTime = DateTime.Today, DepartureTime = DateTime.Now, RouteName = "Маршрут 2", NumberOfTrain = 2, DispatchStation = stations[2], StationOfDestination = stations[7], ListOfStops = stations.Skip(4).Take(2).ToList(), ListWithoutStops = stations.Skip(3).Take(6).ToList() });
            //await _unitOfWork.SaveAsync();



            //Добавление станции
            //_unitOfWork.RailwayStationRepository.Insert(new RailwayStation { Name = "Вокзал 3", Stations = new List<Station>(stations.Skip(0)) });
            //await _unitOfWork.SaveAsync();


            // var operativeSh= _unitOfWork.RailwayStationRepository.Search(r => r.Name == "Вокзал 2").Include(r => r.AllStations).First();
            // var operative = _unitOfWork.RailwayStationRepository.Get().Include(r => r.AllStations).ToList();



            //var editStation = stations[0];
            //editStation.Name = "New NAME!!";
            //_unitOfWork.StationRepository.Update(editStation);
            //await _unitOfWork.SaveAsync();




            //Обновление
            //var rs = _unitOfWork.RailwayStationRepository.GetById(4);  //_unitOfWork.RailwayStationRepository.Get().OrderBy(x => x.Id).ToList().First();
            //rs.OperativeSchedules.Clear();
            //rs.OperativeSchedules = _unitOfWork.OperativeScheduleRepository.Get().ToList();
            //_unitOfWork.RailwayStationRepository.Update(rs);
            //await _unitOfWork.SaveAsync();


            //удаление
            //var rs = _unitOfWork.RailwayStationRepository.GetById(1);  //_unitOfWork.RailwayStationRepository.Get().OrderBy(x => x.Id).ToList().First();
            //_unitOfWork.RailwayStationRepository.Remove(rs);
            //await _unitOfWork.SaveAsync();
        }

        public void LoadXmlDataInDb(string nameRailwayStations)
        {
            //1. Таблица "Станции" должна быть заполнена всеми возможными станциями 8 вокзалов (все станции Росии (примерно 11617))
            //2. Таблица "Вокзалы" должна быть заполнена 8 элементами.

            //Загрузка оперативного расписания (с полным уничтожением предыдущего).
            //3. Из XML файла грузится список настроек (список объектов OperativeScheduleProxyXml).
            //4. Если список создан без ошибок и в нем есть элемпенты, то имеем право очистить таблицу OperativeSchedules и список ссылок на элементы расписания в RailwayStation.
            //5. Перебираем элементы сохданного прокси списка.
            //6. Для каждого объекта из списка ищутся в таблице "Станции" нужные станции (станция отправления, станция назначения, список пропушенных, список остановочных).
            //7. Каждый объект инициализирует новый OperativeSchedule и заполняет свои станции, найденными.
            //8. Созданные объект OperativeSchedule помещается в список объекта RailwayStation.


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
            _serviceHost?.Close();
            base.OnDeactivate(close);
        }
    }
}