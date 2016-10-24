using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Domain.Abstract;
using Domain.DbContext;
using Domain.Entities;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorService;

namespace Server.ViewModels
{
    public class AppViewModel : Conductor<object>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServerContract _autoDictorServise;



        public IDisposable DispouseOrderStatusProcess { get; set; }
        public IDisposable DispouseOrderStatusAcsess { get; set; }



        public AppViewModel(IEventAggregator events, IWindowManager windowManager, IUnitOfWork unitOfWork, IServerContract autoDictorServise)
        {
            _windowManager = windowManager;
            _unitOfWork = unitOfWork;
            _eventAggregator = events;
            //events.Subscribe(this);
            _autoDictorServise = autoDictorServise;
        }






        public async void RailwayStation1()
        {
            const string railwayStationName = "Вокзал 3";
            var railwayStation = _unitOfWork.RailwayStationRepository.Search(r => r.Name == railwayStationName).Include(r=> r.Stations).Include(op=> op.OperativeSchedules).First();
            if (railwayStation != null)
            {
                var editViewModel = new RailwayStationEditViewModel(_unitOfWork, railwayStation);
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







            var stations = _unitOfWork.StationRepository.Get().OrderBy(x => x.Id).ToList();


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






        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }
    }
}