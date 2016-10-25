using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Domain.Abstract;
using Domain.DbContext;
using Domain.Entities;
using System.ServiceModel;
using Castle.Facilities.WcfIntegration;
using Server.HostWCF;
using WCFCis2AvtodictorContract.Contract;



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
            var railwayStation = await _unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName).Include(r => r.Stations).Include(op => op.OperativeSchedules).FirstOrDefaultAsync();
            if(railwayStation != null)
            {
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